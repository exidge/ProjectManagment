using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZarzadzanieProjektem.DAL;
using ZarzadzanieProjektem.Models;

namespace ZarzadzanieProjektem.Controllers
{
    public class SingleProjectController : Controller
    {
        private ProjectContext db = new ProjectContext();
        private ApplicationUser user;
        private Osoba osoba;
        // GET: SingleProject
        [Authorize(Roles = "Administrator,Student")]
        public ActionResult Index()
        {
            user= System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            osoba= db.osobaEntities.First(e => e.OsobaId == user.Details.OsobaId);
            if (osoba.ProjektClaimed == null)
            {
                return View("WithoutFinalClaim", osoba.NonClaimedProjekts);
            }
            return View(osoba.ProjektClaimed);
        }
        //public ActionResult Add()
        //{
        //    return new HttpStatusCodeResult(500);
        //}
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator,Student")]
        public ActionResult Add([Bind(Include = "ProjectDetailsId,GitHubLink")] ProjectDetails projectDetails, [Bind(Include = "DokumentacjaId,NazwaPliku,Rozszerzenie,Plik")] Dokumentacja dokumentacja, HttpPostedFileBase upload,HttpPostedFileBase upload2)
        //public ActionResult Add(HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                if (upload != null && upload.ContentLength > 0)
                {
                    db.Database.Log = s => System.Diagnostics.Debug.WriteLine(s);
                    ProjectDetails det=db.ProjectDetails.First(e => e.ProjectDetailsId == projectDetails.ProjectDetailsId);
                    det.GitHubLink = projectDetails.GitHubLink;
                    MemoryStream target = new MemoryStream();
                    upload.InputStream.CopyTo(target);
                    byte[] data = target.ToArray();
                    BinaryData FileData = new BinaryData { Plik = data };
                    dokumentacja.Plik = FileData;
                    dokumentacja.NazwaPliku = upload.FileName;
                    dokumentacja.Rozszerzenie = Path.GetExtension(upload.FileName).Replace(".", "");
                    MemoryStream target2 = new MemoryStream();
                    upload2.InputStream.CopyTo(target2);
                    byte[] data2 = target2.ToArray();
                    BinaryData FileData2 = new BinaryData { Plik = data2 };
                    Dokumentacja Zalozenia = new Dokumentacja();
                    Zalozenia.Plik = FileData2;
                    Zalozenia.NazwaPliku = upload2.FileName;
                    Zalozenia.Rozszerzenie = Path.GetExtension(upload2.FileName).Replace(".", "");
                    db.Entry(dokumentacja).State = System.Data.Entity.EntityState.Added;
                    db.Entry(Zalozenia).State = System.Data.Entity.EntityState.Added;
                    db.SaveChanges();
                    det.Dokumentacja = dokumentacja;
                    det.DokumentacjaZalozen = Zalozenia;
                    db.Entry(det).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
            }
            ApplicationUser user;
            Osoba osoba;
            user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            osoba = db.osobaEntities.First(e => e.OsobaId == user.Details.OsobaId);
            return View(osoba.ProjektClaimed);
        }


    }
}