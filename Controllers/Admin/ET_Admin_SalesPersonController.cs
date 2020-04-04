using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Euro.Controllers.Admin
{
    public class ET_Admin_SalesPersonController : Controller
    {
        // GET: ET_Admin_SalesPerson
        public ActionResult Et_Admin_SalesPerson()
        {
            ViewBag.Login_Name = Session["DisplayName"].ToString();
            return View();
        }
    }
}