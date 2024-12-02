using UT01325MS3_GYMFEEMANAGEMENT.Data;
using UT01325MS3_GYMFEEMANAGEMENT.Models;
using UT01325MS3_GYMFEEMANAGEMENT.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace UT01325MS3_GYMFEEMANAGEMENT.Repositories
{
    public class AlertRepository : IAlertRepository
    {
        private readonly GymDbContext _context;

        public AlertRepository(GymDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all alerts matching a specific condition.
        /// </summary>
        public async Task<List<Alert>> GetAllAsync(Expression<Func<Alert, bool>> predicate)
        {
            return await _context.Alerts
                .Include(a => a.Member) // Include related Member entity
                .Where(predicate)
                .ToListAsync();
        }

        /// <summary>
        /// Retrieves a specific alert by ID.
        /// </summary>
        public async Task<Alert> GetByIdAsync(int id)
        {
            return await _context.Alerts
                .Include(a => a.Member) // Include related Member entity
                .FirstOrDefaultAsync(a => a.AlertId == id);
        }

        /// <summary>
        /// Adds a new alert to the database.
        /// </summary>
        public async Task AddAsync(Alert alert)
        {
            await _context.Alerts.AddAsync(alert);
        }

        /// <summary>
        /// Updates an existing alert in the database.
        /// </summary>
        public void Update(Alert alert)
        {
            _context.Alerts.Update(alert);
        }

        /// <summary>
        /// Deletes an alert from the database.
        /// </summary>
        public void Delete(Alert alert)
        {
            _context.Alerts.Remove(alert);
        }
    }
}
