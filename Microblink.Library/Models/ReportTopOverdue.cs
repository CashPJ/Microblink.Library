

using Microblink.Library.Services.Models.Dto.Interfaces;

namespace Microblink.Library.Api.Models
{
    /// <inheritdoc/>
    public class ReportTopOverdue : IReportTopOverdue
    {
        public int UserId { get; set; }
        public string FullName { get; set; }
        public int OverdueInDays { get; set; }
    }
}
