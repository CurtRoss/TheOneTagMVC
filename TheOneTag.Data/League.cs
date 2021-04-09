using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheOneTag.Data
{
    public class League
    {
        [Key]
        public int LeagueId { get; set; }

        [Required]
        public Guid OwnerId { get; set; }
        [Required]
        public string LeagueName { get; set; }
        public string LeaguePassword { get; set; }
        [Required]
        public int ZipCode { get; set; }
        [Required]
        public DateTimeOffset LeagueCreated { get; set; }
        public DateTimeOffset? LeagueTermination { get; set; }
        [Required]
        public bool PrivateOrPublic { get; set; }
        
    }
}
