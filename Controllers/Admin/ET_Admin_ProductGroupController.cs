using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using BusinessEntity.EntityModels;
using BusinessEntity.CustomModels;
using BusinessLogic.Admin_BL;
using BusinessLogic;
using System.Web.Script.Serialization;

namespace Euro.Controllers.Admin
{
    public class ET_Admin_ProductGroupController : Controller
    {

        public static string prefix = "";
        public static bool automanual = false;
        ET_Admin_ProductGroup_BL ObjBL = new ET_Admin_ProductGroup_BL();
        BAL bal = new BAL();
        error_master objERR = new error_master();
        tbl_loginfo objLOG = new tbl_loginfo();

        // GET: ET_Admin_ProductGroup

        private void SetPrivilages()
        {
            ViewBag.Create = "CreateYes";
            ViewBag.Edit = "EditYes";
            ViewBag.Delete = "DeleteYes";
            ViewBag.Restore = "RestoreYes";
            ViewBag.View = "ViewYes";
            List<Tbl_AccessPermissions> ObjPermissions = bal.GetPrivilages_BL(Convert.ToInt32(Session["UserID"].ToString()), 3013);
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
            List<Tbl_Document_Master> ObjDoc = bal.AutoManual_BL(3013);
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
        private string validations(string GroupName, decimal GroupID, string GroupCode)
        {
            if (!automanual && GroupCode == "")
            {
                return "Enter the Product Group Code";
            }
            if (GroupName == "")
            {
                return "Enter the GroupName";
            }
            if(!automanual)
            {
                string valid = ObjBL.CheckDuplicateCode_BL(GroupID, GroupCode);
                if (valid != "")
                {
                    return "GroupCode Already Exist";
                }
            }
            {
                string valid = ObjBL.CheckDuplicateGroupName_BL(GroupID, GroupName);
                if (valid != "")
                {
                    return "GroupName Already Exist";
                }
            }

            return "";
        }

        public ActionResult BindDropdown(int id = 0 )
        {
            try
            {
                var product = ObjBL.BindDropdown_BL(id);
                ViewBag.product = product;
                return Json(product, JsonRequestBehavior.AllowGet);
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
        public ActionResult ET_Admin_ProductGroup()
        {
            try
            {
                SetPrivilages();
                AutoManual();
                ViewBag.Login_Name = Session["DisplayName"].ToString();
                // BindDropdown();
                var data = ObjBL.ET_Admin_ProductGrouplist_BL(Convert.ToInt32(Session["CompanyKey"]));
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

        public ActionResult ET_Admin_ProductGroup_Add(decimal GroupID, string GroupCode, string GroupName, decimal ParentType, decimal Parent)
        {
            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                {
                    string valid = validations(GroupName, GroupID, GroupCode);
                    if (valid == "")
                    {

                        Tbl_ProductGroup obj = new Tbl_ProductGroup()
                        {
                            PG_ID = GroupID,
                            PG_NAME = GroupName,
                            PG_CODE = GroupCode,
                            PG_PARENT_ID = Parent,
                            PG_TYPE = ParentType,
                            CREATED_BY = Convert.ToInt64(Session["UserID"]),
                            CREATED_DATE = DateTime.Now,
                            LAST_UPDATED_DATE = DateTime.Now,
                            DELETED = false,
                            LAST_UPDATED_BY = Convert.ToInt64(Session["UserID"]),
                            DELETED_BY = Convert.ToInt64(Session["UserID"]),
                            COM_KEY= Convert.ToInt32(Session["CompanyKey"])
                        };

                        decimal d = ObjBL.ET_Admin_ProductGroup_Add_BL(obj, automanual, prefix);
                        var json = "Success";
                        if (d == 0)
                        {
                            json = "Failed";
                        }
                        else
                        {
                            objLOG.log_dockey = "3013";
                            objLOG.log_userid = Session["UserID"].ToString();
                            objLOG.log_recordkey = d.ToString();
                            if (GroupID == 0)
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
                    // Insert Error Log
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
                //Session Expiry
                return RedirectToAction("ET_SessionExpire", "ET_Login");
            }
        }

        [HttpGet]
        public ActionResult ET_Admin_ProductGroup_Update_GetbyID(int id)
        {
            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                {
                    var data = ObjBL.ET_Admin_ProductGroup_Update_GetbyID_BL(id);
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

        public ActionResult ET_Admin_ProductGroup_Delete(int id)
        {
            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                {
                    var deleteby = Convert.ToInt64(Session["UserID"]);
                    int i = ObjBL.ET_Admin_ProductGroup_Delete_BL(id, deleteby);
                    var json = "Success";
                    if (i == 0)
                        json = "False";
                    else
                    {
                        objLOG.log_dockey = "1007";
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

        public ActionResult ET_Admin_ProductGroup_Restore()
        {
            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                {
                    var data = ObjBL.ET_Admin_ProductGroup_Restore_BL();
                    return PartialView("/Views/Admin/ET_Admin_ProductGroup/_ET_Admin_ProductGroup_Restore.cshtml", data);
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

        public ActionResult ET_Admin_ProductGroup_Restore_Insert(int id)
        {
            try
            {
                var Updatedby = Convert.ToInt64(Session["UserID"]);
                var result = ObjBL.ET_Admin_ProductGroup_Restore_Insert_BL(id, Updatedby);
                var json = "Failed";
                if (result != 0)
                {
                    json = "Success";
                }
                else
                {
                    objLOG.log_dockey = "3013";
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
        public ActionResult ET_Admin_ProductGroup_View(int id)
        {
            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                {
                    var data = ObjBL.ET_Admin_ProductGroup_View_BL(id);
                    return PartialView("/Views/Admin/ET_Admin_ProductGroup/_ET_Admin_ProductGroup_View.cshtml", data);
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