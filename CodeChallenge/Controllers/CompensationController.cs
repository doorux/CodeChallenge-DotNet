using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CodeChallenge.Services;
using CodeChallenge.Models;

namespace CodeChallenge.Controllers
{
    [ApiController]
    [Route("api/compensation")]
    public class CompensationController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly ICompensationService _compensationService;

        public CompensationController(ILogger<CompensationController> logger, ICompensationService compensationService)
        {
            _logger = logger;
            _compensationService = compensationService;
        }

        #region Compensation

        [HttpGet("{employeeId}", Name = "getEmployeeCompensation")]
        public IActionResult GetEmployeeCompensation(String employeeId)
        {
            _logger.LogDebug($"Recieved compensation request for '{employeeId}'");

            var compensation = _compensationService.GetByEmployeeId(employeeId);

            if (compensation == null)
                return NotFound();

            return Ok(compensation);

        }

        [HttpPost(Name = "createEmployeeCompensation")]
        public IActionResult CreateEmployeeCompensation([FromBody] CompensationRequest compensationReq)
        {
            _logger.LogDebug($"Recieved employee compensation create request for '{compensationReq.EmployeeID}'");

            if(compensationReq == null)
            {
                return BadRequest();
            }

            var compensation = _compensationService.Create(compensationReq);

            return CreatedAtRoute("createEmployeeCompensation", new { id = compensation.EmployeeID }, compensation);
        }
        
        #endregion
    }
}
