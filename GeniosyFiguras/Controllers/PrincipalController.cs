using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GeniosyFiguras.Dtos;

namespace GeniosyFiguras.Controllers
{
    public class PrincipalController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AcercaDeNosotros()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contacto()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Ejemplo()
        {
            return View();
        }
        public ActionResult Ejemplo1()
        {
            return View();
        }
        public ActionResult InicioSesion(UsuarioDto usuario)
        {
           
            return View("InicioSesion");
        }
        public ActionResult CerrarSesion()
        {
            Session.Clear(); // Borra toda la sesión
            Session.Abandon(); // Marca la sesión como terminada

            return RedirectToAction("InicioSesion", "Principal"); // Redirige a la vista de inicio de sesión
        }
    }
}