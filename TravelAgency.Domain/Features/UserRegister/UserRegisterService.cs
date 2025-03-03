using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Database.AppDbContextModels;

namespace TravelAgency.Domain.Features.UserRegister;

public class UserRegisterService
{
    private readonly AppDbContext _db;

    public UserRegisterService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<UserRegisterResponseModel> Execute(UserRegisterRequestModel requestModel)
    {
        UserRegisterResponseModel model = new UserRegisterResponseModel();

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
        string otpCode = GenerateOTP();

        var user = new User()
        {
            Id = Guid.NewGuid().ToString(),
            Name = requestModel.Name,
            Email = requestModel.Email,
            PasswordHash = hashPassword,
            Phone = requestModel.Phone,
            Role = "User",
            Status = "N",
            OTP = otpCode,
            OTP_Expiry = DateTime.UtcNow.AddMinutes(2)
        };

      

        await _db.Users.AddAsync(user);
        int result = await _db.SaveChangesAsync();

        if (result > 0)
        {
            bool emailSent = SendOTPEmail(user.Email,  user.Name, otpCode);
            if (!emailSent)
            {
                model.IsSuccess = false;
                model.Message = "User registered but failed to send OTP email.";
                model.Data = user;
                return model;
            }

            model.IsSuccess = true;
            model.Message = "User Register Successful. OTP sent to email.";
            model.Data = user;
        }
        else
        {
            model.IsSuccess = false;
            model.Message = "User Register Failed";
            model.Data = user;
        }

        return model;
    }




    private static string HashPassword(string password)
    {
        using SHA256 sha256 = SHA256.Create();
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(hashedBytes);
    }

    private static string GenerateOTP()
    {
        Random random = new Random();
        return random.Next(100000, 999999).ToString();
    }

    private static bool SendOTPEmail(string toEmail, string userName, string otpCode)
    {
        try
        {
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("acetravelagency.net@gmail.com");
            mail.To.Add(toEmail);
            mail.Subject = "Your OTP Code from TravelAgency";

        
            string htmlBody = $@"
            <div style='font-family: Arial, sans-serif; padding: 20px; border: 1px solid #ddd; border-radius: 10px; max-width: 500px; margin: auto; background-color: #f9f9f9;'>
                <h2 style='color: #007bff; text-align: center;'>Your OTP Code</h2>
                <p style='font-size: 16px; color: #333;'>Dear <strong>{userName}</strong>,</p>
                <p style='font-size: 16px; color: #333;'>Your One-Time Password (OTP) for verification is:</p>
                <p style='font-size: 24px; font-weight: bold; color: #28a745; text-align: center; padding: 10px; border: 2px dashed #28a745; display: inline-block;'>{otpCode}</p>
                <p style='font-size: 14px; color: #ff0000; text-align: center;'>This OTP will expire in 5 minutes.</p>
                <p style='font-size: 16px; color: #333;'>If you did not request this code, please register again after 5 minutes.</p>
                <br>
                <p style='font-size: 14px; color: #666; text-align: center;'>Best regards,</p>
                <p style='font-size: 14px; color: #666; text-align: center;'><strong>TravelAgency Team</strong></p>
            </div>";

            mail.Body = htmlBody;
            mail.IsBodyHtml = true; 

            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("acetravelagency.net@gmail.com", "tkdm txbp kkaa lagm"),
                EnableSsl = true
            };

            smtpClient.Send(mail);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<UserRegisterResponseModel> VerifyEmail(string email, string otp)
    {
        UserRegisterResponseModel model = new UserRegisterResponseModel();
        var user = await _db.Users
     .Where(u => u.Email == email)
     .OrderByDescending(u => u.OTP_Expiry)  
     .FirstOrDefaultAsync();

        if (user == null || user.OTP != otp)
        {
            model.IsSuccess = false;
            model.Message = "Invalid OTP.";
            return model;
        }


        if (user.OTP_Expiry < DateTime.UtcNow)
        {
           
            _db.Users.Remove(user);
            await _db.SaveChangesAsync();

            model.IsSuccess = false;
            model.Message = "OTP expired.Please register again.";
            return model;
        }

        user.Status = "Y";
        user.OTP = null;
        user.OTP_Expiry = DateTime.UtcNow;
        _db.Users.Update(user);
        int result = await _db.SaveChangesAsync();
        if (result > 0)
        {
            model.IsSuccess = true;
            model.Message = "OTP Confirmed. User Activated.";
        }
        else
        {
            model.IsSuccess = false;
            model.Message = "OTP Confirmation Failed.";
        }
        return model;
    }
}
