﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FastEngSite.Controllers
{
    public class SignUpController : CommonController
    {
        //
        // GET: /SignUp/

        public ActionResult Index()
        {
            return View("Views/SignUpPage.cshtml");
        }

        //
        // GET: /SignUp/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /SignUp/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /SignUp/Create

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

        //
        // GET: /SignUp/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /SignUp/Edit/5

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

        //
        // GET: /SignUp/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /SignUp/Delete/5

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
