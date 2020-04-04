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
using System;

namespace Euro.Controllers.Reports
{
    public class CheckStockQtyofanItemController : Controller
    {
        // GET: CheckStockQtyofanItem
        BAL bal = new BAL();
        error_master objERR = new error_master();
        tbl_loginfo objLOG = new tbl_loginfo();
        EntityClasses dbcontext = new EntityClasses();
        public ActionResult CheckStockQtyofanItem()
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
                        var Stores = dbcontext.tbl_StoreMaster.Where(m => m.SM_DeleteStatus == false).OrderBy(m => m.SM_Name).ToList();

                        var result = new { Products, Stores };
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
        public JsonResult GetStockQtyofanItem(int product)
        {
            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                {
                   
                        dbcontext.Configuration.ProxyCreationEnabled = false;
                        
                        List<CheckStockQtyofanItem_CM> chkstkqtyitm = new List<CheckStockQtyofanItem_CM>();
                        DataTable temp = new DataTable();
                    chkstkqtyitm = (from shpheader in dbcontext.tbl_StoreMaster
                                    join a in dbcontext.tbl_StoreDetails on shpheader.SM_Id equals a.SD_SM_ID
                                    join Invoice in dbcontext.Tbl_Product_Master on a.SD_Itemcode equals Invoice.P_ID
                                    where shpheader.SM_Id == 1 && ((product != 0 && a.SD_Itemcode == product) || product == 0)
                                    select new CheckStockQtyofanItem_CM
                                    {
                                        Product_Code = a.SD_Itemcode,
                                        productName = Invoice.P_Name,
                                        Product_Description = Invoice.P_Description,
                                        UOM = a.SD_UOM,
                                        Article_NO = Invoice.P_ArticleNo,
                                        OpnStockQty = a.SD_OpeningStock,
                                        ClosingStockQty = 0,
                                    }).Distinct().ToList();
                    temp.Columns.Add("ID");
                            temp.Columns.Add("Date");
                            temp.Columns.Add("TrnType");
                            temp.Columns.Add("ParyName");
                            temp.Columns.Add("Quantity");
                            temp.Columns.Add("UOM");
                            temp.Columns.Add("OpeningQty");
                            temp.Columns.Add("ClosingQty");
                    //General Data
                    var generaldata = chkstkqtyitm.Select(a => new
                            {
                                //a.Date,
                                //a.TrnType,
                                //a.PartyName,
                                //a.Quantity,
                                //a.UOM,
                                //a.OpeningQty,
                                //a.ClosingQty,
                              
                            }).ToList();

                            for (int l = 0; l < generaldata.Count(); l++)
                            {
                                DataRow dr4 = temp.NewRow();
                        //dr4["Date"] = "general" + generaldata[l].Date;
                        //dr4["TrnType"] = generaldata[l].TrnType;
                        //dr4["PartyName"] = (generaldata[l].PartyName ?? DateTime.Now).ToString("dd-MM-yyyy");
                        //dr4["Quantity"] = generaldata[l].Quantity;
                        //dr4["UOM"] = (generaldata[l].UOM ?? DateTime.Now).ToString("dd-MM-yyyy");
                        //dr4["OpeningQty"] = generaldata[l].OpeningQty;
                        //dr4["ClosingQty"]= generaldata[l].ClosingQty;
                        //temp.Rows.Add(dr4);
                    }
                    var json = Newtonsoft.Json.JsonConvert.SerializeObject(temp, Formatting.Indented);
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



    }
}