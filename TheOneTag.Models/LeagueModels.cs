using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheOneTag.Data;

namespace TheOneTag.Models
{
    public class LeagueListItem
    {
        public int LeagueId { get; set; }

        [Display(Name="League Name")]
        public string LeagueName { get; set; }

        [Display(Name ="League Zip Code")]
        public int LeagueZipCode { get; set; }
        public DateTimeOffset DateCreated { get; set; }

        //Add number of players in the future.

        //Add bool IsAlreadyMember to track users leagues from this view.
    }
    public class LeagueCreate
    {
        [Display(Name = "League Name")]
        public string LeagueName { get; set; }

        [Display(Name = "League Zip Code")]
        public int LeagueZipCode { get; set; }

        [Display(Name = "Is this league private?")]
        public bool IsPrivate { get; set; } 

        [Display(Name ="League Password")]
        public string LeaguePassword { get; set; }

    }

    public class LeagueDetail
    {
        [Display(Name = "League ID")]
        public int LeagueId { get; set; }

        [Display(Name = "League Name")]
        public string LeagueName { get; set; }

        [Display(Name = "League Zip Code")]
        public int ZipCode { get; set; }

        [Display(Name = "League Created Date")]
        public DateTimeOffset LeagueCreated { get; set; }
    }

    public class LeagueEdit
    {
        [Display(Name = "League Id") ]
        public int LeagueId { get; set; }

        [Display(Name = "League Name")]
        public string LeagueName { get; set; }

        [Display(Name = "League Zip Code")]
        public int LeagueZipCode { get; set; }

        [Display(Name = "Is this league Private? Check if yes")]
        public bool IsPrivate { get; set; }

        [Display(Name = "Password")]
        public string Password { get; set; }
    }

    public class PlayARound
    {
        [Required, Display(Name = "League Id")]
        public int LeagueId { get; set; }
        public List<string> UserIds { get; set; } = new List<string>();
        public List<ApplicationUser> AppUsers { get; set; } = new List<ApplicationUser>();

    }

    public class PlayerListItem
    {
        public string PlayerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int LeagueRanking { get; set; }
        [UIHint("Starred")]
        [Display(Name = "Is Playing this Round?")]
        public bool IsStarred { get; set; }
        public int RoundScore { get; set; }

    }
    public class PlayerEdit
    {
        public string PlayerId { get; set; }
        public bool IsStarred { get; set; }
    }

    public class UserLeagueEdit
    {
        public string PlayerName { get; set; }
        public int Score { get; set; }
        public string ID { get; set; }
        public int LeagueId { get; set; }

    }
}
