using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheOneTag.Data
{
    public class Activity
    {
        public int ActivityId { get; set; }


        [ForeignKey(nameof(Player))]
        public string PlayerId { get; set; }
        public virtual ApplicationUser Player { get; set; }

        [ForeignKey(nameof(League))]
        public int LeagueId { get; set; }
        public virtual League League { get; set; }

        public DateTimeOffset DateOfActivity { get; set; }
        public int PlayerZipCode { get; set; }
        public int LeagueZipCode { get; set; }
        public int StartingRank { get; set; }
        public int EndingRank { get; set; }
        public int? RankChange
        {
            get
            {
                return StartingRank - EndingRank;
            }
        }

    }
}
