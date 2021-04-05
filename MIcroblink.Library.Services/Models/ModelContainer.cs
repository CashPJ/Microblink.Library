using Microblink.Library.Services.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Microblink.Library.Services.Models
{
    public enum ModelContainerErrorMessages { None, Currently_Unavailable, Already_Returned, Non_Existing_Entity, Invalid_Input, Missing_One_Or_More_Obligatory_Fields, Unprocessable_Entity }

    /// <summary>
    /// Model wrapper
    /// <para>Holds info about successful actions and error message</para>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ModelContainer<T> : IModelContainer<T>
    {
        public ModelContainer()
        {
        }
        public ModelContainer(T model, bool isSuccessful = true, ModelContainerErrorMessages errorMessage = ModelContainerErrorMessages.None)
        {
            Model = model;
            IsSuccessful = isSuccessful;
            ErrorMessage = errorMessage;
        }

        public bool IsSuccessful { get; set; } = false;
        public ModelContainerErrorMessages ErrorMessage { get; set; } = ModelContainerErrorMessages.None;
        public T Model { get; set; }

    }
}
