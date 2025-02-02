using Application.Abstraction;
using Application.DTOs;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class AccountService : IAccountService
    {
        private readonly AppDbContext _context;
        private readonly Random _random = new();

        public AccountService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<LoginResponse> LoginStep1Async(LoginStep1Request req)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.ICNumber == req.ICNumber);

            if (user == null) return new LoginResponse { Success = false, Message = "No user found with this IC" };

            var code = _random.Next(1000, 9999).ToString();
            _context.VerificationCodes.Add(new VerificationCode
            {
                UserId = user.Id,
                Code = code,
                Type = "PHONE"
            });
            await _context.SaveChangesAsync();

            return new LoginResponse
            {
                Success = true,
                Message = "Verification code sent to phone",
                VerificationCodeDemo = code // For demonstration
            };
        }

        public async Task<BasicResponse> LoginStep2VerifyPhoneCodeAsync(LoginStep2VerifyPhoneRequest req)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.ICNumber == req.ICNumber);
            if (user == null) return new BasicResponse(false, "User not found");

            var vcode = await _context.VerificationCodes
                .Where(x => x.UserId == user.Id && x.Type == "PHONE" && !x.IsUsed)
                .OrderByDescending(x => x.CreatedAt)
                .FirstOrDefaultAsync();
            if (vcode == null) return new BasicResponse(false, "No phone code found, please login again");
            if (vcode.Code != req.PhoneCode) return new BasicResponse(false, "Invalid phone code");

            vcode.IsUsed = true;
            vcode.UsedAt = DateTime.UtcNow;
            user.IsPhoneVerified = true;
            await _context.SaveChangesAsync();

            var code = _random.Next(1000, 9999).ToString();
            _context.VerificationCodes.Add(new VerificationCode
            {
                UserId = user.Id,
                Code = code,
                Type = "EMAIL"
            });
            await _context.SaveChangesAsync();

            return new BasicResponse(true, "Phone verified, email code sent: " + code);
        }

        public async Task<BasicResponse> LoginStep3VerifyEmailCodeAsync(LoginStep3VerifyEmailRequest req)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.ICNumber == req.ICNumber);
            if (user == null) return new BasicResponse(false, "User not found");

            var vcode = await _context.VerificationCodes
                .Where(x => x.UserId == user.Id && x.Type == "EMAIL" && !x.IsUsed)
                .OrderByDescending(x => x.CreatedAt)
                .FirstOrDefaultAsync();
            if (vcode == null) return new BasicResponse(false, "No email code found");
            if (vcode.Code != req.EmailCode) return new BasicResponse(false, "Invalid email code");

            vcode.IsUsed = true;
            vcode.UsedAt = DateTime.UtcNow;
            user.IsEmailVerified = true;
            await _context.SaveChangesAsync();

            return new BasicResponse(true, "Email verified");
        }

        public async Task<BasicResponse> LoginStep4AcceptTermsAsync(LoginStep4TermsRequest req)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.ICNumber == req.ICNumber);
            if (user == null) return new BasicResponse(false, "User not found");

            user.TermsAccepted = req.Accepted;
            await _context.SaveChangesAsync();
            return new BasicResponse(true, "Terms accepted");
        }

        public async Task<BasicResponse> LoginStep5CreatePinAsync(LoginStep5CreatePinRequest req)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.ICNumber == req.ICNumber);
            if (user == null) return new BasicResponse(false, "User not found");

            user.Pin = req.Pin;
            await _context.SaveChangesAsync();
            return new BasicResponse(true, "PIN created (pending confirmation)");
        }

        public async Task<BasicResponse> LoginStep6ConfirmPinAsync(LoginStep6ConfirmPinRequest req)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.ICNumber == req.ICNumber);
            if (user == null) return new BasicResponse(false, "User not found");

            if (user.Pin != req.Pin) return new BasicResponse(false, "PIN mismatch");

            return new BasicResponse(true, "PIN confirmed, you can proceed");
        }

        public async Task<BasicResponse> LoginStep7EnableBiometricAsync(LoginStep7EnableBiometricRequest req)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.ICNumber == req.ICNumber);
            if (user == null) return new BasicResponse(false, "User not found");

            user.BiometricEnabled = req.Enable;
            await _context.SaveChangesAsync();
            return new BasicResponse(true, req.Enable ? "Biometric enabled" : "Biometric disabled");
        }


        public async Task<RegisterResponse> RegisterStep1Async(RegisterStep1Request req)
        {
            var existing = await _context.Users
                .FirstOrDefaultAsync(x => x.ICNumber == req.ICNumber
                                       || x.PhoneNumber == req.PhoneNumber
                                       || x.Email == req.Email);

            if (existing != null)
            {
                return new RegisterResponse
                {
                    Success = false,
                    Message = "User already exists, proceeding to second step of login flow",
                    UserAlreadyExists = true
                };
            }

            var user = new User
            {
                ICNumber = req.ICNumber,
                Name = req.Name,
                PhoneNumber = req.PhoneNumber,
                Email = req.Email
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var codePhone = _random.Next(1000, 9999).ToString();
            _context.VerificationCodes.Add(new VerificationCode
            {
                UserId = user.Id,
                Code = codePhone,
                Type = "PHONE"
            });
            await _context.SaveChangesAsync();

            return new RegisterResponse
            {
                Success = true,
                Message = "Phone code sent",
                VerificationCodePhoneDemo = codePhone
            };
        }

        public async Task<BasicResponse> RegisterStep2PhoneCodeAsync(RegisterStep2PhoneRequest req)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.ICNumber == req.ICNumber);
            if (user == null) return new BasicResponse(false, "User not found");

            var vcode = await _context.VerificationCodes
                .Where(x => x.UserId == user.Id && x.Type == "PHONE" && !x.IsUsed)
                .OrderByDescending(x => x.CreatedAt)
                .FirstOrDefaultAsync();
            if (vcode == null) return new BasicResponse(false, "No phone code found");
            if (vcode.Code != req.PhoneCode) return new BasicResponse(false, "Invalid phone code");

            vcode.IsUsed = true;
            vcode.UsedAt = DateTime.UtcNow;
            user.IsPhoneVerified = true;
            await _context.SaveChangesAsync();

            var codeEmail = _random.Next(1000, 9999).ToString();
            _context.VerificationCodes.Add(new VerificationCode
            {
                UserId = user.Id,
                Code = codeEmail,
                Type = "EMAIL"
            });
            await _context.SaveChangesAsync();

            return new BasicResponse(true, "Phone verified, email code sent: " + codeEmail);
        }

        public async Task<BasicResponse> RegisterStep3EmailCodeAsync(RegisterStep3EmailRequest req)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.ICNumber == req.ICNumber);
            if (user == null) return new BasicResponse(false, "User not found");

            var vcode = await _context.VerificationCodes
                .Where(x => x.UserId == user.Id && x.Type == "EMAIL" && !x.IsUsed)
                .OrderByDescending(x => x.CreatedAt)
                .FirstOrDefaultAsync();
            if (vcode == null) return new BasicResponse(false, "No email code found");
            if (vcode.Code != req.EmailCode) return new BasicResponse(false, "Invalid email code");

            vcode.IsUsed = true;
            vcode.UsedAt = DateTime.UtcNow;
            user.IsEmailVerified = true;
            await _context.SaveChangesAsync();

            return new BasicResponse(true, "Email verified");
        }

        public async Task<BasicResponse> RegisterStep4AcceptTermsAsync(RegisterStep4TermsRequest req)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.ICNumber == req.ICNumber);
            if (user == null) return new BasicResponse(false, "User not found");

            user.TermsAccepted = req.Accepted;
            await _context.SaveChangesAsync();
            return new BasicResponse(true, "Terms accepted");
        }

        public async Task<BasicResponse> RegisterStep5CreatePinAsync(RegisterStep5CreatePinRequest req)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.ICNumber == req.ICNumber);
            if (user == null) return new BasicResponse(false, "User not found");

            user.Pin = req.Pin;
            await _context.SaveChangesAsync();
            return new BasicResponse(true, "PIN created (pending confirmation)");
        }

        public async Task<BasicResponse> RegisterStep6ConfirmPinAsync(RegisterStep6ConfirmPinRequest req)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.ICNumber == req.ICNumber);
            if (user == null) return new BasicResponse(false, "User not found");

            if (user.Pin != req.Pin) return new BasicResponse(false, "PIN mismatch");
            return new BasicResponse(true, "PIN confirmed");
        }

        public async Task<BasicResponse> RegisterStep7EnableBiometricAsync(RegisterStep7EnableBiometricRequest req)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.ICNumber == req.ICNumber);
            if (user == null) return new BasicResponse(false, "User not found");

            user.BiometricEnabled = req.Enable;
            await _context.SaveChangesAsync();
            return new BasicResponse(true, "Biometric updated");
        }

    }
}