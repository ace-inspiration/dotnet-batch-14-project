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

        public async Task<UserListResponseModel> GetUserList()
        {
            UserListResponseModel responseModel = new();
            try
            {
                var userList = await _db.Users.AsNoTracking().ToListAsync();
                responseModel.Success = true;
                responseModel.Message = "Users fetched successfully.";
                responseModel.Data = userList;
            }
            catch (Exception ex)
            {
                responseModel.Success = false;
                responseModel.Message = ex.Message;
                responseModel.Data = null;
            }
            return responseModel;
        }
    }
}
