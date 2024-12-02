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

        public async Task<string> AuthenticateMemberAsync(LoginRequestDto loginDto)
        {
            try
            {
                // Fetch member by NIC (acting as username)
                var member = await _unitOfWork.Members.GetAllMemberAsync(m => m.NIC == loginDto.Username);
                var memberEntity = member.FirstOrDefault();

                if (memberEntity == null)
                {
                    return null; // Member not found
                }

                // Validate the password using PasswordHasher
                var passwordHasher = new PasswordHasher<Member>();
                var result = passwordHasher.VerifyHashedPassword(memberEntity, memberEntity.PasswordHash, loginDto.Password);

                if (result != PasswordVerificationResult.Success)
                {
                    return null; // Invalid password
                }

                // Generate JWT token
                var token = GenerateJwtToken(memberEntity);

                return token;
            }
            catch (Exception ex)
            {
                // Log the exception (you can use ILogger here for better logging)
                Console.WriteLine($"Error in AuthenticateMemberAsync: {ex.Message}");
                throw new Exception("An unexpected error occurred during login.");
            }
        }

    }
}
