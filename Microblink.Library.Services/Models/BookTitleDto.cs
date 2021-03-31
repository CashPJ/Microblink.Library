using Microblink.Library.Services.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace Microblink.Library.Services.Models
{
    public partial class BookTitleDto : IBookTitle
    {
        public BookTitleDto()
        {
            Books = new HashSet<BookDto>();
        }

        public int Id { get; set; }
        public string Isbn { get; set; }
        public string Author { get; set; }
        public string Title { get; set; }

        public virtual ICollection<BookDto> Books { get; set; }

        [NotMapped]
        public virtual int TotalCount => Books.Count;
    }
}
