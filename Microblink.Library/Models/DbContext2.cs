using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microblink.Library.Api.Models
{
    public class DbContext2 : DbContext
    {
        public  DbSet<Book> Books { get; set; }
        public  DbSet<BookTitle> BookTitles { get; set; }
        public  DbSet<Contact> Contacts { get; set; }
        public  DbSet<ContactTypeDto> ContactTypes { get; set; }
        public  DbSet<Rent> Rents { get; set; }
        public  DbSet<User> Users { get; set; }

    }
}
