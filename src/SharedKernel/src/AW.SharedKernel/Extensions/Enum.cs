﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace AW.SharedKernel.Extensions
{
    public static class Enum<T>
    {
        public static IList<T> GetValues()
        {
            var enumValues = new List<T>();
            foreach (var fi in typeof(T).GetFields(BindingFlags.Static | BindingFlags.Public))
            {
                enumValues.Add((T)Enum.Parse(typeof(T), fi.Name, false));
            }
            return enumValues;
        }
        public static IList<T> GetValues(Enum value)
        {
            var enumValues = new List<T>();
            foreach (var fi in value.GetType().GetFields(BindingFlags.Static | BindingFlags.Public))
            {
                enumValues.Add((T)Enum.Parse(value.GetType(), fi.Name, false));
            }
            enumValues.Sort();
            return enumValues;
        }
        public static T Parse(string? value)
        {
            return (T)Enum.Parse(typeof(T), value!, true);
        }
        public static IList<string> GetNames(Enum value)
        {
            return value.GetType()
                .GetFields(BindingFlags.Static | BindingFlags.Public)
                .Select(fi => fi.Name)
                .OrderBy(_ => _)
                .ToList();
        }
        public static IList<string?> GetDisplayValues(Enum value)
        {
            return GetNames(value).Select(obj => GetDisplayValue(Parse(obj)))
                .OrderBy(x => x)
                .ToList();
        }
        private static string? LookupResource(Type? resourceManagerProvider, string resourceKey)
        {
            foreach (var staticProperty in resourceManagerProvider!.GetProperties(BindingFlags.Static | BindingFlags.Public))
            {
                if (staticProperty.PropertyType == typeof(System.Resources.ResourceManager))
                {
                    var resourceManager = (System.Resources.ResourceManager?)staticProperty.GetValue(null, null);
                    return resourceManager?.GetString(resourceKey);
                }
            }
            return resourceKey;
        }
        public static string? GetDisplayValue(T? value)
        {
            var fieldInfo = value?.GetType().GetField(value.ToString()!);
            var descriptionAttributes = fieldInfo?.GetCustomAttributes(
                typeof(DisplayAttribute), false) as DisplayAttribute[];
            if (descriptionAttributes?[0].ResourceType != null)
                return LookupResource(descriptionAttributes?[0].ResourceType, descriptionAttributes?[0].Name!);
            return (descriptionAttributes?.Length > 0) ? descriptionAttributes?[0].Name : value?.ToString();
        }
    }
}