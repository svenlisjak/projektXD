using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using jaj.Models;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace jaj.Controllers
{
    public class videoListController : Controller
    {

        DbBaza db = new DbBaza();
        // GET: videoList
        public ActionResult Index()
        {
            List<video> videolist = new List<video>();
          
            using (DbBaza db = new DbBaza())
            {
                videolist = db.videoFile.ToList();
            }

                return View(videolist);
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