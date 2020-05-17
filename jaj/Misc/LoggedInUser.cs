using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using jaj.Models;
using jaj.Misc;

namespace jaj.Misc
{

    // Klasa koja implementira ILoggedIn sučelje
    public class LoggedInUser : ILoggedInUser
    {
        public string Username { get; set; }
        public string Power { get; set; }
        public string profilePicture { get; set; }

        public IIdentity Identity { get; private set; }

        // Ova metoda provjerava ovlasti usera,mi nemamo ovlasti na našoj stranici 
        // ali interface treba imati tu metodu,ali ju nebudemo iskoristili
        public bool IsInRole(string role)
        {
            if (role == Power) return true;
            return false;
        }

        // Konstruktor klase,parametar je objekt klase user
        public LoggedInUser(user user1)
        {
            // Postavljamo identity na username usera
            this.Identity = new GenericIdentity(user1.Username);
            this.Username = user1.Username;
            this.profilePicture = user1.profilePicture;

        }

        // Konstruktor samo za username i  profilePicture
        public LoggedInUser(string username/*, string profilePicture*/)
        {
            this.Identity = new GenericIdentity(username);
            this.Username = username;
            this.profilePicture = profilePicture;
        }

        //public LoggedInUser(string profilePicture)
        //{
        //    this.Identity = new GenericIdentity(profilePicture);
        //    this.profilePicture = profilePicture;
        //}








    }
}