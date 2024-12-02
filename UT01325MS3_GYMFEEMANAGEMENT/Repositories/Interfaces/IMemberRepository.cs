using System.Linq.Expressions;
using UT01325MS3_GYMFEEMANAGEMENT.Models;

namespace UT01325MS3_GYMFEEMANAGEMENT.Repositories.Interfaces
{
    public interface IMemberRepository
    {
        Task<IEnumerable<Member>> GetAllAsync();
        Task<List<Member>> GetAllMemberAsync(Expression<Func<Member, bool>> predicate);

        Task<List<Member>> GetAllRegularMembersAsync();
        Task<Member> GetByIdAsync(int id);
        Task AddAsync(Member member);
        void Update(Member member);
        void Delete(Member member);
        Task<List<Member>> GetAllWithDetailsAsync();
        Task<List<Member>> GetAllWithPaymentsAsync();
    }
}
