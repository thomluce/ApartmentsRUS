using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ApartmentsRUS
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //routes.MapRoute(
            //       name: "Renter",
            //       url: "renter",
            //       defaults: new { controller = "Renters", action = "Index" }
            //       );

            routes.MapRoute(
                    name: "EasyRenter",
                    url: "renter/{action}/{id}",
                    defaults: new { controller = "Renters", action = "Index", id = UrlParameter.Optional },
                    new { id = @"\d+" });

            routes.MapRoute(
                    name: "Apartment",
                    url: "Apartment/{id}",
                    defaults: new { controller = "Apartments", action = "Details" },
                    new { id = @"\d+" });

            routes.MapRoute(
                    name: "RenterList",
                    url: "renter/list/{searchString}",
                    defaults: new { controller = "Renters", action = "Index", searchString = UrlParameter.Optional });

            routes.MapRoute(
                    name: "AnyActionWithID",
                    url: "{controller}-{action}/{id}",
                    defaults: new { controller = "home", action = "index" },
                     new { id = @"\d+" });

            routes.MapRoute(
                    name: "BuildingSearch",
                    url: "BuildingSearch/{state}/{city}",
                    defaults: new { controller = "Buildings", action = "search", city = UrlParameter.Optional, state = UrlParameter.Optional });

            routes.MapRoute(
                    name: "NewLeases",
                    url: "NewLeases/{start}/{end}",
                    defaults: new { controller = "Leases", action = "newleases", start = UrlParameter.Optional, end = UrlParameter.Optional });

            routes.MapRoute(
                    name: "Default",
                    url: "{controller}/{action}/{id}",
                    defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
                        );
        }
    }
}
