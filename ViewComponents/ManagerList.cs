using BlogHost.Data;
using BlogHost.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogHost.ViewComponents
{
    public class ManagerList : ViewComponent
    {
        AppDBContext _appDbContext;

        public ManagerList(AppDBContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public IViewComponentResult Invoke()
        {
            List<Manager> managers = new List<Manager>();
            foreach(var id in _appDbContext.UserRoles.Select(x => x.UserId))
            {
                var el = _appDbContext.Users.FirstOrDefault(x => x.Id == id);
                managers.Add(new Manager {Email = el.Email, Role =
                    _appDbContext.Roles.FirstOrDefault(
                        x => x.Id == _appDbContext.UserRoles.FirstOrDefault(
                            x => x.UserId == el.Id).RoleId).Name});
            }

            return View(managers);
        }
    }
}
