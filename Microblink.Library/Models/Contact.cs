using Microblink.Library.Services.Models.Interfaces;
using System;
using System.Collections.Generic;

#nullable disable

namespace Microblink.Library.Models
{
    public partial class Contact : IEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ContactTypeId { get; set; }
        public string Value { get; set; }

        public virtual ContactType ContactType { get; set; }
        
        public virtual User User { get; set; }
    }
}
