using System;

namespace Microblink.Library.Services.Models.Dto.Interfaces
{
    /// <summary>
    /// Book rental data
    /// </summary>
    public interface IRent
    {  
        int Id { get; set; }
        int UserId { get; set; }
        int BookId { get; set; }
        int BookTitleId { get; set; }
        DateTime RentDate { get; set; }
        DateTime DueDate { get; set; }
        DateTime? ReturnDate { get; set; }
    }
}
