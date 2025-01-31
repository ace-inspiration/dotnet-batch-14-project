using Microsoft.EntityFrameworkCore;
using TravelAgency.Database.AppDbContextModels;

namespace TravelAgency.Domain.Features.ActivateTravelPackage;

public class ActivateTravelPackageService : IActivateTravelPackageService
{
    private readonly IAppDbContext _appDbContext;

    public ActivateTravelPackageService(IAppDbContext b)
    {
        //_appDbContext = db;
        _appDbContext = db;
    }


    public async Task<ActivateTravelPackageResponseModel> ActivateTravelPackage(ActivateTravelPackageRequestModel request)
    {
        var travelPackage = await _appDbContext.TravelPackages.FirstOrDefaultAsync(x => x.Id == request.TravelPackageId);

        if (travelPackage == null)
        {
            return new ActivateTravelPackageResponseModel { Success = false, Message = "Travel Package not found" };
        }
        if(travelPackage.status == "Activate")
        {
            return new ActivateTravelPackageResponseModel { Success = false, Message = "Travel Package is already activate" };
        }
        travelPackage.status = "Activate";

        travelPackage.IsActive = true;

        await _appDbContext.SaveChangesAsync();

        return new ActivateTravelPackageResponseModel { Success = true, Message = "Travel Package activated" };
    }
}