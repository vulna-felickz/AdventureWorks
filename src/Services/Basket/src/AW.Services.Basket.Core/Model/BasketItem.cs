﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AW.Services.Basket.Core.Model
{
    public class BasketItem : IValidatableObject
    {
        public string Id { get; set; }
        public string ProductNumber { get; set; }
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal OldUnitPrice { get; set; }
        public int Quantity { get; set; }
        public byte[] ThumbnailPhoto { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();

            if (Quantity < 1)
            {
                results.Add(new ValidationResult("Invalid number of units", new[] { "Quantity" }));
            }

            return results;
        }
    }
}