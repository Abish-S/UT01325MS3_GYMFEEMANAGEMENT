using UT01325MS3_GYMFEEMANAGEMENT.Data;
using UT01325MS3_GYMFEEMANAGEMENT.Models;
using UT01325MS3_GYMFEEMANAGEMENT.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace UT01325MS3_GYMFEEMANAGEMENT.Repositories
{
    public class MemberRepository : IMemberRepository
    {
        private readonly GymDbContext _context;

        public MemberRepository(GymDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Member>> GetAllAsync()
        {
            return await _context.Members.Include(m => m.Payments).ToListAsync();
        }

        public async Task<List<Member>> GetAllMemberAsync(Expression<Func<Member, bool>> predicate)
        {
            return await _context.Members
                .Where(predicate)  // Apply the provided condition (e.g., NIC filter)
                .ToListAsync();    // Convert the result to a list asynchronously
        }
        public async Task<Member> GetByIdAsync(int id)
        {
            return await _context.Members
               .Include(m => m.MemberTrainingPrograms)
               .ThenInclude(mt => mt.TrainingProgram)
               .FirstOrDefaultAsync(m => m.MemberId == id);
        }

        public async Task AddAsync(Member member)
        {
            await _context.Members.AddAsync(member);
        }

        public void Update(Member member)
        {
            _context.Members.Update(member);
        }

        public void Delete(Member member)
        {
            _context.Members.Remove(member);
        }
        public async Task<List<Member>> GetAllWithDetailsAsync()
        {
            return await _context.Members
                .Include(m => m.MemberTrainingPrograms)
                    .ThenInclude(mt => mt.TrainingProgram)
                .Include(m => m.Payments)
                .ToListAsync();
        }

        public async Task<List<Member>> GetAllWithPaymentsAsync()
        {
            return await _context.Members
                .Include(m => m.Payments) // Eager load payments
                .ToListAsync();
        }

        public async Task<List<Member>> GetAllRegularMembersAsync()
        {
            return await _context.Members
                .Where(m => !m.IsAdmin) // Exclude admin users
                .ToListAsync();
        }


    }
}
