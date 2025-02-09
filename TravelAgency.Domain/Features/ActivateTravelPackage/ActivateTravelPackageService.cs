using Microsoft.EntityFrameworkCore;
using TravelAgency.Database.AppDbContextModels;

namespace TravelAgency.Domain.Features.ActivateTravelPackage;

public class ActivateTravelPackageService
{
    private readonly AppDbContext _db;

    public ActivateTravelPackageService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<ActivateTravelPackageResponseModel> Execute(string id)
    {
        var travelPackage = await _db.TravelPackages.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

        if (travelPackage == null)
        {
            return new ActivateTravelPackageResponseModel { Success = false, Message = "Travel Package not found" };
        }
        if (travelPackage.Status == "Activate")
        {
            return new ActivateTravelPackageResponseModel { Success = false, Message = "Travel Package is already activate" };
        }
        travelPackage.Status = "Activate";
        _db.TravelPackages.Update(travelPackage);
        await _db.SaveChangesAsync();

        return new ActivateTravelPackageResponseModel { Success = true, Message = "Travel Package activated" };
    }
}