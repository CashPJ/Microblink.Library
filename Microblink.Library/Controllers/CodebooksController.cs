using Mapper;
using Microblink.Library.Api.Models;
using Microblink.Library.Services.Interfaces;
using Microblink.Library.Services.Models;
using Microblink.Library.Services.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microblink.Library.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CodebooksController : ControllerBase
    {
        private readonly ILogger<CodebooksController> _logger;
        private readonly IDatabaseService _databaseService;

        public CodebooksController(ILogger<CodebooksController> logger, IDatabaseService databaseService)
        {
            _logger = logger;
            _databaseService = databaseService;
        }

        [HttpGet]
        [Route("contactTypes")]
        public IEnumerable<_ContactType> Get()
        {
            return _databaseService.GetContactTypeCodebook().Map<IContactType, _ContactType>();
        }
    }
}
