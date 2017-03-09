using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ZarzadzanieProjektem.DAL;
using ZarzadzanieProjektem.Models;

namespace ZarzadzanieProjektem.Controllers
{
    public class ProjectsController : Controller
    {
        private ProjectContext db = new ProjectContext();

        // GET: Projects
        [Authorize(Roles = "Administrator,Student")]
        public ActionResult Index()
        {
            if (User.IsInRole("Administrator"))
            {
                ProjectViewModel viewm = new ProjectViewModel { Projekty = db.projectEntities.ToList(), LoggedUser = null };
                return View(viewm);
            }
            else
            {
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
                Osoba osoba = db.osobaEntities.First(e => e.OsobaId == user.Details.OsobaId);
                ProjectViewModel viewModel = new ProjectViewModel { Projekty = db.projectEntities.ToList(), LoggedUser = osoba };
                return View(viewModel);
            }
        }

        // GET: Projects/Details/5
        [Authorize(Roles = "Administrator,Student")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.projectEntities.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // GET: Projects/Create
        [Authorize(Roles = "Administrator,Student")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator,Student")]
        public ActionResult Create([Bind(Include = "ProjectId,Tytul,Opis,Wymagania")] Project project, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                if (upload != null && upload.ContentLength > 0)
                {
                    MemoryStream target = new MemoryStream();
                    upload.InputStream.CopyTo(target);
                    byte[] data = target.ToArray();
                    BinaryData FileData = new BinaryData { Plik = data };
                    Dokumentacja dokumentacja = new Dokumentacja();
                    dokumentacja.Plik = FileData;
                    dokumentacja.NazwaPliku = upload.FileName;
                    dokumentacja.Rozszerzenie = Path.GetExtension(upload.FileName).Replace(".", "");
                    //db.Entry(dokumentacja).State = System.Data.Entity.EntityState.Added;
                    //db.SaveChanges();
                    project.Zalozenia = dokumentacja;
                    db.projectEntities.Add(project);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    db.projectEntities.Add(project);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            return View(project);
        }

        // GET: Projects/Edit/5
        [Authorize(Roles = "Administrator,Student")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.projectEntities.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator,Student")]
        public ActionResult Edit([Bind(Include = "ProjectId,Tytul,Opis,Wymagania,Zalozenia")] Project project, int? id,HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                if (upload != null && upload.ContentLength > 0)
                {
                    db.Database.Log = s => System.Diagnostics.Debug.WriteLine(s);
                    MemoryStream target = new MemoryStream();
                    upload.InputStream.CopyTo(target);
                    byte[] data = target.ToArray();
                    BinaryData FileData = new BinaryData { Plik = data };
                    if (id != -1)
                    {
                        Dokumentacja docOrg = db.Dokumentacjas.Find(id);
                        docOrg.NazwaPliku = upload.FileName;
                        docOrg.Rozszerzenie = Path.GetExtension(upload.FileName).Replace(".", "");
                        docOrg.Plik = FileData;
                        db.Entry(docOrg).State = EntityState.Modified;
                        db.Entry(project).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    else
                    {
                        Dokumentacja dokumentacja = new Dokumentacja();
                        dokumentacja.Plik = FileData;
                        dokumentacja.NazwaPliku = upload.FileName;
                        dokumentacja.Rozszerzenie = Path.GetExtension(upload.FileName).Replace(".", "");
                        project.Zalozenia = dokumentacja;
                        db.Entry(dokumentacja).State = EntityState.Added;
                        db.Entry(project).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    return RedirectToAction("Index");
                }
                else
                {
                    db.Entry(project).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(project);
        }

        // GET: Projects/Delete/5
        [Authorize(Roles = "Administrator,Student")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.projectEntities.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator,Student")]
        public ActionResult DeleteConfirmed(int id)
        {
            Project project = db.projectEntities.Find(id);
            db.projectEntities.Remove(project);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        // MyCode
        // GET: Projects/ClaimProject/5
        [Authorize(Roles = "Administrator,Student")]
        public ActionResult ClaimProject(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.projectEntities.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            //Claim Project code Here
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            //List<Osoba> testList = new List<Osoba>();
            //testList.Add(user.Details);
            //project.Wykonawcy = testList;
            Osoba osoba = db.osobaEntities.First(e => e.OsobaId == user.Details.OsobaId);
            project.Wykonawcy.Add(osoba);
            db.Entry(project).State = EntityState.Modified;
            db.SaveChanges();
            ProjectViewModel viewModel = new ProjectViewModel { Projekty = db.projectEntities.ToList(),LoggedUser=osoba };
            return View("Index",viewModel);
        }

        // GET: Projects/FinalClaimProject/5
        [Authorize(Roles = "Administrator,Student")]
        public ActionResult FinalClaimProject(int? UserId, int? ProjectId)
        {
            if (UserId == null||ProjectId==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectDetails pd = new ProjectDetails();
            Project proj = db.projectEntities.First(e => e.ProjectId == ProjectId);
            pd.Projekt = proj;
            Osoba osoba = db.osobaEntities.First(e => e.OsobaId == UserId);
            osoba.ProjektClaimed = pd;
            db.Entry(osoba).State = EntityState.Modified;
            db.SaveChanges();

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
        [Authorize(Roles = "Administrator,Student")]
        public ActionResult DisclaimProject(int? UserId, int? ProjectId)
        {
            if (UserId == null || ProjectId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //db.Database.Log = s => System.Diagnostics.Debug.WriteLine(s);
            ProjectDetails pd = db.ProjectDetails.First(e => e.Projekt.ProjectId == ProjectId);
            Osoba osoba = db.osobaEntities.First(e => e.OsobaId == UserId);
            osoba.ProjektClaimed = null;
            db.Entry(osoba).Reference(e => e.ProjektClaimed).CurrentValue = null;
            db.Entry(osoba).State = EntityState.Modified;
            db.SaveChanges();
            db.ProjectDetails.Attach(pd);
            db.ProjectDetails.Remove(pd);
            db.Entry(pd).State = EntityState.Deleted;
            db.SaveChanges();
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
        //Wypisanie sie z projektu
        //GET: ...
        [Authorize(Roles = "Administrator,Student")]
        public ActionResult DisclaimNonFinalProject(int? UserId,int? ProjectId)
        {
            if (UserId == null || ProjectId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Osoba osoba = db.osobaEntities.First(e => e.OsobaId == UserId);
            Project proj = db.projectEntities.First(e => e.ProjectId == ProjectId);
            osoba.NonClaimedProjekts.Remove(proj);
            db.SaveChanges();
            ProjectViewModel viewModel = new ProjectViewModel { Projekty = db.projectEntities.ToList(), LoggedUser = osoba };
            return View("Index",viewModel);
        }
    }
}
