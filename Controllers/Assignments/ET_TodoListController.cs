using BusinessEntity.CustomModels;
using BusinessEntity.EntityModels;
using BusinessLogic;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Euro.Controllers.Assignments
{
    public class ET_TodoListController : Controller
    {
        public static string prefix = "";
        public static bool automanual = false;
        BAL bal = new BAL();
        error_master objERR = new error_master();
        tbl_loginfo objLOG = new tbl_loginfo();
        EntityClasses dbcontext = new EntityClasses();
        // GET: ET_TodoList
        public ActionResult ET_TodoList()
        {
            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                        try
                        {
                            ViewBag.Login_Name = Session["DisplayName"].ToString();
                            return View();

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
        public JsonResult GetShipmentDocumentListForNotifications(bool delete)
        {
            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                {
                    int com_key = Convert.ToInt32(Session["Companykey"].ToString());
                    var today = DateTime.Now.Date;
                    var documentDays = dbcontext.Tbl_SystemSetUp.Where(m => m.DELETED == false).Select(m => m.DOCUMENTDAYS).ToArray();
                    int daysDocument = Convert.ToInt32(documentDays[0]);
                    dbcontext.Configuration.ProxyCreationEnabled = false;
                    var data = (from a in dbcontext.Tbl_Shipment_Header
                                join c in dbcontext.Tbl_Master_CompanyDetails on a.S_CustSup equals c.COM_ID into ord
                                from y in ord
                                join d in dbcontext.Tbl_Master_CompanyDetails on a.S_SupplierID equals d.COM_ID into ord1
                                from y1 in ord1
                                where a.DELETED == delete && a.COM_KEY == com_key && today == System.Data.Entity.DbFunctions.AddDays(a.S_INV_DATE, daysDocument)
                                select new Shipment_CM
                                {
                                    S_ID = a.S_ID,
                                    S_Code = a.S_Code,
                                    S_ETD = a.S_ETD,
                                    S_ETA = a.S_ETA,
                                    COM_DISPLAYNAME = y.COM_DISPLAYNAME,
                                    SuppName = y1.COM_NAME,
                                    S_DeparturePort = a.S_DeparturePort,
                                    S_ArrivalPort = a.S_ArrivalPort,
                                    S_Type = a.S_Type,
                                    S_STATUS = a.S_STATUS,
                                    S_DebitNoteStatus = a.S_DebitNoteStatus,
                                    S_CommissionRecievedStatus = a.S_CommissionRecievedStatus,
                                    S_DebitNoteApprovalStatus = a.S_DebitNoteApprovalStatus,
                                    S_INV_AMT = a.S_INV_AMT,
                                    S_INV_NO = a.S_INV_NO
                                }).Distinct().OrderByDescending(m => m.S_ID).ToList();
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
                return Json("Expired", JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetShipmentListForNotifications(bool delete)
        {
            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                {
                    int com_key = Convert.ToInt32(Session["Companykey"].ToString());
                    var today = DateTime.Now.Date;

                    dbcontext.Configuration.ProxyCreationEnabled = false;
                    var data = (from a in dbcontext.Tbl_Shipment_Header
                                join c in dbcontext.Tbl_Master_CompanyDetails on a.S_CustSup equals c.COM_ID into ord
                                from y in ord
                                join d in dbcontext.Tbl_Master_CompanyDetails on a.S_SupplierID equals d.COM_ID into ord1
                                from y1 in ord1
                                where a.DELETED == delete && a.COM_KEY == com_key && a.S_ETD == today
                                select new Shipment_CM
                                {
                                    S_ID = a.S_ID,
                                    S_Code = a.S_Code,
                                    S_ETD = a.S_ETD,
                                    S_ETA = a.S_ETA,
                                    COM_DISPLAYNAME = y.COM_DISPLAYNAME,
                                    SuppName = y1.COM_NAME,
                                    S_DeparturePort = a.S_DeparturePort,
                                    S_ArrivalPort = a.S_ArrivalPort,
                                    S_Type = a.S_Type,
                                    S_STATUS = a.S_STATUS,
                                    S_DebitNoteStatus = a.S_DebitNoteStatus,
                                    S_CommissionRecievedStatus = a.S_CommissionRecievedStatus,
                                    S_DebitNoteApprovalStatus = a.S_DebitNoteApprovalStatus,
                                    S_INV_AMT = a.S_INV_AMT,
                                    S_INV_NO = a.S_INV_NO
                                }).Distinct().OrderByDescending(m => m.S_ID).ToList();
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
                return Json("Expired", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult GetTaskList()
        {
            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                var today = DateTime.Now.Date;
                try
                {
                    string useridqts = "'" + Session["UserID"].ToString() + "'";
                    int userId = Convert.ToInt32(Session["UserID"].ToString());
                    var data = dbcontext.Tbl_AssignTask.Where(m => m.DELETED == false && System.Data.Entity.DbFunctions.TruncateTime(m.Expec_Date)== System.Data.Entity.DbFunctions.TruncateTime(today) && ((m.TSK_Type == false && m.CREATED_BY == userId) || (m.TSK_Type == true && m.TSK_AssignTo.Contains(useridqts)))).ToList();
                    
                    return Json(data, JsonRequestBehavior.AllowGet);
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
    }
   
}