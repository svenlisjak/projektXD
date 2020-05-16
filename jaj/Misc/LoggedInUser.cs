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

        }

        // Konstruktor samo za username
        public LoggedInUser(string username)
        {
            this.Identity = new GenericIdentity(username);
            this.Username = username;
        }






    }
}