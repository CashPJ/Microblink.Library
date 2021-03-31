using System;
using System.Collections.Generic;
using System.Text;

namespace Microblink.Library.Services.Models.Interfaces
{
    public interface IContactType : IEntity
    {        
        string Name { get; set; }
        int SortOrder { get; set; }
    }
}
