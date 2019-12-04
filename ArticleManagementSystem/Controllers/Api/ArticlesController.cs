using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ArticleManagementSystem.Data;
using ArticleManagementSystem.Models;

namespace ArticleManagementSystem.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class ArticlesController : ControllerBase
    {
        private ArticleManagementDbContext _context;

        public ArticlesController(ArticleManagementDbContext context)
        {
            this._context = context;
        }

        // GET: api/articles
        [HttpGet]
        public IEnumerable<Article> Get([FromQuery(Name = "startDate")] string startDate, [FromQuery(Name = "endDate")] string endDate)
        {
            var articles = from article in _context.Articles select article;
            DateTime startDateTime, endDateTime;

            if (DateTime.TryParse(startDate, out startDateTime) && DateTime.TryParse(endDate, out endDateTime))
            {
                articles = articles.Where(article => article.Date >= startDateTime && article.Date <= endDateTime);
            }

            return articles.ToList();
        }

        // GET api/articles/5
        [HttpGet("{id}")]
        public Article Get(int id)
        {
            return _context.Articles.Find(id);
        }

        // POST api/articles
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/articles/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/articles/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
