using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TheOneTag.Models;

namespace WebApplication1.Controllers
{
    public class LeagueController : Controller
    {
        // GET: League
        public ActionResult Index()
        {
            var model = new LeagueListItem[0];
            return View(model);
        }
    }
}