using Microsoft.EntityFrameworkCore;
using UT01325MS3_GYMFEEMANAGEMENT.Entity;

namespace UT01325MS3_GYMFEEMANAGEMENT.Data
{
    public class GymDbContext
    {
        public DbSet<Member> Members { get; set; }
        public DbSet<TrainingProgram> TrainingPrograms { get; set; }
        public DbSet<Payment> Payments { get; set; }
    }
}
