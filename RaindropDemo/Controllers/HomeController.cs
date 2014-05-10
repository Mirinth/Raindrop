using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RaindropDemo
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewData = DemoModel.GetData();


            return View();
        }
    }
}
