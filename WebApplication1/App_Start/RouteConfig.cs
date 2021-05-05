using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace TheOneTag.WebAPI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "UserLeagueEdit",
                url: "League/UserLeagueEdit/{id}/{leagueId}",
                defaults: new { controller = "League", action = "UserLeagueEdit", id = @"\d+", leagueId = @"\d+" }
                );

            routes.MapRoute(
                name: "PlayRound",
                url: "League/PlayRound/{id}",
                defaults: new { controller = "League", action = "PlayRound", id = @"\d+" }
                );
        }
    }
}
