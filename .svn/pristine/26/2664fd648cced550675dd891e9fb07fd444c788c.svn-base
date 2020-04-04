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
    public class RegisterDetailsController : Controller
    {
        // GET: RegisterDetails
        BAL bal = new BAL();
        error_master objERR = new error_master();
        tbl_loginfo objLOG = new tbl_loginfo();
        EntityClasses dbcontext = new EntityClasses();
        public ActionResult RegisterDetails()
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
        public JsonResult GetAgencyShipmentDetails(int salesperson, int customer, int supplier, int duration, string fromdate, string todate, int type,decimal RegType)
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
                        if (type == 1)
                        {
                            shipmentdetails = (from commission in dbcontext.Tbl_CommissionRecieve
                                               join Shiphead in dbcontext.Tbl_Shipment_Header on commission.CR_ShipmentID equals Shiphead.S_ID
                                               //join shipdet in dbcontext.Tbl_Shipment_Details on Shiphead.S_ID equals shipdet.SD_PID
                                               //join f in dbcontext.Tbl_Order_Details on shipdet.SD_OrderDetailID equals f.ORDER_ID
                                               //join or in dbcontext.Tbl_Master_Order on f.AGEN_TRAD_PO_ID equals or.SO_ID
                                               where ((customer != 0 && Shiphead.S_CustSup == customer) || customer == 0)
                                               && ((supplier != 0 && Shiphead.S_SupplierID == supplier) || supplier == 0)
                                               && ((duration != 0 && Shiphead.S_INV_DATE >= fromdt && Shiphead.S_INV_DATE <= todt) || duration == 0)
                                               select new RegisterDetails_CM
                                               {

                                                   customerID = Shiphead.S_CustSup,
                                                   customerName = (dbcontext.Tbl_Master_CompanyDetails.Where(x => x.COM_ID == Shiphead.S_CustSup).Select(x => x.COM_NAME).FirstOrDefault()),
                                                   supplierID = Shiphead.S_SupplierID,
                                                   supplierName = (dbcontext.Tbl_Master_CompanyDetails.Where(x => x.COM_ID == Shiphead.S_SupplierID).Select(x => x.COM_NAME).FirstOrDefault()),
                                                   InvNo = Shiphead.S_INV_NO,
                                                   InvDate = Shiphead.S_INV_DATE,
                                                   InvAmount = Shiphead.S_INV_AMT ?? 0,
                                                   CommissionAmt = commission.CR_CommissionAmount,
                                                   //suguna
                                                   Product = "",
                                                   Price = 0,
                                                   Quantity = 0,
                                                   //-----
                                               }).ToList();
                        }
                        if (type == 2)
                        {

                            shipmentdetails = (from commission in dbcontext.Tbl_OneToOneInvoice join
                                               ShipHead in dbcontext.Tbl_Shipment_Header on commission.OTOI_ShipmentID equals ShipHead.S_ID
                                               //join shipdet in dbcontext.Tbl_Shipment_Details on ShipHead.S_ID equals shipdet.SD_PID
                                               //join f in dbcontext.Tbl_Order_Details on shipdet.SD_OrderDetailID equals f.ORDER_ID
                                               //join or in dbcontext.Tbl_Master_Order on f.AGEN_TRAD_PO_ID equals or.SO_ID
                                               where ((salesperson != 0 && commission.OTOI_SalesPerson == salesperson) || salesperson == 0)
                                               && ((customer != 0 && commission.OTOI_CustomerID == customer) || customer == 0)
                                               && ((supplier != 0 && ShipHead.S_SupplierID == supplier) || supplier == 0)
                                               && ((duration != 0 && commission.OTOI_InvoiceDate >= fromdt && commission.OTOI_InvoiceDate <= todt) || duration == 0)
                                               select new RegisterDetails_CM
                                               {
                                                   customerID = commission.OTOI_CustomerID,
                                                   customerName = (dbcontext.Tbl_Master_CompanyDetails.Where(x => x.COM_ID == commission.OTOI_CustomerID).Select(x => x.COM_NAME).FirstOrDefault()),
                                                   supplierID = ShipHead.S_SupplierID,
                                                   supplierName = (dbcontext.Tbl_Master_CompanyDetails.Where(x => x.COM_ID == ShipHead.S_SupplierID).Select(x => x.COM_NAME).FirstOrDefault()),
                                                   InvNo = commission.OTOI_Code,
                                                   InvDate = commission.OTOI_InvoiceDate,
                                                   InvAmount = commission.OTOI_InvoiceAmount,
                                                   CommissionAmt = commission.OTOI_InvoiceAmount,
                                                   //suguna
                                                   Product = "",
                                                   Price = 0,
                                                   Quantity = 0,
                                                   //-----
                                               }).ToList();
                        }
                        DataTable temp = new DataTable();
                        if (type == 3)
                        {
                            if(RegType!=2)
                            {
                                shipmentdetails = (from commission in dbcontext.Tbl_OneToManyInvoice join
                                                   ShipHead in dbcontext.Tbl_DespatchHeader on commission.OTMI_DespatchIDs equals ShipHead.D_ID
                                               join shipdet in dbcontext.Tbl_DespatchDetails on ShipHead.D_ID equals shipdet.DD_PID
                                               join f in dbcontext.Tbl_Order_Details on shipdet.DD_OrderDetailID equals f.ORDER_ID
                                               join or in dbcontext.Tbl_Master_Order on f.AGEN_TRAD_PO_ID equals or.SO_ID
                                                   where ((salesperson != 0 && commission.OTMI_SalesPerson == salesperson) || salesperson == 0)
                                                   && ((customer != 0 && commission.OTMI_CustomerID == customer) || customer == 0)

                                                   && ((duration != 0 && commission.OTMI_InvoiceDate >= fromdt && commission.OTMI_InvoiceDate <= todt) || duration == 0)
                                                   select new RegisterDetails_CM
                                                   {
                                                       customerID = commission.OTMI_CustomerID,
                                                       customerName = (dbcontext.Tbl_Master_CompanyDetails.Where(x => x.COM_ID == commission.OTMI_CustomerID).Select(x => x.COM_NAME).FirstOrDefault()),
                                                       InvNo = commission.OTMI_Code,
                                                       InvDate = commission.OTMI_InvoiceDate,
                                                       InvAmount = commission.OTMI_InvoiceAmount,
                                                       Product = (dbcontext.Tbl_Product_Master.Where(x => x.P_ID == shipdet.DD_ProductID).Select(x => x.P_Name).FirstOrDefault()),
                                                       Price = f.PRICE,
                                                       Quantity = shipdet.DD_DespatchQuantity ?? 0,
                                                   }).ToList();
                            }
                           
                            temp.Columns.Add("ID");
                            temp.Columns.Add("PID");
                            temp.Columns.Add("customerID");
                            temp.Columns.Add("customerName");
                            temp.Columns.Add("InvNo");
                            temp.Columns.Add("InvDate");
                            temp.Columns.Add("InvAmount");
                            temp.Columns.Add("Product");
                            temp.Columns.Add("Price");
                            temp.Columns.Add("Quantity");
                                //Customer Wise
                                var customerwisedata = shipmentdetails.GroupBy(x => x.customerID).Select(a => new
                                {
                                    a.FirstOrDefault().customerID,
                                    a.FirstOrDefault().customerName,
                                    InvAmount = a.Sum(b => b.InvAmount),
                                }).ToList();

                                for (int j = 0; j < customerwisedata.Count(); j++)
                                {
                                    DataRow dr2 = temp.NewRow();
                                    dr2["ID"] = "customer" + customerwisedata[j].customerID ;
                                    dr2["PID"] = "0";
                                    dr2["customerID"] = customerwisedata[j].customerID;
                                    dr2["customerName"] = customerwisedata[j].customerName;
                                    dr2["InvAmount"] = customerwisedata[j].InvAmount;
                                    dr2["InvNo"] = "";
                                    dr2["InvDate"] = "";
                                    dr2["Product"] = "";
                                    dr2["Price"] = "";
                                    dr2["Quantity"] = "";
                                    temp.Rows.Add(dr2);
                                    //General Data
                                    var generaldata = shipmentdetails.Where(a => a.customerID == customerwisedata[j].customerID).Select(a => new
                                    {
                                        a.customerID,
                                        a.InvNo,
                                        a.InvDate,
                                        a.Product,
                                        a.Price,
                                        a.Quantity,
                                        a.InvAmount,
                                    }).ToList();

                                    for (int l = 0; l < generaldata.Count(); l++)
                                    {
                                        DataRow dr4 = temp.NewRow();
                                    dr4["ID"] = "general" + generaldata[l].InvNo;
                                        dr4["PID"] = "customer" + customerwisedata[j].customerID ;
                                        dr4["InvNo"] = generaldata[l].InvNo;
                                        dr4["InvDate"] = (generaldata[l].InvDate ?? DateTime.Now).ToString("dd-MM-yyyy");
                                        dr4["InvAmount"] = generaldata[l].InvAmount;
                                        dr4["Product"] = generaldata[l].Product;
                                        dr4["Price"] = generaldata[l].Price;
                                        dr4["Quantity"] = generaldata[l].Quantity;
                                        dr4["customerID"] = "";
                                        dr4["customerName"] = "";
                                        temp.Rows.Add(dr4);
                                    }
                                }
                           
                        }

                        if (type != 3)
                        {
                            if(RegType==1)
                            {
                                temp.Columns.Add("ID");
                                temp.Columns.Add("PID");
                                temp.Columns.Add("customerID");
                                temp.Columns.Add("customerName");
                                temp.Columns.Add("supplierID");
                                temp.Columns.Add("supplierName");
                                //temp.Columns.Add("ETD");
                                temp.Columns.Add("InvNo");
                                temp.Columns.Add("InvDate");
                                temp.Columns.Add("CommissionAmt");
                                //suguna
                                temp.Columns.Add("Product");
                                temp.Columns.Add("Quantity");
                                temp.Columns.Add("Price");                                
                                //--------

                                //Customer Wise
                                var customerwisedata = shipmentdetails.Where(a=>a.CommissionAmt!=0).GroupBy(x => x.customerID).Select(a => new
                                {
                                    a.FirstOrDefault().customerID,
                                    a.FirstOrDefault().customerName,
                                    InvAmount = a.Sum(b => b.InvAmount),
                                    CommissionAmt = a.Sum(b => b.CommissionAmt),
                                }).ToList();

                                for (int j = 0; j < customerwisedata.Count(); j++)
                                {
                                    DataRow dr2 = temp.NewRow();
                                    dr2["ID"] = "customer" + customerwisedata[j].customerID;
                                    dr2["PID"] = "0";
                                    dr2["customerID"] = customerwisedata[j].customerID;
                                    dr2["customerName"] = customerwisedata[j].customerName;
                                    dr2["CommissionAmt"] = customerwisedata[j].CommissionAmt;
                                    dr2["InvNo"] = "";
                                    dr2["InvDate"] = "";
                                    //suguna
                                    dr2["Product"] = "";
                                    dr2["Quantity"] = "";
                                    dr2["Price"] = "";
                                    //------
                                    temp.Rows.Add(dr2);

                                    //General Data
                                    var generaldata = shipmentdetails.Where(a => a.customerID == customerwisedata[j].customerID && a.CommissionAmt != 0).Select(a => new
                                    {
                                        a.shipmentID,
                                        a.ETD,
                                        a.InvNo,
                                        a.InvDate,
                                        a.Price,
                                        a.Product,
                                        a.Quantity,
                                        a.InvAmount,
                                        a.CommissionAmt,
                                    }).ToList();

                                    for (int l = 0; l < generaldata.Count(); l++)
                                    {
                                        DataRow dr4 = temp.NewRow();
                                        dr4["ID"] = "general" + generaldata[l].shipmentID;
                                        dr4["PID"] = "customer" + customerwisedata[j].customerID;
                                        dr4["InvNo"] = generaldata[l].InvNo;
                                        dr4["InvDate"] = (generaldata[l].InvDate ?? DateTime.Now).ToString("dd-MM-yyyy");
                                        dr4["CommissionAmt"] = generaldata[l].CommissionAmt;
                                        dr4["customerID"] = "";
                                        dr4["customerName"] = "";
                                        //suguna
                                        dr4["Product"] = "";
                                        dr4["Quantity"] = "";
                                        dr4["Price"] = "";
                                        //------
                                        temp.Rows.Add(dr4);
                                    }



                                }

                            }
                            if(RegType==2)
                            {
                                temp.Columns.Add("ID");
                                temp.Columns.Add("PID");
                                temp.Columns.Add("supplierID");
                                temp.Columns.Add("supplierName");
                                temp.Columns.Add("InvNo");
                                temp.Columns.Add("InvDate");
                                temp.Columns.Add("CommissionAmt");
                                //suguna
                                temp.Columns.Add("Product");
                                temp.Columns.Add("Quantity");
                                temp.Columns.Add("Price");
                                //--------


                                //Supplier Wise
                                var Supplierwisedata = shipmentdetails.Where(a=> a.CommissionAmt != 0).GroupBy(x => x.supplierID).Select(a => new
                                {
                                    a.FirstOrDefault().supplierID,
                                    a.FirstOrDefault().supplierName,
                                    InvAmount = a.Sum(b => b.InvAmount),
                                    CommissionAmt = a.Sum(b => b.CommissionAmt),
                                }).ToList();

                                for (int j = 0; j < Supplierwisedata.Count(); j++)
                                {
                                    DataRow dr2 = temp.NewRow();
                                    dr2["ID"] = "Supplier" + Supplierwisedata[j].supplierID;
                                    dr2["PID"] = "0";
                                    dr2["SupplierID"] = Supplierwisedata[j].supplierID;
                                    dr2["SupplierName"] = Supplierwisedata[j].supplierName;
                                    dr2["CommissionAmt"] = Supplierwisedata[j].CommissionAmt;
                                    dr2["InvNo"] = "";
                                    dr2["InvDate"] = "";
                                    //suguna
                                    dr2["Product"] = "";
                                    dr2["Quantity"] = "";
                                    dr2["Price"] = "";
                                    //------
                                    temp.Rows.Add(dr2);

                                    //General Data
                                    var generaldata = shipmentdetails.Where(a => a.supplierID == Supplierwisedata[j].supplierID && a.CommissionAmt != 0).Select(a => new
                                    {
                                        a.shipmentID,
                                        a.ETD,
                                        a.InvNo,
                                        a.InvDate,
                                        a.InvAmount,
                                        a.Product,
                                        a.Quantity,
                                        a.Price,
                                        a.CommissionAmt,
                                    }).Distinct().ToList();

                                    for (int l = 0; l < generaldata.Count(); l++)
                                    {
                                        DataRow dr4 = temp.NewRow();
                                        dr4["ID"] = "general" + generaldata[l].shipmentID;
                                        dr4["PID"] = "Supplier" + Supplierwisedata[j].supplierID;
                                        dr4["InvNo"] = generaldata[l].InvNo;
                                        dr4["InvDate"] = (generaldata[l].InvDate ?? DateTime.Now).ToString("dd-MM-yyyy");
                                        dr4["CommissionAmt"] = generaldata[l].CommissionAmt;
                                        dr4["SupplierID"] = "";
                                        dr4["SupplierName"] = "";
                                        //suguna
                                        dr4["Product"] = "";
                                        dr4["Quantity"] = "";
                                        dr4["Price"] = "";
                                        //------
                                        temp.Rows.Add(dr4);
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