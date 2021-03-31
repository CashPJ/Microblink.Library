using Microblink.Library.Services.Models.Interfaces;
using System;
using System.Collections.Generic;

#nullable disable

namespace Microblink.Library.Services.Models
{
    public partial class BookDto : IEntity

    {
        public BookDto()
        {
            Rents = new HashSet<Rent>();
        }

        public int Id { get; set; }
        public int IdBookTitle { get; set; }

        public virtual BookTitleDto IdBookTitleNavigation { get; set; }
        public virtual ICollection<Rent> Rents { get; set; }
    }
}
