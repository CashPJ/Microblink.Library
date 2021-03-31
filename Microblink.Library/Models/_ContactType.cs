using Microblink.Library.Services.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microblink.Library.Api.Models
{
    public class _ContactType : IContactType
    {
        public string Name { get; set; }
        public int SortOrder { get; set; }
        public int Id { get; set; }
    }
}
