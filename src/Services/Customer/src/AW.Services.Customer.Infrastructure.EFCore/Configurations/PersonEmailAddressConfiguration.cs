﻿using AW.Services.Customer.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AW.Services.Customer.Infrastructure.EFCore.Configurations
{
    public class PersonEmailAddressConfiguration : IEntityTypeConfiguration<PersonEmailAddress>
    {
        public void Configure(EntityTypeBuilder<PersonEmailAddress> builder)
        {
            builder.ToTable("PersonEmailAddress");
            builder.HasKey(_ => _.Id);

            builder.Property(_ => _.Id)
                .HasColumnName("PersonEmailAddressID");

            builder.OwnsOne(_ => _.EmailAddress)
                .Property(_ => _.Value)
                    .HasColumnName("EmailAddress");

            builder.Property(c => c.PersonId)
                .HasColumnName("PersonID");
        }
    }
}