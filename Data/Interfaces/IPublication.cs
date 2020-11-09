using BlogHost.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogHost.Data.Interfaces
{
    public interface IPublication
    {
        public void AddPublicationDB(Publication publ);

        public IEnumerable<Publication> AllPost();

        public IEnumerable<Publication> AllThemsPost(int idTopic);

        public IEnumerable<Publication> MyPost(User user);

        public Publication GetPostDB(int id);

        public void UpdatePost(Publication post);

        public void DeletePost(Publication post);

        public IEnumerable<Publication> AllPostIsFavorite();

    }
}
