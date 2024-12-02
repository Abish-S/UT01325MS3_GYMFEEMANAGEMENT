using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UT01325MS3_GYMFEEMANAGEMENT.DTOs.Requests;
using UT01325MS3_GYMFEEMANAGEMENT.DTOs.Responses;
using UT01325MS3_GYMFEEMANAGEMENT.Services;

namespace UT01325MS3_GYMFEEMANAGEMENT.Controllers
{
  
    [ApiController]
    [Route("api/[controller]")]
    public class MembersController : ControllerBase
    {
        private readonly MemberService _memberService;

        public MembersController(MemberService memberService)
        {
            _memberService = memberService;
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

    }

}
