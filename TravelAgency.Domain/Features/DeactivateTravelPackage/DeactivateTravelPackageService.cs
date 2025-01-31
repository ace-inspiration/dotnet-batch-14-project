using Microsoft.EntityFrameworkCore;
using TravelAgency.Database.AppDbContextModels;

namespace TravelAgency.Domain.Features.DeactivateTravelPackage;
public class DeactivateTravelPackageService : IDeactivateTravelPackageService
{
    private readonly IAppDbContext _appDbContext;

    public DeactivateTravelPackageService(IAppDbContext db)
    {
        //_appDbContext = db;
        _appDbContext = db;
    }
    public async Task<DeactivateTravelPackageResponseModel> DeactivateTravelPackage(DeactivateTravelPackageRequestModel request)
    {
        var travelPackage = await _appDbContext.TravelPackages.FirstOrDefaultAsync(x => x.Id == request.TravelPackageId);

        if (travelPackage == null)
        {
            return new DeactivateTravelPackageResponseModel { Success = false, Message = "Travel Package not found" };
        }
        if (travelPackage.status == "Deactivate")
        {
            return new DeactivateTravelPackageResponseModel { Success = false, Message = "Travel Package is already Deactivate" };
        }
        travelPackage.status = "Deactivate";

        travelPackage.IsActive = false;

        await _appDbContext.SaveChangesAsync();

        return new DeactivateTravelPackageResponseModel { Success = true, Message = "Travel Package deactivated" };
    }
}