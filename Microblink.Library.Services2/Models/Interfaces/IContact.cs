using System;
using System.Collections.Generic;
using System.Text;

namespace Microblink.Library.Services.Models.Interfaces
{
    public interface IContact : IEntity
    {
        int UserId { get; set; }
        int ContactTypeId { get; set; }
        string Value { get; set; }
    }
}
