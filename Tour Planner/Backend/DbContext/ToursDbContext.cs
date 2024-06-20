using Microsoft.EntityFrameworkCore;
using Shared.Models;

namespace Backend.DbContext;

public class ToursDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public DbSet<Tour> Tours { get; set; }
    public DbSet<TourLog> TourLogs { get; set; }
    
    public ToursDbContext(DbContextOptions<ToursDbContext> options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Tour>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired();
            entity.Property(e => e.Distance).IsRequired();
            entity.Property(e => e.Duration).IsRequired();
            entity.Property(e => e.TourDescription).IsRequired();
            entity.Property(e => e.TransportationType).IsRequired();
            entity.Property(e => e.From).IsRequired();
            entity.Property(e => e.To).IsRequired();
            entity.Property(e => e.Image).IsRequired();
        });

        modelBuilder.Entity<TourLog>(entity =>
        {
            entity.HasKey(e => e.IdTourLog);
            entity.Property(e => e.Comment).IsRequired();
            // Define foreign key relationship
            entity.HasOne(d => d.Tour)
                .WithMany()
                .HasForeignKey(d => d.TourId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}