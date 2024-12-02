using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;
using UT01325MS3_GYMFEEMANAGEMENT.DTOs;
using UT01325MS3_GYMFEEMANAGEMENT.DTOs.Requests;
using UT01325MS3_GYMFEEMANAGEMENT.DTOs.Responses;
using UT01325MS3_GYMFEEMANAGEMENT.Models;
using UT01325MS3_GYMFEEMANAGEMENT.Repositories.Interfaces;
using UT01325MS3_GYMFEEMANAGEMENT.Services;

namespace UT01325MS3_GYMFEEMANAGEMENT.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly PaymentService _paymentService;

        public PaymentsController(PaymentService paymentService, IUnitOfWork unitOfWork)
        {
            _paymentService = paymentService;
            _unitOfWork = unitOfWork;

        }

        [HttpGet]
        public async Task<ActionResult<List<PaymentResponseDto>>> GetPayments()
        {

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
            Expression<Func<Payment, bool>> predicate;

            if (isAdminUser)
            {
                // Admin can view all members
                predicate = m => true;  // No filter, get all members
            }
            else
            {
                // Non-admin: Filter by the current user's NIC
                predicate = m => m.Member == memberEntity;
            }

            var payments = await _paymentService.GetAllPaymentsAsync(predicate);
            return Ok(payments);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PaymentResponseDto>> GetPayment(int id)
        {
            var payment = await _paymentService.GetPaymentByIdAsync(id);
            if (payment == null) return NotFound();
            return Ok(payment);
        }

        [HttpPost]
        public async Task<ActionResult> CreatePayment([FromBody] PaymentRequestDto dto)
        {
           await _paymentService.AddPaymentAsync(dto);
            
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdatePayment(int id, [FromBody] PaymentRequestDto dto)
        {
            await _paymentService.UpdatePaymentAsync(id, dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePayment(int id)
        {
            await _paymentService.DeletePaymentAsync(id);
            return NoContent();
        }

        [HttpGet("report")]
        public async Task<ActionResult> GetPaymentReport()
        {
            try
            {
                var report = await _paymentService.GeneratePaymentReportAsync();

                return Ok(new
                {
                    success = true,
                    message = "Payment report generated successfully.",
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
