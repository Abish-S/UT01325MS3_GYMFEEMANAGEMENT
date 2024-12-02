using UT01325MS3_GYMFEEMANAGEMENT.DTOs;
using UT01325MS3_GYMFEEMANAGEMENT.DTOs.Requests;
using UT01325MS3_GYMFEEMANAGEMENT.DTOs.Responses;
using UT01325MS3_GYMFEEMANAGEMENT.Enum;
using UT01325MS3_GYMFEEMANAGEMENT.Mappers;
using UT01325MS3_GYMFEEMANAGEMENT.Models;
using UT01325MS3_GYMFEEMANAGEMENT.Repositories.Interfaces;

namespace UT01325MS3_GYMFEEMANAGEMENT.Services
{
    public class PaymentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PaymentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<PaymentResponseDto>> GetAllPaymentsAsync()
        {
            var payments = await _unitOfWork.Payments.GetAllAsync();
            return payments.Select(PaymentMapper.ToResDto).ToList();
        }

        public async Task<PaymentResponseDto> GetPaymentByIdAsync(int id)
        {
            var payment = await _unitOfWork.Payments.GetByIdAsync(id);
            return payment != null ? PaymentMapper.ToResDto(payment) : null;
        }

        public async Task<PaymentResponseDto> AddPaymentAsync(PaymentRequestDto dto)
        {
            // Validate the member
            var member = await _unitOfWork.Members.GetByIdAsync(dto.MemberId);
            if (member == null)
            {
                throw new ArgumentException($"Member with ID {dto.MemberId} does not exist.");
            }

            // Fetch the member's training programs and calculate the total price
            var totalTrainingProgramAmount = member.MemberTrainingPrograms
                .Sum(mt => mt.TrainingProgram.Price);

            if (totalTrainingProgramAmount <= 0)
            {
                throw new ArgumentException("The member has no associated training programs or invalid prices.");
            }

            // Map the request DTO to the Payment entity
            var payment = new Payment
            {
                MemberId = dto.MemberId,
                Amount = totalTrainingProgramAmount, // Use the total calculated amount
                PaymentType = PaymentType.Monthly,
                PaymentDate = dto.PaymentDate ?? DateTime.Now
            };

            // Handle due date logic for monthly payments
            if (dto.PaymentType == PaymentType.Monthly)
            {
                var lastPayment = await _unitOfWork.Payments.FindAsync(p => p.MemberId == dto.MemberId && p.PaymentType == PaymentType.Monthly);
                payment.DueDate = (lastPayment?.DueDate ?? DateTime.Now).AddMonths(1);
            }

            // Save the payment
            await _unitOfWork.Payments.AddAsync(payment);
            await _unitOfWork.CompleteAsync();

            // Map the created entity to a response DTO and include the member's full name
            payment.Member = member; // Attach member details for mapping
            return PaymentMapper.ToResDto(payment);
        }

        public async Task UpdatePaymentAsync(int id, PaymentRequestDto dto)
        {
            var payment = await _unitOfWork.Payments.GetByIdAsync(id);
            if (payment != null)
            {
                payment.PaymentDate = (DateTime)dto.PaymentDate;
                payment.PaymentType = dto.PaymentType;

                _unitOfWork.Payments.Update(payment);
                await _unitOfWork.CompleteAsync();
            }
        }

        public async Task DeletePaymentAsync(int id)
        {
            var payment = await _unitOfWork.Payments.GetByIdAsync(id);
            if (payment != null)
            {
                _unitOfWork.Payments.Delete(payment);
                await _unitOfWork.CompleteAsync();
            }
        }

        public async Task AddMonthlyPaymentAsync(int memberId, decimal amount)
        {
            // Check if the member exists
            var member = await _unitOfWork.Members.GetByIdAsync(memberId);
            if (member == null)
            {
                throw new KeyNotFoundException($"Member with ID {memberId} not found.");
            }

            // Calculate the due date for the next payment (e.g., 1 month after the last payment)
            var lastPayment = await _unitOfWork.Payments.FindAsync(p => p.MemberId == memberId && p.PaymentType == PaymentType.Monthly);
            var nextDueDate = lastPayment?.DueDate ?? DateTime.Now.AddMonths(1);

            // Create the payment record
            var payment = new Payment
            {
                MemberId = memberId,
                Amount = amount,
                PaymentType = PaymentType.Monthly,
                PaymentDate = DateTime.Now,
                DueDate = nextDueDate
            };

            // Save the payment
            await _unitOfWork.Payments.AddAsync(payment);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<List<PaymentReportDto>> GeneratePaymentReportAsync()
        {
            // Fetch all payments with member details
            var payments = await _unitOfWork.Payments.GetAllWithDetailsAsync();

            // Map payments to the report DTO
            var report = payments.Select(payment => new PaymentReportDto
            {
                PaymentId = payment.PaymentId,
                MemberId = payment.MemberId,
                MemberFullName = payment.Member.FullName,
                MemberContact = payment.Member.ContactDetails,
                PaymentType = payment.PaymentType.ToString(),
                Amount = payment.Amount,
                PaymentDate = payment.PaymentDate,
                DueDate = payment.DueDate,
                IsOverdue = payment.DueDate.HasValue && payment.DueDate.Value < DateTime.Now
            }).ToList();

            return report;
        }

    }
}
