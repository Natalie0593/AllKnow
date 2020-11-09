using Microsoft.AspNetCore.Mvc;

namespace BlogHost.Controllers
{
    public class ChatController : Controller
    {
        public IActionResult Chat()
        {
            return View();
        }
    }
}