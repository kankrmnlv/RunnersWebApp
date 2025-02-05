﻿using System.ComponentModel.DataAnnotations;
namespace RunnersWebApp.Models
{
    public class Address
    {
        [Key]
        public int Id { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Street { get; set; }
    }
}
