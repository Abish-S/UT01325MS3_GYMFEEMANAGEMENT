using UT01325MS3_GYMFEEMANAGEMENT.Data;
using UT01325MS3_GYMFEEMANAGEMENT.Repositories.Interfaces;

namespace UT01325MS3_GYMFEEMANAGEMENT.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly GymDbContext _context;

        public UnitOfWork(GymDbContext context)
        {
            _context = context;

            // Initialize repositories
            Members = new MemberRepository(_context);
            Payments = new PaymentRepository(_context);
            TrainingPrograms = new TrainingProgramRepository(_context);
            Alerts = new AlertRepository(_context);
        }

        // Repository properties
        public IMemberRepository Members { get; private set; }
        public IPaymentRepository Payments { get; private set; }
        public ITrainingProgramRepository TrainingPrograms { get; private set; }
        public IAlertRepository Alerts { get; private set; }

        // Save changes to the database
        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        // Dispose the context to release resources
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
