
using Microblink.Library.Services.Models.Dto.Interfaces;
using System;

namespace Microblink.Library.Api.Models
{
    /// <inheritdoc/>
    public class User : IUser
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public bool? IsValidMrz { get; set; }
    }
}
