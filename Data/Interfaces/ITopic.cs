using BlogHost.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogHost.Data.Interfaces
{
    public interface ITopic
    {
        public Topic GetTopicDB(string nameTopic);
        public int GetIDTopicDB(string nameTopic);
        public string GetTopicName(int Id);
    }
}
