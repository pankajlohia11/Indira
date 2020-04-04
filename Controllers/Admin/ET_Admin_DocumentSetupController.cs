using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessEntity.EntityModels;
using BusinessEntity.CustomModels;
using BusinessLogic.Admin_BL;
using BusinessLogic;
/*
    Author=Manoj
    Date = 9th Apr 2018
    Document Setup Controller 
 */
namespace Euro.Controllers.Admin
{
    public class ET_Admin_DocumentSetupController : Controller
    {
        // Creating Object for Document Setup Business Logic
        ET_Admin_DocumentSetup_BL objBL = new ET_Admin_DocumentSetup_BL();
        // Creating Object for Business Access Layer
        BAL bal = new BAL();
        // Creating Object for Error Master Entity
        error_master objERR = new error_master();
        // Creating Object for Log Info Entity
        tbl_loginfo objLOG = new tbl_loginfo();

        // GET: ET_Admin_DocumentSetup for List View
        public ActionResult ET_Admin_DocumentSetup()
        {
            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                {
                    ViewBag.details = null;
                    int com_key = Convert.ToInt32(Session["CompanyKey"]);
                    ViewBag.Login_Name = Session["DisplayName"].ToString();
                    EntityClasses dbcontext = new EntityClasses();
                    ViewBag.Users= dbcontext.Tbl_Master_User.Where(m => m.DELETED ==false && m.COM_KEY == com_key).ToList();

                    var data = (from a in dbcontext.Tbl_Document_Master
                                where a.auto_key != 1 && a.COM_KEY == com_key
                                select new tbl_DocumentMaster_CM
                                {
                                    auto_key = a.auto_key,
                                    autogen_formgroup =a.autogen_formgroup,
                                    autogen_formname=a.autogen_formname,
                                    autogen_type=a.autogen_type,
                                    autogen_prefix=a.autogen_prefix,
                                    autogen_suffix=a.autogen_suffix,
                                    autogen_startno=a.autogen_startno,
                                    autogen_endno=a.autogen_endno,
                                    workflow_status = a.workflow_status,
                                    workflowapprover =a.workflowapprover,
                                    workflowapprovername=(a.workflowapprover == 0 ? "No" : "Yes")
                                });
                        //dbcontext.Tbl_Document_Master.Where(m => m.auto_key != 1 && m.COM_KEY == com_key).ToList();
                    return View(data);
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
                return RedirectToAction("ET_SessionExpire", "ET_Login");
            }

        }


        //Update: Document Setup
        public ActionResult ET_Admin_DocumentSetup_Add(int DocumentId,string FormGroup, string FormName, string CodeType, string Prefix, string Suffix, string StartNo, string EndNo, decimal workflowapprover)
        {
            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                {
                    //Checking Server Side validation 
                    string valid = validations(CodeType, Prefix, Suffix, StartNo, EndNo, workflowapprover);
                    if (valid == "")
                    {
                        Tbl_Document_Master document_master = new Tbl_Document_Master()
                        {
                            auto_key = DocumentId,
                            autogen_formgroup = FormGroup,
                            autogen_formname = FormName,
                            autogen_type = CodeType,
                            autogen_prefix = Prefix.ToUpper(),
                            autogen_suffix = Suffix.ToUpper(),
                            autogen_startno = StartNo,
                            autogen_endno = EndNo,
                            workflowapprover = workflowapprover,
                            COM_KEY= Convert.ToInt32(Session["CompanyKey"])

                        };
                        //Document Master Update
                        decimal d = objBL.Tbl_Document_Master_Add(document_master);
                        var json = "Success";
                        if (d == 0)
                        {
                            json = "Failed";
                        }
                        else
                        {
                            // Inserting Success Log 
                            objLOG.log_dockey = "1";
                            objLOG.log_userid = Session["UserID"].ToString();
                            objLOG.log_recordkey = d.ToString();
                            objLOG.log_operation = "Update";
                            objLOG.log_Remarks = "Record Updated Successfully";
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
                    //Inserting Error Log 
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
                // Session Expire
                return RedirectToAction("ET_SessionExpire", "ET_Login");
            }

        }

        // Server Side Validation Function
        private string validations(string CodeType,string Prefix,string Suffix,string StartNo,string EndNo,decimal workflowapprover)
        {
            if (CodeType == "")
            {
                return "Choose Code Type";
            }
            if (Prefix == "")
            {
                return "Enter Prefix";
            }
            if (Suffix == "")
            {
                return "Enter Suffix";
            }
            if (StartNo == "")
            {
                return "Enter StartNo";
            }
            if (EndNo == "")
            {
                return "Enter EndNo";
            }
            

            return "";
        }
    }
}