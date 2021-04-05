
using Microblink.Library.Services.Models.Dto.Interfaces;
using System;

namespace Microblink.Library.Api.Models
{
    /// <inheritdoc/>
    public class ReportRentHistory : IReportRentHistory
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int BookTitleId { get; set; }
        public int BookId { get; set; }
        public string FullName { get; set; }
        public string BookTitle { get; set; }
        public DateTime RentDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime ReturnDate { get; set; }
    }
}
