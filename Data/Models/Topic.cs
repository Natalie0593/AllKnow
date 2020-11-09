using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlogHost.Data.Models
{
    public class Topic
    {
        [Key]
        public int Id { get; set; }
        public string NameTopic { get; set; }
        public List<Publication> Publications { get; set; }
    }
}
