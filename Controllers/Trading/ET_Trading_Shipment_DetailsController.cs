using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Euro.Controllers.Trading
{
    public class ET_Trading_Shipment_DetailsController : Controller
    {
        // GET: ET_Trading_Shipment_Details
        public ActionResult ET_Trading_Shipment_Details()
        {
            ViewBag.Login_Name = Session["DisplayName"].ToString();
            return View();
        }
    }
}