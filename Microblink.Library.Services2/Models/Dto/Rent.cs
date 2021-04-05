using Microblink.Library.Services.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace Microblink.Library.Services.Models.Dto
{
    [Table("Rents")]
    public partial class Rent : IRent
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int BookId { get; set; }
        public DateTime RentDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public DateTime DueDate { get; set; }
        public int? BookTitleId { get; set; }
        public virtual Book Book { get; set; }
        public virtual BookTitle BookTitle { get; set; }
        public virtual User User { get; set; }
    }
}
