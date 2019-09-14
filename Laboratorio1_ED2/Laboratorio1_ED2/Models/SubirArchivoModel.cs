using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Laboratorio1_ED2.Models
{
    public class SubirArchivoModel
    {
        public String Confirmacion { get; set; }
        public Exception error { get; set; }
        public void SubirArchivo(string ruta, HttpPostedFileBase file)
        {
            try
            {
                file.SaveAs(ruta);
                this.Confirmacion = "Archivo Guardado";
            }
            catch (Exception ex)
            {
                this.error = ex;
            }
        }
    }
}