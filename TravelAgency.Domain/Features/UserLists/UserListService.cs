using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
