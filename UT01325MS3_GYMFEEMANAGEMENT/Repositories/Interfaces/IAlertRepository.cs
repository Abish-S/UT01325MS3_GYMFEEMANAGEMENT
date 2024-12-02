using System.Linq.Expressions;
using UT01325MS3_GYMFEEMANAGEMENT.Models;

namespace UT01325MS3_GYMFEEMANAGEMENT.Repositories.Interfaces
{
    public interface IAlertRepository
    {
        Task<List<Alert>> GetAllAsync(Expression<Func<Alert, bool>> predicate);
        Task<Alert> GetByIdAsync(int id);
        Task AddAsync(Alert alert);
        void Update(Alert alert);
        void Delete(Alert alert);
    }
}
