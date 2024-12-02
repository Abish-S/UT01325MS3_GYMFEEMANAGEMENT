using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using UT01325MS3_GYMFEEMANAGEMENT.DTOs;
using UT01325MS3_GYMFEEMANAGEMENT.DTOs.Requests;
using UT01325MS3_GYMFEEMANAGEMENT.DTOs.Responses;
using UT01325MS3_GYMFEEMANAGEMENT.Services;

namespace UT01325MS3_GYMFEEMANAGEMENT.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController : ControllerBase
    {

        private readonly PaymentService _paymentService;

        public PaymentsController(PaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpGet]
        public async Task<ActionResult<List<PaymentResponseDto>>> GetPayments()
        {
            var payments = await _paymentService.GetAllPaymentsAsync();
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
