using BlogHost.Data;
using BlogHost.Data.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogHost.Initializer
{
    public class DBInitializer
    {
        public static void Initial(AppDBContext context)
        {
            if (!context.Topics.Any())
            {
                context.Topics.AddRange(Topics.Select(x => x.Value));
            }

            context.SaveChanges();
        }

        private static Dictionary<string, Topic> topic;
        public static Dictionary<string,Topic> Topics
        {
            get
            {
                if(topic == null)
                {
                    var list = new Topic[]
                    {
                        new Topic{ NameTopic = "World"},
                        new Topic{ NameTopic = "U.S."},
                        new Topic{ NameTopic = "Technology"},
                        new Topic{ NameTopic = "Design"},
                        new Topic{ NameTopic = "Culture"},
                        new Topic{ NameTopic = "Business"},
                        new Topic{ NameTopic = "Politics"},
                        new Topic{ NameTopic = "Opinion"},
                        new Topic{ NameTopic = "Science"},
                        new Topic{ NameTopic = "Health"},
                        new Topic{ NameTopic = "Style"},
                        new Topic{ NameTopic = "Travel"},
                    };

                    topic = new Dictionary<string, Topic>();
                    foreach(var el in list)
                    {
                        topic.Add(el.NameTopic, el);
                    }
                }
                return topic;
            }
        }
    }
}
