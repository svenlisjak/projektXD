using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace jaj.Models
{
    interface ILoggedInUser : IPrincipal
    {
        string Username { get; set; }
        string Power { get; set; }
        string profilePicture { get; set; }



    }
}
