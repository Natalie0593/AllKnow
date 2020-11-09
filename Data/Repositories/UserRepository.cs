using BlogHost.Data.Interfaces;
using BlogHost.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogHost.Data.Repositories
{
    public class UserRepository : IUser
    {
        private readonly AppDBContext _appDbContext;

        public UserRepository(AppDBContext appDbContext)
        {
            this._appDbContext = appDbContext;
        }

        public User GetUserDB(string id)
        {
            return _appDbContext.Users.FirstOrDefault(x => x.Id == id);
        }
    }
}
