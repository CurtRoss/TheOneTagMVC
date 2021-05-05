using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheOneTag.Data;
using TheOneTag.Models;

namespace TheOneTag.Services
{
    public class ActivityService
    {
        private readonly Guid _userId;
        public ActivityService(Guid userId)
        {
            _userId = userId;
        }

        public ActivityService() { }

        public IEnumerable<ActivityPlayerListItem> GetAllApplicationUsers()
        {
            using (var ctx = new ApplicationDbContext())
            {
                var query =
                    ctx
                    .Users
                    .Select(
                    e => new ActivityPlayerListItem
                    {
                        PdgaNumber = e.PdgaNum,
                        FirstName = e.FirstName,
                        LastName = e.LastName,
                        ZipCode = e.ZipCode,
                        PlayerId = e.Id,
                        
                    }
                    );

                //query.ToList();
                var orderedList = query.OrderBy(x => x.PdgaNumber).ThenBy(x => x.LastName).ThenBy(x => x.FirstName).ThenBy(x => x.PlayerId);



                return orderedList.ToArray();
                  
            }
        }

        public IEnumerable<ActivityPlayerLeagueListItem> GetAllLeaguesForPlayer(int id)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var activities =
                    ctx
                    .Activities
                    .Where(e => e.IdHash == id).ToList();

                    //.Where(e => e.PlayerId == id).ToList();

                if (activities.Count == 0)
                    return null;

                
                var query =
                    ctx
                    .UserLeagues
                    .Where(e => e.IdHash == id).ToList()

                    //.Where(e => e.UserId == id).ToList()
                    .Select(
                        e => new ActivityPlayerLeagueListItem
                        {
                            PlayerName = e.User.FullName,
                            LeagueId = e.LeagueId,
                            LeagueName = e.League.LeagueName,
                            LeagueRanking = e.Ranking,
                            AverageRanking = activities.Where(x => x.LeagueId == e.LeagueId).DefaultIfEmpty().ToArray()[0] == null ? 0 : activities.Where(x => x.LeagueId == e.LeagueId).ToArray().Average(x => x.EndingRank),
                            BiggestRankingJump = activities.Where(x => x.LeagueId == e.LeagueId) == null ? 0 : activities.Where(x => x.LeagueId == e.LeagueId).ToArray().Max(x => x.RankChange),
                            LeagueRoundsPlayed = activities.Where(x => x.LeagueId == e.LeagueId) == null ? 0 : activities.Where(x => x.LeagueId == e.LeagueId).Count()

                        });


                    
                 return query.ToArray();
            }
        }
    }
}
