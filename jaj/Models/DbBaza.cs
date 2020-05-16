using MySql.Data.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace jaj.Models
{
    // Osigurava ispravno funkcioniranje EntityFrameworka i MySQL
    // baze podataka u ovoj verziji NugGet packagea
    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class DbBaza : DbContext
    {

        //tbh nit sam neznam kaj to znaci i dela, al mislim da svaka tablica treba svoju posebnu clasu 
        // sintaksa: public virtual DbSet<IME TABLICE> Ime modela (varijable s kojom se radi) { get; set; }
        public virtual DbSet<video> videoFile { get; set; }

        // Ovo je repozitorij za klasu "user"
        // Ja mislim da je sintaksa - DbSet <ime klase> ime atributa {get; set;} - ime tablice mi je "users"
        // znaci u ovom slucaju ime klase je "user" a ime atributa je "userInfo"
        // Primjer implementacije: dc.userInfo.Add(user1) - Dodaje novog objekta u bazu podataka
        public virtual DbSet<user> userInfo { get; set; }
        //public virtual DbSet<user> profilePic { get; set; }


        //public DbSet<video> videoList { get; set; }



    }
}