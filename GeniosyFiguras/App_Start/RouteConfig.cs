﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace GeniosyFiguras
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Principal", action = "Index", id = UrlParameter.Optional }
            );

           
            routes.MapRoute(
                name: "Profesor",
                url: "{controller}/{action}/{nombre}",
                defaults: new { controller = "Profesor", action = "Buscar", nombre = UrlParameter.Optional }
            );
        }
    }

}
