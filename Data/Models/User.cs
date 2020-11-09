using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogHost.Data.Models
{
    public class User : IdentityUser
    {
        public string Name { get; set; }
        public string SecondName { get; set; }
        public int Year { get; set; }
        public string Gender { get; set; }
        public string Profession { get; set; }
        public string ImgUrl { get; set; }

        public byte[] Avatar { get; set; }
        public DateTime DateRegistration { get; set; } = DateTime.Now;
        public List<Publication> Publications { get; set; }
    }
}
