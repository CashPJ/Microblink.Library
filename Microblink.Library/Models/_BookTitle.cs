using Microblink.Library.Services.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microblink.Library.Api.Models
{
    public class _BookTitle : IBookTitle
    {
        public string Isbn { get; set; }
        public string Author { get; set; }
        public string Title { get; set; }
        public int Id { get; set; }

        public int TotalCount { get; }
    }
}
