using System;

namespace Microblink.Library.Services.Models.Dto.Interfaces
{
    /// <summary>
    /// User 
    /// </summary>
    public interface IUser
    {  
        int Id { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        DateTime DateOfBirth { get; set; }
        bool? IsValidMrz { get; set; }
    }
}
