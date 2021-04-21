using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheOneTag.Data;
using TheOneTag.Models;

namespace TheOneTag.Services
{
    public class LeagueService
    {
        private readonly Guid _userId;
        public LeagueService(Guid userId)
        {
            _userId = userId;
        }

        public bool CreateLeague(LeagueCreate model)
        {
            var entity =
                new League()
                {
                    OwnerId = _userId,
                    LeagueName = model.LeagueName,
                    ZipCode = model.LeagueZipCode,
                    PrivateOrPublic = model.IsPrivate,
                    LeaguePassword = model.LeaguePassword,
                    LeagueCreated = DateTimeOffset.UtcNow,
                };

            using (var ctx = new ApplicationDbContext())
            {
                ctx.Leagues.Add(entity);
                return ctx.SaveChanges() == 1;
            }
        }

        public IEnumerable<LeagueListItem> GetLeagues()
        {
            using (var ctx = new ApplicationDbContext())
            {
                var query =
                    ctx
                    .Leagues
                    .Where(e => e.PrivateOrPublic == false) //in the future use this to look at all public leagues near the users ZipCode 
                    .Select(
                        e =>
                        new LeagueListItem
                        {
                            LeagueId = e.LeagueId,
                            LeagueName = e.LeagueName,
                            LeagueZipCode = e.ZipCode,
                            DateCreated = e.LeagueCreated
                        }
                    );
                return query.ToArray();
            }
        }

        public LeagueDetail GetLeagueById(int id)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity = ctx
                    .Leagues
                    .Single(e => e.LeagueId == id);

                return
                    new LeagueDetail
                    {
                        LeagueId = entity.LeagueId,
                        LeagueName = entity.LeagueName,
                        ZipCode = entity.ZipCode,
                        LeagueCreated = entity.LeagueCreated
                    };
            }
        }

        public IEnumerable<PlayerListItem> GetPlayerListByLeagueId(int id)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var query = ctx
                    .UserLeagues
                    .Where(e => e.LeagueId == id)
                    .Select(
                    e =>
                    new PlayerListItem
                    {
                        PlayerId = e.UserId,
                        FirstName = e.User.FirstName,
                        LastName = e.User.LastName,
                        LeagueRanking = e.Ranking,
                        IsStarred = e.User.IsStarred,
                        RoundScore = e.RoundScore
                    }
                    );
                return query.ToArray();
            }
        }

        public bool UpdateLeague(LeagueEdit model)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                    .Leagues
                    .SingleOrDefault(e => e.LeagueId == model.LeagueId && e.OwnerId == _userId);

                entity.LeagueName = model.LeagueName;
                entity.ZipCode = model.LeagueZipCode;
                entity.PrivateOrPublic = model.IsPrivate;
                //Change password here?
                entity.LeaguePassword = model.Password;

                return ctx.SaveChanges() == 1;
            }
        }


        public bool DeleteLeague(int leagueId)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                    .Leagues
                    .Single(e => e.LeagueId == leagueId && e.OwnerId == _userId);
                ctx.Leagues.Remove(entity);

                return ctx.SaveChanges() == 1;
            }
        }

        public bool AddPlayerToLeague(int id)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var league = ctx.Leagues.Find(id);

                if (league == null)
                    return false;

                var query =
                    ctx
                    .UserLeagues
                    .Where(e => e.LeagueId == league.LeagueId && e.UserId == _userId.ToString());

                var query2 =
                    ctx
                    .UserLeagues
                    .Where(e => e.LeagueId == league.LeagueId);

                var howManyPlayers = query2.Count();
                if (query.Count() == 0)
                {
                    var entity = new UserLeague()
                    {
                        LeagueId = id,
                        UserId = _userId.ToString(),
                        Ranking = howManyPlayers + 1
                    };
                    ctx.UserLeagues.Add(entity);

                    return ctx.SaveChanges() == 1;

                }
                return false;

            }
        }
        public bool UpdateUserLeagueScore(UserLeagueEdit model)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                    .UserLeagues
                    .SingleOrDefault
                    (e => e.UserId == model.ID && e.LeagueId == model.LeagueId);


                if(entity is null)
                {
                    return false;
                }
                
                entity.RoundScore = model.Score;
                return ctx.SaveChanges() == 1;
                
            }
        }
        public bool PlayLeagueRound(int id)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var query = ctx
                    .UserLeagues
                    .Where(e => e.User.IsStarred == true && e.LeagueId == id);
                List<UserLeague> testing = query.ToList();
                
                //var playerList = new List<ApplicationUser>();
                var rankList = new List<int>();
                var league = ctx.UserLeagues.Find(id);

                foreach (var ul in testing)
                {
                    //playerList.Add(ul.User);
                    rankList.Add(ul.Ranking);
                }

                //Sort the Ranks of the players
                rankList.Sort();

                //Take all instances of UserLeague and sort them by score
                var newList = query.ToList();
                
                newList.Sort(
                    delegate (UserLeague ul1, UserLeague ul2)
                    {
                        if (ul1.RoundScore == ul2.RoundScore)
                        {
                            return ul1.Ranking.CompareTo(ul2.Ranking);
                        }
                        return ul1.RoundScore.CompareTo(ul2.RoundScore);
                    }
                    );


                // for each player, give them their new ranking based on their score
                for (int i = 0; i < rankList.Count; i++)
                {
                    newList[i].Ranking = rankList[i];
                }

                // reset all scores to 0
                foreach (UserLeague ul in newList)
                {
                    ul.RoundScore = 0;
                }

                return ctx.SaveChanges() == testing.Count;
            }
        }

        public ApplicationUser GetPlayerById(string id)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity = ctx
                    .Users
                    .Single(e => e.Id == id);

                return entity;

            }
        }
        public bool UpdatePlayer(PlayerEdit model)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                    .Users
                    .Single(e => e.Id == model.PlayerId);

                entity.IsStarred = model.IsStarred;

                return ctx.SaveChanges() == 1;
            }
        }

    }
}
