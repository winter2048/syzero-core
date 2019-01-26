using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SyZero.Domain.Interface;
using SyZero.Domain.Model;
using SyZero.Infrastructure.EfRepository;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SyZero.BlogAPI.Controllers.Admin
{
    [Route("api/admin/u/[controller]")]
    public class UserController : Controller
    {
        private IEfRepository<User> _userRep;
        private IUnitOfWork _ofWork;
        public UserController(IEfRepository<User> userRep, IUnitOfWork ofWork)
        {

            _userRep = userRep;
            _ofWork = ofWork;
        }
        // public UserController 
                                                    
        // GET: api/<controller>
        [HttpGet]
        public async Task<IEnumerable<User>> Get()
        {
            return await _userRep.GetListAsync();
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public async Task<string> Get(int id)
        {
            _userRep.Add(new User() {Name = "sssss" + id, Paw = "sssaasdasd", State = 1, Utype = 1});
          
            Task<int> x =  _ofWork.SaveAsyncChange();
            return (await x).ToString();
        }

        // POST api/<controller>
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public string Delete(int id)
        {
            _userRep.Add(new User() { Name = "sssss" + id, Paw = "sssaasdasd", State = 1, Utype = 1 });

            int x = _ofWork.SaveChange();
            return x.ToString();
        }
    }
}
