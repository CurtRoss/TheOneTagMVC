﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheOneTag.Data
{
    public class UserLeague
    {
        [Key]
        public int UserLeagueId { get; set; }

        [ForeignKey(nameof(League))]
        public int LeagueId { get; set; }
        public League League { get; set; }

        [ForeignKey(nameof(User))]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        [Required]
        public int Ranking { get; set; }
    }
}