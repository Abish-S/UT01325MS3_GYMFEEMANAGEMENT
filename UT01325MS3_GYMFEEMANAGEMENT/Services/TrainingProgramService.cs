using Microsoft.AspNetCore.Http.HttpResults;
using System.Linq.Expressions;
using UT01325MS3_GYMFEEMANAGEMENT.DTOs;
using UT01325MS3_GYMFEEMANAGEMENT.DTOs.Requests;
using UT01325MS3_GYMFEEMANAGEMENT.DTOs.Responses;
using UT01325MS3_GYMFEEMANAGEMENT.Mappers;
using UT01325MS3_GYMFEEMANAGEMENT.Models;
using UT01325MS3_GYMFEEMANAGEMENT.Repositories.Interfaces;

namespace UT01325MS3_GYMFEEMANAGEMENT.Services
{
    public class TrainingProgramService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TrainingProgramService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<TrainingProgramResponseDto>> GetAllTrainingProgramsAsync(Expression<Func<TrainingProgram, bool>> predicate)
        {
            var trainingPrograms = await _unitOfWork.TrainingPrograms.GetAllAsync(predicate);
            return trainingPrograms.Select(TrainingProgramMapper.ToDto).ToList();
        }

        public async Task<TrainingProgramResponseDto> GetTrainingProgramByIdAsync(int id)
        {
            var trainingProgram = await _unitOfWork.TrainingPrograms.GetByIdAsync(id);
            return trainingProgram != null ? TrainingProgramMapper.ToDto(trainingProgram) : null;
        }
        public async Task<TrainingProgram> GetTrainingProgramById(int id)
        {
            return await _unitOfWork.TrainingPrograms.GetByIdAsync(id);
        }

        public async Task AddTrainingProgramAsync(TrainingProgramRequestDto dto)
        {

            // Check for duplicate training program by name
            var existingProgram = await _unitOfWork.TrainingPrograms
                .FindAsync(tp => tp.ProgramName.ToLower() == dto.ProgramName.ToLower());

            if (existingProgram != null)
            {

                throw new ArgumentException("Duplicate Training Program.");
            }
            var trainingProgram = TrainingProgramMapper.ToEntity(dto);
            await _unitOfWork.TrainingPrograms.AddAsync(trainingProgram);
            await _unitOfWork.CompleteAsync();
        }

        public async Task updateTrainingProgram(TrainingProgram dto)
        {


                     _unitOfWork.TrainingPrograms.Update(dto);
                await _unitOfWork.CompleteAsync();
               
           
        }

        public async Task UpdateTrainingProgramAsync(int id, TrainingProgramRequestDto dto)
        {
            var trainingProgram = await _unitOfWork.TrainingPrograms.GetByIdAsync(id);
            if (trainingProgram != null)
            {
                trainingProgram.ProgramName = dto.ProgramName;
                trainingProgram.Description = dto.Description;
                trainingProgram.ImagePath = dto.Base64Image;
                trainingProgram.Price = dto.Price;

                _unitOfWork.TrainingPrograms.Update(trainingProgram);
                await _unitOfWork.CompleteAsync();
            }
        }

        public async Task DeleteTrainingProgramAsync(int id)
        {
            var trainingProgram = await _unitOfWork.TrainingPrograms.GetByIdAsync(id);
            if (trainingProgram != null)
            {
                _unitOfWork.TrainingPrograms.Delete(trainingProgram);
                await _unitOfWork.CompleteAsync();
            }
        }
        public async Task<List<TrainingProgramReportDto>> GenerateTrainingProgramReportAsync()
        {
            // Fetch all training programs with member enrollments
            var trainingPrograms = await _unitOfWork.TrainingPrograms.GetAllWithEnrollmentsAsync();

            // Map training programs to the report DTO
            var report = trainingPrograms.Select(program => new TrainingProgramReportDto
            {
                TrainingProgramId = program.TrainingProgramId,
                ProgramName = program.ProgramName,
                Description = program.Description,
                Price = program.Price,
                TotalMembers = program.MemberTrainingPrograms.Count,
                EnrolledMembers = program.MemberTrainingPrograms
                    .Select(mt => new MemberEnrollmentDto
                    {
                        MemberId = mt.MemberId,
                        FullName = mt.Member.FullName,
                        ContactDetails = mt.Member.ContactDetails
                    }).ToList()
            }).ToList();

            return report;
        }
    }
}
