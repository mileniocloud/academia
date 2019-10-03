using AcademiaEnVivo.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Web;
using System.Web.Mvc;

namespace AcademiaEnVivo.Controllers
{
    public class EstudioController : Controller
    {
        private academiaenvivoEntities db = new academiaenvivoEntities();
        ResourceManager rm = new ResourceManager(typeof(AcademiaEnVivo.Resource));

        // GET: Estudio

        public ActionResult Room()
        {
            var re = Request;
            var headers = re.Headers;

            Guid code = Guid.Parse(Request["tk"]);
            ViewBag.otro = "otra vaina";

            Invitado inv = db.Invitado.Where(i => i.id_invitado == code).SingleOrDefault();
            DateTime hd = DateTime.Today.AddHours(inv.Evento.hora_desde.Hour).AddMinutes(inv.Evento.hora_desde.Minute);
            DateTime hh = DateTime.Today.AddHours(inv.Evento.hora_hasta.Hour).AddMinutes(inv.Evento.hora_hasta.Minute);
            if (inv != null)
            {
                //se valida si el evento es hoy
                if (inv.Evento.fecha_evento.Date == DateTime.Today.Date)
                {

                    //validamos que el evento esta dentro del rango de horas
                    if (hd <= DateTime.Now && hh > DateTime.Now)
                    {

                        ViewBag.room = inv.Evento.id_evento;
                        ViewBag.displayname = inv.email;
                        //ViewBag.disable = "disable";
                        //ViewBag.enabled = "enabled";

                        ViewBag.disable = "enabled";
                        ViewBag.enabled = "disable";
                    }
                    else
                    {
                        //la hora de inicio del evento aun no inicia
                        if (hd > DateTime.Now)
                            return RedirectToAction("Index", "Screen", new { msj = "evant_h", tk = code });

                        //validamos que el evento era hoy pero ya la hora "hasta" se paso
                        if (hh < DateTime.Now)
                            return RedirectToAction("Index", "Screen", new { msj = "evde", tk = code });
                    }
                }
                else
                {
                    string tipo_msj = string.Empty;

                    //se valida si el evento aun no inicia
                    if (inv.Evento.fecha_evento.Date > DateTime.Today.Date)
                        tipo_msj = "evant";


                    //se valida si la fecha del evento ya paso
                    if (inv.Evento.fecha_evento.Date < DateTime.Today.Date)
                        tipo_msj = "evde";

                    return RedirectToAction("Index", "Screen", new { msj = tipo_msj, tk = code });
                }
            }

            return View();
        }
    }
}