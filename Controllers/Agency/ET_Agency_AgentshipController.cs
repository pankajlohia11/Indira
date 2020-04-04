using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Euro.Controllers.Admin
{
    public class ET_Agency_AgentshipController : Controller
    {
        // GET: ET_Agency_Agentship
        public ActionResult ET_Agency_Agentship()
        {
            ViewBag.Login_Name = Session["DisplayName"].ToString();
            return View();
        }
    }
}