using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Projet_MaisonCo_backend.Entities.Configs
{
    public class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder .HasKey(u => u.Id);
            builder.Property(u => u.Id).ValueGeneratedOnAdd();

            builder.HasIndex(u => u.Username).IsUnique();
            builder.Property(u => u.Username).IsRequired();

            builder.Property(u => u.Password).IsRequired();

            builder.Property(u => u.Role).IsRequired();
            builder.Property(u => u.Role).IsRequired().HasConversion<string>();
        }
    }
}
