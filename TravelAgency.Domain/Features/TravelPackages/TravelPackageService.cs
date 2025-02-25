using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TravelAgency.Database.AppDbContextModels;

namespace TravelAgency.Domain.Features.TravelPackages
{
    public class TravelPackageService
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public TravelPackageService(AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _db = context;
            _webHostEnvironment = webHostEnvironment ?? throw new ArgumentNullException(nameof(webHostEnvironment));
        }

        // Get All Travel Packages
        public async Task<List<TravelPackage>> Execute()
        {
            var travelPackages = await _db.TravelPackages.ToListAsync();
            return travelPackages;
        }

        public async Task<TravelPackage> GetTravelPackagById(string id)
        {
            return (await _db.TravelPackages.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id))!;
        }
        public async Task<TravelPackageResponseModel> UpdateTravelPackage(TravelPackageRequestModel requestModel, IFormFile? photo)
        {
            var item = await _db.TravelPackages.FirstOrDefaultAsync(x => x.Id == requestModel.Id);
            if (item == null)
            {
                return new TravelPackageResponseModel { Success = false, Message = "Travel Package not found", Data = null };
            }

            // Update fields if provided
            if (!string.IsNullOrEmpty(requestModel.Title))
                item.Title = requestModel.Title;
            if (!string.IsNullOrEmpty(requestModel.Destination))
                item.Destination = requestModel.Destination;
            if (!string.IsNullOrEmpty(requestModel.Inclusions))
                item.Inclusions = requestModel.Inclusions;
            if (!string.IsNullOrEmpty(requestModel.CancellationPolicy))
                item.CancellationPolicy = requestModel.CancellationPolicy;
            if (!string.IsNullOrEmpty(requestModel.Description))
                item.Description = requestModel.Description;
            if (requestModel.Price > 0)
                item.Price = requestModel.Price;
            if (requestModel.Duration > 0)
                item.Duration = requestModel.Duration;
            if (!string.IsNullOrEmpty(requestModel.Status))
                item.Status = requestModel.Status;

            // Handle image update if a new photo is provided
            if (photo != null)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"), "images");
                Directory.CreateDirectory(uploadsFolder);

                string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(photo.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await photo.CopyToAsync(fileStream);
                }

                // Delete old image (optional, if needed)
                if (!string.IsNullOrEmpty(item.Image))
                {
                    string oldImagePath = Path.Combine(uploadsFolder, Path.GetFileName(item.Image));
                    if (File.Exists(oldImagePath))
                    {
                        File.Delete(oldImagePath);
                    }
                }

                item.Image = "/images/" + uniqueFileName;
            }
            _db.Entry(item).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return new TravelPackageResponseModel { Success = true, Message = "Travel Package updated successfully", Data = item };
        }

        public async Task<TravelPackageResponseModel> CreateTravelPackage(TravelPackageRequestModel model, IFormFile? photo)
        {
            string? photoPath = null;

            if (_webHostEnvironment.WebRootPath == null)
            {
                _webHostEnvironment.WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            }

            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
            Directory.CreateDirectory(uploadsFolder);
            string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(photo.FileName);
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await photo.CopyToAsync(fileStream);
            }

            photoPath = "/images/" + uniqueFileName;

            var trvelpackage = new TravelPackage
            {
                Id = Guid.NewGuid().ToString(),
                Title = model.Title,
                Inclusions = model.Inclusions,
                CancellationPolicy = model.CancellationPolicy,
                Description = model.Description,
                Price = model.Price,
                Destination = model.Destination,
                Status = "Activate",
                Image = photoPath
            };

            _db.TravelPackages.Add(trvelpackage);
            var result = await _db.SaveChangesAsync();
            return result == 1
                ? new TravelPackageResponseModel { Success = true, Message = "Travel package created successfully", Data = trvelpackage }
                : new TravelPackageResponseModel { Success = false, Message = "Travel package creation failed", Data = null };
        }
    public async Task<List<TravelPackage>> GetPopularTravelPackage()
    {
        var lst = await _db.TravelPackages
                    .OrderByDescending(tp => tp.Count)
                    .Take(5)
                    .AsNoTracking()
                    .ToListAsync();
            return lst;
        }
    }

}
