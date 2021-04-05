using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Microblink.Library.Api.Models.Request
{
    /// <summary>
    /// CreateByIdCard request model
    /// </summary>
    public class CreateUserByIdCardRequestModel
    {
        /// <summary>
        /// Base 64 encoded ID card image
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "IdCardBase64Image is obligatory field")]
        [MinLength(50, ErrorMessage = "Minimum length of IdCardBase64Image is 50")]
        public string IdCardBase64Image { get; set;}
    }
}
