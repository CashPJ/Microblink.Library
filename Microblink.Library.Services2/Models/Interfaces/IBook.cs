using Microblink.Library.Services.Models.Dto;
using System.Collections.Generic;

namespace Microblink.Library.Services.Models.Interfaces
{
    public interface IBook : IEntity
    {
        //BookTitleDto BookTitle { get; set; }
        int BookTitleId { get; set; }
        ICollection<Rent> Rents { get; set; }
    }
}
