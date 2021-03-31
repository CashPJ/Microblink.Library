using Microblink.Library.Services.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace Microblink.Library.Models
{
    public partial class BookTitle : IBookTitle
    {
        public int Id { get; set; }
        public string Isbn { get; set; }
        public string Author { get; set; }
        public string Title { get; set; }

        public virtual ICollection<Book> Books { get; set; }

    }
}
