using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheOneTag.Models
{
    public class ActivityPlayerListItem
    {
        public string PlayerId { get; set; }
        public int? PdgaNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }

        public int ZipCode { get; set; }
    }

    public class ActivityPlayerLeagueListItem
    {
        public string PlayerName { get; set; }
        public string LeagueName { get; set; }
        public int LeagueRanking { get; set; }
        public int LeagueRoundsPlayed { get; set; }
        public int? BiggestRankingJump { get; set; }
        public double AverageRanking { get; set; }
        public int LeagueId { get; set; }
    }
}
