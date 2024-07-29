using Backend.DbContext;
using Microsoft.EntityFrameworkCore;
using Shared.Models;

namespace Backend.Repository;

public class TourRepository(ToursDbContext dbContext)
{
    public async Task AddTourAsync(Tour tour)
    {
        //Created
        await dbContext.AddAsync(tour);
        await dbContext.SaveChangesAsync();
    }

    public async Task AddTourLogAsync(TourLog tourLog, Tour tour)
    {
        //Update
        dbContext.Update(tour);
        //Created
        await dbContext.AddAsync(tourLog);
        await dbContext.SaveChangesAsync();
    }

    public async Task<Tour?> ByIdAsync(int id)
    {
        //Get Tour by ID
        return await dbContext.Tours.AsQueryable().FirstOrDefaultAsync(tour => tour.Id == id);
    }

    public async Task<Tour?> ByIdWithTourLogsAsync(int id)
    {
        return await dbContext.Tours.AsQueryable()
            .Include(tour => tour.TourLogs)
            .FirstOrDefaultAsync(tour => tour.Id == id);
    }

    public async Task RemoveTourAsync(int id)
    {
        var tour = await ByIdAsync(id);
        if (tour != null)
        {
            dbContext.Tours.Remove(tour);
            await dbContext.SaveChangesAsync();
        }
    }

    public async Task<List<Tour>> GetAllToursWithLogsAsync()
    {
        return await dbContext.Tours
            .Include(tour => tour.TourLogs)
            .ToListAsync();
    }
}