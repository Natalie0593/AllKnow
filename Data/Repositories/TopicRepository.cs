using BlogHost.Data.Interfaces;
using BlogHost.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogHost.Data.Repositories
{
    public class TopicRepository : ITopic
    {
        private readonly AppDBContext _appDbContext;

        public TopicRepository(AppDBContext appDbContext)
        {
            this._appDbContext = appDbContext;
        }

        public int GetIDTopicDB(string nameTopic)
        {
            return _appDbContext.Topics.FirstOrDefault(x => x.NameTopic == nameTopic).Id;
        }

        public Topic GetTopicDB(string nameTopic)
        {
            return _appDbContext.Topics.FirstOrDefault(x => x.NameTopic == nameTopic);
        }
        public string GetTopicName(int Id)
        {
            return _appDbContext.Topics.FirstOrDefault(x => x.Id == Id).NameTopic;
        }

    }
}
