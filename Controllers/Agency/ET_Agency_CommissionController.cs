using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Euro.Controllers.Agency
{
    public class ET_Agency_CommissionController : Controller
    {
        /* Author       = jesmi
    Created Date =3rd Apr 2018
    [Action Method=ET_Agency_Commission ]
    */
        // GET: ET_Agency_Commission
        public ActionResult ET_Agency_Commission()
        {
            ViewBag.Login_Name = Session["DisplayName"].ToString();
            return View();
        }
    }
}