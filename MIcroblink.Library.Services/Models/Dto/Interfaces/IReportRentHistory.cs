using System;

namespace Microblink.Library.Services.Models.Dto.Interfaces
{
    /// <summary>
    /// Rent report item 
    /// </summary>
    public interface IReportRentHistory
    {  
        int Id { get; set; }
        int UserId { get; set; }
        int BookTitleId { get; set; }
        int BookId { get; set; }
        string FullName { get; set; }
        string BookTitle { get; set; }
        DateTime RentDate { get; set; }
        DateTime DueDate { get; set; }
        DateTime ReturnDate { get; set; }
    }
}
