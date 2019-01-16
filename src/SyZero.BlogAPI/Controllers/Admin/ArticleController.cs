using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SyZero.Infrastructure.EntityFramework;
using SyZero.Domain.Interface;

//using SyZero.BLL;
//using SyZero.BLL.Interfaces;
using SyZero.Domain.Model;
using SyZero.Infrastructure.Repository;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SyZero.BlogAPI.Controllers.Admin
{
    [Route("api/admin/u/[controller]")]
    public class ArticleController : Controller
    {
       private readonly IBaseRepository<Article> _articleRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly SyDbContext _syDbContext;
      //  private readonly IRepository<ArticleEntity> _articleRepository;

        public ArticleController(IBaseRepository<Article> articleRepository, IUnitOfWork unitOfWork, SyDbContext syDbContext)
        {
            this._unitOfWork = unitOfWork;
            this._syDbContext = syDbContext;
            this._articleRepository = articleRepository;
        }

        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<Article> Get()
        {
            for (int i = 0; i < 500; i++)
            {
                Article article = new Article().CreateArticle("122", 2, "22", "22222", "34345dasdasdad");
                _articleRepository.Add(article);
            }
            _unitOfWork.Commit();
            return _articleRepository.GetAll();
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
         
            return "value";
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
        public void Delete(int id)
        {
        }
    }
}
