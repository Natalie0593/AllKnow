using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Services;
using ViewModels;
using Entities;

namespace BlogHost.Controllers
{
    
    public class VideosController : Controller
    {
        //private readonly CourseDbContext _context;
        //private readonly IVideo _video;
        IVideoService _videService;
       public VideosController(/*CourseDbContext context*/ IVideoService videoService)///, IVideo video)
        {
            //_video = video;
            _videService = videoService;
           // _context = context;
        }

        //public async Task<IActionResult> Index()
        //{
        //    //var appDbContext = _context.Videos.Include(v => v.Module);
        //    return View(await _videService.GetAllVideos());
        //}

        public async Task<IActionResult> Details(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var video = await _videService.GetVideo(id);

            if (video == null)
            {
                return NotFound();
            }

            return View(video);
        }

       
        public IActionResult Create()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
      
        public async Task<IActionResult> Create(int id, VideoViewModel video)//[Bind("Id,Name,Url")] Video video)
        {
            if (ModelState.IsValid)
            {
                Video new_video = await _videService.AddVideo(id, video);

                if(new_video != null)
                    return RedirectToAction("Post", "Publication", new { id = new_video.PublicationId });
            }
           
            return View(video);
        }

      
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var video = await _videService.GetVideo(id);
            if (video == null)
            {
                return NotFound();
            }
            return View(video);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public async Task<IActionResult> Edit(int id, VideoViewModel video)
        {
            if (ModelState.IsValid)
            {
                Video v = await _videService.EditVideo(id, video);
                if(v != null)
                    return RedirectToAction("Post", "Publication", new { id = v.PublicationId });                
            }            
            return View(video);
        }

       
        public async Task<IActionResult> Delete(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var video = await _videService.GetVideo(id);
            if (video == null)
            {
                return NotFound();
            }

            return View(video);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        
        public async Task<IActionResult> DeleteConfirmed(int id)
        {    
            var video = await _videService.GetVideo(id);              
            await _videService.DeleteVideoById(id);           
            return RedirectToAction("Post", "Publication", new { id = video.PublicationId });
        }

       
    }
}
