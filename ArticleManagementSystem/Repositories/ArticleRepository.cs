using System;
using System.Collections.Generic;
using System.Linq;
using ArticleManagementSystem.Models;
using ArticleManagementSystem.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ArticleManagementSystem.Services;

namespace ArticleManagementSystem.Repositories
{
    public class ArticleRepository : IArticleRepository
    {
        private readonly ArticleManagementDbContext _context;

        public ArticleRepository(ArticleManagementDbContext context)
        {
            this._context = context;
        }

        public Article GetArticle(int ID)
        {
            return _context.Articles.Find(ID);
        }

        public Article GetArticle(int ID, int createdBy)
        {
            return _context.Articles.Where(article => article.ID == ID && article.CreatedBy == createdBy).FirstOrDefault();
        }

        public List<Article> GetArticles()
        {
            return _context.Articles.ToList();
        }

        public List<Article> GetArticles(string title, int createdBy, string sortBy = "asc")
        {
            IQueryable<Article> articles = from item in _context.Articles where item.CreatedBy == createdBy select item;

            if (!String.IsNullOrEmpty(title))
            {
                articles = articles.Where(article => article.Title.Contains(title));
            }

            if (sortBy == "desc")
            {
                articles = articles.OrderByDescending(article => article.Title);
            }
            else
            {
                articles = articles.OrderBy(article => article.Title);
            }

            return articles.ToList();
        }

        public List<Article> GetArticles(DateTime startDateTime, DateTime endDateTime)
        {
            return _context.Articles.Where(article => article.Date >= startDateTime && article.Date <= endDateTime).ToList();
        }

        public Article Create(Article article)
        {
            _context.Articles.Add(article);
            _context.SaveChanges();

            return article;
        }

        public Article Update(Article article)
        {
            try
            {
                _context.Update(article);
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return null;
            }

            return article;
        }

        public bool Delete(int ID)
        {
            Article article = this.GetArticle(ID);

            if (article != null)
            {
                _context.Remove(article);
                _context.SaveChanges();

                return true;
            }

            return false;
        }
    }
}
