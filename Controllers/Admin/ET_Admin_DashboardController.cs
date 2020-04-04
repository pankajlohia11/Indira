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

namespace Euro.Controllers.Admin
{
    public class ET_Admin_DashboardController : Controller
    {
        // GET: ET_Admin_Dashboard
        BAL bal = new BAL();
        error_master objERR = new error_master();
        EntityClasses dbcontext = new EntityClasses();
        public ActionResult ET_Admin_Dashboard()
        {
            ViewBag.Login_Name = Session["DisplayName"].ToString();
            return View();
        }
        public JsonResult GetSalesPerson(decimal orgtype)
        {

            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                {
                    dbcontext.Configuration.ProxyCreationEnabled = false;
                    int comkey = Convert.ToInt32(Session["Companykey"].ToString());
                    var ObjSales_Org = dbcontext.Tbl_Sales_Organization.Where(m => m.COM_KEY == comkey && m.DELETED == false && m.sales_Organization == orgtype).ToList();
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
                    var Users = dbcontext.Tbl_Master_User.Where(m => m.USER_ID > 0 && m.COM_KEY == comkey && UID.Contains(m.USER_ID)).Distinct().ToList();
                    var json = new JavaScriptSerializer().Serialize(Users);
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
        public JsonResult GetMonthwiseSalesForAgency(int id)
        {
            //int year = DateTime.Now.Year-1;
            int year = DateTime.Now.Year;
            DateTime firstDay = new DateTime(year, 1, 1);
            DateTime todayDate = DateTime.Now.Date;
            DateTime monthday = new DateTime(year, DateTime.Now.Month, 1);
            List<ShipmentReport_CM> shipmentdetails = new List<ShipmentReport_CM>();
            List<ShipmentReport_CM> shipmentdetailsForTrading = new List<ShipmentReport_CM>();
            List<ShipmentReport_CM> shipmentdetailsForStore = new List<ShipmentReport_CM>();
            //Year wise Details
            shipmentdetails = (from shpheader in dbcontext.Tbl_Shipment_Header
                               join commission in dbcontext.Tbl_CommissionRecieve on shpheader.S_ID equals commission.CR_ShipmentID
                               where shpheader.S_Type == 1 &&(DbFunctions.TruncateTime(shpheader.S_INV_DATE) >=DbFunctions.TruncateTime(firstDay) && DbFunctions.TruncateTime(shpheader.S_INV_DATE) <=DbFunctions.TruncateTime(todayDate))
                               select new ShipmentReport_CM
                               {
                                   salespersonID = shpheader.S_SalesPerson,
                                   salespersonName = (dbcontext.Tbl_Master_User.Where(x => x.USER_ID == shpheader.S_SalesPerson).Select(x => x.USER_NAME).FirstOrDefault()),
                                   CommissionAmt = commission.CR_CommissionRecieved,
                                   CREATED_DATE = shpheader.S_INV_DATE,
                                  // monthname = commission.CR_Date.Value.Month
                               }).ToList();
            shipmentdetailsForTrading = (from shpheader in dbcontext.Tbl_Shipment_Header
                               join commission in dbcontext.Tbl_OneToOneInvoice on shpheader.S_ID equals commission.OTOI_ShipmentID
                               where shpheader.S_Type == 2 && (DbFunctions.TruncateTime(commission.OTOI_InvoiceDate) >= DbFunctions.TruncateTime(firstDay) && DbFunctions.TruncateTime(commission.OTOI_InvoiceDate) <= DbFunctions.TruncateTime(todayDate))
                               select new ShipmentReport_CM
                               {
                                   salespersonID = shpheader.S_SalesPerson,
                                   salespersonName = (dbcontext.Tbl_Master_User.Where(x => x.USER_ID == shpheader.S_SalesPerson).Select(x => x.USER_NAME).FirstOrDefault()),
                                   CommissionAmt = commission.OTOI_InvoiceAmount,
                                   CREATED_DATE = commission.OTOI_InvoiceDate,
                                   // monthname = commission.CR_Date.Value.Month
                               }).ToList();
            shipmentdetailsForStore = (from shpheader in dbcontext.Tbl_DespatchHeader
                                         join commission in dbcontext.Tbl_OneToManyInvoice on shpheader.D_ID equals commission.OTMI_DespatchIDs
                                         where  (DbFunctions.TruncateTime(commission.OTMI_InvoiceDate) >= DbFunctions.TruncateTime(firstDay) && DbFunctions.TruncateTime(commission.OTMI_InvoiceDate) <= DbFunctions.TruncateTime(todayDate))
                                         select new ShipmentReport_CM
                                         {
                                             salespersonID = shpheader.D_SalesPerson,
                                             salespersonName = (dbcontext.Tbl_Master_User.Where(x => x.USER_ID == shpheader.D_SalesPerson).Select(x => x.USER_NAME).FirstOrDefault()),
                                             CommissionAmt = commission.OTMI_InvoiceAmount,
                                             CREATED_DATE = commission.OTMI_InvoiceDate,
                                             // monthname = commission.CR_Date.Value.Month
                                         }).ToList();

            //Year wise sales for Agency


            List<ShipmentReport_CM> AgencysalesYearWise = shipmentdetails.GroupBy(_ => new { _.CREATED_DATE.Value.Year })
    .Select(g => new ShipmentReport_CM
    {
        CREATED_DATE = g.FirstOrDefault().CREATED_DATE,
        CommissionAmt = g.Where(_ => _.CommissionAmt != 0).Sum(_ => _.CommissionAmt),
    }).ToList();

            //Year wise sales for Trading

            List<ShipmentReport_CM> TradingsalesYearWise = shipmentdetailsForTrading.GroupBy(_ => new { _.CREATED_DATE.Value.Year })
    .Select(g => new ShipmentReport_CM
    {
        CREATED_DATE = g.FirstOrDefault().CREATED_DATE,
        CommissionAmt = g.Where(_ => _.CommissionAmt != 0).Sum(_ => _.CommissionAmt),
    }).ToList();
            //Year wise sales for Store

            List<ShipmentReport_CM> StoresalesYearWise = shipmentdetailsForStore.GroupBy(_ => new { _.CREATED_DATE.Value.Year })
   .Select(g => new ShipmentReport_CM
   {
       CREATED_DATE = g.FirstOrDefault().CREATED_DATE,
       CommissionAmt = g.Where(_ => _.CommissionAmt != 0).Sum(_ => _.CommissionAmt),
   }).ToList();

            //Month wise Details

            List<ShipmentReport_CM> AgencyMonthwiseDetails = new List<ShipmentReport_CM>();
            List<ShipmentReport_CM> TradingMonthwiseDetails = new List<ShipmentReport_CM>();
            List<ShipmentReport_CM> StoreMonthwiseDetails = new List<ShipmentReport_CM>();
            AgencyMonthwiseDetails = (from shpheader in dbcontext.Tbl_Shipment_Header
                                join commission in dbcontext.Tbl_CommissionRecieve on shpheader.S_ID equals commission.CR_ShipmentID
                                where shpheader.S_Type == 1 && (DbFunctions.TruncateTime(shpheader.S_INV_DATE) >= DbFunctions.TruncateTime(monthday) && DbFunctions.TruncateTime(shpheader.S_INV_DATE) <= DbFunctions.TruncateTime(todayDate))
                                select new ShipmentReport_CM
                                {
                                    salespersonID = shpheader.S_SalesPerson,
                                    salespersonName = (dbcontext.Tbl_Master_User.Where(x => x.USER_ID == shpheader.S_SalesPerson).Select(x => x.USER_NAME).FirstOrDefault()),
                                    CommissionAmt = commission.CR_CommissionRecieved,
                                    CREATED_DATE = shpheader.S_INV_DATE,

                                }).ToList();
            TradingMonthwiseDetails = (from shpheader in dbcontext.Tbl_Shipment_Header
                                      join commission in dbcontext.Tbl_OneToOneInvoice on shpheader.S_ID equals commission.OTOI_ShipmentID
                                      where shpheader.S_Type == 2 && (DbFunctions.TruncateTime(commission.OTOI_InvoiceDate) >= DbFunctions.TruncateTime(monthday) && DbFunctions.TruncateTime(commission.OTOI_InvoiceDate) <= DbFunctions.TruncateTime(todayDate))
                                      select new ShipmentReport_CM
                                      {
                                          salespersonID = shpheader.S_SalesPerson,
                                          salespersonName = (dbcontext.Tbl_Master_User.Where(x => x.USER_ID == shpheader.S_SalesPerson).Select(x => x.USER_NAME).FirstOrDefault()),
                                          CommissionAmt = commission.OTOI_InvoiceAmount,
                                          CREATED_DATE = commission.OTOI_InvoiceDate,

                                      }).ToList();
            StoreMonthwiseDetails = (from shpheader in dbcontext.Tbl_DespatchHeader
                                      join commission in dbcontext.Tbl_OneToManyInvoice on shpheader.D_ID equals commission.OTMI_DespatchIDs
                                      where(DbFunctions.TruncateTime(commission.OTMI_InvoiceDate) >= DbFunctions.TruncateTime(monthday) && DbFunctions.TruncateTime(commission.OTMI_InvoiceDate) <= DbFunctions.TruncateTime(todayDate))
                                      select new ShipmentReport_CM
                                      {
                                          salespersonID = shpheader.D_SalesPerson,
                                          salespersonName = (dbcontext.Tbl_Master_User.Where(x => x.USER_ID == shpheader.D_SalesPerson).Select(x => x.USER_NAME).FirstOrDefault()),
                                          CommissionAmt = commission.OTMI_InvoiceAmount,
                                          CREATED_DATE = commission.OTMI_InvoiceDate,

                                      }).ToList();

            //Agency Month wise Details
            List<ShipmentReport_CM> AgencysalesMonthWise = AgencyMonthwiseDetails.GroupBy(_ => new { _.CREATED_DATE.Value.Month })
.Select(g => new ShipmentReport_CM
{
CREATED_DATE = g.FirstOrDefault().CREATED_DATE,
CommissionAmt = g.Where(_ => _.CommissionAmt != 0).Sum(_ => _.CommissionAmt),
}).ToList();
            //Trading Month wise Details
            List<ShipmentReport_CM> TradingsalesMonthWise = TradingMonthwiseDetails.GroupBy(_ => new { _.CREATED_DATE.Value.Month })
.Select(g => new ShipmentReport_CM
{
CREATED_DATE = g.FirstOrDefault().CREATED_DATE,
CommissionAmt = g.Where(_ => _.CommissionAmt != 0).Sum(_ => _.CommissionAmt),
}).ToList();
            //Store Month wise Details
            List<ShipmentReport_CM> StoresalesMonthWise = StoreMonthwiseDetails.GroupBy(_ => new { _.CREATED_DATE.Value.Month })
.Select(g => new ShipmentReport_CM
{
CREATED_DATE = g.FirstOrDefault().CREATED_DATE,
CommissionAmt = g.Where(_ => _.CommissionAmt != 0).Sum(_ => _.CommissionAmt),
}).ToList();
            var result = new { AgencysalesYearWise, TradingsalesYearWise, StoresalesYearWise , AgencysalesMonthWise , TradingsalesMonthWise , StoresalesMonthWise };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetAgencySales(int id, int startYear, int endYear)
        {
            if(id!=0)
            {
                List<ShipmentReport_CM> shipmentdetails = new List<ShipmentReport_CM>();
                shipmentdetails = (from shpheader in dbcontext.Tbl_Shipment_Header
                                   join commission in dbcontext.Tbl_CommissionRecieve on shpheader.S_ID equals commission.CR_ShipmentID
                                   where shpheader.S_Type == 1 && shpheader.S_SalesPerson == id && (shpheader.S_INV_DATE.Value.Year == startYear || shpheader.S_INV_DATE.Value.Year == endYear )
                                   select new ShipmentReport_CM
                                   {
                                       salespersonID = shpheader.S_SalesPerson,
                                       salespersonName = (dbcontext.Tbl_Master_User.Where(x => x.USER_ID == shpheader.S_SalesPerson).Select(x => x.USER_NAME).FirstOrDefault()),
                                       CommissionAmt = commission.CR_CommissionRecieved,
                                       CREATED_DATE = shpheader.S_INV_DATE,
                                       monthname = commission.CREATED_DATE.Value.Month
                                   }).ToList();

                List<ShipmentReport_CM> salespersonWise = shipmentdetails.GroupBy(_ => new { _.CREATED_DATE.Value.Month, _.salespersonID })
        .Select(g => new ShipmentReport_CM
        {
            salespersonID = g.Key.salespersonID,
            CREATED_DATE = g.FirstOrDefault().CREATED_DATE,
            salespersonName = g.FirstOrDefault().salespersonName,
            CommissionAmt = g.Where(_ => _.CommissionAmt != 0).Sum(_ => _.CommissionAmt),
        }).ToList();
                var f = 0;
                string s = "";
                for (int i = 1; i <= 12; i++)
                {

                    var parentgroups = salespersonWise.Where(x => x.CREATED_DATE.Value.Month == (i)).ToList();
                    if (parentgroups.Count > 0)
                    {
                        if (f == 0)
                        {
                            s = s + "['" + parentgroups[0].CommissionAmt + "'";
                        }
                        else
                        {
                            s = s + ",'" + parentgroups[0].CommissionAmt + "'";
                        }
                        f++;
                    }
                    else
                    {
                        if (f == 0)
                        {
                            s = s + "['0'";
                        }
                        else
                        {
                            s = s + ",'0'";
                        }
                        f++;
                    }


                }

                s = s + "]";
                string s5 = s.Replace("\"", "~");
                s5 = s5.Replace("'", "\"");
                s5 = s5.Replace("~", "'");
                return Json(s5, JsonRequestBehavior.AllowGet);
            }
            else
            {
                List<ShipmentReport_CM> shipmentdetails = new List<ShipmentReport_CM>();
                shipmentdetails = (from shpheader in dbcontext.Tbl_Shipment_Header
                                   join commission in dbcontext.Tbl_CommissionRecieve on shpheader.S_ID equals commission.CR_ShipmentID
                                   where shpheader.S_Type == 1  && (shpheader.S_INV_DATE.Value.Year == startYear || shpheader.S_INV_DATE.Value.Year == endYear)
                                   select new ShipmentReport_CM
                                   {
                                       salespersonID = shpheader.S_SalesPerson,
                                       salespersonName = (dbcontext.Tbl_Master_User.Where(x => x.USER_ID == shpheader.S_SalesPerson).Select(x => x.USER_NAME).FirstOrDefault()),
                                       CommissionAmt = commission.CR_CommissionRecieved,
                                       CREATED_DATE = commission.CREATED_DATE,
                                       monthname = commission.CREATED_DATE.Value.Month
                                   }).ToList();

                List<ShipmentReport_CM> salespersonWise = shipmentdetails.GroupBy(_ => new { _.CREATED_DATE.Value.Month})
        .Select(g => new ShipmentReport_CM
        {
            CREATED_DATE = g.FirstOrDefault().CREATED_DATE,
            CommissionAmt = g.Where(_ => _.CommissionAmt != 0).Sum(_ => _.CommissionAmt),
        }).ToList();
                var f = 0;
                string s = "";
                for (int i = 1; i <= 12; i++)
                {

                    var parentgroups = salespersonWise.Where(x => x.CREATED_DATE.Value.Month == (i)).ToList();
                    if (parentgroups.Count > 0)
                    {
                        if (f == 0)
                        {
                            s = s + "['" + parentgroups[0].CommissionAmt + "'";
                        }
                        else
                        {
                            s = s + ",'" + parentgroups[0].CommissionAmt + "'";
                        }
                        f++;
                    }
                    else
                    {
                        if (f == 0)
                        {
                            s = s + "['0'";
                        }
                        else
                        {
                            s = s + ",'0'";
                        }
                        f++;
                    }


                }

                s = s + "]";
                string s5 = s.Replace("\"", "~");
                s5 = s5.Replace("'", "\"");
                s5 = s5.Replace("~", "'");
                return Json(s5, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetTradingSalesForBarchart(int id, int startYear, int endYear)
        {
            if(id!=0)
            {
                List<ShipmentReport_CM> shipmentdetails = new List<ShipmentReport_CM>();
                shipmentdetails = (from shpheader in dbcontext.Tbl_Shipment_Header
                                   join commission in dbcontext.Tbl_OneToOneInvoice on shpheader.S_ID equals commission.OTOI_ShipmentID
                                   where (commission.OTOI_InvoiceDate.Year == startYear || commission.OTOI_InvoiceDate.Year == endYear ) && shpheader.S_Type == 2 && shpheader.S_SalesPerson == id
                                   select new ShipmentReport_CM
                                   {
                                       salespersonID = shpheader.S_SalesPerson,
                                       salespersonName = (dbcontext.Tbl_Master_User.Where(x => x.USER_ID == shpheader.S_SalesPerson).Select(x => x.USER_NAME).FirstOrDefault()),
                                       CommissionAmt = commission.OTOI_InvoiceAmount,
                                       CREATED_DATE = commission.OTOI_InvoiceDate,
                                       //monthname = commission.OTOI_InvoiceDate.Value.Month
                                   }).ToList();

                List<ShipmentReport_CM> salespersonWise = shipmentdetails.GroupBy(_ => new { _.CREATED_DATE.Value.Month, _.salespersonID })
        .Select(g => new ShipmentReport_CM
        {
            salespersonID = g.Key.salespersonID,
            CREATED_DATE = g.FirstOrDefault().CREATED_DATE,
            salespersonName = g.FirstOrDefault().salespersonName,
            CommissionAmt = g.Where(_ => _.CommissionAmt != 0).Sum(_ => _.CommissionAmt),
        }).ToList();

                var f = 0;
                string s = "";
                for (int i = 1; i <= 12; i++)
                {

                    var parentgroups = salespersonWise.Where(x => x.CREATED_DATE.Value.Month == (i)).ToList();
                    if (parentgroups.Count > 0)
                    {
                        if (f == 0)
                        {
                            s = s + "['" + parentgroups[0].CommissionAmt + "'";
                        }
                        else
                        {
                            s = s + ",'" + parentgroups[0].CommissionAmt + "'";
                        }
                        f++;
                    }
                    else
                    {
                        if (f == 0)
                        {
                            s = s + "['0'";
                        }
                        else
                        {
                            s = s + ",'0'";
                        }
                        f++;
                    }


                }

                s = s + "]";
                string s5 = s.Replace("\"", "~");
                s5 = s5.Replace("'", "\"");
                s5 = s5.Replace("~", "'");
                return Json(s5, JsonRequestBehavior.AllowGet);
            }
            else
            {
                List<ShipmentReport_CM> shipmentdetails = new List<ShipmentReport_CM>();
                shipmentdetails = (from shpheader in dbcontext.Tbl_Shipment_Header
                                   join commission in dbcontext.Tbl_OneToOneInvoice on shpheader.S_ID equals commission.OTOI_ShipmentID
                                   where (commission.OTOI_InvoiceDate.Year == startYear || commission.OTOI_InvoiceDate.Year == endYear) && shpheader.S_Type == 2
                                   select new ShipmentReport_CM
                                   {
                                       salespersonID = shpheader.S_SalesPerson,
                                       salespersonName = (dbcontext.Tbl_Master_User.Where(x => x.USER_ID == shpheader.S_SalesPerson).Select(x => x.USER_NAME).FirstOrDefault()),
                                       CommissionAmt = commission.OTOI_InvoiceAmount,
                                       CREATED_DATE = commission.OTOI_InvoiceDate,
                                       //monthname = commission.OTOI_InvoiceDate.Value.Month
                                   }).ToList();

                List<ShipmentReport_CM> salespersonWise = shipmentdetails.GroupBy(_ => new { _.CREATED_DATE.Value.Month })
        .Select(g => new ShipmentReport_CM
        {
            CREATED_DATE = g.FirstOrDefault().CREATED_DATE,
            CommissionAmt = g.Where(_ => _.CommissionAmt != 0).Sum(_ => _.CommissionAmt),
        }).ToList();

                var f = 0;
                string s = "";
                for (int i = 1; i <= 12; i++)
                {

                    var parentgroups = salespersonWise.Where(x => x.CREATED_DATE.Value.Month == (i)).ToList();
                    if (parentgroups.Count > 0)
                    {
                        if (f == 0)
                        {
                            s = s + "['" + parentgroups[0].CommissionAmt + "'";
                        }
                        else
                        {
                            s = s + ",'" + parentgroups[0].CommissionAmt + "'";
                        }
                        f++;
                    }
                    else
                    {
                        if (f == 0)
                        {
                            s = s + "['0'";
                        }
                        else
                        {
                            s = s + ",'0'";
                        }
                        f++;
                    }


                }

                s = s + "]";
                string s5 = s.Replace("\"", "~");
                s5 = s5.Replace("'", "\"");
                s5 = s5.Replace("~", "'");
                return Json(s5, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetStoreSalesForBarchart(int id, int startYear, int endYear)
        {
            if(id!=0)
            {
                List<ShipmentReport_CM> shipmentdetails = new List<ShipmentReport_CM>();
                shipmentdetails = (from shpheader in dbcontext.Tbl_DespatchHeader
                                   join commission in dbcontext.Tbl_OneToManyInvoice on shpheader.D_ID equals commission.OTMI_DespatchIDs
                                   where (commission.OTMI_InvoiceDate.Year == startYear || commission.OTMI_InvoiceDate.Year == endYear) && shpheader.D_SalesPerson == id
                                   select new ShipmentReport_CM
                                   {
                                       salespersonID = shpheader.D_SalesPerson,
                                       salespersonName = (dbcontext.Tbl_Master_User.Where(x => x.USER_ID == shpheader.D_SalesPerson).Select(x => x.USER_NAME).FirstOrDefault()),
                                       CommissionAmt = commission.OTMI_InvoiceAmount,
                                       CREATED_DATE = commission.OTMI_InvoiceDate,
                                       //monthname = commission.OTOI_InvoiceDate.Value.Month
                                   }).ToList();

                List<ShipmentReport_CM> salespersonWise = shipmentdetails.GroupBy(_ => new { _.CREATED_DATE.Value.Month, _.salespersonID })
        .Select(g => new ShipmentReport_CM
        {
            salespersonID = g.Key.salespersonID,
            CREATED_DATE = g.FirstOrDefault().CREATED_DATE,
            salespersonName = g.FirstOrDefault().salespersonName,
            CommissionAmt = g.Where(_ => _.CommissionAmt != 0).Sum(_ => _.CommissionAmt),
        }).ToList();

                var f = 0;
                string s = "";
                for (int i = 1; i <= 12; i++)
                {

                    var parentgroups = salespersonWise.Where(x => x.CREATED_DATE.Value.Month == (i)).ToList();
                    if (parentgroups.Count > 0)
                    {
                        if (f == 0)
                        {
                            s = s + "['" + parentgroups[0].CommissionAmt + "'";
                        }
                        else
                        {
                            s = s + ",'" + parentgroups[0].CommissionAmt + "'";
                        }
                        f++;
                    }
                    else
                    {
                        if (f == 0)
                        {
                            s = s + "['0'";
                        }
                        else
                        {
                            s = s + ",'0'";
                        }
                        f++;
                    }


                }

                s = s + "]";
                string s5 = s.Replace("\"", "~");
                s5 = s5.Replace("'", "\"");
                s5 = s5.Replace("~", "'");
                return Json(s5, JsonRequestBehavior.AllowGet);
            }
            else
            {
                List<ShipmentReport_CM> shipmentdetails = new List<ShipmentReport_CM>();
                shipmentdetails = (from shpheader in dbcontext.Tbl_DespatchHeader
                                   join commission in dbcontext.Tbl_OneToManyInvoice on shpheader.D_ID equals commission.OTMI_DespatchIDs
                                   where (commission.OTMI_InvoiceDate.Year == startYear || commission.OTMI_InvoiceDate.Year == endYear)
                                   select new ShipmentReport_CM
                                   {
                                       salespersonID = shpheader.D_SalesPerson,
                                       salespersonName = (dbcontext.Tbl_Master_User.Where(x => x.USER_ID == shpheader.D_SalesPerson).Select(x => x.USER_NAME).FirstOrDefault()),
                                       CommissionAmt = commission.OTMI_InvoiceAmount,
                                       CREATED_DATE = commission.OTMI_InvoiceDate,
                                       monthname = commission.OTMI_InvoiceDate.Year,
                                   }).ToList();

                List<ShipmentReport_CM> salespersonWise = shipmentdetails.GroupBy(_ => new { _.CREATED_DATE.Value.Month })
        .Select(g => new ShipmentReport_CM
        {
            CREATED_DATE = g.FirstOrDefault().CREATED_DATE,
            CommissionAmt = g.Where(_ => _.CommissionAmt != 0).Sum(_ => _.CommissionAmt),
        }).ToList();

                var f = 0;
                string s = "";
                for (int i = 1; i <= 12; i++)
                {

                    var parentgroups = salespersonWise.Where(x => x.CREATED_DATE.Value.Month == (i)).ToList();
                    if (parentgroups.Count > 0)
                    {
                        if (f == 0)
                        {
                            s = s + "['" + parentgroups[0].CommissionAmt + "'";
                        }
                        else
                        {
                            s = s + ",'" + parentgroups[0].CommissionAmt + "'";
                        }
                        f++;
                    }
                    else
                    {
                        if (f == 0)
                        {
                            s = s + "['0'";
                        }
                        else
                        {
                            s = s + ",'0'";
                        }
                        f++;
                    }


                }

                s = s + "]";
                string s5 = s.Replace("\"", "~");
                s5 = s5.Replace("'", "\"");
                s5 = s5.Replace("~", "'");
                return Json(s5, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetSalespersonPieChart()
        {

           var shipmentdetails = (from shpheader in dbcontext.Tbl_Shipment_Header
                               join commission in dbcontext.Tbl_CommissionRecieve on shpheader.S_ID equals commission.CR_ShipmentID
                               where shpheader.S_Type == 1 && commission.CR_Date.Year == (DateTime.Now.Year)
                                  select new ShipmentReport_CM
                               {
                                   salespersonID = shpheader.S_SalesPerson,
                                   salespersonName = (dbcontext.Tbl_Master_User.Where(x => x.USER_ID == shpheader.S_SalesPerson).Select(x => x.USER_NAME).FirstOrDefault()),
                                   CommissionAmt = commission.CR_CommissionRecieved,
                               }).ToList();
            var salespersonWise = shipmentdetails.GroupBy(x => x.salespersonID).Select(a => new {
                a.FirstOrDefault().salespersonID,
                a.FirstOrDefault().salespersonName,
                CommissionAmt = a.Sum(b => b.CommissionAmt),
            }).ToList();
            string s1 = "";
            string s = "";
            var f1 = 0;

            foreach (var n in salespersonWise)
            {

                if (f1 == 0)
                {
                    s1 = s1 + "['" + n.salespersonName + "'";
                }
                else
                {
                    s1 = s1+ ",'" + n.salespersonName + "'";
                }
                f1++;


            }
            s1 = s1 + "]";
            var f = 0;
            foreach (var n in salespersonWise)
            {

                if (f == 0)
                {
                    s = s + "['" + n.CommissionAmt + "'";
                }
                else
                {
                    s = s + ",'" + n.CommissionAmt + "'";
                }
                f++;


            }
            s = s + "]";
            string s5 = s.Replace("\"", "~");
            s5 = s5.Replace("'", "\"");
            s5 = s5.Replace("~", "'");
            string s6 = s1.Replace("\"", "~");
            s6 = s6.Replace("'", "\"");
            s6 = s6.Replace("~", "'");
            var result = new { s5, s6 };
            var json = new JavaScriptSerializer().Serialize(result);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GettradingSalespersonPiechart()
        {
           var shipmentdetails = (from shpheader in dbcontext.Tbl_Shipment_Header
                               join Invoice in dbcontext.Tbl_OneToOneInvoice on shpheader.S_ID equals Invoice.OTOI_ShipmentID
                               where shpheader.S_Type == 2 && Invoice.OTOI_InvoiceDate.Year == (DateTime.Now.Year)
                                  select new ShipmentReport_CM
                               {
                                   salespersonID = shpheader.S_SalesPerson,
                                   salespersonName = (dbcontext.Tbl_Master_User.Where(x => x.USER_ID == shpheader.S_SalesPerson).Select(x => x.USER_NAME).FirstOrDefault()),                                   
                                   InvAmount = Invoice.OTOI_InvoiceAmount,
                               }).ToList();
            var salespersonWise = shipmentdetails.GroupBy(x => x.salespersonID).Select(a => new {
                a.FirstOrDefault().salespersonID,
                a.FirstOrDefault().salespersonName,
                CommissionAmt = a.Sum(b => b.InvAmount),
            }).ToList();
            string s1 = "";
            string s = "";
            var f1 = 0;

            foreach (var n in salespersonWise)
            {

                if (f1 == 0)
                {
                    s1 = s1 + "['" + n.salespersonName + "'";
                }
                else
                {
                    s1 = s1 + ",'" + n.salespersonName + "'";
                }
                f1++;


            }
            s1 = s1 + "]";
            var f = 0;
            foreach (var n in salespersonWise)
            {

                if (f == 0)
                {
                    s = s + "['" + n.CommissionAmt + "'";
                }
                else
                {
                    s = s + ",'" + n.CommissionAmt + "'";
                }
                f++;


            }
            s = s + "]";
            string s5 = s.Replace("\"", "~");
            s5 = s5.Replace("'", "\"");
            s5 = s5.Replace("~", "'");
            string s6 = s1.Replace("\"", "~");
            s6 = s6.Replace("'", "\"");
            s6 = s6.Replace("~", "'");
            var result = new { s5, s6 };
            var json = new JavaScriptSerializer().Serialize(result);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetStoreSalespersonPiechart()
        {
           var shipmentdetails = (from shpheader in dbcontext.Tbl_DespatchHeader
                               join Invoice in dbcontext.Tbl_OneToManyInvoice on shpheader.D_ID equals Invoice.OTMI_DespatchIDs
                               where Invoice.OTMI_InvoiceDate.Year == (DateTime.Now.Year)
                               select new ShipmentReport_CM
                               {
                                   salespersonID = shpheader.D_SalesPerson,
                                   salespersonName = (dbcontext.Tbl_Master_User.Where(x => x.USER_ID == shpheader.D_SalesPerson).Select(x => x.USER_NAME).FirstOrDefault()),
                                   InvAmount = Invoice.OTMI_InvoiceAmount,
                               }).ToList();
            var salespersonWise = shipmentdetails.GroupBy(x => x.salespersonID).Select(a => new {
                a.FirstOrDefault().salespersonID,
                a.FirstOrDefault().salespersonName,
                InvAmount = a.Sum(b => b.InvAmount),
            }).ToList();
            string s1 = "";
            string s = "";
            var f1 = 0;

            foreach (var n in salespersonWise)
            {

                if (f1 == 0)
                {
                    s1 = s1 + "['" + n.salespersonName + "'";
                }
                else
                {
                    s1 = s1 + ",'" + n.salespersonName + "'";
                }
                f1++;


            }
            s1 = s1 + "]";
            var f = 0;
            foreach (var n in salespersonWise)
            {

                if (f == 0)
                {
                    s = s + "['" + n.InvAmount + "'";
                }
                else
                {
                    s = s + ",'" + n.InvAmount + "'";
                }
                f++;


            }
            s = s + "]";
            string s5 = s.Replace("\"", "~");
            s5 = s5.Replace("'", "\"");
            s5 = s5.Replace("~", "'");
            string s6 = s1.Replace("\"", "~");
            s6 = s6.Replace("'", "\"");
            s6 = s6.Replace("~", "'");
            var result = new { s5, s6 };
            var json = new JavaScriptSerializer().Serialize(result);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}



//string s1 = "";
//string[] monthArray = new string[12] { "Janv", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
//var salespersonlist = dbcontext.Tbl_Sales_Organization.Where(m => m.DELETED == false && m.sales_Organization == 1).ToList();
//            for (int i = 0; i<salespersonlist.Count(); i++)
//            {
//                if (i == 0)
//                {
//                    s1 = salespersonlist[i].ORG_EMPLOYEE_IDS.ToString();
//                }
//                else
//                {
//                    s1 = s1 + "," + salespersonlist[i].ORG_EMPLOYEE_IDS.ToString();
//                }
//                // s = s + "," + ObjSales_Org[i].ORG_HEAD_ID.ToString();
//            }
//            var UID = new HashSet<decimal>(s1.Split(',').Select(m => Convert.ToDecimal(m)).ToArray());
            
//            
//                if (i == 1)
//                {
//                    s = s + " [{'month':'" + monthArray[i - 1] + "'";
//                }
//                else
//                {
//                    s = s + "{'month':'" + monthArray[i - 1] + "'";
//                }
//                if (salespersonWise.Count() > 0)
//                {
//                    s = s + ",";
//                }
//                
//string s3 = "";
//                for (int g = 0; g<parentgroups.Count(); g++)
//                {
//                    if (g == 0)
//                    {
//                        s3 = parentgroups[g].salespersonID.ToString();
//                    }
//                    else
//                    {
//                        s3 = s3 + "," + parentgroups[g].salespersonID.ToString();
//                    }
                  
//                }
                 
//                if (parentgroups.Count()==0)
//                {
//                    int f = 0;
//                    foreach (var n in UID)
//                    {
                        
//                        var personname = dbcontext.Tbl_Master_User.Where(m => m.USER_ID == n).Select(m => new { USER_NAME = m.USER_NAME }).FirstOrDefault();
//                        if (f == 0)
//                        {
//                            s = s + "'" + personname.USER_NAME + "':0";
//                        }
//                        else
//                        { 
//                            s = s + ",'" + personname.USER_NAME + "':0";
//                        }
//                        f++;
//                    }
//                }
                
//                for (int j = 0; j<parentgroups.Count(); j++)
//                {
                   
//                            if (j == 0)
//                            {
//                                s = s + "'" + salespersonWise[j].salespersonName + "':" + salespersonWise[j].CommissionAmt + "";
//                            }
//                            else
//                            {
//                                s = s + ",'" + salespersonWise[j].salespersonName + "':" + salespersonWise[j].CommissionAmt + "";
//                            }
                    

//                }
//                if(parentgroups.Count!=0)
//                {
//                    foreach (var n in UID)
//                    {
                        
//                        var UID1 = new HashSet<decimal>(s3.Split(',').Select(m => Convert.ToDecimal(m)).ToArray());
//                        if (!UID1.Contains(n))
//                        {
//                            var personname = dbcontext.Tbl_Master_User.Where(m => m.USER_ID == n).Select(m => new { USER_NAME = m.USER_NAME }).FirstOrDefault();

//s = s + ",'" + personname.USER_NAME + "':0";
                            
//                        }

//                    }
//                }
                

//                s = s + "},";
//            }
            
//                s = s + "]";