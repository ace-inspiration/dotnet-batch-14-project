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
            OTP_Expiry = DateTime.UtcNow.AddMinutes(5)
        };

        await _db.Users.AddAsync(user);
        int result = await _db.SaveChangesAsync();

        if (result > 0)
        {
            bool emailSent = SendOTPEmail(user.Email, otpCode);
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

    private static bool SendOTPEmail(string toEmail, string otpCode)
    {
        try
        {
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("nnyi37389@gmail.com");
            mail.To.Add(toEmail);
            mail.Subject = "Your OTP Code From (TravelAgency)";
            mail.Body = $"Your OTP code is: {otpCode}. It will expire in 5 minutes.";
            mail.IsBodyHtml = false;

            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("nnyi37389@gmail.com", "jbrq aqmv ukix sfdv"),
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
}
