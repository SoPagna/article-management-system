using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using ArticleManagementSystem.Models;
using ArticleManagementSystem.Repositories;

namespace ArticleManagementSystem.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class ArticlesController : ControllerBase
    {
        private readonly IArticleRepository articleRepository;

        public ArticlesController(IArticleRepository articleRepository)
        {
            this.articleRepository = articleRepository;
        }

        // GET: api/articles
        [HttpGet]
        public IEnumerable<Article> Get([FromQuery(Name = "startDate")] string startDate, [FromQuery(Name = "endDate")] string endDate)
        {
            List<Article> articles;

            if (DateTime.TryParse(startDate, out DateTime startDateTime) && DateTime.TryParse(endDate, out DateTime endDateTime))
            {
                articles = this.articleRepository.GetArticles(startDateTime, endDateTime);
            }
            else
            {
                articles = this.articleRepository.GetArticles();
            }

            return articles;
        }

        // GET api/articles/5
        [HttpGet("{id}")]
        public Article Get(int id)
        {
            return this.articleRepository.GetArticle(id);
        }
    }
}
