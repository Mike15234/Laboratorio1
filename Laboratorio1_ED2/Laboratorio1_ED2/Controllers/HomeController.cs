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
                ////Nuevo = Direccion[0] + "/" + Direccion[1] + "/" + Direccion[2] + "/" + Direccion[3] + "/" + Direccion[4] + "/" + Direccion[5] + "/";
                filePath = NuevaRuta + Path.GetFileName(file.FileName);
                Data.Instancia.LecturaArchivo(filePath);
               //Data.Instancia.Conteo_y_Probabilidad();

            
            
        }
            return View();
        }
}
}

//SubirArchivoModel modelo = new SubirArchivoModel();
//if (file != null)
//{
//    string ruta = Server.MapPath("~/Uploads/");
//    //ruta += file.FileName;
//    string[] direcciones = ruta.Split('\\');
//    string Ruta_archivo = string.Empty;

//    for (int i = 0; i < direcciones.Length; i++)
//    {
//        Ruta_archivo += direcciones[i] + "/";
//    }
//    Ruta_archivo += Path.GetFileName(file.FileName);
//    modelo.SubirArchivo(ruta, file);
//    Data.Instancia.LecturaArchivo(Ruta_archivo);



//    ViewBag.Error = modelo.error;
//    ViewBag.Correcto = modelo.Confirmacion;
