using Mapper;
using Microblink.Library.Api.Models;
using Microblink.Library.Services.Interfaces;
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
    public class BooksController : ControllerBase
    {
        private readonly ILogger<BooksController> _logger;
        private readonly IDatabaseService _databaseService;

        public BooksController(ILogger<BooksController> logger, IDatabaseService databaseService)
        {
            _logger = logger;
            _databaseService = databaseService;
        }

        [HttpGet]
        [Route("books")]
        public IEnumerable<_BookTitle> Books()
        {
            return _databaseService.GetBookTitles().Map<IBookTitle, _BookTitle>();
        }
    }
}
