using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TheOneTag.Models;
using TheOneTag.Services;

namespace WebApplication1.Controllers
{
    public class LeagueController : Controller
    {
        // GET: League
        public ActionResult Index()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new LeagueService(userId);
            var model = service.GetLeagues();
            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [Authorize, HttpPost, ValidateAntiForgeryToken]
        public ActionResult Create(LeagueCreate model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var service = CreateLeagueService();

            if (service.CreateLeague(model))
            {
                TempData["SaveResult"] = "Your league was created.";
                return RedirectToAction("Index");
            };

            ModelState.AddModelError("", "League could not be created.");

            return View(model);
        }

        public ActionResult Details(int id)
        {
            var service = CreateLeagueService();
            var model = service.GetLeagueById(id);
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            var service = CreateLeagueService();
            var detail = service.GetLeagueById(id);
            var model =
                new LeagueEdit
                {
                    LeagueId = detail.LeagueId,
                    LeagueName = detail.LeagueName,
                    LeagueZipCode = detail.ZipCode
                };
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken, Authorize]
        public ActionResult Edit(int id, LeagueEdit model)
        {
            if (!ModelState.IsValid) return View(model);

            if (model.LeagueId != id)
            {
                ModelState.AddModelError("", "ID mismatch");
                return View(model);
            }
            var service = CreateLeagueService();
            if (service.UpdateLeague(model))
            {
                TempData["SaveResult"] = "Your league was updated";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Your league could not be udpated.");
            return View();
        }

        [HttpGet, ActionName("Delete")]
        public ActionResult Delete(int id)
        {
            var service = CreateLeagueService();
            var model = service.GetLeagueById(id);

            return View(model);
        }

        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public ActionResult LeagueDelete(int id)
        {
            var service = CreateLeagueService();
            service.DeleteLeague(id);
            TempData["SaveResult"] = "Your league was deleted";
            return RedirectToAction("Index");
        }

        [ActionName("AddPlayer"), Authorize]
        public ActionResult AddPlayerToLeague(int id)
        {
            var service = CreateLeagueService();

            if (!service.AddPlayerToLeague(id))
            {
                TempData["NotSaved"] = "Player is already in league.";
                return RedirectToAction("Index");
            }

            TempData["SaveResult"] = "Player was added to league.";
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult PlayLeagueRound()
        {
            return View();
        }


        [HttpPost, ActionName("PlayRound"), Authorize]
        public ActionResult PlayLeagueRound()
        {
            var service = CreateLeagueService();

            service.PlayARound(id);
        }

        private LeagueService CreateLeagueService()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new LeagueService(userId);
            return service;
        }
    }
}