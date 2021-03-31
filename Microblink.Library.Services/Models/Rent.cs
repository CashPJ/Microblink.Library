using Microblink.Library.Services.Models.Interfaces;
using System;
using System.Collections.Generic;

#nullable disable

namespace Microblink.Library.Services.Models
{
    public partial class Rent : IEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int BookId { get; set; }
        public DateTime RentDate { get; set; }
        public DateTime ReturnDate { get; set; }

        public virtual BookDto Book { get; set; }
        public virtual User User { get; set; }
    }
}
