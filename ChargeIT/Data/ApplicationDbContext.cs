using ChargeIT.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ChargeIT.Data;

public class ApplicationDbContext : IdentityDbContext {
    public DbSet<ChargeMachineEntity> ChargeMachines { get; set; }
    public DbSet<CarEntity> Cars { get; set; }
    public DbSet<CarOwnerEntity> CarOwners { get; set; }
    public DbSet<BookingEntity> Bookings { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    override protected void OnModelCreating(ModelBuilder builder) {
        base.OnModelCreating(builder);

        builder.Entity<ChargeMachineEntity>()
            .Property(e => e.Created).HasDefaultValueSql("getdate()");
        builder.Entity<ChargeMachineEntity>()
            .Property(e => e.IsDeleted).HasDefaultValue(false);
        
        builder.Entity<CarEntity>()
            .Property(e => e.Created).HasDefaultValueSql("getdate()");
        builder.Entity<CarEntity>()
            .Property(e => e.IsDeleted).HasDefaultValue(false);
        
        builder.Entity<CarOwnerEntity>()
            .Property(e => e.Created).HasDefaultValueSql("getdate()");
        builder.Entity<CarOwnerEntity>()
            .Property(e => e.IsDeleted).HasDefaultValue(false);
        
        builder.Entity<BookingEntity>()
            .Property(e => e.Created).HasDefaultValueSql("getdate()");
        builder.Entity<BookingEntity>()
            .Property(e => e.IsDeleted).HasDefaultValue(false);
    }
}