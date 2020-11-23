using System.Linq;
using System.Threading.Tasks;
using BlogHost.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using Entities;
using Interfaces;
using Services;
using ViewModels;

namespace BlogHost.Controllers
{
    public class PublicationController : Controller
    {
        UserManager<User> _userManager;
        IUserService _userService;
        private readonly IUser _user; //заменить
        //private readonly IPublication _publication;
        IPublicationService _publicationService;
        ITopicService _topicService;
        ICommentSevice _commentSevice;
        IVideoService _videoService;
        
        //private readonly ITopic _topic; //заменить
       
        public PublicationController(IVideoService videoService, ICommentSevice commentSevice, UserManager<User> userManager, IUser iUser, IPublicationService iPublication, ITopicService iTopic)
        {
            _videoService = videoService;
            _commentSevice = commentSevice;
            _user = iUser;
            _publicationService = iPublication;
            _topicService = iTopic;
            _userManager = userManager;
        }

        public IActionResult CreatePost()
        {
            return View();
        }

        public async Task<IActionResult> Post(int id)
        {
            Publication post = _publicationService.GetPostDB(id);
            
            ViewBag.Videos = await _videoService.GetAllVideos(post.Id);
            if (post == null)
            {
                return NotFound();
            }
            return View(@post);
        }

        public IActionResult AllPosts(bool flag)
        {
            if (flag)
            {
                return View(_publicationService.AllPostIsFavorite());
            }
            else
            {
                return View(_publicationService.AllPost());
            }
        }

        [HttpGet]
        public IActionResult ThemsPost(string nameTopic)
        {
            return View(_publicationService.AllThemsPost(_topicService.GetIDTopicDB(nameTopic)));
        }

        [HttpGet]
        public IActionResult MyPosts()
        {
            return View(_publicationService.MyPost(_user.GetUserDB(_userManager.GetUserId(User))));
        }

        [HttpPost]
        public IActionResult CreatePost(CreatePublicationViewModel model)
        {
            if (ModelState.IsValid)
            {
                
                var user = _user.GetUserDB(_userManager.GetUserId(User));
                // считываем переданный файл в массив байтов
                

               // string a = _userManager.GetUserId(User);
                var publ = _publicationService.AddPublicationDB(model, user);
                return RedirectToAction("AllPosts");
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditPost(int id)
        {
            Publication post = _publicationService.GetPostDB(id);
            if (post == null)
            {
                return NotFound();
            }
            EditePostViewModel model = new EditePostViewModel
            {
                Id = post.Id,
                PublicationName = post.PublicationName,
                PublicationText = post.PublicationText,
                Discription = post.Discription,
                TopicName = _topicService.GetTopicName(post.TopicId)
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditPost(EditePostViewModel model)
        {
            if (ModelState.IsValid)
            {
                Publication post = _publicationService.GetPostDB(model.Id);
                if (post != null)
                {
                    post.PublicationName = model.PublicationName;
                    post.PublicationText = model.PublicationText;
                    post.Discription = model.Discription;
                    post.TopicId = _topicService.GetTopicDB(model.TopicName).Id;
                    post.Topic = _topicService.GetTopicDB(model.TopicName);

                    _publicationService.UpdatePost(post);

                    return RedirectToAction("AllPosts");
                }
            }
            return View(model);
        }

        public async Task<IActionResult> Like(int id)
        {
            Publication post = _publicationService.GetLikePost(id);
            return RedirectToAction("Post", new { id = id });
        }


        public async Task<ActionResult> Delete(int id)
        {
            Publication post = _publicationService.GetPostDB(id);
            if (post != null)
            {
                _publicationService.DeletePost(post);
            }
            return RedirectToAction("AllPosts");
        }
    }
}