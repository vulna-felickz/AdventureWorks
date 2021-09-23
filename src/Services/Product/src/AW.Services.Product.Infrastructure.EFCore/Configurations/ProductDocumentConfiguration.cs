﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AW.Services.Product.Infrastructure.EFCore.Configurations
{
    public class ProductDocumentConfiguration : IEntityTypeConfiguration<Core.Entities.ProductDocument>
    {
        public void Configure(EntityTypeBuilder<Core.Entities.ProductDocument> builder)
        {
            builder.ToTable("ProductDocument");
            builder.HasKey(pd => new { pd.ProductID, pd.DocumentNode });

            builder.Property(pd => pd.ProductID)
                .HasColumnName("ProductID");
        }
    }
}