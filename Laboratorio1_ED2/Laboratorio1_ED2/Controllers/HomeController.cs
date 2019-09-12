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
        
            
        [HttpPost]
        public ActionResult SubirArchivo(HttpPostedFileBase file)
        {
            FileInfo Archivo = new FileInfo("C:\\miArchivo.txt");
            var f = Archivo.Length;
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
                Data.Instancia.LecturaArchivo(filePath,fileName,path,f);
            }
            return View();
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