using Laboratorio1_ED2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Laboratorio1_ED2.Controllers;
using Laboratorio1_ED2.Helpers;
using System.IO;
using Laboratorio1_ED2.HuffmanTree;

namespace Laboratorio1_ED2.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        //LISTADO DE COMPRESION CON DATOS
        public ActionResult Listado()
        {
            return View("");
        }

        //HUFFMAN
        public ActionResult SubirArchivo()
        {

            return View();
        }

        [HttpPost]
        public ActionResult SubirArchivo(HttpPostedFileBase file)
        {

            var fileName = Path.GetFileName(file.FileName);//Nombre del archivo a cargar
            file.SaveAs(Server.MapPath(@"~\Uploads\" + fileName));//Guardado del archivo en la ruta física 
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
                Data.Instancia.LecturaArchivo(filePath, fileName, path, 0);
            }
            return View();
        }

        public ActionResult DescompresionArchivo()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DescompresionArchivo(HttpPostedFileBase file)
        {
            file = Request.Files.Get(0);
            var allowedExtensions = new string[] { ".huff" };
            string extension = Path.GetExtension(file.FileName);
            if (allowedExtensions.Contains(extension))
            {
                var fileName = Path.GetFileName(file.FileName);//obtenemos el nombre del archivo a cargar
                //file.SaveAs(Server.MapPath(@"~\Uploads\" + fileName));//guardamos el archivo en la ruta física que corresponde a la ruta virtual del archivo
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
                    Data.Instancia.LecturaArchivo(filePath, fileName, path, 1); //lee
                }
                return View("DescompresionArchivo");
            }
            else
            {
                    return View("SubirArchivo");   
            }
        }


        //LZW
        public ActionResult SubirLZW()
        {

            return View();
        }

        [HttpPost]
        public ActionResult SubirLZW(HttpPostedFileBase file)
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
               // MANDAR A LEER CON LZW
            }
            return View();
        }

        public ActionResult DescompresionLZW()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DescompresionLZW(HttpPostedFileBase file)
        {
            file = Request.Files.Get(0);
            var allowedExtensions = new string[] { ".huff" };
            string extension = Path.GetExtension(file.FileName);
            if (allowedExtensions.Contains(extension))
            {
                var fileName = Path.GetFileName(file.FileName);
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
                   //MANDAR A LEER TIPO LZW
                }
                return View("DescompresionLZW");
            }
            else
            {
                return View("SubirLZW");
            }
        }

        //DOWNLOAD
        public FileResult Download()
        {
            string examplePathToFile = Server.MapPath("~/Downloads/");
            string exampleMimeType = "*.*";
            return new FileStreamResult(new FileStream(examplePathToFile, FileMode.Open, FileAccess.Read), exampleMimeType);
        }
    }
}

