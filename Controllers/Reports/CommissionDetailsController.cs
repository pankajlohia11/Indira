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
    public class CommissionDetailsController : Controller
    {
        // GET: CommissionDetails
        BAL bal = new BAL();
        error_master objERR = new error_master();
        tbl_loginfo objLOG = new tbl_loginfo();
        EntityClasses dbcontext = new EntityClasses();
        public ActionResult CommissionDetails()
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
                        List<RegisterDetails_CM> shipmentdetails = new List<RegisterDetails_CM>();
                        string s = "";
                        var ShipmentSchdule = dbcontext.Tbl_CommissionRecieve.Where(m => m.DELETED == false).ToList();
                        for (int i = 0; i < ShipmentSchdule.Count; i++)
                        {
                            if (i == 0)
                            {
                                s = ShipmentSchdule[i].CR_ShipmentID.ToString();
                            }
                            else
                            {
                                s = s + "," + ShipmentSchdule[i].CR_ShipmentID.ToString();
                            }

                        }
                        var UID = new HashSet<string>(s.Split(',').ToList());
                        var schduleCode = dbcontext.Tbl_Shipment_Header.Where(m => m.DELETED == false  && !UID.Contains(m.S_ID.ToString())).Select(m => m.S_ID).Distinct().ToList();
                        shipmentdetails = (from commission in dbcontext.Tbl_GenerateDebitNote join
                                           ShipHead in dbcontext.Tbl_Shipment_Header on commission.DN_ShipmentID equals ShipHead.S_ID
                                           join a in dbcontext.Tbl_Shipment_Details on ShipHead.S_ID equals a.SD_PID
                                           join f in dbcontext.Tbl_Order_Details on a.SD_OrderDetailID equals f.ORDER_ID
                                           join or in dbcontext.Tbl_Master_Order on f.AGEN_TRAD_PO_ID equals or.SO_ID
                                           where schduleCode.Contains(ShipHead.S_ID)&& ShipHead.S_Type == 1 && ((customer != 0 && ShipHead.S_CustSup == customer) || customer == 0)
                                           && ((supplier != 0 && ShipHead.S_SupplierID == supplier) || supplier == 0)
                                           && ((duration != 0 && ShipHead.S_INV_DATE >= fromdt && ShipHead.S_INV_DATE <= todt) || duration == 0)
                                           select new RegisterDetails_CM
                                           {
                                               shipmentID = commission.DN_ID,
                                               customerID = ShipHead.S_CustSup,
                                               customerName = (dbcontext.Tbl_Master_CompanyDetails.Where(x => x.COM_ID == ShipHead.S_CustSup).Select(x => x.COM_NAME).FirstOrDefault()),
                                               supplierID = ShipHead.S_SupplierID,
                                               supplierName = (dbcontext.Tbl_Master_CompanyDetails.Where(x => x.COM_ID == ShipHead.S_SupplierID).Select(x => x.COM_NAME).FirstOrDefault()),
                                               InvAmount = commission.DN_ShipmentAmount,
                                              // CommissionAmt = commission.DN_ShipmentAmount,
                                              // CommissionAmtRece = commission.DN_ShipmentAmount,
                                               //suguna
                                               //Comm = (from b in dbcontext.Tbl_Master_Order
                                               //                join c in dbcontext.Tbl_Order_Details on b.SO_ID equals c.AGEN_TRAD_PO_ID
                                               //                where c.ORDER_ID == a.SD_OrderDetailID
                                               //                select
                                               //                b.SO_Commision).FirstOrDefault(),
                                               Comm=or.SO_Commision,
                                               Currency = (from b in dbcontext.Tbl_Master_Order
                                                              join c in dbcontext.Tbl_Order_Details on b.SO_ID equals c.AGEN_TRAD_PO_ID
                                                              join f in dbcontext.Tbl_Currency_Master on b.SO_CusCurrency equals f.CURRENCY_ID
                                                              where c.ORDER_ID == a.SD_OrderDetailID
                                                              select
                                                              f.CURRENCY_NAME).FirstOrDefault(),
                                               Debit_No = "",
                                               Debit_Date = DateTime.Today,
                                               RecdDate = DateTime.Today,
                                               OrderAmt = (from b in dbcontext.Tbl_Master_Order
                                                           join c in dbcontext.Tbl_Order_Details on b.SO_ID equals c.AGEN_TRAD_PO_ID
                                                           where c.ORDER_ID == a.SD_OrderDetailID
                                                           select (c.PRICE * a.SD_Quantity)).FirstOrDefault(),
                                               Comm_Disc = 0.00,
                                               User = (from b in dbcontext.Tbl_Master_Order
                                                           join c in dbcontext.Tbl_Order_Details on b.SO_ID equals c.AGEN_TRAD_PO_ID
                                                           join f in dbcontext.Tbl_Master_User on b.SO_SalesPersonID equals f.USER_ID
                                                           where c.ORDER_ID == a.SD_OrderDetailID
                                                           select
                                                           f.DISPLAY_NAME).FirstOrDefault(),
                                               Bal_Amt =00,
                                               Due=DateTime.Today,
                                               InvNo=ShipHead.S_INV_NO,
                                               Inv_Date=ShipHead.S_INV_DATE,
                                               Sc_No = (from b in dbcontext.Tbl_Master_Order
                                                             join c in dbcontext.Tbl_Order_Details on b.SO_ID equals c.AGEN_TRAD_PO_ID
                                                             where c.ORDER_ID == a.SD_OrderDetailID
                                                             select
                                                             b.SO_SupSCNO).FirstOrDefault(),
                                               Factor ="",
                                               //-----------
                                               }).Distinct().ToList();

                        var amountwise = shipmentdetails.GroupBy(x => x.shipmentID).Select(a => new {
                            a.FirstOrDefault().shipmentID,
                            a.FirstOrDefault().InvNo,
                            a.FirstOrDefault().InvDate,
                            a.FirstOrDefault().Sc_No,
                            a.FirstOrDefault().Currency,
                            a.FirstOrDefault().customerID,
                            a.FirstOrDefault().CommissionAmtRece,
                            a.FirstOrDefault().supplierName,
                            a.FirstOrDefault().customerName,
                            a.FirstOrDefault().ETD,
                            a.FirstOrDefault().User,
                            a.FirstOrDefault().Comm,
                            a.FirstOrDefault().supplierID,
                            a.FirstOrDefault().salespersonID,
                            a.FirstOrDefault().salespersonName,
                            a.FirstOrDefault().InvAmount,
                            CommissionAmt = a.Sum(b => b.Comm * b.OrderAmt / 100),
                        }).ToList();
                        DataTable temp = new DataTable();

                        temp.Columns.Add("ID");
                        temp.Columns.Add("PID");
                        temp.Columns.Add("customerID");
                        temp.Columns.Add("customerName");
                        temp.Columns.Add("supplierID");
                        temp.Columns.Add("supplierName");
                        temp.Columns.Add("InvAmount");
                        temp.Columns.Add("CommissionAmt");
                        temp.Columns.Add("CommissionAmtRece");
                        //suguna
                        temp.Columns.Add("Currency");
                        temp.Columns.Add("Debit_No");
                        temp.Columns.Add("Debit_Date");
                        temp.Columns.Add("Comm");
                        temp.Columns.Add("Recd_Date");
                        temp.Columns.Add("Comm_Disc");
                        temp.Columns.Add("User");
                        temp.Columns.Add("Bal_Amt");
                        temp.Columns.Add("Due");
                        temp.Columns.Add("Inv_No");
                        temp.Columns.Add("Inv_Date");
                        temp.Columns.Add("Sc_No");
                     //   temp.Columns.Add("Due");
                        temp.Columns.Add("Factor");
                        //-------------

                        //Customer Wise
                        var customerwisedata = amountwise.GroupBy(x => x.customerID).Select(a => new
                            {
                                a.FirstOrDefault().customerID,
                                a.FirstOrDefault().customerName,
                                InvAmount = a.Sum(b => b.InvAmount),
                                a.FirstOrDefault().Currency,
                            CommissionAmt = a.Sum(b => b.CommissionAmt),
                                CommissionAmtRece = a.Sum(b => b.CommissionAmtRece),
                            }).ToList();

                            for (int j = 0; j < customerwisedata.Count(); j++)
                            {
                                DataRow dr2 = temp.NewRow();
                                dr2["ID"] = "customer" + customerwisedata[j].customerID ;
                                dr2["PID"] = "0";
                                dr2["customerID"] = customerwisedata[j].customerID;
                                dr2["customerName"] = customerwisedata[j].customerName;
                                dr2["InvAmount"] = customerwisedata[j].InvAmount;
                                dr2["CommissionAmt"] = customerwisedata[j].CommissionAmt;
                                dr2["supplierID"] = "";
                                dr2["supplierName"] = "";
                                dr2["CommissionAmtRece"] = customerwisedata[j].CommissionAmtRece;
                            //suguna
                                dr2["Currency"] = customerwisedata[j].Currency;
                                dr2["Debit_No"] = "";
                                dr2["Debit_Date"] = "";
                                dr2["Comm"] = "";
                                dr2["Recd_Date"] = "";
                                dr2["Comm_Disc"] = "";
                                dr2["User"] = "";
                                dr2["Bal_Amt"] = "";
                                dr2["Due"] = "";
                                dr2["Inv_No"] = "";
                                dr2["Inv_Date"] = "";
                                dr2["Sc_No"] = "";
                              //  dr2["Due"] ="";
                                dr2["factor"] = "";
                            //-------
                                temp.Rows.Add(dr2);
                                //Supplier Wise
                                var supplierwisedata = amountwise.Where(a =>  a.customerID == customerwisedata[j].customerID).GroupBy(x => x.supplierID).Select(a => new
                                {
                                    a.FirstOrDefault().supplierID,
                                    a.FirstOrDefault().supplierName,
                                    InvAmount = a.Sum(b => b.InvAmount),
                                    a.FirstOrDefault().Currency,
                                    CommissionAmt = a.Sum(b => b.CommissionAmt),
                                    CommissionAmtRece = a.Sum(b => b.CommissionAmtRece),
                                }).ToList();

                                for (int k = 0; k < supplierwisedata.Count(); k++)
                                {

                                    DataRow dr3 = temp.NewRow();
                                    dr3["ID"] = "supplier" + supplierwisedata[k].supplierID + "customer" + customerwisedata[j].customerID ;
                                    dr3["PID"] = "customer" + customerwisedata[j].customerID ;
                                    dr3["supplierID"] = supplierwisedata[k].supplierID;
                                    dr3["supplierName"] = supplierwisedata[k].supplierName;
                                    dr3["InvAmount"] = supplierwisedata[k].InvAmount;
                                    dr3["CommissionAmt"] = supplierwisedata[k].CommissionAmt;
                                    dr3["customerID"] = "";
                                    dr3["customerName"] = "";
                                    dr3["CommissionAmtRece"] = supplierwisedata[k].CommissionAmtRece;
                                //suguna
                                   dr3["Currency"] = supplierwisedata[k].Currency;
                                   dr3["Debit_No"] = "";
                                   dr3["Debit_Date"] = "";
                                   dr3["Comm"] = "";
                                   dr3["Recd_Date"] = "";
                                   dr3["Comm_Disc"] = "";
                                   dr3["User"] = "";
                                   dr3["Bal_Amt"] = "";
                                   dr3["Due"] = "";
                                   dr3["Inv_No"] = "";
                                   dr3["Inv_Date"] = "";
                                   dr3["Sc_No"] = "";
                                 //  dr3["Due"] = "";
                                   dr3["factor"] = "";
                                //-------
                                temp.Rows.Add(dr3);
                                    //General Data
                                    var generaldata = amountwise.Where(a =>  a.customerID == customerwisedata[j].customerID && a.supplierID == supplierwisedata[k].supplierID).Select(a => new
                                    {
                                        a.shipmentID,
                                        a.ETD,
                                        a.InvNo,
                                        a.InvDate,
                                        a.InvAmount,
                                        a.Currency,
                                        a.User,
                                        a.Comm,
                                        a.Sc_No,
                                        a.CommissionAmt,
                                        a.CommissionAmtRece,
                                    }).ToList();

                                    for (int l = 0; l < generaldata.Count(); l++)
                                    {
                                        DataRow dr4 = temp.NewRow();
                                        dr4["ID"] = "general" + generaldata[l].shipmentID;
                                        dr4["PID"] = "supplier" + supplierwisedata[k].supplierID + "customer" + customerwisedata[j].customerID ;
                                        dr4["CommissionAmtRece"] = generaldata[l].CommissionAmtRece;
                                        dr4["InvAmount"] = generaldata[l].InvAmount;
                                        dr4["CommissionAmt"] = generaldata[l].CommissionAmt;
                                        dr4["customerID"] = "";
                                        dr4["customerName"] = "";
                                        dr4["supplierID"] = "";
                                        dr4["supplierName"] = "";
                                    //suguna
                                    dr4["Currency"] = generaldata[l].Currency;
                                    dr4["Debit_No"] = "";
                                    dr4["Debit_Date"] = "";
                                    dr4["Comm"] = generaldata[l].Comm;
                                    dr4["Recd_Date"] = "";
                                    dr4["Comm_Disc"] = "";
                                    dr4["User"] = generaldata[l].User;
                                    dr4["Bal_Amt"] = (generaldata[l].CommissionAmt-generaldata[l].CommissionAmtRece);
                                    dr4["Due"] = "";
                                    dr4["Inv_No"] = generaldata[l].InvNo;
                                    dr4["Inv_Date"] = (generaldata[l].InvDate ?? DateTime.Now).ToString("dd-MM-yyyy");
                                    dr4["Sc_No"] = generaldata[l].Sc_No;
                                    //dr4["Due"] = "";
                                    dr4["factor"] = "";
                                    //-------
                                    temp.Rows.Add(dr4);
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