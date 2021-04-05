using Microblink.Library.Services.Models.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microblink.Library.Services.Models.Interfaces
{
    public interface IBookTitle : IEntity
    {
        string Isbn { get; set; }
        string Author { get; set; }
        string Title { get; set; }
        ICollection<Book> Books { get; set; }
        ICollection<Rent> Rents { get; set; }
    }
}
