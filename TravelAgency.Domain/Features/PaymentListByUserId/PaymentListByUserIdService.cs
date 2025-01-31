using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Database.AppDbContextModels;
using TravelAgency.Domain.Features.BookingListByUserId;

namespace TravelAgency.Domain.Features.PaymentListByUserId
{
    public class PaymentListByUserIdService
    {
        private readonly AppDbContext _db;
        public PaymentListByUserIdService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<PaymentListByUserIdResponseModel> Execute(string userId)
        {
            PaymentListByUserIdResponseModel responseModel = new();
            try
            {
                var payments = await _db.Payments.Where(x => x.UserId == userId).ToListAsync();
                responseModel.Success = true;
                responseModel.Message = "Operation successful.";
                responseModel.Data = payments;
                return responseModel;
            }
            catch (Exception ex)
            {
                responseModel.Success = false;
                responseModel.Message = ex.ToString();
                return responseModel;
            }
        }
    }
}
