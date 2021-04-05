using System;
using System.Collections.Generic;
using System.Text;

namespace Microblink.Library.Services.Models.Interfaces
{
    public interface IModelContainer<T>
    {
        bool IsSuccessful { get; set; }
        ModelContainerErrorMessages ErrorMessage { get; set; }
        T Model { get; set; }
    }
}
