﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Euro.ExcelModel;

namespace Euro.Controllers
{
    public class ExelImportController : Controller
    {
        // GET: ExelImport
        public ActionResult Index()
        {
            ViewBag.PackingListDetails = "";
            return View();
        }
    }
}