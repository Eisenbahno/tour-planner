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
        modelBuilder.Entity<Tour>(tour =>
        {
            tour.HasKey(t => t.Id);
            tour.Property(t => t.Name).IsRequired();
            tour.Property(t => t.Distance).IsRequired();
            tour.Property(t => t.Duration).IsRequired();
            tour.Property(t => t.TourDescription).IsRequired();
            tour.Property(t => t.TransportationType).IsRequired();
            tour.Property(t => t.From).IsRequired();
            tour.Property(t => t.To).IsRequired();
            tour.Property(t => t.Image).IsRequired();
            tour.HasMany(t => t.TourLogs);
        });

        modelBuilder.Entity<TourLog>(tourLog =>
        {
            tourLog.HasKey(tl => tl.IdTourLog);
            tourLog.Property(tl => tl.Comment).IsRequired();
            // Define foreign key relationship
            tourLog.HasOne(d => d.Tour)
                .WithMany()
                .HasForeignKey(d => d.TourId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}