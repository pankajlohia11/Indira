using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessEntity.EntityModels;
using BusinessLogic;

namespace Euro.Controllers
{
    public class ET_LoginController : Controller
    {
        BAL bal = new BAL();

        // GET: ET_Login
        public ActionResult ET_Login(string logout)
        {
            if (logout != null)
            {
                Session.Abandon();
                return RedirectToAction("ET_Login");
            }
            else if (Session["UserID"] != null)
            {
                return RedirectToAction("ET_Admin_Roles", "ET_Admin_Roles");
            }
            return View();
        }

        [HttpPost]
        public ActionResult ET_Login_post(Tbl_Master_User objBE)
        {
            BALCrypto Cryp = new BALCrypto();
            string pwd = Cryp.Encrypting(objBE.USER_PASSWORD, "12345");
            objBE.USER_PASSWORD = pwd;
            List<Tbl_Master_User> result = bal.Authenticate_BL(objBE);
            if (result.Count != 0)
            {
                Session["UserID"] = result[0].USER_ID;
                Session["UserName"] = result[0].USER_NAME;
                Session["DisplayName"] = result[0].DISPLAY_NAME;
                Session["CompanyKey"] = result[0].COM_KEY;
                //return RedirectToAction("ET_Admin_Roles", "ET_Admin_Roles");
                return RedirectToAction("ET_Admin_Dashboard", "ET_Admin_Dashboard");
            }
            else
            {
                TempData["Message"] = "Invalid Credentials";
                return RedirectToAction("ET_Login");
            }
        }
        public ActionResult ET_SessionExpire()
        {
            return View();
        }
        public ActionResult ET_ForgotPassword(string logout)
        {
            return View();
        }
        public ActionResult ET_InvalidRequest()
        {
            return View();
        }
    }
}