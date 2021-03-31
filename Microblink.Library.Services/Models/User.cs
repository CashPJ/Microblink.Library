using Microblink.Library.Services.Models.Interfaces;
using System;
using System.Collections.Generic;

#nullable disable

namespace Microblink.Library.Services.Models
{
    public partial class  User : IEntity
    {
        public User()
        {
            Contacts = new HashSet<Contact>();
            Rents = new HashSet<Rent>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }

        public virtual ICollection<Contact> Contacts { get; set; }
        public virtual ICollection<Rent> Rents { get; set; }
    }
}
