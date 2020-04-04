using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Euro.Controllers.Admin
{
    public class ET_General_Shipment_ScheduleController : Controller
    {
        // GET: ET_General_Shipment_Schedule
        public ActionResult ET_General_Shipment_Schedule()
        {
            ViewBag.Login_Name = Session["DisplayName"].ToString();
            return View();
        }
    }
}