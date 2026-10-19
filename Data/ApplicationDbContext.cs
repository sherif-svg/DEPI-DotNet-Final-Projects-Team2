using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Mini_Project_DEPI.Models;

namespace Mini_Project_DEPI.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Fluent API Configurations for Identity Tables
            builder.Entity<ApplicationUser>(entity =>
            {
                entity.Property(u => u.FullName)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(u => u.BloodType)
                      .HasMaxLength(5);

                entity.Property(u => u.Gender)
                      .HasMaxLength(10);
            });
        }
    }
}