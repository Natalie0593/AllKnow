using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlogHost.ViewModels
{
    public class CreateCommentViewModel
    {
        [Required]
        [Display(Name = "Остаавьте свой комментарий")] 
        public string CommentText { get; set; }
    }
}
