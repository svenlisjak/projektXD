using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.ComponentModel;
using System.Web;

namespace jaj.Models
{
    public class user
    {
        public string profilePicture { get; set; }
        [NotMapped]
        public HttpPostedFileBase userInfo { get; set; }

        // Key - Primarni ključ
        [Key]
        public int UserID { get; set; }

        //Display - što će se prikazati u textboxu za upis
        //Required - Validacija - Ne dozvoljava ostavljanje praznog textboxa prilikom registracije
        // ErrorMessage - Prikazuje se ukoliko Required uvjet nije zadovoljen
        [Display(Name = "Username")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Username required")]
        public string Username { get; set; }


        //DataType - dodatna validacija za email adresu - provjerava da li
        // je pravilna forma upisa,da li postoji na pocetku mailbox name
        // nakon toga "@" simbol i nakon toga ime domene
        [Display(Name = "Email")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Email adress is required")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }


        // atribut lozinke koji se zapravo sprema u bazu
        public string Password { get; set; }



        [NotMapped]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Minimum 6 characters required")]
        public string PasswordEnter { get; set; }

        //Compare - uspoređuje PasswordEnter sa PasswordEnterom2 - ukoliko lozinke nisu 
        // iste,javlja se ErrorMessage 
        [NotMapped]
        [Required]
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("PasswordEnter", ErrorMessage = "Confirm password and password do not match")]
        public string PasswordEnter2 { get; set; }

        [Display(Name = "Favorite Tag")]
        public string FavTag { get; set; }


        public string Salt { get; set; }


        // Ovaj atribut se ne mapira jer nema set metodu
        public string NameUser
        {
            get
            {
                return this.NameUser;
            }
        }

        




    }
}
