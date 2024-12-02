using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using UT01325MS3_GYMFEEMANAGEMENT.Data;
using UT01325MS3_GYMFEEMANAGEMENT.Models;
using UT01325MS3_GYMFEEMANAGEMENT.Repositories.Interfaces;

namespace UT01325MS3_GYMFEEMANAGEMENT.Repositories
{
    public class TrainingProgramRepository : ITrainingProgramRepository
    {
        private readonly GymDbContext _context;

        public TrainingProgramRepository(GymDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TrainingProgram>> GetAllAsync()
        {
            return await _context.TrainingPrograms.ToListAsync();
        }

        public async Task<TrainingProgram> GetByIdAsync(int id)
        {
            return await _context.TrainingPrograms.FirstOrDefaultAsync(tp => tp.TrainingProgramId == id);
        }

        public async Task AddAsync(TrainingProgram trainingProgram)
        {
            await _context.TrainingPrograms.AddAsync(trainingProgram);
        }

        public void Update(TrainingProgram trainingProgram)
        {
            _context.TrainingPrograms.Update(trainingProgram);
        }

        public void Delete(TrainingProgram trainingProgram)
        {
            _context.TrainingPrograms.Remove(trainingProgram);
        }
        // Find a training program based on a condition (e.g., for duplicate checking)
        public async Task<TrainingProgram> FindAsync(Expression<Func<TrainingProgram, bool>> predicate)
        {
            return await _context.TrainingPrograms.FirstOrDefaultAsync(predicate);
        }
        public async Task<List<TrainingProgram>> GetAllWithEnrollmentsAsync()
        {
            return await _context.TrainingPrograms
                .Include(tp => tp.MemberTrainingPrograms)
                    .ThenInclude(mt => mt.Member) // Eager load members
                .ToListAsync();
        }

    }
}
