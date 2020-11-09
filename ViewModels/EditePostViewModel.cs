using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogHost.ViewModels
{
    public class EditePostViewModel
    {
        public int Id { get; set; }
        public string PublicationName { get; set; }
        public string Discription { get; set; }
        public string PublicationText { get; set; }
        public string TopicName { get; set; }
    }
}
