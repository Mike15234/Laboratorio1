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

        //Get SubirArchivo
        public ActionResult SubirArchivo()
        {

            return View();
        }
        //Post SubirArchivo
        
        public ActionResult Listado()
        {
            return View();
        }

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
                Data.Instancia.LecturaArchivo(filePath, fileName, path, 0);
            }
            return View();
        }

        public ActionResult DescompresionArchivo()
        {
            return View();
        }
        Arbol Arbolitu = new Arbol();
        [HttpPost]
        public ActionResult DescompresionArchivo(HttpPostedFileBase file)
        {
            file = Request.Files.Get(0);
            var allowedExtensions = new string[] { ".huff" };
            string extension = Path.GetExtension(file.FileName);
            if (allowedExtensions.Contains(extension))
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
                    Arbolitu.Desifrado(filePath);
                }
                return View("DescompresionArchivo");
            }
            else
            {
              
                    return View("SubirArchivo");
                
            }
        }
    
        

        //DOWNLOAD
        public ActionResult Download()
        {
            string path = Server.MapPath("~/Descargas");
            DirectoryInfo dirInfo = new DirectoryInfo(path);
            FileInfo[] files = dirInfo.GetFiles("*.*");
            List<string> listaDescargas = new List<string>(files.Length);
            foreach (var item in files)
            {
                listaDescargas.Add(item.Name);
            }
            return View(listaDescargas);
        }
        //HAY QUE CAMBIAR PARA QUE NO SEA SOLO IAMGENES, SINO CUALQUIERA Y QUE SEA LA LISTA DE NUESTROS ARCHIVOS GUARDADOS NADA MAS
        //pero no recuerdo como dijo godoy que era gg
        public ActionResult DownloadFile (string filename)
        {
            if (Path.GetExtension(filename) == ".png")
            {
                string fullpath = Path.Combine(Server.MapPath("~/Descargas"), filename);
                return File(fullpath, "Images/png");
            }
            else
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.Forbidden);
            }
        }
    }
}

