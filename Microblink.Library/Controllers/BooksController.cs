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
    /// Books, books, books... And some more books...
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class BooksController : ApiControllerBase
    {
        private readonly ILogger<BooksController> _logger;
        private readonly IDataContext _dataContext;

        /// <summary>
        /// Books, books, books... And some more books...
        /// </summary>
        public BooksController(ILogger<BooksController> logger, IDataContext dataContext)
        {
            _logger = logger;
            _dataContext = dataContext;
        }

        /// <summary>
        /// Returns all book titles
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        public async Task<ActionResult<List<BookTitle>>> Titles()
        {            
            var result = await _dataContext.GetBookTitles();
            return ApiActionResult<IBookTitle, BookTitle>(result);
        }

        /// <summary>
        /// Creates new rent of the book title and returns newly created <see cref="Api.Models.Rent"/> entity.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("Rent")]
        public async Task<ActionResult<List<Rent>>> Rent(int bookTitleId, int userId, int durationInDays)
        {
            var result = await _dataContext.RentBookTitle(bookTitleId, userId, durationInDays);
            return ApiActionResult<IRent, Rent>(result);
        }

        /// <summary>
        /// Returns rented book
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [Route("Return")]
        public async Task<ActionResult<Rent>> Return(int rentId, int bookId, int userId)
        {
            var result = await _dataContext.ReturnRentedBookTitle(rentId, bookId, userId);
            return ApiActionResult<IRent, Rent>(result);            
        }
    }
}
