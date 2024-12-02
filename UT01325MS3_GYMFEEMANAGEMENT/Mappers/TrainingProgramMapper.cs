using UT01325MS3_GYMFEEMANAGEMENT.DTOs.Requests;
using UT01325MS3_GYMFEEMANAGEMENT.DTOs.Responses;
using UT01325MS3_GYMFEEMANAGEMENT.Models;

namespace UT01325MS3_GYMFEEMANAGEMENT.Mappers
{
    public class TrainingProgramMapper
    {
        public static TrainingProgram ToEntity(TrainingProgramRequestDto dto)
        {
            return new TrainingProgram
            {
                ProgramName = dto.ProgramName,
                Description = dto.Description,
                Price = dto.Price // Map Price

            };
        }

        public static TrainingProgramResponseDto ToDto(TrainingProgram entity)
        {
            return new TrainingProgramResponseDto
            {
                TrainingProgramId = entity.TrainingProgramId,
                ProgramName = entity.ProgramName,
                Description = entity.Description,
                Price = entity.Price, // Map Price
            };
        }
    }
}
