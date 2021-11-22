
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using SyZero.AspNetCore.Controllers;
using SyZero.Serialization;

namespace SyZero.AspNetCore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class instanceController : SyZeroController
    {
        private readonly IConfiguration _configuration;
        public IJsonSerialize jsonSerialize { get; set; }

        public instanceController(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        [HttpGet("beat")]
        public IActionResult Beat()
        {
            Console.WriteLine($"Nacos健康检查: " + DateTime.Now.ToString());
            return Ok();
        }
    }
}



