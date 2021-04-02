using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheOneTag.Models
{
    public class LeagueListItem
    {
        public int LeagueId { get; set; }

        [Display(Name="League Name")]
        public string LeagueName { get; set; }

        [Display(Name ="League Zip Code")]
        public int LeagueZipCode { get; set; }

        //Add number of players in the future.
    }
}
