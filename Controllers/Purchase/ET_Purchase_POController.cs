using BusinessEntity.EntityModels;
using BusinessLogic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using BusinessEntity.CustomModels;
using System.Data.Entity;
using BusinessEntity.EntityModels;
using BusinessLogic.Admin_BL;
using BusinessLogic;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Web.Script.Serialization;
using System.Data;
using System.IO;
using System.Globalization;
using System.Net.Mail;
using System.Net;
using Euro.Controllers.Sales;

namespace Euro.Controllers.Purchase
{
    public class ET_Purchase_POController : Controller
    {
        public static string prefix = "";
        public static bool automanual = false;
        BAL bal = new BAL();
        error_master objERR = new error_master();
        tbl_loginfo objLOG = new tbl_loginfo();
        EntityClasses dbcontext = new EntityClasses();
        // GET: ET_Purchase_PO
        public ActionResult ET_Purchase_PO(string type)
        {
            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                if(type!=null)
                {
                    if (type == "Trading" || type == "Store")
                    {
                        try
                        {
                            AutoManual();
                            ViewBag.details = null;
                            int com_key = Convert.ToInt32(Session["CompanyKey"]);
                            ViewBag.Login_Name = Session["DisplayName"].ToString();
                            EntityClasses dbcontext = new EntityClasses();
                            ViewBag.Users = dbcontext.Tbl_Product_Master.Where(m => m.COM_KEY == com_key).ToList();
                            return View();
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
                        return RedirectToAction("ET_InvalidRequest", "ET_Login");
                    }

                }
                else
                {
                    return RedirectToAction("ET_InvalidRequest", "ET_Login");
                }

            }
            else
            {
                return RedirectToAction("ET_SessionExpire", "ET_Login");
            }
        }
        //Getting whether this document automaic or manual
        private void AutoManual()
        {
            ViewBag.AutoManual = false;
            automanual = false;
            List<Tbl_Document_Master> ObjDoc = bal.AutoManual_BL(8017);
            if (ObjDoc.Count > 0)
            {
                foreach (var item in ObjDoc)
                {
                    if (item.autogen_type == "Automatic")
                    {
                        ViewBag.AutoManual = true;
                        prefix = item.autogen_prefix;
                        automanual = true;
                    }
                    else
                    {
                        ViewBag.Required = "required";
                    }
                }
            }
        }
        //Get the Privillage access for this document
        public JsonResult GetPrivilages()
        {

            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                {
                    var privilagelist = bal.GetPrivilages_BL(Convert.ToInt32(Session["UserID"].ToString()), 8017);
                    var json = new JavaScriptSerializer().Serialize(privilagelist);
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
        //This is for one-one PO. Get The orderReference
        public JsonResult GetOrderReference(decimal PP_ID)
        {
            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                {

                    int comp_key = Convert.ToInt32(Session["CompanyKey"].ToString());
                    //Submit Time all order will get
                    if(PP_ID==0)
                    {
                        using (EntityClasses dbcontext = new EntityClasses())
                        {
                            dbcontext.Configuration.ProxyCreationEnabled = false;
                            var purchaseHeader = dbcontext.tbl_PurchaseOrderHeader.Select(m => m.PO_OrderReference).ToList();
                            var OrderReference = dbcontext.Tbl_Master_Order.Where(m => !purchaseHeader.Contains(m.SO_ID) && m.DELETED == false && m.SO_OrderType == 2).ToList();
                            var json = new JavaScriptSerializer().Serialize(OrderReference);
                            return Json(json, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        //Edit Time all which order we choosen that order only get
                        var purchaseHeader = dbcontext.tbl_PurchaseOrderHeader.Where(m => m.PP_ID == PP_ID).Select(m => m.PO_OrderReference).ToList();
                        var OrderReference = dbcontext.Tbl_Master_Order.Where(m => purchaseHeader.Contains(m.SO_ID)).ToList();
                        var json = new JavaScriptSerializer().Serialize(OrderReference);
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
        //Get the Products For the supplier which are ordered at that Edit time
        public JsonResult GetProductBySupplierAndOrder(decimal PP_ID,decimal SupplierKey)
        {
            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                {

                    int comp_key = Convert.ToInt32(Session["CompanyKey"].ToString());
                    using (EntityClasses dbcontext = new EntityClasses())
                    {
                        dbcontext.Configuration.ProxyCreationEnabled = false;
                       
                       var OrderProduct = dbcontext.tbl_PurchaseOrderDetails.Where(m=>m.PD_PID == PP_ID).Select(m => m.PD_ProductID).ToList();
                        var SupplierProduct = (from a in dbcontext.Tbl_SupplierPriceList
                                               join b in dbcontext.Tbl_SupplierPriceListDetails on a.SPL_ID equals b.SLD_ID
                                               where a.SPL_SupplierKey == SupplierKey && b.SLD_Status == true
                                               select b.SLD_ProductID).ToList();
                        var data = dbcontext.Tbl_Product_Master.Where(z => OrderProduct.Contains(z.P_ID) || SupplierProduct.Contains(z.P_ID)).ToList();
                        var json = new JavaScriptSerializer().Serialize(data);
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
        // This is for one-one PO. Price is exist in Supplier Price List for the ordered Product
        public ActionResult GetSupplierPriceList(int id,string catalogSelected)
        {
            bool val = Session["UserID"] == null ? false : true;
            if(val)
            {
                try
                {

                     decimal SupplierId = dbcontext.Tbl_Master_Order.Single(m => m.SO_ID == id).SO_SupplierID??0;

                    var ProductDetails = (from a in dbcontext.Tbl_Order_Details
                                          where a.AGEN_TRAD_PO_ID == id
                                          select new SUppProduct
                                          {
                                              PD_ProductID = a.PRODUCT_ID
                                          }).Distinct().ToList();
                    var data2 = (from z in ProductDetails select z.PD_ProductID).ToList();
                    //var PriceDetails = (from header in dbcontext.Tbl_SupplierPriceList join
                    //                    SPL in dbcontext.Tbl_SupplierPriceListDetails on header.SPL_ID equals SPL.SLD_ID
                    //                    where  header.SPL_SupplierKey==SupplierId  && SPL.SLD_Status==true && data2.Contains(SPL.SLD_ProductID??0)
                    //                    select new SUppProduct{
                    //                        PD_ProductID = SPL.SLD_ProductID??0
                    //                    }).ToList();

                    var PriceDetails = (from PCL in dbcontext.Tbl_ProductCatalog
                                        join PCM in dbcontext.Tbl_ProductCatalog_Master on PCL.PC_CODE equals PCM.PC_CatalogueCode
                                        //join product in DbContext.Tbl_Product_Master on MUOM.LU_Code equals product.P_UOM
                                        where PCM.PC_CatalogType == 2 && PCM.PC_CatalogueCode == catalogSelected
                                        select PCL.PRODUCT_ID).ToList();


                    if (ProductDetails.Count() == PriceDetails.Count())
                    {
                        //get Price exist Product
                        var data1 = (from OD in dbcontext.Tbl_Master_Order
                                     join PM in dbcontext.Tbl_Order_Details on OD.SO_ID equals PM.AGEN_TRAD_PO_ID into comp
                                     from PM in comp
                                     join OM in dbcontext.Tbl_Product_Master on PM.PRODUCT_ID equals OM.P_ID into ore
                                     from OM in ore
                                     join SPL in dbcontext.tbl_LookUp  on OM.P_UOM equals SPL.LU_Code
                                     join PCL in dbcontext.Tbl_ProductCatalog on PM.PRODUCT_ID equals PCL.PRODUCT_ID
                                     join PCM in dbcontext.Tbl_ProductCatalog_Master on PCL.PC_CODE equals PCM.PC_CatalogueCode
                                     //join MUOM in dbcontext.Tbl_SupplierPriceListDetails on PM.PRODUCT_ID equals MUOM.SLD_ProductID into uom
                                     //from MUOM in uom
                                     //join SPLH in dbcontext.Tbl_SupplierPriceList on MUOM.SLD_ID equals SPLH.SPL_ID
                                     where PM.AGEN_TRAD_PO_ID == id && SPL.LU_Type == 2 && PCM.PC_CatalogType == 2 && PCM.PC_CatalogueCode == catalogSelected
                                     select new PO_CM
                                     {
                                         PO_OrderDetailID = PM.ORDER_ID,
                                         PD_ProductID = PM.PRODUCT_ID,
                                         PD_ShortName = OM.P_ShortName,
                                         PD_UOM = SPL.LU_Description,
                                         PD_Quantity = PM.QUANTITY,
                                         PD_UnitPrice = PCL.Summer_Price,
                                         Description = OM.P_Description,
                                         DesignDetail = PM.DesignDetail,
                                         //SO_SupplierID = OD.SO_SupplierID,
                                         OrderPrice = PM.PRICE??0,
                                         PD_SuppDef=OM.P_Remark2,
                                         //DesignDetail=PM.DesignDetail,
                                         //Description=OM.P_Description

                                     }).Distinct().ToList();
                        List<PO_CM> modelItems = (List<PO_CM>)data1.Select((purchaseOrdItem, index) => new PO_CM() { SO_Serial = index + 1,PO_OrderDetailID = purchaseOrdItem.PO_OrderDetailID, PD_ProductID = purchaseOrdItem.PD_ProductID, PD_ShortName = purchaseOrdItem.PD_ShortName, PD_UOM = purchaseOrdItem.PD_UOM, PD_Quantity = purchaseOrdItem.PD_Quantity, PD_UnitPrice = purchaseOrdItem.PD_UnitPrice, Description = purchaseOrdItem.Description, DesignDetail = purchaseOrdItem.DesignDetail, OrderPrice = purchaseOrdItem.OrderPrice,PD_SuppDef = purchaseOrdItem.PD_SuppDef }).ToList();

                        return PartialView("/Views/Purchase/ET_Purchase_PO/ET_Purchase_PO_Details.cshtml", modelItems);

                    }
                    else
                    {
                        var data1 = (from OD in dbcontext.Tbl_Master_Order
                                     join PM in dbcontext.Tbl_Order_Details on OD.SO_ID equals PM.AGEN_TRAD_PO_ID into comp
                                     from PM in comp
                                     join OM in dbcontext.Tbl_Product_Master on PM.PRODUCT_ID equals OM.P_ID into ore
                                     from OM in ore
                                     join SPL in dbcontext.tbl_LookUp on OM.P_UOM equals SPL.LU_Code
                                     //join MUOM in dbcontext.Tbl_SupplierPriceListDetails on PM.PRODUCT_ID equals MUOM.SLD_ProductID into uom
                                     //from MUOM in uom
                                     //join SPLH in dbcontext.Tbl_SupplierPriceList on MUOM.SLD_ID equals SPLH.SPL_ID
                                     where PM.AGEN_TRAD_PO_ID == id && SPL.LU_Type == 2
                                     select new PO_CM
                                     {
                                         PO_OrderDetailID = PM.ORDER_ID,
                                         PD_ProductID = PM.PRODUCT_ID,
                                         PD_ShortName = OM.P_ShortName,
                                         PD_UOM = SPL.LU_Description,
                                         PD_Quantity = PM.QUANTITY,
                                         PD_UnitPrice = (from PCL in dbcontext.Tbl_ProductCatalog
                                                        join PCM in dbcontext.Tbl_ProductCatalog_Master on PCL.PC_CODE equals PCM.PC_CatalogueCode
                                                        where PCM.PC_CatalogType == 2 && PCM.PC_CatalogueCode == catalogSelected && PCL.PRODUCT_ID == PM.PRODUCT_ID
                                                        select PCL.Summer_Price
                                                        ).FirstOrDefault(),
                                         Description = OM.P_Description,
                                         DesignDetail = PM.DesignDetail,
                                         //SO_SupplierID = OD.SO_SupplierID,
                                         OrderPrice = PM.PRICE ?? 0,
                                         PD_SuppDef = OM.P_Remark2,
                                         //DesignDetail=PM.DesignDetail,
                                         //Description=OM.P_Description

                                     }).Distinct().ToList();
                        List<PO_CM> modelItems = (List<PO_CM>)data1.Select((purchaseOrdItem, index) => new PO_CM() { SO_Serial = index + 1, PO_OrderDetailID = purchaseOrdItem.PO_OrderDetailID, PD_ProductID = purchaseOrdItem.PD_ProductID, PD_ShortName = purchaseOrdItem.PD_ShortName, PD_UOM = purchaseOrdItem.PD_UOM, PD_Quantity = purchaseOrdItem.PD_Quantity, PD_UnitPrice = purchaseOrdItem.PD_UnitPrice, Description = purchaseOrdItem.Description, DesignDetail = purchaseOrdItem.DesignDetail, OrderPrice = purchaseOrdItem.OrderPrice, PD_SuppDef = purchaseOrdItem.PD_SuppDef }).ToList();

                        return PartialView("/Views/Purchase/ET_Purchase_PO/ET_Purchase_PO_Details.cshtml", modelItems);
                        //get Price is not exist Product
                        //var data3 = (from z in PriceDetails select z.PD_ProductID).ToList();
                        //var data = string.Join(",",(from a in ProductDetails join 
                        //            b in dbcontext.Tbl_Product_Master on a.PD_ProductID equals b.P_ID 
                        //            where !data3.Contains(a.PD_ProductID)
                        //            select b.P_ArticleNo).ToArray());
                        //return Json("Validations : Supplier Price does'nt Exist for the catalog : " + catalogSelected, JsonRequestBehavior.AllowGet);
                    }

                   
                }
                catch(Exception exe)
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
        // This is For One-many PO Get the store list
        public JsonResult GetStorelist(decimal PO_Id)
        {
            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                {
                    int comp_key = Convert.ToInt32(Session["CompanyKey"].ToString());
                    if (PO_Id==0)
                    {
                        using (EntityClasses dbcontext = new EntityClasses())
                        {
                            dbcontext.Configuration.ProxyCreationEnabled = false;
                            var Supplier = dbcontext.tbl_StoreMaster.Where(m => m.SM_CompanyKey == comp_key && m.SM_DeleteStatus == false).ToList();
                            var json = new JavaScriptSerializer().Serialize(Supplier);
                            return Json(json, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        using (EntityClasses dbcontext = new EntityClasses())
                        {
                            dbcontext.Configuration.ProxyCreationEnabled = false;
                            var PO_SMId = dbcontext.tbl_PurchaseOrderHeader.Single(m => m.PP_ID == PO_Id).PO_SMId;
                            var Supplier = dbcontext.tbl_StoreMaster.Where(m => m.SM_CompanyKey == comp_key && (m.SM_DeleteStatus == false || m.SM_Id==PO_SMId)).ToList();
                            var json = new JavaScriptSerializer().Serialize(Supplier);
                            return Json(json, JsonRequestBehavior.AllowGet);
                        }
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
        //this is for one - one PO .Get the Supplier name from order
        public JsonResult GetSuppliersByOrder(int id)
        {
            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                {
                    dbcontext.Configuration.ProxyCreationEnabled = false;
                    int com_key = Convert.ToInt32(Session["CompanyKey"]);
                    var SupplierID = (from a in dbcontext.Tbl_Master_Order
                                    join b in dbcontext.Tbl_Order_Details on a.SO_ID equals b.AGEN_TRAD_PO_ID
                                    where a.SO_ID == id
                                    select  a.SO_SupplierID).Distinct();
                   // var data = dbcontext.Tbl_Product_Master.Where(m => m.COM_KEY == com_key && products.Contains(m.P_ID)).ToList();
                    var json = new JavaScriptSerializer().Serialize(SupplierID);
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
        //Checking the Bank details for Supplier
        public JsonResult GetBankDetailsForSupplier(int id)
        {
            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                {
                    dbcontext.Configuration.ProxyCreationEnabled = false;
                    int com_key = Convert.ToInt32(Session["CompanyKey"]);
                    var BankCOunt = dbcontext.Tbl__Master_CompanyBank.Where(m => m.B_PID == id).Count();
                      
                    // var data = dbcontext.Tbl_Product_Master.Where(m => m.COM_KEY == com_key && products.Contains(m.P_ID)).ToList();
                    var json = new JavaScriptSerializer().Serialize(BankCOunt);
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
        //this is for one - Many PO .Get the Suppliers List
        public JsonResult GetSupplier(decimal ID)
        {
            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                {

                    int comp_key = Convert.ToInt32(Session["CompanyKey"].ToString());
                    if(ID==0)
                    {
                        using (EntityClasses dbcontext = new EntityClasses())
                        {
                            dbcontext.Configuration.ProxyCreationEnabled = false;
                            var Supplier = dbcontext.Tbl_Master_CompanyDetails.Where(m => m.COM_KEY == comp_key && m.Cust_Supp != 0).OrderBy(m => m.COM_NAME).ToList();
                            var json = new JavaScriptSerializer().Serialize(Supplier);
                            return Json(json, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        dbcontext.Configuration.ProxyCreationEnabled = false;
                        var Po_Supplierkey = dbcontext.tbl_PurchaseOrderHeader.Single(m => m.PP_ID == ID).Po_Supplierkey;
                        var Supplier = dbcontext.Tbl_Master_CompanyDetails.Where(m => m.COM_KEY == comp_key && m.Cust_Supp != 0 &&(m.DELETED==false || m.COM_ID==Po_Supplierkey)).OrderBy(m => m.COM_NAME).ToList();
                        var json = new JavaScriptSerializer().Serialize(Supplier);
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
        //Get All PO list Which are not deleted
        public JsonResult GetPOList(bool delete,int type)
        { 
            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                {
                    int comp_key = Convert.ToInt32(Session["CompanyKey"].ToString());
                    dbcontext.Configuration.ProxyCreationEnabled = false;
                    var data = (from a in dbcontext.tbl_PurchaseOrderHeader
                                join b in dbcontext.Tbl_Master_CompanyDetails on a.Po_Supplierkey equals b.COM_ID into comp
                                from x in comp
                                where a.DELETED == delete && a.COM_KEY == comp_key && a.PO_Type==type
                                select new
                                {
                                    ID = a.PP_ID, 
                                    Code = a.PO_Code,
                                    Date = a.PO_Date,
                                    Supplier = x.COM_DISPLAYNAME,
                                    PaymentTerms = a.Po_PaymentTerms,
                                    DeliveryDate = a.Po_DeliveryDate,
                                    TotalAmount = a.Po_TotalAmount, 
                                    ShippingAddress = a.Po_ShippingAddress,
                                    SupplierAddress = a.Po_SupplierAddress,
                                    SpecialInstruction = a.Po_SpecialInstruction,
                                    TermsConditions = a.Po_TermsandConditions,
                                    PO_SCNo = a.PO_SCNo,
                                    PO_SCDate = a.PO_SCDate,
                                    Status = a.Po_ApprovalStatus,
                                    Po_OrderReference=a.PO_OrderReference,
                                    POType = a.PO_Type,
                                    PO_status=a.PO_status,
                                    CusPONo = (from masord in dbcontext.Tbl_Master_Order
                                                     join c in dbcontext.tbl_PurchaseOrderHeader on masord.SO_ID equals c.PO_OrderReference
                                                     where c.PP_ID == a.PP_ID && c.DELETED == delete && c.PO_Type == type
                                                     select
                                                     masord.SO_CusPONO).FirstOrDefault(),
                                }
                                ).ToList();
                    var json = new JavaScriptSerializer().Serialize(data);
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


        /// <summary>
        /// Get Purchase Order Reference.
        /// </summary>
        /// <param name="orderReference">Order Reference</param>
        /// <param name="type">Type of PO</param>
        /// <returns>Json Result.</returns>
        public JsonResult GetPurchaseOrder(string orderReference, int type)
        {
            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                {
                    var orderRef = dbcontext.Tbl_Master_Order.Single(m => m.SO_Code == orderReference).SO_ID;
                    int comp_key = Convert.ToInt32(Session["CompanyKey"].ToString());
                    dbcontext.Configuration.ProxyCreationEnabled = false;
                    var data = (from a in dbcontext.tbl_PurchaseOrderHeader
                                join b in dbcontext.Tbl_Master_CompanyDetails on a.Po_Supplierkey equals b.COM_ID into comp
                                from x in comp
                                where a.DELETED == false && a.COM_KEY == comp_key && a.PO_Type == type && a.PO_OrderReference == orderRef
                                select new
                                {
                                    ID = a.PP_ID,
                                    Code = a.PO_Code
                                    /*Date = a.PO_Date,
                                    Supplier = x.COM_DISPLAYNAME,
                                    PaymentTerms = a.Po_PaymentTerms,
                                    DeliveryDate = a.Po_DeliveryDate,
                                    TotalAmount = a.Po_TotalAmount,
                                    ShippingAddress = a.Po_ShippingAddress,
                                    SupplierAddress = a.Po_SupplierAddress,
                                    SpecialInstruction = a.Po_SpecialInstruction,
                                    TermsConditions = a.Po_TermsandConditions,
                                    PO_SCNo = a.PO_SCNo,
                                    PO_SCDate = a.PO_SCDate,
                                    Status = a.Po_ApprovalStatus,
                                    Po_OrderReference = a.PO_OrderReference,
                                    POType = a.PO_Type,
                                    PO_status = a.PO_status,
                                    CusPONo = (from masord in dbcontext.Tbl_Master_Order
                                               join c in dbcontext.tbl_PurchaseOrderHeader on masord.SO_ID equals c.PO_OrderReference
                                               select
                                               masord.SO_CusPONO).FirstOrDefault(),
                                               */
                                }
                                ).ToList();
                    var json = new JavaScriptSerializer().Serialize(data);
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

        public JsonResult GetPO()
        {
            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                {

                    int comp_key = Convert.ToInt32(Session["CompanyKey"].ToString());
                    using (EntityClasses dbcontext = new EntityClasses())
                    {
                        dbcontext.Configuration.ProxyCreationEnabled = false;
                        var Supplier = dbcontext.tbl_PurchaseOrderHeader.Where(m => m.COM_KEY == comp_key).ToList();
                        var json = new JavaScriptSerializer().Serialize(Supplier);
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
        //Get Products For Particular Supplier
        public JsonResult GetProducts(int id)
        {

            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                {
                    dbcontext.Configuration.ProxyCreationEnabled = false;
                    int com_key = Convert.ToInt32(Session["CompanyKey"]);
                    var products = (from a in dbcontext.Tbl_SupplierPriceList
                                    join b in dbcontext.Tbl_SupplierPriceListDetails on a.SPL_ID equals b.SLD_ID
                                    where a.SPL_SupplierKey == id && b.SLD_Status == true && a.COM_KEY == com_key
                                    select b.SLD_ProductID);
                    //var data = dbcontext.Tbl_Product_Master.Where(m => m.COM_KEY == com_key && products.Contains(m.P_ID)).ToList();
                    //var data = dbcontext.Tbl_Product_Master.Where(m => m.COM_KEY == com_key && (m.DELETED == false && products.Contains(m.P_ID))).OrderBy(m => m.P_ArticleNo).ToList();
                    var data = dbcontext.Tbl_Product_Master.Where(m => m.COM_KEY == com_key && (m.DELETED == false)).OrderBy(m => m.P_ArticleNo).ToList();
                    var json = new JavaScriptSerializer().Serialize(data);
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
        //Get Payment Terms
        [HttpPost]
        public JsonResult Payment_terms_dropdown(int id)
        {
            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                {
                    dbcontext.Configuration.ProxyCreationEnabled = false;
                    int companykey = Convert.ToInt32(Session["CompanyKey"]);
                    if (id == 0)
                    {
                        var Customer = dbcontext.Tbl_Payment_Terms.Where(m => m.COM_KEY == companykey && m.DELETED == false).OrderBy(m => m.PT_Name).ToList();
                        var json = new JavaScriptSerializer().Serialize(Customer);
                        return Json(json, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        var payment = dbcontext.tbl_PurchaseOrderHeader.Single(m => m.PP_ID == id).Po_PaymentTerms;
                        var Customer = dbcontext.Tbl_Payment_Terms.Where(m => m.COM_KEY == companykey && (m.DELETED == false || m.PT_ID == payment)).OrderBy(m => m.PT_Name).ToList();
                        var json = new JavaScriptSerializer().Serialize(Customer);
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
        //Get order Product For One-One PO
        public JsonResult GetOrderedProduct(int id)
        {
            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                {
                    dbcontext.Configuration.ProxyCreationEnabled = false;
                    int com_key = Convert.ToInt32(Session["CompanyKey"]);
                    var products = (from a in dbcontext.Tbl_Master_Order
                                    join b in dbcontext.Tbl_Order_Details on a.SO_ID equals b.AGEN_TRAD_PO_ID
                                    where a.SO_ID == id 
                                    select b.PRODUCT_ID);
                    var data = dbcontext.Tbl_Product_Master.Where(m => m.COM_KEY == com_key && products.Contains(m.P_ID)).ToList();
                    var json = new JavaScriptSerializer().Serialize(data);
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
        //Get Supplier address 
        public JsonResult BindSupplierAddress(int id)
        {
            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                {
                    using (EntityClasses dbcontext = new EntityClasses())
                    {
                        dbcontext.Configuration.ProxyCreationEnabled = false;
                        var street = dbcontext.Tbl_Master_CompanyDetails.Where(m=>m.COM_ID == id).ToList()[0].COM_STREET;
                        var loccity =dbcontext.Tbl_Master_CompanyDetails.Where(m => m.COM_ID == id).ToList()[0].COM_CITY;
                        var locstate = dbcontext.Tbl_Master_CompanyDetails.Where(m => m.COM_ID == id).ToList()[0].COM_STATE;
                        int loccountry = Convert.ToInt32(dbcontext.Tbl_Master_CompanyDetails.Where(m => m.COM_ID == id).ToList()[0].COM_COUNTRY);
                        var data1 = street + ", " + loccity + ", "
                                    + locstate + ", "
                                    + (dbcontext.locations.Where(m => m.location_id == loccountry).Select(m => m.name).ToList()[0]);
                        return Json(data1, JsonRequestBehavior.AllowGet);
                    }
                    // s = a.COM_STREET + ", " + a.COM_COUNTRY + ", " + a.COM_CITY 
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
        //Get Particular product Details for one-many PO
        public JsonResult GetProductDetailsByID(int id, string catalogSelected,int supplierId)
        {

            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                {
                    dbcontext.Configuration.ProxyCreationEnabled = false;
                    if (catalogSelected == "" || catalogSelected == string.Empty)
                    {
                        var priceval = (from a in dbcontext.Tbl_SupplierPriceList
                                        join b in dbcontext.Tbl_SupplierPriceListDetails on a.SPL_ID equals b.SLD_ID
                                        where a.SPL_SupplierKey == supplierId && b.SLD_ProductID == id && b.SLD_Status == true
                                        select b.SLD_UnitPrice);

                        var data1 = (from a in dbcontext.Tbl_Product_Master
                                     join b in dbcontext.tbl_LookUp on a.P_UOM equals b.LU_Code
                                     into m
                                     from n in m.DefaultIfEmpty()
                                     where a.P_ID == id && n.LU_Type == 2
                                     select new
                                     {
                                         name = a.P_ShortName,
                                         uom = n.LU_Description,
                                         price = (from a in dbcontext.Tbl_SupplierPriceList
                                                  join b in dbcontext.Tbl_SupplierPriceListDetails on a.SPL_ID equals b.SLD_ID
                                                  where a.SPL_SupplierKey == supplierId && b.SLD_ProductID == id && b.SLD_Status == true
                                                  select b.SLD_UnitPrice),
                                         Remarks = a.P_Remark1,
                                         Description = a.P_Description
                                     });
                        var json = new JavaScriptSerializer().Serialize(data1);
                        return Json(json, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        var data1 = (from a in dbcontext.Tbl_Product_Master
                                     join b in dbcontext.tbl_LookUp on a.P_UOM equals b.LU_Code into m
                                     from n in m.DefaultIfEmpty()
                                     where a.P_ID == id && n.LU_Type == 2
                                     select new
                                     {
                                         name = a.P_ShortName,
                                         ShortName = a.P_ShortName,
                                         uom = n.LU_Description,
                                         price = (
                                                   from PCL in dbcontext.Tbl_ProductCatalog
                                                   join PCM in dbcontext.Tbl_ProductCatalog_Master on PCL.PC_CODE equals PCM.PC_CatalogueCode
                                               //join product in DbContext.Tbl_Product_Master on MUOM.LU_Code equals product.P_UOM
                                               where PCM.PC_CatalogueCode == catalogSelected && PCL.PRODUCT_ID == id
                                                   select PCL.Summer_Price
                                         ).FirstOrDefault(),
                                         //price = (priceType == 1 ? (dbcontext.Tbl_ProductCatalog.FirstOrDefault(m => m.ACTIVE_STATUS == true && m.PRODUCT_ID == a.P_ID).UNIT_PRICE1) : priceType == 2 ? (dbcontext.Tbl_ProductCatalog.FirstOrDefault(m => m.ACTIVE_STATUS == true && m.PRODUCT_ID == a.P_ID).UNIT_PRICE2) : priceType == 3 ? (dbcontext.Tbl_ProductCatalog.FirstOrDefault(m => m.ACTIVE_STATUS == true && m.PRODUCT_ID == a.P_ID).UNIT_PRICE3) : (dbcontext.Tbl_ProductCatalog.FirstOrDefault(m => m.ACTIVE_STATUS == true && m.PRODUCT_ID == a.P_ID).UNIT_PRICE4)) != null? (priceType == 1 ? (dbcontext.Tbl_ProductCatalog.FirstOrDefault(m => m.ACTIVE_STATUS == true && m.PRODUCT_ID == a.P_ID).UNIT_PRICE1) : priceType == 2 ? (dbcontext.Tbl_ProductCatalog.FirstOrDefault(m => m.ACTIVE_STATUS == true && m.PRODUCT_ID == a.P_ID).UNIT_PRICE2) : priceType == 3 ? (dbcontext.Tbl_ProductCatalog.FirstOrDefault(m => m.ACTIVE_STATUS == true && m.PRODUCT_ID == a.P_ID).UNIT_PRICE3) : (dbcontext.Tbl_ProductCatalog.FirstOrDefault(m => m.ACTIVE_STATUS == true && m.PRODUCT_ID == a.P_ID).UNIT_PRICE4)):0,
                                         quantity = a.P_PackingQuantity,
                                         CustomerDef = a.P_Remark1,
                                         P_Description = a.P_Description,
                                         Description = (dbcontext.tbl_LookUp.FirstOrDefault(m => m.LU_Type == 2 && m.LU_Code == a.P_UOM).LU_Description),
                                         P_PackingQuantityUOM = a.P_PackingQuantityUOM,
                                     });
                        var json = new JavaScriptSerializer().Serialize(data1);
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
        //Get Currency List
        public JsonResult GetCurrency()
        {

            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                {
                    dbcontext.Configuration.ProxyCreationEnabled = false;
                    var data = dbcontext.tbl_LookUp.Where(m => m.LU_Type == 1002).ToList();
                    var json = new JavaScriptSerializer().Serialize(data);
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
        // Validations For Saving The data
        private string Validations(int PP_ID, string PO_Code, string PO_Date, decimal PO_Type,decimal PO_OrderReference, decimal Po_Supplierkey, decimal Po_PaymentTerms, decimal Po_TotalAmount, string PODetails,string Q_CatalogId,out decimal user)
        {
            user = 0;
            //try
            //{
            //    var approver = dbcontext.Tbl_Document_Master.Single(m => m.auto_key == 8017).workflow_status;
            //    if (approver == 1)
            //        user = dbcontext.Tbl_Document_Master.Single(m => m.auto_key == 8017).workflowapprover ?? 0;
            //    else
            //        user = 1000000;
            //}
            //catch { }
            //if (user == 0)
            //{
            //    return "Please contact Admin to set Approver.";
            //}
            if (!automanual && PO_Code == "")
            {
                return "Enter PO Code";
            }
            if (PO_Date == "")
            {
                return "Select PO Date";
            }
            if (Po_Supplierkey == 0)
            {
                return "Select Supplier";
            }
            if (Po_PaymentTerms == 0)
            {
                return "Enter Payment Terms";
            }
            if(PO_OrderReference!=0)
            {
                var count1 = dbcontext.tbl_PurchaseOrderHeader.Where(m => m.PO_OrderReference == PO_OrderReference).ToList().Count();
                if (PP_ID == 0)
                {
                    if (count1 > 0)
                    {
                        return "PO for this order is already Exist";
                    }
                }
            }
           
            
            if (!automanual)
            {
                if (PP_ID == 0)
                {
                    var count = dbcontext.tbl_PurchaseOrderHeader.Where(m => m.PO_Code == PO_Code).ToList().Count();
                    if (count > 0)
                    {
                        return "PO Code Already Exist";
                    }
                
                }
                else
                {
                    var count = dbcontext.tbl_PurchaseOrderHeader.Where(m => m.PP_ID != PP_ID && m.PO_Code== PO_Code).ToList().Count();
                    if (count > 0)
                    {
                        return "PO Code Already Exist";
                    }
                }
            }
            try
            {
               
                string[] ChildRow = PODetails.Split('|');
                string[] tableColumns = new string[ChildRow.Length];
                for (int i = 0; i < ChildRow.Length - 1; i++)
                {
                    string[] ChildRecord = ChildRow[i].Split('}');
                    if(PO_Type==1)
                    {
                        if (tableColumns.Contains(Convert.ToDecimal(ChildRecord[1]).ToString()))
                        {
                            return "Product is repeated at row :" + (i + 1);
                        }
                    }
                    //Checking Product Deleted at the Edit time
                    if (PP_ID != 0)
                    {
                        if (Q_CatalogId == "" || Q_CatalogId == string.Empty)
                        {
                            var SupplierProduct = (from header in dbcontext.Tbl_SupplierPriceList
                                                   join SPL in dbcontext.Tbl_SupplierPriceListDetails on header.SPL_ID equals SPL.SLD_ID
                                                   where header.SPL_SupplierKey == Po_Supplierkey && SPL.SLD_Status == true
                                                   select new SUppProduct
                                                   {
                                                       PD_ProductID = SPL.SLD_ProductID ?? 0
                                                   }).ToList().Distinct();

                            var data3 = (from z in SupplierProduct select z.PD_ProductID).ToList();
                            decimal ProductID = Convert.ToDecimal(ChildRecord[1].ToString());
                            if (!data3.Contains(ProductID))
                            {
                                var data = string.Join(",", (from a in dbcontext.Tbl_Product_Master
                                                             where a.P_ID == ProductID
                                                             select a.P_ArticleNo).ToArray());
                                return "Price is not available for the Products:" + data;
                            }
                        }
                    }
                    tableColumns[i] = Convert.ToDecimal(ChildRecord[1]).ToString();
                }
            }
            catch(Exception ex)
            {
                return "Unable to process your request. Please verify product data.";
            }
            return "";
        }
        //Add the PO

        [HttpPost]
        public JsonResult ET_Master_PO_Add(int PP_ID, string PO_Code, string PO_Date,decimal PO_Type,decimal PO_OrderReference, string Po_CurrencyKey, decimal Po_Supplierkey, bool Po_DeliveryShedule, decimal Po_PaymentTerms, string Po_DeliveryDate, decimal Po_TotalAmount, string Po_ShippingAddress,decimal Po_StoreId,string Po_SupplierAddress,string PO_ScNo,string PO_ScDate,string Po_SpecialInstruction,string Po_TermsandConditions, string Q_CatalogId,string PODetails)
        {
            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                // decimal k = 0;
                int k = 0;
                var context = new EntityClasses();
                var transaction = context.Database.BeginTransaction();
                bool success = true;

                try
                {
                    decimal user;
                    string valid = Validations(PP_ID, PO_Code, PO_Date,PO_Type, PO_OrderReference, Po_Supplierkey, Po_PaymentTerms, Po_TotalAmount, PODetails, Q_CatalogId,out user);
                    if (valid == "")
                    {
                        var Username = Session["UserID"].ToString();
                        DateTime PODate ;
                        PODate = DateTime.ParseExact(PO_Date, "dd-MM-yyyy", null); 
                        DateTime? DeliveryDate=null;
                        if (Po_DeliveryDate != "") { DeliveryDate = DateTime.ParseExact(Po_DeliveryDate, "dd-MM-yyyy", null); }
                        DateTime? SCDate=null;
                        if (PO_ScDate != "") { SCDate = DateTime.ParseExact(PO_ScDate, "dd-MM-yyyy", null); }
                        tbl_PurchaseOrderHeader Objmc = new tbl_PurchaseOrderHeader()
                        {
                            PP_ID = PP_ID,
                            PO_Code = PO_Code,
                            PO_Date = PODate,
                            PO_Type = PO_Type,
                            PO_OrderReference = PO_OrderReference,
                            Po_CurrencyKey = Po_CurrencyKey,
                            Po_Supplierkey = Po_Supplierkey,
                            Po_DeliveryShedule = Po_DeliveryShedule,
                            Po_PaymentTerms = Po_PaymentTerms,
                            Po_DeliveryDate = DeliveryDate,
                            Po_TotalAmount = Po_TotalAmount,
                            Po_ShippingAddress = Po_ShippingAddress,
                            PO_SCNo = PO_ScNo,
                            PO_SCDate = SCDate,
                            PO_SMId = Po_StoreId,
                            Po_ApprovalStatus = 0,
                            PO_Approver = user,
                            Po_SupplierAddress = Po_SupplierAddress,
                            Po_SpecialInstruction = Po_SpecialInstruction,
                            Po_TermsandConditions = Po_TermsandConditions,
                            CREATED_BY = Convert.ToInt32(Session["UserID"].ToString()),
                            LAST_UPDATED_BY = Convert.ToInt32(Session["UserID"].ToString()),
                            CREATED_DATE = DateTime.Now,
                            DELETED = false,
                            COM_KEY = Convert.ToInt32(Session["CompanyKey"]),
                            PO_status = false,
                            Q_CatalogId = Q_CatalogId
                            
                        };

                        
                        if (PP_ID == 0)
                        {
                            context.tbl_PurchaseOrderHeader.Add(Objmc);
                            if (context.SaveChanges() == 0)
                            {
                                success = false;
                            }


                            if (automanual == true)
                            {
                                int len = 10 - (prefix + Objmc.PP_ID).Length;
                                string code = prefix + new String('0', len) + Objmc.PP_ID;
                                tbl_PurchaseOrderHeader Obj_tbl_PurchaseOrderHeader = context.tbl_PurchaseOrderHeader.Single(m => m.PP_ID == Objmc.PP_ID);
                                {
                                    Obj_tbl_PurchaseOrderHeader.PO_Code = code;
                                };
                                if (context.SaveChanges() == 0)
                                {
                                    success = false;
                                }
                            }
                            //Update the Sales Contract No to the Order Reference
                            if (Objmc.PO_OrderReference != 0)
                            {
                                Tbl_Master_Order Obj_tbl_Master_Order = context.Tbl_Master_Order.Single(m => m.SO_ID == Objmc.PO_OrderReference);
                                {
                                    Obj_tbl_Master_Order.SO_SupSCNO = Objmc.PO_SCNo;
                                    Obj_tbl_Master_Order.SO_SupSCDate = Objmc.PO_SCDate;
                                };
                            }
                            context.SaveChanges();
                        }
                        else
                        {

                           
                            tbl_PurchaseOrderHeader Obj_tbl_PurchaseOrderHeader = context.tbl_PurchaseOrderHeader.Single(m => m.PP_ID == Objmc.PP_ID);
                            {
                                Obj_tbl_PurchaseOrderHeader.PO_Code = Objmc.PO_Code;
                                Obj_tbl_PurchaseOrderHeader.PO_Date = Objmc.PO_Date;
                                Obj_tbl_PurchaseOrderHeader.PO_Type = PO_Type;
                                Obj_tbl_PurchaseOrderHeader.PO_OrderReference = PO_OrderReference;
                                Obj_tbl_PurchaseOrderHeader.Po_CurrencyKey = Objmc.Po_CurrencyKey;
                                Obj_tbl_PurchaseOrderHeader.Po_Supplierkey = Objmc.Po_Supplierkey;
                                Obj_tbl_PurchaseOrderHeader.Po_DeliveryShedule = Objmc.Po_DeliveryShedule;
                                Obj_tbl_PurchaseOrderHeader.Po_PaymentTerms = Objmc.Po_PaymentTerms;
                                Obj_tbl_PurchaseOrderHeader.Po_DeliveryDate = Objmc.Po_DeliveryDate;
                                Obj_tbl_PurchaseOrderHeader.Po_TotalAmount = Objmc.Po_TotalAmount;
                                Obj_tbl_PurchaseOrderHeader.Po_ShippingAddress = Objmc.Po_ShippingAddress;
                                Obj_tbl_PurchaseOrderHeader.PO_SMId = Objmc.PO_SMId;
                                Obj_tbl_PurchaseOrderHeader.Po_ApprovalStatus = 0;
                                Obj_tbl_PurchaseOrderHeader.PO_Approver = user;
                                Obj_tbl_PurchaseOrderHeader.Po_SupplierAddress = Objmc.Po_SupplierAddress;
                                Obj_tbl_PurchaseOrderHeader.PO_SCNo = Objmc.PO_SCNo;
                                Obj_tbl_PurchaseOrderHeader.PO_SCDate = Objmc.PO_SCDate;
                                Obj_tbl_PurchaseOrderHeader.Po_SpecialInstruction = Objmc.Po_SpecialInstruction;
                                Obj_tbl_PurchaseOrderHeader.Po_TermsandConditions = Objmc.Po_TermsandConditions; 
                                Obj_tbl_PurchaseOrderHeader.LAST_UPDATED_DATE = DateTime.Now;
                                Obj_tbl_PurchaseOrderHeader.LAST_UPDATED_BY = Objmc.LAST_UPDATED_BY;
                                Obj_tbl_PurchaseOrderHeader.PO_status = Objmc.PO_status;
                                Obj_tbl_PurchaseOrderHeader.Q_CatalogId = Objmc.Q_CatalogId;

                            };
                            k = context.SaveChanges();
                            if (k == 0)
                            {
                                success = false;
                            }

                            //Update the Sales Contract No to the Order Reference
                            if (Objmc.PO_OrderReference != 0)
                            {
                                Tbl_Master_Order Obj_tbl_Master_Order = context.Tbl_Master_Order.Single(m => m.SO_ID == Objmc.PO_OrderReference);
                                {
                                    Obj_tbl_Master_Order.SO_SupSCNO = Objmc.PO_SCNo;
                                    Obj_tbl_Master_Order.SO_SupSCDate = Objmc.PO_SCDate;
                                };
                            }
                            context.SaveChanges();
                        }
                        // Delete previous contact data
                        tbl_PurchaseOrderHeader objdeletedetails = new tbl_PurchaseOrderHeader();
                        context.tbl_PurchaseOrderDetails.RemoveRange(context.tbl_PurchaseOrderDetails.Where(m => m.PD_PID == Objmc.PP_ID));
                        context.SaveChanges();

                        // delete schedules
                        tbl_PurchaseOrderDeliverySchedule obj_PurchaseOrderDeliverySchedule = new tbl_PurchaseOrderDeliverySchedule();
                        context.tbl_PurchaseOrderDeliverySchedule.RemoveRange(context.tbl_PurchaseOrderDeliverySchedule.Where(m => m.PODS_PID == Objmc.PP_ID));
                        context.SaveChanges();

                        // Insert new contacts data
                        string[] ChildRow = PODetails.Split('|');
                        for (int i = 0; i < ChildRow.Length-1 ; i++)
                        {
                            //Splitting the Article Number to support Maximum of 50 characters
                            //for Article Number.
                            string[] ChildRecord = ChildRow[i].Split('}');
                            string[] articleData = ChildRecord[2].Split('-');
                            decimal orderDetailId = 0;
                            if (ChildRecord[0] == string.Empty || ChildRecord[0] == "")
                            {
                                orderDetailId = i;
                            }
                            else
                                orderDetailId = Convert.ToDecimal(ChildRecord[0]);
                            tbl_PurchaseOrderDetails objPOdetails = new tbl_PurchaseOrderDetails()
                            {
                                PD_PID = Convert.ToInt32(Objmc.PP_ID),
                                PO_OrderDetailID = orderDetailId,
                                PD_ProductID = Convert.ToInt32(ChildRecord[1]),
                                PD_ArticleNo = articleData[0],
                                PD_UOM = ChildRecord[4],
                                PD_Quantity = Convert.ToDecimal(ChildRecord[6]),
                                PD_UnitPrice = Convert.ToDecimal(ChildRecord[7]),
                                //PD_Tax = Convert.ToDecimal(ChildRecord[6]),
                                PD_TotalAmount = Convert.ToDecimal(ChildRecord[8]),
                                PD_DeliveryDate = Objmc.Po_DeliveryDate,
                                DesignDetail = ChildRecord[9],
                                SupplierDes = ChildRecord[10]
                            };
                            context.tbl_PurchaseOrderDetails.Add(objPOdetails);
                            if (context.SaveChanges() == 0)
                            {
                                success = false;
                            }
                            k = (int)objPOdetails.PD_PID;
                            if (Po_DeliveryShedule)
                            {
                                string[] schrecord = ChildRecord[10].Split('$');
                                for (int j = 0; j < schrecord.Length - 1; j++)
                                {
                                    string[] schrow = schrecord[j].Split('@');
                                    tbl_PurchaseOrderDeliverySchedule obj_PurchaseOrderDeliverySchedule1 = new tbl_PurchaseOrderDeliverySchedule()
                                    {
                                        PODS_PID = Convert.ToInt32(Objmc.PP_ID),
                                        PODS_PDID = Convert.ToInt32(objPOdetails.PD_ID),
                                        PODS_DeliveryDate = Convert.ToDateTime(schrow[0]),
                                        PODS_Quantity = Convert.ToDecimal(schrow[1]),
                                    };
                                    context.tbl_PurchaseOrderDeliverySchedule.Add(obj_PurchaseOrderDeliverySchedule1);
                                    if (context.SaveChanges() == 0)
                                    {
                                        success = false;
                                    }

                                }

                            }
                        }
                        if (success)
                        {
                            transaction.Commit();
                        }
                        else
                        {
                            k = 0;
                            transaction.Rollback();
                        }

                        var json = "Success:" + Objmc.PO_Code;
                        if (k == 0)
                        {
                            json = "Failed";
                        }
                        else
                        {
                            objLOG.log_dockey = "8017";
                            objLOG.log_userid = Session["UserID"].ToString();
                            objLOG.log_recordkey = k.ToString();
                            if (PP_ID == 0)
                            {
                                objLOG.log_operation = "Insert";
                                objLOG.log_Remarks = "Record Inserted Successfully";
                            }
                            else
                            {
                                objLOG.log_operation = "Update";
                                objLOG.log_Remarks = "Record Updated Successfully";
                            }
                            bal.OperationInsertLogs_BL(objLOG);
                        }
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
                    k = 0;
                    transaction.Rollback();
                    string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                    string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                    objERR.err_title = controllerName + "-" + controllerName;
                    objERR.err_message = "Sorry for the inconvience. Some error has been occured.";
                    objERR.err_details = exe.Message.Replace("'", "");
                    int errid = bal.ExceptionInsertLogs_BL(objERR);
                    return Json("ERR" + errid.ToString(), JsonRequestBehavior.AllowGet);
                }
                finally
                {
                    transaction.Dispose();
                    context.Dispose();
                }
            }
            else
            {
                return Json("Expired", JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Clones the Existing Record
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ET_Purchase_ClonePO(int PP_ID)
        {
            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                {
                    string OrdCode = string.Empty;
                    decimal k = 0;
                    var context = new EntityClasses();
                    var transaction = context.Database.BeginTransaction();
                    bool success = true;
                    tbl_PurchaseOrderHeader originalOrder = dbcontext.tbl_PurchaseOrderHeader.Single(m => m.PP_ID == PP_ID);
                    tbl_PurchaseOrderHeader Obj_tbl_PurchaseOrderHeader = new tbl_PurchaseOrderHeader()
                    {
                        PO_Code = string.Empty,
                        PO_Date = DateTime.Now,
                        PO_Type = originalOrder.PO_Type,
                        PO_OrderReference = 0,
                        Po_CurrencyKey = originalOrder.Po_CurrencyKey,
                        Po_Supplierkey = originalOrder.Po_Supplierkey,
                        Po_DeliveryShedule = originalOrder.Po_DeliveryShedule,
                        Po_PaymentTerms = originalOrder.Po_PaymentTerms,
                        Po_DeliveryDate = DateTime.Now,
                        Po_TotalAmount = originalOrder.Po_TotalAmount,
                        Po_ShippingAddress = originalOrder.Po_ShippingAddress,
                        PO_SMId = originalOrder.PO_SMId,
                        Po_ApprovalStatus = 0,
                        PO_Approver = originalOrder.PO_Approver,
                        Po_SupplierAddress = originalOrder.Po_SupplierAddress,
                        PO_SCNo = originalOrder.PO_SCNo,
                        PO_SCDate = DateTime.Now,
                        Po_SpecialInstruction = originalOrder.Po_SpecialInstruction,
                        Po_TermsandConditions = originalOrder.Po_TermsandConditions,
                        CREATED_BY = Convert.ToInt32(Session["UserID"].ToString()),
                        CREATED_DATE = DateTime.Now,
                        LAST_UPDATED_DATE = DateTime.Now,
                        LAST_UPDATED_BY = Convert.ToInt32(Session["UserID"].ToString()),
                        PO_status = false,
                        Q_CatalogId = originalOrder.Q_CatalogId,
                        COM_KEY = Convert.ToInt32(Session["CompanyKey"]),
                        DELETED = false
                    };
                    context.tbl_PurchaseOrderHeader.Add(Obj_tbl_PurchaseOrderHeader);
                    //context.SaveChanges();
                    if (context.SaveChanges() == 0)
                    {
                        success = false;
                    }

                    int len = 10 - (prefix + Obj_tbl_PurchaseOrderHeader.PP_ID).Length;
                    string code = prefix + new String('0', len) + Obj_tbl_PurchaseOrderHeader.PP_ID;
                    Obj_tbl_PurchaseOrderHeader.PO_Code = code;

                    if (context.SaveChanges() == 0)
                    {
                        success = false;
                    }

                    k = Obj_tbl_PurchaseOrderHeader.PP_ID;
                    OrdCode = Obj_tbl_PurchaseOrderHeader.PO_Code;
                    Random rnd = new Random();
                    var poDetails = (from Ol in dbcontext.tbl_PurchaseOrderHeader
                                        join Od in dbcontext.tbl_PurchaseOrderDetails on Ol.PP_ID equals Od.PD_PID
                                        join Pct in dbcontext.Tbl_Product_Master on Od.PD_ProductID equals Pct.P_ID
                                        where Ol.PP_ID == PP_ID
                                        select new 
                                        {
                                            PD_PID = Ol.PP_ID,
                                            PO_OrderDetailID = Od.PO_OrderDetailID ?? 0,
                                            PD_ProductID = Od.PD_ProductID,
                                            PD_ArticleNo = Od.PD_ArticleNo,
                                            PD_UOM = Od.PD_UOM,
                                            PD_Quantity = Od.PD_Quantity ?? 0,
                                            PD_UnitPrice = Od.PD_UnitPrice ?? 0,
                                            PD_TotalAmount = Od.PD_TotalAmount ?? 0,
                                            PD_DeliveryDate = Od.PD_DeliveryDate,
                                            DesignDetail = Od.DesignDetail,
                                            SupplierDes = Od.SupplierDes

                                        }).ToList();

                    for (int i = 0; i < poDetails.Count; i++)
                    {
                        tbl_PurchaseOrderDetails newOrderDetaills = new tbl_PurchaseOrderDetails()
                        {
                            PD_PID = Obj_tbl_PurchaseOrderHeader.PP_ID,
                            PO_OrderDetailID = (i+1),
                            PD_ProductID = poDetails[i].PD_ProductID,
                            PD_ArticleNo = poDetails[i].PD_ArticleNo,
                            PD_UOM = poDetails[i].PD_UOM,
                            PD_Quantity = poDetails[i].PD_Quantity,
                            PD_UnitPrice = poDetails[i].PD_UnitPrice,
                            //PD_Tax = Convert.ToDecimal(ChildRecord[6]),
                            PD_TotalAmount = poDetails[i].PD_TotalAmount,
                            PD_DeliveryDate = poDetails[i].PD_DeliveryDate,
                            DesignDetail = poDetails[i].DesignDetail,
                            SupplierDes = poDetails[i].SupplierDes
                        };
                        context.tbl_PurchaseOrderDetails.Add(newOrderDetaills);
                    }

                    if (context.SaveChanges() != poDetails.Count)
                    {
                        success = false;
                    }
                    if (success)
                    {
                        transaction.Commit();
                    }
                    else
                    {
                        k = 0;
                        transaction.Rollback();
                    }
                    var json = "Success:" + OrdCode;
                    if (k == 0)
                    {
                        json = "Failed";
                    }
                    else
                    {
                        objLOG.log_dockey = "6014";
                        objLOG.log_userid = Session["UserID"].ToString();
                        objLOG.log_recordkey = k.ToString();
                        if (k == 0)
                        {
                            objLOG.log_operation = "Insert";
                            objLOG.log_Remarks = "Record Inserted Successfully";
                        }
                        else
                        {
                            objLOG.log_operation = "Update";
                            objLOG.log_Remarks = "Record Updated Successfully";
                        }
                        bal.OperationInsertLogs_BL(objLOG);
                    }
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
                return RedirectToAction("ET_SessionExpire", "ET_Login");
            }
        }
        //Get the one Po Details For View
        public ActionResult ET_Purchase_PO_View(int id)
        {
            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                List<PO_CM> data1 = new List<PO_CM> ();
                try
                {
                    
                    dbcontext.Configuration.ProxyCreationEnabled = false;
                         data1 = (from a in dbcontext.tbl_PurchaseOrderHeader
                                     join b in dbcontext.Tbl_Master_CompanyDetails on a.Po_Supplierkey equals b.COM_ID into comp
                                     from x in comp
                                     where a.PP_ID == id
                                     select new PO_CM
                                     {
                                         PP_ID = a.PP_ID,
                                         PO_Code = a.PO_Code,
                                         PO_Type = a.PO_Type ?? 0,
                                         PO_Date = a.PO_Date ?? DateTime.Now,
                                         Po_Supplierkey = a.Po_Supplierkey ?? 0,
                                         Po_CurrencyKey = a.Po_CurrencyKey,
                                         Po_PaymentTerms = a.Po_PaymentTerms ?? 0,
                                         Po_SupplierAddress = a.Po_SupplierAddress,
                                         Po_TotalAmount = a.Po_TotalAmount,
                                         Po_ShippingAddress = a.Po_ShippingAddress,
                                         Po_SpecialInstruction = a.Po_SpecialInstruction,
                                         Po_TermsandConditions = a.Po_TermsandConditions,
                                         Po_DeliveryDate = a.Po_DeliveryDate??DateTime.Now,
                                         PO_OrderReference=a.PO_OrderReference??0,
                                         Po_DeliveryShedule=a.Po_DeliveryShedule

                                     }).ToList();
                    
                    var data2 = (from c in dbcontext.tbl_PurchaseOrderDetails
                                 join a in dbcontext.Tbl_Product_Master on c.PD_ProductID equals a.P_ID
                                 join b in dbcontext.tbl_LookUp on a.P_UOM equals b.LU_Code
                                 into m
                                 from n in m.DefaultIfEmpty()
                                 where c.PD_PID == id && n.LU_Type == 2
                                
                                 select new PO_CM
                                 {
                                     PD_ArticleNo = a.P_ArticleNo,
                                     PD_ShortName = a.P_ShortName,
                                     PD_UOM = n.LU_Description,
                                      PD_UnitPrice = c.PD_UnitPrice,
                                      PD_Quantity = c.PD_Quantity,
                                     PD_TotalAmount = c.PD_TotalAmount,
                                     PD_ID=c.PD_ID,
                                     PD_PID=c.PD_PID
                                     
                                 }).ToList();
                    POCombo_CM obj = new POCombo_CM();
                    obj.POHeader = data1;
                    obj.PODetails = data2;
                    return PartialView("/Views/Purchase/ET_Purchase_PO/ET_Purchase_PO_View.cshtml", obj);
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
                return RedirectToAction("ET_SessionExpire", "ET_Login");
            }
        }
        // code for the print option
        public ActionResult ET_Purchase_PO_Print(int id, string lang,int orderType)
        {
            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                {
                    dbcontext.Configuration.ProxyCreationEnabled = false;
                    int com_key = Convert.ToInt32(Session["Companykey"]);
                    var data1 = new List<PO_CM>();
                    if (orderType == 3)
                    {
                        data1 = (
                            from k in dbcontext.tbl_PurchaseOrderHeader
                            //join MO in dbcontext.Tbl_Master_Order on k.PO_OrderReference equals MO.SO_ID
                            //join MU in dbcontext.Tbl_Master_User on k.Po_Supplierkey equals MU.USER_ID
                            join CD in dbcontext.Tbl_Master_CompanyDetails on k.Po_Supplierkey equals CD.COM_ID
                            join SCD in dbcontext.Tbl_Master_CompanyDetails on k.Po_Supplierkey equals SCD.COM_ID
                            where k.PP_ID == id
                            select new PO_CM
                            {
                                Street = CD.COM_STREET,
                                CityState = (CD.COM_CITY),
                                Zipcode = CD.COM_COUNTRY,
                                CountryZip = ((dbcontext.locations.Where(a => a.location_id == CD.COM_COUNTRY).Select(a => a.name).FirstOrDefault()) + ", " + (CD.COM_ZIP)),
                                imgurl = (dbcontext.Tbl_SystemSetUp.Where(a => a.COMPANY_ID == com_key).Select(a => a.COMPANY_LOGO).FirstOrDefault()),
                                SysCompany = (dbcontext.Tbl_SystemSetUp.Where(a => a.COMPANY_ID == com_key).Select(a => a.COMPANY_NAME).FirstOrDefault()),
                                VatPer = (dbcontext.Tbl_SystemSetUp.Select(a => a.TAX).FirstOrDefault()),
                                USER_NAME = string.Empty,
                                CustomerName = CD.COM_NAME,
                                SUPPLIER_NAME = SCD.COM_NAME,
                                salesPersonName = string.Empty,
                                //PO_CutomerID = MO.SO_CutomerID,
                                PO_Date = (k.PO_Date.HasValue ? k.PO_Date.Value : DateTime.Now),
                                PO_Code = k.PO_Code,
                                PO_CusDeliveryTerms = k.Po_ShippingAddress,
                                Po_ShippingAddress = k.Po_ShippingAddress,
                                Po_TermsandConditions = k.Po_TermsandConditions,
                                Po_SpecialInstruction = k.Po_SpecialInstruction,
                                PO_Remarks = string.Empty,
                                PaymentTermsDescription = (dbcontext.Tbl_Payment_Terms.Where(m => m.PT_ID == k.Po_PaymentTerms).Select(m => m.PT_Name).FirstOrDefault()),
                                PO_OrderCode = string.Empty,
                                PO_CusPONO =string.Empty,
                                PO_SCDate = (k.PO_SCDate.HasValue ? k.PO_SCDate.Value : DateTime.Now),
                                PO_SCNo = k.PO_SCNo
                            }).ToList();
                    }
                    else
                    {
                        data1 = (
                                from k in dbcontext.tbl_PurchaseOrderHeader
                                join MO in dbcontext.Tbl_Master_Order on k.PO_OrderReference equals MO.SO_ID
                                join MU in dbcontext.Tbl_Master_User on MO.SO_SalesPersonID equals MU.USER_ID
                                join CD in dbcontext.Tbl_Master_CompanyDetails on MO.SO_SupplierID equals CD.COM_ID
                                join SCD in dbcontext.Tbl_Master_CompanyDetails on MO.SO_SupplierID equals SCD.COM_ID
                                where k.PP_ID == id
                                select new PO_CM
                                {
                                    Street = CD.COM_STREET,
                                    CityState = (CD.COM_CITY),
                                    Zipcode = CD.COM_COUNTRY,
                                    CountryZip = ((dbcontext.locations.Where(a => a.location_id == CD.COM_COUNTRY).Select(a => a.name).FirstOrDefault()) + ", " + (CD.COM_ZIP)),
                                    imgurl = (dbcontext.Tbl_SystemSetUp.Where(a => a.COMPANY_ID == com_key).Select(a => a.COMPANY_LOGO).FirstOrDefault()),
                                    SysCompany = (dbcontext.Tbl_SystemSetUp.Where(a => a.COMPANY_ID == com_key).Select(a => a.COMPANY_NAME).FirstOrDefault()),
                                    VatPer = (dbcontext.Tbl_SystemSetUp.Select(a => a.TAX).FirstOrDefault()),
                                    USER_NAME = MU.USER_NAME,
                                    CustomerName = CD.COM_NAME,
                                    SUPPLIER_NAME = SCD.COM_NAME,
                                    salesPersonName = MU.DISPLAY_NAME,
                                    PO_CutomerID = MO.SO_CutomerID,
                                    PO_Date = (k.PO_Date.HasValue ? k.PO_Date.Value : DateTime.Now),
                                    PO_Code = k.PO_Code,
                                    PO_CusDeliveryTerms = MO.SO_CusDeliveryTerms,
                                    Po_ShippingAddress = k.Po_ShippingAddress,
                                    Po_TermsandConditions = k.Po_TermsandConditions,
                                    Po_SpecialInstruction = k.Po_SpecialInstruction,
                                    PO_Remarks = MO.SO_Remarks,
                                    PaymentTermsDescription = (dbcontext.Tbl_Payment_Terms.Where(m => m.PT_ID == k.Po_PaymentTerms).Select(m => m.PT_Name).FirstOrDefault()),
                                    PO_OrderCode = MO.SO_Code,
                                    PO_CusPONO = MO.SO_CusPONO,
                                    PO_SCDate = (k.PO_SCDate.HasValue ? k.PO_SCDate.Value : DateTime.Now),
                                    PO_SCNo = k.PO_SCNo
                                }).ToList();
                    }
                    var data2 = (from c in dbcontext.tbl_PurchaseOrderDetails
                                 join k in dbcontext.tbl_PurchaseOrderHeader on c.PD_PID equals k.PP_ID
                                 join a in dbcontext.Tbl_Product_Master on c.PD_ProductID equals a.P_ID
                                 join b in dbcontext.tbl_LookUp on a.P_UOM equals b.LU_Code
                                 into m
                                 from n in m.DefaultIfEmpty()
                                 where c.PD_PID == id && n.LU_Type == 2
                                 select new PO_CM
                                 {
                                     PO_OrderDetailID = (c.PO_OrderDetailID == null) ? 0 : c.PO_OrderDetailID,
                                     PD_ProductID = (a.P_ID == null) ? 0 : a.P_ID,
                                     PD_ShortName = a.P_ShortName,
                                     PD_UOM = n.LU_Description,
                                     PD_UOMCode = n.LU_Code,
                                     Description = a.P_Description,
                                     PD_Quantity = (c.PD_Quantity.HasValue ? c.PD_Quantity.Value:0),
                                     PD_UnitPrice = (c.PD_UnitPrice.HasValue ? c.PD_UnitPrice.Value:0),
                                     //DesignDetail = (dbcontext.Tbl_Order_Details.Where(a => a.AGEN_TRAD_PO_ID == head.PO_OrderReference).Select(a => a.DesignDetail).FirstOrDefault()),
                                     PD_TotalAmount = (c.PD_TotalAmount.HasValue ? c.PD_TotalAmount.Value:0),
                                     DesignDetail = c.DesignDetail,
                                     //PD_ID = c.PD_ID,
                                     //PD_PID = c.PD_PID,
                                     PD_SuppDef = c.SupplierDes
                                 }).Distinct().ToList();

                    POCombo_CM obj = new POCombo_CM();
                    obj.POHeader = data1;
                    obj.PODetails = data2;
                    if (lang == "E")
                        return PartialView("/Views/Purchase/ET_Purchase_PO/ET_Purchase_PO_Print.cshtml", obj);
                    else
                        return PartialView("/Views/Purchase/ET_Purchase_PO/ET_Purchase_PO_Print_German.cshtml", obj);
                }
                catch (Exception exe)
                {
                    string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                    string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                    objERR.err_title = controllerName + "-" + controllerName;
                    objERR.err_message = "Sorry for the inconvience. Some error has been occured." + exe.Message;
                    objERR.err_details = exe.Message.Replace("'", "");
                    int errid = bal.ExceptionInsertLogs_BL(objERR);
                    return Json("ERR" + errid.ToString(), JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return RedirectToAction("ET_SessionExpire", "ET_Login");
            }

        }
        public JsonResult ET_Purchase_PO_DetailsPrint(int id, string lang,int orderType)
        {
            objERR.err_title = "ET_Purchase_PO_DetailsPrint";
            objERR.err_message = "Details Print";
            objERR.err_details = "Details Print";
            bal.ExceptionInsertLogs_BL(objERR);

            bool val = Session["UserID"] == null ? false : true;
            var context = new EntityClasses();
            var transaction = context.Database.BeginTransaction();
            var doc1 = new iTextSharp.text.Document(PageSize.A4, 30, 25, 130, 90);
            try
            {
                dbcontext.Configuration.ProxyCreationEnabled = false;
                int com_key = Convert.ToInt32(Session["Companykey"]);
                var data1 = new List<PO_CM>();
                if (orderType == 3)
                {
                    data1 = (
                        from k in dbcontext.tbl_PurchaseOrderHeader
                            //join MO in dbcontext.Tbl_Master_Order on k.PO_OrderReference equals MO.SO_ID
                            //join MU in dbcontext.Tbl_Master_User on k.Po_Supplierkey equals MU.USER_ID
                        join CD in dbcontext.Tbl_Master_CompanyDetails on k.Po_Supplierkey equals CD.COM_ID
                        join SCD in dbcontext.Tbl_Master_CompanyDetails on k.Po_Supplierkey equals SCD.COM_ID
                        where k.PP_ID == id
                        select new PO_CM
                        {
                            Street = CD.COM_STREET,
                            CityState = (CD.COM_CITY),
                            Zipcode = CD.COM_COUNTRY,
                            CountryZip = ((dbcontext.locations.Where(a => a.location_id == CD.COM_COUNTRY).Select(a => a.name).FirstOrDefault()) + ", " + (CD.COM_ZIP)),
                            imgurl = (dbcontext.Tbl_SystemSetUp.Where(a => a.COMPANY_ID == com_key).Select(a => a.COMPANY_LOGO).FirstOrDefault()),
                            SysCompany = (dbcontext.Tbl_SystemSetUp.Where(a => a.COMPANY_ID == com_key).Select(a => a.COMPANY_NAME).FirstOrDefault()),
                            VatPer = (dbcontext.Tbl_SystemSetUp.Select(a => a.TAX).FirstOrDefault()),
                            USER_NAME = string.Empty,
                            CustomerName = CD.COM_NAME,
                            SUPPLIER_NAME = SCD.COM_NAME,
                            salesPersonName = string.Empty,
                            //PO_CutomerID = MO.SO_CutomerID,
                            PO_Date = (k.PO_Date.HasValue ? k.PO_Date.Value : DateTime.Now),
                            PO_Code = k.PO_Code,
                            PO_CusDeliveryTerms = k.Po_ShippingAddress,
                            Po_ShippingAddress = k.Po_ShippingAddress,
                            Po_TermsandConditions = k.Po_TermsandConditions,
                            Po_SpecialInstruction = k.Po_SpecialInstruction,
                            PO_Remarks = string.Empty,
                            PaymentTermsDescription = (dbcontext.Tbl_Payment_Terms.Where(m => m.PT_ID == k.Po_PaymentTerms).Select(m => m.PT_Name).FirstOrDefault()),
                            PO_OrderCode = string.Empty,
                            PO_CusPONO = string.Empty,
                            PO_SCDate = (k.PO_SCDate.HasValue ? k.PO_SCDate.Value : DateTime.Now),
                            PO_SCNo = k.PO_SCNo
                        }).ToList();
                }
                else
                {
                    data1 = (
                                 from k in dbcontext.tbl_PurchaseOrderHeader
                                 join MO in dbcontext.Tbl_Master_Order on k.PO_OrderReference equals MO.SO_ID
                                 join MU in dbcontext.Tbl_Master_User on MO.SO_SalesPersonID equals MU.USER_ID
                                 join CD in dbcontext.Tbl_Master_CompanyDetails on MO.SO_SupplierID equals CD.COM_ID
                                 join SCD in dbcontext.Tbl_Master_CompanyDetails on MO.SO_SupplierID equals SCD.COM_ID
                                 where k.PP_ID == id
                                 select new PO_CM
                                 {
                                     Street = CD.COM_STREET,
                                     CityState = (CD.COM_CITY),
                                     Zipcode = CD.COM_COUNTRY,
                                     CountryZip = ((dbcontext.locations.Where(a => a.location_id == CD.COM_COUNTRY).Select(a => a.name).FirstOrDefault()) + ", " + (CD.COM_ZIP)),
                                     imgurl = (dbcontext.Tbl_SystemSetUp.Where(a => a.COMPANY_ID == com_key).Select(a => a.COMPANY_LOGO).FirstOrDefault()),
                                     PO_CutomerID = MO.SO_CutomerID,
                                     SysCompany = (dbcontext.Tbl_SystemSetUp.Where(a => a.COMPANY_ID == com_key).Select(a => a.COMPANY_NAME).FirstOrDefault()),
                                     VatPer = (dbcontext.Tbl_SystemSetUp.Select(a => a.TAX).FirstOrDefault()),
                                     USER_NAME = MU.USER_NAME,
                                     CustomerName = CD.COM_NAME,
                                     SUPPLIER_NAME = SCD.COM_NAME,
                                     salesPersonName = MU.DISPLAY_NAME,
                                     PO_Code = k.PO_Code,
                                     PO_OrderType = MO.SO_OrderType,
                                     PO_CusDeliveryTerms = MO.SO_CusDeliveryTerms,
                                     Po_ShippingAddress = k.Po_ShippingAddress,
                                     Po_SpecialInstruction = k.Po_SpecialInstruction,
                                     Po_TermsandConditions = k.Po_TermsandConditions,
                                     PO_Remarks = string.Empty,
                                     PaymentTermsDescription = (dbcontext.Tbl_Payment_Terms.Where(m => m.PT_ID == k.Po_PaymentTerms).Select(m => m.PT_Name).FirstOrDefault()),
                                     PO_Date = k.PO_Date.Value,
                                     PO_OrderCode = MO.SO_Code,
                                     PO_CusPONO = MO.SO_CusPONO,
                                     PO_SCNo = k.PO_SCNo,
                                     PO_SCDate = (k.PO_SCDate.HasValue ? k.PO_SCDate.Value : DateTime.Now)
                                 }).ToList();
                }
                var data2 = (from c in dbcontext.tbl_PurchaseOrderDetails
                             join k in dbcontext.tbl_PurchaseOrderHeader on c.PD_PID equals k.PP_ID
                             join a in dbcontext.Tbl_Product_Master on c.PD_ProductID equals a.P_ID
                             //join OD in dbcontext.Tbl_Order_Details on k.PO_OrderReference equals OD.AGEN_TRAD_PO_ID
                             //join MO in dbcontext.Tbl_Master_Order on OD.AGEN_TRAD_PO_ID equals MO.SO_ID
                             join b in dbcontext.tbl_LookUp on a.P_UOM equals b.LU_Code
                             into m
                             from n in m.DefaultIfEmpty()
                             where c.PD_PID == id && n.LU_Type == 2
                             select new PO_CM
                             {
                                 PO_OrderDetailID = c.PO_OrderDetailID,
                                 PD_ProductID = a.P_ID,
                                 PD_ShortName = a.P_ShortName,
                                 PD_UOM = n.LU_Description,
                                 PD_UOMCode = n.LU_Code,
                                 Description = a.P_Description,
                                 PD_Quantity = c.PD_Quantity,
                                 PD_UnitPrice = c.PD_UnitPrice,
                                 //DesignDetail = (dbcontext.Tbl_Order_Details.Where(a => a.AGEN_TRAD_PO_ID == head.PO_OrderReference).Select(a => a.DesignDetail).FirstOrDefault()),
                                 PD_TotalAmount = c.PD_TotalAmount,
                                 DesignDetail = c.DesignDetail,
                                 //PD_ID = c.PD_ID,
                                 //PD_PID = c.PD_PID,
                                 PD_SuppDef = c.SupplierDes,
                                 //DiscountPer = OD.DiscountPer,
                                 PRODUCT_Name = a.P_Name,
                                 //CustomerDesc = OD.CustomerDes,
                                 PO_Date = k.PO_Date.Value,
                                 //PO_CusPONO = MO.SO_CusPONO,
                                 PO_Remarks = c.SupplierDes
                             }).Distinct().ToList();
                string path = "";
               
                if (lang == "E")
                {
                    for (int i = 0; i < data1.Count; i++)
                    {
                        string subPath = "~/Purchase/PDFList/PO/" + data1[i].PO_Code+ "/";
                        bool exists = System.IO.Directory.Exists(HttpContext.Server.MapPath(subPath));
                        if (!exists)
                            System.IO.Directory.CreateDirectory(HttpContext.Server.MapPath(subPath));

                       FileStream output = null;
                        bool fileReadProblem = false;
                        try
                        {
                            if (System.IO.File.Exists(subPath + data1[i].PO_Code + ".pdf"))
                            {
                                System.IO.File.Delete(subPath + data1[i].PO_Code + ".pdf");
                            }
                        }
                        catch (System.IO.IOException ex)
                        {
                            fileReadProblem = true;
                        }
                        if (fileReadProblem)
                        {
                            output = new FileStream(Server.MapPath(subPath + data1[i].PO_Code + "_" + Guid.NewGuid() + ".pdf"), FileMode.Create);
                        }
                        else
                        {
                            output = new FileStream(Server.MapPath(subPath + data1[i].PO_Code + ".pdf"), FileMode.Create);
                        }

                        var writer = PdfWriter.GetInstance(doc1, output);
                        writer.PageEvent = new pdffooterclass();
                        //PdfWriter.GetInstance(doc1, new FileStream(Request.PhysicalApplicationPath + "\\Invoice_Statement.pdf", FileMode.Create));
                        doc1.Open();
                        path = output.Name;
                        //font size change from default  added by gv on 12/12/18
                        FontFactory.RegisterDirectories();
                        Font font = new Font(FontFactory.GetFont("Arial", 10, Font.NORMAL));
                        Font fontsmall = new Font(FontFactory.GetFont("Arial", 9, Font.NORMAL));
                        Font fontsmall1 = new Font(FontFactory.GetFont("Arial", 10, Font.NORMAL | Font.UNDERLINE));
                        Font Fontbigger = new Font(FontFactory.GetFont("Arial", 20, Font.BOLD));
                        Font Fontsmaller = new Font(FontFactory.GetFont("Arial", 10, Font.BOLD));
                        Font Fontsmaller1 = new Font(FontFactory.GetFont("Arial", 12, Font.BOLD));

                        PdfPTable table1 = new PdfPTable(1);
                        table1.DefaultCell.Border = 0;
                        table1.WidthPercentage = 100f;

                        PdfPCell Title = new PdfPCell();
                        Title.Border = 0;

                        Title.AddElement(new Paragraph(new Chunk("Eurotextiles GmbH & Co. KG, Mammolshainer Weg 14, 61462 Königstein Ts.", fontsmall1)));
                        Title.VerticalAlignment = 2;
                        Title.PaddingTop = 1.0f;
                        Title.PaddingBottom = 3.0f;
                        Title.PaddingLeft = 30.0f;
                        table1.AddCell(Title);
                        Paragraph pg2 = new Paragraph();
                        Phrase phraseConstant2 = new Phrase("" + data1[i].SUPPLIER_NAME + "\n", font);
                        Phrase phraseConstant3 = new Phrase("" + data1[i].Street + "\n", font);
                        Phrase phraseConstant4 = new Phrase("" + data1[i].CityState + "\n", font);
                        Phrase phraseConstant5 = new Phrase("" + data1[i].CountryZip + "\n", font);
                        pg2.Add(phraseConstant2);
                        pg2.Add(phraseConstant3);
                        pg2.Add(phraseConstant4);
                        pg2.Add(phraseConstant5);
                        PdfPCell cell21 = new PdfPCell(pg2);
                        cell21.HorizontalAlignment = 0;
                        cell21.PaddingTop = 1.0f;
                        cell21.PaddingLeft = 30.0f;
                        cell21.Border = 0;
                        table1.AddCell(cell21);
                        doc1.Add(table1);

                        PdfPTable table2 = new PdfPTable(3);
                        table2.WidthPercentage = 100f;
                        float[] widthsvalforcus = new float[] { 55f,20f,25f };
                        table2.TotalWidth = 100f;
                        table2.WidthPercentage = 100f;
                        table2.SetWidths(widthsvalforcus);

                        Phrase emp1 = new Phrase("", font);
                        PdfPCell cell26emp = new PdfPCell(emp1);
                        cell26emp.Border = 0;
                        cell26emp.HorizontalAlignment = 2;
                        cell26emp.PaddingBottom = 2.5f;
                        table2.AddCell(cell26emp);

                        Phrase cuscode = new Phrase("Customer Code:", font);
                        PdfPCell cell26 = new PdfPCell(cuscode);
                        cell26.Border = 0;
                        cell26.HorizontalAlignment = 0;
                        cell26.PaddingBottom = 2.5f;
                        table2.AddCell(cell26);

                        Phrase cuscodeval = new Phrase("" + data1[i].PO_CutomerID + "", font);
                        PdfPCell cell26val = new PdfPCell(cuscodeval);
                        cell26val.Border = 0;
                        cell26val.HorizontalAlignment = 2;
                        cell26val.PaddingBottom = 2.5f;
                        table2.AddCell(cell26val);

                        Phrase emp2 = new Phrase("", font);
                        PdfPCell cell26emp1 = new PdfPCell(emp2);
                        cell26emp1.Border = 0;
                        cell26emp1.HorizontalAlignment = 0;
                        cell26emp1.PaddingBottom = 2.5f;
                        table2.AddCell(cell26emp1);
                        Phrase Processby = new Phrase("Processed By:", font);
                        PdfPCell cell26Processby = new PdfPCell(Processby);
                        cell26Processby.Border = 0;
                        cell26Processby.HorizontalAlignment = 0;
                        cell26Processby.PaddingBottom = 2.5f;
                        table2.AddCell(cell26Processby);
                        Phrase cuscodeval1 = new Phrase("" + data1[i].salesPersonName + "", font);
                        PdfPCell cell26Processby1 = new PdfPCell(cuscodeval1);
                        cell26Processby1.Border = 0;
                        cell26Processby1.HorizontalAlignment = 2;
                        cell26Processby1.PaddingBottom = 2.5f;
                        table2.AddCell(cell26Processby1);

                        Phrase emp3 = new Phrase("", font);
                        PdfPCell cell26emp2 = new PdfPCell(emp3);
                        cell26emp2.Border = 0;
                        cell26emp2.HorizontalAlignment = 2;
                        cell26emp2.PaddingBottom = 2.5f;
                        table2.AddCell(cell26emp2);
                        Phrase Datecell = new Phrase("Date:", font);
                        PdfPCell cell26Datecell = new PdfPCell(Datecell);
                        cell26Datecell.Border = 0;
                        cell26Datecell.HorizontalAlignment = 0;
                        cell26Datecell.PaddingBottom = 2.5f;
                        table2.AddCell(cell26Datecell);
                        Phrase cuscodeval2 = new Phrase("" + data1[i].PO_Date.ToString("dd-MM-yyyy") + "", font);
                        PdfPCell cell26Processby2 = new PdfPCell(cuscodeval2);
                        cell26Processby2.Border = 0;
                        cell26Processby2.HorizontalAlignment = 2;
                        cell26Processby2.PaddingBottom = 2.5f;
                        table2.AddCell(cell26Processby2);
                        Phrase emp4 = new Phrase("", font);
                        PdfPCell cell26emp3 = new PdfPCell(emp4);
                        cell26emp3.Border = 0;
                        cell26emp3.HorizontalAlignment = 2;
                        cell26emp3.PaddingBottom = 2.5f;
                        table2.AddCell(cell26emp3);

                        //Phrase Datecell12 = new Phrase("Customer Order No:", font);
                        //PdfPCell cell26Datecell12 = new PdfPCell(Datecell12);
                        //cell26Datecell12.Border = 0;
                        //cell26Datecell12.HorizontalAlignment = 0;
                        //cell26Datecell12.PaddingBottom = 2.5f;
                        //table2.AddCell(cell26Datecell12);
                        //Phrase cuscodeval211 = new Phrase("" + data1[i].PO_CusPONO + "", font);
                        //PdfPCell cell26Processby23 = new PdfPCell(cuscodeval211);
                        //cell26Processby23.Border = 0;
                        //cell26Processby23.HorizontalAlignment = 0;
                        //cell26Processby23.PaddingBottom = 2.5f;
                        //table2.AddCell(cell26Processby23);

                        //Phrase emp5 = new Phrase("", font);
                        //PdfPCell cell26emp4 = new PdfPCell(emp5);
                        //cell26emp4.Border = 0;
                        //cell26emp4.HorizontalAlignment = 2;
                        //cell26emp4.PaddingBottom = 2.5f;
                        //table2.AddCell(cell26emp4);

                        Phrase Datecell1 = new Phrase("Purchase Order No:", font);
                        PdfPCell cell26Datecell1 = new PdfPCell(Datecell1);
                        cell26Datecell1.Border = 0;
                        cell26Datecell1.HorizontalAlignment = 0;
                        cell26Datecell1.PaddingBottom = 2.5f;
                        table2.AddCell(cell26Datecell1);
                        Phrase cuscodeval21 = new Phrase("" + data1[i].PO_Code + "", font);
                        PdfPCell cell26Processby21 = new PdfPCell(cuscodeval21);
                        cell26Processby21.Border = 0;
                        cell26Processby21.HorizontalAlignment = 2;
                        cell26Processby21.PaddingBottom = 2.5f;
                        table2.AddCell(cell26Processby21);

                        Phrase emp6 = new Phrase("", font);
                        PdfPCell cell26emp6 = new PdfPCell(emp6);
                        cell26emp6.Border = 0;
                        cell26emp6.HorizontalAlignment = 2;
                        cell26emp6.PaddingBottom = 2.5f;
                        table2.AddCell(cell26emp6);

                        Phrase scCodePhrase = new Phrase("Sales Contract No:", font);
                        PdfPCell cellSC26 = new PdfPCell(scCodePhrase);
                        cellSC26.Border = 0;
                        cellSC26.HorizontalAlignment = 0;
                        cellSC26.PaddingBottom = 2.5f;
                        table2.AddCell(cellSC26);

                        Phrase scCodeValue = new Phrase("" + data1[i].PO_SCNo + "", font);
                        PdfPCell cellSCCodeVal = new PdfPCell(scCodeValue);
                        cellSCCodeVal.Border = 0;
                        cellSCCodeVal.HorizontalAlignment = 2;
                        cellSCCodeVal.PaddingBottom = 2.5f;
                        table2.AddCell(cellSCCodeVal);

                        Phrase emp7 = new Phrase("", font);
                        PdfPCell cell26emp7 = new PdfPCell(emp7);
                        cell26emp7.Border = 0;
                        cell26emp7.HorizontalAlignment = 2;
                        cell26emp7.PaddingBottom = 2.5f;
                        table2.AddCell(cell26emp7);

                        Phrase scDatePhrase = new Phrase("Sales Contract Date:", font);
                        PdfPCell cellSCDate26 = new PdfPCell(scDatePhrase);
                        cellSCDate26.Border = 0;
                        cellSCDate26.HorizontalAlignment = 0;
                        cellSCDate26.PaddingBottom = 2.5f;
                        table2.AddCell(cellSCDate26);

                        Phrase scDateValue = new Phrase("" + data1[i].PO_SCDate.ToString("dd-MM-yyyy") + "", font);
                        PdfPCell cellSCDateVal = new PdfPCell(scDateValue);
                        cellSCDateVal.Border = 0;
                        cellSCDateVal.HorizontalAlignment = 2;
                        cellSCDateVal.PaddingBottom = 2.5f;
                        table2.AddCell(cellSCDateVal);
                        doc1.Add(table2);

                        PdfPTable contenttable = new PdfPTable(1);
                        contenttable.TotalWidth = 100f;
                        contenttable.WidthPercentage = 100f;
                        Paragraph pp1con = new Paragraph();
                        Phrase phar1con = new Phrase("PO Confirmation", Fontsmaller1);
                        pp1con.Add(phar1con);
                        PdfPCell cell29Con = new PdfPCell(pp1con);
                        //cell29Con.AddElement(new Paragraph(new Chunk("" , Fontsmaller)));
                        cell29Con.PaddingTop = 4.5f;
                        cell29Con.HorizontalAlignment = 1;
                        cell29Con.PaddingBottom = 5.5f;
                        cell29Con.Border = 0;
                        contenttable.AddCell(cell29Con);
                        doc1.Add(contenttable);


                        PdfPTable table4 = new PdfPTable(6);
                        float[] widths1 = new float[] { 10f, 12f, 12f, 50f, 15f, 15f};
                        table4.TotalWidth = 100f;
                        table4.WidthPercentage = 100f;
                        table4.HeaderRows = 1;
                        table4.SetWidths(widths1);

                        Phrase phraseConstantde1 = new Phrase("S.No", Fontsmaller);
                        PdfPCell cell41 = new PdfPCell(phraseConstantde1);
                        //cell41.AddElement(new Paragraph(new Chunk("Item", Fontsmaller)));
                        cell41.HorizontalAlignment = 1;
                        cell41.PaddingTop = 2.5f;
                        cell41.PaddingBottom = 2.5f;
                        table4.AddCell(cell41);

                        Phrase phraseConstantde2 = new Phrase("Quantity", Fontsmaller);
                        PdfPCell cell42 = new PdfPCell(phraseConstantde2);
                        cell42.Colspan = 2;
                        //cell42.AddElement(new Paragraph(new Chunk("Qty", Fontsmaller)));
                        cell42.HorizontalAlignment = 1;
                        cell42.PaddingTop = 2.5f;
                        cell42.PaddingBottom = 2.5f;

                        table4.AddCell(cell42);
                        Phrase phraseConstantde3 = new Phrase("Product Description", Fontsmaller);
                        PdfPCell cell421 = new PdfPCell(phraseConstantde3);
                        cell421.HorizontalAlignment = 1;
                        //cell421.AddElement(new Paragraph(new Chunk("UOM", Fontsmaller)));
                        cell421.PaddingTop = 2.5f;
                        cell421.PaddingBottom = 2.5f;
                        table4.AddCell(cell421);

                        //Phrase phraseConstantdeDesign = new Phrase("Design", Fontsmaller);
                        //PdfPCell cellDesignHeader = new PdfPCell(phraseConstantdeDesign);
                        //cellDesignHeader.HorizontalAlignment = 1;
                        ////cell421.AddElement(new Paragraph(new Chunk("UOM", Fontsmaller)));
                        //cellDesignHeader.PaddingTop = 2.5f;
                        //cellDesignHeader.PaddingBottom = 2.5f;
                        //table4.AddCell(cellDesignHeader);

                        //Phrase phraseConstantdeRemarks = new Phrase("Remarks", Fontsmaller);
                        //PdfPCell cellRemarksHeader = new PdfPCell(phraseConstantdeRemarks);
                        //cellRemarksHeader.HorizontalAlignment = 1;
                        ////cell421.AddElement(new Paragraph(new Chunk("UOM", Fontsmaller)));
                        //cellRemarksHeader.PaddingTop = 2.5f;
                        //cellRemarksHeader.PaddingBottom = 2.5f;
                        //table4.AddCell(cellRemarksHeader);

                        Phrase phraseConstantde4 = new Phrase("Price(€)", Fontsmaller);
                        PdfPCell cell431 = new PdfPCell(phraseConstantde4);
                        cell431.HorizontalAlignment = 1;
                        // cell431.Colspan = 2;
                        //cell431.AddElement(new Paragraph(new Chunk("Product Name", Fontsmaller)));
                        cell431.PaddingTop = 2.5f;
                        cell431.PaddingBottom = 2.5f;

                        table4.AddCell(cell431);
                        //Phrase phraseConstantde4d = new Phrase("Discount(%)", Fontsmaller);
                        //PdfPCell cell431d = new PdfPCell(phraseConstantde4d);
                        //cell431d.HorizontalAlignment = 1;
                        //// cell431d.Colspan = 2;
                        ////cell431.AddElement(new Paragraph(new Chunk("Product Name", Fontsmaller)));

                        //cell431d.PaddingTop = 2.5f;
                        //cell431d.PaddingBottom = 2.5f;

                        //table4.AddCell(cell431d);
                        Phrase phraseConstantde5 = new Phrase("Total(€)", Fontsmaller);
                        PdfPCell cell43 = new PdfPCell(phraseConstantde5);
                        cell43.HorizontalAlignment = 1;
                        // cell43.AddElement(new Paragraph(new Chunk("Description", Fontsmaller)));
                        cell43.PaddingTop = 2.5f;
                        cell43.PaddingBottom = 2.5f;
                        table4.AddCell(cell43);

                        decimal? total = 0;
                        for (int j = 0; j < data2.Count; j++)
                        {
                            Paragraph Snopp = new Paragraph();
                            Phrase Snophar = new Phrase("" + (j + 1) + "\n", font);
                            Snopp.Add(Snophar);
                            PdfPCell cell51 = new PdfPCell(Snopp);
                            cell51.HorizontalAlignment = 1;
                            //cell51.AddElement(new Paragraph(new Chunk("" + (j + 1) + "", font)));

                            // cell51.PaddingTop = 2.5f;
                            // cell51.PaddingBottom = 2.5f;

                            table4.AddCell(cell51);
                            Paragraph qtypp = new Paragraph();
                            Phrase qtyphar = new Phrase("" + Convert.ToDecimal(data2[j].PD_Quantity).ToString("N2", CultureInfo.CreateSpecificCulture("de-DE")) + "\n", font);
                            qtypp.Add(qtyphar);
                            PdfPCell cell52 = new PdfPCell(qtypp);
                            cell52.HorizontalAlignment = 2;
                            //  cell52.PaddingTop = 2.5f;
                            // cell52.PaddingBottom = 2.5f;

                            table4.AddCell(cell52);
                            Paragraph uompp = new Paragraph();
                            Phrase uomphar = new Phrase("" + data2[j].PD_UOM + "\n", font);
                            uompp.Add(uomphar);
                            PdfPCell uomcell = new PdfPCell(uompp);

                            // uomcell.AddElement(new Paragraph(new Chunk("" + data2[j].UOM_NAME + "", font)));
                            uomcell.HorizontalAlignment = 0;
                            // uomcell.PaddingTop = 2.5f;
                            // uomcell.PaddingBottom = 2.5f;

                            table4.AddCell(uomcell);

                            if (data1[i].PO_OrderType == 2)
                            {
                                Paragraph Productpp = new Paragraph();
                                Phrase Productphar1 = new Phrase("" + data2[j].PD_ShortName + "\n", font);
                                Phrase Productphar2 = new Phrase("" + data2[j].Description + "\n", font);
                                Phrase Productphar3 = new Phrase("" + data2[j].DesignDetail + "\n", font);
                                Phrase Productphar4 = new Phrase("" + data2[j].PO_Remarks + "\n", font);
                                Productpp.Add(Productphar1);
                                Productpp.Add(Productphar2);
                                Productpp.Add(Productphar3);
                                Productpp.Add(Productphar4);
                                PdfPCell cell53 = new PdfPCell(Productpp);
                                // cell53.AddElement(new Paragraph(new Chunk("" + data2[j].PRODUCT_Name + "\n", font)));
                                // cell53.AddElement(new Paragraph(new Chunk("" + data2[j].DesignDetail + "\n", font)));
                                cell53.HorizontalAlignment = 0;
                                cell53.PaddingTop = 0f;
                                // cell53.PaddingBottom = 2.5f;
                                table4.AddCell(cell53);

                                //Paragraph ProductDesignPara = new Paragraph();
                                //Phrase ProductDesign = new Phrase("" + data2[j].DesignDetail + "\n", Fontsmaller);
                                //ProductDesignPara.Add(ProductDesign);
                                //PdfPCell cellDesign = new PdfPCell(ProductDesignPara);
                                //cellDesign.HorizontalAlignment = 0;
                                //cellDesign.PaddingTop = 0f;
                                //table4.AddCell(cellDesign);

                                //Paragraph ProductRemarksPara = new Paragraph();
                                //Phrase ProductRemarks = new Phrase("" + data2[j].PD_SuppDef + "\n", Fontsmaller);
                                //ProductRemarksPara.Add(ProductRemarks);
                                //PdfPCell cellRemarks = new PdfPCell(ProductRemarksPara);
                                //cellRemarks.HorizontalAlignment = 0;
                                //cellRemarks.PaddingTop = 0f;
                                //table4.AddCell(cellRemarks);

                            }
                            else
                            {
                                Paragraph Productpp = new Paragraph();
                                Phrase Productphar1 = new Phrase("" + data2[j].PD_ShortName + "\n", font);
                                Phrase Productphar2 = new Phrase("" + data2[j].Description + "\n", font);
                                Phrase Productphar3 = new Phrase("" + data2[j].DesignDetail + "\n", font);
                                Phrase Productphar4 = new Phrase("" + data2[j].PO_Remarks + "\n", font);
                                Productpp.Add(Productphar1);
                                Productpp.Add(Productphar2);
                                Productpp.Add(Productphar3);
                                Productpp.Add(Productphar4);
                                PdfPCell cell53 = new PdfPCell(Productpp);
                                // cell53.AddElement(new Paragraph(new Chunk("" + data2[j].PRODUCT_Name + "", font)));
                                cell53.HorizontalAlignment = 0;
                                cell53.PaddingTop = 0f;
                                //cell53.PaddingTop = 2.5f;
                                // cell53.PaddingBottom = 2.5f;
                                table4.AddCell(cell53);
                            }

                            if (data1[i].PO_OrderType == 2 || data1[i].PO_OrderType== 3)
                            {
                                Paragraph pp = new Paragraph();
                                Phrase phar = new Phrase("" + Convert.ToDecimal(data2[j].PD_UnitPrice).ToString("c2", CultureInfo.CreateSpecificCulture("de-DE")) + "\n", font);
                                pp.Add(phar);
                                PdfPCell cell541 = new PdfPCell(pp);
                                //cell541.AddElement(pp);
                                // cell541.Colspan = 2;
                                cell541.HorizontalAlignment = 2;
                                cell541.PaddingTop = 2.5f;
                                cell541.PaddingBottom = 2.5f;
                                table4.AddCell(cell541);
                                decimal? DisPer = 0;
                                //if (data2[j].DiscountPer == 0)
                                //{
                                //    DisPer = 0;
                                //}
                                //else
                                //{
                                //    DisPer = data2[j].DiscountPer;
                                //}
                                //Paragraph ppd = new Paragraph();
                                //Phrase phard = new Phrase("" + DisPer + "\n", font);
                                //ppd.Add(phard);
                                //PdfPCell cell541d = new PdfPCell(ppd);
                                ////cell541.AddElement(pp);
                                ////cell541.Colspan = 2;
                                //cell541d.HorizontalAlignment = 2;
                                //cell541d.PaddingTop = 2.5f;
                                //cell541d.PaddingBottom = 2.5f;

                                //table4.AddCell(cell541d);
                                decimal discountAmt = ((Convert.ToDecimal(data2[j].PD_UnitPrice) * Convert.ToDecimal(data2[j].PD_Quantity)) * Convert.ToDecimal(DisPer) / 100);
                                Paragraph pp12 = new Paragraph();
                                Phrase phar12 = new Phrase("" + ((Convert.ToDecimal(data2[j].PD_UnitPrice) * Convert.ToDecimal(data2[j].PD_Quantity)) - discountAmt).ToString("c2", CultureInfo.CreateSpecificCulture("de-DE")) + "\n", font);
                                pp12.Add(phar12);
                                PdfPCell cell54 = new PdfPCell(pp12);
                                cell54.HorizontalAlignment = 2;
                                cell54.PaddingTop = 2.5f;
                                cell54.PaddingBottom = 2.5f;

                                table4.AddCell(cell54);
                                total = total + ((Convert.ToDecimal(data2[j].PD_UnitPrice) * Convert.ToDecimal(data2[j].PD_Quantity)) - discountAmt);
                            }
                            else
                            {
                                Paragraph pp = new Paragraph();
                                Phrase phar = new Phrase("" + Convert.ToDecimal(data2[j].PD_UnitPrice).ToString("c2", CultureInfo.CreateSpecificCulture("de-DE")) + "\n", font);
                                pp.Add(phar);
                                PdfPCell cell541 = new PdfPCell(pp);
                                //cell541.AddElement(pp);
                                //cell541.Colspan = 2;
                                cell541.HorizontalAlignment = 2;
                                cell541.PaddingTop = 2.5f;
                                cell541.PaddingBottom = 2.5f;

                                table4.AddCell(cell541);

                                Paragraph pp12 = new Paragraph();
                                Phrase phar12 = new Phrase("" + (Convert.ToDecimal(data2[j].PD_UnitPrice) * Convert.ToDecimal(data2[j].PD_Quantity)).ToString("c2", CultureInfo.CreateSpecificCulture("de-DE")) + "\n", font);
                                pp12.Add(phar12);
                                PdfPCell cell54 = new PdfPCell(pp12);
                                cell54.HorizontalAlignment = 2;
                                cell54.PaddingTop = 2.5f;
                                cell54.PaddingBottom = 2.5f;
                                table4.AddCell(cell54);
                                total = total + (Convert.ToDecimal(data2[j].PD_UnitPrice) * Convert.ToDecimal(data2[j].PD_Quantity));
                            }
                        }

                        //PdfPCell NetValCell = new PdfPCell();

                        //NetValCell.AddElement(new Paragraph(new Chunk("Net Value", font)));
                        //NetValCell.Colspan = 8;
                        //NetValCell.PaddingTop = 2.5f;
                        //NetValCell.PaddingBottom = 2.5f;

                        //table4.AddCell(NetValCell);
                        //Paragraph pp1 = new Paragraph();
                        //Phrase phar1 = new Phrase("" + Convert.ToDecimal(total).ToString("c2", CultureInfo.CreateSpecificCulture("de-DE")) + "\n", font);
                        //pp1.Add(phar1);
                        //PdfPCell NetValCell1 = new PdfPCell(pp1);
                        //NetValCell1.Colspan = 8;
                        //NetValCell1.HorizontalAlignment = 2;
                        //NetValCell1.VerticalAlignment = 2;
                        //NetValCell1.PaddingTop = 2.5f;
                        //NetValCell1.PaddingBottom = 2.5f;

                        //table4.AddCell(NetValCell1);

                        /*if (data1[i].Zipcode == 82)
                        {
                            PdfPCell inccell = new PdfPCell();
                            if (data1[i].VatPer == null)
                                data1[i].VatPer = 0;
                            inccell.AddElement(new Paragraph(new Chunk(Convert.ToDecimal(data1[i].VatPer).ToString("0") + " % Value", font)));
                            inccell.Colspan = 8;
                            inccell.PaddingTop = 2.5f;
                            inccell.PaddingBottom = 2.5f;

                            table4.AddCell(inccell);
                            Paragraph pp1inc = new Paragraph();
                            Phrase phar1inc = new Phrase("" + (Convert.ToDecimal(total * data1[i].VatPer / 100).ToString("C2", CultureInfo.CreateSpecificCulture("de-DE"))) + "\n", font);
                            pp1inc.Add(phar1inc);
                            PdfPCell inccell1 = new PdfPCell(pp1inc);
                            inccell1.HorizontalAlignment = 2;
                            inccell1.VerticalAlignment = 2;
                            inccell1.PaddingTop = 2.5f;
                            inccell1.PaddingBottom = 2.5f;

                            table4.AddCell(inccell1);
                        }
                        else
                        {

                            PdfPCell inccell = new PdfPCell();
                            inccell.AddElement(new Paragraph(new Chunk("0 % Value", font)));
                            inccell.Colspan = 8;
                            inccell.PaddingTop = 2.5f;
                            inccell.PaddingBottom = 2.5f;

                            table4.AddCell(inccell);
                            Paragraph pp1inc = new Paragraph();
                            Phrase phar1inc = new Phrase("" + (Convert.ToDecimal(total * 0 / 100).ToString("C2", CultureInfo.CreateSpecificCulture("de-DE"))) + "\n", font);
                            pp1inc.Add(phar1inc);
                            PdfPCell inccell1 = new PdfPCell(pp1inc);
                            inccell1.HorizontalAlignment = 2;
                            inccell1.PaddingTop = 2.5f;
                            inccell1.PaddingBottom = 2.5f;

                            table4.AddCell(inccell1);

                        }*/

                        decimal Discountamt = 0;

                        if (data1[i].Zipcode == 82)
                        {

                            if (data1[i].VatPer == null)
                                data1[i].VatPer = 0;

                            //var Vatamount = (Convert.ToDecimal(total + ((total * data1[i].VatPer / 100))));
                            //Vatamount = (total * data1[i].VatPer / 100) ?? 0;


                            //if (data1[i].Discount != 0)
                            //{
                            //    Discountamt = (Convert.ToDecimal(total * data1[i].Discount / 100));
                            //    PdfPCell AllDiscountcell = new PdfPCell();

                            //    AllDiscountcell.AddElement(new Paragraph(new Chunk("Discount(%)", font)));
                            //    AllDiscountcell.Colspan = 8;
                            //    AllDiscountcell.PaddingTop = 2.5f;
                            //    AllDiscountcell.PaddingBottom = 2.5f;

                            //    table4.AddCell(AllDiscountcell);

                            //    Paragraph pp1discount = new Paragraph();
                            //    Phrase phar1Discount = new Phrase("" + Discountamt.ToString("C2", CultureInfo.CreateSpecificCulture("de-DE")) + "\n", Fontsmaller);
                            //    pp1discount.Add(phar1Discount);
                            //    PdfPCell AllDiscountcellde = new PdfPCell(pp1discount);
                            //    AllDiscountcellde.HorizontalAlignment = 2;
                            //    AllDiscountcellde.Colspan = 8;
                            //    AllDiscountcellde.PaddingTop = 2.5f;
                            //    AllDiscountcellde.PaddingBottom = 2.5f;
                            //    AllDiscountcellde.HorizontalAlignment = 2;
                            //    table4.AddCell(AllDiscountcellde);
                            //}
                            PdfPCell vataddcell = new PdfPCell();

                            vataddcell.AddElement(new Paragraph(new Chunk("Total", font)));
                            vataddcell.Colspan = 5;
                            vataddcell.PaddingTop = 2.5f;
                            vataddcell.PaddingBottom = 2.5f;
                            table4.AddCell(vataddcell);

                            Paragraph pp1vat = new Paragraph();
                            Phrase phar1vat = new Phrase("" + ((Convert.ToDecimal(total - Discountamt)).ToString("C2", CultureInfo.CreateSpecificCulture("de-DE"))) + "\n", Fontsmaller);
                            pp1vat.Add(phar1vat);
                            PdfPCell vataddcell1 = new PdfPCell(pp1vat);
                            vataddcell1.HorizontalAlignment = 2;
                            //vataddcell1.Colspan = 6;
                            vataddcell1.PaddingTop = 2.5f;
                            vataddcell1.PaddingBottom = 2.5f;
                            table4.AddCell(vataddcell1);

                        }
                        else
                        {
                            //var Vatamount = (Convert.ToDecimal(total + ((total * 0 / 100))));
                            //Vatamount = (total * 0 / 100) ?? 0;

                            //if (data1[i].Discount != 0)
                            //{
                            //    Discountamt = (Convert.ToDecimal(total * data1[i].Discount / 100));
                            //    PdfPCell AllDiscountcell = new PdfPCell();

                            //    AllDiscountcell.AddElement(new Paragraph(new Chunk("Discount(%)", font)));
                            //    AllDiscountcell.Colspan = 8;
                            //    AllDiscountcell.PaddingTop = 2.5f;
                            //    AllDiscountcell.PaddingBottom = 2.5f;

                            //    table4.AddCell(AllDiscountcell);
                            //    Paragraph pp1discount = new Paragraph();
                            //    Phrase phar1Discount = new Phrase("" + Discountamt.ToString("C2", CultureInfo.CreateSpecificCulture("de-DE")) + "\n", Fontsmaller);
                            //    pp1discount.Add(phar1Discount);
                            //    PdfPCell AllDiscountcellde = new PdfPCell(pp1discount);
                            //    AllDiscountcellde.Colspan = 8;
                            //    AllDiscountcellde.PaddingTop = 2.5f;
                            //    AllDiscountcellde.PaddingBottom = 2.5f;
                            //    AllDiscountcellde.HorizontalAlignment = 2;
                            //    table4.AddCell(AllDiscountcellde);
                            //}
                            PdfPCell vataddcell = new PdfPCell();

                            vataddcell.AddElement(new Paragraph(new Chunk("Total", font)));
                            vataddcell.Colspan = 5;
                            vataddcell.PaddingTop = 2.5f;
                            vataddcell.PaddingBottom = 2.5f;
                            table4.AddCell(vataddcell);

                            Paragraph pp1vat = new Paragraph();
                            Phrase phar1vat = new Phrase("" + ((Convert.ToDecimal(total - Discountamt)).ToString("C2", CultureInfo.CreateSpecificCulture("de-DE"))) + "\n", Fontsmaller);
                            pp1vat.Add(phar1vat);
                            PdfPCell vataddcell1 = new PdfPCell(pp1vat);
                            vataddcell1.HorizontalAlignment = 2;
                            //vataddcell1.Colspan = 8;
                            vataddcell1.PaddingTop = 2.5f;
                            vataddcell1.PaddingBottom = 2.5f;
                            table4.AddCell(vataddcell1);

                        }
                        doc1.Add(table4);

                        PdfPTable table6 = new PdfPTable(1);
                        PdfPCell cell31 = new PdfPCell();
                        cell31.Border = 0;
                        cell31.AddElement(new Paragraph(new Chunk("", font)));
                        cell31.PaddingTop = 20.5f;
                        cell31.PaddingBottom = 2.5f;
                        table6.AddCell(cell31);

                        PdfPCell cell39 = new PdfPCell();
                        cell39.Border = 0;
                        cell39.AddElement(new Paragraph(""));
                        //cell39.PaddingTop = 280.0f;
                        cell39.PaddingBottom = 2.5f;
                        table6.AddCell(cell39);
                        doc1.Add(table6);

                        PdfPTable table7 = new PdfPTable(1);
                        table7.WidthPercentage = 100f;

                        //string[] splitedRemarks = data1[i].PO_Remarks.Split('.');
                        //for (int k = 0; k < splitedRemarks.Count(); k++)
                        //{
                        //    if (k == 0)
                        //    {
                        //        PdfPCell cell33re = new PdfPCell();
                        //        cell33re.AddElement(new Paragraph(new Chunk("Remarks:" + splitedRemarks[k], font)));

                        //        cell33re.PaddingTop = 2.5f;
                        //        cell33re.PaddingBottom = 2.5f;
                        //        cell33re.Border = 0;
                        //        table7.AddCell(cell33re);
                        //    }
                        //    else
                        //    {
                        //        if (splitedRemarks[k] != "")
                        //        {
                        //            PdfPCell cell33re = new PdfPCell();
                        //            cell33re.AddElement(new Paragraph(new Chunk(splitedRemarks[k], font)));

                        //            cell33re.PaddingTop = 2.5f;
                        //            cell33re.PaddingBottom = 2.5f;
                        //            cell33re.Border = 0;
                        //            table7.AddCell(cell33re);
                        //        }

                        //    }

                        //}

                        if (data1[i].PaymentTermsDescription != string.Empty)
                        {
                            PdfPCell cell33 = new PdfPCell();
                            cell33.AddElement(new Paragraph(new Chunk("Payment Terms: " + data1[i].PaymentTermsDescription + "", font)));
                            cell33.PaddingTop = 2.5f;
                            cell33.PaddingBottom = 2.5f;
                            cell33.Border = 0;
                            table7.AddCell(cell33);
                        }

                        if (data1[i].Po_ShippingAddress != string.Empty)
                        {
                            PdfPCell delitercell = new PdfPCell();
                            //delitercell.AddElement(new Paragraph(new Chunk("Delivery Terms: " + data1[i].SO_CusDeliveryTerms + "", font)));
                            //delitercell.PaddingTop = 2.5f;
                            //delitercell.PaddingBottom = 2.5f;
                            //delitercell.Border = 0;
                            //table7.AddCell(delitercell);
                            string[] splitedDeliveryTerms = data1[i].Po_ShippingAddress.Split('.');
                            for (int k = 0; k < splitedDeliveryTerms.Count(); k++)
                            {
                                if (k == 0)
                                {
                                    PdfPCell cell33re = new PdfPCell();
                                    cell33re.AddElement(new Paragraph(new Chunk("Delivery Terms:" + splitedDeliveryTerms[k], font)));

                                    cell33re.PaddingTop = 2.5f;
                                    cell33re.PaddingBottom = 2.5f;
                                    cell33re.Border = 0;
                                    table7.AddCell(cell33re);
                                }
                                else
                                {
                                    if (splitedDeliveryTerms[k] != "")
                                    {
                                        PdfPCell cell33re = new PdfPCell();
                                        cell33re.AddElement(new Paragraph(new Chunk(splitedDeliveryTerms[k], font)));

                                        cell33re.PaddingTop = 2.5f;
                                        cell33re.PaddingBottom = 2.5f;
                                        cell33re.Border = 0;
                                        table7.AddCell(cell33re);
                                    }
                                }
                            }
                        }

                        if (data1[i].Po_SpecialInstruction != string.Empty)
                        {
                           

                            string[] splittedSpecialInstruction = data1[i].Po_SpecialInstruction.Split('.');
                            for (int k = 0; k < splittedSpecialInstruction.Count(); k++)
                            {
                                if (k == 0)
                                {
                                    PdfPCell specialCell = new PdfPCell();
                                    specialCell.AddElement(new Paragraph(new Chunk("Special Instructions: " + splittedSpecialInstruction[k] + "", font)));
                                    specialCell.PaddingTop = 2.5f;
                                    specialCell.PaddingBottom = 2.5f;
                                    specialCell.Border = 0;
                                    table7.AddCell(specialCell);

                                    //PdfPCell cell33re = new PdfPCell();
                                    //cell33re.AddElement(new Paragraph(new Chunk("Delivery Terms:" + splitedDeliveryTerms[k], font)));

                                    //cell33re.PaddingTop = 2.5f;
                                    //cell33re.PaddingBottom = 2.5f;
                                    //cell33re.Border = 0;
                                    //table7.AddCell(cell33re);
                                }
                                else
                                {
                                    if (splittedSpecialInstruction[k] != "")
                                    {
                                        PdfPCell cell33re = new PdfPCell();
                                        cell33re.AddElement(new Paragraph(new Chunk(splittedSpecialInstruction[k], font)));
                                        cell33re.PaddingTop = 2.5f;
                                        cell33re.PaddingBottom = 2.5f;
                                        cell33re.Border = 0;
                                        table7.AddCell(cell33re);
                                    }
                                }
                            }

                        }

                        if (data1[i].Po_TermsandConditions != string.Empty)
                        {
                            PdfPCell termsCell = new PdfPCell();
                            termsCell.AddElement(new Paragraph(new Chunk("Terms & Conditions: " + data1[i].Po_TermsandConditions + "", font)));
                            termsCell.PaddingTop = 2.5f;
                            termsCell.PaddingBottom = 2.5f;
                            termsCell.Border = 0;
                            table7.AddCell(termsCell);
                        }

                        PdfPCell Linecell = new PdfPCell();
                        Linecell.AddElement(new Paragraph(new Chunk("___________________________", font)));
                        Linecell.PaddingTop = 25.5f;
                        Linecell.PaddingBottom = 0f;
                        Linecell.Border = 0;
                        table7.AddCell(Linecell);
                        PdfPCell signcell = new PdfPCell();

                        signcell.AddElement(new Paragraph(new Chunk("Eurotextiles GmbH & Co. KG", font)));

                        signcell.PaddingTop = 0f;
                        signcell.PaddingBottom = 2.5f;
                        signcell.Border = 0;
                        table7.AddCell(signcell);
                        PdfPCell line1 = new PdfPCell();

                        line1.AddElement(new Paragraph(new Chunk("Our general terms of business apply.", font)));

                        line1.PaddingTop = 6.5f;
                        line1.PaddingBottom = 2.5f;
                        line1.Border = 0;
                        table7.AddCell(line1);
                        PdfPCell line2 = new PdfPCell();

                        line2.AddElement(new Paragraph(new Chunk("Any quality/quantity variations have to be notified to us before using the goods but not later than 7 days on receipt of the goods.", font)));

                        line2.PaddingTop = 2.5f;
                        line2.PaddingBottom = 2.5f;
                        line2.Border = 0;
                        table7.AddCell(line2);
                        PdfPCell line3 = new PdfPCell();
                        line3.AddElement(new Paragraph(new Chunk("No claims will be accepted after washing/using the goods.", font)));
                        line3.PaddingTop = 2.5f;
                        line3.PaddingBottom = 2.5f;
                        line3.Border = 0;
                        table7.AddCell(line3);

                        PdfPCell line4 = new PdfPCell();
                        line4.AddElement(new Paragraph(new Chunk("Data Protection: Please note that we are working with IT Systems and have stored your name and other relevant data in the context of the matter.The Data will not be transfered to a third party.", font)));
                        line4.PaddingTop = 2.5f;
                        line4.PaddingBottom = 2.5f;
                        line4.Border = 0;
                        table7.AddCell(line4);

                        doc1.Add(table7);

                        doc1.Close();

                    }
                }
                //var doc1 = new iTextSharp.text.Document(PageSize.A4, 20, 20, 130, 90);
                if (lang == "G")
                {
                    for (int i = 0; i < data1.Count; i++)
                    {

                        //var doc1 = new iTextSharp.text.Document(PageSize.A4, 20, 20, 130, 90);
                        string subPath = "~/Purchase/PDFList/PO/" + data1[i].PO_Code + "/";
                        bool exists = System.IO.Directory.Exists(HttpContext.Server.MapPath(subPath));
                        if (!exists)
                            System.IO.Directory.CreateDirectory(HttpContext.Server.MapPath(subPath));
                        var output = new FileStream(Server.MapPath(subPath + data1[i].PO_Code + ".pdf"), FileMode.Create);
                        var writer = PdfWriter.GetInstance(doc1, output);
                        writer.PageEvent = new pdffooterclass();
                        //PdfWriter.GetInstance(doc1, new FileStream(Request.PhysicalApplicationPath + "\\Invoice_Statement.pdf", FileMode.Create));
                        doc1.Open();
                        path = output.Name;
                        //font size change from default  added by gv on 12/12/18
                        FontFactory.RegisterDirectories();
                        Font font = new Font(FontFactory.GetFont("Arial", 10, Font.NORMAL));
                        Font fontsmall = new Font(FontFactory.GetFont("Arial", 9, Font.NORMAL));
                        Font fontsmall1 = new Font(FontFactory.GetFont("Arial", 10, Font.NORMAL | Font.UNDERLINE));
                        Font Fontbiggest = new Font(FontFactory.GetFont("Arial", 50, Font.BOLD, Color.BLUE));
                        Font Fontbigger = new Font(FontFactory.GetFont("Arial", 20, Font.BOLD));
                        Font Fontsmaller = new Font(FontFactory.GetFont("Arial", 10, Font.BOLD));
                        Font Fontsmaller1 = new Font(FontFactory.GetFont("Arial", 12, Font.BOLD));

                        PdfPTable table1 = new PdfPTable(1);
                        table1.DefaultCell.Border = 0;
                        table1.WidthPercentage = 100f;

                        PdfPCell Title = new PdfPCell();
                        Title.Border = 0;
                        Title.AddElement(new Paragraph(new Chunk("Eurotextiles GmbH & Co. KG, Mammolshainer Weg 14, 61462 Königstein", fontsmall1)));
                        Title.VerticalAlignment = 2;
                        Title.PaddingTop = 1.0f;
                        Title.PaddingBottom = 3.0f;
                        Title.PaddingLeft = 30.0f;
                        table1.AddCell(Title);
                        Paragraph pg2 = new Paragraph();
                        Phrase phraseConstant2 = new Phrase("" + data1[i].SUPPLIER_NAME + "\n", font);
                        Phrase phraseConstant3 = new Phrase("" + data1[i].Street + "\n", font);
                        Phrase phraseConstant4 = new Phrase("" + data1[i].CityState + "\n", font);
                        Phrase phraseConstant5 = new Phrase("" + data1[i].CountryZip + "\n", font);
                        pg2.Add(phraseConstant2);
                        pg2.Add(phraseConstant3);
                        pg2.Add(phraseConstant4);
                        pg2.Add(phraseConstant5);
                        PdfPCell cell21 = new PdfPCell(pg2);
                        cell21.HorizontalAlignment = 0;
                        cell21.PaddingTop = 1.0f;
                        cell21.PaddingLeft = 30.0f;
                        cell21.Border = 0;
                        table1.AddCell(cell21);
                        doc1.Add(table1);

                        PdfPTable table2 = new PdfPTable(3);
                        table2.WidthPercentage = 100f;
                        float[] widthsvalforcus = new float[] { 55f,20f,25f };
                        table2.TotalWidth = 100f;
                        table2.WidthPercentage = 100f;
                        table2.SetWidths(widthsvalforcus);

                        Phrase emp1 = new Phrase("", font);
                        PdfPCell cell26emp = new PdfPCell(emp1);
                        cell26emp.Border = 0;
                        cell26emp.HorizontalAlignment = 2;
                        cell26emp.PaddingBottom = 2.5f;
                        table2.AddCell(cell26emp);
                        Phrase cuscode = new Phrase("Kunde Nr:", font);
                        PdfPCell cell26 = new PdfPCell(cuscode);
                        cell26.Border = 0;
                        cell26.HorizontalAlignment = 0;
                        cell26.PaddingBottom = 2.5f;
                        table2.AddCell(cell26);
                        Phrase cuscodeval = new Phrase("" + data1[i].PO_CutomerID+ "", font);
                        PdfPCell cell26val = new PdfPCell(cuscodeval);
                        cell26val.Border = 0;
                        cell26val.HorizontalAlignment = 2;
                        cell26val.PaddingBottom = 2.5f;
                        table2.AddCell(cell26val);

                        Phrase emp2 = new Phrase("", font);
                        PdfPCell cell26emp1 = new PdfPCell(emp2);
                        cell26emp1.Border = 0;
                        cell26emp1.HorizontalAlignment = 2;
                        cell26emp1.PaddingBottom = 2.5f;
                        table2.AddCell(cell26emp1);
                        Phrase Processby = new Phrase("Bearbeitet Von:", font);
                        PdfPCell cell26Processby = new PdfPCell(Processby);
                        cell26Processby.Border = 0;
                        cell26Processby.HorizontalAlignment = 0;
                        cell26Processby.PaddingBottom = 2.5f;
                        table2.AddCell(cell26Processby);
                        Phrase cuscodeval1 = new Phrase("" + data1[i].salesPersonName + "", font);
                        PdfPCell cell26Processby1 = new PdfPCell(cuscodeval1);
                        cell26Processby1.Border = 0;
                        cell26Processby1.HorizontalAlignment = 2;
                        cell26Processby1.PaddingBottom = 2.5f;
                        table2.AddCell(cell26Processby1);

                        Phrase emp3 = new Phrase("", font);
                        PdfPCell cell26emp2 = new PdfPCell(emp3);
                        cell26emp2.Border = 0;
                        cell26emp2.HorizontalAlignment = 0;
                        cell26emp2.PaddingBottom = 2.5f;
                        table2.AddCell(cell26emp2);
                        Phrase Datecell = new Phrase("Datum:", font);
                        PdfPCell cell26Datecell = new PdfPCell(Datecell);
                        cell26Datecell.Border = 0;
                        cell26Datecell.HorizontalAlignment = 0;
                        cell26Datecell.PaddingBottom = 2.5f;
                        table2.AddCell(cell26Datecell);
                        Phrase cuscodeval2 = new Phrase("" + data1[i].PO_Date.ToString("dd-MM-yyyy") + "", font);
                        PdfPCell cell26Processby2 = new PdfPCell(cuscodeval2);
                        cell26Processby2.Border = 0;
                        cell26Processby2.HorizontalAlignment = 2;
                        cell26Processby2.PaddingBottom = 2.5f;
                        table2.AddCell(cell26Processby2);

                        Phrase emp4 = new Phrase("", font);
                        PdfPCell cell26emp3 = new PdfPCell(emp4);
                        cell26emp3.Border = 0;
                        cell26emp3.HorizontalAlignment = 2;
                        cell26emp3.PaddingBottom = 2.5f;
                        table2.AddCell(cell26emp3);

                        //Phrase Datecell1 = new Phrase("Bestätigung Nein:", font);
                        //PdfPCell cell26Datecell1 = new PdfPCell(Datecell1);
                        //cell26Datecell1.Border = 0;
                        //cell26Datecell1.HorizontalAlignment = 0;
                        //cell26Datecell1.PaddingBottom = 2.5f;
                        //table2.AddCell(cell26Datecell1);
                        //Phrase cuscodeval21 = new Phrase("" + data1[i].PO_CusPONO + "", font);
                        //PdfPCell cell26Processby21 = new PdfPCell(cuscodeval21);
                        //cell26Processby21.Border = 0;
                        //cell26Processby21.HorizontalAlignment = 0;
                        //cell26Processby21.PaddingBottom = 2.5f;
                        //table2.AddCell(cell26Processby21);

                        //Phrase emp5 = new Phrase("", font);
                        //PdfPCell cell26emp4 = new PdfPCell(emp5);
                        //cell26emp4.Border = 0;
                        //cell26emp4.HorizontalAlignment = 2;
                        //cell26emp4.PaddingBottom = 2.5f;
                        //table2.AddCell(cell26emp4);

                        Phrase Datecell12 = new Phrase("Kundenbestellung Nr:", font);
                        PdfPCell cell26Datecell12 = new PdfPCell(Datecell12);
                        cell26Datecell12.Border = 0;
                        cell26Datecell12.HorizontalAlignment = 0;
                        cell26Datecell12.PaddingBottom = 2.5f;
                        table2.AddCell(cell26Datecell12);
                        Phrase cuscodeval211 = new Phrase("" + data1[i].PO_Code + "", font);
                        PdfPCell cell26Processby23 = new PdfPCell(cuscodeval211);
                        cell26Processby23.Border = 0;
                        cell26Processby23.HorizontalAlignment = 2;
                        cell26Processby23.PaddingBottom = 2.5f;
                        table2.AddCell(cell26Processby23);

                        Phrase emp6 = new Phrase("", font);
                        PdfPCell cell26emp6 = new PdfPCell(emp6);
                        cell26emp6.Border = 0;
                        cell26emp6.HorizontalAlignment = 2;
                        cell26emp6.PaddingBottom = 2.5f;
                        table2.AddCell(cell26emp6);

                        Phrase scCodePhrase = new Phrase("Kaufvertrag Nr: ", font);
                        PdfPCell cellSC26 = new PdfPCell(scCodePhrase);
                        cellSC26.Border = 0;
                        cellSC26.HorizontalAlignment = 0;
                        cellSC26.PaddingBottom = 2.5f;
                        table2.AddCell(cellSC26);

                        Phrase scCodeValue = new Phrase("" + data1[i].PO_SCNo + "", font);
                        PdfPCell cellSCCodeVal = new PdfPCell(scCodeValue);
                        cellSCCodeVal.Border = 0;
                        cellSCCodeVal.HorizontalAlignment = 2;
                        cellSCCodeVal.PaddingBottom = 2.5f;
                        table2.AddCell(cellSCCodeVal);

                        Phrase emp7 = new Phrase("", font);
                        PdfPCell cell26emp7 = new PdfPCell(emp7);
                        cell26emp7.Border = 0;
                        cell26emp7.HorizontalAlignment = 2;
                        cell26emp7.PaddingBottom = 2.5f;
                        table2.AddCell(cell26emp7);

                        Phrase scDatePhrase = new Phrase("Kaufvertragsdatum: ", font);
                        PdfPCell cellSCDate26 = new PdfPCell(scDatePhrase);
                        cellSCDate26.Border = 0;
                        cellSCDate26.HorizontalAlignment = 0;
                        cellSCDate26.PaddingBottom = 2.5f;
                        table2.AddCell(cellSCDate26);

                        Phrase scDateValue = new Phrase("" + data1[i].PO_SCDate.ToString("dd-MM-yyyy") + "", font);
                        PdfPCell cellSCDateVal = new PdfPCell(scDateValue);
                        cellSCDateVal.Border = 0;
                        cellSCDateVal.HorizontalAlignment = 2;
                        cellSCDateVal.PaddingBottom = 2.5f;
                        table2.AddCell(cellSCDateVal);
                        doc1.Add(table2);

                        PdfPTable contenttable = new PdfPTable(1);
                        contenttable.TotalWidth = 100f;
                        contenttable.WidthPercentage = 100f;
                        Paragraph pp1con = new Paragraph();
                        Phrase phar1con = new Phrase("Auftragsbestätigung", Fontsmaller1);
                        pp1con.Add(phar1con);
                        PdfPCell cell29Con = new PdfPCell(pp1con);
                        //cell29Con.AddElement(new Paragraph(new Chunk("" , Fontsmaller)));
                        cell29Con.PaddingTop = 4.5f;
                        cell29Con.HorizontalAlignment = 1;
                        cell29Con.PaddingBottom = 5.5f;
                        cell29Con.Border = 0;
                        contenttable.AddCell(cell29Con);
                        //PdfPCell cell29 = new PdfPCell();
                        //cell29.AddElement(new Paragraph(new Chunk("Bestätigung Nein:" + data1[i].PO_Code + "", Fontsmaller)));
                        //cell29.PaddingTop = 6.5f;
                        //cell29.PaddingBottom = 20.5f;
                        //cell29.Border = 0;
                        //contenttable.AddCell(cell29);
                        doc1.Add(contenttable);
                        PdfPTable table4 = new PdfPTable(6);
                        float[] widths1 = new float[] { 5f, 10f, 5f, 50f,15f, 15f};
                        table4.TotalWidth = 100f;
                        table4.WidthPercentage = 100f;
                        table4.HeaderRows = 1;
                        table4.SetWidths(widths1);

                        Phrase phraseConstantde1 = new Phrase("Pos", Fontsmaller);
                        PdfPCell cell41 = new PdfPCell(phraseConstantde1);

                        // cell41.AddElement(new Paragraph(new Chunk("Item", Fontsmaller)));

                        cell41.HorizontalAlignment = 1;
                        cell41.PaddingTop = 2.5f;
                        cell41.PaddingBottom = 2.5f;

                        table4.AddCell(cell41);
                        Phrase phraseConstantde2 = new Phrase("Menge", Fontsmaller);
                        PdfPCell cell42 = new PdfPCell(phraseConstantde2);
                        cell42.Colspan = 2;
                        //cell42.AddElement(new Paragraph(new Chunk("Qty", Fontsmaller)));
                        cell42.HorizontalAlignment = 1;
                        cell42.PaddingTop = 2.5f;
                        cell42.PaddingBottom = 2.5f;

                        table4.AddCell(cell42);
                        Phrase phraseConstantde3 = new Phrase("Produktbeschreibung", Fontsmaller);
                        PdfPCell cell421 = new PdfPCell(phraseConstantde3);
                        cell421.HorizontalAlignment = 1;
                        //cell421.AddElement(new Paragraph(new Chunk("UOM", Fontsmaller)));
                        cell421.PaddingTop = 2.5f;
                        cell421.PaddingBottom = 2.5f;
                        table4.AddCell(cell421);

                        //Phrase phraseConstantdeDesign = new Phrase("Designen", Fontsmaller);
                        //PdfPCell cellDesignHeader = new PdfPCell(phraseConstantdeDesign);
                        //cellDesignHeader.HorizontalAlignment = 1;
                        ////cell421.AddElement(new Paragraph(new Chunk("UOM", Fontsmaller)));
                        //cellDesignHeader.PaddingTop = 2.5f;
                        //cellDesignHeader.PaddingBottom = 2.5f;
                        //table4.AddCell(cellDesignHeader);

                        //Phrase phraseConstantdeRemarks = new Phrase("Anmerkungen", Fontsmaller);
                        //PdfPCell cellRemarksHeader = new PdfPCell(phraseConstantdeRemarks);
                        //cellRemarksHeader.HorizontalAlignment = 1;
                        ////cell421.AddElement(new Paragraph(new Chunk("UOM", Fontsmaller)));
                        //cellRemarksHeader.PaddingTop = 2.5f;
                        //cellRemarksHeader.PaddingBottom = 2.5f;
                        //table4.AddCell(cellRemarksHeader);

                        Phrase phraseConstantde4 = new Phrase("Einzelpreis(€)", Fontsmaller);
                        PdfPCell cell431 = new PdfPCell(phraseConstantde4);
                        cell431.HorizontalAlignment = 1;
                        // cell431.Colspan = 2;
                        //cell431.AddElement(new Paragraph(new Chunk("Product Name", Fontsmaller)));
                        cell431.PaddingTop = 2.5f;
                        cell431.PaddingBottom = 2.5f;
                        table4.AddCell(cell431);
                      
                        Phrase phraseConstantde5 = new Phrase("Gesamtpreis(€)", Fontsmaller);
                        PdfPCell cell43 = new PdfPCell(phraseConstantde5);
                        cell43.HorizontalAlignment = 1;
                        // cell43.AddElement(new Paragraph(new Chunk("Description", Fontsmaller)));
                        cell43.PaddingTop = 2.5f;
                        cell43.PaddingBottom = 2.5f;
                        table4.AddCell(cell43);

                        decimal? total = 0;
                        for (int j = 0; j < data2.Count; j++)
                        {
                            Paragraph Snopp = new Paragraph();
                            Phrase Snophar = new Phrase("" + (j + 1) + "\n", font);
                            Snopp.Add(Snophar);
                            PdfPCell cell51 = new PdfPCell(Snopp);
                            cell51.HorizontalAlignment = 1;
                            //cell51.AddElement(new Paragraph(new Chunk("" + (j + 1) + "", font)));

                            // cell51.PaddingTop = 2.5f;
                            // cell51.PaddingBottom = 2.5f;

                            table4.AddCell(cell51);
                            Paragraph qtypp = new Paragraph();
                            Phrase qtyphar = new Phrase("" + Convert.ToDecimal(data2[j].PD_Quantity).ToString("N2", CultureInfo.CreateSpecificCulture("de-DE")) + "\n", font);
                            qtypp.Add(qtyphar);
                            PdfPCell cell52 = new PdfPCell(qtypp);
                            cell52.HorizontalAlignment = 2;
                            //  cell52.PaddingTop = 2.5f;
                            // cell52.PaddingBottom = 2.5f;

                            table4.AddCell(cell52);
                            //German Text for UOM
                            string uomText = data2[j].PD_UOM;
                            if (data2[j].PD_UOMCode == "1")
                                uomText = "Kg";
                            else if (data2[j].PD_UOMCode == "2")
                                uomText = "Mtrs";
                            else if (data2[j].PD_UOMCode == "3")
                                uomText = "Stk";
                            Paragraph uompp = new Paragraph();
                            Phrase uomphar = new Phrase("" + uomText + "\n", font);
                            uompp.Add(uomphar);
                            PdfPCell uomcell = new PdfPCell(uompp);

                            // uomcell.AddElement(new Paragraph(new Chunk("" + data2[j].UOM_NAME + "", font)));
                            uomcell.HorizontalAlignment = 0;
                            // uomcell.PaddingTop = 2.5f;
                            // uomcell.PaddingBottom = 2.5f;

                            table4.AddCell(uomcell);

                            if (data1[i].PO_OrderType == 2)
                            {
                                Paragraph Productpp = new Paragraph();
                                Phrase Productphar = new Phrase("" + data2[j].PD_ShortName + "\n", font);
                                Phrase Productphar1 = new Phrase("" + data2[j].DesignDetail + "\n", font);
                                Phrase Productphar3 = new Phrase("" + data2[j].Description + "\n", font);
                                Phrase Productphar2 = new Phrase("" + data2[j].PO_Remarks + "\n", font);
                                Productpp.Add(Productphar);

                                Productpp.Add(Productphar3);
                                Productpp.Add(Productphar2);
                                Productpp.Add(Productphar1);
                                PdfPCell cell53 = new PdfPCell(Productpp);
                                // cell53.AddElement(new Paragraph(new Chunk("" + data2[j].PRODUCT_Name + "\n", font)));
                                // cell53.AddElement(new Paragraph(new Chunk("" + data2[j].DesignDetail + "\n", font)));
                                cell53.HorizontalAlignment = 0;
                                cell53.PaddingTop = 0f;
                                // cell53.PaddingBottom = 2.5f;

                                table4.AddCell(cell53);
                                //Paragraph ProductDesignPara = new Paragraph();
                                //Phrase ProductDesign = new Phrase("" + data2[j].DesignDetail + "\n", Fontsmaller);
                                //ProductDesignPara.Add(ProductDesign);

                                //PdfPCell cellDesign = new PdfPCell(ProductDesignPara);
                                //cellDesign.HorizontalAlignment = 0;
                                //cellDesign.PaddingTop = 0f;
                                //table4.AddCell(cellDesign);

                                //Paragraph ProductRemarksPara = new Paragraph();
                                //Phrase ProductRemarks = new Phrase("" + data2[j].PD_SuppDef + "\n", Fontsmaller);
                                //ProductRemarksPara.Add(ProductRemarks);
                                //PdfPCell cellRemarks = new PdfPCell(ProductRemarksPara);
                                //cellRemarks.HorizontalAlignment = 0;
                                //cellRemarks.PaddingTop = 0f;
                                //table4.AddCell(cellRemarks);
                            }
                            else
                            {
                                Paragraph Productpp = new Paragraph();
                                Phrase Productphar = new Phrase("" + data2[j].PD_ShortName + "\n", font);
                                Phrase Productphar3 = new Phrase("" + data2[j].Description + "\n", font);
                                Phrase Productphar2 = new Phrase("" + data2[j].PO_Remarks + "\n", font);
                                Productpp.Add(Productphar);
                                Productpp.Add(Productphar3);
                                Productpp.Add(Productphar2);
                                PdfPCell cell53 = new PdfPCell(Productpp);
                                // cell53.AddElement(new Paragraph(new Chunk("" + data2[j].PRODUCT_Name + "", font)));
                                cell53.HorizontalAlignment = 0;
                                cell53.PaddingTop = 0f;
                                //cell53.PaddingTop = 2.5f;
                                // cell53.PaddingBottom = 2.5f;
                                table4.AddCell(cell53);
                            }

                            if (data1[i].PO_OrderType == 2 || data1[i].PO_OrderType == 3)
                            {
                                Paragraph pp = new Paragraph();
                                Phrase phar = new Phrase("" + Convert.ToDecimal(data2[j].PD_UnitPrice).ToString("c2", CultureInfo.CreateSpecificCulture("de-DE")) + "\n", font);
                                pp.Add(phar);
                                PdfPCell cell541 = new PdfPCell(pp);
                                //cell541.AddElement(pp);
                                // cell541.Colspan = 2;
                                cell541.HorizontalAlignment = 2;
                                cell541.PaddingTop = 2.5f;
                                cell541.PaddingBottom = 2.5f;

                                table4.AddCell(cell541);
                                decimal? DisPer = 0;
                                //if (data2[j].DiscountPer == 0)
                                //{
                                //    DisPer = 0;
                                //}
                                //else
                                //{
                                //    DisPer = data2[j].DiscountPer;
                                //}
                                //Paragraph ppd = new Paragraph();
                                //Phrase phard = new Phrase("" + DisPer + "\n", font);
                                //ppd.Add(phard);
                                //PdfPCell cell541d = new PdfPCell(ppd);
                                ////cell541.AddElement(pp);
                                ////cell541.Colspan = 2;
                                //cell541d.HorizontalAlignment = 2;
                                //cell541d.PaddingTop = 2.5f;
                                //cell541d.PaddingBottom = 2.5f;

                                //table4.AddCell(cell541d);
                                decimal discountAmt = 0;
                                //decimal discountAmt = ((Convert.ToDecimal(data2[j].PD_UnitPrice) * Convert.ToDecimal(data2[j].PD_Quantity)) * Convert.ToDecimal(DisPer) / 100);
                                Paragraph pp12 = new Paragraph();
                                Phrase phar12 = new Phrase("" + ((Convert.ToDecimal(data2[j].PD_UnitPrice) * Convert.ToDecimal(data2[j].PD_Quantity)) - discountAmt).ToString("c2", CultureInfo.CreateSpecificCulture("de-DE")) + "\n", font);
                                pp12.Add(phar12);
                                PdfPCell cell54 = new PdfPCell(pp12);
                                cell54.HorizontalAlignment = 2;
                                cell54.PaddingTop = 2.5f;
                                cell54.PaddingBottom = 2.5f;

                                table4.AddCell(cell54);
                                total = total + ((Convert.ToDecimal(data2[j].PD_UnitPrice) * Convert.ToDecimal(data2[j].PD_Quantity)) - discountAmt);
                            }
                            else
                            {
                                Paragraph pp = new Paragraph();
                                Phrase phar = new Phrase("" + Convert.ToDecimal(data2[j].PD_UnitPrice).ToString("c2", CultureInfo.CreateSpecificCulture("de-DE")) + "\n", font);
                                pp.Add(phar);
                                PdfPCell cell541 = new PdfPCell(pp);
                                //cell541.AddElement(pp);
                                //cell541.Colspan = 2;
                                cell541.HorizontalAlignment = 2;
                                cell541.PaddingTop = 2.5f;
                                cell541.PaddingBottom = 2.5f;
                                table4.AddCell(cell541);

                                Paragraph pp12 = new Paragraph();
                                Phrase phar12 = new Phrase("" + (Convert.ToDecimal(data2[j].PD_UnitPrice) * Convert.ToDecimal(data2[j].PD_Quantity)).ToString("c2", CultureInfo.CreateSpecificCulture("de-DE")) + "\n", font);
                                pp12.Add(phar12);
                                PdfPCell cell54 = new PdfPCell(pp12);
                                cell54.HorizontalAlignment = 2;
                                cell54.PaddingTop = 2.5f;
                                cell54.PaddingBottom = 2.5f;
                                table4.AddCell(cell54);

                                total = total + (Convert.ToDecimal(data2[j].PD_UnitPrice) * Convert.ToDecimal(data2[j].PD_Quantity));
                            }
                        }

                        //PdfPCell NetValCell = new PdfPCell();

                        //NetValCell.AddElement(new Paragraph(new Chunk("Gesamt Netto", font)));
                        //NetValCell.Colspan = 8;
                        //NetValCell.PaddingTop = 2.5f;
                        //NetValCell.PaddingBottom = 2.5f;

                        //table4.AddCell(NetValCell);
                        //Paragraph pp1 = new Paragraph();
                        //Phrase phar1 = new Phrase("" + Convert.ToDecimal(total).ToString("c2", CultureInfo.CreateSpecificCulture("de-DE")) + "\n", font);
                        //pp1.Add(phar1);
                        //PdfPCell NetValCell1 = new PdfPCell(pp1);
                        //NetValCell1.Colspan = 8;
                        //NetValCell1.HorizontalAlignment = 2;
                        //NetValCell1.VerticalAlignment = 2;
                        //NetValCell1.PaddingTop = 2.5f;
                        //NetValCell1.PaddingBottom = 2.5f;

                        //table4.AddCell(NetValCell1);

                        //if (data1[i].Zipcode == 82)
                        //{
                        //    PdfPCell inccell = new PdfPCell();

                        //    inccell.AddElement(new Paragraph(new Chunk(Convert.ToDecimal(data1[i].VatPer).ToString("0") + " % Value", font)));
                        //    inccell.Colspan = 8;
                        //    inccell.PaddingTop = 2.5f;
                        //    inccell.PaddingBottom = 2.5f;

                        //    table4.AddCell(inccell);
                        //    Paragraph pp1inc = new Paragraph();
                        //    Phrase phar1inc = new Phrase("" + (Convert.ToDecimal(total * data1[i].VatPer / 100).ToString("C2", CultureInfo.CreateSpecificCulture("de-DE"))) + "\n", font);
                        //    pp1inc.Add(phar1inc);
                        //    PdfPCell inccell1 = new PdfPCell(pp1inc);
                        //    inccell1.HorizontalAlignment = 2;
                        //    inccell1.VerticalAlignment = 2;
                        //    inccell1.PaddingTop = 2.5f;
                        //    inccell1.PaddingBottom = 2.5f;

                        //    table4.AddCell(inccell1);
                        //}
                        //else
                        //{

                        //    PdfPCell inccell = new PdfPCell();
                        //    inccell.AddElement(new Paragraph(new Chunk("0 % Vat", font)));
                        //    inccell.Colspan = 8;
                        //    inccell.PaddingTop = 2.5f;
                        //    inccell.PaddingBottom = 2.5f;

                        //    table4.AddCell(inccell);
                        //    Paragraph pp1inc = new Paragraph();
                        //    Phrase phar1inc = new Phrase("" + (Convert.ToDecimal(total * 0 / 100).ToString("C2", CultureInfo.CreateSpecificCulture("de-DE"))) + "\n", font);
                        //    pp1inc.Add(phar1inc);
                        //    PdfPCell inccell1 = new PdfPCell(pp1inc);
                        //    inccell1.HorizontalAlignment = 2;
                        //    inccell1.PaddingTop = 2.5f;
                        //    inccell1.PaddingBottom = 2.5f;

                        //    table4.AddCell(inccell1);

                        //}
                        decimal Discountamt = 0;

                        if (data1[i].Zipcode == 82)
                        {

                            //var Vatamount = (Convert.ToDecimal(total + ((total * data1[i].VatPer / 100))));
                            //Vatamount = (total * data1[i].VatPer / 100) ?? 0;


                            //if (data1[i].Discount != 0)
                            //{
                            //    Discountamt = (Convert.ToDecimal(total * data1[i].Discount / 100));
                            //    PdfPCell AllDiscountcell = new PdfPCell();

                            //    AllDiscountcell.AddElement(new Paragraph(new Chunk("Rabatt(%)", font)));
                            //    AllDiscountcell.Colspan = 8;
                            //    AllDiscountcell.PaddingTop = 2.5f;
                            //    AllDiscountcell.PaddingBottom = 2.5f;

                            //    table4.AddCell(AllDiscountcell);

                            //    Paragraph pp1discount = new Paragraph();
                            //    Phrase phar1Discount = new Phrase("" + Discountamt.ToString("C2", CultureInfo.CreateSpecificCulture("de-DE")) + "\n", Fontsmaller);
                            //    pp1discount.Add(phar1Discount);
                            //    PdfPCell AllDiscountcellde = new PdfPCell(pp1discount);
                            //    AllDiscountcellde.HorizontalAlignment = 2;
                            //    AllDiscountcellde.Colspan = 8;
                            //    AllDiscountcellde.PaddingTop = 2.5f;
                            //    AllDiscountcellde.PaddingBottom = 2.5f;
                            //    AllDiscountcellde.HorizontalAlignment = 2;
                            //    table4.AddCell(AllDiscountcellde);
                            //}
                            PdfPCell vataddcell = new PdfPCell();
                            vataddcell.AddElement(new Paragraph(new Chunk("Gesamt", font)));
                            vataddcell.Colspan = 5;
                            vataddcell.PaddingTop = 2.5f;
                            vataddcell.PaddingBottom = 2.5f;
                            table4.AddCell(vataddcell);

                            Paragraph pp1vat = new Paragraph();
                            Phrase phar1vat = new Phrase("" + ((Convert.ToDecimal(total - Discountamt)).ToString("C2", CultureInfo.CreateSpecificCulture("de-DE"))) + "\n", Fontsmaller);
                            pp1vat.Add(phar1vat);
                            PdfPCell vataddcell1 = new PdfPCell(pp1vat);
                            vataddcell1.HorizontalAlignment = 2;
                            //vataddcell1.Colspan = 5;
                            vataddcell1.PaddingTop = 2.5f;
                            vataddcell1.PaddingBottom = 2.5f;
                            table4.AddCell(vataddcell1);
                        }
                        else
                        {
                            //var Vatamount = (Convert.ToDecimal(total + ((total * 0 / 100))));
                            //Vatamount = (total * 0 / 100) ?? 0;

                            //if (data1[i].Discount != 0)
                            //{
                            //    Discountamt = (Convert.ToDecimal(total * data1[i].Discount / 100));
                            //    PdfPCell AllDiscountcell = new PdfPCell();

                            //    AllDiscountcell.AddElement(new Paragraph(new Chunk("Rabatt(%)", font)));
                            //    AllDiscountcell.Colspan = 8;
                            //    AllDiscountcell.PaddingTop = 2.5f;
                            //    AllDiscountcell.PaddingBottom = 2.5f;

                            //    table4.AddCell(AllDiscountcell);
                            //    Paragraph pp1discount = new Paragraph();
                            //    Phrase phar1Discount = new Phrase("" + Discountamt.ToString("C2", CultureInfo.CreateSpecificCulture("de-DE")) + "\n", Fontsmaller);
                            //    pp1discount.Add(phar1Discount);
                            //    PdfPCell AllDiscountcellde = new PdfPCell(pp1discount);
                            //    AllDiscountcellde.Colspan = 8;
                            //    AllDiscountcellde.PaddingTop = 2.5f;
                            //    AllDiscountcellde.PaddingBottom = 2.5f;
                            //    AllDiscountcellde.HorizontalAlignment = 2;
                            //    table4.AddCell(AllDiscountcellde);
                            //}
                            //PdfPCell vataddcell = new PdfPCell();

                            //vataddcell.AddElement(new Paragraph(new Chunk("Einzelpreis(€)", font)));
                            //vataddcell.Colspan = 6;
                            //vataddcell.PaddingTop = 2.5f;
                            //vataddcell.PaddingBottom = 2.5f;

                            //table4.AddCell(vataddcell);
                            //Paragraph pp1vat = new Paragraph();
                            //Phrase phar1vat = new Phrase("" + ((Convert.ToDecimal(total + Vatamount - Discountamt)).ToString("C2", CultureInfo.CreateSpecificCulture("de-DE"))) + "\n", Fontsmaller);
                            //pp1vat.Add(phar1vat);
                            //PdfPCell vataddcell1 = new PdfPCell(pp1vat);
                            //vataddcell1.HorizontalAlignment = 2;
                            //vataddcell1.Colspan = 8;
                            //vataddcell1.PaddingTop = 2.5f;
                            //vataddcell1.PaddingBottom = 2.5f;

                            //table4.AddCell(vataddcell1);
                            PdfPCell vataddcell = new PdfPCell();

                            vataddcell.AddElement(new Paragraph(new Chunk("Gesamt", font)));
                            vataddcell.Colspan = 5;
                            vataddcell.PaddingTop = 2.5f;
                            vataddcell.PaddingBottom = 2.5f;

                            table4.AddCell(vataddcell);
                            Paragraph pp1vat = new Paragraph();
                            Phrase phar1vat = new Phrase("" + ((Convert.ToDecimal(total - Discountamt)).ToString("C2", CultureInfo.CreateSpecificCulture("de-DE"))) + "\n", Fontsmaller);
                            pp1vat.Add(phar1vat);
                            PdfPCell vataddcell1 = new PdfPCell(pp1vat);
                            vataddcell1.HorizontalAlignment = 2;
                            //vataddcell1.Colspan = 5;
                            vataddcell1.PaddingTop = 2.5f;
                            vataddcell1.PaddingBottom = 2.5f;

                            table4.AddCell(vataddcell1);
                        }
                        doc1.Add(table4);

                        PdfPTable table6 = new PdfPTable(1);
                        PdfPCell cell31 = new PdfPCell();
                        cell31.Border = 0;
                        cell31.AddElement(new Paragraph(new Chunk("", font)));
                        cell31.PaddingTop = 20.5f;
                        cell31.PaddingBottom = 2.5f;
                        table6.AddCell(cell31);

                        PdfPCell cell39 = new PdfPCell();
                        cell39.Border = 0;
                        cell39.AddElement(new Paragraph(""));
                        //cell39.PaddingTop = 280.0f;
                        cell39.PaddingBottom = 2.5f;
                        table6.AddCell(cell39);
                        doc1.Add(table6);

                        PdfPTable table7 = new PdfPTable(1);
                        table7.WidthPercentage = 100f;
                        //string[] splitedRemarks = data1[i].PO_Remarks.Split('.');
                        //for (int k = 0; k < splitedRemarks.Count(); k++)
                        //{
                        //    if (k == 0)
                        //    {
                        //        PdfPCell cell33re = new PdfPCell();
                        //        cell33re.AddElement(new Paragraph(new Chunk("Anmerkungen:" + splitedRemarks[k], font)));
                        //        cell33re.PaddingTop = 2.5f;
                        //        cell33re.PaddingBottom = 2.5f;
                        //        cell33re.Border = 0;
                        //        table7.AddCell(cell33re);
                        //    }
                        //    else
                        //    {
                        //        if (splitedRemarks[k] != "")
                        //        {
                        //            PdfPCell cell33re = new PdfPCell();
                        //            cell33re.AddElement(new Paragraph(new Chunk(splitedRemarks[k], font)));
                        //            cell33re.PaddingTop = 2.5f;
                        //            cell33re.PaddingBottom = 2.5f;
                        //            cell33re.Border = 0;
                        //            table7.AddCell(cell33re);
                        //        }

                        //    }

                        //}
                        //PdfPCell cell33re = new PdfPCell();

                        //cell33re.AddElement(new Paragraph(new Chunk(data1[i].SO_Remarks, font)));

                        //cell33re.PaddingTop = 2.5f;
                        //cell33re.PaddingBottom = 2.5f;
                        //cell33re.Border = 0;
                        //table7.AddCell(cell33re);
                        if (data1[i].PaymentTermsDescription != string.Empty)
                        {
                            PdfPCell cell33 = new PdfPCell();
                            cell33.AddElement(new Paragraph(new Chunk("Zahlungsbedingungen: " + data1[i].PaymentTermsDescription + "", font)));
                            cell33.PaddingTop = 2.5f;
                            cell33.PaddingBottom = 2.5f;
                            cell33.Border = 0;
                            table7.AddCell(cell33);
                        }
                        //PdfPCell delitercell = new PdfPCell();
                        //delitercell.AddElement(new Paragrnaph(new Chunk("Delivery Terms: " + data1[i].SO_CusDeliveryTerms + "", font)));
                        //delitercell.PaddingTop = 2.5f;
                        //delitercell.PaddingBottom = 2.5f;
                        //delitercell.Border = 0;
                        //table7.AddCell(delitercell);
                        if (data1[i].Po_ShippingAddress != string.Empty)
                        {
                            string[] splitedDeliveryTerms = data1[i].Po_ShippingAddress.Split('.');
                            for (int k = 0; k < splitedDeliveryTerms.Count(); k++)
                            {
                                if (k == 0)
                                {
                                    PdfPCell cell33re = new PdfPCell();
                                    cell33re.AddElement(new Paragraph(new Chunk("Lieferbedingungen:" + splitedDeliveryTerms[k], font)));
                                    cell33re.PaddingTop = 2.5f;
                                    cell33re.PaddingBottom = 2.5f;
                                    cell33re.Border = 0;
                                    table7.AddCell(cell33re);
                                }
                                else
                                {
                                    if (splitedDeliveryTerms[k] != "")
                                    {
                                        PdfPCell cell33re = new PdfPCell();
                                        cell33re.AddElement(new Paragraph(new Chunk(splitedDeliveryTerms[k], font)));
                                        cell33re.PaddingTop = 2.5f;
                                        cell33re.PaddingBottom = 2.5f;
                                        cell33re.Border = 0;
                                        table7.AddCell(cell33re);
                                    }
                                }
                            }
                        }

                        if (data1[i].Po_SpecialInstruction != string.Empty)
                        {
                            //PdfPCell specialCell = new PdfPCell();
                            //specialCell.AddElement(new Paragraph(new Chunk("Spezielle Anweisungen: " + data1[i].Po_SpecialInstruction + "", font)));
                            //specialCell.PaddingTop = 2.5f;
                            //specialCell.PaddingBottom = 2.5f;
                            //specialCell.Border = 0;
                            //table7.AddCell(specialCell);

                            string[] splittedSpecialInstruction = data1[i].Po_SpecialInstruction.Split('.');
                            for (int k = 0; k < splittedSpecialInstruction.Count(); k++)
                            {
                                if (k == 0)
                                {
                                    PdfPCell specialCell = new PdfPCell();
                                    specialCell.AddElement(new Paragraph(new Chunk("Spezielle Anweisungen: " + splittedSpecialInstruction[k] + "", font)));
                                    specialCell.PaddingTop = 2.5f;
                                    specialCell.PaddingBottom = 2.5f;
                                    specialCell.Border = 0;
                                    table7.AddCell(specialCell);

                                    //PdfPCell cell33re = new PdfPCell();
                                    //cell33re.AddElement(new Paragraph(new Chunk("Delivery Terms:" + splitedDeliveryTerms[k], font)));

                                    //cell33re.PaddingTop = 2.5f;
                                    //cell33re.PaddingBottom = 2.5f;
                                    //cell33re.Border = 0;
                                    //table7.AddCell(cell33re);
                                }
                                else
                                {
                                    if (splittedSpecialInstruction[k] != "")
                                    {
                                        PdfPCell cell33re = new PdfPCell();
                                        cell33re.AddElement(new Paragraph(new Chunk(splittedSpecialInstruction[k], font)));
                                        cell33re.PaddingTop = 2.5f;
                                        cell33re.PaddingBottom = 2.5f;
                                        cell33re.Border = 0;
                                        table7.AddCell(cell33re);
                                    }
                                }
                            }
                        }
                        if (data1[i].Po_TermsandConditions != string.Empty)
                        {
                            PdfPCell termsCell = new PdfPCell();
                            termsCell.AddElement(new Paragraph(new Chunk("Geschäftsbedingungen: " + data1[i].Po_TermsandConditions + "", font)));
                            termsCell.PaddingTop = 2.5f;
                            termsCell.PaddingBottom = 2.5f;
                            termsCell.Border = 0;
                            table7.AddCell(termsCell);
                        }
                        PdfPCell Linecell = new PdfPCell();
                        Linecell.AddElement(new Paragraph(new Chunk("___________________________", font)));
                        Linecell.PaddingTop = 25.5f;
                        Linecell.PaddingBottom = 0f;
                        Linecell.Border = 0;
                        table7.AddCell(Linecell);

                        PdfPCell signcell = new PdfPCell();
                        signcell.AddElement(new Paragraph(new Chunk("Eurotextiles GmbH & Co. KG", font)));
                        signcell.PaddingTop = 0f;
                        signcell.PaddingBottom = 2.5f;
                        signcell.Border = 0;
                        table7.AddCell(signcell);
                        PdfPCell line1 = new PdfPCell();

                        line1.AddElement(new Paragraph(new Chunk("Es gelten die Allgemeinen Geschäftsbedingungen (AGB) der Eurotextiles GmbH & Co. KG.", font)));
                        line1.PaddingTop = 6.5f;
                        line1.PaddingBottom = 2.5f;
                        line1.Border = 0;
                        table7.AddCell(line1);
                        PdfPCell line2 = new PdfPCell();

                        line2.AddElement(new Paragraph(new Chunk("Abweichungen von Qualität/Stückzahl der Ware müssen uns vor Benutzung, aber in jedem Fall innerhalb von 7 Tagen nach Erhalt der Ware, schriftlich mitgeteilt werden.", font)));
                        line2.PaddingTop = 2.5f;
                        line2.PaddingBottom = 2.5f;
                        line2.Border = 0;
                        table7.AddCell(line2);
                        PdfPCell line3 = new PdfPCell();

                        line3.AddElement(new Paragraph(new Chunk("Ware die bereits gewaschen oder benutzt wurde ist von der Reklamation ausgeschlossen.", font)));
                        line3.PaddingTop = 2.5f;
                        line3.PaddingBottom = 2.5f;
                        line3.Border = 0;
                        table7.AddCell(line3);

                        PdfPCell line4 = new PdfPCell();
                        line4.AddElement(new Paragraph(new Chunk("Datenschutz: Bitte beachten Sie, dass wir mit IT-Systemen zusammenarbeiten und Ihren Namen und andere relevante Daten im Rahmen der Angelegenheit gespeichert haben. Die Daten werden nicht an Dritte weitergegeben.", font)));
                        line4.PaddingTop = 2.5f;
                        line4.PaddingBottom = 2.5f;
                        line4.Border = 0;
                        table7.AddCell(line4);
                        doc1.Add(table7);

                        doc1.Close();
                    }
                }
                return Json(data1, JsonRequestBehavior.AllowGet);
                //return path;
            }
            catch (Exception exe)
            {
                transaction.Rollback();
                string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                objERR.err_title = controllerName + "-" + controllerName;
                objERR.err_message = "Sorry for the inconvience. Some error has been occured." + exe.Message;
                objERR.err_details = exe.Message.Replace("'", "");
                int errid = bal.ExceptionInsertLogs_BL(objERR);
                //close the doucment print
                doc1.Close();
                return Json("ERR" + errid.ToString(), JsonRequestBehavior.AllowGet);
                // return "ERR" + errid.ToString();
            }
            finally
            {
                transaction.Dispose();
                context.Dispose();
            }

        }
        //Get The po List Which are Deleted
        public ActionResult ET_Master_PO_RestoreDelete(int id, bool type,int type1)
        {
            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                {
                    int i;
                    tbl_PurchaseOrderHeader deleted = dbcontext.tbl_PurchaseOrderHeader.Single(m => m.PP_ID == id);
                    deleted.DELETED = type;
                    i = dbcontext.SaveChanges();
                    var json = "Failed";
                    if (i != 0)
                    {
                        json = "Success";
                    }
                    else
                    {
                        objLOG.log_dockey = "8017";
                        objLOG.log_operation = "Restore";
                        objLOG.log_userid = Session["UserID"].ToString();
                        objLOG.log_recordkey = id.ToString();
                        objLOG.log_Remarks = "Record Restored Successfully";
                        bal.OperationInsertLogs_BL(objLOG);
                    }
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
                return RedirectToAction("ET_SessionExpire", "ET_Login");
            }
        }

        /// <summary>
        /// Get Catalog List for the System.
        /// </summary>
        /// <param name="catalogId"></param>
        /// <returns></returns>
        public JsonResult GetCatlogList(int catalogId,int catalogType)
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
                        var CatalogList = new List<Tbl_ProductCatalog_Master>();
                        if (catalogId == 0)
                            CatalogList = dbcontext.Tbl_ProductCatalog_Master.Where(m => m.COM_KEY == companykey && m.DELETED == false && m.PC_CatalogType == catalogType).OrderBy(m => m.PC_Name).ToList();
                        else
                            CatalogList = dbcontext.Tbl_ProductCatalog_Master.Where(m => m.COM_KEY == companykey && m.DELETED == false && m.PC_MasterID == catalogId && m.PC_CatalogType == catalogType).OrderBy(m => m.PC_Name).ToList();
                        var json = new JavaScriptSerializer().Serialize(CatalogList);
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

        // Get the One PO details For Edit
        public ActionResult ET_Master_PO_Update_GetbyID(int id)
        {
            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                {
                    var data = dbcontext.tbl_PurchaseOrderHeader.Single(m => m.PP_ID == id);
                    var json = new JavaScriptSerializer().Serialize(data);
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
                return RedirectToAction("ET_SessionExpire", "ET_Login");
            }
        }
        //Get the PurchaseOrder Product Details
        public ActionResult ET_Master_PO_Details_Load(int id)
        {
            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                {
                    var data1 = (from c in dbcontext.tbl_PurchaseOrderDetails
                                 join head in dbcontext.tbl_PurchaseOrderHeader on c.PD_PID equals head.PP_ID
                                 join a in dbcontext.Tbl_Product_Master on c.PD_ProductID equals a.P_ID
                                 
                                 join b in dbcontext.tbl_LookUp on a.P_UOM equals b.LU_Code
                                 into m
                                 from n in m.DefaultIfEmpty()
                                 where c.PD_PID == id && n.LU_Type == 2
                                 select new PO_CM
                                 {
                                     PO_OrderDetailID = c.PO_OrderDetailID,
                                     PD_ProductID = a.P_ID,
                                     PD_ShortName = a.P_ShortName,
                                     PD_UOM = n.LU_Description,
                                     Description = a.P_Description,
                                     PD_Quantity = c.PD_Quantity,
                                     PD_UnitPrice = c.PD_UnitPrice,
                                    //DesignDetail = (dbcontext.Tbl_Order_Details.Where(a => a.AGEN_TRAD_PO_ID == head.PO_OrderReference).Select(a => a.DesignDetail).FirstOrDefault()),
                                     PD_TotalAmount = c.PD_TotalAmount,
                                     DesignDetail = c.DesignDetail,
                                     //PD_ID = c.PD_ID,
                                     //PD_PID = c.PD_PID,
                                     PD_SuppDef = c.SupplierDes
                                 }).Distinct().ToList();
                    List<PO_CM> modelItems = (List<PO_CM>)data1.Select((purchaseOrdItem, index) => new PO_CM() { SO_Serial = index + 1, PO_OrderDetailID = purchaseOrdItem.PO_OrderDetailID, PD_ProductID = purchaseOrdItem.PD_ProductID, PD_ShortName = purchaseOrdItem.PD_ShortName, PD_UOM = purchaseOrdItem.PD_UOM, PD_Quantity = purchaseOrdItem.PD_Quantity, PD_UnitPrice = purchaseOrdItem.PD_UnitPrice, Description = purchaseOrdItem.Description, DesignDetail = purchaseOrdItem.DesignDetail, PD_TotalAmount = purchaseOrdItem.PD_TotalAmount, PD_ID = purchaseOrdItem.PD_ID, PD_PID = purchaseOrdItem.PD_PID, PD_SuppDef = purchaseOrdItem.PD_SuppDef }).ToList();
                    var json = new JavaScriptSerializer().Serialize(modelItems);
                    return Json(json, JsonRequestBehavior.AllowGet);
                    //return PartialView("/Views/Purchase/ET_Purchase_PO/ET_Purchase_PO_Details.cshtml", data1);
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
                return RedirectToAction("ET_SessionExpire", "ET_Login");
            }
        }
        public ActionResult BindRow_Employees()
        {
            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                {
                     return PartialView("/Views/Purchase/ET_Purchase_PO/ET_Purchase_PO_Schedule.cshtml");
                }
                catch (Exception exe)
                {
                    //Insert Error Log
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
                //Session Expire
                return RedirectToAction("ET_SessionExpire", "ET_Login");
            }
        }

    }
    public class pdffooterclass : PdfPageEventHelper
    {
        PdfTemplate headerTemplate, footerTemplate;
        BaseFont bf = null;
        PdfContentByte cb;
        Color FontColour = new Color(35, 31, 32);
        Font Fontbiggest = new Font(FontFactory.GetFont("Arial", 50, Font.BOLD, new Color(180)));
        Font Fontbigger = new Font(FontFactory.GetFont("Arial", 20, Font.BOLD));
        Font Fontsmaller = new Font(FontFactory.GetFont("Arial", 10, Font.BOLD));
        //Image logo = iTextSharp.text.Image.GetInstance("E:/Projects/Indra/Images/euro_logowithText.png");
        string logoPath = string.Empty;
        string serverLogoPath = string.Empty;
        Image logo = null;
        float scaledWidth = 331f;
        float scaledHeight = 91f;

        public pdffooterclass()
        {
            logoPath = "~/Images/euro_logo2912019_10.png";
            serverLogoPath = System.Web.HttpContext.Current.Server.MapPath(logoPath);
            using (FileStream fs = new FileStream(serverLogoPath, FileMode.Open))
            {

                logo = Image.GetInstance(System.Drawing.Image.FromStream(fs), System.Drawing.Imaging.ImageFormat.Png);
            }
            logo.ScaleToFit(scaledWidth, scaledHeight);
        }
        public override void OnStartPage(PdfWriter writer, Document document)
        {
            base.OnStartPage(writer, document);
            //string logoPath = "~/Images/euro_logo2912019.png";
            //string serverLogoPath = System.Web.HttpContext.Current.Server.MapPath(logoPath);
            //string logoPath = Path.Combine(Server.MapPath("~/Images/euro_logowithText.png"));
            //Image logo = iTextSharp.text.Image.GetInstance(serverLogoPath);
            //logo.ScaleAbsolute(331f, 91f);
            PdfPCell imageCell = new PdfPCell(logo);
            imageCell.HorizontalAlignment = Element.ALIGN_CENTER;
            imageCell.Border = 0;
            imageCell.PaddingLeft = (document.PageSize.Width - scaledWidth) / 2;
            PdfPTable table0 = new PdfPTable(1);
            table0.WidthPercentage = 100f;
            table0.TotalWidth = 400f;
            table0.HorizontalAlignment = Element.ALIGN_CENTER;
            //float[] widths = new float[] { 200f };
            //table0.SetWidths(widths);
            table0.AddCell(imageCell);

            table0.WriteSelectedRows(0, -1, 0, (document.PageSize.Height - 10), writer.DirectContent);
        }
        public override void OnEndPage(PdfWriter writer, Document document)
        {
            Font fontsmall = new Font(FontFactory.GetFont("Arial", 8, Font.NORMAL));

            base.OnEndPage(writer, document);

            PdfPTable tabFot = new PdfPTable(4);
            tabFot.WidthPercentage = 100f;
            tabFot.TotalWidth = 100f;
            //PdfPCell cell;
            tabFot.TotalWidth = 300F;
            //PdfPTable table10 = new PdfPTable(4);
            PdfPCell cell35 = new PdfPCell();

            PdfPCell cell35line = new PdfPCell();
            float[] widths2 = new float[] { 120f, 130f, 162f, 160f };
            tabFot.SetTotalWidth(widths2);
            tabFot.SetWidthPercentage(widths2, PageSize.A4);
            tabFot.SetWidths(widths2);
            cell35line.AddElement(new Paragraph(new Chunk("________________________________________________________________________________________________________________", fontsmall)));
            cell35line.Colspan = 4;
            cell35line.Border = 0;
            cell35line.NoWrap = true;
            tabFot.AddCell(cell35line);
            cell35.AddElement(new Paragraph(new Chunk("Eurotextiles GmbH & Co. KG", fontsmall)));
            //cell35.PaddingTop = 7.5f;
            //cell35.PaddingBottom = 7.5f;
            cell35.Border = 0;
            tabFot.AddCell(cell35);

            PdfPCell cell36 = new PdfPCell();

            cell36.AddElement(new Paragraph(new Chunk("HRA 3451", fontsmall)));
            cell36.PaddingTop = 1.5f;
            cell36.Border = 0;
            tabFot.AddCell(cell36);

            PdfPCell cell37 = new PdfPCell();

            cell37.AddElement(new Paragraph(new Chunk("Kompl. Indra Enterprises GmbH", fontsmall)));
            cell37.PaddingTop = 1.5f;
            cell37.Border = 0;
            tabFot.AddCell(cell37);

            PdfPCell cell38 = new PdfPCell();

            cell38.AddElement(new Paragraph(new Chunk("Bankverbindung", fontsmall)));
            cell38.PaddingTop = 1.5f;
            cell38.Border = 0;
            tabFot.AddCell(cell38);

            PdfPCell cell310 = new PdfPCell();

            cell310.AddElement(new Paragraph(new Chunk("Mammolshainer Weg 14", fontsmall)));
            cell310.PaddingTop = 1.5f;
            cell310.Border = 0;
            tabFot.AddCell(cell310);

            PdfPCell cell40 = new PdfPCell();

            cell40.AddElement(new Paragraph(new Chunk("Amtsgericht Königstein", fontsmall)));
            cell40.PaddingTop = 1.5f;
            cell40.Border = 0;
            tabFot.AddCell(cell40);

            PdfPCell cell59 = new PdfPCell();

            cell59.AddElement(new Paragraph(new Chunk("Geschäftsführer: Krishna Javaji", fontsmall)));
            cell59.PaddingTop = 1.5f;
            cell59.Border = 0;
            tabFot.AddCell(cell59);

            PdfPCell cell60 = new PdfPCell();

            cell60.AddElement(new Paragraph(new Chunk("Nassauische Sparkasse", fontsmall)));
            cell60.PaddingTop = 1.5f;
            cell60.Border = 0;
            tabFot.AddCell(cell60);

            PdfPCell cell61 = new PdfPCell();

            cell61.AddElement(new Paragraph(new Chunk("61462 Königstein im Taunus", fontsmall)));
            cell61.PaddingTop = 1.5f;
            cell61.Border = 0;
            tabFot.AddCell(cell61);


            PdfPCell cell62 = new PdfPCell();

            cell62.AddElement(new Paragraph(new Chunk("Steuerrnummer 003 315 60117", fontsmall)));
            cell62.PaddingTop = 1.5f;
            cell62.Border = 0;
            tabFot.AddCell(cell62);
            PdfPCell cell64 = new PdfPCell();

            cell64.AddElement(new Paragraph(new Chunk("HRB 5137 Amtsgericht Königstein", fontsmall)));
            cell64.PaddingTop = 1.5f;
            cell64.Border = 0;
            tabFot.AddCell(cell64);

            PdfPCell cell65 = new PdfPCell();

            cell65.AddElement(new Paragraph(new Chunk("IBAN :DE15 5105 0015 0270 0607 06", fontsmall)));
            cell65.PaddingTop = 1.5f;
            cell65.Border = 0;
            tabFot.AddCell(cell65);

            PdfPCell cell66 = new PdfPCell();

            cell66.AddElement(new Paragraph(new Chunk("Fon +49 6174/25980", fontsmall)));
            cell66.PaddingTop = 1.5f;
            cell66.PaddingBottom = 5.5f;
            cell66.Border = 0;
            tabFot.AddCell(cell66);

            PdfPCell cell67 = new PdfPCell();

            cell67.AddElement(new Paragraph(new Chunk("USt-IdNr. DE279479010", fontsmall)));
            cell67.PaddingTop = 1.5f;
            cell67.PaddingBottom = 5.5f;
            cell67.Border = 0;
            tabFot.AddCell(cell67);

            PdfPCell cell68 = new PdfPCell();

            cell68.AddElement(new Paragraph(new Chunk("Steuernummer 003 236 16128", fontsmall)));
            cell68.PaddingTop = 1.5f;
            cell68.PaddingBottom = 5.5f;
            cell68.Border = 0;
            tabFot.AddCell(cell68);


            PdfPCell cell71 = new PdfPCell();

            cell71.AddElement(new Paragraph(new Chunk("SWIFT:-BIC NASSDE55XXX", fontsmall)));
            cell71.PaddingTop = 1.5f;
            cell71.PaddingBottom = 5.5f;
            cell71.Border = 0;
            tabFot.AddCell(cell71);
            tabFot.WriteSelectedRows(-300, -1, 20, (document.PageSize.Height - 735), writer.DirectContent);
        }
        public override void OnCloseDocument(PdfWriter writer, Document document)
        {
            base.OnCloseDocument(writer, document);

            //headerTemplate.BeginText();
            //headerTemplate.SetFontAndSize(bf, 12);
            //headerTemplate.SetTextMatrix(0, 0);
            //headerTemplate.ShowText((writer.PageNumber - 1).ToString());
            //headerTemplate.EndText();

            //footerTemplate.BeginText();
            //footerTemplate.SetFontAndSize(bf, 12);
            //footerTemplate.SetTextMatrix(0, 0);
            //footerTemplate.ShowText((writer.PageNumber - 1).ToString());
            //footerTemplate.EndText();
        }
        public override void OnOpenDocument(PdfWriter writer, Document document)
        {
            try
            {
                DateTime PrintTime = DateTime.Now;
                bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                cb = writer.DirectContent;
                headerTemplate = cb.CreateTemplate(100, 100);
                footerTemplate = cb.CreateTemplate(50, 50);
            }
            catch (DocumentException de)
            {
            }
            catch (System.IO.IOException ioe)
            {
            }
        }
    }
}