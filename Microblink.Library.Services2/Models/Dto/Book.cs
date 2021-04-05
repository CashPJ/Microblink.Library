using Microblink.Library.Services.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace Microblink.Library.Services.Models.Dto
{
    [Table("Books")]
    public partial class Book : IBook
    {
        public int Id { get; set; }
        public int BookTitleId { get; set; }
        public virtual BookTitle BookTitle { get; set; }
        public virtual ICollection<Rent> Rents { get; set; } = new HashSet<Rent>();
    }
}
