using Microsoft.AspNetCore.Mvc;
using UT01325MS3_GYMFEEMANAGEMENT.DTOs.Requests;
using UT01325MS3_GYMFEEMANAGEMENT.DTOs.Responses;
using UT01325MS3_GYMFEEMANAGEMENT.Services;

namespace UT01325MS3_GYMFEEMANAGEMENT.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TrainingProgramsController : ControllerBase
    {

        private readonly TrainingProgramService _trainingProgramService;

        public TrainingProgramsController(TrainingProgramService trainingProgramService)
        {
            _trainingProgramService = trainingProgramService;
        }

        [HttpGet]
        public async Task<ActionResult<List<TrainingProgramResponseDto>>> GetTrainingPrograms()
        {
            var trainingPrograms = await _trainingProgramService.GetAllTrainingProgramsAsync();
            return Ok(trainingPrograms);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TrainingProgramResponseDto>> GetTrainingProgram(int id)
        {
            var trainingProgram = await _trainingProgramService.GetTrainingProgramByIdAsync(id);
            if (trainingProgram == null) return NotFound();
            return Ok(trainingProgram);
        }

        [HttpPost]
        public async Task<ActionResult> CreateTrainingProgram([FromBody] TrainingProgramRequestDto dto)
        {
            await _trainingProgramService.AddTrainingProgramAsync(dto);
            return CreatedAtAction(nameof(GetTrainingProgram), new { id = dto.ProgramName }, dto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateTrainingProgram(int id, [FromBody] TrainingProgramRequestDto dto)
        {
            await _trainingProgramService.UpdateTrainingProgramAsync(id, dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTrainingProgram(int id)
        {
            await _trainingProgramService.DeleteTrainingProgramAsync(id);
            return NoContent();
        }

        [HttpGet("report")]
        public async Task<ActionResult> GetTrainingProgramReport()
        {
            try
            {
                var report = await _trainingProgramService.GenerateTrainingProgramReportAsync();

                return Ok(new
                {
                    success = true,
                    message = "Training program report generated successfully.",
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
