using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using UT01325MS3_GYMFEEMANAGEMENT.DTOs.Requests;
using UT01325MS3_GYMFEEMANAGEMENT.DTOs.Responses;
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
            var members = await _memberService.GetAllMembersAsync();
            return Ok(members);
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
        public async Task<ActionResult> UpdateMember(int id, [FromBody] MemberRequestDto dto)
        {
            await _memberService.UpdateMemberAsync(id, dto);
            return NoContent();
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
        [HttpGet("current")]
        public async Task<IActionResult> GetCurrentMemberDetails()
        {
            try
            {
                // Extract NIC from JWT claims
                var nic = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;

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
