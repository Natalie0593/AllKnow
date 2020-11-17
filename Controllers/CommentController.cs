using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogHost.Data.Interfaces;
using BlogHost.ViewModels;
using BlogHost.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BlogHost.Controllers
{
    public class CommentController : Controller
    {
        UserManager<User> _userManager;
        private readonly IUser _user;
        private readonly IPublication _publication;
        private readonly ITopic _topic;
        private readonly IComment _comment;

        public CommentController(UserManager<User> userManager, IUser iUser, IPublication iPublication, ITopic iTopic, IComment iComment)
        {
            _user = iUser;
            _publication = iPublication;
            _topic = iTopic;
            _userManager = userManager;
            _comment = iComment;
        }

        public static int ID;

        public IActionResult CreateComment(int id)
        {
            ID = id;
            return View();
        }

        [HttpPost]
        public IActionResult CreateComment(CreateCommentViewModel model)
        {
            Comment comment = new Comment
            {
                CommentText = model.CommentText,
                LoginUser = _user.GetUserDB(_userManager.GetUserId(User)).UserName,
                PublicationID = ID,
                Publication = _publication.GetPostDB(ID)
            };

            _comment.AddCommentDB(comment);
            return RedirectToAction("Post","Publication", new { ID });
        }

        [HttpGet]
        public IActionResult AllComments()
        {
            return View(_comment.AllComments());
        }

        [HttpGet]
        public async Task<IActionResult> EditComment(int id)
        {
            Comment comment = _comment.GetCommentDB(id);
            if (comment == null)
            {
                return NotFound();
            }
            CreateCommentViewModel model = new CreateCommentViewModel
            {
                CommentText = comment.CommentText
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditComment(CreateCommentViewModel model, int id)
        {
            if (ModelState.IsValid)
            {
                Comment comment = _comment.GetCommentDB(id);
                if (comment != null)
                {
                    comment.CommentText = model.CommentText;

                    _comment.UpdateComment(comment);
                    return RedirectToAction("AllComments");
                }
            }
            return View(model);
        }

        public async Task<ActionResult> Delete(int id)
        {
            Comment comment = _comment.GetCommentDB(id);
            if (comment != null)
            {
                _comment.DeleteComment(comment);
            }
            return RedirectToAction("AllComments");
        }
    }
}