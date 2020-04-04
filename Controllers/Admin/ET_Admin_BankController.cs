using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using BusinessEntity.EntityModels;
using BusinessLogic.Admin_BL;
using BusinessLogic;
using System.Web.Script.Serialization;

namespace Euro.Controllers.Admin
{
    public class ET_Admin_BankController : Controller
    {
        public static string prefix = "";
        public static bool automanual = false;
        Admin_Bank_BL ObjBL = new Admin_Bank_BL();
        ET_Admin_Company_BL ObjBLdrp = new ET_Admin_Company_BL();
        BAL bal = new BAL();
        error_master objERR = new error_master();
        tbl_loginfo objLOG = new tbl_loginfo();

        // GET: ET_Admin_Bank
        private void SetPrivilages()
        {
            ViewBag.Create = "CreateYes";
            ViewBag.Edit = "EditYes";
            ViewBag.Delete = "DeleteYes";
            ViewBag.Restore = "RestoreYes";
            ViewBag.View = "ViewYes";
            List<Tbl_AccessPermissions> ObjPermissions = bal.GetPrivilages_BL(Convert.ToInt32(Session["UserID"].ToString()), 7);
            foreach (var item in ObjPermissions)
            {
                if (item.IS_ADD)
                {
                    ViewBag.Create = "CreateYes";
                }
                if (item.IS_EDIT)
                {
                    ViewBag.Edit = "EditYes";
                }
                if (item.IS_DELETE)
                {
                    ViewBag.Delete = "DeleteYes";
                }
                if (item.IS_FULLCONTROL)
                {
                    ViewBag.Restore = "RestoreYes";
                }
                if (item.IS_VIEW)
                {
                    ViewBag.View = "ViewYes";
                }
            }

        }

        private void AutoManual()
        {
            ViewBag.AutoManual = false;
            automanual = false;
            List<Tbl_Document_Master> ObjDoc = bal.AutoManual_BL(7);
            if (ObjDoc.Count > 0)
            {
                foreach (var item in ObjDoc)
                {
                    if (item.autogen_type == "Automatic")
                    {
                        ViewBag.AutoManual = true;
                        prefix = item.autogen_prefix;
                        automanual = true;
                    }
                    else
                    {
                        ViewBag.Required = "required";
                    }
                }

            }
        }

        private string validations(string BankName, string DisplayName, int BankID,string Bankcode,string AccountNo,string ZipCode)
        {
            if (!automanual && BankID == 0)
            {
                return "Enter the Bank Code";
            }
            if (BankName == "")
            {
                return "Enter the Bank Name";
            }
            if (DisplayName == "")
            {
                return "Enter the Display Name";
            }
            if (AccountNo == "")
            {
                return "Enter the AccountNo";
            }
            if (ZipCode == "")
            {
                return "Enter the ZipCode";
            }
            if (!automanual)
            {
                string valid = ObjBL.CheckDuplicateCode_BL(BankID, Bankcode);
                if (valid != "")
                {
                    return "Bank Code Already Exist";
                }
            }
            {
                string valid = ObjBL.CheckDuplicateBankName_BL(BankID, BankName);
                if (valid != "")
                {
                    return "Bank Name Already Exist";
                }
            }
            {
                string valid = ObjBL.CheckDuplicateDisplayName_BL(BankID, DisplayName);
                if (valid != "")
                {
                    return "Display Name Already Exist";
                }
            }
            {
                string valid = ObjBL.CheckDuplicateAccountno_BL(BankID, AccountNo);
                if (valid != "")
                {
                    return "AccountNo Already Exist";
                }
            }
            return "";
        }
            public ActionResult ET_Admin_Bank()
        {
            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                {
                    SetPrivilages();
                    AutoManual();
                    Binddropdown_Country();
                    ViewBag.Login_Name = Session["DisplayName"].ToString();
                    var data = ObjBL.ET_Admin_Bank_BL(Convert.ToInt32(Session["CompanyKey"]));
                    return View(data);
                }
                catch (Exception exe)
                {
                    string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                    string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                    objERR.err_title = controllerName + "-" + controllerName;
                    objERR.err_message = "Sorry for the inconvience. Some error has been occured.";
                    objERR.err_details = exe.Message.Replace("'", "");
                    int errid = bal.ExceptionInsertLogs_BL(objERR);
                    return Json("ERR" + errid.ToString(), JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return RedirectToAction("ET_SessionExpire", "ET_Login");
            }
        }

        public ActionResult Binddropdown_Country()
        {
            try
            {
                var Country = ObjBLdrp.Binddropdown_County_BL();
                ViewBag.Country = Country;
            }
            catch (Exception exe)
            {
                string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                objERR.err_title = controllerName + "-" + controllerName;
                objERR.err_message = "Sorry for the inconvience. Some error has been occured.";
                objERR.err_details = exe.Message.Replace("'", "");
                int errid = bal.ExceptionInsertLogs_BL(objERR);
                return Json("ERR" + errid.ToString(), JsonRequestBehavior.AllowGet);
            }
            return View();
        }
        public ActionResult Binddropdown_State(int id)
        {
            try
            {
                var State = ObjBLdrp.Binddropdown_State_BL(id);
                // Binddropdown_City(1);
                ViewBag.State = State;
                return Json(State, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exe)
            {
                string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                objERR.err_title = controllerName + "-" + controllerName;
                objERR.err_message = "Sorry for the inconvience. Some error has been occured.";
                objERR.err_details = exe.Message.Replace("'", "");
                int errid = bal.ExceptionInsertLogs_BL(objERR);
                return Json("ERR" + errid.ToString(), JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Binddropdown_City(int id)
        {
            try
            {
                var City = ObjBLdrp.Binddropdown_City_BL(id);
                ViewBag.City = City;
                return Json(City, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exe)
            {
                string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                objERR.err_title = controllerName + "-" + controllerName;
                objERR.err_message = "Sorry for the inconvience. Some error has been occured.";
                objERR.err_details = exe.Message.Replace("'", "");
                int errid = bal.ExceptionInsertLogs_BL(objERR);
                return Json("ERR" + errid.ToString(), JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult ET_Admin_Bank_Add(int BankID, string Bankcode, string BankName, string DisplayName, string Address, string Country, string City, string State, string ZipCode, string SWIFT, string Blz, string Bic, string Iban, string AccountNo, string ContactName, string ContactNo, string Remarks)
        {
            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                {
                    string valid = validations(BankName, DisplayName, BankID, Bankcode, AccountNo, ZipCode);
                    if (valid == "")
                    {
                        Tbl_BankMaster obj = new Tbl_BankMaster()
                        {
                            BANK_CODE = Bankcode,
                            BANK_ID = BankID,
                            BANK_NAME = BankName,
                            DISPLAY_NAME = DisplayName,
                            STREET = Address,
                            CITY = City,
                            COUNTRY = Country,
                            STATE = State,
                            ZIP = ZipCode,
                            BLZ = Blz,
                            BIC = Bic,
                            CONTACT_NAME = ContactName,
                            CONTACT_NO = ContactNo,
                            IBAN = Iban,
                            SWIFT = SWIFT,
                            REMARKS = Remarks,
                            DELETED = false,
                            ACCOUNT = AccountNo,
                            CREATED_BY = Convert.ToInt32(Session["UserID"]),
                           // CREATED_DATE = DateTime.Now,
                            LAST_UPDATED_BY = Convert.ToInt32(Session["UserID"]),
                            COM_KEY= Convert.ToInt32(Session["CompanyKey"])
                        };
                        string bankcode;
                        var data = ObjBL.ET_Admin_Bank_Add_BL(obj, prefix, automanual,out bankcode);
                        var json = "Success:"+ bankcode;
                        if (data == 0)
                        {
                            json = "Failed";
                        }
                        else
                        {
                            objLOG.log_dockey = "7";
                            objLOG.log_userid = Session["UserID"].ToString();
                            objLOG.log_recordkey = data.ToString();
                            if (BankID == 0)
                            {
                                objLOG.log_operation = "Insert";
                                objLOG.log_Remarks = "Record Inserted Successfully";
                            }
                            else
                            {
                                objLOG.log_operation = "Update";
                                objLOG.log_Remarks = "Record Updated Successfully";
                            }
                            bal.OperationInsertLogs_BL(objLOG);
                        }
                        return Json(json, JsonRequestBehavior.AllowGet);
                    }
                    else {
                        var json = "Validation:" + valid;
                        return Json(json, JsonRequestBehavior.AllowGet);
                    }

                }
                catch (Exception exe)
                {
                    string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                    string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                    objERR.err_title = controllerName + "-" + controllerName;
                    objERR.err_message = "Sorry for the inconvience. Some error has been occured.";
                    objERR.err_details = exe.Message.Replace("'", "");
                    int errid = bal.ExceptionInsertLogs_BL(objERR);
                    return Json("ERR" + errid.ToString(), JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return RedirectToAction("ET_SessionExpire", "ET_Login");
            }
        }

        public ActionResult ET_Admin_Bank_Delete(int id)
        {
            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                {
                    var deleteby = Convert.ToInt64(Session["UserID"]);
                    int i = ObjBL.ET_Admin_Roles_Deleted_BL(id, deleteby);
                    var json = "Success";
                    if (i == 0)
                        json = "False";
                    else
                    {
                        objLOG.log_dockey = "7";
                        objLOG.log_operation = "Delete";
                        objLOG.log_userid = Session["UserID"].ToString();
                        objLOG.log_recordkey = id.ToString();
                        objLOG.log_Remarks = "Record Deleted Successfully";
                        bal.OperationInsertLogs_BL(objLOG);
                    }
                    return Json(json, JsonRequestBehavior.AllowGet);
                }
                catch (Exception exe)
                {
                    string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                    string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                    objERR.err_title = controllerName + "-" + controllerName;
                    objERR.err_message = "Sorry for the inconvience. Some error has been occured.";
                    objERR.err_details = exe.Message.Replace("'", "");
                    int errid = bal.ExceptionInsertLogs_BL(objERR);
                    return Json("ERR" + errid.ToString(), JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return RedirectToAction("ET_SessionExpire", "ET_Login");
            }
        }

        public ActionResult ET_Admin_Bank_Restore_Insert(int id)
        {
            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                {
                    var deleteby = Convert.ToInt64(Session["UserID"]);
                    var result = ObjBL.ET_Admin_Bank_Restore_Insert_BL(id, deleteby);
                    var json = "Failed";
                    if (result != 0)
                    {
                        json = "Success";
                    }
                    else
                    {
                        objLOG.log_dockey = "7";
                        objLOG.log_operation = "Restore";
                        objLOG.log_userid = Session["UserID"].ToString();
                        objLOG.log_recordkey = id.ToString();
                        objLOG.log_Remarks = "Record Restored Successfully";
                        bal.OperationInsertLogs_BL(objLOG);
                    }
                    return Json(json, JsonRequestBehavior.AllowGet);
                }
                catch (Exception exe)
                {
                    string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                    string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                    objERR.err_title = controllerName + "-" + controllerName;
                    objERR.err_message = "Sorry for the inconvience. Some error has been occured.";
                    objERR.err_details = exe.Message.Replace("'", "");
                    int errid = bal.ExceptionInsertLogs_BL(objERR);
                    return Json("ERR" + errid.ToString(), JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return RedirectToAction("ET_SessionExpire", "ET_Login");
            }
        }

        public ActionResult ET_Admin_Bank_Restore()
        {
            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                {
                    var data = ObjBL.ET_Admin_Bank_Restore_BL();
                    return PartialView("/Views/Admin/ET_Admin_Bank/_ET_Admin_Bank_Restore.cshtml",data);
                }
                catch (Exception exe)
                {
                    string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                    string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                    objERR.err_title = controllerName + "-" + controllerName;
                    objERR.err_message = "Sorry for the inconvience. Some error has been occured.";
                    objERR.err_details = exe.Message.Replace("'", "");
                    int errid = bal.ExceptionInsertLogs_BL(objERR);
                    return Json("ERR" + errid.ToString(), JsonRequestBehavior.AllowGet);
                }

            }
            else
            {
                return RedirectToAction("ET_SessionExpire", "ET_Login");
            }
        }

        public ActionResult ET_Admin_Bank_View(int id)
        {
            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                {
                    var data = ObjBL.ET_Admin_Bank_View_BL(id);
                    return PartialView("/Views/Admin/ET_Admin_Bank/_ET_Admin_Bank_View.cshtml", data);
                }
                catch (Exception exe)
                {
                    string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                    string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                    objERR.err_title = controllerName + "-" + controllerName;
                    objERR.err_message = "Sorry for the inconvience. Some error has been occured.";
                    objERR.err_details = exe.Message.Replace("'", "");
                    int errid = bal.ExceptionInsertLogs_BL(objERR);
                    return Json("ERR" + errid.ToString(), JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return RedirectToAction("ET_SessionExpire", "ET_Login");
            }
        }

        public ActionResult ET_Admin_Bank_Update_GetbyID(int id)
        {
            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                {
                    var data = ObjBL.ET_Admin_Bank_Update_GetbyID_BL(id);
                    var json = new JavaScriptSerializer().Serialize(data);
                    return Json(json, JsonRequestBehavior.AllowGet);
                }
                catch (Exception exe)
                {
                    string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                    string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                    objERR.err_title = controllerName + "-" + controllerName;
                    objERR.err_message = "Sorry for the inconvience. Some error has been occured.";
                    objERR.err_details = exe.Message.Replace("'", "");
                    int errid = bal.ExceptionInsertLogs_BL(objERR);
                    return Json("ERR" + errid.ToString(), JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return RedirectToAction("ET_SessionExpire", "ET_Login");
            }
        }
    }
}
