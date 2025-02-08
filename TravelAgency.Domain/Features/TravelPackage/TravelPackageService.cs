using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TravelAgency.Database.AppDbContextModels;
using TravelAgency.Domain.Features.TravelPackage;

namespace TravelAgency.Domain.Features.TravelPackage
{
    public class TravelPackageService
    {
        private readonly AppDbContext _db;

        public TravelPackageService(AppDbContext db)
        {
            _db = db;
        }

        // Get All Travel Packages
        public async Task<List<TravelPackageRequestModel>> Execute()
        {
            return await _db.TravelPackages
                .Select(tp => new TravelPackageRequestModel
                {
                    Id = tp.Id,
                    Title = tp.Title,
                    Destination = tp.Destination,
                    Description = tp.Description,
                    Price = tp.Price,
                    Duration = tp.Duration,
                    Inclusions = tp.Inclusions,
                    CancellationPolicy = tp.CancellationPolicy,
                    Status = tp.Status
                })
                .ToListAsync();
        }
    }
}
