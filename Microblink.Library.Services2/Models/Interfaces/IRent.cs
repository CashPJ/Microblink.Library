using Microblink.Library.Services.Models.Dto;
using System;

namespace Microblink.Library.Services.Models.Interfaces
{
    public interface IRent : IEntity
    {
        Book Book { get; set; }
        int BookId { get; set; }
        DateTime DueDate { get; set; }
        DateTime RentDate { get; set; }
        DateTime ReturnDate { get; set; }
        User User { get; set; }
        int UserId { get; set; }
    }
}
