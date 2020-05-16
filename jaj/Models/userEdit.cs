using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace jaj.Models
{


    //Klasa za edit user accounta
    public class userEdit
    {

        [Display(Name = "Username")]
        [Required]
        public string Username { get; set; }

        [Display(Name = "Email")]
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "Favorite Tag")]
        [Required]
        public string FavTag { get; set; }

        [Display(Name = "Profile Image")]
        public string profilePicture { get; set; }

        [NotMapped]
        public HttpPostedFileBase userInfo { get; set; }

        public int UserID { get; set; }



    }
}