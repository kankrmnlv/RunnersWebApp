﻿using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace RunnersWebApp.Models
{
    public class AppUser : IdentityUser
    {
        public int? Pace { get; set; }

        public int? Mileage { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        [ForeignKey("Address")]
        public int? AddressId { get; set; }
        public Address? Address { get; set; }

        public ICollection<Club> Clubs { get; set; }
        public ICollection<Race> Races { get; set; }
    }
}
