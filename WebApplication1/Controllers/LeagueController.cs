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

        public ActionResult DeletePlayerFromLeague(string id, int leagueId)
        {
            var service = CreateLeagueService();
            var detail = service.GetPlayerById1(id, leagueId);
           
            return View(detail);
        }

        [Authorize]
        [HttpPost]
        [ActionName("DeletePlayerFromLeague")]
        public ActionResult DeletePlayerFromLeagues(string id, int leagueId)
        {
            if (!ModelState.IsValid)
                return HttpNotFound();

            var service = CreateLeagueService();
            service.DeletePlayerFromLeague(id, leagueId);
            TempData["SaveResult"] = "Player was deleted";
            
            return RedirectToAction($"PlayLeagueRound/{leagueId}");

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
        
        public ActionResult PlayLeagueRound(int id)
        {
            //User needs to checkmark all players who are playing the round, return PlayRound Model with all info added for each player playing.
            var service = CreateLeagueService();
            var model = service.GetPlayerListByLeagueId(id);
            ViewBag.leagueId = id;
            return View(model);
            
        }


        [Authorize]
        public ActionResult PlayRound(int id)
        {
            //This should take the PlayRound model and use the information to reorder the players and edit the players ranking in the UserLeague junction table entity.
            var service = CreateLeagueService();

            if (!service.PlayLeagueRound(id))
                ModelState.AddModelError("", "Your ranks have not been updated.");

            ViewBag.id = id;
            
            //I want to return a view of the League with the Players in their new ranking.
            return RedirectToAction($"PlayLeagueRound/{id}");
        }

        
        public ActionResult UserLeagueEdit(string id, int leagueId)
        {
            var service = CreateLeagueService();
            var detail = service.GetPlayerById(id);
            var model =
                new UserLeagueEdit
                {
                    Score = detail.Score,
                    ID = detail.Id,
                    LeagueId = leagueId,
                    PlayerName = detail.FirstName + " " + detail.LastName
                };
            return View(model);
        }

        //Create UserLeagueEdit View
        [HttpPost, ValidateAntiForgeryToken, Authorize]
        public ActionResult UserLeagueEdit(UserLeagueEdit model)
        {
            var service = CreateLeagueService();

            
            service.UpdateUserLeagueScore(model);
            return RedirectToAction($"PlayLeagueRound/{model.LeagueId}");
        }

        private LeagueService CreateLeagueService()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new LeagueService(userId);
            return service;
        }

        // Service to re-order tags if records have been deleted from the database.
        public ActionResult ResetRanks(int leagueId)
        {
            var service = CreateLeagueService();

            if (service.ResetScores(leagueId) == true)
            return RedirectToAction($"PlayLeagueRound/{leagueId}");

            else
            {
                return RedirectToAction("Index");
            }
        }
    }
}