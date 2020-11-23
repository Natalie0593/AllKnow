using System.Threading.Tasks;
using BlogHost.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Entities;
using Interfaces;
using Services;
using ViewModels;

namespace BlogHost.Controllers
{
    public class CommentController : Controller
    {
        UserManager<User> _userManager;
        private readonly IUser _user;
        // private readonly IPublication _publication;
        IPublicationService _publicationService;
       // private readonly ITopic _topic;
        ITopicService _topicService;
        //private readonly IComment _comment;
        ICommentSevice _commentService;

        

        public CommentController( UserManager<User> userManager, IUser iUser, IPublicationService iPublication, ITopicService iTopic, ICommentSevice commentService)
        {
            
            _user = iUser;
            _publicationService = iPublication;
            _topicService = iTopic;
            _userManager = userManager;
            _commentService = commentService;
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
            var login = _user.GetUserDB(_userManager.GetUserId(User)).UserName;

            var comment = _commentService.CreateComment(model, ID, login);
            //Comment comment = new Comment
            //{
            //    CommentText = model.CommentText,
            //    LoginUser =,
            //    PublicationID = ID,
            //    Publication = _publication.GetPostDB(ID)
            //};

            //_comment.AddCommentDB(comment);
            if (comment != null)
                return RedirectToAction("Post", "Publication", new { ID });

            return View(comment);
        }

        [HttpGet]
        public IActionResult AllComments(int id)
        {
            var comments = _commentService.AllComments(id);
            //var count =  _likeService.GetAllLikes( Likes.Where(userLike => userLike.UserId == ViewBag.UserId).Count();
            //return RedirectToAction("Post", "Publication", new { ID });
            return View(comments);
        }

        //кол-во лайков
      
        //public void Hello(int id, int postId) // передаем id комента
        //{
        //    var com = _commentService.AllComments(postId); //айди поста
        //    var likes = _likeService.GetAllLikes(id);
        //    int count = 0;
        //    foreach(var c in com)
        //    {
        //        foreach (var item in likes)
        //        {
        //            count += 1;
        //        }
        //        c.polelike = count;

        //    }                 

        //}

        [HttpGet]
        public async Task<IActionResult> EditComment(int id)
        {
            Comment comment = _commentService.GetCommentDB(id);
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
                Comment comment = _commentService.GetCommentDB(id);
                if (comment != null)
                {
                    //comment.CommentText = model.CommentText;
                    //_comment.UpdateComment(comment);

                    comment = _commentService.EditComment(model,comment);
                    return RedirectToAction("AllComments");
                }
            }
            return View(model);
        }

        public async Task<ActionResult> Delete(int id)
        {
            Comment comment = _commentService.GetCommentDB(id);
            if (comment != null)
            {
                _commentService.DeleteComment(comment);
            }
            return RedirectToAction("AllComments");
        }
    }
}