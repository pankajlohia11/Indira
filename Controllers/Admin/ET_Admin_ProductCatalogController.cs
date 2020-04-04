using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using BusinessEntity.EntityModels;
using BusinessEntity.CustomModels;
using BusinessLogic;

namespace Euro.Controllers.Admin
{

    public class ET_Admin_ProductCatalogController : Controller
    {
        // GET: ET_Admin_ProductCatalog
        EntityClasses DbContext = new EntityClasses();
        public static string prefix = "";
        public static bool automanual = false;
        BAL bal = new BAL();
        error_master objERR = new error_master();
        tbl_loginfo objLOG = new tbl_loginfo();
        //Checking Code Auto/Manual
        private void AutoManual()
        {
            ViewBag.AutoManual = false;
            automanual = false;
            List<Tbl_Document_Master> ObjDoc = bal.AutoManual_BL(8024);
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
        public ActionResult ET_Admin_ProductCatalog()
        {
            AutoManual();
            ViewBag.Login_Name = Session["DisplayName"].ToString();
            return View();
        }
        //Get the Product catalog list
        public JsonResult GetProductCatalogList(bool deleted)
        {
            try
            {

                //var data = DbContext.Tbl_ProductCatalog.Where(m => m.PC_ID != 0 && m.DELETED == false && m.ACTIVE_STATUS==true).ToList();
                //var data = (from PC in DbContext.Tbl_ProductCatalog
                //           join PM in DbContext.Tbl_Product_Master on PC.PRODUCT_ID equals PM.P_ID into comp
                //            from PM in comp
                //            join MUOM in DbContext.tbl_LookUp on PM.P_UOM equals MUOM.LU_Code into uom
                //            from MUOM in uom
                //            where PC.DELETED == deleted && PC.ACTIVE_STATUS == true && MUOM.LU_Type==2 && PM.DELETED == false
                //            select new 
                //            {
                //                PM.P_ID,
                //                PM.P_Name,
                //                PC.PC_CODE,
                //                PC.PRODUCT_ID,
                //                PC.PRODUCT_CODE,
                //                PC.PRODUCT_SPECIFICATION,
                //                PC.PRODUCT_UOM,
                //                PC.UNIT_PRICE1,
                //                PC.UNIT_PRICE2,
                //                PC.UNIT_PRICE3,
                //                PC.UNIT_PRICE4,
                //                PC.LAST_PRICE_REWISE_DATE,
                //                PC.ACTIVE_STATUS,
                //                MUOM.LU_Description,
                //                PC.DELETED,
                //                PC.PC_ID

                //            }
                //            ).ToList();
                var data = (from PC in DbContext.Tbl_ProductCatalog_Master
                            where PC.DELETED == deleted
                            select new
                            {
                                PC.PC_MasterID,
                                PC.PC_CODE,
                                PC.PC_Name,
                                PC.PC_Description,
                                PC.PC_CatalogueCode,
                                //PC.PC_ProductCategory,
                                PC.LAST_UPDATED_DATE,
                                PC.PC_CatalogType,
                                PC.CREATED_DATE
                            }
                            ).OrderByDescending(a=>a.CREATED_DATE).ToList();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exe)
            {
                throw exe;
            }
        }

        /// <summary>
        /// Get the Product Articles for Specific Catalog.
        /// </summary>
        /// <param name="catalogCode"></param>
        /// <returns></returns>
        public JsonResult GetProductsForCatalog(string catalogCode)
        {
            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                {
                    DbContext.Configuration.ProxyCreationEnabled = false;
                    int companykey = Convert.ToInt32(Session["CompanyKey"]);
                    var products = new[] { new {   P_ID = decimal.MinValue,
                                            P_CategoryID = decimal.MinValue,
                                            P_ArticleNo = string.Empty,
                                            P_ShortName = string.Empty,
                                            P_Name = string.Empty} }.ToList();

                    if (!String.IsNullOrEmpty(catalogCode))
                    {
                        products = (from PCL in DbContext.Tbl_ProductCatalog
                                    join PCM in DbContext.Tbl_ProductCatalog_Master on PCL.PC_CODE equals PCM.PC_CatalogueCode
                                    join Pct in DbContext.Tbl_Product_Master on PCL.PRODUCT_ID equals Pct.P_ID
                                    join MUOM in DbContext.tbl_LookUp on Pct.P_UOM equals MUOM.LU_Code
                                    //join product in DbContext.Tbl_Product_Master on MUOM.LU_Code equals product.P_UOM
                                    where PCM.PC_CatalogueCode == catalogCode
                                    select new
                                    {
                                        Pct.P_ID,
                                        Pct.P_CategoryID,
                                        Pct.P_ArticleNo,
                                        Pct.P_ShortName,
                                        Pct.P_Name
                                    }).Distinct().OrderBy(m => m.P_ArticleNo).ToList();
                    }
                    else
                    {
                        products = (from PM in DbContext.Tbl_Product_Master
                                        //join CG in dbcontext.Tbl_Master_Category on PM.P_CategoryID equals CG.CAT_CODE
                                    where PM.COM_KEY == companykey && PM.DELETED == false
                                    select new
                                    {
                                        PM.P_ID,
                                        PM.P_CategoryID,
                                        PM.P_ArticleNo,
                                        PM.P_ShortName,
                                        PM.P_Name
                                    }).Distinct().OrderBy(m => m.P_ArticleNo).ToList();
                    }

                    var json = new JavaScriptSerializer().Serialize(products);
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

        public JsonResult ET_Admin_ProductCatalog_Details_UpdateChildtable(string id)
        {
            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                {
                    var data = (from PCL in DbContext.Tbl_ProductCatalog
                                join PCM in DbContext.Tbl_ProductCatalog_Master on PCL.PC_CODE equals PCM.PC_CatalogueCode
                                join Pct in DbContext.Tbl_Product_Master on PCL.PRODUCT_ID equals Pct.P_ID
                                join MUOM in DbContext.tbl_LookUp on Pct.P_UOM equals MUOM.LU_Code
                                //join product in DbContext.Tbl_Product_Master on MUOM.LU_Code equals product.P_UOM
                                where PCM.PC_CatalogueCode == id
                                select new
                                {
                                    P_ID = Pct.P_ID,
                                    PCG_ID = PCL.PRODUCT_CATEGORY_CODE,
                                    PCSG_ID = PCL.PRODUCT_SUBCATEGORY_CODE,
                                    P_ShortName = Pct.P_ShortName,
                                    LU_Description = MUOM.LU_Description,
                                    LU_PackingUOM = Pct.P_PackingQuantityUOM,
                                    P_PC_CODE = PCL.PC_CODE,
                                    P_PC_ID = PCL.PC_ID,
                                    P_PRODUCT_CODE = PCL.PRODUCT_CODE,
                                    P_Description = PCL.PRODUCT_SPECIFICATION,
                                    P_PRODUCT_UOM = PCL.PRODUCT_UOM,
                                    P_PRODUCT_ID = PCL.PRODUCT_ID,
                                    P_SummerPrice = PCL.Summer_Price == null ? 0 : PCL.Summer_Price,
                                    P_WinterPrice = PCL.Winter_Price == null ? 0 : PCL.Winter_Price,
                                    P_LAST_PRICE_REWISE_DATE = PCL.LAST_PRICE_REWISE_DATE,
                                    P_UNIT_PRICE1 = PCL.UNIT_PRICE1,
                                    P_UNIT_PRICE2 = PCL.UNIT_PRICE2,
                                    P_UNIT_PRICE3 = PCL.UNIT_PRICE3,
                                    P_UNIT_PRICE4 = PCL.UNIT_PRICE4,
                                }).ToList();

                    var modelItems = data.Select((orderItem, index) => new{ SO_Serial = index + 1, P_ID = orderItem.P_ID, P_PC_CODE = orderItem.P_PC_CODE, PCG_ID = orderItem.PCG_ID, PCSG_ID = orderItem.PCSG_ID, P_ShortName = orderItem.P_ShortName,  P_PC_ID = orderItem.P_PC_ID, P_PRODUCT_CODE = orderItem.P_PRODUCT_CODE, P_Description = orderItem.P_Description, P_PRODUCT_UOM = orderItem.P_PRODUCT_UOM, LU_Description = orderItem.LU_Description, P_PRODUCT_ID = orderItem.P_PRODUCT_ID, P_SummerPrice = orderItem.P_SummerPrice, P_WinterPrice = orderItem.P_WinterPrice, P_UNIT_PRICE1 = orderItem.P_UNIT_PRICE1, P_UNIT_PRICE2 = orderItem.P_UNIT_PRICE2, P_UNIT_PRICE3 = orderItem.P_UNIT_PRICE3, P_UNIT_PRICE4 = orderItem.P_UNIT_PRICE4, P_LAST_PRICE_REWISE_DATE = orderItem.P_LAST_PRICE_REWISE_DATE }).ToList();

                    var json = new JavaScriptSerializer().Serialize(modelItems);
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
                    return Json("ERR" + errid.ToString() + objERR.err_details, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json("ET_SessionExpire", "ET_Login");
            }
        }
        //Get the product restore list
        public JsonResult GetProductCatalogRestoreList(bool deleted)
        {
            try
            {
                //var data = DbContext.Tbl_ProductCatalog.Where(m => m.PC_ID != 0 && m.DELETED == true).ToList();
                var data = (from PCM in DbContext.Tbl_ProductCatalog_Master
                             where PCM.DELETED == true && PCM.PC_CatalogueCode != string.Empty
                             select new
                             {
                                 PC_MasterID = PCM.PC_MasterID,
                                 PC_CODE = PCM.PC_CatalogueCode,
                                 PC_Name = PCM.PC_Name,
                                 PC_Description = PCM.PC_Description,
                                 CATALOG_VALIDITY = PCM.VALIDITY_DATE.Value
                             }
                                 ).ToList();
                var modelItems = data.Select((productItem, index) => new { SO_Serial = index + 1,PC_MasterID = productItem.PC_MasterID, PC_CODE = productItem.PC_CODE, PC_Name = productItem.PC_Name, PC_Description = productItem.PC_Description, CATALOG_VALIDITY = productItem.CATALOG_VALIDITY}).ToList();
                return Json(modelItems, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exe)
            {
                throw exe;
            }
        }
        public JsonResult GetProductCatalogValidityDate(string Q_CatalogId)
        {
            var catalogValidity = DateTime.Now;
            if (Q_CatalogId != null)
            {
                var data = DbContext.Tbl_ProductCatalog_Master.Where(m => m.PC_CatalogueCode == Q_CatalogId && m.DELETED == false).SingleOrDefault();
                if (data.VALIDITY_DATE != null)
                {
                    catalogValidity = data.VALIDITY_DATE.Value;
                }
            }
            return Json(catalogValidity, JsonRequestBehavior.AllowGet);
        }
        //editg the product
        public JsonResult ET_Admin_ProductCatalog_Update_GetbyID(int id)
        {
            var data = DbContext.Tbl_ProductCatalog_Master.Single(m => m.PC_MasterID == id);
            var json = new JavaScriptSerializer().Serialize(data);
            return Json(json, JsonRequestBehavior.AllowGet);
        }
        //Get the product list
        public JsonResult GetProductList()
        {
            var ProductList = DbContext.Tbl_Product_Master.Where(m => m.DELETED == false).ToList();
            return Json(ProductList, JsonRequestBehavior.AllowGet);
        }
        //Get the UOM list
        public JsonResult GetUOMList()
        {
            var ProductList = DbContext.tbl_LookUp.Where(m => m.LU_Type == 2).ToList();
            return Json(ProductList, JsonRequestBehavior.AllowGet);
        }

        //Get the product
        public JsonResult GetProducts(int id, string productCategory)
        {

            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                {
                    //yarn 
                    var masterCategory = 0;
                    string productGroupName = string.Empty;
                    if (productCategory == "1")
                    {
                        masterCategory = 1;
                        productGroupName = "YARN";
                    }
                    else if (productCategory == "2")
                    {
                        masterCategory = 4;
                        productGroupName = "FABRIC";
                    }
                    var data = 1;
                    var yarnCat = 1;
                    var fabCat = 1;
                    if (masterCategory != 0)
                    {
                        data = (int)(DbContext.Tbl_ProductGroup.Where(m => m.PG_TYPE == 1 && m.PG_NAME.ToLowerInvariant() == productGroupName.ToLowerInvariant()).Select(a => a.PG_ID)).SingleOrDefault();
                        masterCategory = Convert.ToInt32(data);
                    }
                    else
                    {
                        yarnCat = (int)(DbContext.Tbl_ProductGroup.Where(m => m.PG_TYPE == 1 && m.PG_NAME.ToLowerInvariant() == "yarn").Select(a => a.PG_ID)).SingleOrDefault();
                        fabCat = (int)(DbContext.Tbl_ProductGroup.Where(m => m.PG_TYPE == 1 && m.PG_NAME.ToLower() == "fabric").Select(a => a.PG_ID)).SingleOrDefault();
                    }

                    DbContext.Configuration.ProxyCreationEnabled = false;
                    int companykey = Convert.ToInt32(Session["CompanyKey"]);
                    if (id == 0)
                    {

                        var Products = (from PM in DbContext.Tbl_Product_Master
                                        //join CG in DbContext.Tbl_Master_Category on PM.P_CategoryID equals CG.CAT_CODE
                                        join MUOM in DbContext.tbl_LookUp on PM.P_UOM equals MUOM.LU_Code
                                        where PM.COM_KEY == companykey && PM.DELETED == false && (PM.P_CategoryID == masterCategory || (masterCategory == 0 && PM.P_CategoryID != yarnCat && PM.P_CategoryID != fabCat))
                                        select new
                                        {
                                            PM.P_ID,
                                            PM.P_CategoryID,
                                            PM.P_ArticleNo,
                                            PM.P_ShortName,
                                            PM.P_Name,
                                            MUOM.LU_Description,
                                            PM.P_Description
                                        }).Distinct().OrderBy(m => m.P_ArticleNo).ToList();

                        var  modelItems = Products.Select((productItem, index) => new{ SO_Serial = index + 1, P_ID = productItem.P_ID, P_CategoryID = productItem.P_CategoryID, P_ArticleNo = productItem.P_ArticleNo, P_ShortName = productItem.P_ShortName, P_Name = productItem.P_Name, P_SummerPrice = 0.0, P_WinterPrice = 0.0, P_LAST_UPDATED_DATE = DateTime.Now, LU_Description = productItem.LU_Description,P_Description = productItem.P_Description}).ToList();
                        //var Products = dbcontext.Tbl_Product_Master.Where(m => m.COM_KEY == companykey && m.DELETED == false).OrderBy(m => m.P_ArticleNo).ToList();
                        var json = new JavaScriptSerializer().Serialize(modelItems);
                        return Json(json, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        var product = DbContext.Tbl_Order_Details.Where(m => m.AGEN_TRAD_PO_ID == id).Select(a => a.PRODUCT_ID);
                        //var Products = dbcontext.Tbl_Product_Master.Where(m => m.COM_KEY == companykey && (m.DELETED == false || product.Contains(m.P_ID))).OrderBy(m => m.P_ArticleNo).ToList();
                        var Products = (from PM in DbContext.Tbl_Product_Master
                                        join PCL in DbContext.Tbl_ProductCatalog on PM.P_ID equals PCL.PRODUCT_ID
                                        //join CG in DbContext.Tbl_Master_Category on PM.P_CategoryID equals CG.CAT_CODE
                                        join MUOM in DbContext.tbl_LookUp on PM.P_UOM equals MUOM.LU_Code
                                        where PM.COM_KEY == companykey && PM.DELETED == false && (PM.P_CategoryID == masterCategory || (masterCategory == 0 && PM.P_CategoryID != yarnCat && PM.P_CategoryID != fabCat))
                                        select new
                                        {
                                            PM.P_ID,
                                            PM.P_CategoryID,
                                            PM.P_ArticleNo,
                                            PM.P_ShortName,
                                            PM.P_Name,
                                            PCL.Summer_Price,
                                            PCL.Winter_Price,
                                            PCL.LAST_UPDATED_DATE,
                                            MUOM.LU_Description,
                                            PM.P_Description
                                        }).Distinct().OrderBy(m => m.P_ArticleNo).ToList();
                        var modelItems = Products.Select((productItem, index) => new { SO_Serial = index + 1, P_ID = productItem.P_ID, P_CategoryID = productItem.P_CategoryID, P_ArticleNo = productItem.P_ArticleNo, P_ShortName = productItem.P_ShortName, P_Name = productItem.P_Name, P_SummerPrice = productItem.Summer_Price, P_WinterPrice = productItem.Winter_Price, P_LAST_UPDATED_DATE = productItem.LAST_UPDATED_DATE, LU_Description = productItem.LU_Description,P_Description = productItem.P_Description }).ToList();
                        //var Products = dbcontext.Tbl_Product_Master.Where(m => m.COM_KEY == companykey && m.DELETED == false).OrderBy(m => m.P_ArticleNo).ToList();
                        var json = new JavaScriptSerializer().Serialize(modelItems);
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

        //Get the product details
        public JsonResult GetProductDetails(int id)
        {
            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                {
                    var data = DbContext.Tbl_Product_Master.Single(m => m.P_ID == id);
                    var json = new JavaScriptSerializer().Serialize(data);
                    return Json(json, JsonRequestBehavior.AllowGet);
                }
                catch (Exception exe)
                {
                    // Insert Error Log
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
                //Session Expiry
                return Json("Expired", JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetProductsCollection(int parentCategory, int subCategory)
        {

            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                {
                    DbContext.Configuration.ProxyCreationEnabled = false;
                    int companykey = Convert.ToInt32(Session["CompanyKey"]);
                    var Products = (from PM in DbContext.Tbl_Product_Master
                                    //join CG in DbContext.Tbl_Master_Category on PM.P_CategoryID equals CG.CAT_CODE
                                    where PM.COM_KEY == companykey && PM.DELETED == false && (PM.P_CategoryID == parentCategory && PM.P_SubcategoryID == subCategory)
                                    select new
                                    {
                                        PM.P_ID,
                                        PM.P_CategoryID,
                                        PM.P_ArticleNo,
                                        PM.P_ShortName,
                                        PM.P_Name
                                    }).Distinct().OrderBy(m => m.P_ArticleNo).ToList();

                    //var Products = dbcontext.Tbl_Product_Master.Where(m => m.COM_KEY == companykey && m.DELETED == false).OrderBy(m => m.P_ArticleNo).ToList();
                    var json = new JavaScriptSerializer().Serialize(Products);
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

        public JsonResult GetProductSubGroupDetails(int parentGroup)
        {
            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                {
                    using (EntityClasses dbcontext = new EntityClasses())
                    {
                        int com_key = Convert.ToInt32(Session["CompanyKey"]);
                        //get all the parent groups
                        var productGroupList = dbcontext.Tbl_ProductGroup.Where(x => x.COM_KEY == com_key && x.DELETED == false && x.PG_TYPE == 2 && x.PG_PARENT_ID == parentGroup).ToList();
                        var productGroupJson = new JavaScriptSerializer().Serialize(productGroupList);
                        return Json(productGroupJson, JsonRequestBehavior.AllowGet);
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

        public JsonResult GetProductGroupList()
        {
            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                {
                    using (EntityClasses dbcontext = new EntityClasses())
                    {
                        int com_key = Convert.ToInt32(Session["CompanyKey"]);
                        //get all the parent groups
                        var productGroupList = dbcontext.Tbl_ProductGroup.Where(x => x.COM_KEY == com_key && x.DELETED == false && x.PG_TYPE == 1).ToList();
                        var productGroupJson = new JavaScriptSerializer().Serialize(productGroupList);
                        return Json(productGroupJson, JsonRequestBehavior.AllowGet);
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
        //Insert/update the product
        public JsonResult ET_Admin_ProductCatalog_Add(decimal ProductMasterId, decimal PriceBookId, string PriceBookCode, string ProductCatalogCode, string ProductCatalogName,string ProductCatalogDes, string ValidityDate,string catalogStringType, string articleDetails)
        {
            decimal k = 0;
            var context = new EntityClasses();
            var transaction = context.Database.BeginTransaction();
            var json = "";
            int catalogtype = Convert.ToInt32(catalogStringType);
            //int productType = Convert.ToInt32(MasterCategory);
            //var productCategory = 0;
            //if (productType == 1)
            //{
            //    productCategory = 1;
            //}
            //else if (productType == 2)
            //{
            //    productCategory = 4;
            //}

            //decimal ProductId, string ProductCode,string ProductSpecification,string uom, decimal unitprice1, decimal unitprice2, decimal unitprice3, decimal unitprice4, string lastpricerewisedate
            try
            {
                string valid = validations(PriceBookId, PriceBookCode, ValidityDate, ProductCatalogCode,catalogtype, articleDetails);
                if (valid == "")
                {
                    DateTime RewiseDate = DateTime.ParseExact(ValidityDate, "dd-MM-yyyy", null);
                    int data = DbContext.Tbl_ProductCatalog.Where(m => m.DELETED == false).Count();

                    //if (DbContext.Tbl_ProductCatalog.Count() > 0)
                    //{
                    //    var ActiveRecords = DbContext.Tbl_ProductCatalog.Where(m => m.DELETED == false && m.ACTIVE_STATUS == true).ToList();
                    //    ActiveRecords.ForEach(m => m.ACTIVE_STATUS = false);
                    //}
                    if (ProductMasterId == 0)
                    {
                        Tbl_ProductCatalog_Master objtpt = new Tbl_ProductCatalog_Master()
                        {
                            PC_MasterID = ProductMasterId,
                            PC_CatalogueCode = ProductCatalogCode,
                            PC_Name = ProductCatalogName,
                            PC_Description = ProductCatalogDes,
                            //PC_ProductCategory = MasterCategory,
                            //PC_SubProductCategory = SubCategory,
                            DELETED = false,
                            CREATED_DATE = DateTime.Now,
                            CREATED_BY = Convert.ToInt32(Session["UserID"]),
                            LAST_UPDATED_DATE = DateTime.Now,
                            LAST_UPDATED_BY = Convert.ToInt32(Session["UserID"]),
                            COM_KEY = Convert.ToInt32(Session["Companykey"]),
                            VALIDITY_DATE = RewiseDate,
                            PC_CatalogType = catalogtype
                        };
                        DbContext.Tbl_ProductCatalog_Master.Add(objtpt);
                        DbContext.SaveChanges();
                        if (automanual == true)
                        {
                            int len = 10 - (prefix + objtpt.PC_MasterID).Length;
                            string code = prefix + new String('0', len) + objtpt.PC_MasterID;
                            Tbl_ProductCatalog_Master ProductCatalogMaster = DbContext.Tbl_ProductCatalog_Master.Single(m => m.PC_MasterID == objtpt.PC_MasterID);
                            {
                                ProductCatalogMaster.PC_CatalogueCode = code;
                                ProductCatalogCode = code;
                            };
                            DbContext.SaveChanges();
                            json = "Success:" + ProductCatalogCode;
                            if (ProductCatalogMaster.PC_MasterID == 0)
                            {
                                json = "Failed";
                            }
                            else
                            {
                                objLOG.log_dockey = "8024";
                                objLOG.log_userid = Session["UserID"].ToString();
                                objLOG.log_recordkey = data.ToString();
                                if (ProductMasterId == 0)
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
                        }
                    }
                    else
                    {
                        Tbl_ProductCatalog_Master objtom = DbContext.Tbl_ProductCatalog_Master.Single(m => m.PC_MasterID == ProductMasterId);
                        {
                            objtom.PC_Name = ProductCatalogName;
                            objtom.PC_Description = ProductCatalogDes;
                            //objtom.PC_ProductCategory = MasterCategory;
                            objtom.DELETED = false;
                            objtom.CREATED_DATE = DateTime.Now;
                            objtom.CREATED_BY = Convert.ToInt32(Session["UserID"]);
                            objtom.LAST_UPDATED_DATE = DateTime.Now;
                            objtom.LAST_UPDATED_BY = Convert.ToInt32(Session["UserID"]);
                            objtom.VALIDITY_DATE = RewiseDate;
                            objtom.PC_CatalogType = catalogtype;
                            //objtom.PC_SubProductCategory = SubCategory;
                        };

                        DbContext.SaveChanges();
                        //if (context.SaveChanges() == 0)
                        //{
                        //    success = false;
                        //}
                        //k = obj.SO_ID;
                        //tomgl.SO_ID = objtom.SO_ID;
                        //OrdCode = objtom.SO_Code;
                    }
                    //Add Catalog Details
                    var response = ET_Admin_ProductCatalog_AddDetails(ProductMasterId, PriceBookId, PriceBookCode, ProductCatalogCode, RewiseDate, articleDetails);
                    if (response)
                    {
                        transaction.Commit();
                        json = "Success:" + ProductCatalogCode;
                    }
                    else
                    {
                        k = 0;
                        transaction.Rollback();
                    }
                }
                else
                {
                    json = "Validation:" + valid;
                    return Json(json, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception exe)
            {

                k = 0;
                transaction.Rollback();
                throw exe;
            }
            finally
            {
                transaction.Dispose();
                context.Dispose();
            }
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        private bool ET_Admin_ProductCatalog_AddDetails(decimal productCatalogMasterId, decimal PriceBookId, string PriceBookCode, string ProductCatalogCode, DateTime lastpricerewisedate, string articleDetails)
        {
            bool success = true;
            try
            {
                if (productCatalogMasterId == 0)
                {
                    // Insert new contacts data
                    string[] ChildRow = articleDetails.Split('|');
                    for (int i = 0; i < ChildRow.Length - 1; i++)
                    {
                        string[] ChildRecord = ChildRow[i].Split('}');

                        var productGroupId = Convert.ToDecimal(ChildRecord[0]);
                        var productSubGroupId = Convert.ToDecimal(ChildRecord[1]);
                        var Productid = Convert.ToDecimal(ChildRecord[2]);
                        var summerPrice = Convert.ToDecimal(ChildRecord[5]);
                        var winterPrice = Convert.ToDecimal(ChildRecord[6]);

                        var Categoryid = (from id in DbContext.Tbl_Product_Master
                                          where id.P_ID == Productid
                                          select id.P_CategoryID).FirstOrDefault();

                        var productCode = (from id in DbContext.Tbl_Product_Master
                                           where id.P_ID == Productid
                                           select id.P_Code).FirstOrDefault();

                        var productSpecification = (from id in DbContext.Tbl_Product_Master
                                                    where id.P_ID == Productid
                                                    select id.P_Description).FirstOrDefault();

                        var Uom = (from id in DbContext.Tbl_Product_Master
                                   where id.P_ID == Productid
                                   select id.P_UOM).FirstOrDefault();
                        Tbl_ProductCatalog objPCDetails = new Tbl_ProductCatalog()
                        {
                            //PC_ID = PriceBookId,
                            PC_CODE = ProductCatalogCode,
                            PRODUCT_CATEGORY_CODE = productGroupId,
                            PRODUCT_SUBCATEGORY_CODE = productSubGroupId,
                            PRODUCT_ID = Productid,
                            PRODUCT_CODE = productCode,
                            PRODUCT_SPECIFICATION = productSpecification,
                            UNIT_PRICE1 = 0,
                            UNIT_PRICE2 = 0,
                            UNIT_PRICE3 = 0,
                            UNIT_PRICE4 = 0,
                            Summer_Price = summerPrice,
                            Winter_Price = winterPrice,
                            PRODUCT_UOM = Uom,
                            LAST_PRICE_REWISE_DATE = lastpricerewisedate,
                            ACTIVE_STATUS = true,
                            DELETED = false,
                            CREATED_DATE = DateTime.Now,
                            CREATED_BY = Convert.ToInt32(Session["UserID"]),
                            LAST_UPDATED_DATE = DateTime.Now,
                            LAST_UPDATED_BY = Convert.ToInt32(Session["UserID"]),
                            COM_KEY = Convert.ToInt32(Session["Companykey"])
                        };
                        DbContext.Tbl_ProductCatalog.Add(objPCDetails);
                    }
                    if (DbContext.SaveChanges() != ChildRow.Length - 1)
                    {
                        success = false;
                    }
                }
                else
                {
                    string[] ChildRow = articleDetails.Split('|');
                    decimal[] products = new decimal[ChildRow.Length - 1];
                    // Delete previous Offer data
                    var delOrderdetails = DbContext.Tbl_ProductCatalog.Where(m => m.PC_CODE == ProductCatalogCode);
                    if (delOrderdetails.Count() > 0)
                    {
                        DbContext.Tbl_ProductCatalog.RemoveRange(delOrderdetails);
                        DbContext.SaveChanges();
                    }
                    for (int i = 0; i < ChildRow.Length - 1; i++)
                    {
                        string[] ChildRecord = ChildRow[i].Split('}');

                        var productGroupId = Convert.ToDecimal(ChildRecord[0]);
                        var productSubGroupId = Convert.ToDecimal(ChildRecord[1]);
                        var Productid = Convert.ToDecimal(ChildRecord[2]);
                        var summerPrice = Convert.ToDecimal(ChildRecord[5]);
                        var winterPrice = Convert.ToDecimal(ChildRecord[6]);

                        var Categoryid = (from id in DbContext.Tbl_Product_Master
                                          where id.P_ID == Productid
                                          select id.P_CategoryID).FirstOrDefault();

                        var productCode = (from id in DbContext.Tbl_Product_Master
                                           where id.P_ID == Productid
                                           select id.P_Code).FirstOrDefault();

                        var productSpecification = (from id in DbContext.Tbl_Product_Master
                                                    where id.P_ID == Productid
                                                    select id.P_Description).FirstOrDefault();

                        var Uom = (from id in DbContext.Tbl_Product_Master
                                   where id.P_ID == Productid
                                   select id.P_UOM).FirstOrDefault();
                        //Update Detail
                        //decimal orderrefId = Convert.ToDecimal(obj.SO_ID);
                        Tbl_ProductCatalog objPCDetails = new Tbl_ProductCatalog()
                        {
                            //PC_ID = PriceBookId,
                            PC_CODE = ProductCatalogCode,
                            PRODUCT_CATEGORY_CODE = productGroupId,
                            PRODUCT_SUBCATEGORY_CODE = productSubGroupId,
                            PRODUCT_ID = Productid,
                            PRODUCT_CODE = productCode,
                            PRODUCT_SPECIFICATION = productSpecification,
                            UNIT_PRICE1 = 0,
                            UNIT_PRICE2 = 0,
                            UNIT_PRICE3 = 0,
                            UNIT_PRICE4 = 0,
                            Summer_Price = summerPrice,
                            Winter_Price = winterPrice,
                            PRODUCT_UOM = Uom,
                            LAST_PRICE_REWISE_DATE = lastpricerewisedate,
                            ACTIVE_STATUS = true,
                            DELETED = false,
                            CREATED_DATE = DateTime.Now,
                            CREATED_BY = Convert.ToInt32(Session["UserID"]),
                            LAST_UPDATED_DATE = DateTime.Now,
                            LAST_UPDATED_BY = Convert.ToInt32(Session["UserID"]),
                            COM_KEY = Convert.ToInt32(Session["Companykey"])
                        };
                        DbContext.Tbl_ProductCatalog.Add(objPCDetails);
                    }
                    if (DbContext.SaveChanges() != ChildRow.Length - 1)
                    {
                        success = false;
                    }
                }
            }
            catch (Exception exe)
            {
                success = false;
            }
            finally
            {
            }
            return success;
        }

        //Validate the product
        private string validations(decimal ProductBookId, string productbookcode,string lastpriceRewiseDate,string productCatalogCode,int catalogType, string articleDetails)
        {
            if (!automanual && productbookcode == "")
            {
                return "Enter the Product Book Code";
            }
            if(lastpriceRewiseDate=="")
            {
                return "Select Catalog Validity Date";
            }
            if(catalogType <= 0)
            {
                return "Customer/Supplier Catalog is missing";
            }

            if (!automanual)
            {
                string valid = "";
                if (ProductBookId == 0)
                {
                    int count = DbContext.Tbl_ProductCatalog.Where(m => m.PC_CODE == productbookcode).Count();
                    if (count > 0)
                    {
                         valid="exist";
                    }
                }
                else
                {
                    int count = DbContext.Tbl_ProductCatalog.Where(m => m.PC_CODE == productbookcode && m.PC_ID != ProductBookId).Count();
                    if (count > 0)
                    {
                         valid= "exist";
                    }
                }
                if (valid != "")
                {
                    return "Product Book Code Already Exist";
                }
            }

            try
            {
                string[] ChildRow = articleDetails.Split('|');
                string[] tableColumns = new string[ChildRow.Length];
                for (int i = 0; i < ChildRow.Length - 1; i++)
                {
                    string[] ChildRecord = ChildRow[i].Split('}');
                    if (tableColumns.Contains(Convert.ToDecimal(ChildRecord[2]).ToString()))
                    {
                        return "Product is repeated at row :" + (i + 1);
                    }
                    tableColumns[i] = Convert.ToDecimal(ChildRecord[2]).ToString();
                }
            }
            catch
            {
                return "Unable to process your request. Please verify product data.";
            }
            return "";
        }
        //Restore the delete product
        public ActionResult ET_Admin_ProductCatalog_Restore_delete(decimal id, bool type)
        {
            // 
            int result = 0;
            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                {
                    Tbl_ProductCatalog_Master obj = DbContext.Tbl_ProductCatalog_Master.Single(m => m.PC_MasterID == id);
                    {
                        obj.DELETED = type;
                        obj.ACTIVE_STATUS = false;
                        obj.LAST_UPDATED_DATE = DateTime.Now;
                        obj.LAST_UPDATED_BY = Convert.ToInt32(Session["UserID"]);
                    }
                    result = DbContext.SaveChanges();
                    var json = "Failed";
                    if (result != 0)
                    {
                        json = "Success";
                    }
                    else
                    {
                        objLOG.log_dockey = "8024";
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
                    throw exe;
                }
            }
            else
            {
                return RedirectToAction("ET_SessionExpire", "ET_Login");
            }
        }
        //View: Popup view
        public ActionResult ET_Admin_ProductCatalog_View(int id)
        {
            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                {
                    //var data = (from PC in DbContext.Tbl_ProductCatalog
                    //            join PM in DbContext.Tbl_Product_Master on PC.PRODUCT_ID equals PM.P_ID into comp
                    //            from PM in comp
                    //            join MUOM in DbContext.tbl_LookUp on PM.P_UOM equals MUOM.LU_Code into uom
                    //            from MUOM in uom
                    //            where PC.DELETED == false && PC.ACTIVE_STATUS ==  true && PC.PC_ID == id && MUOM.LU_Type==2
                    //            select new Product_Catalog_Details
                    //            {
                    //               P_ID= PM.P_ID,
                    //               P_Name= PM.P_Name,
                    //               PC_CODE= PC.PC_CODE,
                    //                PRODUCT_ID= PC.PRODUCT_ID,
                    //                PRODUCT_CODE= PC.PRODUCT_CODE,
                    //                PRODUCT_SPECIFICATION=  PC.PRODUCT_SPECIFICATION,
                    //                PRODUCT_UOM= PC.PRODUCT_UOM,
                    //                UNIT_PRICE1=  PC.UNIT_PRICE1,
                    //                UNIT_PRICE2 = PC.UNIT_PRICE2,
                    //                UNIT_PRICE3 = PC.UNIT_PRICE3,
                    //                UNIT_PRICE4 = PC.UNIT_PRICE4,
                    //                LAST_PRICE_REWISE_DATE =  PC.LAST_PRICE_REWISE_DATE,
                    //                ACTIVE_STATUS= PC.ACTIVE_STATUS,
                    //                UOM_NAME=  MUOM.LU_Description,
                    //                PC_ID=PC.PC_ID

                    //            }
                    //        ).ToList();

                    var data1 = (from PCM in DbContext.Tbl_ProductCatalog_Master
                                 where PCM.DELETED == false && PCM.PC_MasterID == id
                                 select new Product_Catalog_Details()
                                 {
                                     PC_CODE = PCM.PC_CatalogueCode,
                                     PC_Name = PCM.PC_Name,
                                     PC_Description = PCM.PC_Description,
                                     CATALOG_VALIDITY = PCM.VALIDITY_DATE.Value
                                 }
                                 ).ToList();

                    var data2 = (from PCL in DbContext.Tbl_ProductCatalog
                                join PCM in DbContext.Tbl_ProductCatalog_Master on PCL.PC_CODE equals PCM.PC_CatalogueCode
                                join Pct in DbContext.Tbl_Product_Master on PCL.PRODUCT_ID equals Pct.P_ID
                                join MUOM in DbContext.tbl_LookUp on Pct.P_UOM equals MUOM.LU_Code
                                //join product in DbContext.Tbl_Product_Master on MUOM.LU_Code equals product.P_UOM
                                where PCM.DELETED == false && PCM.PC_MasterID == id && MUOM.LU_Type == 2
                                select new Product_Catalog_Details()
                                {
                                    P_ID = Pct.P_ID,
                                    P_Name = Pct.P_Name,
                                    PC_CODE = PCL.PC_CODE,
                                    PC_Name = PCM.PC_Name,
                                    PC_Description = PCM.PC_Description,
                                    UOM_NAME = MUOM.LU_Description,
                                    PC_ID = PCL.PC_ID,
                                    PRODUCT_CODE = PCL.PRODUCT_CODE,
                                    PRODUCT_SPECIFICATION = PCL.PRODUCT_SPECIFICATION,
                                    PRODUCT_UOM = PCL.PRODUCT_UOM,
                                    PRODUCT_ID = PCL.PRODUCT_ID,
                                    P_SummerPrice = PCL.Summer_Price == null ? 0 : PCL.Summer_Price,
                                    P_WinterPrice = PCL.Winter_Price == null ? 0 : PCL.Winter_Price,
                                    //LAST_PRICE_REWISE_DATE = PCL.LAST_PRICE_REWISE_DATE,
                                    CATALOG_VALIDITY = PCM.VALIDITY_DATE.Value,
                                    UNIT_PRICE1 = PCL.UNIT_PRICE1,
                                    UNIT_PRICE2 = PCL.UNIT_PRICE2,
                                    UNIT_PRICE3 = PCL.UNIT_PRICE3,
                                    UNIT_PRICE4 = PCL.UNIT_PRICE4,
                                   
                                }).ToList();

                    ProductCatalogView_CM catalogViewCM = new ProductCatalogView_CM();
                    catalogViewCM.headerObj = data1 ;
                    catalogViewCM.detailsObj = data2;
                    return PartialView("/Views/Admin/ET_Admin_ProductCatalog/ET_Admin_ProductCatalog_View.cshtml", catalogViewCM);
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
    }
}