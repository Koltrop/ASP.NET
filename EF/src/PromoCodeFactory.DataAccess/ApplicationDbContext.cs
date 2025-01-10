using Microsoft.EntityFrameworkCore;
using PromoCodeFactory.Core.Domain.Administration;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using System;
using static PromoCodeFactory.DataAccess.DbConstraints;

namespace PromoCodeFactory.DataAccess;

public sealed class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Employee> Employees { get; set; }

    public DbSet<Role> Roles { get; set; }

    public DbSet<Customer> Customers { get; set; }

    public DbSet<Preference> Preferences { get; set; }

    public DbSet<PromoCode> Promocodes { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Employee>(entity =>
        {
            entity.HasOne(x => x.Role)
                  .WithMany()
                  .HasForeignKey(x => x.RoleId)
                  .OnDelete(DeleteBehavior.NoAction);

            entity.Property(x => x.FirstName)
                  .HasMaxLength(MaxLength.Name);

            entity.Property(x => x.LastName)
                  .HasMaxLength(MaxLength.Name);

            entity.Property(x => x.Email)
                  .HasMaxLength(MaxLength.Email);
        });

        builder.Entity<Role>(entity =>
        {
            entity.Property(x => x.Name)
                  .HasMaxLength(MaxLength.Name);

            entity.Property(x => x.Description)
                  .HasMaxLength(MaxLength.Text);
        });

        builder.Entity<Customer>(entity =>
        {
            entity.HasMany(x => x.Preferences)
                  .WithMany(x => x.Customers)
                  .UsingEntity<CustomerPreference>();

            entity.HasMany(x => x.Promocodes)
                  .WithOne(x => x.Customer);

            entity.Property(x => x.FirstName)
                  .HasMaxLength(MaxLength.Name);

            entity.Property(x => x.LastName)
                  .HasMaxLength(MaxLength.Name);

            entity.Property(x => x.Email)
                  .HasMaxLength(MaxLength.Email);
        });

        builder.Entity<Preference>(entity =>
        {
            entity.HasMany(x => x.Customers)
                  .WithMany(x => x.Preferences)
                  .UsingEntity<CustomerPreference>();

            entity.Property(x => x.Name)
                  .HasMaxLength(MaxLength.Name);
        });

        builder.Entity<PromoCode>(entity =>
        {
            entity.Property(x => x.Code)
                  .HasMaxLength(MaxLength.Code);

            entity.Property(x => x.ServiceInfo)
                  .HasMaxLength(MaxLength.Text);
        });
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        optionsBuilder.LogTo(Console.WriteLine);
    }
}
