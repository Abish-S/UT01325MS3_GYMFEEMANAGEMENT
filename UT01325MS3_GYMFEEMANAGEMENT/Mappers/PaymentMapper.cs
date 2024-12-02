using UT01325MS3_GYMFEEMANAGEMENT.DTOs;
using UT01325MS3_GYMFEEMANAGEMENT.DTOs.Requests;
using UT01325MS3_GYMFEEMANAGEMENT.DTOs.Responses;
using UT01325MS3_GYMFEEMANAGEMENT.Models;

namespace UT01325MS3_GYMFEEMANAGEMENT.Mappers
{
    public class PaymentMapper
    {
        public static Payment ToEntity(PaymentRequestDto dto)
        {
            return new Payment
            {
                MemberId = dto.MemberId,
                PaymentType = dto.PaymentType,
                PaymentDate = dto.PaymentDate ?? DateTime.Now
            };
        }

        public static PaymentDto ToDto(Payment entity)
        {
            return new PaymentDto
            {
                PaymentId = entity.PaymentId,
                Amount = entity.Amount,
                PaymentDate = entity.PaymentDate,
                PaymentType = entity.PaymentType
            };
        }
        public static PaymentResponseDto ToResDto(Payment payment)
        {
            return new PaymentResponseDto
            {
                PaymentId = payment.PaymentId,
                MemberId = payment.MemberId,
                MemberFullName = payment.Member?.FullName, // Include the member's full name
                Amount = payment.Amount,
                PaymentType = payment.PaymentType,
                PaymentDate = payment.PaymentDate,
                DueDate = payment.DueDate // Include due date in response
            };
        }
    }
}
