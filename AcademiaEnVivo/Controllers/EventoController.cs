using AcademiaEnVivo.DAL;
using AcademiaEnVivo.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Resources;
using System.Web.Mvc;
using System.Web;
using System.Globalization;
using System.Net.Mime;

namespace AcademiaEnVivo.Controllers
{
    public class EventoController : Controller
    {
        private academiaenvivoEntities db = new academiaenvivoEntities();
        ResourceManager rm = new ResourceManager(typeof(AcademiaEnVivo.Resource));
        // GET: Evento
        public ActionResult Index()
        {

            List<EventoModels> el = db.Evento.Select(u => new EventoModels
            {
                id_evento = u.id_evento,
                nombre_categoria = u.CategoriaCurso.nombre,
                nombre = u.nombre,
                descripcion = u.descripcion,
                fecha_evento = u.fecha_evento,
                hora_desde = u.hora_desde,
                hora_hasta = u.hora_hasta,
                precio_decimal = u.precio,
                estado = u.estado,
                estado_string = "",
                duracion = u.duracion
            }).ToList();
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
            EventoModels ev = new EventoModels();
            ev.fecha_evento = DateTime.Today;
            ev.hora_desde_string = "8:00 a. m.";
            ev.hora_hasta_string = "8:00 a. m.";
            ViewBag.id_categoria = new SelectList(db.CategoriaCurso, "id_categoria", "nombre").OrderBy(o => o.Text);
            return View(ev);
        }

        // POST: Evento/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_evento,id_categoria,nombre,descripcion,fecha_evento,hora_desde,hora_hasta,duracion,precio,invitados,estado,fecha_creado")] EventoModels evento_)
        {
            try
            {
                DateTime hd = evento_.hora_desde;
                DateTime hh = evento_.hora_hasta;
                decimal duracion = decimal.Parse((hh - hd).TotalHours.ToString());

                evento_.hora_desde_string = evento_.hora_desde.ToShortTimeString();
                evento_.hora_hasta_string = evento_.hora_hasta.ToShortTimeString();

                if (ModelState.IsValid)
                {
                    //validamos los emails
                    bool emailerror = false;
                    foreach (var i in evento_.invitados.Split(','))
                    {
                        if (!IsValidEmail(i.Trim()))
                        {
                            emailerror = true;
                            break;
                        }
                    }


                    if (hh > hd)
                    {
                        evento_.fecha_evento = evento_.fecha_evento.AddHours(1);

                        List<Evento> lev = db.Evento.Where(e => e.fecha_evento == evento_.fecha_evento && e.estado == true
                                                && ((e.hora_desde.Hour < hd.Hour && e.hora_hasta.Hour > hd.Hour)
                                                || (e.hora_desde.Hour < hh.Hour && e.hora_hasta.Hour > hh.Hour)
                                                || (e.hora_desde.Hour >= hd.Hour && e.hora_hasta.Hour <= hh.Hour))
                                                ).ToList();
                        if (lev.Count() == 0)
                        {
                            if (emailerror != true)
                            {
                                if ((duracion % 1) == 0)
                                {
                                    //si llega hasta aqui es que no hay validaciones fallidas
                                    Evento ev = new Evento();
                                    ev.id_evento = Guid.NewGuid();
                                    ev.id_categoria = evento_.id_categoria;
                                    ev.nombre = evento_.nombre;
                                    ev.descripcion = evento_.descripcion;
                                    ev.fecha_evento = evento_.fecha_evento;
                                    ev.hora_desde = hd;
                                    ev.hora_hasta = hh;
                                    ev.precio = double.Parse(evento_.precio);
                                    ev.duracion = int.Parse(duracion.ToString());
                                    ev.invitados = evento_.invitados;
                                    ev.estado = evento_.estado;

                                    ev.fecha_creado = DateTime.Now;
                                    db.Evento.Add(ev);

                                    List<Invitado> liv = new List<Invitado>();
                                    foreach (var i in evento_.invitados.Split(','))
                                    {
                                        Invitado iv = new Invitado();
                                        iv.id_invitado = Guid.NewGuid();
                                        iv.email = i;
                                        iv.id_evento = ev.id_evento;
                                        iv.asistio = false;
                                        iv.codigo_asistencia = Guid.NewGuid();
                                        iv.ip = string.Empty;

                                        liv.Add(iv);

                                    }
                                    if (liv.Count() != 0)
                                        db.Invitado.AddRange(liv);

                                    db.SaveChanges();

                                    foreach (var e in liv)
                                    {
                                        //DESPUES DE CREAR EL EVENTO SE ENVIAN LAS INVITACIONES
                                        SendInvitation(e.email, ev.nombre, ev.fecha_evento, ev.descripcion, ev.id_evento.ToString(), ev.hora_desde.ToShortTimeString(), ev.hora_hasta.ToShortTimeString(), e);
                                    }

                                    evento_.hora_desde_string = "8:00 a. m.";
                                    evento_.hora_hasta_string = "8:00 a. m.";

                                    ev.fecha_evento = DateTime.Today;
                                    evento_.ShowError = true;
                                    evento_.Clean = true;
                                    evento_.type = TypeError.success;
                                    evento_.title = Titles.Exito;
                                    evento_.ErrorMessage = rm.GetString("save");
                                }
                                else
                                {
                                    //si algun email esta mal formateado
                                    evento_.ShowError = true;
                                    evento_.Clean = false;
                                    evento_.type = TypeError.warning;
                                    evento_.title = Titles.Atención;
                                    evento_.ErrorMessage = rm.GetString("horas_Completas");
                                }

                            }
                            else
                            {
                                //si algun email esta mal formateado
                                evento_.ShowError = true;
                                evento_.Clean = false;
                                evento_.type = TypeError.warning;
                                evento_.title = Titles.Atención;
                                evento_.ErrorMessage = rm.GetString("mail_error");
                            }
                        }
                        else
                        {
                            //si no coincide ninguna fecha con horas
                            evento_.ShowError = true;
                            evento_.Clean = false;
                            evento_.type = TypeError.error;
                            evento_.title = Titles.Error;
                            evento_.ErrorMessage = rm.GetString("horario_error");
                        }
                    }
                    else
                    {
                        //si las horas son iguales o la hora desde es mayor que la hora hasta
                        evento_.ShowError = true;
                        evento_.Clean = false;
                        evento_.type = TypeError.error;
                        evento_.title = Titles.Error;
                        evento_.ErrorMessage = rm.GetString("hora_error");
                    }
                }
            }
            catch (Exception ex)
            {
                evento_.ShowError = true;
                evento_.type = TypeError.error;
                evento_.ErrorMessage = ex.Message;
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

            EventoModels em = new EventoModels();
            em.id_evento = id.Value;
            em.id_categoria = evento.id_categoria;
            em.nombre = evento.nombre;
            em.descripcion = evento.descripcion;
            em.fecha_evento = evento.fecha_evento;
            em.hora_desde = evento.hora_desde;
            em.hora_hasta = evento.hora_hasta;
            //em.invitados = evento.invitados;
            em.estado = evento.estado;
            em.precio = evento.precio.ToString();
            em.hora_desde_string = evento.hora_desde.ToShortTimeString();
            em.hora_hasta_string = evento.hora_hasta.ToShortTimeString();

            em.lista_invitados = evento.Invitado.Select(i => new BasicModel
            {
                id = i.id_invitado,
                value = i.email
            }).ToList();

            if (evento == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_categoria = new SelectList(db.CategoriaCurso, "id_categoria", "nombre", evento.id_categoria);
            return View(em);
        }

        // POST: Evento/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_evento,id_categoria,nombre,descripcion,fecha_evento,hora_desde,hora_hasta,duracion,precio,estado,fecha_creado")] EventoModels evento_)
        {
            try
            {
                DateTime hd = evento_.hora_desde;
                DateTime hh = evento_.hora_hasta;
                decimal duracion = decimal.Parse((hh - hd).TotalHours.ToString());

                evento_.hora_desde_string = evento_.hora_desde.ToShortTimeString();
                evento_.hora_hasta_string = evento_.hora_hasta.ToShortTimeString();
                //evento_.invitados = "Invitados@leche.com";

                if (ModelState.IsValid)
                {
                    if (hh > hd)
                    {
                        evento_.fecha_evento = evento_.fecha_evento.AddHours(1);

                        List<Evento> lev = db.Evento.Where(e => e.id_evento != evento_.id_evento &&
                                                e.fecha_evento == evento_.fecha_evento
                                                && e.estado == true
                                                && ((e.hora_desde.Hour < hd.Hour && e.hora_hasta.Hour > hd.Hour)
                                                || (e.hora_desde.Hour < hh.Hour && e.hora_hasta.Hour > hh.Hour)
                                                || (e.hora_desde.Hour >= hd.Hour && e.hora_hasta.Hour <= hh.Hour))
                                                ).ToList();
                        if (lev.Count() == 0)
                        {
                            if ((duracion % 1) == 0)
                            {
                                //si llega hasta aqui es que no hay validaciones fallidas
                                Evento ev = db.Evento.Where(t => t.id_evento == evento_.id_evento).SingleOrDefault();
                                ev.id_categoria = evento_.id_categoria;
                                ev.nombre = evento_.nombre;
                                ev.descripcion = evento_.descripcion;
                                ev.fecha_evento = evento_.fecha_evento;
                                ev.hora_desde = hd;
                                ev.hora_hasta = hh;
                                ev.precio = double.Parse(evento_.precio);
                                ev.duracion = int.Parse(duracion.ToString());
                                ev.estado = evento_.estado;

                                db.SaveChanges();

                                //evento_.hora_desde_string = "";
                                //evento_.hora_hasta_string = "";

                                evento_.ShowError = true;
                                evento_.Clean = false;
                                evento_.type = TypeError.success;
                                evento_.title = Titles.Exito;
                                evento_.ErrorMessage = rm.GetString("edit");
                            }
                            else
                            {
                                //si algun email esta mal formateado
                                evento_.ShowError = true;
                                evento_.Clean = false;
                                evento_.type = TypeError.warning;
                                evento_.title = Titles.Atención;
                                evento_.ErrorMessage = rm.GetString("horas_Completas");
                            }
                        }
                        else
                        {
                            //si no coincide ninguna fecha con horas
                            evento_.ShowError = true;
                            evento_.Clean = false;
                            evento_.type = TypeError.error;
                            evento_.title = Titles.Error;
                            evento_.ErrorMessage = rm.GetString("horario_error");
                        }
                    }
                    else
                    {
                        //si las horas son iguales o la hora desde es mayor que la hora hasta
                        evento_.ShowError = true;
                        evento_.Clean = false;
                        evento_.type = TypeError.error;
                        evento_.title = Titles.Error;
                        evento_.ErrorMessage = rm.GetString("hora_error");
                    }
                }
            }
            catch (Exception ex)
            {
                evento_.ShowError = true;
                evento_.type = TypeError.error;
                evento_.ErrorMessage = ex.Message;
            }
            ViewBag.id_categoria = new SelectList(db.CategoriaCurso, "id_categoria", "nombre", evento_.id_categoria);

            evento_.lista_invitados = db.Invitado.Where(t => t.id_evento == evento_.id_evento).Select(i => new BasicModel
            {
                id = i.id_invitado,
                value = i.email
            }).ToList();

            return View(evento_);
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

        public ActionResult DeleteInvitado(Guid id)
        {
            Invitado iv = db.Invitado.Where(i => i.id_invitado == id).SingleOrDefault();

            if (iv != null)
            {
                db.Invitado.Remove(iv);
                db.SaveChanges();
            }

            return RedirectToAction("Edit", new { id = iv.id_evento });
        }

        public ActionResult AddInvitado(EventoModels evento_)
        {

            if (!string.IsNullOrEmpty(evento_.invitados))
            {
                //validamos los emails
                bool emailerror = false;
                foreach (var i in evento_.invitados.Split(','))
                {
                    if (!IsValidEmail(i.Trim()))
                    {
                        emailerror = true;
                        break;
                    }
                }
                if (emailerror != true)
                {
                    List<Invitado> liv = new List<Invitado>();
                    Evento ev = db.Evento.Where(r => r.id_evento == evento_.id_evento).SingleOrDefault();
                    foreach (var i in evento_.invitados.Split(','))
                    {
                        Invitado iv = new Invitado();
                        iv.id_invitado = Guid.NewGuid();
                        iv.email = i;
                        iv.id_evento = evento_.id_evento;
                        iv.asistio = false;
                        iv.codigo_asistencia = Guid.NewGuid();
                        iv.ip = string.Empty;

                        liv.Add(iv);

                    }
                    if (liv.Count() != 0)
                    {
                        db.Invitado.AddRange(liv);

                        db.SaveChanges();
                    }

                    foreach (var e in liv)
                    {
                        //DESPUES DE CREAR EL EVENTO SE ENVIAN LAS INVITACIONES
                        SendInvitation(e.email, ev.nombre, ev.fecha_evento, ev.descripcion, ev.id_evento.ToString(), ev.hora_desde.ToShortTimeString(), ev.hora_hasta.ToShortTimeString(), e);
                    }
                }
                else
                {
                    //si algun email esta mal formateado
                    evento_.ShowError = true;
                    evento_.Clean = false;
                    evento_.type = TypeError.warning;
                    evento_.title = Titles.Atención;
                    evento_.ErrorMessage = rm.GetString("mail_error");
                }
            }
            return RedirectToAction("Edit", new { id = evento_.id_evento });
        }

        public bool Invitar(Guid id)
        {
            try
            {
                Invitado inv = db.Invitado.Where(r => r.id_invitado == id).SingleOrDefault();

                if (inv != null)
                {
                    SendInvitation(inv.email, inv.Evento.nombre, inv.Evento.fecha_evento, inv.Evento.descripcion, inv.Evento.id_evento.ToString(), inv.Evento.hora_desde.ToShortTimeString(), inv.Evento.hora_hasta.ToShortTimeString(), inv);
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
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

        bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private void SendInvitation(string toAddress, string evento, DateTime fecha, string descipcion, string idevento, string hd, string hh, Invitado inv)
        {
            SendMail(toAddress, SetEmailBody(evento, fecha, descipcion, idevento, hd, hh, inv), ConfigurationManager.AppSettings["EmailSubjec"]);
        }

        public bool SendMail(string toAddress, AlternateView emailbody, string Subject)
        {
            try
            {
                var userCredentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["SMTPUserName"].ToString(), ConfigurationManager.AppSettings["SMTPPassword"]);

                SmtpClient smtp = new SmtpClient
                {
                    Host = Convert.ToString(ConfigurationManager.AppSettings["SMTPHost"]),
                    Port = Convert.ToInt32(ConfigurationManager.AppSettings["SMTPPort"]),
                    EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["SMTPEnableSsl"]),
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Timeout = Convert.ToInt32(ConfigurationManager.AppSettings["SMTPTimeout"])
                };
                smtp.Credentials = userCredentials;

                MailMessage message = new MailMessage();
                message.From = new MailAddress(ConfigurationManager.AppSettings["SenderEmailAddress"], ConfigurationManager.AppSettings["SenderDisplayName"]);
                message.Subject = Subject;
                message.IsBodyHtml = true;
                message.AlternateViews.Add(emailbody);
                message.IsBodyHtml = true;
                //System.Net.Mail.Attachment attachment = new System.Net.Mail.Attachment(path);
                //message.Attachments.Add(attachment);

                //message.To.Add(email);
                foreach (var m in toAddress.Split(','))
                {
                    message.To.Add(m);
                };

                smtp.Send(message);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private AlternateView SetEmailBody(string evento, DateTime fecha, string descripcion, string idevento, string hd, string hh, Invitado inv)
        {
            try
            {
                //se arma el correo que se envia para el ambio de clave
                string plantilla = HttpContext.Server.MapPath(ConfigurationManager.AppSettings["InvitationTemplate"]);
                string url = ConfigurationManager.AppSettings["urlevento"];

                url = url + "?tk=" + inv.id_invitado;
                var html = System.IO.File.ReadAllText(plantilla);
                html = html.Replace("{{event}}", evento);
                html = html.Replace("{{mes}}", fecha.Day.ToString() + " de " + fecha.ToString("MMMM", CultureInfo.CreateSpecificCulture("es")).ToUpper());
                html = html.Replace("{{dia}}", hd + " - " + hh);
                html = html.Replace("{{descripcion}}", descripcion);
                html = html.Replace("{{action_url}}", url);

                AlternateView av = AlternateView.CreateAlternateViewFromString(html, null, "text/html");

                //create the LinkedResource(embedded image)
                //LinkedResource logo = new LinkedResource(HttpContext.Server.MapPath("..\\Content\\MailTemplates\\images\\9701519718227204.jpg"));
                //logo.ContentId = "fondo";

                //av.LinkedResources.Add(logo);

                return av;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
