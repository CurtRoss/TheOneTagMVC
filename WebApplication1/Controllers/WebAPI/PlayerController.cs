using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TheOneTag.Models;
using TheOneTag.Services;

namespace WebApplication1.Controllers.WebAPI
{
    [Authorize]
    [RoutePrefix("api/Player")]
    public class PlayerController : ApiController
    {
        private bool SetStarState(string playerId, bool newState)
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new LeagueService(userId);

            //Get Player
            var detail = service.GetPlayerById(playerId);

            //Create the PlayerDetail model instance with the new star state
            var updatedPlayer =
                new PlayerEdit
                {
                    PlayerId = detail.Id,
                    IsStarred = newState
                };

            //Return a value indicating whether the update succeeded

            return service.UpdatePlayer(updatedPlayer);
        }

        [Route("{id}/Star")]
        [HttpPut]
        public bool ToggleStarOn(string id) => SetStarState(id, true);

        [Route("{id}/Star")]
        [HttpDelete]
        public bool ToggleStarOff(string id) => SetStarState(id, false);
    }
}
