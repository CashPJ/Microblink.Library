using System;
using System.Collections.Generic;
using System.Text;

namespace Microblink.Library.Services.Models.Interfaces
{
    public interface IUser : IEntity
    {        
        DateTime? DateOfBirth { get; set; }
        string FirstName { get; set; }        
        string LastName { get; set; }
    }
}
