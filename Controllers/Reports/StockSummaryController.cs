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
    public class StockSummaryController : Controller
    {
        // GET: StockSummary
        BAL bal = new BAL();
        error_master objERR = new error_master();
        tbl_loginfo objLOG = new tbl_loginfo();
        EntityClasses dbcontext = new EntityClasses();
        public ActionResult StockSummary()
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

        //This Method is used to get Product data
        public JsonResult GetProductData()
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
                        var Products = dbcontext.Tbl_Product_Master.Where(m => m.COM_KEY == companykey && m.DELETED == false).OrderBy(m => m.P_ArticleNo).ToList();
                        var Stores = dbcontext.tbl_StoreMaster.Where(m =>  m.SM_DeleteStatus == false).OrderBy(m => m.SM_Name).ToList();
                        
                        var result = new { Products ,Stores};
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


        //Gets Stock Summary Data
        public JsonResult GetStockSummaryDetails(int product,int Store, int type)
        {
            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                {
                    string valid ="";
                    if (valid == "")
                    {
                        dbcontext.Configuration.ProxyCreationEnabled = false;
                       
                        List<StockSummary_CM> stocksummary = new List<StockSummary_CM>();

                        DataTable temp = new DataTable();
                        if (type == 3)
                        {

                            stocksummary = (from shpheader in dbcontext.tbl_StoreMaster
                                            join a in dbcontext.tbl_StoreDetails on shpheader.SM_Id equals a.SD_SM_ID
                                            join Invoice in dbcontext.Tbl_Product_Master on a.SD_Itemcode equals Invoice.P_ID
                                            where shpheader.SM_Id==Store && ((product != 0 && a.SD_Itemcode == product) || product == 0)
                                            select new StockSummary_CM
                                            {
                                                 Product_Code = a.SD_Itemcode,
                                                 productName = Invoice.P_Name,
                                                 Product_Description=Invoice.P_Description,
                                                 UOM=a.SD_UOM,
                                                 Article_NO=Invoice.P_ArticleNo,
                                                 OpnStockQty=a.SD_OpeningStock,
                                                 ClosingStockQty=0,
                                                 SoldQty=0,
                                                 ReceiptQty=0,TrnsfrQty=0,
                                            }).Distinct().ToList();
                            temp.Columns.Add("ID");
                            temp.Columns.Add("ProductCode");
                            temp.Columns.Add("ProductDescription");
                            temp.Columns.Add("UOM");
                            temp.Columns.Add("OpnStockQty");
                            temp.Columns.Add("ReceiptQty");
                            temp.Columns.Add("SoldQty");
                            temp.Columns.Add("TrnsfrQty");
                            temp.Columns.Add("ClosingStockQty");
                            
                            //General Data
                            var generaldata = stocksummary.Select(a => new
                            {
                                a.Product_Code,
                                a.Product_Description,
                                a.UOM,
                                a.OpnStockQty,
                                a.ReceiptQty,
                                a.SoldQty,
                                a.TrnsfrQty,
                                a.Article_NO,
                                a.ClosingStockQty,
                            }).ToList();

                            for (int l = 0; l < generaldata.Count(); l++)
                            {
                                DataRow dr4 = temp.NewRow();
                                dr4["ProductCode"] =  generaldata[l].Article_NO;
                                dr4["ProductDescription"] = generaldata[l].Product_Description;
                                dr4["UOM"] = generaldata[l].UOM ;
                                dr4["OpnStockQty"] = generaldata[l].OpnStockQty;
                                dr4["ReceiptQty"] = generaldata[l].ReceiptQty ;
                                dr4["SoldQty"] = generaldata[l].SoldQty;
                                dr4["TrnsfrQty"] = generaldata[l].TrnsfrQty;
                                dr4["ClosingStockQty"] = generaldata[l].ClosingStockQty;
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