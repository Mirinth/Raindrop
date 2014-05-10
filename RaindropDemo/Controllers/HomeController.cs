using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RaindropDemo
{
    public class HomeController : Controller
    {
        private ArticleDBContext db = new ArticleDBContext();

        public ActionResult Index()
        {
            var query = from a in db.Articles
                        orderby a.Date descending
                        select a;

            Article result = null;

            foreach (var first in query.Take(1))
            {
                result = first;
            }

            if (result != null)
            {
                ViewData.Add("title", result.Title);
                ViewData.Add("date", result.Date);
                ViewData.Add("id", result.ID);
                ViewData.Add("content", result.Content);
            }
            else
            {
                ViewData.Add("title", "No articles.");
                ViewData.Add("date", DateTime.Now);
                ViewData.Add("id", -1);
                ViewData.Add("content", "No content.");
            }

            return View();
        }
    }
}
