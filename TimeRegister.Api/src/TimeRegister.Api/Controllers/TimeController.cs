using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TimeRegister.Domain.AppServices;
using TimeRegister.Domain.Dtos;
using TimeRegister.Domain.Entities;

namespace TimeRegister.Api.Controllers
{
    [Route("v1/[controller]")]
    public class TimeController : Controller
    {
        private readonly TimeAppService _app;

        public TimeController(TimeAppService app)
        {
            _app = app;
        }

        [HttpGet("{from}/{to}")]
        public async Task<IActionResult> Get(DateTime from, DateTime to)
        {
            var result = await _app.Get(from, to);
            return Json(result);
        }

        [HttpGet("{from}/{to}/csv")]
        public async Task<IActionResult> GetCsv(DateTime from, DateTime to)
        {
            var result = await _app.GetCsv(from, to);
            var reportName = "Report from ";
            reportName += from.ToString("yyyy-MM-dd");
            reportName += " to ";
            reportName += to.ToString("yyyy-MM-dd");
            reportName += ".csv";

            return File(new System.Text.UTF8Encoding().GetBytes(result), "text/csv", reportName);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] TimeKeepingBatch model)
        {
            await _app.CreateRegister(model);
            return Ok();
        }
    }
}
