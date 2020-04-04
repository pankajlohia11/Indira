using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessEntity.CustomModels;
using BusinessEntity.EntityModels;
using BusinessLogic.Admin_BL;
using BusinessLogic;
using System.Web.Script.Serialization;

namespace Euro.Controllers.Admin
{
    public class ET_Admin_PaymentConfigurationController : Controller
    {
        public static string prefix = "";
        public static bool automanual = false;
        BAL bal = new BAL();
        error_master objERR = new error_master();
        tbl_loginfo objLOG = new tbl_loginfo();
        ET_Admin_PaymentConfiguration_BL ObjBL = new ET_Admin_PaymentConfiguration_BL();

        // GET: ET_Admin_PaymentConfiguration

        private void SetPrivilages()
        {
            ViewBag.Create = "CreateYes";
            ViewBag.Edit = "EditYes";
            ViewBag.Delete = "DeleteYes";
            ViewBag.Restore = "RestoreYes";
            ViewBag.View = "ViewYes";
            List<Tbl_AccessPermissions> ObjPermissions = bal.GetPrivilages_BL(Convert.ToInt32(Session["UserID"].ToString()), 2007);
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
            List<Tbl_Document_Master> ObjDoc = bal.AutoManual_BL(2007);
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

        private string validations(int PaymentID, string PaymentCode, int PaymentFrom, int PaymentTo)
        {
            if (!automanual && PaymentCode == "")
            {
                return "Enter the Payment Code";
            }
            if (PaymentFrom == 0)
            {
                return "Enter the Payment Code";
            }
            if (PaymentTo == 0)
            {
                return "Enter the Payment To";
            }
            if (!automanual)
            {
                string valid = ObjBL.CheckDuplicateCode_BL(PaymentID, PaymentCode);
                if (valid != "")
                {
                    return "Payment Code Already Exist";
                }
            }
            return "";
        }
        public ActionResult ET_Admin_PaymentConfiguration()
        {
            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                {
                    SetPrivilages();
                    AutoManual();
                    ViewBag.Login_Name = Session["DisplayName"].ToString();
                    var data = ObjBL.ET_Admin_PaymentConfigurationlist_BL(Convert.ToInt32(Session["CompanyKey"]));
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

        public ActionResult ET_Admin_PaymentConfiguration_Add(int PaymentID, string PaymentCode, int PaymentFrom, int PaymentTo, string DiscountType, decimal Discount, decimal DiscountAmount)
        {
            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                {
                    string valid = validations(PaymentID, PaymentCode, PaymentFrom, PaymentTo);
                    if (valid == "")
                    {
                        Tbl_Payment_Config obj = new Tbl_Payment_Config()
                        {
                            Payment_Config_Id = PaymentID,
                            Payment_Config_Code = PaymentCode,
                            Payment_From = PaymentFrom,
                            Payment_To = PaymentTo,
                            Payment_Discount_Type = DiscountType,
                            Payment_Discount_Amount = DiscountAmount,
                            Payment_Discount_Percentage = Discount,
                            Created_By = Session["UserID"].ToString(),
                            Last_Updated_By = Session["UserID"].ToString(),
                            COM_KEY = Convert.ToInt32(Session["CompanyKey"])
                        };
                        var data = ObjBL.ET_Admin_PaymentConfiguration_Add_BL(obj, prefix, automanual);
                        var json = "Success";
                        if (data == 0)
                        {
                            json = "Failed";
                        }
                        else
                        {
                            objLOG.log_dockey = "2007";
                            objLOG.log_userid = Session["UserID"].ToString();
                            objLOG.log_recordkey = data.ToString();
                            if (PaymentID == 0)
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
                    else
                    {
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

        public ActionResult ET_Admin_PaymentConfig_Update_GetbyID(int id)
        {
            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                {
                    var data = ObjBL.ET_Admin_PaymentConfig_Update_GetbyID_BL(id);
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

        public ActionResult ET_Admin_PaymentConfig_Delete(int id)
        {
            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                {
                    var deleteby = Convert.ToInt64(Session["UserID"]);
                    int i = ObjBL.ET_Admin_PaymentConfig_Delete_BL(id, deleteby);
                    var json = "Success";
                    if (i == 0)
                        json = "False";
                    else
                    {
                        objLOG.log_dockey = "2007";
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
        public ActionResult ET_Admin_PaymentConfig_Restore()
        {
            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                {
                    var data = ObjBL.ET_Admin_PaymentConfig_Restore_BL();
                    return PartialView("/Views/Admin/ET_Admin_PaymentConfiguration/_ET_Admin_PaymentConfig_Restore.cshtml", data);
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

        public ActionResult ET_Admin_PaymentConfig_Restore_Insert(int id)
        {
            try
            {
                var Updatedby = Session["UserID"].ToString();
                var result = ObjBL.ET_Admin_PaymentConfig_Restore_Insert_BL(id, Updatedby);
                var json = "Failed";
                if (result != 0)
                {
                    json = "Success";
                }
                else
                {
                    objLOG.log_dockey = "2007";
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
        public ActionResult ET_Admin_PaymentConfig_View(int id)
        {
            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                {
                    var data = ObjBL.ET_Admin_PaymentConfig_View_BL(id);
                    return PartialView("/Views/Admin/ET_Admin_PaymentConfiguration/_ET_Admin_PaymentConfig_View.cshtml", data);
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