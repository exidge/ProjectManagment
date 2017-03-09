using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZarzadzanieProjektem.DAL;
using ZarzadzanieProjektem.Models;

namespace ZarzadzanieProjektem.Controllers
{
    public class DokumentacjaController : Controller
    {
        private ProjectContext db = new ProjectContext();
        // GET: Dokumentacja
        [Authorize(Roles = "Administrator,Student")]
        public ActionResult Index()
        {
            return View();
        }
        [Authorize(Roles = "Administrator,Student")]
        public ActionResult Download(int? Id)
        {
            Dokumentacja doc = db.Dokumentacjas.First(e => e.DokumentacjaId == Id);
            db.Entry(doc).Reference("Plik").Load();
            FileContentResult res= new FileContentResult(doc.Plik.Plik, MimeMapping.GetMimeMapping(doc.NazwaPliku));
            res.FileDownloadName = doc.NazwaPliku;
            return res;
        }
    }
}