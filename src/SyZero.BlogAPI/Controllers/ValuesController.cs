using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using SyZero.Domain.Interface;
using SyZero.Domain.Model;
using SyZero.Infrastructure.Mongo;

namespace SyZero.BlogAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
       // private IMongoContext<UserMo> _context;

      private IMongoRepository<UserMo> _repository;
        public ValuesController( IMongoRepository<UserMo> repository)
        {
           _repository = repository;
          //  _context = context;
        }
        // GET api/values
        [HttpGet]
        public IEnumerable<UserMo> Get()
        {
         return   _repository.GetList();

        }

        /// <summary>
        /// 这是一个带参数的get请求
        /// </summary>
        /// <remarks>
        /// 例子:
        /// Get api/Values/1
        /// </remarks>
        /// <param name="id">主键</param>
        /// <returns>测试字符串</returns> 
        /// <response code="201">返回value字符串</response>
        /// <response code="400">如果id为空</response>  
        [HttpGet("{id}")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public ActionResult<string> Get(int id)
        {
            return "这是id=" + id;
        }

        // POST api/values
        [HttpPost]
        public UserMo  Post([FromBody] string value)
        {
            return _repository.Add(new UserMo()
                { Name = "sssss" + value, Paw = "sssaasdasd", State = 1, Utype = 1 });
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
