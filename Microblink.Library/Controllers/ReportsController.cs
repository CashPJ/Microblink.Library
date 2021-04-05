
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
    /// Reports
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class ReportsController : ControllerBase
    {
        private readonly ILogger<ReportsController> _logger;
        private readonly IDataContext _dataContext;

        /// <summary>
        /// Reports
        /// </summary>
        public ReportsController(ILogger<ReportsController> logger, IDataContext dataContext)
        {
            _logger = logger;
            _dataContext = dataContext;
        }

        /// <summary>
        /// Rent history report
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("rentHistory")]
        public async Task<ActionResult<List<ReportRentHistory>>> RentHistory()
        {
            var result = await _dataContext.GetRentHistoryReport();
            return Ok(result.Map<IReportRentHistory, ReportRentHistory>());
        }

        /// <summary>
        /// Top overdue users
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("topOverdueUsers")]
        public async Task<ActionResult<List<ReportTopOverdue>>> TopOverdueUsers()
        {
            var result = await _dataContext.GetTopOverdueReport();
            return Ok(result.Map<IReportTopOverdue, ReportTopOverdue>());
        }
    }
    
}
