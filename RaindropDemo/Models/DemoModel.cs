using System.Collections.Generic;
using System.Web.Mvc;

namespace RaindropDemo
{
    public class DemoModel
    {
        public static ViewDataDictionary GetData()
        {
            string[] names = { "Alice",
                               "Bob",
                               "Cindy",
                               "Alice",
                               "Bob", 
                               "Alice",
                               "Eve",
                               "Alice",
                               "Eve",
                               "Alice" };

            string[] comments = { "Hello, everyone!",
                                  "Hi!",
                                  "Hey, how's it going?",
                                  "Not so good...",
                                  "Why? What's up?",
                                  "Eve's being a pain.",
                                  "No I'm not.",
                                  "Yes you are! You keep following me everywhere!",
                                  "You're just paranoid.",
                                  "Whatever. Leave me alone."
                                };

            string[] dates = { "12/24/2014 09:17",
                               "12/24/2014 09:19",
                               "12/24/2014 09:20",
                               "12/24/2014 09:25",
                               "12/24/2014 09:27",
                               "12/24/2014 09:37",
                               "12/24/2014 09:40",
                               "12/24/2014 09:41",
                               "12/24/2014 09:42",
                               "12/24/2014 09:42",
                             };


            List<ViewDataDictionary> posts = new List<ViewDataDictionary>();

            for (int i = 0; i < 10; i++)
            {
                ViewDataDictionary post = new ViewDataDictionary();
                post.Add("name", names[i]);
                post.Add("content", comments[i]);
                post.Add("date", dates[i]);
                posts.Add(post);
            }

            ViewDataDictionary data = new ViewDataDictionary();

            data.Add("posts", posts);
            data.Add("page-title", "Recent posts");
            
            return data;
        }
    }
}