using UT01325MS3_GYMFEEMANAGEMENT.DTOs.Requests;
using UT01325MS3_GYMFEEMANAGEMENT.DTOs.Responses;
using UT01325MS3_GYMFEEMANAGEMENT.Mappers;
using UT01325MS3_GYMFEEMANAGEMENT.Models;
using UT01325MS3_GYMFEEMANAGEMENT.Repositories.Interfaces;

namespace UT01325MS3_GYMFEEMANAGEMENT.Services
{
    public class AlertService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AlertService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task GenerateOverdueAlertsAsync()
        {
            // Fetch all members with their payments
            var members = await _unitOfWork.Members.GetAllWithPaymentsAsync();

            foreach (var member in members)
            {
                // Find overdue payments
                var overduePayments = member.Payments
                    .Where(p => p.DueDate.HasValue && p.DueDate.Value < DateTime.Now)
                    .ToList();

                foreach (var payment in overduePayments)
                {
                    // Check if an alert already exists for this overdue payment
                    var existingAlert = await _unitOfWork.Alerts.GetAllAsync(a =>
                        a.MemberId == member.MemberId &&
                        a.Message.Contains($"overdue payment as of {payment.DueDate.Value.ToShortDateString()}"));

                    if (!existingAlert.Any())
                    {
                        // Generate a new alert
                        var alert = new Alert
                        {
                            MemberId = member.MemberId,
                            Message = $"Member {member.FullName} has an overdue payment as of {payment.DueDate.Value.ToShortDateString()}."
                        };

                        await _unitOfWork.Alerts.AddAsync(alert);
                    }
                }
            }

            // Save all changes
            await _unitOfWork.CompleteAsync();
        }


        /// <summary>
        /// Retrieves all unresolved alerts.
        /// </summary>
        public async Task<List<Alert>> GetUnresolvedAlertsAsync()
        {
            return await _unitOfWork.Alerts.GetAllAsync(a => !a.IsResolved);
        }

        public async Task ResolveAlertAsync(int alertId)
        {
            var alert = await _unitOfWork.Alerts.GetByIdAsync(alertId);
            if (alert == null)
            {
                throw new KeyNotFoundException($"Alert with ID {alertId} not found.");
            }

            alert.IsResolved = true;
            _unitOfWork.Alerts.Update(alert);
            await _unitOfWork.CompleteAsync();
        }
        public async Task GenerateMembershipRemindersAsync()
        {
            // Define the reminder period (e.g., 7 days)
            var reminderThreshold = DateTime.Now.AddDays(7);

            // Fetch all members
            var members = await _unitOfWork.Members.GetAllWithDetailsAsync();

            foreach (var member in members)
            {
                // Check if the membership expiration date is within the reminder threshold
                if (member.MembershipExpirationDate.HasValue &&
                    member.MembershipExpirationDate.Value.Date <= reminderThreshold.Date &&
                    member.MembershipExpirationDate.Value.Date > DateTime.Now.Date)
                {
                    // Check if an alert already exists for this reminder
                    var existingAlert = await _unitOfWork.Alerts.GetAllAsync(a =>
                        a.MemberId == member.MemberId &&
                        a.Message.Contains("membership renewal reminder"));

                    if (!existingAlert.Any())
                    {
                        // Create a new alert
                        var alert = new Alert
                        {
                            MemberId = member.MemberId,
                            Message = $"Reminder: Membership renewal due on {member.MembershipExpirationDate.Value.ToShortDateString()}."
                        };

                        await _unitOfWork.Alerts.AddAsync(alert);
                    }
                }

                // Check for upcoming due dates in payments
                var upcomingPayments = member.Payments
                    .Where(p => p.DueDate.HasValue &&
                                p.DueDate.Value.Date <= reminderThreshold.Date &&
                                p.DueDate.Value.Date > DateTime.Now.Date)
                    .ToList();

                foreach (var payment in upcomingPayments)
                {
                    var existingAlert = await _unitOfWork.Alerts.GetAllAsync(a =>
                        a.MemberId == member.MemberId &&
                        a.Message.Contains($"payment due on {payment.DueDate.Value.ToShortDateString()}"));

                    if (!existingAlert.Any())
                    {
                        // Create a new alert
                        var alert = new Alert
                        {
                            MemberId = member.MemberId,
                            Message = $"Reminder: Payment due on {payment.DueDate.Value.ToShortDateString()}."
                        };

                        await _unitOfWork.Alerts.AddAsync(alert);
                    }
                }
            }

            // Commit all alerts
            await _unitOfWork.CompleteAsync();
        }
        public async Task<List<AlertResponseDto>> GetUnresolvedRemindersAsync(string reminderType = null)
        {
            // Fetch all unresolved alerts
            var alerts = await _unitOfWork.Alerts.GetAllAsync(a => !a.IsResolved);

            // Filter alerts based on reminder type
            if (!string.IsNullOrEmpty(reminderType))
            {
                alerts = alerts.Where(a =>
                    reminderType.Equals("membership", System.StringComparison.OrdinalIgnoreCase) &&
                    a.Message.Contains("membership renewal reminder") ||
                    reminderType.Equals("payment", System.StringComparison.OrdinalIgnoreCase) &&
                    a.Message.Contains("payment due on"))
                    .ToList();
            }

            // Map to DTOs
            return alerts.Select(AlertMapper.ToDto).ToList();
        }

    }
}
