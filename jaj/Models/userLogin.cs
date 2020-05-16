using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace jaj.Models
{

    // Ova klasa će nam služiti da provjerimo podatke prilikom logiranja
    public class userLogin
    {
        // model sadrži validaciju i Display komponentu

        [Display(Name = "Email")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Email adress is required]")]
        public string Email { get; set; }


        //DataType.Password - prilikom upisa lozinke se sakrivaju slova i znamenke 
        // i prikazuju se točkice - ( neka forma sigurnosti prilikom upisa )
        [Required(AllowEmptyStrings = false, ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string password { get; set; }

       
    }
}