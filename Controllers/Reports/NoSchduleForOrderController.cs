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
    public class NoSchduleForOrderController : Controller
    {
        // GET: NoSchduleForOrder
        BAL bal = new BAL();
        error_master objERR = new error_master();
        tbl_loginfo objLOG = new tbl_loginfo();
        EntityClasses dbcontext = new EntityClasses();
        public ActionResult NoSchduleForOrder()
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
        public JsonResult GetAgencyShipmentDetails(int customer, int duration, string fromdate, string todate, int type)
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
                        
                        string s1 = "";
                        
                        var schduleOrder = dbcontext.Tbl_Schedule.Where(m => m.DELETED == false).ToList();
                        for (int i = 0; i < schduleOrder.Count; i++)
                        {
                            if (i == 0)
                            {
                                s1 = schduleOrder[i].SH_OrderID.ToString();
                            }
                            else
                            {
                                s1 = s1 + "," + schduleOrder[i].SH_OrderID.ToString();
                            }

                        }
                        var UID2 = new HashSet<string>(s1.Split(',').ToList());
                        
                        var schduleCode1 = dbcontext.Tbl_Master_Order.Where(m => m.DELETED == false && !UID2.Contains(m.SO_ID.ToString())).Select(m => m.SO_ID).Distinct().ToList();
                     
                        DataTable temp = new DataTable();
                        

                            PendingShipmentDetails = (from shpheader in dbcontext.Tbl_Master_Order
                                                      join order in dbcontext.Tbl_Order_Details on shpheader.SO_ID equals order.AGEN_TRAD_PO_ID
                                                      where !UID2.Contains(shpheader.SO_ID.ToString()) && shpheader.SO_OrderType == type && shpheader.DELETED==false
                                                      && ((customer != 0 && shpheader.SO_CutomerID == customer) || customer == 0)
                                                      && ((duration != 0 && shpheader.SO_OrderDate >= fromdt && shpheader.SO_OrderDate <= todt) || duration == 0)
                                                      select new PendingShipmentReport_CM
                                                      {
                                                          SchduleID = shpheader.SO_ID,
                                                          SchduleCode = shpheader.SO_Code,
                                                          customerID = shpheader.SO_CutomerID,
                                                          customerName = (dbcontext.Tbl_Master_CompanyDetails.Where(x => x.COM_ID == shpheader.SO_CutomerID).Select(x => x.COM_NAME).FirstOrDefault()),
                                                          Sc_Date=shpheader.SO_OrderDate,
                                                          ProductId = order.PRODUCT_ID,
                                                          UOM= (dbcontext.tbl_LookUp.Where(x => x.LU_Code == order.UOM).Select(x => x.LU_Description).FirstOrDefault()),
                                                          ProductName = (dbcontext.Tbl_Product_Master.Where(x => x.P_ID == order.PRODUCT_ID).Select(x => x.P_ShortName).FirstOrDefault()),
                                                          SchduleQty = order.QUANTITY ?? 0,
                                                          UnitPrice=order.PRICE,
                                                      }).ToList();
                            temp.Columns.Add("ID");
                            temp.Columns.Add("PID");
                            temp.Columns.Add("customerID");
                            temp.Columns.Add("customerName");
                            temp.Columns.Add("Order_No");
                            temp.Columns.Add("Order_Date");
                            temp.Columns.Add("Product");
                            temp.Columns.Add("UOM");
                            temp.Columns.Add("Qty");
                            temp.Columns.Add("Unit_Price");
                            temp.Columns.Add("Amount");

                            //Customer Wise
                            var customerwisedata = PendingShipmentDetails.GroupBy(x => x.customerID).Select(a => new
                                {
                                    a.FirstOrDefault().customerID,
                                    a.FirstOrDefault().customerName,
                                    quantity = a.Sum(b => b.SchduleQty),
                                }).ToList();

                                for (int j = 0; j < customerwisedata.Count(); j++)
                                {
                                    DataRow dr2 = temp.NewRow();
                                    dr2["ID"] = "customer" + customerwisedata[j].customerID ;
                                    dr2["PID"] = "0";
                                    dr2["customerID"] = customerwisedata[j].customerID;
                                    dr2["customerName"] = customerwisedata[j].customerName;
                                    dr2["Qty"] = customerwisedata[j].quantity;
                                    dr2["Product"] = "";
                                dr2["UOM"] = "";
                                dr2["Order_No"] = "";
                                dr2["Order_Date"] = "";
                                dr2["Unit_Price"] = "";
                                dr2["Amount"] = "";
                                temp.Rows.Add(dr2);

                                    //General Data
                                    var generaldata = PendingShipmentDetails.Where(a => a.customerID == customerwisedata[j].customerID).Select(a => new
                                    {
                                        a.SchduleID,
                                        a.SchduleCode,
                                        a.ProductName,
                                        a.Sc_No,
                                        a.Sc_Date,
                                        a.UOM,
                                        a.SchduleQty,
                                        a.UnitPrice,
                                        a.ProductId,
                                    }).ToList();

                                    for (int l = 0; l < generaldata.Count(); l++)
                                    {
                                        DataRow dr4 = temp.NewRow();
                                        dr4["ID"] = "general" + generaldata[l].ProductId;
                                        dr4["PID"] = "customer" + customerwisedata[j].customerID ;
                                        dr4["Product"] = generaldata[l].ProductName;
                                    dr4["UOM"] = generaldata[l].UOM;
                                    dr4["Qty"] = generaldata[l].SchduleQty;
                                    dr4["Order_No"] = generaldata[l].SchduleCode;
                                    dr4["Order_Date"] = generaldata[l].Sc_Date.ToString("dd-MM-yyyy");
                                    dr4["Unit_Price"] = generaldata[l].UnitPrice;
                                    dr4["customerName"] = "";
                                    dr4["Amount"] = generaldata[l].SchduleQty* generaldata[l].UnitPrice;
                                    temp.Rows.Add(dr4);
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