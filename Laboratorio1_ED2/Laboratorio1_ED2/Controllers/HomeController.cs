using Laboratorio1_ED2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Laboratorio1_ED2.Controllers;
using Laboratorio1_ED2.Helpers;
using System.IO;

namespace Laboratorio1_ED2.Controllers
{
    public class HomeController : Controller
    {


        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        //Get SubirArchivo
        public ActionResult SubirArchivo()
        {

            return View();
        }
        //Post SubirArchivo


        [HttpPost]
        public ActionResult SubirArchivo(HttpPostedFileBase file)
        {
            var fileName = Path.GetFileName(file.FileName);//obtenemos el nombre del archivo a cargar
            file.SaveAs(Server.MapPath(@"~\Uploads\" + fileName));//guardamos el archivo en la ruta física que corresponde a la ruta virtual del archivo
            string filePath = string.Empty;
            if (file != null)
            {
                string NuevaRuta = "";
                string path = Server.MapPath("~/Uploads");
                string[] Direccion = path.Split('\\');
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            for (var i = 0; i < Direccion.Length; i++)
                {
                    NuevaRuta += Direccion[i] + "/";
                }
                filePath = NuevaRuta + Path.GetFileName(file.FileName);
                Data.Instancia.LecturaArchivo(filePath);
        }
            return View();
        }
    }
}