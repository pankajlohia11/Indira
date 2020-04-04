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
    public class BookingDetailsController : Controller
    {
        // GET: BookingDetails
        BAL bal = new BAL();
        error_master objERR = new error_master();
        tbl_loginfo objLOG = new tbl_loginfo();
        EntityClasses dbcontext = new EntityClasses();
        public ActionResult BookingDetails()
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
                        List<BookingReport_CM> OrderDetails = new List<BookingReport_CM>();
                        if (type == 1)
                        {

                            OrderDetails = (from order in dbcontext.Tbl_Master_Order
                                            join detail in dbcontext.Tbl_Order_Details on order.SO_ID equals detail.AGEN_TRAD_PO_ID
                                            where order.SO_OrderType == 1 && ((customer != 0 && order.SO_CutomerID == customer) || customer == 0)
                                                      && ((supplier != 0 && order.SO_SupplierID == supplier) || supplier == 0)
                                                      && ((duration != 0 && order.SO_OrderDate >= fromdt && order.SO_OrderDate <= todt) || duration == 0)
                                            select new BookingReport_CM
                                            {
                                                OrderID = order.SO_ID,
                                                customerID = order.SO_CutomerID,
                                                salespersonID = order.SO_SalesPersonID,
                                                salespersonName = (dbcontext.Tbl_Master_User.Where(x => x.USER_ID == order.SO_SalesPersonID).Select(x => x.USER_NAME).FirstOrDefault()),
                                                customerName = (dbcontext.Tbl_Master_CompanyDetails.Where(x => x.COM_ID == order.SO_CutomerID).Select(x => x.COM_NAME).FirstOrDefault()),
                                                supplierID = order.SO_SupplierID,
                                                supplierName = (dbcontext.Tbl_Master_CompanyDetails.Where(x => x.COM_ID == order.SO_SupplierID).Select(x => x.COM_NAME).FirstOrDefault()),
                                                SupScNo = order.SO_SupSCNO,
                                                ProductId = detail.PRODUCT_ID,
                                                ProductName = (dbcontext.Tbl_Product_Master.Where(x => x.P_ID == detail.PRODUCT_ID).Select(x => x.P_ShortName).FirstOrDefault()),
                                                OrderedQty = detail.QUANTITY,
                                                //suguna
                                                Tradpo = "",
                                                Sc_Date=order.SO_SupSCDate??DateTime.Now,
                                                Po_No=order.SO_CusPONO,
                                                PODate=order.SO_CusPODate,
                                                DealDate=DateTime.Today,
                                                Deal_No=0,
                                                Uom = (from c in dbcontext.Tbl_Order_Details
                                                       join f in dbcontext.tbl_LookUp on c.UOM equals f.LU_Code
                                                       where c.AGEN_TRAD_PO_ID == order.SO_ID
                                                       select
                                                       f.LU_Description).FirstOrDefault(),
                                                Currency = (from b in dbcontext.Tbl_Master_Order
                                                               join c in dbcontext.Tbl_Order_Details on b.SO_ID equals c.AGEN_TRAD_PO_ID
                                                               join f in dbcontext.Tbl_Currency_Master on b.SO_CusCurrency equals f.CURRENCY_ID
                                                               where c.AGEN_TRAD_PO_ID == order.SO_ID
                                                               select
                                                               f.CURRENCY_NAME).FirstOrDefault(),
                                                Price =detail.PRICE,
                                                Amount=(detail.PRICE*detail.QUANTITY),
                                                Comm=order.SO_Commision,
                                                Comm_Amt =0,
                                                //--------
                                                      }).ToList();
                        }
                        if (type == 2)
                        {

                            OrderDetails = (from order in dbcontext.Tbl_Master_Order
                                            join detail in dbcontext.Tbl_Order_Details on order.SO_ID equals detail.AGEN_TRAD_PO_ID
                                            where order.SO_OrderType == 2 && ((customer != 0 && order.SO_CutomerID == customer) || customer == 0)
                                                      && ((supplier != 0 && order.SO_SupplierID == supplier) || supplier == 0)
                                                      && ((duration != 0 && order.SO_OrderDate >= fromdt && order.SO_OrderDate <= todt) || duration == 0)
                                            select new BookingReport_CM
                                            {
                                                OrderID = order.SO_ID,
                                                customerID = order.SO_CutomerID,
                                                salespersonID = order.SO_SalesPersonID,
                                                salespersonName = (dbcontext.Tbl_Master_User.Where(x => x.USER_ID == order.SO_SalesPersonID).Select(x => x.USER_NAME).FirstOrDefault()),
                                                customerName = (dbcontext.Tbl_Master_CompanyDetails.Where(x => x.COM_ID == order.SO_CutomerID).Select(x => x.COM_NAME).FirstOrDefault()),
                                                supplierID = order.SO_SupplierID,
                                                supplierName = (dbcontext.Tbl_Master_CompanyDetails.Where(x => x.COM_ID == order.SO_SupplierID).Select(x => x.COM_NAME).FirstOrDefault()),
                                                SupScNo = order.SO_SupSCNO,
                                                ProductId = detail.PRODUCT_ID,
                                                ProductName = (dbcontext.Tbl_Product_Master.Where(x => x.P_ID == detail.PRODUCT_ID).Select(x => x.P_ShortName).FirstOrDefault()),
                                                OrderedQty = detail.QUANTITY ,
                                                //suguna
                                                Tradpo = "",
                                                Po_No = order.SO_CusPONO,
                                                PODate = order.SO_CusPODate,
                                                DealDate = DateTime.Today,
                                                Deal_No = 0,
                                                Uom = (from  c in dbcontext.Tbl_Order_Details 
                                                       join f in dbcontext.tbl_LookUp on c.UOM equals f.LU_Code
                                                       where c.AGEN_TRAD_PO_ID == order.SO_ID
                                                       select
                                                       f.LU_Description).FirstOrDefault(),
                                                Currency = (from b in dbcontext.Tbl_Master_Order
                                                            join c in dbcontext.Tbl_Order_Details on b.SO_ID equals c.AGEN_TRAD_PO_ID
                                                            join f in dbcontext.Tbl_Currency_Master on b.SO_CusCurrency equals f.CURRENCY_ID
                                                            where c.AGEN_TRAD_PO_ID == order.SO_ID
                                                            select
                                                            f.CURRENCY_NAME).FirstOrDefault(),
                                                Price = detail.PRICE,
                                                Amount = (detail.PRICE*detail.QUANTITY),
                                                Comm = 0,
                                                Comm_Amt = 0,
                                                //--------

                                            }).ToList();
                        }
                        DataTable temp = new DataTable();
                        if (type == 3)
                        {
                            OrderDetails = (from order in dbcontext.Tbl_Master_Order
                                            join detail in dbcontext.Tbl_Order_Details on order.SO_ID equals detail.AGEN_TRAD_PO_ID
                                            where order.SO_OrderType == 3 && ((customer != 0 && order.SO_CutomerID == customer) || customer == 0)
                                                      && ((supplier != 0 && order.SO_SupplierID == supplier) || supplier == 0)
                                                      && ((duration != 0 && order.SO_OrderDate >= fromdt && order.SO_OrderDate <= todt) || duration == 0)
                                            select new BookingReport_CM
                                            {
                                                OrderID = order.SO_ID,
                                                salespersonID = order.SO_SalesPersonID,
                                                salespersonName = (dbcontext.Tbl_Master_User.Where(x => x.USER_ID == order.SO_SalesPersonID).Select(x => x.USER_NAME).FirstOrDefault()),
                                                customerID = order.SO_CutomerID,
                                                customerName = (dbcontext.Tbl_Master_CompanyDetails.Where(x => x.COM_ID == order.SO_CutomerID).Select(x => x.COM_NAME).FirstOrDefault()),
                                                supplierID = order.SO_SupplierID,
                                                supplierName = (dbcontext.Tbl_Master_CompanyDetails.Where(x => x.COM_ID == order.SO_SupplierID).Select(x => x.COM_NAME).FirstOrDefault()),
                                                CusPoNo = order.SO_CusPONO,
                                                PODate=order.SO_CusPODate,
                                                ProductId = detail.PRODUCT_ID,
                                                ProductName = (dbcontext.Tbl_Product_Master.Where(x => x.P_ID == detail.PRODUCT_ID).Select(x => x.P_ShortName).FirstOrDefault()),
                                                OrderedQty = detail.QUANTITY ?? 0,
                                                Uom = (from c in dbcontext.Tbl_Order_Details
                                                       join f in dbcontext.tbl_LookUp on c.UOM equals f.LU_Code
                                                       where c.AGEN_TRAD_PO_ID == order.SO_ID
                                                       select
                                                       f.LU_Description).FirstOrDefault(),
                                                Currency = (from b in dbcontext.Tbl_Master_Order
                                                            join c in dbcontext.Tbl_Order_Details on b.SO_ID equals c.AGEN_TRAD_PO_ID
                                                            join f in dbcontext.Tbl_Currency_Master on b.SO_CusCurrency equals f.CURRENCY_ID
                                                            where c.AGEN_TRAD_PO_ID == order.SO_ID
                                                            select
                                                            f.CURRENCY_NAME).FirstOrDefault(),
                                                Price = detail.PRICE,
                                                Amount = (detail.PRICE * detail.QUANTITY),
                                                Comm = 0,
                                                Comm_Amt = 0,

                                            }).ToList();
                            temp.Columns.Add("ID");
                            temp.Columns.Add("PID");
                            temp.Columns.Add("SchduleID");
                            temp.Columns.Add("salespersonID");
                            temp.Columns.Add("salespersonName");
                            temp.Columns.Add("customerID");
                            temp.Columns.Add("customerName");
                            temp.Columns.Add("PoNo");

                            temp.Columns.Add("PoDate");
                            temp.Columns.Add("Uom");
                            temp.Columns.Add("Currency");
                            temp.Columns.Add("Price");
                            temp.Columns.Add("Amount");
                            temp.Columns.Add("Comm");
                            temp.Columns.Add("Product");
                            temp.Columns.Add("Qty");
                            //Sales Person Wise
                            var salespersonWise = OrderDetails.GroupBy(x => x.salespersonID).Select(a => new
                            {
                                a.FirstOrDefault().salespersonID,
                                a.FirstOrDefault().salespersonName,
                                Quantity = a.Sum(b => b.OrderedQty),
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
                                dr1["PoNo"] = "";
                                dr1["PoDate"] = "";
                                dr1["Uom"] = "";
                                dr1["Currency"] = "";
                                dr1["Price"] = "";
                                dr1["Amount"] = "";
                                dr1["Comm"] = "";
                                dr1["Product"] = "";
                                dr1["Qty"] = salespersonWise[i].Quantity;
                                temp.Rows.Add(dr1);
                                //Customer Wise
                                var customerwisedata = OrderDetails.Where(a => a.salespersonID == salespersonWise[i].salespersonID).GroupBy(x => x.customerID).Select(a => new
                                {
                                    a.FirstOrDefault().customerID,
                                    a.FirstOrDefault().customerName,
                                    quantity = a.Sum(b => b.OrderedQty),
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
                                    dr2["PoNo"] = "";
                                    dr2["PoDate"] = "";
                                    dr2["Uom"] = "";
                                    dr2["Currency"] = "";
                                    dr2["Price"] = "";
                                    dr2["Amount"] = "";
                                    dr2["Comm"] = "";
                                    dr2["Product"] = "";
                                    temp.Rows.Add(dr2);

                                    //General Data
                                    var generaldata = OrderDetails.Where(a => a.salespersonID == salespersonWise[i].salespersonID && a.customerID == customerwisedata[j].customerID).Select(a => new
                                    {
                                        a.OrderID,
                                        a.CusPoNo,
                                        a.ProductName,
                                        a.PODate,
                                        a.Price,
                                        a.Amount,
                                        a.Comm,
                                        a.Uom,
                                        a.Currency,
                                        a.OrderedQty,
                                        a.ProductId,
                                    }).ToList();

                                    for (int l = 0; l < generaldata.Count(); l++)
                                    {
                                        DataRow dr4 = temp.NewRow();
                                        dr4["ID"] = "general" + generaldata[l].OrderID;
                                        dr4["PID"] = "customer" + customerwisedata[j].customerID + "salesperson" + salespersonWise[i].salespersonID;
                                        dr4["SchduleID"] = "";
                                        dr4["PoNo"] = generaldata[l].CusPoNo;
                                        dr4["PoDate"] = generaldata[l].PODate;
                                        dr4["Uom"] = generaldata[l].Uom;
                                        dr4["Currency"] =generaldata[l].Currency;
                                        dr4["Price"] = generaldata[l].Price;
                                        dr4["Amount"] = generaldata[l].Amount;
                                        dr4["Comm"] = "";
                                        dr4["Product"] = generaldata[l].ProductName;
                                        dr4["Qty"] = generaldata[l].OrderedQty;
                                        dr4["salespersonID"] = "";
                                        dr4["salespersonName"] = "";
                                        dr4["customerID"] = "";
                                        dr4["customerName"] = "";
                                        temp.Rows.Add(dr4);
                                    }



                                }
                            }
                        }

                        if (type != 3)
                        {

                            temp.Columns.Add("ID");
                            temp.Columns.Add("PID");
                            temp.Columns.Add("SchduleID");
                            temp.Columns.Add("salespersonID");
                            temp.Columns.Add("salespersonName");
                            temp.Columns.Add("customerID");
                            temp.Columns.Add("customerName");
                            temp.Columns.Add("supplierID");
                            temp.Columns.Add("supplierName");
                            temp.Columns.Add("ScNo");
                            temp.Columns.Add("Product");
                            temp.Columns.Add("Qty");
                            //suguna
                            temp.Columns.Add("Tradpo");
                            temp.Columns.Add("Sc_Date");
                            temp.Columns.Add("Po_No");
                            temp.Columns.Add("PoDate");
                            temp.Columns.Add("DealDate");
                            temp.Columns.Add("Deal_No");
                            temp.Columns.Add("Uom");
                            temp.Columns.Add("Currency");
                            temp.Columns.Add("Price");
                            temp.Columns.Add("Amount");
                            temp.Columns.Add("Comm");
                            temp.Columns.Add("Comm_Amt");
                            temp.Columns.Add("Comm_Euro");
                            //-------

                            //Sales Person Wise
                            var salespersonWise = OrderDetails.GroupBy(x => x.salespersonID).Select(a => new {
                                a.FirstOrDefault().salespersonID,
                                a.FirstOrDefault().salespersonName,
                                Amount=a.Sum(b=>b.Amount),
                                Quantity = a.Sum(b => b.OrderedQty),
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
                                dr1["supplierID"] = "";
                                dr1["supplierName"] = "";
                                dr1["ScNo"] = "";
                                dr1["Product"] = "";
                                dr1["Qty"] = salespersonWise[i].Quantity;
                                //suguna
                                dr1["Tradpo"] = "";
                                dr1["Sc_Date"] = "";
                                dr1["Po_No"] = "";
                                dr1["PoDate"] = "";
                                dr1["DealDate"] = "";
                                dr1["Deal_No"] = "";
                                dr1["Uom"] = "";
                                dr1["Currency"] = "";
                                dr1["Price"] = "";
                                dr1["Amount"] = salespersonWise[i].Amount;
                                dr1["Comm"] = "";
                                dr1["Comm_Amt"] = "";
                                dr1["Comm_Euro"] = "";
                                //-----
                                temp.Rows.Add(dr1);
                                //Customer Wise
                                var customerwisedata = OrderDetails.Where(a => a.salespersonID == salespersonWise[i].salespersonID).GroupBy(x => x.customerID).Select(a => new
                                {
                                    a.FirstOrDefault().customerID,
                                    a.FirstOrDefault().customerName,
                                    Amount=a.Sum(b=>b.Amount),
                                    quantity = a.Sum(b => b.OrderedQty),
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
                                    dr2["supplierID"] = "";
                                    dr2["supplierName"] = "";
                                    dr2["ScNo"] = "";
                                    dr2["Product"] = "";
                                    //suguna
                                    dr2["Tradpo"] = "";
                                    dr2["Sc_Date"] = "";
                                    dr2["Po_No"] = "";
                                    dr2["PoDate"] = "";
                                    dr2["DealDate"] = "";
                                    dr2["Deal_No"] = "";
                                    dr2["Uom"] = "";
                                    dr2["Currency"] = "";
                                    dr2["Price"] = "";
                                    dr2["Amount"] = customerwisedata[j].Amount;
                                    dr2["Comm"] = "";
                                    dr2["Comm_Amt"] = "";
                                    dr2["Comm_Euro"] = "";
                                    //-----
                                    temp.Rows.Add(dr2);
                                    //Supplier Wise
                                    var supplierwisedata = OrderDetails.Where(a => a.salespersonID == salespersonWise[i].salespersonID && a.customerID == customerwisedata[j].customerID).GroupBy(x => x.supplierID).Select(a => new
                                    {
                                        a.FirstOrDefault().supplierID,
                                        a.FirstOrDefault().supplierName,
                                        Amount=a.Sum(b=>b.Amount),
                                        quantity = a.Sum(b => b.OrderedQty),
                                    }).ToList();

                                    for (int k = 0; k < supplierwisedata.Count(); k++)
                                    {

                                        DataRow dr3 = temp.NewRow();
                                        dr3["ID"] = "supplier" + supplierwisedata[k].supplierID + "customer" + customerwisedata[j].customerID + "salesperson" + salespersonWise[i].salespersonID;
                                        dr3["PID"] = "customer" + customerwisedata[j].customerID + "salesperson" + salespersonWise[i].salespersonID;
                                        dr3["supplierID"] = supplierwisedata[k].supplierID;
                                        dr3["supplierName"] = supplierwisedata[k].supplierName;
                                        dr3["Qty"] = supplierwisedata[k].quantity;
                                        dr3["SchduleID"] = "";
                                        dr3["salespersonID"] = "";
                                        dr3["salespersonName"] = "";
                                        dr3["customerID"] = "";
                                        dr3["customerName"] = "";
                                        dr3["ScNo"] = "";
                                        dr3["Product"] = "";
                                        temp.Rows.Add(dr3);
                                        //suguna
                                        dr3["Tradpo"] = "";
                                        dr3["Sc_Date"] = "";
                                        dr3["Po_No"] = "";
                                        dr3["PoDate"] = "";
                                        dr3["DealDate"] = "";
                                        dr3["Deal_No"] = "";
                                        dr3["Uom"] = "";
                                        dr3["Currency"] = "";
                                        dr3["Price"] = "";
                                        dr3["Amount"] = supplierwisedata[k].Amount;
                                        dr3["Comm"] = "";
                                        dr3["Comm_Amt"] = "";
                                        dr3["Comm_Euro"] = "";
                                        //-----
                                        //General Data

                                        //SCNo Wise
                                        var SCnoWise =OrderDetails.Where(a=>a.salespersonID==salespersonWise[i].salespersonID && a.customerID == customerwisedata[j].customerID && a.supplierID == supplierwisedata[k].supplierID).Select(a => new
                                        {
                                            a.SupScNo,
                                            a.OrderCode,
                                            a.ProductName,
                                            a.OrderedQty,
                                            a.ProductId,
                                        }).ToList();
                                       
                                            var generaldata = OrderDetails.Where(a => a.salespersonID == salespersonWise[i].salespersonID && a.customerID == customerwisedata[j].customerID && a.supplierID == supplierwisedata[k].supplierID ).Select(a => new
                                            {
                                                a.OrderID,
                                                a.SupScNo,
                                                a.Sc_Date,
                                                a.PODate,
                                                a.Currency,
                                                a.Amount,
                                                a.Price,
                                                a.Uom,
                                                a.Po_No,
                                                a.Comm,
                                                a.ProductName,
                                                a.OrderedQty,
                                                a.ProductId,
                                            }).ToList();

                                            for (int l = 0; l < generaldata.Count(); l++)
                                            {
                                                DataRow dr4 = temp.NewRow();
                                                dr4["ID"] = "general" + generaldata[l].OrderID;
                                                dr4["PID"] = "supplier" + supplierwisedata[k].supplierID + "customer" + customerwisedata[j].customerID + "salesperson" + salespersonWise[i].salespersonID;
                                                dr4["SchduleID"] = generaldata[l].OrderID;
                                                dr4["ScNo"] = generaldata[l].SupScNo;
                                                dr4["Product"] = generaldata[l].ProductName;
                                                dr4["Qty"] = generaldata[l].OrderedQty;
                                                dr4["salespersonID"] = "";
                                                dr4["salespersonName"] = "";
                                                dr4["customerID"] = "";
                                                dr4["customerName"] = "";
                                                dr4["supplierID"] = "";
                                                dr4["supplierName"] = "";
                                            //suguna
                                               dr4["Tradpo"] = "";
                                               if(type==1)
                                            {
                                                dr4["Sc_Date"] = (generaldata[l].Sc_Date).ToString("dd-MM-yyyy");
                                            }
                                               else
                                            {
                                                dr4["Sc_Date"] = "";
                                            }
                                               dr4["Po_No"] = generaldata[l].Po_No;
                                               dr4["PoDate"] = (generaldata[l].PODate ?? DateTime.Now).ToString("dd-MM-yyyy");
                                            dr4["DealDate"] = "";
                                               dr4["Deal_No"] = "";
                                               dr4["Uom"] = generaldata[l].Uom;
                                               dr4["Currency"] = generaldata[l].Currency;
                                               dr4["Price"] = generaldata[l].Price;
                                               dr4["Amount"] = generaldata[l].Amount;
                                               dr4["Comm"] = generaldata[l].Comm;
                                               dr4["Comm_Amt"] = "";
                                               dr4["Comm_Euro"] = "";
                                            //-----
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