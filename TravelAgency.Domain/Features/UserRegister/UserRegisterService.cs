using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Database.AppDbContextModels;

namespace TravelAgency.Domain.Features.UserRegister
{
    public class UserRegisterService
    {
        private readonly AppDbContext _db;
        public UserRegisterService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<UserRegisterResponseModel> UserRegister(UserRegisterRequestModel requestModel)
        {
            UserRegisterResponseModel model = new UserRegisterResponseModel();

            if (requestModel.Role == "Admin")
            {

                model.Message = "Admin registration is not allowed";
                return model;
                
            }
            var existingUser = await _db.Users
        .Where(u => u.Email == requestModel.Email)
        .FirstOrDefaultAsync();

            if (existingUser != null)
            {
                model.IsSuccess = false;
                model.Message = "User already exists. Register Failed";
                model.Data = existingUser;
                return model;
            }

            string hashPassword = HashPassword(requestModel.Password);

            var user = new User()
            {
                Id = Guid.NewGuid().ToString(),
                Name = requestModel.Name,
                Email = requestModel.Email,
                PasswordHash = hashPassword,
                Phone = requestModel.Phone,
                Role = requestModel.Role
            };

            await _db.Users.AddAsync(user);
            int result = await _db.SaveChangesAsync();
            string message = result > 0 ? "User Register Successful" : "User Register Failed";

            model.IsSuccess = result > 0;
            model.Message = message;
            model.Data = user;

            return model;
        }

        private static string HashPassword(string password)
        {
            using SHA256 sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            string hashedPassword = Convert.ToBase64String(hashedBytes);
            return hashedPassword;
        }

    }
}
