using BloodDonationPlatform.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BloodDonationPlatform.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Hospital> Hospitals => Set<Hospital>();
    public DbSet<HospitalInventory> HospitalInventories => Set<HospitalInventory>();
    public DbSet<Donor> Donors => Set<Donor>();
    public DbSet<BloodRequest> BloodRequests => Set<BloodRequest>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

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

        builder.Entity<Hospital>(entity =>
        {
            entity.Property(h => h.Name).IsRequired().HasMaxLength(150);
            entity.Property(h => h.City).HasMaxLength(100);
            entity.HasOne(h => h.User)
                  .WithMany()
                  .HasForeignKey(h => h.UserId)
                  .OnDelete(DeleteBehavior.SetNull);
        });

        builder.Entity<HospitalInventory>(entity =>
        {
            entity.HasIndex(i => new { i.HospitalId, i.BloodType }).IsUnique();
            entity.Property(i => i.Units).HasDefaultValue(0);
            entity.HasOne(i => i.Hospital)
                  .WithMany(h => h.Inventory)
                  .HasForeignKey(i => i.HospitalId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<Donor>(entity =>
        {
            entity.Property(d => d.Name).IsRequired().HasMaxLength(120);
            entity.HasOne(d => d.User)
                  .WithMany()
                  .HasForeignKey(d => d.UserId)
                  .OnDelete(DeleteBehavior.SetNull);
        });

        builder.Entity<BloodRequest>(entity =>
        {
            entity.HasOne(r => r.Hospital)
                  .WithMany(h => h.BloodRequests)
                  .HasForeignKey(r => r.HospitalId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(r => r.MatchedDonor)
                  .WithMany()
                  .HasForeignKey(r => r.MatchedDonorId)
                  .OnDelete(DeleteBehavior.SetNull);
        });
    }
}
