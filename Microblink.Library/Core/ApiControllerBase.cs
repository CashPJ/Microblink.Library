using Mapper;
using Microblink.Library.Api.Models;
using Microblink.Library.Services.Models;
using Microblink.Library.Services.Models.Dto.Interfaces;
using Microblink.Library.Services.Models.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microblink.Library.Api.Core
{
    public class ApiControllerBase : ControllerBase
    {
        /// <summary>
        /// Mapping generic interface I to generic destination type D and returns <see cref="IActionResult"/> acording <see cref="ModelContainer&lt;T&gt;.ResponseStatusCode"/>
        /// </summary>
        /// <typeparam name="I"></typeparam>
        /// <typeparam name="D"></typeparam>
        /// <param name="modelContainer"></param>
        /// <returns></returns>
        public ActionResult ApiActionResult<I, D>(IModelContainer<I> modelContainer) where D : I
        {
            var mappedResponseModel = modelContainer.Model.Map<I, D>();
            return ApiActionResult(mappedResponseModel, modelContainer.ResponseStatusCode, modelContainer.ErrorMessage);

        }

        /// <summary>
        /// Mapps generic interface IEnumerable&lt;I&gt; to generic destination type List&lt;D&gt; and returns appropriate <see cref="IActionResult"/> acording <see cref="ModelContainer&lt;T&gt;.ResponseStatusCode"/>
        /// </summary>
        /// <typeparam name="SourceInterface"></typeparam>
        /// <typeparam name="DestinationType"></typeparam>
        /// <param name="modelContainer"></param>
        /// <returns></returns>
        public ActionResult ApiActionResult<SourceInterface, DestinationType>(IModelContainer<IEnumerable<SourceInterface>> modelContainer) where DestinationType : SourceInterface
        {
            var mappedResponseModel = modelContainer.Model.Map<SourceInterface, DestinationType>();
            return ApiActionResult<IEnumerable<DestinationType>>(mappedResponseModel, modelContainer.ResponseStatusCode, modelContainer.ErrorMessage);
        }


        /// <summary>
        /// Returns generic response model T <see cref="IActionResult"/> acording <see cref="ModelContainer&lt;T&gt;.ResponseStatusCode"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="mappedResponseModel"></param>
        /// <param name="responseStatusCode"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        private ActionResult ApiActionResult<T>(T mappedResponseModel, ResponseStatusCode responseStatusCode, ModelContainerErrorMessages errorMessage)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(JsonConvert.SerializeObject(ModelState.Values.Select(e => e.Errors).ToList()));
            }

            return responseStatusCode switch
            {
                ResponseStatusCode.Ok => Ok((mappedResponseModel)),
                ResponseStatusCode.Created => Created("", mappedResponseModel),//todo: uri
                ResponseStatusCode.Accepted => Accepted(mappedResponseModel),
                ResponseStatusCode.NoContent => NoContent(),
                ResponseStatusCode.UnprocessableEntity => UnprocessableEntity(errorMessage),
                ResponseStatusCode.NotFound => NotFound(errorMessage),
                ResponseStatusCode.BadRequest => BadRequest(errorMessage),
                _ => throw new Exception("Not implemented Response status code."),
            };
        }




    }
}
