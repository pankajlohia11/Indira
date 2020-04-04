using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessEntity.EntityModels;
using BusinessLogic.Admin_BL;
using BusinessLogic;
using System.Web.Script.Serialization;

namespace Euro.Controllers.Admin
{
    public class ET_Admin_SalesGroupController : Controller
    {

        // Creating Object for Sales Group Target Business Logic
        ET_Admin_SalesGroup_Target_BL objBL = new ET_Admin_SalesGroup_Target_BL();
        // Creating Object for Business Access Layer
        BAL bal = new BAL();

        // Creating Object for Error Master Entity
        error_master objERR = new error_master();
        // Creating Object for Log Info Entity
        tbl_loginfo objLOG = new tbl_loginfo();
        public ActionResult ET_Admin_SalesGroup()
        {
            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                {
                    var data = objBL.ET_Admin_SalesGroupTargetList_BL();
                    ViewBag.Login_Name = Session["DisplayName"].ToString();
                    return View(data);
                }
                catch (Exception exe)
                {
                    // Inserting Error Log 
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
                return RedirectToAction("ET_SessionExpire", "ET_Login");
            }
        }

        //Get : Dynamic Years binding for dropdown
        public ActionResult GetYears()
        {
            List<int> Years = new List<int>();
            DateTime startYear = DateTime.Now;
            while (startYear.Year <= DateTime.Now.AddYears(10).Year)
            {
                Years.Add(startYear.Year);
                startYear = startYear.AddYears(1);
            }
            ViewBag.Years = Years;
            return Json(Years, JsonRequestBehavior.AllowGet);
        }

        //Get : Sales Group List
        public ActionResult GetSalesGroup()
        {
            var SalesGroup = objBL.Binddropdown_Organization_BL();
            ViewBag.SalesGroup = SalesGroup;
            return Json(SalesGroup, JsonRequestBehavior.AllowGet);
        }

        // Server Side Validation for Sales Group Target
        private string validations(int GroupTargetID, int FinYear, int Salesorg, int Target)
        {
            
            if (FinYear.ToString() == "")
            {
                return "Enter User Name";
            }
            if (Salesorg.ToString() == "")
            {
                return "Enter Display Name";
            }
            if (Target.ToString() == "")
            {
                return "Enter Password";
            }
            {
                //Check UserName Already Exists or Not
                string valid = objBL.CheckDuplicateTarget_BL(GroupTargetID, FinYear, Salesorg);
                if (valid != "")
                {
                    return "Target Already Exist";
                }
            }
            return "";
        }

        // Insert / Update Sales Group target
        [HttpGet]
        public ActionResult ET_Admin_SalesGroup_Add(int GroupTargetID, int FinYear, int Salesorg, int Target)
        {
            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                {
                    string valid = validations(GroupTargetID, FinYear, Salesorg, Target);
                    if (valid == "")
                    {
                        Tbl_SalesGroup_Target tbl_SalesGroup_Target = new Tbl_SalesGroup_Target()
                        {
                            SGT_ID = GroupTargetID,
                            SGT_FIN_YEAR = FinYear,
                            SGT_GROUP_ID = Salesorg,
                            SGT_TARGET = Target,
                            CREATED_BY = Convert.ToInt32(Session["UserID"].ToString())
                        };

                        decimal d = objBL.Tbl_SalesGroup_Target_Add(tbl_SalesGroup_Target);
                        var json = "Success";
                        if (d == 0)
                        {
                            json = "Failed";
                        }
                        else
                        {
                            objLOG.log_dockey = "4";
                            objLOG.log_userid = Session["UserID"].ToString();
                            objLOG.log_recordkey = d.ToString();
                            if (GroupTargetID == 0)
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
                return RedirectToAction("ET_SessionExpire", "ET_Login");
            }
        }
    }
}