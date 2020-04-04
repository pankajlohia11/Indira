using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Routing;
using BusinessEntity.EntityModels;
using BusinessEntity.CustomModels;
using BusinessLogic.Admin_BL;
using BusinessLogic;

namespace Euro.Controllers.Calendar
{
    public class ET_Shipment_ScheduleController : Controller
    {
        // GET: ET_Shipment_Schedule
        error_master objERR = new error_master();
        BAL bal = new BAL();
        EntityClasses dbcontext = new EntityClasses();

        public ActionResult ET_Shipment_Schedule()
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

        public ActionResult GetShipmentScheduleList()
        {
            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                {
                    var data = (from a in dbcontext.Tbl_Schedule
                                 where a.DELETED == false
                                 select new
                                 {
                                     a.SH_Code,
                                     a.SH_DATE,
                                     customerID=(dbcontext.Tbl_Master_Order.Where(m=>m.SO_ID==a.SH_OrderID).Select(e=>e.SO_CutomerID).FirstOrDefault()),
                                     Status =(dbcontext.Tbl_Shipment_Header.Where(m => m.S_ScheduleID == a.SH_Code).Count() > 0 ? "yes" : "No")
                                 }).GroupBy(m => m.SH_Code).Select(m=>m.FirstOrDefault()).ToList();
                    
                    
                    string s = "";
                    for (int i = 0; i < data.Count(); i++)
                    {
                        string classs = "b-l b-2x bg-success";
                        if(data[i].SH_DATE < DateTime.Today.Date && data[i].Status != "yes")
                           classs = "b-l b-2x bg-danger";
                        decimal customerIDs = Convert.ToDecimal(data[i].customerID);
                        string count=dbcontext.Tbl_Master_CompanyDetails.Where(m => m.COM_ID == customerIDs).Select(m=>m.COM_DISPLAYNAME).FirstOrDefault();
                        //var CustomerName = dbcontext.Tbl_Master_CompanyDetails.Where(m => m.COM_ID == customerIDs).Selects1 => s1.COM(_NAME).FirstOrDefault();
                        if (i == 0)
                        {
                            s = s + "[{'title':'" + data[i].SH_Code+" "+ count + "','start':'" + DateTime.Parse(data[i].SH_DATE.ToString()).ToString("yyyy-MM-dd") + "','url':'shipping(&" + data[i].SH_Code + "&)','className':'" + classs + "'}";
                        }
                        else
                        {
                            s = s + ",{'title':'" + data[i].SH_Code+" "+ count + "','start':'" + DateTime.Parse(data[i].SH_DATE.ToString()).ToString("yyyy-MM-dd") + "','url':'shipping(&" + data[i].SH_Code + "&)','className':'" + classs + "'}";
                        }
                    }
                    if (data.Count() > 0)
                        s = s + "]";
                    s = s.Replace("'", "\"");
                    s = s.Replace("&", "'");
                    var json = new JavaScriptSerializer().Serialize(s);
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
    }
}