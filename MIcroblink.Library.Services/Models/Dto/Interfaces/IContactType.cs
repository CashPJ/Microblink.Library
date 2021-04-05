using System;

namespace Microblink.Library.Services.Models.Dto.Interfaces
{
    /// <summary>
    /// Contact type
    /// </summary>
    public interface IContactType
    {  
        int Id { get; set; }
        string Name { get; set; }
        int SortOrder { get; set; }        
    }
}
