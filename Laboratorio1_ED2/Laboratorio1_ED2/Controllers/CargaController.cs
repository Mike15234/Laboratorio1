﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using Laboratorio1_ED2.Helpers;
using Laboratorio1_ED2.Models;

namespace Laboratorio1_ED2.Controllers
{
    public class CargaController : Controller
    {


        //IFNORA ESTE CONTROLADOR PARA MIENTRAS
    

    ////#region INDEX
    //// GET: Archivo
    //public ActionResult Index(HttpPostedFileBase file)
    //{
    //    string filePath = string.Empty;
    //    if (postedFile != null)
    //    {
    //        string path = Server.MapPath("~/Uploads/");
    //        if (!Directory.Exists(path))
    //        {
    //            Directory.CreateDirectory(path);
    //        }
    //        filePath = path + Path.GetFileName(postedFile.FileName);
    //        string extension = Path.GetExtension(postedFile.FileName);
    //        postedFile.SaveAs(filePath);
    //        string huffData = System.IO.File.ReadAllText(filePath);

    //        if (Data.Instancia.Inicial == true)
    //        {
    //            Data.Instancia.LecturaArchivo(filePath);
    //            Data.Instancia.Inicial = false;
    //        }
    //    }

    //    return View();
    //}



    // GET: Archivo/Details/5
    public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Archivo/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Archivo/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Archivo/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Archivo/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Archivo/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Archivo/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}