using BusinessEntity.CustomModels;
using BusinessEntity.EntityModels;
using BusinessLogic;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Euro.Controllers.Reports
{
    public class AllSchduleDetailsController : Controller
    {
        public static string prefix = "";
        public static bool automanual = false;
        BAL bal = new BAL();
        error_master objERR = new error_master();
        tbl_loginfo objLOG = new tbl_loginfo();
        EntityClasses dbcontext = new EntityClasses();
        // GET: AllSchduleDetails
        public ActionResult AllSchduleDetails()
        {
            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                ViewBag.COMPANY_LOGO = dbcontext.Tbl_SystemSetUp.Single(m => m.COMPANY_ID == 1).COMPANY_LOGO;
                ViewBag.COMPANY_NAME = dbcontext.Tbl_SystemSetUp.Single(m => m.COMPANY_ID == 1).COMPANY_NAME;
                ViewBag.Login_Name = Session["DisplayName"].ToString();
                return View();
            }
            else
            {
                return RedirectToAction("ET_SessionExpire", "ET_Login");
            }
        }
        public string Validate(int duration, string fromdate, string todate)
        {
            if (duration == 7 && (fromdate == "" || todate == ""))
            {
                return "Select From date and To date";
            }
            if (duration == 7 && fromdate != "" && todate != "")
            {
                if (DateTime.ParseExact(fromdate, "dd-MM-yyyy", null) > DateTime.ParseExact(todate, "dd-MM-yyyy", null))
                {
                    return "From date cannot be greater than To date";
                }
            }
            return "";
        }
        //Gets Report Data
        public JsonResult GetAgencyShipmentDetails(int duration, string fromdate, string todate, int type)
        {
            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                {
                    string valid = Validate(duration, fromdate, todate);
                    if (valid == "")
                    {
                        dbcontext.Configuration.ProxyCreationEnabled = false;
                        DateTime fromdt = DateTime.Now;
                        DateTime todt = DateTime.Now;
                        switch (duration)
                        {
                            case 1:
                                fromdt = DateTime.ParseExact("01-" + fromdt.Month + "-" + fromdt.Year, "dd-M-yyyy", null);
                                todt = DateTime.ParseExact(DateTime.DaysInMonth(todt.Year, todt.Month) + "-" + todt.Month + "-" + todt.Year, "dd-M-yyyy", null);
                                break;
                            case 2:
                                fromdt = fromdt.AddMonths(-1);
                                todt = todt.AddMonths(-1);
                                fromdt = DateTime.ParseExact("01-" + fromdt.Month + "-" + fromdt.Year, "dd-M-yyyy", null);
                                todt = DateTime.ParseExact(DateTime.DaysInMonth(todt.Year, todt.Month) + "-" + todt.Month + "-" + todt.Year, "dd-M-yyyy", null);
                                break;
                            case 3:
                                fromdt = fromdt.AddMonths(-3);
                                fromdt = DateTime.ParseExact("01-" + fromdt.Month + "-" + fromdt.Year, "dd-M-yyyy", null);
                                todt = DateTime.ParseExact(DateTime.DaysInMonth(todt.Year, todt.Month) + "-" + todt.Month + "-" + todt.Year, "dd-M-yyyy", null);
                                break;
                            case 4:
                                fromdt = fromdt.AddMonths(-6);
                                fromdt = DateTime.ParseExact("01-" + fromdt.Month + "-" + fromdt.Year, "dd-M-yyyy", null);
                                todt = DateTime.ParseExact(DateTime.DaysInMonth(todt.Year, todt.Month) + "-" + todt.Month + "-" + todt.Year, "dd-M-yyyy", null);
                                break;
                            case 5:
                                fromdt = fromdt.AddMonths(-9);
                                fromdt = DateTime.ParseExact("01-" + fromdt.Month + "-" + fromdt.Year, "dd-M-yyyy", null);
                                todt = DateTime.ParseExact(DateTime.DaysInMonth(todt.Year, todt.Month) + "-" + todt.Month + "-" + todt.Year, "dd-M-yyyy", null);
                                break;
                            case 6:
                                fromdt = fromdt.AddMonths(-12);
                                fromdt = DateTime.ParseExact("01-" + fromdt.Month + "-" + fromdt.Year, "dd-M-yyyy", null);
                                todt = DateTime.ParseExact(DateTime.DaysInMonth(todt.Year, todt.Month) + "-" + todt.Month + "-" + todt.Year, "dd-M-yyyy", null);
                                break;
                            case 7:
                                fromdt = DateTime.ParseExact(fromdate, "dd-M-yyyy", null);
                                todt = DateTime.ParseExact(todate, "dd-M-yyyy", null);
                                break;
                        }
                        List<PendingShipmentReport_CM> PendingShipmentDetails = new List<PendingShipmentReport_CM>();
                        PendingShipmentDetails = (from order1 in dbcontext.Tbl_Master_Order
                                                  join OrderDetails in dbcontext.Tbl_Order_Details on order1.SO_ID equals OrderDetails.AGEN_TRAD_PO_ID
                                                  join order in dbcontext.Tbl_Schedule on order1.SO_ID equals order.SH_OrderID

                                                  where ((duration != 0 && order.SH_DATE >= fromdt && order.SH_DATE <= todt) || duration == 0)
                                                  select new PendingShipmentReport_CM
                                                  {
                                                      SchduleID = order1.SO_ID,
                                                      SchduleCode = order.SH_Code,
                                                      Sc_No = order1.SO_CusPONO,
                                                      Sc_Date = order1.SO_CusPODate ?? DateTime.Now,
                                                      ETD = order.SH_DATE,
                                                      salespersonID = order1.SO_SalesPersonID,
                                                      salespersonName = (dbcontext.Tbl_Master_User.Where(x => x.USER_ID == order1.SO_SalesPersonID).Select(x => x.USER_NAME).FirstOrDefault()),
                                                      customerID = order1.SO_CutomerID,
                                                      supplierID = order1.SO_CutomerID,
                                                      customerName = (dbcontext.Tbl_Master_CompanyDetails.Where(x => x.COM_ID == order1.SO_CutomerID).Select(x => x.COM_NAME).FirstOrDefault()),
                                                      supplierName = (dbcontext.Tbl_Master_CompanyDetails.Where(x => x.COM_ID == order1.SO_SupplierID).Select(x => x.COM_NAME).FirstOrDefault()),
                                                      ProductId = OrderDetails.PRODUCT_ID,
                                                      ProductName = (dbcontext.Tbl_Product_Master.Where(x => x.P_ID == OrderDetails.PRODUCT_ID).Select(x => x.P_ShortName).FirstOrDefault()),
                                                      QUANTITY = OrderDetails.QUANTITY,
                                                      SchduleQty=order.SH_SheduledQuantity,

                                                  }).Distinct().ToList();

                        DataTable temp = new DataTable();
                        temp.Columns.Add("ID");
                        temp.Columns.Add("PID");
                        temp.Columns.Add("shipmentID");
                        temp.Columns.Add("salespersonID");
                        temp.Columns.Add("salespersonName");
                        temp.Columns.Add("customerID");
                        temp.Columns.Add("customerName");
                        temp.Columns.Add("supplierID");
                        temp.Columns.Add("supplierName");
                        temp.Columns.Add("ETD");
                        temp.Columns.Add("ProductName");
                        temp.Columns.Add("Quantity");
                        temp.Columns.Add("BalanceQty");
                        temp.Columns.Add("ScNo");
                        temp.Columns.Add("SchduleNo");
                        temp.Columns.Add("ScDate");
                        temp.Columns.Add("Comm");
                     
                                                     
                                    //General Data
                                    var generaldata = PendingShipmentDetails.Select(a => new
                                    {
                                        a.SchduleID,
                                        a.ETD,
                                        a.ProductName,
                                        a.ProductId,
                                        a.SchduleCode,
                                        a.QUANTITY,
                                        a.SchduleQty,
                                        a.Sc_No,
                                        a.Sc_Date,

                                        a.SO_Commision,
                                    }).Distinct().OrderBy(m => m.SchduleID).ToList();

                                    for (int l = 0; l < generaldata.Count(); l++)
                                    {
                                        DataRow dr4 = temp.NewRow();
                                        dr4["ID"] = "general" + generaldata[l].SchduleID;
                                        dr4["PID"] = "";
                                        dr4["shipmentID"] = generaldata[l].SchduleID;
                                        dr4["ETD"] = (generaldata[l].ETD).ToString("dd-MM-yyyy");
                                        dr4["ProductName"] = generaldata[l].ProductName;
                                        dr4["Quantity"] = generaldata[l].QUANTITY;
                                        dr4["salespersonID"] = "";
                                        dr4["salespersonName"] = "";
                                        dr4["customerID"] = "";
                                        dr4["customerName"] = "";
                                        dr4["supplierID"] = "";
                                        dr4["supplierName"] = "";
                                        dr4["ScNo"] = generaldata[l].Sc_No;
                                        dr4["BalanceQty"] = Convert.ToDecimal(generaldata[l].QUANTITY)-Convert.ToDecimal(generaldata[l].SchduleQty);
                                        dr4["SchduleNo"] = generaldata[l].SchduleCode;
                                        dr4["ScDate"] = (generaldata[l].Sc_Date).ToString("dd-MM-yyyy"); ;

                                        if (generaldata[l].SO_Commision == null)
                                        {
                                            dr4["Comm"] = 0;
                                        }
                                        else
                                        {
                                            dr4["Comm"] = generaldata[l].SO_Commision;
                                        }

                                        temp.Rows.Add(dr4);
                                    }
                               

                        var json = Newtonsoft.Json.JsonConvert.SerializeObject(temp, Formatting.Indented);
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
                return Json("Expired", JsonRequestBehavior.AllowGet);
            }
        }
    }
}