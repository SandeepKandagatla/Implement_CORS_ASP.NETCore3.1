using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ImplementCORS.Controllers
{
    [ApiController]
    [Route("[controller]")]

    [EnableCors("PublicApi")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            SetPainationHeaders(10, 1, 100, 1000);

            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        private void SetPainationHeaders(int pagesize,int pageno,int pagecount,int totalrecords)
        {
            HttpContext.Response.Headers.Add("PageNo", pageno.ToString());
            HttpContext.Response.Headers.Add("PageSize", pagesize.ToString());
            HttpContext.Response.Headers.Add("PageCount", pagecount.ToString());
            HttpContext.Response.Headers.Add("PageTotalRecords", totalrecords.ToString());

        }
    }
}
