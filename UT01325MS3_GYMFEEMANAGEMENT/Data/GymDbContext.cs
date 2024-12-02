using Microsoft.EntityFrameworkCore;
using UT01325MS3_GYMFEEMANAGEMENT.Models;

namespace UT01325MS3_GYMFEEMANAGEMENT.Data
{
    public class GymDbContext:DbContext
    {
        public GymDbContext(DbContextOptions<GymDbContext> options)
           : base(options)
        {
        }
       
        public DbSet<Member> Members { get; set; }
        public DbSet<TrainingProgram> TrainingPrograms { get; set; }
        public DbSet<MemberTrainingProgram> MemberTrainingPrograms { get; set; }

        public DbSet<Payment> Payments { get; set; }
        public DbSet<Alert> Alerts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // ==========================
            // MemberTrainingProgram Configuration
            // ==========================
            modelBuilder.Entity<MemberTrainingProgram>()
                .HasKey(mt => new { mt.MemberId, mt.TrainingProgramId }); // Composite Key

            modelBuilder.Entity<MemberTrainingProgram>()
                .HasOne(mt => mt.Member)
                .WithMany(m => m.MemberTrainingPrograms)
                .HasForeignKey(mt => mt.MemberId);

            modelBuilder.Entity<MemberTrainingProgram>()
                .HasOne(mt => mt.TrainingProgram)
                .WithMany(tp => tp.MemberTrainingPrograms)
                .HasForeignKey(mt => mt.TrainingProgramId);

            // ==========================
            // Member Configuration
            // ==========================
            modelBuilder.Entity<Member>(entity =>
            {
                entity.HasKey(m => m.MemberId); // Primary Key
                entity.Property(m => m.FullName)
                      .IsRequired()
                      .HasMaxLength(100); // Example constraint
                entity.Property(m => m.NIC)
                      .IsRequired()
                      .HasMaxLength(20); // Example constraint
                entity.Property(m => m.ContactDetails)
                      .HasMaxLength(200);
                entity.Property(m => m.RegistrationDate)
                      .HasDefaultValueSql("GETDATE()"); // Default value for RegistrationDate
            });

            // ==========================
            // TrainingProgram Configuration
            // ==========================
            modelBuilder.Entity<TrainingProgram>(entity =>
            {
                entity.HasKey(tp => tp.TrainingProgramId); // Primary Key
                entity.Property(tp => tp.ProgramName)
                      .IsRequired()
                      .HasMaxLength(100);
                entity.Property(tp => tp.Description)
                      .HasMaxLength(500);
                entity.Property(tp => tp.Price)
                      .HasColumnType("decimal(18, 2)"); // Precision and scale for monetary values
            });

            // ==========================
            // Payment Configuration
            // ==========================
            modelBuilder.Entity<Payment>(entity =>
            {
                entity.HasKey(p => p.PaymentId); // Primary Key
                entity.Property(p => p.Amount)
                      .IsRequired()
                      .HasColumnType("decimal(18, 2)"); // Precision and scale for monetary values
                entity.Property(p => p.PaymentType)
                      .IsRequired()
                      .HasConversion<int>(); // Store enum as int
                entity.Property(p => p.PaymentDate)
                      .HasDefaultValueSql("GETDATE()"); // Default value for PaymentDate
                entity.HasOne(p => p.Member)
                      .WithMany(m => m.Payments)
                      .HasForeignKey(p => p.MemberId)
                      .OnDelete(DeleteBehavior.Cascade); // Cascade delete payments if member is deleted
            });

            // ==========================
            // Alert Configuration
            // ==========================
            modelBuilder.Entity<Alert>(entity =>
            {
                entity.HasKey(a => a.AlertId); // Primary Key
                entity.Property(a => a.Message)
                      .IsRequired()
                      .HasMaxLength(500); // Example constraint
               
                entity.HasOne(a => a.Member)
                      .WithMany(m => m.Alerts)
                      .HasForeignKey(a => a.MemberId)
                      .OnDelete(DeleteBehavior.Cascade); // Cascade delete alerts if member is deleted
            });
        }


    }
}
