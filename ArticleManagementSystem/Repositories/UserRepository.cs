using System.Linq;
using ArticleManagementSystem.Models;
using ArticleManagementSystem.Data;

namespace ArticleManagementSystem.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ArticleManagementDbContext _context;

        public UserRepository(ArticleManagementDbContext context)
        {
            this._context = context;
        }

        public User GetUser(string email)
        {
            return _context.Users.Where(user => user.Email.Equals(email)).FirstOrDefault();
        }

        public User Create(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();

            return user;
        }
    }
}
