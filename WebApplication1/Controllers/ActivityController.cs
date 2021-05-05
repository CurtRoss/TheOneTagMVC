using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TheOneTag.Services;

namespace WebApplication1.Controllers
{
    public class ActivityController : Controller
    {
        // GET: Activity
        public ActionResult Index()
        {
            var service = CreateActivityService();
            var model = service.GetAllApplicationUsers();
            return View(model);
        }

        public ActionResult GetLeaguesForPlayer(int id)
        {
            var service = CreateActivityService();
            var model = service.GetAllLeaguesForPlayer(id);

            if (model is null)
            {
                return RedirectToAction("Index");
                //TempData
            }

            if (model != null)
            {
                string player = model.FirstOrDefault().PlayerName;
                ViewBag.User = $"{player}'s Leagues";
                return View(model);

            }

            ModelState.AddModelError("", "Player has not played any League Rounds");

            return RedirectToAction("Index");

        }

        private ActivityService CreateActivityService()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());

            if (userId != null)
            {
                var service = new ActivityService(userId);
                return service;
            }

            var serviceNoLogin = new ActivityService();
            return serviceNoLogin;
        }


    }
}