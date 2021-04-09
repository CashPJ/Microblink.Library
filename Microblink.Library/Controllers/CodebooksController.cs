using Mapper;
using Microblink.Library.Api.Core;
using Microblink.Library.Api.Models;
using Microblink.Library.Services.Context.Interfaces;
using Microblink.Library.Services.Models.Dto.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microblink.Library.Controllers
{

    /// <summary>
    /// Codebooks
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class CodebooksController : ApiControllerBase
    {
        private readonly ILogger<CodebooksController> _logger;
        private readonly IDataContext _dataContext;

        public CodebooksController(ILogger<CodebooksController> logger, IDataContext dataContext)
        {
            _logger = logger;
            _dataContext = dataContext;
        }

        /// <summary>
        /// Contact types
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("contactTypes")]
        public async Task<ActionResult<List<ContactType>>> ContactTypes()
        {
            var result = await _dataContext.GetContactTypes();
            return ApiActionResult<IContactType, ContactType>(result);
        }
    }
    
}
