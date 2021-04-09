using System;
using System.Collections.Generic;
using System.Text;

namespace Microblink.Library.Services.Models.Interfaces
{
    /// <summary>
    /// Model wrapper
    /// <para>Holds info about successful actions and error message</para>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IModelContainer<T>
    {
        /// <summary>
        /// Entity model
        /// </summary>
        T Model { get; set; }        
        
        /// <summary>
        /// True if action was entity creation, otherwise False.
        /// </summary>
        bool IsCreateAction { get; }

        /// <summary>
        /// True if action was entity reading, otherwise False.
        /// </summary>
        bool IsReadAction { get; }

        /// <summary>
        /// True if action was entity update, otherwise False.
        /// </summary>
        bool IsUpdateAction { get; }

        /// <summary>
        /// True if action was entity deletion, otherwise False.
        /// </summary>
        bool IsDeleteAction { get; }

        /// <summary>
        /// Response code which client should return to describe response. 
        /// ApiResponseStatusCode can be casted into <see cref="Microsoft.AspNetCore.Http.StatusCodes"/>
        /// </summary>
        ResponseStatusCode ResponseStatusCode { get; }

        /// <summary>
        /// Indicates that ApiResponseStatusCode is successfull status code
        /// </summary>
        bool IsSuccessful { get; }
        ModelContainerErrorMessages ErrorMessage { get; set; }

    }
}
