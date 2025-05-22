using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GeniosyFiguras.Dtos;
using GeniosyFiguras.Repositories.Models;
using GeniosyFiguras.Services;


namespace GeniosyFiguras.Controllers
{
    

    public class EnemigoController : Controller
    {
        private readonly EnemigoServicio _enemigoServicio = new EnemigoServicio();

        public EnemigoController()
        {
            _enemigoServicio = new EnemigoServicio();
        }
        [HttpGet]
        public ActionResult NuevoEnemigo()
        {
            
            var modelo = new EnemigoAtributo
            {
                Enemigo = new EnemigoDto(),
                Atributos = new Atributo_Poder()
            };

            return View(modelo);
        }
        [HttpPost]
        public ActionResult NuevoEnemigo(EnemigoAtributo model)
        {
            if (string.IsNullOrWhiteSpace(model.Enemigo.Nombre))
            {
                ModelState.AddModelError("Enemigo.Nombre", "El campo Nombre está vacío o nulo.");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            _enemigoServicio.Crear(model);
            return RedirectToAction("IndexEnemigo");
        }
        public ActionResult IndexEnemigo()
        {
            var enemigos = _enemigoServicio.ObtenerTodos();
            return View(enemigos);
        }



    }
}