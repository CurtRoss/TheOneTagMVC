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

        public bool UpdateLeague(LeagueEdit model)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                    .Leagues
                    .Single(e => e.LeagueId == model.LeagueId && e.OwnerId == _userId);

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
    }
}
