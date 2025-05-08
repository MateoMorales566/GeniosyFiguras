using GeniosyFiguras.Dtos;
using GeniosyFiguras.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GeniosyFiguras.Controllers
{
    public class CursoController : Controller
    {
        /*public ActionResult IndexCurso()
        {
            CursoServicio cursoServicio = new CursoServicio();
            List<CursoDto> cursos = cursoServicio.ObtenerTodos();
            return View(cursos);
        }*/
        public ActionResult IndexCurso(string IdCurso, string NombreCurso)
        {
            string nombreUsuario = Session["UsuarioNombre"] as string;

            if (string.IsNullOrEmpty(nombreUsuario))
            {
                return RedirectToAction("InicioSesion", "Home");
            }

            CursoServicio servicio = new CursoServicio();
            var cursos = servicio.ObtenerCursosPorUsuario(nombreUsuario);

            //cursos = cursos.Where(c => c.UsuarioCreador == nombreUsuario).ToList();

            // Aplicar filtros si vienen parámetros
            if (!string.IsNullOrEmpty(IdCurso) && int.TryParse(IdCurso, out int idCursoInt))
            {
                cursos = cursos.Where(c => c.IdCurso == idCursoInt).ToList();
            }

            if (!string.IsNullOrEmpty(NombreCurso))
            {
                cursos = cursos.Where(c => c.NombreCurso.ToLower().Contains(NombreCurso.ToLower())).ToList();
            }

            return View(cursos);
        }

        // GET: Cursos/NuevoCurso
        [HttpGet]
        public ActionResult NuevoCurso()
        {
            return View(); 
        }

        // POST: Cursos/NuevoCurso


        [HttpPost]
        public ActionResult NuevoCurso(CursoDto curso)
        {
            string usuarioNombre = Session["UsuarioNombre"] as string;

            if (string.IsNullOrEmpty(usuarioNombre))
            {
                return RedirectToAction("InicioSesion", "Home");
            }

            curso.UsuarioCreador = usuarioNombre;

            CursoServicio servicio = new CursoServicio();
            var response = servicio.CrearCurso(curso);

            if (response.Response == 1)
            {
                return RedirectToAction("IndexCurso");
            }

            ViewBag.Error = response.Message;
            return View(curso);
        }
        [HttpGet]
        public ActionResult EditarCurso(int? id)
        {
            string usuarioNombre = Session["UsuarioNombre"] as string;
            if (string.IsNullOrEmpty(usuarioNombre))
            {
                return RedirectToAction("InicioSesion", "Home");
            }

            CursoServicio servicio = new CursoServicio();
            var cursos = servicio.ObtenerCursosPorUsuario(usuarioNombre);
            var curso = cursos.FirstOrDefault(c => c.IdCurso == id);

            if (curso == null)
            {
                ViewBag.Error = "Curso no encontrado.";
                return HttpNotFound();
            }

            return View(curso);
        }


        [HttpPost]
        public ActionResult EditarCurso(CursoDto curso)
        {
            CursoServicio servicio = new CursoServicio();
            var response = servicio.ActualizarCurso(curso);

            if (response.Response == 1)
            {
                return RedirectToAction("IndexCurso");
            }

            ViewBag.Error = response.Message;
            return View(curso);
        }

        [HttpGet]
        public ActionResult EliminarCurso(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("IndexCurso");
            }

            CursoServicio servicio = new CursoServicio();
            var response = servicio.EliminarCurso(id.Value);

            if (response.Response == 1)
            {
                return RedirectToAction("IndexCurso");
            }

            ViewBag.Error = response.Message;
            return RedirectToAction("IndexCurso");
        }
        [HttpGet]
        public ActionResult IndexEstudiante(int id)
        {
            ViewBag.IdCurso = id;

            var servicioUsuario = new UsuarioServicio();
            var estudiantes = servicioUsuario.ObtenerUsuariosPorCursoConNotas(id);

            return View(estudiantes);
        }








    }


}