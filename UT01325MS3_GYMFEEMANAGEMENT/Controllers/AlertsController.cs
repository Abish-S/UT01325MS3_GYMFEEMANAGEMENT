using Microsoft.AspNetCore.Mvc;
using UT01325MS3_GYMFEEMANAGEMENT.DTOs.Requests;
using UT01325MS3_GYMFEEMANAGEMENT.DTOs.Responses;
using UT01325MS3_GYMFEEMANAGEMENT.Services;

namespace UT01325MS3_GYMFEEMANAGEMENT.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AlertsController : ControllerBase
    {

        private readonly AlertService _alertService;

        public AlertsController(AlertService alertService)
        {
            _alertService = alertService;
        }

        [HttpGet("unresolved")]
        public async Task<ActionResult> GetUnresolvedAlerts()
        {
            try
            {
                var alerts = await _alertService.GetUnresolvedAlertsAsync();

                return Ok(new
                {
                    success = true,
                    message = "Unresolved alerts retrieved successfully.",
                    data = alerts
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "An error occurred while retrieving unresolved alerts.",
                    detail = ex.Message
                });
            }
        }

        /// <summary>
        /// Marks an alert as resolved.
        /// </summary>
        [HttpPut("{alertId}/resolve")]
        public async Task<ActionResult> ResolveAlert(int alertId)
        {
            try
            {
                await _alertService.ResolveAlertAsync(alertId);

                return Ok(new
                {
                    success = true,
                    message = $"Alert with ID {alertId} has been marked as resolved."
                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new
                {
                    success = false,
                    message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "An error occurred while resolving the alert.",
                    detail = ex.Message
                });
            }
        }

        /// <summary>
        /// Manually generates overdue alerts for all overdue payments.
        /// </summary>
        [HttpPost("generate")]
        public async Task<ActionResult> GenerateOverdueAlerts()
        {
            try
            {
                await _alertService.GetUnresolvedRemindersAsync();

                return Ok(new
                {
                    success = true,
                    message = "Overdue alerts have been generated successfully."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "An error occurred while generating overdue alerts.",
                    detail = ex.Message
                });
            }


        
    }
        [HttpGet("reminders")]
        public async Task<ActionResult> GetReminders([FromQuery] string reminderType = null)
        {
            try
            {
                var reminders = await _alertService.GetUnresolvedRemindersAsync(reminderType);

                return Ok(new
                {
                    success = true,
                    message = "Reminders retrieved successfully.",
                    data = reminders
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "An error occurred while retrieving reminders.",
                    detail = ex.Message
                });
            }
        }

    }
}
