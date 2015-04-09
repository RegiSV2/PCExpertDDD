using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PCExpert.Web.Api.Controllers.ViewControllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

	    public ActionResult Welcome()
	    {
		    return View();
	    }
    }
}