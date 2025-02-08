using Microsoft.EntityFrameworkCore;
using TravelAgency.Database.AppDbContextModels;
using TravelAgency.Domain.Features.ActivateTravelPackage;

namespace TravelAgency.Domain.Features.DeactivateTravelPackage;
public class DeactivateTravelPackageService 
{
    private readonly AppDbContext _db;

    public DeactivateTravelPackageService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<DeactivateTravelPackageResponseModel> Execute(string id)
    {
        var travelPackage = await _db.TravelPackages.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

        if (travelPackage == null)
        {
            return new DeactivateTravelPackageResponseModel { Success = false, Message = "Travel Package not found" };
        }
        if (travelPackage.Status == "Deactivate")
        {
            return new DeactivateTravelPackageResponseModel { Success = false, Message = "Travel Package is already Deactivate" };
        }
        travelPackage.Status = "Deactivate";
        _db.TravelPackages.Update(travelPackage);
        await _db.SaveChangesAsync();

        return new DeactivateTravelPackageResponseModel { Success = true, Message = "Travel Package deactivated" };
    }
}