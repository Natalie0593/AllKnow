using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlogHost.ViewModels
{
    public class CreatePublicationViewModel
    {
        [Required]
        [Display(Name = "Название")]
        public string PublicationName { get; set; }

        [Required]
        [Display(Name = "Описание")]
        public string Discription { get; set; }

        [Required]
        [Display(Name = "Текст")]
        public string PublicationText { get; set; }

        [Required]
        [Display(Name = "Фото")]
        public IFormFile AvatarPost { get; set; }

        [Required]
        [Display(Name = "Фото")]
        public IFormFile AvatarPost2 { get; set; }

        [Display(Name = "Любимое")]
        public bool isFavorite { get; set; }

        public string TopicName { get; set; }
    }
}
