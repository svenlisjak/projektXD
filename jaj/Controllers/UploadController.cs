using jaj.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace jaj.Controllers
{
    public class UploadController : Controller
    {

        DbBaza db = new DbBaza();
        // GET: Upload
        
        public ActionResult Upload()
        {
            // Ako je user loginani,onda ga prolazi na view
            if (User.Identity.IsAuthenticated)
            {
                
                ViewBag.Title = "Upload page";
                return View();

            }

            // a ako nije onda ga vraca na login
            else
            {

                return RedirectToAction("Login", "User");
            }
        }
            

        //upload video controller
        [HttpPost]
        
        public ActionResult Add(video videoModel) {

            // U ovoj varijabli iUser je spremljen ID usera koji je loginan trenutno,pa moremo koristiti da ga spremimo u taj 
            // videouserId kad uploadamo video 
            string username = User.Identity.GetUserName();
            var wholeUser = db.userInfo.FirstOrDefault(x => x.Username == username);
            int iUser = wholeUser.UserID;


            //convert v file path ime datoteke i extension posebno
            string fileName = Path.GetFileNameWithoutExtension(videoModel.videoFile.FileName);
            string extension = Path.GetExtension(videoModel.videoFile.FileName);
            //provjera ako je file video formata
            if(extension == ".mp4" ||extension == ".3gp" || extension == ".webm" || extension == ".flv" || extension == ".ogg" || extension == ".gifv" || extension == ".avi" || extension == ".mov" || extension == ".amv")
            {
                //stvaranje modela za upis u bazu
                //svaki stupac tablice se može dodati i uredi ručno sa sintaksom videoModel.imeStupca
                fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                //definira da se sprema u folder SaveDir unutar projekta, promjenjeno bude na neku server mapu
                videoModel.videoPath = "~/SaveDir/" + fileName;
                fileName = Path.Combine(Server.MapPath("~/SaveDir/"), fileName);
                videoModel.videoFile.SaveAs(fileName);
                videoModel.uploadDate = DateTime.Now;
                videoModel.userID = iUser;

                db.videoFile.Add(videoModel);
                db.SaveChanges();

                //trebalo bi returnat stranicu s prikazom uploadonog videja return Content("uploadan");
               // return Content("uploadan");
                return RedirectToAction("videoPage", new { id = videoModel.videoID });
            }
            else
            {
                //trebalo bi napraviti exception kad se pokusa uploadad file koji nije video formata
                return Content("File nije video");
            }

        }
        [HttpGet]
        public ActionResult videoPage(int id)
        {
            video videoModel = new video();

            using (DbBaza db = new DbBaza())
            {
                videoModel = db.videoFile.Where(x => x.videoID == id).FirstOrDefault();
            }

            return View(videoModel);
        }

       


    }
}