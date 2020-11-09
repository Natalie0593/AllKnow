using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogHost.ViewModels
{
    public class EditUserViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string SecondName { get; set; }
        public int Year { get; set; }
        public string Gender { get; set; }
        public string Profession { get; set; }
        public string Email { get; set; }
    }
}
