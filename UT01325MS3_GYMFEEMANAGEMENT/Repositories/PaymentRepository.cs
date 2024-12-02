using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using UT01325MS3_GYMFEEMANAGEMENT.Data;
using UT01325MS3_GYMFEEMANAGEMENT.Models;
using UT01325MS3_GYMFEEMANAGEMENT.Repositories.Interfaces;

namespace UT01325MS3_GYMFEEMANAGEMENT.Repositories
{
    public class PaymentRepository: IPaymentRepository
    {
        private readonly GymDbContext _context;

        public PaymentRepository(GymDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Payment>> GetAllAsync()
        {
            return await _context.Payments.ToListAsync();
        }

        public async Task<Payment> GetByIdAsync(int id)
        {
            return await _context.Payments.FirstOrDefaultAsync(p => p.PaymentId == id);
        }

        public async Task AddAsync(Payment payment)
        {
            await _context.Payments.AddAsync(payment);
        }
        // Find a payment by condition (e.g., last payment)
        public async Task<Payment> FindAsync(Expression<Func<Payment, bool>> predicate)
        {
            return await _context.Payments.FirstOrDefaultAsync(predicate);
        }

        public void Update(Payment payment)
        {
            _context.Payments.Update(payment);
        }

        public void Delete(Payment payment)
        {
            _context.Payments.Remove(payment);
        }
        public async Task<List<Payment>> GetAllWithDetailsAsync()
        {
            return await _context.Payments
                .Include(p => p.Member) // Eager load member details
                .ToListAsync();
        }
    }
}
