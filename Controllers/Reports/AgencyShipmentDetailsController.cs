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
    public class AgencyShipmentDetailsController : Controller
    {
        BAL bal = new BAL();
        error_master objERR = new error_master();
        tbl_loginfo objLOG = new tbl_loginfo();
        EntityClasses dbcontext = new EntityClasses();
        // GET: AgencyShipmentDetails
        public ActionResult AgencyShipmentDetails()
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
            if (duration == 7 && (fromdate=="" || todate == ""))
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
        public JsonResult GetAgencyShipmentDetails(int salesperson,int customer,int supplier,int duration,string fromdate,string todate,int type)
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
                        DateTime fromdt=DateTime.Now;
                        DateTime todt= DateTime.Now;
                        switch (duration)
                        {
                            case 1:
                                fromdt = DateTime.ParseExact("01-"+ fromdt.Month + "-"+ fromdt.Year, "dd-M-yyyy", null);
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
                        List<ShipmentReport_CM> shipmentdetails=new List<ShipmentReport_CM>();
                        if (type==1)
                        {
                             shipmentdetails = (from shpheader in dbcontext.Tbl_Shipment_Header
                                                join a in dbcontext.Tbl_Shipment_Details on shpheader.S_ID equals a.SD_PID
                                                   where shpheader.S_Type == 1 && ((salesperson != 0 && shpheader.S_SalesPerson == salesperson) || salesperson == 0)
                                                   && ((customer != 0 && shpheader.S_CustSup == customer) || customer == 0)
                                                   && ((supplier != 0 && shpheader.S_SupplierID == supplier) || supplier == 0)
                                                   && ((duration != 0 && shpheader.S_INV_DATE >= fromdt && shpheader.S_INV_DATE <= todt) || duration == 0)
                                                   select new ShipmentReport_CM
                                                   {
                                                       shipmentID = shpheader.S_ID,
                                                       salespersonID = shpheader.S_SalesPerson,
                                                       salespersonName = (dbcontext.Tbl_Master_User.Where(x => x.USER_ID == shpheader.S_SalesPerson).Select(x => x.USER_NAME).FirstOrDefault()),
                                                       customerID = shpheader.S_CustSup,
                                                       customerName = (dbcontext.Tbl_Master_CompanyDetails.Where(x => x.COM_ID == shpheader.S_CustSup).Select(x => x.COM_NAME).FirstOrDefault()),
                                                       supplierID = shpheader.S_SupplierID,
                                                       supplierName = (dbcontext.Tbl_Master_CompanyDetails.Where(x => x.COM_ID == shpheader.S_SupplierID).Select(x => x.COM_NAME).FirstOrDefault()),
                                                       ETD = shpheader.S_ETD,
                                                       InvNo = shpheader.S_INV_NO,
                                                       InvDate = shpheader.S_INV_DATE,
                                                       BlNo=shpheader.S_BL_NO,
                                                       BlDAte=shpheader.S_BL_DATE,
                                                       SO_SupSCNO = (from b in dbcontext.Tbl_Master_Order
                                                                       join c in dbcontext.Tbl_Order_Details on b.SO_ID equals c.AGEN_TRAD_PO_ID
                                                                       where c.ORDER_ID == a.SD_OrderDetailID
                                                                       select
                                                                       b.SO_SupSCNO).FirstOrDefault(),
                                                       SO_SupSCDate = (from b in dbcontext.Tbl_Master_Order
                                                                     join c in dbcontext.Tbl_Order_Details on b.SO_ID equals c.AGEN_TRAD_PO_ID
                                                                     where c.ORDER_ID == a.SD_OrderDetailID
                                                                     select
                                                                     b.SO_SupSCDate).FirstOrDefault(),
                                                       SO_Commision = (from b in dbcontext.Tbl_Master_Order
                                                                       join c in dbcontext.Tbl_Order_Details on b.SO_ID equals c.AGEN_TRAD_PO_ID
                                                                       where c.ORDER_ID == a.SD_OrderDetailID
                                                                       select
                                                                       b.SO_Commision).FirstOrDefault(),
                                                       SO_Currency = (from b in dbcontext.Tbl_Master_Order
                                                                      join c in dbcontext.Tbl_Order_Details on b.SO_ID equals c.AGEN_TRAD_PO_ID
                                                                      join f in dbcontext.Tbl_Currency_Master on b.SO_CusCurrency equals f.CURRENCY_ID
                                                                      where c.ORDER_ID == a.SD_OrderDetailID
                                                                      select
                                                                      f.CURRENCY_NAME).FirstOrDefault(),
                                                       InvAmount = shpheader.S_INV_AMT??0,
                                                       //CommissionAmt = commission.CR_CommissionRecieved
                                                   }).Distinct().ToList();
                        }
                       if(type==2)
                        {

                             shipmentdetails = (from shpheader in dbcontext.Tbl_Shipment_Header
                                                join a in dbcontext.Tbl_Shipment_Details on shpheader.S_ID equals a.SD_PID
                                                join f in dbcontext.Tbl_Order_Details on a.SD_OrderDetailID equals f.ORDER_ID 
                                                join or in dbcontext.Tbl_Master_Order on f.AGEN_TRAD_PO_ID equals or.SO_ID
                                                   where shpheader.S_Type == 2 && ((salesperson != 0 && shpheader.S_SalesPerson == salesperson) || salesperson == 0)
                                                   && ((customer != 0 && shpheader.S_CustSup == customer) || customer == 0)
                                                   && ((supplier != 0 && shpheader.S_SupplierID == supplier) || supplier == 0)
                                                   && ((duration != 0 && shpheader.S_INV_DATE >= fromdt && shpheader.S_INV_DATE <= todt) || duration == 0)
                                                   select new ShipmentReport_CM
                                                   {
                                                       shipmentID = shpheader.S_ID,
                                                       salespersonID = shpheader.S_SalesPerson,
                                                       salespersonName = (dbcontext.Tbl_Master_User.Where(x => x.USER_ID == shpheader.S_SalesPerson).Select(x => x.USER_NAME).FirstOrDefault()),
                                                       customerID = shpheader.S_CustSup,
                                                       customerName = (dbcontext.Tbl_Master_CompanyDetails.Where(x => x.COM_ID == shpheader.S_CustSup).Select(x => x.COM_NAME).FirstOrDefault()),
                                                       supplierID = shpheader.S_SupplierID,
                                                       supplierName = (dbcontext.Tbl_Master_CompanyDetails.Where(x => x.COM_ID == shpheader.S_SupplierID).Select(x => x.COM_NAME).FirstOrDefault()),
                                                       //SO_SupSCNO = (from b in dbcontext.Tbl_Master_Order
                                                       //              join c in dbcontext.Tbl_Order_Details on b.SO_ID equals c.AGEN_TRAD_PO_ID
                                                       //              where c.ORDER_ID == a.SD_OrderDetailID
                                                       //              select
                                                       //              b.SO_CusPONO).Distinct().FirstOrDefault(),
                                                       SO_SupSCNO=or.SO_CusPONO,
                                                       
                                                       SO_SupSCDate=or.SO_CusPODate,
                                                       SO_Currency = (from b in dbcontext.Tbl_Master_Order
                                                                       join c in dbcontext.Tbl_Order_Details on b.SO_ID equals c.AGEN_TRAD_PO_ID
                                                                       join f in dbcontext.Tbl_Currency_Master on b.SO_CusCurrency equals f.CURRENCY_ID
                                                                       where c.ORDER_ID == a.SD_OrderDetailID
                                                                       select
                                                                       f.CURRENCY_NAME).FirstOrDefault(),
                                                       ETD = shpheader.S_ETD,
                                                       InvNo = shpheader.S_INV_NO,
                                                       InvDate = shpheader.S_INV_DATE,
                                                       BlNo = shpheader.S_BL_NO,
                                                       BlDAte = shpheader.S_BL_DATE,
                                                       InvAmount = shpheader.S_INV_AMT??0,
                                                       CommissionAmtTrade = shpheader.S_INV_AMT ?? 0
                                                   }).Distinct().ToList();
                        }
                        DataTable temp = new DataTable();
                        if (type == 3)
                        {

                            var orderamount = (from shpheader in dbcontext.Tbl_DespatchHeader
                                           join a in dbcontext.Tbl_DespatchDetails on shpheader.D_ID equals a.DD_PID
                                           join f in dbcontext.Tbl_Order_Details on a.DD_OrderDetailID equals f.ORDER_ID
                                           join or in dbcontext.Tbl_Master_Order on f.AGEN_TRAD_PO_ID equals or.SO_ID
                                           select new ShipmentReport_CM
                                           {
                                               shipmentID=shpheader.D_ID,
                                               InvAmount = (a.DD_OrderQuantity * f.PRICE) ?? 0,
                                               CommissionAmt = (a.DD_OrderQuantity * f.PRICE) ?? 0,
                                               salespersonID = shpheader.D_SalesPerson,
                                               salespersonName = (dbcontext.Tbl_Master_User.Where(x => x.USER_ID == shpheader.D_SalesPerson).Select(x => x.USER_NAME).FirstOrDefault()),
                                               customerID = shpheader.D_CustomerID,
                                               customerName = (dbcontext.Tbl_Master_CompanyDetails.Where(x => x.COM_ID == shpheader.D_CustomerID).Select(x => x.COM_NAME).FirstOrDefault()),
                                               ETD=shpheader.D_DespatchDate,
                                               SO_SupSCNO = or.SO_CusPONO,
                                               SO_SupSCDate = or.SO_CusPODate,
                                               SO_Currency = (from b in dbcontext.Tbl_Master_Order
                                                              join c in dbcontext.Tbl_Order_Details on b.SO_ID equals c.AGEN_TRAD_PO_ID
                                                              join f in dbcontext.Tbl_Currency_Master on b.SO_CusCurrency equals f.CURRENCY_ID
                                                              where c.ORDER_ID == a.DD_OrderDetailID
                                                              select
                                                              f.CURRENCY_NAME).FirstOrDefault(),
                                               DD_OrderDetailID = a.DD_OrderDetailID,
                                               InvNo = "",
                                               InvDate = DateTime.Now,
                                           }).ToList();
                            var amountwise = orderamount.GroupBy(x => x.shipmentID).Select(a => new {
                                a.FirstOrDefault().shipmentID,
                                a.FirstOrDefault().InvNo,
                                a.FirstOrDefault().InvDate,
                                a.FirstOrDefault().SO_SupSCNO,
                                a.FirstOrDefault().SO_SupSCDate,
                                a.FirstOrDefault().SO_Currency,
                                a.FirstOrDefault().customerID,
                                a.FirstOrDefault().customerName,
                                a.FirstOrDefault().ETD,
                                a.FirstOrDefault().salespersonID,
                                a.FirstOrDefault().salespersonName,
                                InvAmount = a.Sum(b => b.InvAmount),
                                CommissionAmt = a.Sum(b => b.CommissionAmt),
                            }).ToList();

                            //var shipmentdetails1 = (from shpheader in dbcontext.Tbl_DespatchHeader
                            //                   join a in dbcontext.Tbl_DespatchDetails on shpheader.D_ID equals a.DD_PID
                            //                   join f in dbcontext.Tbl_Order_Details on a.DD_OrderDetailID equals f.ORDER_ID
                            //                   join or in dbcontext.Tbl_Master_Order on f.AGEN_TRAD_PO_ID equals or.SO_ID
                                               
                            //                   where  ((salesperson != 0 && shpheader.D_SalesPerson == salesperson) || salesperson == 0)
                            //                   && ((customer != 0 && shpheader.D_CustomerID == customer) || customer == 0)
                            //                   && ((duration != 0 && shpheader.D_DespatchDate >= fromdt && shpheader.D_DespatchDate <= todt) || duration == 0) 
                                               
                            //                   select new ShipmentReport_CM
                            //                   {
                            //                       shipmentID = shpheader.D_ID,
                            //                       salespersonID = shpheader.D_SalesPerson,
                            //                       salespersonName = (dbcontext.Tbl_Master_User.Where(x => x.USER_ID == shpheader.D_SalesPerson).Select(x => x.USER_NAME).FirstOrDefault()),
                            //                       customerID = shpheader.D_CustomerID,
                            //                       customerName = (dbcontext.Tbl_Master_CompanyDetails.Where(x => x.COM_ID == shpheader.D_CustomerID).Select(x => x.COM_NAME).FirstOrDefault()),
                                                   
                            //                       SO_SupSCNO=or.SO_CusPONO,
                            //                       SO_SupSCDate=or.SO_CusPODate,
                            //                       SO_Currency = (from b in dbcontext.Tbl_Master_Order
                            //                                      join c in dbcontext.Tbl_Order_Details on b.SO_ID equals c.AGEN_TRAD_PO_ID
                            //                                      join f in dbcontext.Tbl_Currency_Master on b.SO_CusCurrency equals f.CURRENCY_ID
                            //                                      where c.ORDER_ID == a.DD_OrderDetailID
                            //                                      select
                            //                                      f.CURRENCY_NAME).FirstOrDefault(),
                            //                       DD_OrderDetailID = a.DD_OrderDetailID,
                            //                       InvNo = "",
                            //                       InvDate = DateTime.Now,
                                                  
                            //                       CommissionAmt = 0
                            //                   }).Distinct().ToList();
                            // shipmentdetails = (from a in shipmentdetails1
                            //          join b in amountwise on a.shipmentID equals b.shipmentID
                            //          select new ShipmentReport_CM
                            //          {
                            //              shipmentID = a.shipmentID,
                            //              salespersonID = a.salespersonID,
                            //              salespersonName = (dbcontext.Tbl_Master_User.Where(x => x.USER_ID == a.salespersonID).Select(x => x.USER_NAME).FirstOrDefault()),
                            //              customerID = a.customerID,
                            //              customerName = (dbcontext.Tbl_Master_CompanyDetails.Where(x => x.COM_ID == a.customerID).Select(x => x.COM_NAME).FirstOrDefault()),

                            //              SO_SupSCNO = a.SO_SupSCNO,
                            //              SO_SupSCDate = a.SO_SupSCDate,
                            //              SO_Currency = (from b1 in dbcontext.Tbl_Master_Order
                            //                             join c in dbcontext.Tbl_Order_Details on b1.SO_ID equals c.AGEN_TRAD_PO_ID
                            //                             join f in dbcontext.Tbl_Currency_Master on b1.SO_CusCurrency equals f.CURRENCY_ID
                            //                             where c.ORDER_ID == a.DD_OrderDetailID
                            //                             select
                            //                             f.CURRENCY_NAME).FirstOrDefault(),
                            //              InvNo = "",
                            //              InvDate = DateTime.Now,

                            //              InvAmount = b.InvAmount,
                            //              CommissionAmt = b.InvAmount,
                            //          }).Distinct().ToList();
                           // DataTable temp = new DataTable();
                            temp.Columns.Add("ID");
                            temp.Columns.Add("PID");
                            temp.Columns.Add("shipmentID");
                            temp.Columns.Add("salespersonID");
                            temp.Columns.Add("salespersonName");
                            temp.Columns.Add("customerID");
                            temp.Columns.Add("customerName");
                            temp.Columns.Add("ETD");
                            temp.Columns.Add("InvNo");
                            temp.Columns.Add("InvDate");
                            temp.Columns.Add("InvAmount");
                            temp.Columns.Add("CommissionAmt");
                            temp.Columns.Add("ScNo");
                            temp.Columns.Add("ScDate");
                            temp.Columns.Add("Currency");
                            //Sales Person Wise
                            var salespersonWise = amountwise.GroupBy(x => x.salespersonID).Select(a => new {
                                a.FirstOrDefault().salespersonID,
                                a.FirstOrDefault().salespersonName,
                                InvAmount = a.Sum(b => b.InvAmount),
                                CommissionAmt = a.Sum(b => b.CommissionAmt),
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
                                dr1["ETD"] = "";
                                dr1["InvNo"] = "";
                                dr1["InvDate"] = "";
                                dr1["InvAmount"] = salespersonWise[i].InvAmount;
                                dr1["CommissionAmt"] = salespersonWise[i].CommissionAmt;
                                dr1["ScNo"] = "";
                                dr1["ScDate"] ="";
                                dr1["Currency"] ="";
                                temp.Rows.Add(dr1);
                                //Customer Wise
                                var customerwisedata = amountwise.Where(a => a.salespersonID == salespersonWise[i].salespersonID).GroupBy(x => x.customerID).Select(a => new
                                {
                                    a.FirstOrDefault().customerID,
                                    a.FirstOrDefault().customerName,
                                    InvAmount = a.Sum(b => b.InvAmount),
                                    CommissionAmt = a.Sum(b => b.CommissionAmt),
                                    
                                }).ToList();

                                for (int j = 0; j < customerwisedata.Count(); j++)
                                {
                                    DataRow dr2 = temp.NewRow();
                                    dr2["ID"] = "customer" + customerwisedata[j].customerID + "salesperson" + salespersonWise[i].salespersonID;
                                    dr2["PID"] = "salesperson" + salespersonWise[i].salespersonID;
                                    dr2["customerID"] = customerwisedata[j].customerID;
                                    dr2["customerName"] = customerwisedata[j].customerName;
                                    dr2["InvAmount"] = customerwisedata[j].InvAmount;
                                    dr2["CommissionAmt"] = customerwisedata[j].CommissionAmt;

                                    dr2["shipmentID"] = "";
                                    dr2["salespersonID"] = "";
                                    dr2["salespersonName"] = "";
                                    dr2["ETD"] = "";
                                    dr2["InvNo"] = "";
                                    dr2["InvDate"] = "";
                                    dr2["ScNo"] = "";
                                    dr2["ScDate"] ="";
                                    dr2["Currency"] = "";
                                    temp.Rows.Add(dr2);
                                    //General Data
                                    var generaldata = amountwise.Where(a => a.salespersonID == salespersonWise[i].salespersonID && a.customerID == customerwisedata[j].customerID).Select(a => new
                                    {
                                        a.shipmentID,
                                        a.ETD,
                                        a.InvNo,
                                        a.InvDate,
                                        a.InvAmount,
                                        a.CommissionAmt,
                                       // a.Comm,
                                        a.SO_Currency,
                                        a.SO_SupSCDate,
                                        a.SO_SupSCNO,
                                            }).Distinct().OrderBy(m=>m.ETD).ToList();

                                            for (int l = 0; l < generaldata.Count(); l++)
                                            {
                                                DataRow dr4 = temp.NewRow();
                                                dr4["ID"] = "general" + generaldata[l].shipmentID;
                                                dr4["PID"] =  "customer" + customerwisedata[j].customerID + "salesperson" + salespersonWise[i].salespersonID;
                                                dr4["shipmentID"] = generaldata[l].shipmentID;
                                                dr4["ETD"] = (generaldata[l].ETD ?? DateTime.Now).ToString("dd-MM-yyyy");
                                                dr4["InvNo"] = generaldata[l].InvNo;
                                                dr4["InvDate"] = (generaldata[l].InvDate ?? DateTime.Now).ToString("dd-MM-yyyy");
                                                dr4["InvAmount"] = generaldata[l].InvAmount;
                                                dr4["CommissionAmt"] = generaldata[l].CommissionAmt;
                                                dr4["salespersonID"] = "";
                                                dr4["salespersonName"] = "";
                                                dr4["customerID"] = "";
                                                dr4["customerName"] = "";
                                        ////dr4["supplierID"] = "";
                                        ////dr4["supplierName"] = "";
                                                dr4["ScNo"] = generaldata[l].SO_SupSCNO;
                                                dr4["ScDate"] = (generaldata[l].SO_SupSCDate ?? DateTime.Now).ToString("dd-MM-yyyy");
                            dr4["Currency"] =generaldata[l].SO_Currency;
                                                temp.Rows.Add(dr4);
                                            }
                                }
                            }
                        }
                        
                        if (type!=3)
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
                            temp.Columns.Add("InvNo");
                            temp.Columns.Add("InvDate");
                            temp.Columns.Add("BLNo");
                            temp.Columns.Add("BLDate");
                            temp.Columns.Add("InvAmount");
                            temp.Columns.Add("CommissionAmt");
                            temp.Columns.Add("ScNo");
                            temp.Columns.Add("ScDate");
                            temp.Columns.Add("Currency");
                            temp.Columns.Add("Comm");
                            //Sales Person Wise
                            var salespersonWise = shipmentdetails.GroupBy(x => x.salespersonID).Select(a => new {
                                a.FirstOrDefault().salespersonID,
                                a.FirstOrDefault().salespersonName,
                                a.FirstOrDefault().SO_Commision,

                                InvAmount = a.Sum(b => b.InvAmount),
                                //CommissionAmt = a.Sum(b => b.CommissionAmt),
                                CommissionAmt = a.Sum(b => b.SO_Commision * b.InvAmount / 100)
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
                                dr1["InvNo"] = "";
                                dr1["InvDate"] = "";
                                dr1["BLNo"] = "";
                                dr1["BLDate"] = "";
                                dr1["InvAmount"] = salespersonWise[i].InvAmount;
                                dr1["CommissionAmt"] = salespersonWise[i].CommissionAmt;
                                dr1["ScNo"] = "";
                                dr1["ScDate"] = "";
                                dr1["Currency"] = "";
                                dr1["Comm"] = 0;
                                temp.Rows.Add(dr1);
                                //Customer Wise
                                var customerwisedata = shipmentdetails.Where(a => a.salespersonID == salespersonWise[i].salespersonID).GroupBy(x => x.customerID).Select(a => new
                                {
                                    a.FirstOrDefault().customerID,
                                    a.FirstOrDefault().customerName,
                                    a.FirstOrDefault().SO_Commision,
                                    InvAmount = a.Sum(b => b.InvAmount),
                                   // CommissionAmt = a.Sum(b => b.CommissionAmt),
                                    CommissionAmt = a.Sum(b => b.SO_Commision * b.InvAmount / 100)
                                }).ToList();

                                for (int j = 0; j < customerwisedata.Count(); j++)
                                {
                                    DataRow dr2 = temp.NewRow();
                                    dr2["ID"] = "customer" + customerwisedata[j].customerID + "salesperson" + salespersonWise[i].salespersonID;
                                    dr2["PID"] = "salesperson" + salespersonWise[i].salespersonID;
                                    dr2["customerID"] = customerwisedata[j].customerID;
                                    dr2["customerName"] = customerwisedata[j].customerName;
                                    dr2["InvAmount"] = customerwisedata[j].InvAmount;
                                    dr2["CommissionAmt"] = customerwisedata[j].CommissionAmt;
                                    dr2["shipmentID"] = "";
                                    dr2["salespersonID"] = "";
                                    dr2["salespersonName"] = "";
                                    dr2["supplierID"] = "";
                                    dr2["supplierName"] = "";
                                    dr2["ETD"] = "";
                                    dr2["InvNo"] = "";
                                    dr2["InvDate"] = "";
                                    dr2["BLNo"] = "";
                                    dr2["BLDate"] = "";
                                    dr2["ScNo"] = "";
                                    dr2["ScDate"] = "";
                                    dr2["Currency"] = "";
                                    dr2["Comm"] = 0;
                                    temp.Rows.Add(dr2);
                                    //Supplier Wise
                                    var supplierwisedata = shipmentdetails.Where(a => a.salespersonID == salespersonWise[i].salespersonID && a.customerID == customerwisedata[j].customerID).GroupBy(x => x.supplierID).Select(a => new
                                    {
                                        a.FirstOrDefault().supplierID,
                                        a.FirstOrDefault().supplierName,
                                        InvAmount = a.Sum(b => b.InvAmount),
                                        //CommissionAmt = a.Sum(b => b.CommissionAmt),
                                        CommissionAmt = a.Sum(b => b.SO_Commision * b.InvAmount / 100)

                                    }).ToList();

                                    for (int k = 0; k < supplierwisedata.Count(); k++)
                                    {

                                        DataRow dr3 = temp.NewRow();
                                        dr3["ID"] = "supplier" + supplierwisedata[k].supplierID + "customer" + customerwisedata[j].customerID + "salesperson" + salespersonWise[i].salespersonID;
                                        dr3["PID"] = "customer" + customerwisedata[j].customerID + "salesperson" + salespersonWise[i].salespersonID;
                                        dr3["supplierID"] = supplierwisedata[k].supplierID;
                                        dr3["supplierName"] = supplierwisedata[k].supplierName;
                                        dr3["InvAmount"] = supplierwisedata[k].InvAmount;
                                        dr3["CommissionAmt"] = supplierwisedata[k].CommissionAmt;
                                        dr3["shipmentID"] = "";
                                        dr3["salespersonID"] = "";
                                        dr3["salespersonName"] = "";
                                        dr3["customerID"] = "";
                                        dr3["customerName"] = "";
                                        dr3["ETD"] = "";
                                        dr3["InvNo"] = "";
                                        dr3["InvDate"] = "";
                                        dr3["BLNo"] = "";
                                        dr3["BLDate"] = "";
                                        dr3["ScNo"] = "";
                                        dr3["ScDate"] = "";
                                        dr3["Currency"] = "";
                                        dr3["Comm"] = 0;
                                        temp.Rows.Add(dr3);
                                            //General Data
                                            var generaldata = shipmentdetails.Where(a => a.salespersonID == salespersonWise[i].salespersonID && a.customerID == customerwisedata[j].customerID && a.supplierID == supplierwisedata[k].supplierID).Select(a => new
                                            {
                                                a.shipmentID,
                                                a.ETD,
                                                a.InvNo,
                                                a.InvDate,
                                                a.InvAmount,
                                                a.CommissionAmtTrade,
                                                a.BlDAte,
                                                a.BlNo,
                                                CommissionAmt = (a.SO_Commision * a.InvAmount / 100),
                                                a.SO_SupSCNO,
                                                a.SO_SupSCDate,
                                                a.SO_Currency,
                                                a.SO_Commision,
                                            }).Distinct().OrderBy(m=>m.InvDate).ToList();

                                            for (int l = 0; l < generaldata.Count(); l++)
                                            {
                                                DataRow dr4 = temp.NewRow();
                                                dr4["ID"] = "general" + generaldata[l].shipmentID;
                                                dr4["PID"] = "supplier" + supplierwisedata[k].supplierID + "customer" + customerwisedata[j].customerID + "salesperson" + salespersonWise[i].salespersonID;
                                                dr4["shipmentID"] = generaldata[l].shipmentID;
                                                dr4["ETD"] = (generaldata[l].ETD ?? DateTime.Now).ToString("dd-MM-yyyy");
                                                dr4["InvNo"] = generaldata[l].InvNo;
                                                dr4["BLNo"] = generaldata[l].BlNo;
                                                dr4["InvDate"] = (generaldata[l].InvDate ?? DateTime.Now).ToString("dd-MM-yyyy");
                                                dr4["BLDate"] = (generaldata[l].BlDAte ?? DateTime.Now).ToString("dd-MM-yyyy");
                                                dr4["InvAmount"] = generaldata[l].InvAmount;
                                            if(type==1)
                                            {
                                                dr4["CommissionAmt"] = generaldata[l].CommissionAmt;
                                            }
                                            if(type==2)
                                            {
                                                dr4["CommissionAmt"] = generaldata[l].CommissionAmtTrade;
                                            }
                                                
                                                dr4["salespersonID"] = "";
                                                dr4["salespersonName"] = "";
                                                dr4["customerID"] = "";
                                                dr4["customerName"] = "";
                                                dr4["supplierID"] = "";
                                                dr4["supplierName"] = "";
                                                dr4["ScNo"] = generaldata[l].SO_SupSCNO;
                                                dr4["ScDate"] =(generaldata[l].SO_SupSCDate ?? DateTime.Now).ToString("dd-MM-yyyy"); ;
                                                dr4["Currency"] = generaldata[l].SO_Currency;
                                            if(generaldata[l].SO_Commision==null)
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