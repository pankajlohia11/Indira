using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Euro.Controllers.Trading
{
    public class ET_Trading_OTOPurchaseController : Controller
    {
        // GET: ET_Trading_OTOPurchase
        public ActionResult ET_Trading_OTOPurchase()
        {
            ViewBag.Login_Name = Session["DisplayName"].ToString();
            return View();
        }
    }
}