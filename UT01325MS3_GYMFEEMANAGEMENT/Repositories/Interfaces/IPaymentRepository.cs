using System.Linq.Expressions;
using UT01325MS3_GYMFEEMANAGEMENT.Models;

namespace UT01325MS3_GYMFEEMANAGEMENT.Repositories.Interfaces
{
    public interface IPaymentRepository
    {
        Task<IEnumerable<Payment>> GetAllAsync(Expression<Func<Payment, bool>> predicate);
        Task<Payment> GetByIdAsync(int id);
        Task AddAsync(Payment payment);
        Task<Payment> FindAsync(Expression<Func<Payment, bool>> predicate);

        void Update(Payment payment);
        void Delete(Payment payment);

        Task<List<Payment>> GetAllWithDetailsAsync();
    }
}
