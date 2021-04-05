using Mapper;
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
    public class BooksController : ControllerBase
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
            return Ok(result.Map<IBookTitle, BookTitle>());
        }

        /// <summary>
        /// Rents book title
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("Rent")]
        public async Task<ActionResult<List<Rent>>> Rent(int bookTitleId, int userId, int durationInDays)
        {
            var result = await _dataContext.RentBookTitle(bookTitleId, userId, durationInDays);
            if (result.IsSuccessful)
            {
                return Ok(result.Model.Map<IRent, Rent>());
            }
            else
            {
                return NotFound(result.ErrorMessage.ToString());
            }
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
            if (result.IsSuccessful)
            {
                return Ok(result.Model.Map<IRent, Rent>());
            }
            else
            {
                return NotFound(result.ErrorMessage.ToString());
            }
        }




    }
}
