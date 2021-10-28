using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AdminDashboardMVC.Models.UserSettings
{
    public class UserDetailsModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Nickname { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Country { get; set; }
        public string Website { get; set; }
        public string WorkPhone { get; set; }
    }
}