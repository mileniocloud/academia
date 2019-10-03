using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AcademiaEnVivo.DAL;
using AcademiaEnVivo.Models;

namespace AcademiaEnVivo.Controllers
{
    public class EventoController : Controller
    {
        private academiaenvivoEntities db = new academiaenvivoEntities();

        // GET: Evento
        public ActionResult Index()
        {

            List<EventoModels> el = db.Evento.Select(u => new EventoModels
            {
                nombre_categoria = u.CategoriaCurso.nombre,
                nombre = u.nombre,
                descripcion = u.descripcion,
                fecha_evento = u.fecha_evento,
                hora_desde = u.hora_desde,
                hora_hasta = u.hora_hasta,
                precio = u.precio,
                estado = u.estado,
                estado_string = "",
                duracion = u.duracion
            }).ToList();

            var evento = db.Evento.Include(e => e.CategoriaCurso);
            return View(el);
        }

        // GET: Evento/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Evento evento = db.Evento.Find(id);
            if (evento == null)
            {
                return HttpNotFound();
            }
            return View(evento);
        }

        // GET: Evento/Create
        public ActionResult Create()
        {
            ViewBag.id_categoria = new SelectList(db.CategoriaCurso, "id_categoria", "nombre").OrderBy(o => o.Text);
            return View();
        }

        // POST: Evento/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_evento,id_categoria,nombre,descripcion,fecha_evento,hora_desde,hora_hasta,duracion,precio,invitados,estado,fecha_creado")] EventoModels evento_)
        {
            DateTime hd = DateTime.Parse(evento_.hora_desde);
            DateTime hh = DateTime.Parse(evento_.hora_hasta);
            int duracion = int.Parse((hh - hd).TotalHours.ToString());

            if (ModelState.IsValid)
            {
                Evento ev = new Evento();
                ev.id_evento = Guid.NewGuid();
                ev.id_categoria = evento_.id_categoria;
                ev.nombre = evento_.nombre;
                ev.descripcion = evento_.descripcion;
                ev.fecha_evento = evento_.fecha_evento;
                ev.hora_desde = evento_.hora_desde;
                ev.hora_hasta = evento_.hora_hasta;
                ev.precio = evento_.precio;
                ev.duracion = duracion;
                ev.invitados = evento_.invitados;

                ev.fecha_creado = DateTime.Now;
                db.Evento.Add(ev);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.id_categoria = new SelectList(db.CategoriaCurso, "id_categoria", "nombre", evento_.id_categoria);
            return View(evento_);
        }

        // GET: Evento/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Evento evento = db.Evento.Find(id);
            if (evento == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_categoria = new SelectList(db.CategoriaCurso, "id_categoria", "nombre", evento.id_categoria);
            return View(evento);
        }

        // POST: Evento/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_evento,id_categoria,nombre,descripcion,fecha_evento,hora_desde,hora_hasta,duracion,precio,invitados,estado,fecha_creado")] Evento evento)
        {
            if (ModelState.IsValid)
            {
                db.Entry(evento).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_categoria = new SelectList(db.CategoriaCurso, "id_categoria", "nombre", evento.id_categoria);
            return View(evento);
        }

        // GET: Evento/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Evento evento = db.Evento.Find(id);
            if (evento == null)
            {
                return HttpNotFound();
            }
            return View(evento);
        }

        // POST: Evento/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Evento evento = db.Evento.Find(id);
            db.Evento.Remove(evento);
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
    }
}
