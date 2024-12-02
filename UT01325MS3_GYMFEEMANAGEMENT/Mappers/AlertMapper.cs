using UT01325MS3_GYMFEEMANAGEMENT.DTOs.Requests;
using UT01325MS3_GYMFEEMANAGEMENT.DTOs.Responses;
using UT01325MS3_GYMFEEMANAGEMENT.Models;

namespace UT01325MS3_GYMFEEMANAGEMENT.Mappers
{
    public class AlertMapper
    {
        public static AlertResponseDto ToDto(Alert alert)
        {
            return new AlertResponseDto
            {
                AlertId = alert.AlertId,
                MemberId = alert.MemberId,
                MemberFullName = alert.Member?.FullName,
                Message = alert.Message,
                CreatedAt = alert.CreatedAt,
                IsResolved = alert.IsResolved
            };
        }

        /// <summary>
        /// Converts an AlertResponseDto to an Alert entity.
        /// (Typically used for testing or creating new alerts manually.)
        /// </summary>
        public static Alert ToEntity(AlertResponseDto dto)
        {
            return new Alert
            {
                AlertId = dto.AlertId,
                MemberId = dto.MemberId,
                Message = dto.Message,
                CreatedAt = dto.CreatedAt,
                IsResolved = dto.IsResolved
            };
        }
    }
}
