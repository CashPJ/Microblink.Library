using System;

namespace Microblink.Library.Services.Models.Dto.Interfaces
{
    /// <summary>
    /// Users contact
    /// </summary>
    public interface IContact
    {  
        int Id { get; set; }
        int UserId { get; set; }
        int ContactTypeId { get; set; }
        string Value { get; set; }
        
    }
}
