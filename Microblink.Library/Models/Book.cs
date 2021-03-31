using Microblink.Library.Services.Models.Interfaces;
using System;
using System.Collections.Generic;

#nullable disable

namespace Microblink.Library.Models
{
    public partial class Book : IEntity
    {
        public int Id { get; set; }
        public int IdBookTitle { get; set; }

        public virtual BookTitle BookTitle { get; set; }
    }
}
