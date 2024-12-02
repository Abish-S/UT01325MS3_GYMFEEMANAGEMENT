using Microsoft.AspNetCore.Identity;
using UT01325MS3_GYMFEEMANAGEMENT.DTOs;
using UT01325MS3_GYMFEEMANAGEMENT.DTOs.Requests;
using UT01325MS3_GYMFEEMANAGEMENT.DTOs.Responses;
using UT01325MS3_GYMFEEMANAGEMENT.Enum;
using UT01325MS3_GYMFEEMANAGEMENT.Mappers;
using UT01325MS3_GYMFEEMANAGEMENT.Models;
using UT01325MS3_GYMFEEMANAGEMENT.Repositories.Interfaces;

namespace UT01325MS3_GYMFEEMANAGEMENT.Services
{
    public class MemberService
    {
        private readonly IUnitOfWork _unitOfWork;

        public MemberService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<MemberResponseDto>> GetAllMembersAsync()
        {
            var members = await _unitOfWork.Members.GetAllAsync();
            return members.Select(MemberMapper.ToDto).ToList();
        }

        public async Task<MemberResponseDto> GetMemberByIdAsync(int id)
        {
            var member = await _unitOfWork.Members.GetByIdAsync(id);
            return member != null ? MemberMapper.ToDto(member) : null;
        }

        public async Task AddMemberAsync(MemberRequestDto memberDto)
        {
            // Validate the payment details
            if (memberDto.Payment == null || memberDto.Payment.PaymentType != PaymentType.Registration)
            {
                throw new ArgumentException("Valid initial registration payment details are required.");
            }

            // Create the member entity
            var member = MemberMapper.ToEntity(memberDto);
            member.IsRegistrationFeePaid = false; // Default to unpaid
            var passwordHasher = new PasswordHasher<Member>();
            var passwordHash = passwordHasher.HashPassword(member, memberDto.Password);

            member.PasswordHash = passwordHash;

            // Add the member to the database
            await _unitOfWork.Members.AddAsync(member);
            await _unitOfWork.CompleteAsync();

            // Create the payment entity for the registration fee
            var payment = new Payment
            {
                MemberId = member.MemberId,
                PaymentDate = memberDto.Payment.PaymentDate ?? DateTime.Now,
                PaymentType = PaymentType.Registration
            };

            await _unitOfWork.Payments.AddAsync(payment);

            // Associate the selected training programs with the member
            if (memberDto.SelectedTrainingProgramIds != null && memberDto.SelectedTrainingProgramIds.Any())
            {
                foreach (var programId in memberDto.SelectedTrainingProgramIds)
                {
                    var trainingProgram = await _unitOfWork.TrainingPrograms.GetByIdAsync(programId);
                    if (trainingProgram != null)
                    {
                        member.MemberTrainingPrograms.Add(new MemberTrainingProgram
                        {
                            MemberId = member.MemberId,
                            TrainingProgramId = trainingProgram.TrainingProgramId
                        });
                    }
                }
            }

            // Mark the registration fee as paid and update the member
            member.IsRegistrationFeePaid = true;
            _unitOfWork.Members.Update(member);

            // Save all changes
            await _unitOfWork.CompleteAsync();
        }



        public async Task UpdateMemberAsync(int id, MemberRequestDto dto)
        {
            var member = await _unitOfWork.Members.GetByIdAsync(id);
            if (member != null)
            {
                member.FullName = dto.FullName;
                member.NIC = dto.NIC;
                member.ContactDetails = dto.ContactDetails;
                member.RegistrationDate = dto.RegistrationDate;

                _unitOfWork.Members.Update(member);
                await _unitOfWork.CompleteAsync();
            }
        }

        public async Task DeleteMemberAsync(int id)
        {
            var member = await _unitOfWork.Members.GetByIdAsync(id);
            if (member != null)
            {
                _unitOfWork.Members.Delete(member);
                await _unitOfWork.CompleteAsync();
            }
        }
        public async Task<List<MemberReportDto>> GenerateMemberReportAsync()
        {
            // Fetch all members with their training programs and payments
            var members = await _unitOfWork.Members.GetAllWithDetailsAsync();

            // Map members to the report DTO
            var report = members.Select(member => new MemberReportDto
            {
                MemberId = member.MemberId,
                FullName = member.FullName,
                NIC = member.NIC,
                ContactDetails = member.ContactDetails,
                TrainingPrograms = member.MemberTrainingPrograms
                    .Select(mt => mt.TrainingProgram.ProgramName)
                    .ToList(),
                TotalPaid = member.Payments.Sum(p => p.Amount), // Sum of all payments
                LastPaymentDate = member.Payments
                    .OrderByDescending(p => p.PaymentDate)
                    .FirstOrDefault()?.PaymentDate,
                NextDueDate = member.Payments
                    .OrderByDescending(p => p.PaymentDate)
                    .FirstOrDefault()?.DueDate
            }).ToList();

            return report;
        }


    }
}
