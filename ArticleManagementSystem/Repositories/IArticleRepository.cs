using System;
using System.Collections.Generic;
using ArticleManagementSystem.Models;

namespace ArticleManagementSystem.Repositories
{
    public interface IArticleRepository
    {
        Article GetArticle(int ID);
        Article GetArticle(int ID, int createdBy);
        List<Article> GetArticles();
        List<Article> GetArticles(string title, int createdBy, string sortBy);
        List<Article> GetArticles(DateTime startDateTime, DateTime endDateTime);
        Article Create(Article article);
        Article Update(Article article);
        bool Delete(int ID);
    }
}
