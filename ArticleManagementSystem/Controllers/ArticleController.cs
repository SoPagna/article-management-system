using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ArticleManagementSystem.Data;
using ArticleManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;

namespace ArticleManagementSystem.Controllers
{
    [Authorize]
    public class ArticleController : Controller
    {
        private ArticleManagementDbContext _context;

        public ArticleController(ArticleManagementDbContext context)
        {
            this._context = context;
        }

        [HttpGet]
        public IActionResult Index([FromQuery(Name = "search")] string searchTerm, [FromQuery(Name = "sortTitleBy")] string sortTitleBy)
        {
            int userID = Int32.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var articles = from item in _context.Articles where item.CreatedBy == userID select item;

            // Search by Title
            if (!String.IsNullOrEmpty(searchTerm))
            {
                articles = articles.Where(article => article.Title.Contains(searchTerm));
            }

            // Sort by Title
            if (sortTitleBy == "desc")
            {
                articles = articles.OrderByDescending(article => article.Title);
            }
            else
            {
                articles = articles.OrderBy(article => article.Title);
            }

            return View(articles.ToList());
        }

        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        public IActionResult Create([Bind("Title", "Description", "Date")] Article article)
        {
            if (ModelState.IsValid)
            {
                article.CreatedBy = Int32.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                _context.Articles.Add(article);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            return View(article);
        }

        [HttpGet]
        public IActionResult Edit(int ID)
        {
            Article article = _context.Articles.Find(ID);

            if (article == null || !IsArticleWriter(ID))
            {
                return NotFound();
            }

            return View(article);
        }

        [HttpPost]
        public IActionResult Edit(int ID, [Bind("ID", "Title", "Description", "Date")] Article article)
        {
            if (ID != article.ID || !IsArticleWriter(ID))
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    article.CreatedBy = Int32.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                    _context.Update(article);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArticleExists(article.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Index));
            }

            return View(article);
        }

        [HttpGet]
        public IActionResult Delete(int ID)
        {
            Article article = _context.Articles.Find(ID);

            if (article == null || !IsArticleWriter(ID))
            {
                return NotFound();
            }

            _context.Remove(article);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Show(int ID)
        {
            Article article = _context.Articles.Find(ID);

            if (article == null)
            {
                return NotFound();
            }

            return View(article);
        }

        private bool ArticleExists(int ID) => _context.Articles.Any(article => article.ID == ID);

        private bool IsArticleWriter(int ID)
        {
            int userID = Int32.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            return _context.Articles.Any(article => article.ID == ID && article.CreatedBy == userID);
        }
    }
}
