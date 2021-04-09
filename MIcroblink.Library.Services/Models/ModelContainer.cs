using Microblink.Library.Services.Models.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Microblink.Library.Services.Models
{
    public enum ModelContainerErrorMessages { None, Currently_Unavailable, Already_Returned, Non_Existing_Entity, Invalid_Input, Missing_One_Or_More_Obligatory_Fields, Unprocessable_Entity }
    public enum ActionType { Create, Read, Update, Delete }

    /// <summary>
    /// Defines response status codes on data layer.
    /// All response codes are fully compactible with <see cref="StatusCodes"/>
    /// </summary>
    public enum ResponseStatusCode {
        /// <summary>
        /// It indicates that the REST API successfully carried out whatever action the client requested and that no more specific code in the 2xx series is appropriate.
        /// Unlike the 204 status code, a 200 response should include a response body.
        /// </summary>
        Ok = StatusCodes.Status200OK,
        /// <summary>
        /// A REST API responds with the 201 status code whenever a resource is created inside a collection. There may also be times when a new resource is created as a result of some controller action, in which case 201 would also be an appropriate response.
        /// The newly created resource can be referenced by the URI(s) returned in the entity of the response, with the most specific URI for the resource given by a Location header field.
        /// The origin server MUST create the resource before returning the 201 status code. If the action cannot be carried out immediately, the server SHOULD respond with a 202 (Accepted) response instead.
        /// </summary>
        Created = StatusCodes.Status201Created,
        /// <summary>
        /// A 202 response is typically used for actions that take a long while to process. It indicates that the request has been accepted for processing, but the processing has not been completed. The request might or might not be eventually acted upon, or even maybe disallowed when processing occurs.
        /// Its purpose is to allow a server to accept a request for some other process (perhaps a batch-oriented process that is only run once per day) without requiring that the user agent’s connection to the server persist until the process is completed.
        /// The entity returned with this response SHOULD include an indication of the request’s current status and either a pointer to a status monitor (job queue location) or some estimate of when the user can expect the request to be fulfilled.
        /// </summary>
        Accepted = StatusCodes.Status202Accepted,
        /// <summary>
        /// The 204 status code is usually sent out in response to a PUT, POST, or DELETE request when the REST API declines to send back any status message or representation in the response message’s body.
        /// </summary>
        NoContent = StatusCodes.Status204NoContent,
        /// <summary>
        /// It indicates that the server understands the content type of the request entity, and the syntax of the request entity is correct, but it was unable to process the contained instructions.
        /// </summary>
        UnprocessableEntity = StatusCodes.Status422UnprocessableEntity,        
        /// <summary>
        /// The 404 error status code indicates that the REST API can’t map the client’s URI to a resource but may be available in the future. Subsequent requests by the client are permissible.
        /// </summary>
        NotFound = StatusCodes.Status404NotFound,
        /// <summary>
        /// 400 is the generic client-side error status, used when no other 4xx error code is appropriate. Errors can be like malformed request syntax, invalid request message parameters, or deceptive request routing etc.
        ///The client SHOULD NOT repeat the request without modifications.
        /// </summary>
        BadRequest = StatusCodes.Status400BadRequest
    }

    /// <inheritdoc/>    
    public class ModelContainer<T> : IModelContainer<T>
    {
        /// <inheritdoc/>    
        public T Model { get; set; }
        /// <inheritdoc/>    
        public bool IsCreateAction { get; private set; }
        /// <inheritdoc/>    
        public bool IsReadAction { get; private set; }
        /// <inheritdoc/>    
        public bool IsUpdateAction { get; private set; }
        /// <inheritdoc/>    
        public bool IsDeleteAction { get; private set; }
        /// <inheritdoc/>    
        public ResponseStatusCode ResponseStatusCode { get; private set; }
        /// <inheritdoc/>    
        public ModelContainerErrorMessages ErrorMessage { get; set; } = ModelContainerErrorMessages.None;
        /// <inheritdoc/>    
        public bool IsSuccessful => 
            ResponseStatusCode != ResponseStatusCode.UnprocessableEntity 
            && ResponseStatusCode != ResponseStatusCode.BadRequest 
            && ResponseStatusCode != ResponseStatusCode.NotFound;

        public ModelContainer(ActionType actionType, ResponseStatusCode responseCode)
        {
            ResponseStatusCode = responseCode;
            TransformActionData(actionType);            
        }

        public ModelContainer(T model, ActionType actionType, ResponseStatusCode responseCode, bool isSuccessful = true, ModelContainerErrorMessages errorMessage = ModelContainerErrorMessages.None)
        {
            Model = model;
            ErrorMessage = errorMessage;
            ResponseStatusCode = responseCode;
            TransformActionData(actionType);
            
        }
        
        private void TransformActionData(ActionType actionType)
        {
            switch (actionType)
            {
                case ActionType.Create: IsCreateAction = true; break;
                case ActionType.Read: IsReadAction = true; break;
                case ActionType.Update: IsUpdateAction = true; break;
                case ActionType.Delete: IsDeleteAction = true; break;
            }
        }
    }
}
