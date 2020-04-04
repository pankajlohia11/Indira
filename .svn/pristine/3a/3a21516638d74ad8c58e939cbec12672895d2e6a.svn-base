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
    public class PoStatusController : Controller
    {
        // GET: PoStatus
        BAL bal = new BAL();
        error_master objERR = new error_master();
        tbl_loginfo objLOG = new tbl_loginfo();
        EntityClasses dbcontext = new EntityClasses();
        public ActionResult PoStatus()
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

        //This Method is used to get Supplier data
        public JsonResult GetSupplierData()
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
                        var result = new { supplier };
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

        //Gets PO Status Data
        public JsonResult GetPoStatusDetails(int supplier, int duration, string fromdate, string todate, int type)
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
                        List<PoStatus_CM> postatus = new List<PoStatus_CM>();

                        DataTable temp = new DataTable();
                        if (type == 3)
                        {
                            string s1 = "";

                            var schduleOrder = dbcontext.tbl_GoodsInwardHeader.Where(m => m.DELETED == false).ToList();
                            for (int i = 0; i < schduleOrder.Count; i++)
                            {
                                if (i == 0)
                                {
                                    s1 = schduleOrder[i].GI_POCode.ToString();
                                }
                                else
                                {
                                    s1 = s1 + "," + schduleOrder[i].GI_POCode.ToString();
                                }

                            }
                            var UID2 = new HashSet<string>(s1.Split(',').ToList());

                            var schduleCode1 = dbcontext.tbl_PurchaseOrderHeader.Where(m => m.DELETED == false && !UID2.Contains(m.PP_ID.ToString())).Select(m => m.PP_ID).Distinct().ToList();
                            postatus = (from shpheader in dbcontext.tbl_PurchaseOrderHeader
                                        join order in dbcontext.tbl_PurchaseOrderDetails on shpheader.PP_ID equals order.PD_PID
                                        where shpheader.PO_Type==3 && !UID2.Contains(shpheader.PP_ID.ToString())&& ((duration != 0 && shpheader.PO_Date >= fromdt && shpheader.PO_Date <= todt) || duration == 0)
                                        select new PoStatus_CM
                                        {
                                            PO_ID = shpheader.PP_ID,
                                            PO_Code = shpheader.PO_Code,
                                            supplierID = shpheader.Po_Supplierkey,
                                            supplierName = (dbcontext.Tbl_Master_CompanyDetails.Where(x => x.COM_ID == shpheader.Po_Supplierkey).Select(x => x.COM_NAME).FirstOrDefault()),
                                            Po_Date = shpheader.PO_Date?? DateTime.Now,
                                            ProductId = order.PD_ProductID,
                                            UOM = order.PD_UOM,
                                            Product_Desc = (dbcontext.Tbl_Product_Master.Where(x => x.P_ID == order.PD_ProductID).Select(x => x.P_ShortName).FirstOrDefault()),
                                            Qty = order.PD_Quantity ,
                                            PD_UnitPrice = order.PD_UnitPrice ,
                                        }).ToList();
                          

                            temp.Columns.Add("ID");
                            temp.Columns.Add("PID");
                            temp.Columns.Add("SupplierId");
                            temp.Columns.Add("SupplierName");
                            temp.Columns.Add("PO_No");
                            temp.Columns.Add("PO_Date");
                            temp.Columns.Add("Product");
                            temp.Columns.Add("UOM");
                            temp.Columns.Add("Qty");
                            temp.Columns.Add("Unit_Price");
                            temp.Columns.Add("Amount");

                            //Customer Wise
                            var SupplierWiseDAra = postatus.GroupBy(x => x.supplierID).Select(a => new
                            {
                                a.FirstOrDefault().supplierID,
                                a.FirstOrDefault().supplierName,
                                quantity = a.Sum(b => b.Qty),
                            }).ToList();

                            for (int j = 0; j < SupplierWiseDAra.Count(); j++)
                            {
                                DataRow dr2 = temp.NewRow();
                                dr2["ID"] = "customer" + SupplierWiseDAra[j].supplierID;
                                dr2["PID"] = "0";
                                dr2["SupplierId"] = SupplierWiseDAra[j].supplierID;
                                dr2["SupplierName"] = SupplierWiseDAra[j].supplierName;
                                dr2["Qty"] = SupplierWiseDAra[j].quantity;
                                dr2["Product"] = "";
                                dr2["UOM"] = "";
                                dr2["PO_No"] = "";
                                dr2["PO_Date"] = "";
                                dr2["Unit_Price"] = "";
                                dr2["Amount"] = "";
                                temp.Rows.Add(dr2);

                                //General Data
                                var generaldata = postatus.Where(a => a.supplierID == SupplierWiseDAra[j].supplierID).Select(a => new
                                {
                                    a.supplierID,
                                    a.supplierName,
                                    a.ProductId,
                                    a.Product_Desc,
                                    a.Po_Date,
                                    a.UOM,
                                    a.PO_Code,
                                    a.PO_ID,
                                    a.PD_UnitPrice,
                                    a.Qty,
                                }).ToList();

                                for (int l = 0; l < generaldata.Count(); l++)
                                {
                                    DataRow dr4 = temp.NewRow();
                                    dr4["ID"] = "general" + generaldata[l].PO_ID;
                                    dr4["PID"] = "customer" + SupplierWiseDAra[j].supplierID;
                                    dr4["Product"] = generaldata[l].Product_Desc;
                                    dr4["UOM"] = generaldata[l].UOM;
                                    dr4["Qty"] = generaldata[l].Qty;
                                    dr4["PO_No"] = generaldata[l].PO_Code;
                                    dr4["PO_Date"] = (generaldata[l].Po_Date ?? DateTime.Now).ToString("dd-MM-yyyy");
                                    dr4["Unit_Price"] = generaldata[l].PD_UnitPrice;
                                    dr4["SupplierName"] = "";
                                    dr4["Amount"] = generaldata[l].Qty * generaldata[l].PD_UnitPrice;
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