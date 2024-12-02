using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;
using UT01325MS3_GYMFEEMANAGEMENT.DTOs.Requests;
using UT01325MS3_GYMFEEMANAGEMENT.DTOs.Responses;
using UT01325MS3_GYMFEEMANAGEMENT.Models;
using UT01325MS3_GYMFEEMANAGEMENT.Repositories.Interfaces;
using UT01325MS3_GYMFEEMANAGEMENT.Services;

namespace UT01325MS3_GYMFEEMANAGEMENT.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MembersController : ControllerBase
    {
        private readonly MemberService _memberService;
        private readonly IUnitOfWork _unitOfWork;

      

        public MembersController(MemberService memberService, IUnitOfWork unitOfWork)
        {
            _memberService = memberService;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<ActionResult<List<MemberResponseDto>>> GetMembers()
        {
            try
            {
                // Get the Authorization header
                string authorizationHeader = Request.Headers["Authorization"];

                if (string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.StartsWith("Bearer "))
                {
                    return Unauthorized(new { success = false, message = "Authorization header is missing or invalid." });
                }

                // Extract the token
                string token = authorizationHeader.Substring("Bearer ".Length).Trim();

                // Decode the token (optional, for debugging purposes)
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);
                var claims = jwtToken.Claims.ToList();
                // Extract claims from the JWT token

                
                // Extract NIC from claims
                var nic = claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;

                if (string.IsNullOrEmpty(nic))
                {
                    return Unauthorized(new { success = false, message = "User is not authenticated." });
                }
                var currentmember = await _unitOfWork.Members.GetAllMemberAsync(m => m.NIC == nic);
                var memberEntity = currentmember.FirstOrDefault();

                bool isAdminUser = memberEntity.IsAdmin;
                Expression<Func<Member, bool>> predicate;

                if (isAdminUser)
                {
                    // Admin can view all members
                    predicate = m => true;  // No filter, get all members
                }
                else
                {
                    // Non-admin: Filter by the current user's NIC
                    predicate = m => m.NIC == nic;
                }

                // Fetch the member's details from the database
                var member = await _memberService.GetAllMembersPredicateAsync(predicate);
              

               

                return Ok(member);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "An error occurred.", detail = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MemberResponseDto>> GetMember(int id)
        {
            var member = await _memberService.GetMemberByIdAsync(id);
            if (member == null) return NotFound();
            return Ok(member);
        }

        [HttpPost]
        public async Task<ActionResult> CreateMember([FromBody] MemberRequestDto dto)
        {
            try
            {
                await _memberService.AddMemberAsync(dto);
                var response = new
                {
                    success = true,
                    message = "Member registered successfully with the initial registration fee.",
                    
                };

                return Ok(response);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateMember(int id, [FromBody] MemberUpdateRequest dto)
        {
            await _memberService.UpdateMemberAsync(id, dto);
            var response = new
            {
                success = true,
                message = "Member updated successfully",

            };
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMember(int id)
        {
            await _memberService.DeleteMemberAsync(id);
            return NoContent();
        }
        [HttpGet("report")]
        public async Task<ActionResult> GetMemberReport()
        {
            try
            {
                var report = await _memberService.GenerateMemberReportAsync();

                return Ok(new
                {
                    success = true,
                    message = "Member report generated successfully.",
                    data = report
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "An unexpected error occurred while generating the report.",
                    detail = ex.Message
                });
            }
        }
       [Authorize]
[HttpGet("current")]
public async Task<IActionResult> GetCurrentMemberDetails()
{
    try
    {
        // Get the Authorization header
        string authorizationHeader = Request.Headers["Authorization"];

        if (string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.StartsWith("Bearer "))
        {
            return Unauthorized(new { success = false, message = "Authorization header is missing or invalid." });
        }

        // Extract the token
        string token = authorizationHeader.Substring("Bearer ".Length).Trim();

        // Decode the token (optional, for debugging purposes)
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);
        var claims = jwtToken.Claims.ToList();

        // Print claims for debugging
        foreach (var claim in claims)
        {
            Console.WriteLine($"Claim Type: {claim.Type}, Claim Value: {claim.Value}");
        }

        // Extract NIC from claims
        var nic = claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;

        if (string.IsNullOrEmpty(nic))
        {
            return Unauthorized(new { success = false, message = "User is not authenticated." });
        }

        // Fetch the member's details from the database
        var member = await _unitOfWork.Members.GetAllMemberAsync(m => m.NIC == nic);
        var memberEntity = member.FirstOrDefault();

        if (memberEntity == null)
        {
            return NotFound(new { success = false, message = "Member not found." });
        }

        // Map member details to DTO
        var memberDetails = new
        {
            memberEntity.FullName,
            memberEntity.NIC,
            memberEntity.ContactDetails,
            memberEntity.RegistrationDate,
            memberEntity.IsAdmin
        };

        return Ok(new { success = true, data = memberDetails });
    }
    catch (Exception ex)
    {
        return StatusCode(500, new { success = false, message = "An error occurred.", detail = ex.Message });
    }
}


    }

}
