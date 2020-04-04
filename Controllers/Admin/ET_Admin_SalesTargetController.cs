using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessEntity.EntityModels;
using BusinessEntity.CustomModels;
using BusinessLogic.Admin_BL;
using BusinessLogic;
using System.Web.Script.Serialization;
/*Author: Manoj
 created Date : 18th April 2018
 Sales Target Controller
 */
namespace Euro.Controllers.Admin
{
    public class ET_Admin_SalesTargetController : Controller
    {
        public static string prefix = "";
        public static bool automanual = false;

        // Creating Object for Sales Target Business Logic
        ET_Admin_SalesTarget_BL objBL = new ET_Admin_SalesTarget_BL();
        // Creating Object for Business Access Layer
        BAL bal = new BAL();
        
        // Creating Object for Error Master Entity
        error_master objERR = new error_master();
        // Creating Object for Log Info Entity
        tbl_loginfo objLOG = new tbl_loginfo();

        // GET: ET_Admin_SalesTarget
        public ActionResult ET_Admin_SalesTarget()
        {
            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                {
                    AutoManual();
                    ViewBag.Login_Name = Session["DisplayName"].ToString();
                    return View();
                }
                catch (Exception exe)
                {
                    //Insert Error Log
                    string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                    string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                    objERR.err_title = controllerName + "-" + controllerName;
                    objERR.err_message = "Sorry for the inconvience. Some error has been occured.";
                    objERR.err_details = exe.Message.Replace("'", "");
                    int errid = bal.ExceptionInsertLogs_BL(objERR);
                    ViewBag.Error = errid;
                    return View();
                }
            }
            else
            {
                //Session Expiry
                return RedirectToAction("ET_SessionExpire", "ET_Login");
            }
            
        }
        //Checking privilage
        public JsonResult GetPrivilages()
        {

            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                {
                    using (EntityClasses dbcontext = new EntityClasses())
                    {
                        var privilagelist = bal.GetPrivilages_BL(Convert.ToInt32(Session["UserID"].ToString()), 1008);
                        var json = new JavaScriptSerializer().Serialize(privilagelist);
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
                return Json("Expired", JsonRequestBehavior.AllowGet);
            }
        }
        // Get Target List
        public JsonResult GetTargetList(bool deleted)
        {
            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                {
                    using (EntityClasses dbcontext = new EntityClasses())
                    {
                        var ObjSales_Org = dbcontext.Tbl_Sales_Organization.Where(m => m.DELETED != true).Select(m => m.ORG_HEAD_ID).ToList();
                        var data4 = dbcontext.Tbl_Sales_Target.Where(m => m.DELETED != true && ObjSales_Org.Contains(m.ST_EMP_ID)).ToList();
                        //var user1 = objBL.ET_Admin_SalesOrganizationList_BL(deleted, Convert.ToInt32(Session["CompanyKey"]));
                        int com_key = Convert.ToInt32(Session["CompanyKey"]);
                        var data = (from grp in dbcontext.Tbl_SalesGroup_Target
                                    join org in dbcontext.Tbl_Sales_Organization on grp.SGT_GROUP_ID equals org.ORG_ID into t
                                    from rt in t
                                    where grp.DELETED == deleted && grp.COM_KEY == com_key
                                    select new
                                    {
                                        grp.SGT_ID,
                                        rt.ORG_NAME,
                                        grp.SGT_FIN_YEAR,
                                        grp.SGT_TARGET
                                    }).ToList();
                        var json = new JavaScriptSerializer().Serialize(data);
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
                return Json("Expired", JsonRequestBehavior.AllowGet);
            }
        }
        //Get : Dynamic Years binding for dropdown
        //public void GetYears()
        //{
        //    List<int> Years = new List<int>();
        //    DateTime startYear = DateTime.Now;
        //    while (startYear.Year <= DateTime.Now.AddYears(10).Year)
        //    {
        //        Years.Add(startYear.Year);
        //        startYear = startYear.AddYears(1);
        //    }
        //    ViewBag.Years = Years;
        //}

        // Bind Dropdown Roles
        public JsonResult Binddropdown_Organization()
        {
            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                {
                    var OrganizationList = objBL.Binddropdown_Organization_BL(Convert.ToInt32(Session["CompanyKey"]));
                    var json = new JavaScriptSerializer().Serialize(OrganizationList);
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
                return Json("Expired", JsonRequestBehavior.AllowGet);
            }
        }
        
        //Checking Code Auto/Manual
        private void AutoManual()
        {
            ViewBag.AutoManual = false;
            automanual = false;
            List<Tbl_Document_Master> ObjDoc = bal.AutoManual_BL(1008);
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

        //Bind Sales Target Employees
        public ActionResult BindRow_Employees(int ORG_ID,int FIN_YEAR)
        {
            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                {
                    var data = objBL.BindRow_Employees_BL(ORG_ID, Convert.ToInt32(Session["CompanyKey"]), FIN_YEAR);
                    return PartialView("/Views/Admin/ET_Admin_SalesTarget/_ET_Admin_SalesTarget_Employees.cshtml", data);
                }
                catch (Exception exe)
                {
                    //Insert Error Log
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
                //Session Expire
                return RedirectToAction("ET_SessionExpire", "ET_Login");
            }
        }

        // Server Side Validation for User Master
        private string validations(int TargetID, int FinYear, int ORG_ID, int Target)
        {
            if (FinYear.ToString() == "")
            {
                return "Choose Finacial Year";
            }
            if (ORG_ID.ToString() == "")
            {
                return "Choose Saless Organization";
            }
            if (Target.ToString() == "" || Target==0)
            {
                return "Enter Target";
            }
            {
                //Check Financial Year and Organization Exists
                string valid = objBL.CheckFinancialORGExists_BL(TargetID, FinYear,ORG_ID);
                if (valid != "")
                {
                    return "Financial Year and Organization Already Exist";
                }
            }
            
            return "";
        }

        // Insert / Update Sales Target
        [HttpPost]
        public JsonResult ET_Admin_SalesTarget_Add(int TargetID, int FinYear, int ORG_ID, int Target, string TargetData)
        {
            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                {
                    string valid = validations(TargetID, FinYear, ORG_ID, Target);
                    if (valid == "")
                    {
                        SalesTarget_CM tbl_Sales_Target = new SalesTarget_CM()
                        {
                            ST_ID = TargetID,
                            ST_FINANCIAL_YEAR = FinYear,
                            ORG_ID = ORG_ID,
                            ST_TARGET=Target,
                            CREATED_BY = Convert.ToInt32(Session["UserID"].ToString()),
                            LAST_UPDATED_BY = Convert.ToInt32(Session["UserID"].ToString()),
                            COM_KEY= Convert.ToInt32(Session["CompanyKey"]),
                            Targetdata = TargetData
                        };

                        decimal d = objBL.Tbl_Sales_Target_Add(tbl_Sales_Target);
                        var json = "Success";
                        if (d == 0)
                        {
                            json = "Failed";
                        }
                        else
                        {
                            objLOG.log_dockey = "1008";
                            objLOG.log_userid = Session["UserID"].ToString();
                            objLOG.log_recordkey = d.ToString();
                            if (TargetID == 0)
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
                    //Insert Error Log
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
                var json = "Session Expired:";
                return Json(json, JsonRequestBehavior.AllowGet);
            }
        }

        //Get: Record for Edit Row 
        [HttpPost]
        public ActionResult ET_Admin_SalesTarget_Update_Get(int id)
        {
            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                {
                    var data = objBL.ET_Admin_SalesTarget_Update_Get(id);
                    var json = new JavaScriptSerializer().Serialize(data);
                    return Json(json, JsonRequestBehavior.AllowGet);
                }
                catch (Exception exe)
                {
                    //Insert Error Log
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
        //Restore the delete records
        public ActionResult ET_Admin_SalesTarget_RestoreDelete(int id, bool type)
        {
            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                {
                    int i = objBL.ET_Admin_SalesTarget_Deleted_BL(id, type, Convert.ToInt32(Session["UserID"].ToString()));
                    var json = "Success";
                    if (i == 0)
                        json = "False";
                    else
                    {
                        objLOG.log_dockey = "5";
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
                    //Insert Error Log
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
        // View : Popup View record
        public ActionResult ET_Admin_SalesTarget_View(int id)
        {
            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                {
                    EntityClasses dbcontext = new EntityClasses();
                    var data1 = (from t in dbcontext.Tbl_SalesGroup_Target
                                 join o in dbcontext.Tbl_Sales_Organization on t.SGT_GROUP_ID equals o.ORG_ID
                                 select new SalesTarget_CM
                                 {
                                     ORG_Name = o.ORG_NAME,
                                     ST_FINANCIAL_YEAR = t.SGT_FIN_YEAR ?? 0,
                                     ST_TARGET = t.SGT_TARGET ?? 0
                                 });
                    return PartialView("/Views/Admin/ET_Admin_SalesTarget/ET_Admin_SalesTarget_View.cshtml", data1);
                }
                catch (Exception exe)
                {
                    //Insert Error Log
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
                return Json("Expired", JsonRequestBehavior.AllowGet);
            }
        }
    }
}