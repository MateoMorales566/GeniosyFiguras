using GeniosyFiguras.Dtos;
using GeniosyFiguras.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GeniosyFiguras.Controllers
{
    public class EstudianteController : Controller
    {
        private readonly UsuarioServicio _usuarioServicio = new UsuarioServicio();
        private readonly CursoServicio _cursoServicio = new CursoServicio();
        

        public EstudianteController()
        {
            _usuarioServicio = new UsuarioServicio();
        }

        public ActionResult IndexEstudiante(int id)
        {
            ViewBag.IdCurso = id;
            var estudiantesConNotas = _usuarioServicio.ObtenerUsuariosPorCursoConNotas(id);
            return View(estudiantesConNotas);
        }

        /*public ActionResult NuevoEstudiante(int idCurso)
        {
            var estudiantes = _usuarioServicio.ObtenerTodos();

            ViewBag.Estudiantes = estudiantes.Select(e => new {
                IdUsuario = e.IdUsuario,
                NombreCompleto = $"{e.Nombres} {e.Apellidos}"
            }).ToList();

            return View(new UsuarioCurso { IdCurso = idCurso });
        }


        [HttpPost]
        public ActionResult NuevoEstudiante(UsuarioCurso modelo)
        {
            if (ModelState.IsValid)
            {
                
                _cursoServicio.AsignarEstudianteACurso(modelo.IdUsuario, modelo.IdCurso);
                return RedirectToAction("IndexEstudiante", new { id = modelo.IdCurso });
            }
            return View(modelo);
        }*/
        [HttpGet]
        public ActionResult NuevoEstudiante(int id)
        {
            ViewBag.IdCurso = id;
            return View();
        }

        [HttpPost]
        public ActionResult NuevoEstudiante(UsuarioConCalificacionDto modelo)
        {
            modelo.Usuario.IdRol = 3; // Asegúrate que siempre tenga el rol estudiante

            _usuarioServicio.CrearEstudianteConCalificaciones(modelo);

            return RedirectToAction("IndexEstudiante", "Estudiante", new { id = modelo.IdCurso });
        }

        public ActionResult EditarEstudiante(int idUsuario, int idCurso)
        {
            var estudiante = _usuarioServicio.ObtenerUsuarioConNotas(idUsuario);
            estudiante.IdCurso = idCurso; 
            return View(estudiante);
        }


        [HttpPost]
        public ActionResult EditarEstudiante(UsuarioConCalificacionDto estudianteActualizado)
        {
            _usuarioServicio.ActualizarUsuarioConNotas(estudianteActualizado);
            return RedirectToAction("IndexEstudiante", new { id = estudianteActualizado.IdCurso });
        }

        public ActionResult EliminarEstudiante(int id, int idCurso)
        {
            _usuarioServicio.EliminarUsuarioYNotas(id);
            return RedirectToAction("IndexEstudiante", new { id = idCurso });
        }




    }

}