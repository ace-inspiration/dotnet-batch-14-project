using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Database.AppDbContextModels;
using TravelAgency.Domain.Features.BookingListByUserId;

namespace TravelAgency.Domain.Features.UserLists
{
    public class UserListService
    {
        private readonly AppDbContext _db;
        public UserListService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<User>> Execute()
        {
            var userList = await _db.Users.AsNoTracking().ToListAsync();
            return userList;
        }

        public async Task<User> GetuserbyId(string id)
        {
            var user = await _db.Users.AsNoTracking().Where(x => x.Id == id).FirstOrDefaultAsync();
            return user!;
        }

        public async Task<User> UpdateUser(User user)
        {
            var userToUpdate = await _db.Users.Where(x => x.Id == user.Id).FirstOrDefaultAsync();
            if (userToUpdate == null)
            {
                return null!;
            }
            if (!string.IsNullOrEmpty(user.Name))
            {
                userToUpdate.Name = user.Name;
            }
            if (!string.IsNullOrEmpty(user.Email))
            {
                userToUpdate.Email = user.Email;
            }
            if (!string.IsNullOrEmpty(user.Phone))
            {
                userToUpdate.Phone = user.Phone;
            }
            if (!string.IsNullOrEmpty(user.Role))
            {
                userToUpdate.Role = user.Role;
            }
            if (!string.IsNullOrEmpty(user.PasswordHash))
            {
                string hashPassword = HashPassword(user.PasswordHash);
                userToUpdate.PasswordHash = hashPassword;
            }
            _db.Entry(userToUpdate).State = EntityState.Modified;
            var result = await _db.SaveChangesAsync();
            return result > 0 ? userToUpdate : null!;
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
