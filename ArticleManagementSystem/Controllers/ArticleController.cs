using Microsoft.AspNetCore.Mvc;
using ArticleManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using ArticleManagementSystem.Repositories;
using ArticleManagementSystem.Services;

namespace ArticleManagementSystem.Controllers
{
    [Authorize]
    public class ArticleController : Controller
    {
        private readonly IArticleRepository articleRepository;
        private readonly UserService userService;

        public ArticleController(IArticleRepository articleRepository, UserService userService)
        {
            this.articleRepository = articleRepository;
            this.userService = userService;
        }

        [HttpGet]
        public IActionResult Index([FromQuery(Name = "search")] string searchTerm, [FromQuery(Name = "sortTitleBy")] string sortTitleBy)
        {
            return View(this.articleRepository.GetArticles(searchTerm, this.userService.GetAuthUserID(), sortTitleBy));
        }

        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        public IActionResult Create([Bind("Title", "Description", "Date")] Article article)
        {
            if (ModelState.IsValid)
            {
                article.CreatedBy = this.userService.GetAuthUserID();
                this.articleRepository.Create(article);

                return RedirectToAction(nameof(Index));
            }

            return View(article);
        }

        [HttpGet]
        public IActionResult Edit(int ID)
        {
            Article article = this.articleRepository.GetArticle(ID, this.userService.GetAuthUserID());

            if (article == null)
            {
                return NotFound();
            }

            return View(article);
        }

        [HttpPost]
        public IActionResult Edit(int ID, [Bind("ID", "Title", "Description", "Date")] Article article)
        {
            if (ID != article.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                article.CreatedBy = this.userService.GetAuthUserID();
                this.articleRepository.Update(article);

                return RedirectToAction(nameof(Index));
            }

            return View(article);
        }

        [HttpGet]
        public IActionResult Delete(int ID)
        {
            if (this.articleRepository.GetArticle(ID, this.userService.GetAuthUserID()) == null)
            {
                return NotFound();
            }

            this.articleRepository.Delete(ID);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Show(int ID)
        {
            Article article = this.articleRepository.GetArticle(ID);

            if (article == null)
            {
                return NotFound();
            }

            return View(article);
        }
    }
}
