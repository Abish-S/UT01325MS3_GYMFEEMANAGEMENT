using System.Linq.Expressions;
using UT01325MS3_GYMFEEMANAGEMENT.Models;

namespace UT01325MS3_GYMFEEMANAGEMENT.Repositories.Interfaces
{
    public interface ITrainingProgramRepository
    {
        Task<IEnumerable<TrainingProgram>> GetAllAsync(Expression<Func<TrainingProgram, bool>> predicate);
        Task<TrainingProgram> GetByIdAsync(int id);
        Task AddAsync(TrainingProgram trainingProgram);
        void Update(TrainingProgram trainingProgram);
        void Delete(TrainingProgram trainingProgram);
        Task<TrainingProgram> FindAsync(Expression<Func<TrainingProgram, bool>> predicate); // New method
        Task<List<TrainingProgram>> GetAllWithEnrollmentsAsync();

    }
}
