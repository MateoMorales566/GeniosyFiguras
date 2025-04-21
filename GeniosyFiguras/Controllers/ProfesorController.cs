using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GeniosyFiguras.Controllers
{
    public class ProfesorController : Controller
    {
        // GET: Profesor
        public ActionResult Buscar(string nombre)
        {
            var nomsalida = Server.HtmlEncode(nombre);

            return Content(nomsalida);
        }
        public ActionResult IndexProfesor()
        {
            return View();
        }
    }
}