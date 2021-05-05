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
                            DateCreated = e.LeagueCreated,
                            OwnerId = e.OwnerId.ToString(),
                            CurrentUser = _userId.ToString()
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

                if (ctx.UserLeagues.SingleOrDefault(e => e.LeagueId == id && e.Ranking == 1) != null)
                {
                    return
                        new LeagueDetail
                        {
                            LeagueId = entity.LeagueId,
                            LeagueOwnerId = entity.OwnerId.ToString(),
                            CurrentUser = _userId.ToString(),
                            LeagueName = entity.LeagueName,
                            ZipCode = entity.ZipCode,
                            LeagueCreated = entity.LeagueCreated,
                            NumberOfPlayers = ctx.UserLeagues.Where(e => e.LeagueId == id).ToArray().Count(),
                            PlayerName = ctx.UserLeagues.SingleOrDefault(e => e.LeagueId == id && e.Ranking == 1).User.FullName
                        };
                }
                else
                {
                    return
                        new LeagueDetail
                        {
                            LeagueId = entity.LeagueId,
                            LeagueName = entity.LeagueName,
                            ZipCode = entity.ZipCode,
                            LeagueCreated = entity.LeagueCreated,
                            NumberOfPlayers = ctx.UserLeagues.Where(e => e.LeagueId == id).ToArray().Count(),
                            PlayerName = "Nobody is in this league yet!"
                        };
                }


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
                        RoundScore = e.RoundScore,
                        OwnerId = e.League.OwnerId.ToString(),
                        CurrentUserId = _userId.ToString()
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

        public bool DeletePlayerFromLeague(string id, int leagueId)
        {
            using (var ctx = new ApplicationDbContext())
            {

                var entity =
                    ctx
                    .UserLeagues
                    .SingleOrDefault(e => e.UserId == id && e.LeagueId == leagueId);

                if (entity is null)
                {
                    return false;
                }

                ctx.UserLeagues.Remove(entity);

                if (ctx.SaveChanges() == 1)
                {
                    var query =
                        ctx
                        .UserLeagues
                        .Where(e => e.LeagueId == leagueId);

                    var playerList = query.ToList();

                    playerList.Sort(
                     delegate (UserLeague ul1, UserLeague ul2)
                     {
                         return ul1.Ranking.CompareTo(ul2.Ranking);
                     }
                     );

                    for (int i = 0; i < playerList.Count; i++)
                    {
                        playerList[i].Ranking = i+1;
                    }

                    return ctx.SaveChanges() == playerList.Count;
                }

                return false;
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


                if (entity is null)
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
                List<UserLeague> playerList = query.ToList();


                var rankList = new List<int>();
                //var league = ctx.UserLeagues.Find(id);


                foreach (var ul in playerList)
                {
                    //playerList.Add(ul.User);
                    rankList.Add(ul.Ranking);
                }

                var oldRankList = new List<int>();



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

                // At this point the UserLeagues are sorted by top score to bottom score, but their rank hasnt changed, so I should be able to pull thier old ranks here
                for (int i = 0; i < newList.Count; i++)
                {
                    oldRankList.Add(newList[i].Ranking);
                }

                // for each player, give them their new ranking based on their score
                for (int i = 0; i < rankList.Count; i++)
                {
                    newList[i].Ranking = rankList[i];
                }

                for (int i = 0; i < playerList.Count; i++)
                {
                    var activity = new Activity
                    {
                        LeagueId = newList[i].LeagueId,
                        PlayerId = newList[i].UserId,
                        LeagueZipCode = newList[i].League.ZipCode,
                        PlayerZipCode = newList[i].User.ZipCode,
                        DateOfActivity = DateTimeOffset.Now,
                        EndingRank = newList[i].Ranking,
                        StartingRank = oldRankList[i]

                    };

                    ctx.Activities.Add(activity);
                    ctx.SaveChanges();
                }

                // reset all scores to 0
                foreach (UserLeague ul in newList)
                {
                    ul.RoundScore = 0;
                    ul.User.IsStarred = false;
                }

                return ctx.SaveChanges() == playerList.Count;
            }
        }

        public ApplicationUser GetPlayerById(string id)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity = ctx
                    .Users
                    .SingleOrDefault(e => e.Id == id);

                return entity;

            }
        }

        public UserLeagueDelete GetPlayerById1(string id, int leagueId)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity = ctx.UserLeagues.SingleOrDefault(e => e.UserId == id && e.LeagueId == leagueId);
                return new UserLeagueDelete
                {
                    LeagueId = entity.LeagueId,
                    PlayerName = entity.User.FullName,
                    UserId = entity.UserId
                };

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
