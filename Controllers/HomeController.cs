using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using BlogHost.Data.Interfaces;
using BlogHost.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BlogHost.Controllers
{

    public class HomeController : Controller
    {
        UserManager<User> _userManager;
        private readonly IUser _user;
        private readonly IPublication _publication;
        private readonly ITopic _topic;

        public HomeController(UserManager<User> userManager, IUser iUser, IPublication iPublication, ITopic iTopic)
        {
            _user = iUser;
            _publication = iPublication;
            _topic = iTopic;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View(_publication.AllPost().Reverse().Take(3));
        }

        public IActionResult About()
        {
            return View();
        }
    }
}
