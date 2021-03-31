using Microblink.Library.Services.Models.Interfaces;
using System;
using System.Collections.Generic;

#nullable disable

namespace Microblink.Library.Services.Models
{
    public partial class ContactType : IContactType
    {
        
        public int Id { get; set; }
        public string Name { get; set; }
        public int SortOrder { get; set; }

    }
}
