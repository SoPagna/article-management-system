using System;
using ArticleManagementSystem.Models;

namespace ArticleManagementSystem.Repositories
{
    public interface IUserRepository
    {
        User GetUser(string email);
        User Create(User user);
    }
}
