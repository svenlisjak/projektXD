using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;


namespace jaj.Models
{
    [Table("videos")]
    public class video
    {
        //classa koja određuje sve stupce koje se upisuju u tablicu video u bazi
        public int videoID { get; set; }
        public int userID { get; set; }
        public string videoName { get; set; }
        public string videoPath { get; set; }

        public string description { get; set; }

        public DateTime uploadDate { get; set; }

        public string comments { get; set; }
        public string tag { get; set; }

        //kod upisa u bazu, [NotMapped] je jako bitan jer inace imas pun kup errora kad pokusas uploadad file dok baza već ima zapise u sebi
        [NotMapped]
        public HttpPostedFileBase videoFile { get; set; }
    }

 
}