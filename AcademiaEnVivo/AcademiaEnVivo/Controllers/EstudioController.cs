using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AcademiaEnVivo.Controllers
{
    public class EstudioController : Controller
    {
        // GET: Estudio
       
        public ActionResult Room()
        {
            ViewBag.room = "1";
            return View();
        }
    }
}