using Microsoft.AspNetCore.Mvc;

namespace PL.Controllers
{
    public class CitasController : Controller
    {
        [HttpGet]
        public ActionResult GetAll()
        {
            ML.Cita cita = new ML.Cita();
            ML.Result result = BL.Citas.ConsultarCitas();

            if (result.Correct)
            {
                cita.Citas = result.Objects.Cast<ML.Cita>().ToList();
            }
            else
            {
                ViewBag.Mensaje = result.ErrorMessage;
            }
            return View(cita);
        }

        [HttpGet]
        public ActionResult Form()
        {
            ML.Cita cita = new ML.Cita
            {
                Fecha = DateTime.Now,
                Hora = DateTime.Now.TimeOfDay,
                DuracionMinutos = 30,
                Estatus = "Agendada"
            };
            return View(cita);
        }

        [HttpPost]
        public ActionResult Form(ML.Cita cita)
        {
            ML.Result result = BL.Citas.AddCita(cita);
            ViewBag.Mensaje = result.Correct ? "Cita registrada correctamente" : result.ErrorMessage;
            return RedirectToAction("GetAll");
        }

        public ActionResult Cancelar(int IdCita)
        {
            ML.Result result = BL.Citas.CancelarCita(IdCita);
            ViewBag.Mensaje = result.Correct ? "Cita cancelada correctamente" : result.ErrorMessage;
            return RedirectToAction("GetAll");
        }

        [HttpGet]
        public ActionResult ConsultarPorFecha(DateTime fecha)
        {
            ML.Cita cita = new ML.Cita();
            ML.Result result = BL.Citas.ConsultarByFecha(fecha);

            if (result.Correct)
            {
                cita.Citas = result.Objects.Cast<ML.Cita>().ToList();
            }
            else
            {
                ViewBag.Mensaje = result.ErrorMessage;
            }

            return View("GetAll", cita);
        }

        [HttpPost]
        public JsonResult ActualizarEstatus(int idCita, string estatus)
        {
            ML.Result result = new ML.Result();

            using (DL.CitasAgendaContext context = new DL.CitasAgendaContext())
            {
                var cita = context.Cita.SingleOrDefault(c => c.IdCita == idCita);
                if (cita != null)
                {
                    cita.Estatus = estatus;
                    context.SaveChanges();
                    result.Correct = true;
                }
                else
                {
                    result.Correct = false;
                    result.ErrorMessage = "No se encontró la cita.";
                }
            }

            return Json(result);
        }
    }
}
