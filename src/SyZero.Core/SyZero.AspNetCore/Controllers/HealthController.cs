
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using SyZero.Serialization;

namespace SyZero.AspNetCore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HealthController : SyZeroController
    {
        private readonly IConfiguration _configuration;
        public IJsonSerialize jsonSerialize { get; set; }

        public HealthController(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        [HttpGet]
        public IActionResult Index()
        {
            Console.WriteLine($"Consul健康检查: " + DateTime.Now.ToString());
            return Ok();
        }


    }
}



