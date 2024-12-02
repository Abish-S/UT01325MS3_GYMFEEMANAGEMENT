using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UT01325MS3_GYMFEEMANAGEMENT.DTOs.Requests;
using UT01325MS3_GYMFEEMANAGEMENT.Models;
using UT01325MS3_GYMFEEMANAGEMENT.Repositories.Interfaces;

namespace UT01325MS3_GYMFEEMANAGEMENT.Services
{
    public class AuthService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IConfiguration _configuration;

        public AuthService(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        /// <summary>
        /// Validates user credentials. Replace with your database check.
        /// </summary>
        public bool ValidateUser(string username, string password)
        {
            // Replace with actual user validation logic
            return username == "admin" && password == "password"; // Example only
        }

        /// <summary>
        /// Generates a JWT token for a valid user.
        /// </summary>
        public string GenerateJwtToken(Member member)
        {
            var jwtKey = _configuration["Jwt:Key"];
            var jwtIssuer = _configuration["Jwt:Issuer"];

            if (string.IsNullOrWhiteSpace(jwtKey) || string.IsNullOrWhiteSpace(jwtIssuer))
            {
                throw new InvalidOperationException("JWT Key or Issuer is not configured properly.");
            }

            var claims = new[]
            {
        new Claim(JwtRegisteredClaimNames.Sub, member.NIC), // NIC as username
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtIssuer,
                audience: jwtIssuer,
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        public async Task<(string token, bool isAdmin)> AuthenticateMemberAsync(LoginRequestDto loginDto)
        {
            try
            {
                // Check for null or empty input
                if (string.IsNullOrWhiteSpace(loginDto.Username) || string.IsNullOrWhiteSpace(loginDto.Password))
                {
                    throw new ArgumentException("NIC and Password must be provided.");
                }

                // Fetch member by NIC
                var members = await _unitOfWork.Members.GetAllMemberAsync(m => m.NIC == loginDto.Username);
                var memberEntity = members.FirstOrDefault();

                if (memberEntity == null)
                {
                    throw new KeyNotFoundException($"Member with NIC '{loginDto.Username}' not found.");
                }

                // Validate password
                var passwordHasher = new PasswordHasher<Member>();
                var passwordValidationResult = passwordHasher.VerifyHashedPassword(memberEntity, memberEntity.PasswordHash, loginDto.Password);

                if (passwordValidationResult != PasswordVerificationResult.Success)
                {
                    throw new UnauthorizedAccessException("Invalid password provided.");
                }

                // Generate JWT token
                var token = GenerateJwtToken(memberEntity);

                // Return the token and admin status
                return (token, memberEntity.IsAdmin);
            }
            catch (ArgumentException ex)
            {
                // Log and rethrow input validation errors
                Console.WriteLine($"ArgumentException: {ex.Message}");
                throw;
            }
            catch (KeyNotFoundException ex)
            {
                // Log and rethrow member not found errors
                Console.WriteLine($"KeyNotFoundException: {ex.Message}");
                throw;
            }
            catch (UnauthorizedAccessException ex)
            {
                // Log and rethrow password validation errors
                Console.WriteLine($"UnauthorizedAccessException: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                // Log unexpected errors
                Console.WriteLine($"Unexpected error in AuthenticateMemberAsync: {ex.Message}");
                throw new Exception("An unexpected error occurred during login.");
            }
        }

        public async Task RegisterAdminAsync(AdminRegisterRequestDto registerDto)
        {
            var admin = new Member
            {
                FullName = registerDto.FullName,
                NIC = registerDto.NIC,
                ContactDetails = registerDto.ContactDetails,
                RegistrationDate = DateTime.Now,
                IsAdmin = true // Mark as admin
            };

            var passwordHasher = new PasswordHasher<Member>();
            admin.PasswordHash = passwordHasher.HashPassword(admin, registerDto.Password);

            await _unitOfWork.Members.AddAsync(admin);
            await _unitOfWork.CompleteAsync();
        }



    }
}
