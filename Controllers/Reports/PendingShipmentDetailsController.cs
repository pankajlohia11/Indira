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
using System.Data;
using Newtonsoft.Json;

namespace Euro.Controllers.Reports
{
    public class PendingShipmentDetailsController : Controller
    {
        // GET: PendingShipmentDetails
        BAL bal = new BAL();
        error_master objERR = new error_master();
        tbl_loginfo objLOG = new tbl_loginfo();
        EntityClasses dbcontext = new EntityClasses();
        public ActionResult PendingShipmentDetails()
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
        //This Method is used to get Customer,Supplier,Salesperson Data
        public JsonResult GetCustomerSupplierSalesperson()
        {
            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                {
                    using (EntityClasses dbcontext = new EntityClasses())
                    {
                        dbcontext.Configuration.ProxyCreationEnabled = false;
                        int companykey = Convert.ToInt32(Session["CompanyKey"]);

                        var customer = dbcontext.Tbl_Master_CompanyDetails.Where(m => m.Cust_Supp != 1 && m.COM_KEY == companykey && m.DELETED == false).OrderBy(m => m.COM_NAME).ToList();

                        var supplier = dbcontext.Tbl_Master_CompanyDetails.Where(m => m.Cust_Supp != 0 && m.COM_KEY == companykey && m.DELETED == false).OrderBy(m => m.COM_NAME).ToList();

                        var ObjSales_Org = dbcontext.Tbl_Sales_Organization.Where(m => m.COM_KEY == companykey && m.DELETED == false).ToList();
                        string s = "";
                        for (int i = 0; i < ObjSales_Org.Count(); i++)
                        {
                            if (i == 0)
                            {
                                s = ObjSales_Org[i].ORG_EMPLOYEE_IDS.ToString();
                            }
                            else
                            {
                                s = s + "," + ObjSales_Org[i].ORG_EMPLOYEE_IDS.ToString();
                            }
                        }
                        var UID = new HashSet<decimal>(s.Split(',').Select(m => Convert.ToDecimal(m)).ToList());
                        var salesperson = dbcontext.Tbl_Master_User.Where(m => m.USER_ID > 0 && UID.Contains(m.USER_ID) && m.COM_KEY == companykey && m.DELETED == false).Distinct().OrderBy(a => a.DISPLAY_NAME).ToList();

                        var result = new { customer, supplier, salesperson };
                        var json = new JavaScriptSerializer().Serialize(result);
                        return Json(result, JsonRequestBehavior.AllowGet);
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
        //validate date
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
        public JsonResult GetAgencyShipmentDetails(int salesperson, int customer, int supplier, int duration, string fromdate, string todate, int type)
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
                        string s = "";
                        string s1 = "";
                        var ShipmentSchdule = dbcontext.Tbl_Shipment_Header.Where(m => m.DELETED == false).ToList();
                        for (int i = 0; i < ShipmentSchdule.Count; i++)
                        {
                            if (i == 0)
                            {
                                s = ShipmentSchdule[i].S_ScheduleID.ToString();
                            }
                            else
                            {
                                s = s + "," + ShipmentSchdule[i].S_ScheduleID.ToString();
                            }

                        }
                        var UID = new HashSet<string>(s.Split(',').ToList());
                        var schduleCode = dbcontext.Tbl_Schedule.Where(m => m.DELETED == false && m.SH_Type==type && !UID.Contains(m.SH_Code)).Select(m => m.SH_Code).Distinct().ToList();
                        PendingShipmentDetails = (from  order1 in dbcontext.Tbl_Master_Order 
                                                  join OrderDetails in dbcontext.Tbl_Order_Details on order1.SO_ID equals OrderDetails.AGEN_TRAD_PO_ID
                                                  join  order in dbcontext.Tbl_Schedule on order1.SO_ID equals order.SH_OrderID

                                                  where schduleCode.Contains(order.SH_Code) && order.SH_Type == type && ((salesperson != 0 && order.SH_SalesPerson == salesperson) || salesperson == 0)
                                                  && ((customer != 0 && order1.SO_CutomerID == customer) || customer == 0)
                                                  && ((supplier != 0 && order1.SO_SupplierID == supplier) || supplier == 0)
                                                  && ((duration != 0 && order.SH_DATE >= fromdt && order.SH_DATE <= todt) || duration == 0)
                                                  select new PendingShipmentReport_CM
                                                  {
                                                      SchduleID = order1.SO_ID,
                                                      SchduleCode = order.SH_Code,
                                                      Sc_No = order1.SO_CusPONO,
                                                      Sc_Date = order1.SO_CusPODate ?? DateTime.Now,
                                                      ETD = order.SH_DATE ,
                                                      salespersonID = order1.SO_SalesPersonID,
                                                      salespersonName = (dbcontext.Tbl_Master_User.Where(x => x.USER_ID == order1.SO_SalesPersonID).Select(x => x.USER_NAME).FirstOrDefault()),
                                                      customerID = order1.SO_CutomerID,
                                                      supplierID = order1.SO_CutomerID,
                                                      customerName = (dbcontext.Tbl_Master_CompanyDetails.Where(x => x.COM_ID == order1.SO_CutomerID).Select(x => x.COM_NAME).FirstOrDefault()),
                                                      supplierName = (dbcontext.Tbl_Master_CompanyDetails.Where(x => x.COM_ID == order1.SO_SupplierID).Select(x => x.COM_NAME).FirstOrDefault()),
                                                      ProductId = OrderDetails.PRODUCT_ID,
                                                      ProductName = (dbcontext.Tbl_Product_Master.Where(x => x.P_ID == OrderDetails.PRODUCT_ID).Select(x => x.P_ShortName).FirstOrDefault()),
                                                      QUANTITY = OrderDetails.QUANTITY,

                                                  }).Distinct().ToList();

                        DataTable temp = new DataTable();
                        if (type == 3)
                        {

                            var DespatchSchdule = dbcontext.Tbl_DespatchHeader.Where(m => m.DELETED == false).ToList();

                            for (int i = 0; i < DespatchSchdule.Count; i++)
                            {
                                if (i == 0)
                                {
                                    s = DespatchSchdule[i].D_OrderID;
                                }
                                else
                                {
                                    s = s + "," + DespatchSchdule[i].D_OrderID;
                                }

                            }
                            var UID1 = new HashSet<string>(s.Split(',').ToList());
                            PendingShipmentDetails = (from order in dbcontext.Tbl_Master_Order
                                                  join OrderDetails in dbcontext.Tbl_Order_Details on order.SO_ID equals OrderDetails.AGEN_TRAD_PO_ID
                                                  where !UID1.Contains(order.SO_ID.ToString()) && order.SO_OrderType == 3 && ((salesperson != 0 && order.SO_SalesPersonID == salesperson) || salesperson == 0)
                                                  && ((customer != 0 && order.SO_CutomerID == customer) || customer == 0)
                                                  && ((supplier != 0 && order.SO_SupplierID == supplier) || supplier == 0)
                                                  && ((duration != 0 && order.SO_OrderDate >= fromdt && order.SO_OrderDate <= todt) || duration == 0)
                                                  select new PendingShipmentReport_CM
                                                  {
                                                      SchduleID = order.SO_ID,
                                                      SchduleCode = order.SO_Code,
                                                      Sc_No=order.SO_CusPONO,
                                                      Sc_Date=order.SO_CusPODate ?? DateTime.Now,
                                                      salespersonID = order.SO_SalesPersonID,
                                                      salespersonName = (dbcontext.Tbl_Master_User.Where(x => x.USER_ID == order.SO_SalesPersonID).Select(x => x.USER_NAME).FirstOrDefault()),
                                                      customerID = order.SO_CutomerID,
                                                      customerName = (dbcontext.Tbl_Master_CompanyDetails.Where(x => x.COM_ID == order.SO_CutomerID).Select(x => x.COM_NAME).FirstOrDefault()),
                                                      ProductId = OrderDetails.PRODUCT_ID,
                                                      ProductName = (dbcontext.Tbl_Product_Master.Where(x => x.P_ID == OrderDetails.PRODUCT_ID).Select(x => x.P_ShortName).FirstOrDefault()),
                                                      QUANTITY = OrderDetails.QUANTITY,

                                                  }).ToList();
                        temp.Columns.Add("ID");
                        temp.Columns.Add("PID");
                        temp.Columns.Add("SchduleID");
                        temp.Columns.Add("salespersonID");
                        temp.Columns.Add("salespersonName");
                        temp.Columns.Add("Sc_No");
                        temp.Columns.Add("Sc_Date");
                        temp.Columns.Add("customerID");
                        temp.Columns.Add("customerName");
                        temp.Columns.Add("SchduleNo");
                        temp.Columns.Add("Product");
                        temp.Columns.Add("Qty");
                        //Sales Person Wise
                        var salespersonWise = PendingShipmentDetails.GroupBy(x => x.salespersonID).Select(a => new {
                            a.FirstOrDefault().salespersonID,
                            a.FirstOrDefault().salespersonName,
                            Quantity = a.Sum(b => b.QUANTITY),
                        }).ToList();

                        for (int i = 0; i < salespersonWise.Count(); i++)
                        {
                            DataRow dr1 = temp.NewRow();
                            dr1["ID"] = "salesperson" + salespersonWise[i].salespersonID;
                            dr1["PID"] = "0";
                            dr1["salespersonID"] = salespersonWise[i].salespersonID;
                            dr1["salespersonName"] = salespersonWise[i].salespersonName;
                            dr1["SchduleID"] = "";
                            dr1["customerID"] = "";
                            dr1["customerName"] = "";
                            dr1["SchduleNo"] = "";
                            dr1["Sc_No"] = "";
                            dr1["Sc_Date"] = "";
                            dr1["Product"] = "";
                            dr1["Qty"] = salespersonWise[i].Quantity;
                            temp.Rows.Add(dr1);
                            //Customer Wise
                            var customerwisedata = PendingShipmentDetails.Where(a => a.salespersonID == salespersonWise[i].salespersonID).GroupBy(x => x.customerID).Select(a => new
                            {
                                a.FirstOrDefault().customerID,
                                a.FirstOrDefault().customerName,
                                quantity = a.Sum(b => b.QUANTITY),
                            }).ToList();

                            for (int j = 0; j < customerwisedata.Count(); j++)
                            {
                                DataRow dr2 = temp.NewRow();
                                dr2["ID"] = "customer" + customerwisedata[j].customerID + "salesperson" + salespersonWise[i].salespersonID;
                                dr2["PID"] = "salesperson" + salespersonWise[i].salespersonID;
                                dr2["customerID"] = customerwisedata[j].customerID;
                                dr2["customerName"] = customerwisedata[j].customerName;
                                dr2["Qty"] = customerwisedata[j].quantity;
                                dr2["SchduleID"] = "";
                                dr2["salespersonID"] = "";
                                dr2["salespersonName"] = "";
                                dr2["SchduleNo"] = "";
                                dr2["Sc_No"] = "";
                                dr2["Sc_Date"] = "";
                                dr2["Product"] = "";
                                temp.Rows.Add(dr2);
                               
                                    //General Data
                                    var generaldata = PendingShipmentDetails.Where(a => a.salespersonID == salespersonWise[i].salespersonID && a.customerID == customerwisedata[j].customerID).Select(a => new
                                    {
                                        a.SchduleID,
                                        a.SchduleCode,
                                        a.ProductName,
                                        a.Sc_No,
                                        a.Sc_Date,
                                        a.QUANTITY,
                                        a.ProductId,
                                    }).ToList();

                                    for (int l = 0; l < generaldata.Count(); l++)
                                    {
                                        DataRow dr4 = temp.NewRow();
                                        dr4["ID"] = "general" + generaldata[l].SchduleID;
                                        dr4["PID"] = "customer" + customerwisedata[j].customerID + "salesperson" + salespersonWise[i].salespersonID;
                                        dr4["SchduleID"] = generaldata[l].SchduleID;
                                        dr4["SchduleNo"] = generaldata[l].SchduleCode;
                                        dr4["Sc_Date"] = (generaldata[l].Sc_Date).ToString("dd-mm-yyyy");
                                        dr4["Sc_No"] = generaldata[l].Sc_No;
                                        dr4["Product"] = generaldata[l].ProductName;
                                        dr4["Qty"] = generaldata[l].QUANTITY;
                                        dr4["salespersonID"] = "";
                                        dr4["salespersonName"] = "";
                                        dr4["customerID"] = "";
                                        dr4["customerName"] = "";
                                        temp.Rows.Add(dr4);
                                    }
                                


                            }
                        }
                        }
                        if(type!=3)
                        {
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
                            temp.Columns.Add("ScNo");
                            temp.Columns.Add("SchduleNo");
                            temp.Columns.Add("ScDate");
                            temp.Columns.Add("Comm");
                            //Sales Person Wise
                            var salespersonWise = PendingShipmentDetails.GroupBy(x => x.salespersonID).Select(a => new {
                                a.FirstOrDefault().salespersonID,
                                a.FirstOrDefault().salespersonName,
                                a.FirstOrDefault().SO_Commision,
                                Quantity = a.Sum(b => b.QUANTITY),
                            }).ToList();

                            for (int i = 0; i < salespersonWise.Count(); i++)
                            {
                                DataRow dr1 = temp.NewRow();
                                dr1["ID"] = "salesperson" + salespersonWise[i].salespersonID;
                                dr1["PID"] = "0";
                                dr1["salespersonID"] = salespersonWise[i].salespersonID;
                                dr1["salespersonName"] = salespersonWise[i].salespersonName;
                                dr1["shipmentID"] = "";
                                dr1["customerID"] = "";
                                dr1["customerName"] = "";
                                dr1["supplierID"] = "";
                                dr1["supplierName"] = "";
                                dr1["ETD"] = "";
                                dr1["ProductName"] = "";
                                dr1["Quantity"] = salespersonWise[i].Quantity;
                                dr1["ScNo"] = "";
                                dr1["SchduleNo"] = "";
                                dr1["ScDate"] = "";
                                dr1["Comm"] = 0;
                                temp.Rows.Add(dr1);
                                //Customer Wise
                                var customerwisedata = PendingShipmentDetails.Where(a => a.salespersonID == salespersonWise[i].salespersonID).GroupBy(x => x.customerID).Select(a => new
                                {
                                    a.FirstOrDefault().customerID,
                                    a.FirstOrDefault().customerName,
                                    a.FirstOrDefault().SO_Commision,
                                    Quantity = a.Sum(b => b.QUANTITY),
                                }).ToList();

                                for (int j = 0; j < customerwisedata.Count(); j++)
                                {
                                    DataRow dr2 = temp.NewRow();
                                    dr2["ID"] = "customer" + customerwisedata[j].customerID + "salesperson" + salespersonWise[i].salespersonID;
                                    dr2["PID"] = "salesperson" + salespersonWise[i].salespersonID;
                                    dr2["customerID"] = customerwisedata[j].customerID;
                                    dr2["customerName"] = customerwisedata[j].customerName;
                                    dr2["shipmentID"] = "";
                                    dr2["salespersonID"] = "";
                                    dr2["salespersonName"] = "";
                                    dr2["supplierID"] = "";
                                    dr2["supplierName"] = "";
                                    dr2["ETD"] = "";
                                    dr2["ProductName"] = "";
                                    dr2["Quantity"] = salespersonWise[i].Quantity;
                                    dr2["ScNo"] = "";
                                    dr2["SchduleNo"] = "";
                                    dr2["ScDate"] = "";
                                    dr2["Comm"] = 0;
                                    temp.Rows.Add(dr2);
                                    //Supplier Wise
                                    var supplierwisedata = PendingShipmentDetails.Where(a => a.salespersonID == salespersonWise[i].salespersonID && a.customerID == customerwisedata[j].customerID).GroupBy(x => x.supplierID).Select(a => new
                                    {
                                        a.FirstOrDefault().supplierID,
                                        a.FirstOrDefault().supplierName,
                                        Quantity = a.Sum(b => b.QUANTITY),

                                    }).ToList();

                                    for (int k = 0; k < supplierwisedata.Count(); k++)
                                    {

                                        DataRow dr3 = temp.NewRow();
                                        dr3["ID"] = "supplier" + supplierwisedata[k].supplierID + "customer" + customerwisedata[j].customerID + "salesperson" + salespersonWise[i].salespersonID;
                                        dr3["PID"] = "customer" + customerwisedata[j].customerID + "salesperson" + salespersonWise[i].salespersonID;
                                        dr3["supplierID"] = supplierwisedata[k].supplierID;
                                        dr3["supplierName"] = supplierwisedata[k].supplierName;
                                        dr3["shipmentID"] = "";
                                        dr3["salespersonID"] = "";
                                        dr3["salespersonName"] = "";
                                        dr3["customerID"] = "";
                                        dr3["customerName"] = "";
                                        dr3["ETD"] = "";
                                        dr3["ProductName"] = "";
                                        dr3["Quantity"] = supplierwisedata[k].Quantity;
                                        dr3["ScNo"] = "";
                                        dr3["SchduleNo"] = "";
                                        dr3["ScDate"] = "";
                                        dr3["Comm"] = 0;
                                        temp.Rows.Add(dr3);
                                        //General Data
                                        var generaldata = PendingShipmentDetails.Where(a => a.salespersonID == salespersonWise[i].salespersonID && a.customerID == customerwisedata[j].customerID && a.supplierID == supplierwisedata[k].supplierID).Select(a => new
                                        {
                                            a.SchduleID,
                                            a.ETD,
                                            a.ProductName,
                                            a.ProductId,
                                            a.SchduleCode,
                                            a.QUANTITY,
                                            a.Sc_No,
                                            a.Sc_Date,
                                            
                                            a.SO_Commision,
                                        }).Distinct().OrderBy(m => m.ETD).ToList();

                                        for (int l = 0; l < generaldata.Count(); l++)
                                        {
                                            DataRow dr4 = temp.NewRow();
                                            dr4["ID"] = "general" + generaldata[l].SchduleID;
                                            dr4["PID"] = "supplier" + supplierwisedata[k].supplierID + "customer" + customerwisedata[j].customerID + "salesperson" + salespersonWise[i].salespersonID;
                                            dr4["shipmentID"] = generaldata[l].SchduleID;
                                            dr4["ETD"] = (generaldata[l].ETD ).ToString("dd-MM-yyyy");
                                            dr4["ProductName"] = generaldata[l].ProductName;
                                            dr4["Quantity"] = generaldata[l].QUANTITY ;
                                            dr4["salespersonID"] = "";
                                            dr4["salespersonName"] = "";
                                            dr4["customerID"] = "";
                                            dr4["customerName"] = "";
                                            dr4["supplierID"] = "";
                                            dr4["supplierName"] = "";
                                            dr4["ScNo"] = generaldata[l].Sc_No;
                                            dr4["SchduleNo"] = generaldata[l].SchduleCode;
                                            dr4["ScDate"] = (generaldata[l].Sc_Date ).ToString("dd-MM-yyyy"); ;
                                            
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
                                    }


                                }
                            }
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