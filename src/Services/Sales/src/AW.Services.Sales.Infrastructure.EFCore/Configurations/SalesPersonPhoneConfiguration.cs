﻿using AW.Services.Sales.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AW.Services.Sales.Infrastructure.EFCore.Configurations
{
    public class SalesPersonPhoneConfiguration : IEntityTypeConfiguration<SalesPersonPhone>
    {
        public void Configure(EntityTypeBuilder<SalesPersonPhone> builder)
        {
            builder.ToTable("SalesPersonPhone");
            builder.HasKey(p => p.Id);

            builder.Property(c => c.Id)
                .HasColumnName("SalesPersonPhoneID");
        }
    }
}