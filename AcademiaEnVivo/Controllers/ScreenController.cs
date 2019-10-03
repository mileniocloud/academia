using AcademiaEnVivo.DAL;
using System;
using System.Linq;
using System.Resources;
using System.Web.Mvc;

namespace AcademiaEnVivo.Controllers
{
    public class ScreenController : Controller
    {
        private academiaenvivoEntities db = new academiaenvivoEntities();
        ResourceManager rm = new ResourceManager(typeof(AcademiaEnVivo.Resource));
        // GET: Screen
        public ActionResult Index(string msj, Guid tk)
        {
            Invitado inv = db.Invitado.Where(i => i.id_invitado == tk).SingleOrDefault();

            string mensaje = string.Empty;
            string fecha = inv.Evento.fecha_evento.ToShortDateString();
            string hd = inv.Evento.hora_desde.ToShortTimeString();
            string hh = inv.Evento.hora_hasta.ToShortTimeString();

            string hora = string.Empty;

            //evento aun no es la fecha de inicio
            if (msj == "evant")
                hora = hd + " a " + hh;

            //evento ya paso la fecha o la hora
            if (msj == "evde")
                hora = hh;

            //evento es hoy pero aun no inicia
            if (msj == "evant_h")
                hora = hd;

            mensaje = string.Format(rm.GetString(msj), fecha, hora);

            ViewBag.mensaje = mensaje;
            return View();
        }
    }
}