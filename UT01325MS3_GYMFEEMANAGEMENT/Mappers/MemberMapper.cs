using Microsoft.AspNetCore.Identity;
using UT01325MS3_GYMFEEMANAGEMENT.DTOs.Requests;
using UT01325MS3_GYMFEEMANAGEMENT.DTOs.Responses;
using UT01325MS3_GYMFEEMANAGEMENT.Models;

namespace UT01325MS3_GYMFEEMANAGEMENT.Mappers
{
    public class MemberMapper
    {
        public static Member ToEntity(MemberRequestDto dto)
        {
            return new Member
            {
                FullName = dto.FullName,
                NIC = dto.NIC,
                ContactDetails = dto.ContactDetails,
                PasswordHash = dto.Password,
                RegistrationDate = dto.RegistrationDate, // Default to now if not provided
                IsRegistrationFeePaid = false // Default to unpaid
                
            };
        }

        public static MemberResponseDto ToDto(Member member)
        {
            return new MemberResponseDto
            {
                MemberId = member.MemberId,
                FullName = member.FullName,
                NIC = member.NIC,
                ContactDetails = member.ContactDetails,
                RegistrationDate = member.RegistrationDate,
                IsRegistrationFeePaid = member.IsRegistrationFeePaid,
                SelectedTrainingPrograms = member.MemberTrainingPrograms?
                    .Select(mt => new TrainingProgramResponseDto
                    {
                        TrainingProgramId = mt.TrainingProgramId,
                        ProgramName = mt.TrainingProgram.ProgramName,
                        Description = mt.TrainingProgram.Description,
                        Price = mt.TrainingProgram.Price
                    }).ToList()
            };
        }
    }
}
