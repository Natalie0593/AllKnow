using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogHost.Data.Interfaces;
using BlogHost.Data.Models;
using BlogHost.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using BlogHost.Initializer;
using System.IO;
using BlogHost.Data;

namespace BlogHost.Controllers
{
    public class PublicationController : Controller
    {
        UserManager<User> _userManager;
        private readonly IUser _user;
        private readonly IPublication _publication;
        private readonly ITopic _topic;
        private readonly AppDBContext _appDbContext;

        public PublicationController(UserManager<User> userManager, IUser iUser, IPublication iPublication, ITopic iTopic)
        {
            _user = iUser;
            _publication = iPublication;
            _topic = iTopic;
            _userManager = userManager;
        }

        public IActionResult CreatePost()
        {
            return View();
        }

        public async Task<IActionResult> Post(int id)
        {
            Publication post = _publication.GetPostDB(id);
            if (post == null)
            {
                return NotFound();
            }
            return View(post);
        }

        public IActionResult AllPosts(bool flag)
        {
            if (flag)
            {
                return View(_publication.AllPostIsFavorite());
            }
            else
            {
                return View(_publication.AllPost());
            }
        }

        [HttpGet]
        public IActionResult ThemsPost(string nameTopic)
        {
            return View(_publication.AllThemsPost(_topic.GetIDTopicDB(nameTopic)));
        }

        [HttpGet]
        public IActionResult MyPosts()
        {
            return View(_publication.MyPost(_user.GetUserDB(_userManager.GetUserId(User))));
        }

        [HttpPost]
        public IActionResult CreatePost(CreatePublicationViewModel model)
        {
            if (ModelState.IsValid)
            {
                byte[] imageData = null;
                byte[] imageData2 = null;
                // считываем переданный файл в массив байтов
                using (var binaryReader = new BinaryReader(model.AvatarPost.OpenReadStream()))
                {
                    imageData = binaryReader.ReadBytes((int)model.AvatarPost.Length);
                }
                using (var binaryReader = new BinaryReader(model.AvatarPost2.OpenReadStream()))
                {
                    imageData2 = binaryReader.ReadBytes((int)model.AvatarPost2.Length);
                }

                string a = _userManager.GetUserId(User);
                Publication publ = new Publication
                {
                    PublicationName = model.PublicationName,
                    Discription = model.Discription,
                    PublicationText = model.PublicationText,
                    AvatarPost = imageData,
                    AvatarPost2 = imageData2,
                    isFavorite = model.isFavorite,
                    TopicId = _topic.GetTopicDB(model.TopicName).Id,
                    Topic = _topic.GetTopicDB(model.TopicName),
                    User = _user.GetUserDB(_userManager.GetUserId(User)),
                };
                publ.LikePost = 0;
                _publication.AddPublicationDB(publ);
                return RedirectToAction("AllPosts");
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditPost(int id)
        {
            Publication post = _publication.GetPostDB(id);
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
                TopicName = _topic.GetTopicName(post.TopicId)
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditPost(EditePostViewModel model)
        {
            if (ModelState.IsValid)
            {
                Publication post = _publication.GetPostDB(model.Id);
                if (post != null)
                {
                    post.PublicationName = model.PublicationName;
                    post.PublicationText = model.PublicationText;
                    post.Discription = model.Discription;
                    post.TopicId = _topic.GetTopicDB(model.TopicName).Id;
                    post.Topic = _topic.GetTopicDB(model.TopicName);

                    _publication.UpdatePost(post);

                    return RedirectToAction("AllPosts");
                }
            }
            return View(model);
        }

        public async Task<IActionResult> Like(int id)
        {
            Publication update = _appDbContext.Publications.ToList()
                                                          .Find(u => u.Id == id);
            update.LikePost += 1;
            _appDbContext.SaveChanges();

            return RedirectToAction("Post", new { id = id });
        }


        public async Task<ActionResult> Delete(int id)
        {
            Publication post = _publication.GetPostDB(id);
            if (post != null)
            {
                _publication.DeletePost(post);
            }
            return RedirectToAction("AllPosts");
        }
    }
}