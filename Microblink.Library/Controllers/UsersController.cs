using Mapper;
using Microblink.Library.Api.Models;
using Microblink.Library.Api.Models.Request;
using Microblink.Library.Services.Context.Interfaces;
using Microblink.Library.Services.Models.Dto.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microblink.Library.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        private readonly IDataContext _dataContext;

        public UsersController(ILogger<UsersController> logger, IDataContext dataContext)
        {
            _logger = logger;
            _dataContext = dataContext;
        }

        /// <summary>
        /// Returns all users
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        public async Task<ActionResult<List<User>>> Get()
        {
            var result = await _dataContext.GetUsers();
            return Ok(result.Map<IUser, User>());
        }

        /// <summary>
        /// Returns specific users
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("User")]
        public async Task<ActionResult<User>> Get(int userId)
        {
            var result = await _dataContext.GetUser(userId);

            if (result.IsSuccessful)
            {
                return Ok(result.Model.Map<IUser, User>());
            }
            else
            {
                return NotFound(result.ErrorMessage.ToString());
            }
        }

        /// <summary>
        /// Creates user
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="dateOfBirth"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("User/Create")]
        public async Task<ActionResult<User>> Create(string firstName, string lastName, DateTime dateOfBirth)
        {
            var result = await _dataContext.CreateUser(firstName, lastName, dateOfBirth, null);
            return Ok(result.Map<IUser, User>());            
        }

        /// <summary>
        /// Creates user by OCRing ID card
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("User/CreateByIdCard")]
        public async Task<ActionResult<User>> CreateUserByIdCard(CreateUserByIdCardRequestModel model)
        {
            if (ModelState.IsValid)
            {
                var ocrResult = await _dataContext.OcrData(model.IdCardBase64Image);

                if (ocrResult.IsSuccessful)
                {
                    var result = await _dataContext.CreateUser(ocrResult.Model.FirstName, ocrResult.Model.LastName, ocrResult.Model.DateOfBirth, ocrResult.Model.IsValidMrz);
                    return Ok(result.Map<IUser, User>());
                }
                else
                {
                    return UnprocessableEntity(ocrResult.ErrorMessage.ToString());
                }
            }
            else
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Updates specific user
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="dateOfBirth"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("User/Update")]
        public async Task<ActionResult<User>> Update(int userId, string firstName, string lastName, DateTime dateOfBirth)
        {
            var result = await _dataContext.UpdateUser(userId, firstName, lastName, dateOfBirth);

            if (result.IsSuccessful)
            {
                return Ok(result.Model.Map<IUser, User>());
            }
            else
            {
                return NotFound(result.ErrorMessage.ToString());
            }
        }

        /// <summary>
        /// Deletes specific user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("User/Delete")]
        public async Task<ActionResult> Delete(int userId)
        {
            var result = await _dataContext.DeleteUser(userId);

            if (result.IsSuccessful)
            {
                return Ok();
            }
            else
            {
                return NotFound(result.ErrorMessage.ToString());
            }
        }

        /// <summary>
        /// Gets contacts for specific user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("User/Contacts")]
        public async Task<ActionResult<List<Contact>>> Contacts(int userId)
        {
            var result = await _dataContext.GetUserContacts(userId);
            if (result.IsSuccessful)
            {
                return Ok(result.Model.Map<IContact, Contact>());
            }
            else
            {
                return NotFound(result.ErrorMessage.ToString());
            }
        }

        /// <summary>
        /// Creates users contact
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="contactTypeId"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("User/Contact/Create")]
        public async Task<ActionResult<List<Contact>>> CreateContact(int userId, int contactTypeId, string value)
        {
            var result = await _dataContext.CreateContact(userId, contactTypeId, value);
            return Ok(result.Map<IContact, Contact>());
        }

        /// <summary>
        /// Deletes users contact
        /// </summary>
        /// <param name="contactId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("User/Contact/Delete")]
        public async Task<ActionResult> DeleteContact(int contactId, int userId)
        {
            var result = await _dataContext.DeleteContact(contactId, userId);
            if (result.IsSuccessful)
            {
                return Ok();
            }
            else
            {
                return NotFound(result.ErrorMessage.ToString());
            }            
        }
    }
    
}
