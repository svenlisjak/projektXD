using System;
using System.IO;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;
using jaj.Models;
using System.Security.Principal;
using Microsoft.AspNet.Identity;
using jaj.Misc;
using System.Net;
using System.Web.Script.Serialization;

namespace jaj.Controllers
{
    public class UserController : Controller
    {
        DbBaza db = new DbBaza();

        //ActionResult za registraciju - HttpGet
        public ActionResult Registration()
        {
            return View();
        }
        //HttpPost ActionResult za registraciju
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Registration(user user1)
        {
            string message = "";

            DbBaza dc = new DbBaza();

            string fileName = Path.GetFileNameWithoutExtension(user1.userInfo.FileName);
            string extension = Path.GetExtension(user1.userInfo.FileName);

            fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
            user1.profilePicture = "~/PPDir/" + fileName;
            fileName = Path.Combine(Server.MapPath("~/PPDir/"), fileName);
            user1.userInfo.SaveAs(fileName);

            if (!String.IsNullOrEmpty(user1.Username))
            {
                var TakenUsername = dc.userInfo.Any(x => x.Username == user1.Username);
                if (TakenUsername)
                {
                    ModelState.AddModelError("Username", "This username is already taken");
                }
            }

            if (!String.IsNullOrEmpty(user1.Email))
            {
                var emailTaken = dc.userInfo.Any(x => x.Email == user1.Email);
                if (emailTaken)
                {
                    ModelState.AddModelError("Email", "There is already a user registered with this email");
                }
            }



            // Validacija modela
            if (ModelState.IsValid)
            {
                // Hashing lozinke - preuzima se lozinka koju korisnik unosi i hashira se - takva se pohranjuje u bazu podataka
                // Znači da ne spremamo čisti string u bazu podataka,što je ključno za sigurnost lozinki računa korisnika
                var passwordHash = jaj.Misc.PasswordHelper.HashPassword(user1.PasswordEnter);

                // slučajna vrijednost koja sprječava rainbow napad (unaprijed izračunati hashevi lozinki)
                user1.Salt = passwordHash.Item1;
                user1.Password = passwordHash.Item2;

                // Pohranjivanje podataka u bazu podataka
                dc.userInfo.Add(user1);
                try
                {
                    dc.SaveChanges();
                    message = "Registration successfully done,you can now login ";
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (var entityValidationErrors in ex.EntityValidationErrors)
                    {
                        foreach (var validationError in entityValidationErrors.ValidationErrors)
                        {
                            Response.Write("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);
                        }
                    }
                }
                ViewBag.Message = message;
                RedirectToAction("Index");

            }

            return View(user1);
        }


        // HttpGet Akcija za login
        [HttpGet]
        [AllowAnonymous]
        // U ovaj returnUrl spremamo stranicu s koje je user preusmjeren na login
        // ako želi pristupiti nekoj metodi koja zahtjeva da je prijavljeni,kad se prijavi
        // onda ga vraćamo na stranicu kojoj je htio pristupiti
        public ActionResult Login(string returnUrl)
        {
            userLogin user = new userLogin();
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }


        //HttpPost akcija za login,prosljeduje model login koji sadrži email i hashiranu lozinku usera
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(userLogin login, string returnUrl)
        {
            

            using (DbBaza dc = new DbBaza())
            {
                if (ModelState.IsValid)
                {
                    // u varijablu v spremamo email koji smo unijeli prilikom logina
                    var v = dc.userInfo.Where(a => a.Email == login.Email).FirstOrDefault();
                    if (v != null)
                    {
                        // Tu imamo znaci usporedbu lozinka,one upisane u login formi i lozinke koja je u bazi podataka od toga usera
                        // samo kaj se prvo ova unesena lozinka mora isto hashirati jer se inace nemre uspoređivati s ovom u bazi
                        // također imamo validaciju salta,ova metoda ValidatePassword je definirano u Misc/PasswordHelper
                        var paswordOk = Misc.PasswordHelper.ValidatePassword(login.password, v.Password, v.Salt);

                        if (paswordOk)
                        {
                            LoggedInUser userIn = new LoggedInUser(v);
                            // Serijalizacija - pretvorba objekta klase u tekstualni oblik
                            // omogućava da podatke smjestimo u cookie za autentikaciju
                            LoggedInUserSerializeModel serializeUser = new LoggedInUserSerializeModel();
                            serializeUser.CopyFromUser(userIn);
                            // Serijalizacija pomocu javascript serijalizatora
                            JavaScriptSerializer serializer = new JavaScriptSerializer();
                            // Pretvorba serializeUser objekta u string
                            string userInformation = serializer.Serialize(serializeUser);

                            // Generiramo autorizacijski tiket i spremamo ga u cookie,aplikacija koristi cookie
                            // da li je user prijavljen i da daje sve druge podatke useru (username)
                            FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(
                                1, // Verzija
                                userIn.Identity.Name, // Ime tiketa,korisnicko ime jer smo postavili Identity.Name na username
                                DateTime.Now, // Vrijeme trajanja ticketa - od
                                DateTime.Now.AddDays(1), // Vrijeme trajanja ticketa - do - jedan dan traje
                                false, // isPersistent
                                userInformation); // Korisnicki podaci koji su serijalizirani

                            // Enkripcija kreiranog ticketa
                            string ticketEncrypted = FormsAuthentication.Encrypt(authTicket);

                            // Spremanje ticketa u cookie
                            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, ticketEncrypted);
                            // Odgovor servera korisniku - dodaje cookie
                            Response.Cookies.Add(cookie);

                            // Tu vadimo sve informacije o useru koji ima taj upisani email prilikom logina
                            var userDetails = dc.userInfo.Where(x => x.Email == login.Email).FirstOrDefault();
                            // Mi netrebamo bar za sad nist drugo osim userId,tak da se samo on vadi iz userDetailsa i sprema v varijablu
                            var userID = userDetails.UserID;

                            // E sad trebamo spremiti v nekom obliku taj userId da se ne zgubi prilikom premjestanja v drugi kontroler pa sam
                            // koristil tempData - neka vrsta privremene varijable
                            TempData["mydata"] = userID;




                            // Ako postoji returnUrl onda ga vraćamo na taj url
                            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                            {
                                return Redirect(returnUrl);
                            }
                            // Ako ne onda na neki drugi view
                            return RedirectToAction("Upload", "Upload");
                        }
                    }
                }

                ModelState.AddModelError("", "Entered username or password is not valid");
                return View(login);

            }

        }

        // [Authorize] -  Ograničava taj ActionResult da je dostupan samo userima koji su loginani
        [Authorize]
        //[HttpPost]
        public ActionResult Logout()
        {
            // Označava da se user logoutal pa nema više pristup akcijama s [Authorize]
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "User");
        }


        

    }
}