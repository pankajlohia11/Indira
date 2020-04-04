using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
//using BusinessEntity.CustomModels;
using BusinessEntity.EntityModels;
using BusinessLogic;
using BusinessLogic.Admin_BL;
using System.Web.Script.Serialization;
using BusinessEntity.CustomModels;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System.IO;
using System.Web.UI;
using System.Net.Mail;
using System.Net;
using System.Globalization;
using System.Data.Entity.Core.Objects;

namespace Euro.Controllers.Sales
{
    public class ET_Sales_QuotationController : Controller
    {
        public static string prefix = "";
        public static bool automanual = false;
        BAL bal = new BAL();
        error_master objERR = new error_master();
        tbl_loginfo objLOG = new tbl_loginfo();
        EntityClasses dbcontext = new EntityClasses();
        public ActionResult ET_Sales_Quotation(string type)
        {
            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                if (type != null)
                {
                    if (type == "Trading" || type == "Store" || type == "Agency")
                    {
                        try
                        {
                            AutoManual();
                            ViewBag.Login_Name = Session["DisplayName"].ToString();
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
        public ActionResult ET_Sales_EnquirytoQuotation(string type)
        {
            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                if (type != null)
                {
                    if (type == "Trading" || type == "Store" || type == "Agency")
                    {
                        try
                        {
                            AutoManual();
                            ViewBag.Login_Name = Session["DisplayName"].ToString();
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
        public ActionResult ET_Sales_EnquiryListNotification(string type)
        {
            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
               
                        try
                        {
                            AutoManual();
                            ViewBag.Login_Name = Session["DisplayName"].ToString();
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
                return RedirectToAction("ET_SessionExpire", "ET_Login");
            }
        }
        public ActionResult ET_Sales_QuotationListNotification(string type)
        {
            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {

                try
                {
                    AutoManual();
                    ViewBag.Login_Name = Session["DisplayName"].ToString();
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
                return RedirectToAction("ET_SessionExpire", "ET_Login");
            }
        }
        
        //Check autp/manual
        private void AutoManual()
        {
            ViewBag.AutoManual = false;
            automanual = false;
            List<Tbl_Document_Master> ObjDoc = bal.AutoManual_BL(8015);
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
        //get privillages
        public JsonResult GetPrivilages()
        {

            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                {
                    var privilagelist = bal.GetPrivilages_BL(Convert.ToInt32(Session["UserID"].ToString()), 8015);
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
        //Get the quation list
        public JsonResult GetQuotationList(bool delete,int type)
        {

            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                {
                    int com_key = Convert.ToInt32(Session["CompanyKey"].ToString());

                    dbcontext.Configuration.ProxyCreationEnabled = false;
                    var data = (from a in dbcontext.tbl_QuotationHeader
                                join b in dbcontext.Tbl_Master_CompanyDetails on a.Q_CustomerID equals b.COM_ID into comp
                                from x in comp
                                join d in dbcontext.Tbl_Master_User on a.Q_Sales_Person equals d.USER_ID into user
                                from z in user
                                join e in dbcontext.tbl_LookUp on a.Q_CurrencyCode equals e.LU_Code
                                join w in dbcontext.tbl_EnquiryHeader on a.Q_EnquiryID equals w.E_ID into p
                                from enq in p.DefaultIfEmpty()
                                
                                where a.DELETED == delete && a.COM_KEY == com_key && a.Q_Type == type && e.LU_Type == 1002
                                select new
                                {
                                    Q_ID = a.Q_ID,
                                    Q_Code = a.Q_Code,
                                    Q_Type = a.Q_Type,
                                    Q_Date = a.Q_Date,
                                    Q_CustomerName = x.COM_NAME,
                                    Q_EnquiryCode = enq.E_Code,
                                    Q_CurrencyCode = a.Q_CurrencyCode,
                                    Q_PaymentTerms = a.Q_PaymentTerms,
                                    Q_DeliveryTerms = a.Q_DeliveryTerms,
                                    Q_ValidityDays = a.Q_ValidityDays,
                                    Q_ValidityDate = DbFunctions.AddDays(a.Q_Date,a.Q_ValidityDays),
                                    Q_Enclosures = a.Q_Enclosures,
                                    Q_Quotedescription = a.Q_Quotedescription,
                                    Q_TotalValue = a.Q_TotalValue,
                                    Q_TotalValueCurrency = e.LU_Description ,
                                    Q_SalesPersonName = z.DISPLAY_NAME,
                                    Q_ApprovedStatus = a.Q_ApprovedStatus,
                                    Q_Status=a.Q_Status
                                }
                                ).Distinct().OrderByDescending(m=>m.Q_ID).ToList();
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

        public string TryGetCurrencySymbol(string ISOCurrencySymbol)
        {
            string symbol = CultureInfo
                .GetCultures(CultureTypes.AllCultures)
                .Where(c => !c.IsNeutralCulture)
                .Select(culture => {
                    try
                    {
                        return new RegionInfo(culture.Name);
                    }
                    catch
                    {
                        return null;
                    }
                })
                .Where(ri => ri != null && ri.ISOCurrencySymbol == ISOCurrencySymbol)
                .Select(ri => ri.CurrencySymbol)
                .FirstOrDefault();
            if (symbol != null)
                return symbol;
            return string.Empty;
        }
        public JsonResult GetEnquiryNotinQuotationList(bool delete)
        {

            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                {
                    int com_key = Convert.ToInt32(Session["CompanyKey"].ToString());
                    var data1 = dbcontext.tbl_QuotationHeader.Select(m => m.Q_EnquiryID).ToList();
                    dbcontext.Configuration.ProxyCreationEnabled = false;
                    var data = (from a in dbcontext.tbl_EnquiryHeader
                                join b in dbcontext.Tbl_Master_CompanyDetails on a.E_CustomerID equals b.COM_ID into comp
                                from x in comp
                                join d in dbcontext.Tbl_Master_User on a.E_SalesPerson equals d.USER_ID into user
                                from z in user
                                where a.DELETED == delete && a.COM_KEY == com_key && !data1.Contains(a.E_ID)
                                select new
                                {
                                    E_ID= a.E_ID,
                                    E_Date = a.E_Date,
                                    E_CustomerName =  x.COM_DISPLAYNAME,
                                    E_SalesPersonName = z.USER_NAME,
                                    E_Code= a.E_Code,
                                    E_ContactName = dbcontext.Tbl_Master_CompanyContacts.Where(m => m.CONTACT_ID == a.E_ContactID).Select(m => m.FIRST_NAME).FirstOrDefault() + " " + dbcontext.Tbl_Master_CompanyContacts.Where(m => m.CONTACT_ID == a.E_ContactID).Select(m => m.LAST_NAME).FirstOrDefault(),
                                }
                                ).Distinct().OrderByDescending(m => m.E_ID).ToList();
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
        public JsonResult GetQuotationNotinOrderList(bool delete)
        {

            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                {
                    int com_key = Convert.ToInt32(Session["CompanyKey"].ToString());
                    var data1 = dbcontext.Tbl_Master_Order.Select(m => m.SO_QuotationID).ToList();
                    dbcontext.Configuration.ProxyCreationEnabled = false;
                    var data = (from a in dbcontext.tbl_QuotationHeader
                                join b in dbcontext.Tbl_Master_CompanyDetails on a.Q_CustomerID equals b.COM_ID into comp
                                from x in comp
                                join d in dbcontext.Tbl_Master_User on a.Q_Sales_Person equals d.USER_ID into user
                                from z in user
                                where a.DELETED == delete && a.COM_KEY == com_key  && !data1.Contains(a.Q_ID)
                                select new
                                {
                                    Q_ID = a.Q_ID,
                                    Q_Date = a.Q_Date,
                                    Q_CustomerName = x.COM_DISPLAYNAME,
                                    Q_SalesPersonName = z.USER_NAME,
                                    Q_Code = a.Q_Code,
                                    Q_ApprovedStatus = a.Q_ApprovedStatus,
                                    Q_Type = a.Q_Type
                                    //E_ContactName = dbcontext.Tbl_Master_CompanyContacts.Where(m => m.CONTACT_ID == a.).Select(m => m.FIRST_NAME).FirstOrDefault() + " " + dbcontext.Tbl_Master_CompanyContacts.Where(m => m.CONTACT_ID == a.E_ContactID).Select(m => m.LAST_NAME).FirstOrDefault(),
                                }
                                ).Distinct().OrderByDescending(m => m.Q_ID).ToList();
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
        public JsonResult GetEnquiriesDetails(int id)
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
                            var Customer = (from a in dbcontext.tbl_EnquiryHeader
                                            where a.E_ID == id 
                                            select new
                                            {
                                                a.E_CustomerID,
                                                a.E_ID,
                                            });
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
        //Get the customer list 
        public JsonResult GetCustomers(int id)
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
                        if (id == 0)
                        {
                            var Customer = dbcontext.Tbl_Master_CompanyDetails.Where(m => m.Cust_Supp != 1 && m.COM_KEY == companykey && m.DELETED == false).OrderBy(m => m.COM_NAME).ToList();
                            var json = new JavaScriptSerializer().Serialize(Customer);
                            return Json(json, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            var cust = dbcontext.tbl_QuotationHeader.Single(m => m.Q_ID == id).Q_CustomerID;
                            var Customer = dbcontext.Tbl_Master_CompanyDetails.Where(m => m.Cust_Supp != 1 && m.COM_KEY == companykey && (m.DELETED == false || m.COM_ID == cust)).ToList();
                            var json = new JavaScriptSerializer().Serialize(Customer);
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

        //Get the Catalog List.
        public JsonResult GetCatlogList(int catalogId)
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
                            CatalogList = dbcontext.Tbl_ProductCatalog_Master.Where(m => m.COM_KEY == companykey && m.DELETED == false && m.PC_CatalogType == 1).OrderBy(m => m.PC_Name).ToList();
                        else
                            CatalogList = dbcontext.Tbl_ProductCatalog_Master.Where(m => m.COM_KEY == companykey && m.DELETED == false && m.PC_MasterID == catalogId && m.PC_CatalogType == 1).OrderBy(m => m.PC_Name).ToList();
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

        //Get the enquiry list
        public JsonResult GetEnquiries(int id,int custid,int type)
        {

            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                {
                    int comkey = Convert.ToInt32(Session["CompanyKey"].ToString());
                    dbcontext.Configuration.ProxyCreationEnabled = false;
                    if (id == 0)
                    {
                        var data = dbcontext.tbl_EnquiryHeader.Where(m => m.COM_KEY == comkey && m.DELETED == false && m.E_CustomerID == custid && m.E_Type == type).ToList();
                        var json = new JavaScriptSerializer().Serialize(data);
                        return Json(json, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        var enquiry = dbcontext.tbl_QuotationHeader.Single(a => a.Q_ID == id).Q_EnquiryID;
                        var data = dbcontext.tbl_EnquiryHeader.Where(m => m.COM_KEY == comkey && m.E_CustomerID == custid && m.E_Type == type && (m.DELETED == false || m.E_ID == enquiry)).ToList();
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
        //Get the sales person for customer
        public JsonResult GetSalesPersonForCustomer( int custid)
        {

            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                {
                    int comkey = Convert.ToInt32(Session["CompanyKey"].ToString());
                    dbcontext.Configuration.ProxyCreationEnabled = false;
                   var data = dbcontext.Tbl_Master_CompanyDetails.Where(m => m.COM_KEY == comkey && m.DELETED == false && m.COM_ID == custid).Select(n => n.COM_Sales_Person).ToList();
                        var json = new JavaScriptSerializer().Serialize(data);
                        return Json(data, JsonRequestBehavior.AllowGet);
                    
                    
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
        //Get the currency list
        public JsonResult GetCurrency()
        {

            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                {
                    int comkey = Convert.ToInt32(Session["CompanyKey"].ToString());
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
        //Get the contact list
        public JsonResult GetContacts(int id)
        {

            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                {
                    
                    dbcontext.Configuration.ProxyCreationEnabled = false;
                    var data = dbcontext.Tbl_Master_CompanyContacts.Where(m => m.COM_ID == id).ToList();
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
        //Get the sales person list
        public JsonResult GetSalesPerson(int id,int type)
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
                        var ObjSales_Org = dbcontext.Tbl_Sales_Organization.Where(m => m.COM_KEY == companykey && m.DELETED == false ).ToList();
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

                        if (id == 0)
                        {
                            var SalesPerson = dbcontext.Tbl_Master_User.Where(m => m.USER_ID > 0  && m.COM_KEY == companykey && m.DELETED == false).Distinct().OrderBy(a => a.DISPLAY_NAME).ToList();
                            var json = new JavaScriptSerializer().Serialize(SalesPerson);
                            return Json(json, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            var user = dbcontext.tbl_QuotationHeader.Single(m => m.Q_ID == id).Q_Sales_Person;
                            var SalesPerson = dbcontext.Tbl_Master_User.Where(m => m.USER_ID > 0 && m.COM_KEY == companykey && (m.DELETED == false  || m.USER_ID == user)).Distinct().OrderBy(a => a.DISPLAY_NAME).ToList();
                            var json = new JavaScriptSerializer().Serialize(SalesPerson);
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
        //Get the product list
        public JsonResult GetProducts(int id)
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
                        var Customer = dbcontext.Tbl_Product_Master.Where(m => m.COM_KEY == companykey && m.DELETED == false).OrderBy(m => m.P_ArticleNo).ToList();
                        var json = new JavaScriptSerializer().Serialize(Customer);
                        return Json(json, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        var product = dbcontext.tbl_QuotationDetails.Where(m => m.QD_PID == id).Select(a=>a.QD_ProductID);
                        var Customer = dbcontext.Tbl_Product_Master.Where(m => m.COM_KEY == companykey && (m.DELETED == false || product.Contains(m.P_ID))).OrderBy(m => m.P_ArticleNo).ToList();
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
        //Get the product details
        public JsonResult GetProductDetailsByID(int id,string catalogSelected)
        {

            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                {
                    dbcontext.Configuration.ProxyCreationEnabled = false;
                    var validPriceCatalog = (
                                               from PCL in dbcontext.Tbl_ProductCatalog
                                               join PCM in dbcontext.Tbl_ProductCatalog_Master on PCL.PC_CODE equals PCM.PC_CatalogueCode
                                               //join product in DbContext.Tbl_Product_Master on MUOM.LU_Code equals product.P_UOM
                                               where PCM.PC_CatalogueCode == catalogSelected && PCL.PRODUCT_ID == id
                                               select PCL.Summer_Price).FirstOrDefault();
                    if (validPriceCatalog == null)
                        validPriceCatalog = 0;

                    var data1 = (from a in dbcontext.Tbl_Product_Master
                                 join b in dbcontext.tbl_LookUp on a.P_UOM equals b.LU_Code into m
                                 from n in m.DefaultIfEmpty()
                                 where a.P_ID == id && n.LU_Type == 2
                                 select new
                                 {
                                     name = a.P_ShortName,
                                     uom = n.LU_Description,
                                     price = validPriceCatalog,
                                     //price = (priceType == 1 ? (dbcontext.Tbl_ProductCatalog.FirstOrDefault(m => m.ACTIVE_STATUS == true && m.PRODUCT_ID == a.P_ID).UNIT_PRICE1) : priceType == 2 ? (dbcontext.Tbl_ProductCatalog.FirstOrDefault(m => m.ACTIVE_STATUS == true && m.PRODUCT_ID == a.P_ID).UNIT_PRICE2) : priceType == 3 ? (dbcontext.Tbl_ProductCatalog.FirstOrDefault(m => m.ACTIVE_STATUS == true && m.PRODUCT_ID == a.P_ID).UNIT_PRICE3) : (dbcontext.Tbl_ProductCatalog.FirstOrDefault(m => m.ACTIVE_STATUS == true && m.PRODUCT_ID == a.P_ID).UNIT_PRICE4)) != null? (priceType == 1 ? (dbcontext.Tbl_ProductCatalog.FirstOrDefault(m => m.ACTIVE_STATUS == true && m.PRODUCT_ID == a.P_ID).UNIT_PRICE1) : priceType == 2 ? (dbcontext.Tbl_ProductCatalog.FirstOrDefault(m => m.ACTIVE_STATUS == true && m.PRODUCT_ID == a.P_ID).UNIT_PRICE2) : priceType == 3 ? (dbcontext.Tbl_ProductCatalog.FirstOrDefault(m => m.ACTIVE_STATUS == true && m.PRODUCT_ID == a.P_ID).UNIT_PRICE3) : (dbcontext.Tbl_ProductCatalog.FirstOrDefault(m => m.ACTIVE_STATUS == true && m.PRODUCT_ID == a.P_ID).UNIT_PRICE4)):0,
                                     quantity = a.P_PackingQuantity,
                                     CustomerDef=a.P_Remark1,
                                     Description=a.P_Description
                                 });
                    var json = new JavaScriptSerializer().Serialize(data1);
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
        //Get the payment terms
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
                        var payment = dbcontext.tbl_QuotationHeader.Single(m => m.Q_ID == id).Q_PaymentTerms;
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
        //Validate the data
        private string Validations(int Q_ID, string Q_Code, string Q_Date,int Q_Type, decimal Q_CustomerID, string Q_CurrencyCode, decimal Q_Sales_Person, string Q_DeliveryTerms, string Q_Enclosures, string Q_Quotedescription, string Q_specialdescription,string Q_CatalogId, string QuotationDetails)
        {
            if (!automanual && Q_Code == "")
            {
                return "Enter Quotation Code";
            }
            if (Q_Date == "")
            {
                return "Enter Date";
            }
            if (Q_CustomerID == 0)
            {
                return "Select Customer";
            }
            if(Q_CatalogId == "")
            {
                return "Select Product Catalog";
            }
            if (Q_CurrencyCode == "")
            {
                return "Select Currency Code";
            }
            if (Q_Sales_Person == 0)
            {
                return "Select Sales Person";
            }
            if (Q_DeliveryTerms == "")
            {
                return "Select DeliveryTerms";
            }
            if (!automanual)
            {
                if (Q_ID == 0)
                {
                    var count = dbcontext.tbl_QuotationHeader.Where(m => m.Q_Code == Q_Code).ToList().Count();
                    if (count > 0)
                    {
                        return "Quotation Code Already Exist";
                    }
                }
                else
                {
                    var count = dbcontext.tbl_QuotationHeader.Where(m => m.Q_ID != Q_ID && m.Q_Code == Q_Code).ToList().Count();
                    if (count > 0)
                    {
                        return "Quotation Code Already Exist";
                    }
                }
            }
            try
            {
                string[] ChildRow = QuotationDetails.Split('|');
                string[] tableColumns = new string[ChildRow.Length];
                for (int i = 0; i < ChildRow.Length - 1; i++)
                {
                    string[] ChildRecord = ChildRow[i].Split('}');
                    if(Q_Type==1)
                    {
                        if (tableColumns.Contains(Convert.ToDecimal(ChildRecord[0]).ToString()))
                        {
                            return "Product is repeated at row :" + (i + 1);
                        }
                    }
                    tableColumns[i] = Convert.ToDecimal(ChildRecord[0]).ToString();
                }
            }
            catch
            {
                return "Unable to process your request. Please verify product data.";
            }
            return "";
        }
        //Add the quoation
        [HttpPost]
        public JsonResult ET_Master_Quotation_Add(int Q_ID, string Q_Code,int Q_Type,int Q_PriceType , string Q_Date, decimal Q_CustomerID, decimal Q_EnquiryID, string Q_CurrencyCode, decimal Q_Sales_Person, int Q_PaymentTerms, string Q_DeliveryTerms, int Q_ValidityDays, decimal Q_Freight_Cost, string Q_Enclosures, decimal Q_TotalValue, string Q_Quotedescription, string Q_specialdescription,string Q_CatalogId, string QuotationDetails)
        {
            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                {
                    string valid = Validations(Q_ID, Q_Code, Q_Date,Q_Type, Q_CustomerID, Q_CurrencyCode, Q_Sales_Person, Q_DeliveryTerms, Q_Enclosures, Q_Quotedescription, Q_specialdescription, Q_CatalogId,QuotationDetails);
                    if (valid == "")
                    {
                        int approvalstatus;
                        decimal user = Convert.ToInt32(Session["UserID"].ToString());
                        var isApprovalRequired = dbcontext.Tbl_Document_Master.Single(m => m.auto_key == 8015).workflow_status;
                        if (isApprovalRequired == 1 && user != Q_Sales_Person)
                        {
                            approvalstatus = 0;
                            user = Q_Sales_Person;
                        }
                        else
                        {
                            approvalstatus = 1;
                        }
                        DateTime QDate = DateTime.ParseExact(Q_Date, "dd-MM-yyyy", null);
                        string Quocode;
                        tbl_QuotationHeader Objmc = new tbl_QuotationHeader()
                        {
                            Q_ID = Q_ID,
                            Q_Code = Q_Code,
                            Q_Type = Q_Type,
                            Q_PriceType = Q_PriceType,
                            Q_Date = QDate,
                            Q_CustomerID = Q_CustomerID,
                            Q_EnquiryID = Q_EnquiryID,
                            Q_CurrencyCode = Q_CurrencyCode,
                            Q_PaymentTerms = Q_PaymentTerms,
                            Q_DeliveryTerms = Q_DeliveryTerms,
                            Q_ValidityDays = Q_ValidityDays,
                            Q_Freight_Cost = Q_Freight_Cost,
                            Q_Enclosures = Q_Enclosures,
                            Q_TotalValue = Q_TotalValue,
                            Q_Sales_Person = Q_Sales_Person,
                            Q_Quotedescription = Q_Quotedescription,
                            Q_specialdescription = Q_specialdescription,
                            Q_ApprovedStatus = approvalstatus,
                            Q_Approver = user,
                            CREATED_BY = Convert.ToInt32(Session["UserID"].ToString()),
                            CREATED_DATE = DateTime.Now,
                            DELETED = false,
                            Q_Status=false,
                            Q_CatalogId = Q_CatalogId,
                            COM_KEY = Convert.ToInt64(Session["CompanyKey"])
                        };
                        decimal data = 0;
                        var context = new EntityClasses();
                        var transaction = context.Database.BeginTransaction();
                        bool success = true;
                        try
                        {
                            if (Objmc.Q_ID == 0)
                            {
                                context.tbl_QuotationHeader.Add(Objmc);
                                if (context.SaveChanges() == 0)
                                {
                                    success = false;
                                }
                                if (automanual == true)
                                {
                                    int len = 10 - (prefix + Objmc.Q_ID).Length;
                                    string code = prefix + new String('0', len) + Objmc.Q_ID;
                                    tbl_QuotationHeader _tbl_QuotationHeader = context.tbl_QuotationHeader.Single(m => m.Q_ID == Objmc.Q_ID);
                                    {
                                        _tbl_QuotationHeader.Q_Code = code;
                                    };
                                    if (context.SaveChanges() == 0)
                                    {
                                        success = false;
                                    }
                                }
                                Quocode = Objmc.Q_Code;
                                data = Objmc.Q_ID;

                            }
                            else
                            {
                                tbl_QuotationHeader Obj_tbl_QuotationHeader = context.tbl_QuotationHeader.Single(m => m.Q_ID == Objmc.Q_ID);
                                {
                                    Obj_tbl_QuotationHeader.Q_ID = Q_ID;
                                    Obj_tbl_QuotationHeader.Q_Code = Q_Code;
                                    Obj_tbl_QuotationHeader.Q_Type = Q_Type;
                                    Obj_tbl_QuotationHeader.Q_PriceType = Q_PriceType;
                                    Obj_tbl_QuotationHeader.Q_Date = QDate;
                                    Obj_tbl_QuotationHeader.Q_CustomerID = Q_CustomerID;
                                    Obj_tbl_QuotationHeader.Q_EnquiryID = Q_EnquiryID;
                                    Obj_tbl_QuotationHeader.Q_CurrencyCode = Q_CurrencyCode;
                                    Obj_tbl_QuotationHeader.Q_PaymentTerms = Q_PaymentTerms;
                                    Obj_tbl_QuotationHeader.Q_DeliveryTerms = Q_DeliveryTerms;
                                    Obj_tbl_QuotationHeader.Q_ValidityDays = Q_ValidityDays;
                                    Obj_tbl_QuotationHeader.Q_Freight_Cost = Q_Freight_Cost;
                                    Obj_tbl_QuotationHeader.Q_Enclosures = Q_Enclosures;
                                    Obj_tbl_QuotationHeader.Q_TotalValue = Q_TotalValue;
                                    Obj_tbl_QuotationHeader.Q_Sales_Person = Q_Sales_Person;
                                    Obj_tbl_QuotationHeader.Q_Quotedescription = Q_Quotedescription;
                                    Obj_tbl_QuotationHeader.Q_specialdescription = Q_specialdescription;
                                    Obj_tbl_QuotationHeader.Q_ApprovedStatus = approvalstatus;
                                    Obj_tbl_QuotationHeader.Q_Approver = user;
                                    Obj_tbl_QuotationHeader.LAST_UPDATED_BY = Convert.ToInt32(Session["UserID"].ToString());
                                    Obj_tbl_QuotationHeader.LAST_UPDATED_DATE = DateTime.Now;
                                    Obj_tbl_QuotationHeader.Q_CatalogId = Q_CatalogId;
                                };
                                if (context.SaveChanges() == 0)
                                {
                                    success = false;
                                }
                                Quocode = Obj_tbl_QuotationHeader.Q_Code;
                                data = Obj_tbl_QuotationHeader.Q_ID;
                            }

                            // Delete previous contact data
                            tbl_QuotationDetails objdeletecontact = new tbl_QuotationDetails();
                            context.tbl_QuotationDetails.RemoveRange(context.tbl_QuotationDetails.Where(m => m.QD_PID == Objmc.Q_ID));
                            context.SaveChanges();

                            // Insert new contacts data
                            string[] ChildRow = QuotationDetails.Split('|');
                            for (int i = 0; i < ChildRow.Length - 1; i++)
                            {
                                string[] ChildRecord = ChildRow[i].Split('}');
                                tbl_QuotationDetails objquotationdetails = new tbl_QuotationDetails()
                                {
                                    QD_PID = Objmc.Q_ID,
                                    QD_ProductID = Convert.ToDecimal(ChildRecord[0]),
                                    QD_UOM = ChildRecord[1],
                                    //QD_CatalogPriceType = Convert.ToInt32(ChildRecord[2]),
                                    QD_Unit_Price = Convert.ToDecimal(ChildRecord[2]),
                                    QD_Quantity = Convert.ToDecimal(ChildRecord[3]),
                                    QD_Amount = Convert.ToDecimal(ChildRecord[4]),
                                    QD_Description = ChildRecord[5]
                                };
                                context.tbl_QuotationDetails.Add(objquotationdetails);
                            }
                            if (context.SaveChanges() != ChildRow.Length - 1)
                            {
                                success = false;
                            }
                            if (success)
                            {
                                transaction.Commit();
                            }
                            else
                            {
                                data = 0;
                                transaction.Rollback();
                            }
                        }
                        catch (Exception ex)
                        {
                            data = 0;
                            transaction.Rollback();
                            throw ex;
                        }
                        finally
                        {
                            transaction.Dispose();
                            context.Dispose();
                        }
                        //Send Mail For Approver
                        try
                        {
                            if (isApprovalRequired == 1 && Convert.ToInt32(Session["UserID"].ToString()) != Q_Sales_Person)
                            {
                                var uid = Convert.ToInt32(Session["UserID"].ToString());
                                var settings = dbcontext.Tbl_MailSettings.Where(a => a.MS_UserId == uid);
                                if (settings.Count() == 1)
                                {
                                    string usermail = settings.Select(a => a.MS_EmailID).FirstOrDefault();
                                    string custmail = dbcontext.Tbl_Master_CompanyDetails.Single(a => a.COM_ID == Q_CustomerID).COM_EMAIL;
                                    MailMessage mm = new MailMessage(usermail, custmail);
                                    mm.Subject = "Regarding Approval of Quotation";
                                    string newLine = Environment.NewLine;
                                    string body = "Dear Sir/Madam, ";
                                    body = body + newLine;
                                    body = body + newLine;
                                    body = body + "Quotation with code "+ Quocode + " has been created and send for your approval. With your approval the Quotation will proceed to next step.";
                                    body = body + newLine;
                                    body = body + "You can approve the Quotation login to Indra application and follow the below steps.";
                                    body = body + newLine;
                                    body = body + "Step 1                    Login to Indra Application.";
                                    body = body + newLine;
                                    body = body + "Step 2                    Click on My Approvals.";
                                    body = body + newLine;
                                    body = body + "Step 3                    View all pending Approvals.";
                                    body = body + newLine;
                                    body = body + "Step 4                    Click Approve or Reject button.";
                                    body = body + newLine;
                                    body = body + newLine;
                                    body = body + "Regards";
                                    body = body + newLine;
                                    body = body + newLine;
                                    body = body + Session["UserName"].ToString();
                                    mm.Body = string.Format(body);
                                    SmtpClient smtp = new SmtpClient();
                                    smtp.Host = settings.Select(a => a.MS_OutGoingHostName).FirstOrDefault();
                                    NetworkCredential NetworkCred = new NetworkCredential();
                                    NetworkCred.UserName = settings.Select(a => a.MS_EmailID).FirstOrDefault();
                                    NetworkCred.Password = settings.Select(a => a.MS_Password).FirstOrDefault();
                                    smtp.UseDefaultCredentials = true;
                                    smtp.Credentials = NetworkCred;
                                    smtp.Port = settings.Select(a => a.MS_OutGoingPort).FirstOrDefault();
                                    smtp.Send(mm);
                                }
                            }
                        }
                        catch { }
                        var json = "Success:"+ Quocode;
                        if (data == 0)
                        {
                            json = "Failed";
                        }
                        else
                        {
                            objLOG.log_dockey = "8015";
                            objLOG.log_userid = Session["UserID"].ToString();
                            objLOG.log_recordkey = data.ToString();
                            if (Q_ID == 0)
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
                //catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
                //{
                //    Exception raise = dbEx;
                //    foreach (var validationErrors in dbEx.EntityValidationErrors)
                //    {
                //        foreach (var validationError in validationErrors.ValidationErrors)
                //        {
                //            string message = string.Format("{0}:{1}",
                //                validationErrors.Entry.Entity.ToString(),
                //                validationError.ErrorMessage);
                //            // raise a new exception nesting  
                //            // the current instance as InnerException  
                //            raise = new InvalidOperationException(message, raise);
                //        }
                //    }
                //    throw raise;
                //}
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
        //View: popup view
        public ActionResult ET_Master_Quotation_View(int id)
        {
            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                {
                    dbcontext.Configuration.ProxyCreationEnabled = false;
                    var data1 = (from a in dbcontext.tbl_QuotationHeader
                                join b in dbcontext.Tbl_Master_CompanyDetails on a.Q_CustomerID equals b.COM_ID into comp
                                from x in comp
                                join d in dbcontext.Tbl_Master_User on a.Q_Sales_Person equals d.USER_ID into user
                                from z in user
                                join w in dbcontext.tbl_EnquiryHeader on a.Q_EnquiryID equals w.E_ID into p
                                from enq in p.DefaultIfEmpty()
                                where a.Q_ID == id
                                select new Quotation_CM
                                {
                                    Q_ID = a.Q_ID,
                                    Q_Code = a.Q_Code,
                                    Q_Type = a.Q_Type,
                                    Q_PriceType = a.Q_PriceType,
                                    Q_Date = a.Q_Date,
                                    Q_CustomerName = x.COM_NAME,
                                    Q_EnquiryCode = enq.E_Code,
                                    Q_SalesPersonName = z.DISPLAY_NAME,
                                    Q_CurrencyCode = a.Q_CurrencyCode,
                                    Q_PaymentTerms = a.Q_PaymentTerms,
                                    Q_DeliveryTerms = a.Q_DeliveryTerms,
                                    Q_ValidityDays = a.Q_ValidityDays,
                                    Q_Freight_Cost = a.Q_Freight_Cost,
                                    Q_Enclosures = a.Q_Enclosures,
                                    Q_TotalValue = a.Q_TotalValue??0,
                                    Q_Quotedescription = a.Q_Quotedescription,
                                    Q_specialdescription = a.Q_specialdescription,
                                    Q_ApprovedStatus = a.Q_ApprovedStatus

                                }).ToList();
                    var data2 = (from c in dbcontext.tbl_QuotationDetails
                                 join a in dbcontext.Tbl_Product_Master on c.QD_ProductID equals a.P_ID
                                 join b in dbcontext.tbl_LookUp on a.P_UOM equals b.LU_Code
                                 into m
                                 from n in m.DefaultIfEmpty()
                                 where c.QD_PID == id && n.LU_Type == 2
                                 select new Quotation_CM
                                 {
                                     QD_ArticleNo = a.P_ArticleNo,
                                     QD_ProductName = a.P_ShortName,
                                     QD_UOM = n.LU_Description,
                                     QD_Unit_Price = c.QD_Unit_Price??0,
                                     QD_Quantity = c.QD_Quantity??0,
                                     QD_Amount = c.QD_Amount??0,
                                     QD_Description = c.QD_Description,
                                     QD_CatalogPriceType = c.QD_CatalogPriceType
                                 }).ToList();
                    Quotation_View_CM obj = new Quotation_View_CM();
                    obj.QHeader = data1;
                    obj.QChild = data2;
                    return PartialView("/Views/Sales/ET_Sales_Quotation/ET_Sales_Quotation_View.cshtml", obj);
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
        //Print the quation
        public ActionResult ET_Master_Quotation_Print(int id, string lang)
        {
            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                {
                    dbcontext.Configuration.ProxyCreationEnabled = false;
                    int com_key = Convert.ToInt32(Session["Companykey"]);
                    var data1 = (from a in dbcontext.tbl_QuotationHeader
                                 join b in dbcontext.Tbl_Master_CompanyDetails on a.Q_CustomerID equals b.COM_ID into comp
                                 from x in comp
                                 join d in dbcontext.Tbl_Master_User on a.Q_Sales_Person equals d.USER_ID into user
                                 from z in user
                                 join w in dbcontext.tbl_EnquiryHeader on a.Q_EnquiryID equals w.E_ID into p
                                 from enq in p.DefaultIfEmpty()
                                 where a.Q_ID == id
                                 select new Quotation_CM
                                 {
                                     Q_ID = a.Q_ID,
                                     Q_Code = a.Q_Code,
                                     Q_Type = a.Q_Type,
                                     Q_Date = a.Q_Date,
                                     Q_CustomerName = x.COM_NAME,
                                     CompanyCode = x.COM_CODE,
                                     Street = x.COM_STREET,
                                     CityState = (x.COM_CITY + ", " + x.COM_STATE),
                                     CountryZip = ((dbcontext.locations.Where(a => a.location_id == x.COM_COUNTRY).Select(a => a.name).FirstOrDefault()) + ", " + (x.COM_ZIP)),
                                     VatPer = (dbcontext.Tbl_SystemSetUp.Where(a => a.COMPANY_ID == com_key).Select(a => a.TAX).FirstOrDefault()),
                                     imgurl = (dbcontext.Tbl_SystemSetUp.Where(a => a.COMPANY_ID == com_key).Select(a => a.COMPANY_LOGO).FirstOrDefault()),
                                     SysCompany = (dbcontext.Tbl_SystemSetUp.Where(a => a.COMPANY_ID == com_key).Select(a => a.COMPANY_NAME).FirstOrDefault()),
                                     Q_EnquiryCode = enq.E_Code,
                                     Q_SalesPersonName = z.DISPLAY_NAME,
                                     Q_CurrencyCode = a.Q_CurrencyCode,
                                     PaymentTerms = (dbcontext.Tbl_Payment_Terms.Where(m=> m.PT_ID == a.Q_PaymentTerms).Select(m=>m.PT_Name).FirstOrDefault()),
                                     PaymentTermsDescription = (dbcontext.Tbl_Payment_Terms.Where(m => m.PT_ID == a.Q_PaymentTerms).Select(m => m.PT_Details).FirstOrDefault()),
                                     Q_DeliveryTerms = a.Q_DeliveryTerms,
                                     Q_ValidityDays = a.Q_ValidityDays,
                                     Q_Freight_Cost = a.Q_Freight_Cost,
                                     Q_Enclosures = a.Q_Enclosures,
                                     Q_TotalValue = a.Q_TotalValue??0,
                                     Q_Quotedescription = a.Q_Quotedescription,
                                     Q_specialdescription = a.Q_specialdescription,
                                     Q_ApprovedStatus = a.Q_ApprovedStatus

                                 }).ToList();
                    var data2 = (from c in dbcontext.tbl_QuotationDetails
                                 join a in dbcontext.Tbl_Product_Master on c.QD_ProductID equals a.P_ID
                                 join b in dbcontext.tbl_LookUp on a.P_UOM equals b.LU_Code
                                 into m
                                 from n in m.DefaultIfEmpty()
                                 where c.QD_PID == id && n.LU_Type == 2
                                 select new Quotation_CM
                                 {
                                     QD_ArticleNo = a.P_ArticleNo,
                                     QD_ProductName = a.P_Description,
                                     QD_UOM = n.LU_Description,
                                     QD_UOMCode = n.LU_Code,
                                     QD_Unit_Price = c.QD_Unit_Price??0,
                                     QD_Quantity = c.QD_Quantity??0,
                                     QD_Amount = c.QD_Amount??0,
                                     QD_Description = c.QD_Description,
                                     QD_CatalogPriceType = c.QD_CatalogPriceType
                                 }).ToList();
                    Quotation_View_CM obj = new Quotation_View_CM();
                    obj.QHeader = data1;
                    obj.QChild = data2;
                    if (lang == "E")
                        return PartialView("/Views/Sales/ET_Sales_Quotation/ET_Sales_Quotation_Print.cshtml", obj);
                    else
                        return PartialView("/Views/Sales/ET_Sales_Quotation/ET_Sales_Quotation_Print_German.cshtml", obj);
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
        public ActionResult ET_Master_QuotationPrint(int id, string lang)
        {
            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                {
                    dbcontext.Configuration.ProxyCreationEnabled = false;
                    int com_key = Convert.ToInt32(Session["Companykey"]);
                    var data1 = (from a in dbcontext.tbl_QuotationHeader
                                 join b in dbcontext.Tbl_Master_CompanyDetails on a.Q_CustomerID equals b.COM_ID into comp
                                 from x in comp
                                 join d in dbcontext.Tbl_Master_User on a.Q_Sales_Person equals d.USER_ID into user
                                 from z in user
                                 join w in dbcontext.tbl_EnquiryHeader on a.Q_EnquiryID equals w.E_ID into p
                                 from enq in p.DefaultIfEmpty()
                                 where a.Q_ID == id
                                 select new Quotation_CM
                                 {
                                     Q_ID = a.Q_ID,
                                     Q_Code = a.Q_Code,
                                     Q_Type = a.Q_Type,
                                     Q_Date = a.Q_Date,
                                     Q_CustomerName = x.COM_NAME,
                                     CompanyCode = x.COM_CODE,
                                     Street = x.COM_STREET,
                                     CityState = (x.COM_CITY + ", " + x.COM_STATE),
                                     CountryZip = ((dbcontext.locations.Where(a => a.location_id == x.COM_COUNTRY).Select(a => a.name).FirstOrDefault()) + ", " + (x.COM_ZIP)),
                                     VatPer = (dbcontext.Tbl_SystemSetUp.Where(a => a.COMPANY_ID == com_key).Select(a => a.TAX).FirstOrDefault()),
                                     imgurl = (dbcontext.Tbl_SystemSetUp.Where(a => a.COMPANY_ID == com_key).Select(a => a.COMPANY_LOGO).FirstOrDefault()),
                                     SysCompany = (dbcontext.Tbl_SystemSetUp.Where(a => a.COMPANY_ID == com_key).Select(a => a.COMPANY_NAME).FirstOrDefault()),
                                     Q_EnquiryCode = enq.E_Code,
                                     Q_SalesPersonName = z.DISPLAY_NAME,
                                     Q_CurrencyCode = a.Q_CurrencyCode,
                                     PaymentTerms = (dbcontext.Tbl_Payment_Terms.Where(m => m.PT_ID == a.Q_PaymentTerms).Select(m => m.PT_Name).FirstOrDefault()),
                                     PaymentTermsDescription = (dbcontext.Tbl_Payment_Terms.Where(m => m.PT_ID == a.Q_PaymentTerms).Select(m => m.PT_Details).FirstOrDefault()),
                                     Q_DeliveryTerms = a.Q_DeliveryTerms,
                                     Q_ValidityDays = a.Q_ValidityDays,
                                     Q_Freight_Cost = a.Q_Freight_Cost,
                                     Q_Enclosures = a.Q_Enclosures,
                                     Q_TotalValue = a.Q_TotalValue ?? 0,
                                     Q_Quotedescription = a.Q_Quotedescription,
                                     Q_specialdescription = a.Q_specialdescription,
                                     Q_ApprovedStatus = a.Q_ApprovedStatus,
                                     zipCode=x.COM_COUNTRY

                                 }).ToList();
                    var data2 = (from c in dbcontext.tbl_QuotationDetails
                                 join a in dbcontext.Tbl_Product_Master on c.QD_ProductID equals a.P_ID
                                 join b in dbcontext.tbl_LookUp on a.P_UOM equals b.LU_Code
                                 into m
                                 from n in m.DefaultIfEmpty()
                                 where c.QD_PID == id && n.LU_Type == 2
                                 select new Quotation_CM
                                 {
                                     QD_ArticleNo = a.P_ArticleNo,
                                     QD_ProductName = a.P_Name,
                                     QD_UOM = n.LU_Description,
                                     QD_UOMCode = n.LU_Code,
                                     QD_Unit_Price = c.QD_Unit_Price ?? 0,
                                     QD_Quantity = c.QD_Quantity ?? 0,
                                     QD_Amount = c.QD_Amount ?? 0,
                                     QD_Description = c.QD_Description,
                                     QD_CatalogPriceType = c.QD_CatalogPriceType
                                 }).ToList();
                    string path = "";
                    if (lang == "E")
                    {
                        for (int i = 0; i < data1.Count; i++)
                        {

                            var doc1 = new iTextSharp.text.Document(PageSize.A4, 30, 25, 130, 90);
                            string subPath = "~/Sales/PDFList/Quotation/" + data1[i].Q_Code + "/";
                            bool exists = System.IO.Directory.Exists(HttpContext.Server.MapPath(subPath));
                            if (!exists)
                                System.IO.Directory.CreateDirectory(HttpContext.Server.MapPath(subPath));
                            var output = new FileStream(Server.MapPath(subPath + data1[i].Q_Code + ".pdf"), FileMode.Create);
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
                            Title.AddElement(new Paragraph(new Chunk("Eurotextiles GmbH & Co. KG, Mammolshainer Weg 14, 61462 Königstein", fontsmall1)));
                            Title.VerticalAlignment = 2;
                            Title.PaddingTop = 1.0f;
                            Title.PaddingBottom = 3.0f;
                            Title.PaddingLeft = 30.0f;
                            table1.AddCell(Title);
                            Paragraph pg2 = new Paragraph();
                            Phrase phraseConstant2 = new Phrase("" + data1[i].Q_CustomerName + "\n", font);
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
                            float[] widthsvalforcus = new float[] { 55f,20f, 25f };
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
                            Phrase cuscodeval = new Phrase("" + data1[i].CompanyCode + "", font);
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
                            Phrase cuscodeval1 = new Phrase("" + data1[i].Q_SalesPersonName + "", font);
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
                            Phrase cuscodeval2 = new Phrase("" + data1[i].Q_Date.ToString("dd-MM-yyyy") + "", font);
                            PdfPCell cell26Processby2 = new PdfPCell(cuscodeval2);
                            cell26Processby2.Border = 0;
                            cell26Processby2.HorizontalAlignment = 2;
                            cell26Processby2.PaddingBottom = 2.5f;
                            table2.AddCell(cell26Processby2);
                            doc1.Add(table2);

                            PdfPTable contenttable = new PdfPTable(1);
                            contenttable.TotalWidth = 100f;
                            contenttable.WidthPercentage = 100f;
                            Paragraph pp1con = new Paragraph();
                            Phrase phar1con = new Phrase("", Fontsmaller1);
                            pp1con.Add(phar1con);
                            PdfPCell cell29Con = new PdfPCell(pp1con);

                            cell29Con.PaddingTop = 4.5f;
                            cell29Con.HorizontalAlignment = 1;
                            cell29Con.PaddingBottom = 5.5f;
                            cell29Con.Border = 0;
                            contenttable.AddCell(cell29Con);
                            PdfPCell cell29 = new PdfPCell();
                            cell29.AddElement(new Paragraph(new Chunk("Customer Offer No:" + data1[i].Q_Code + "", Fontsmaller)));
                            cell29.PaddingTop = 6.5f;
                            cell29.PaddingBottom = 20.5f;
                            cell29.Border = 0;
                            contenttable.AddCell(cell29);
                            doc1.Add(contenttable);
                            PdfPTable table4 = new PdfPTable(6);
                            float[] widths1 = new float[] { 8f, 12f, 10f, 30f, 20f, 20f };
                            table4.TotalWidth = 100f;
                            table4.WidthPercentage = 100f;
                            table4.HeaderRows = 1;
                            table4.SetWidths(widths1);

                            Phrase phraseConstantde1 = new Phrase("S.No", Fontsmaller);
                            PdfPCell cell41 = new PdfPCell(phraseConstantde1);

                            cell41.HorizontalAlignment = 1;
                            cell41.PaddingTop = 2.5f;
                            cell41.PaddingBottom = 2.5f;

                            table4.AddCell(cell41);
                            Phrase phraseConstantde2 = new Phrase("Quantity", Fontsmaller);
                            PdfPCell cell42 = new PdfPCell(phraseConstantde2);
                            cell42.Colspan = 2;

                            cell42.HorizontalAlignment = 1;
                            cell42.PaddingTop = 2.5f;
                            cell42.PaddingBottom = 2.5f;

                            table4.AddCell(cell42);
                            Phrase phraseConstantde3 = new Phrase("Product Description", Fontsmaller);
                            PdfPCell cell421 = new PdfPCell(phraseConstantde3);
                            cell421.HorizontalAlignment = 1;

                            cell421.PaddingTop = 2.5f;
                            cell421.PaddingBottom = 2.5f;

                            table4.AddCell(cell421);
                            Phrase phraseConstantde4 = new Phrase("Price(€)", Fontsmaller);
                            PdfPCell cell431 = new PdfPCell(phraseConstantde4);
                            cell431.HorizontalAlignment = 1;


                            cell431.PaddingTop = 2.5f;
                            cell431.PaddingBottom = 2.5f;

                            table4.AddCell(cell431);

                            Phrase phraseConstantde5 = new Phrase("Total(€)", Fontsmaller);
                            PdfPCell cell43 = new PdfPCell(phraseConstantde5);
                            cell43.HorizontalAlignment = 1;
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
                                Phrase qtyphar = new Phrase("" + Convert.ToDecimal(data2[j].QD_Quantity).ToString("N2", CultureInfo.CreateSpecificCulture("de-DE")) + "\n", font);
                                qtypp.Add(qtyphar);
                                PdfPCell cell52 = new PdfPCell(qtypp);
                                cell52.HorizontalAlignment = 2;
                                //  cell52.PaddingTop = 2.5f;
                                // cell52.PaddingBottom = 2.5f;

                                table4.AddCell(cell52);
                                Paragraph uompp = new Paragraph();
                                Phrase uomphar = new Phrase("" + data2[j].QD_UOM + "\n", font);
                                uompp.Add(uomphar);
                                PdfPCell uomcell = new PdfPCell(uompp);

                                // uomcell.AddElement(new Paragraph(new Chunk("" + data2[j].UOM_NAME + "", font)));
                                uomcell.HorizontalAlignment = 0;
                                // uomcell.PaddingTop = 2.5f;
                                // uomcell.PaddingBottom = 2.5f;

                                table4.AddCell(uomcell);


                                Paragraph Productpp = new Paragraph();
                                Phrase Productphar = new Phrase("" + data2[j].QD_ProductName + "\n", font);
                                Phrase Productphar1 = new Phrase("" + data2[j].QD_Description + "\n", font);
                                Productpp.Add(Productphar);
                                Productpp.Add(Productphar1);

                                PdfPCell cell53 = new PdfPCell(Productpp);
                                cell53.HorizontalAlignment = 0;
                                table4.AddCell(cell53);

                                Paragraph pp = new Paragraph();
                                Phrase phar = new Phrase("" + Convert.ToDecimal(data2[j].QD_Unit_Price).ToString("c2", CultureInfo.CreateSpecificCulture("de-DE")) + "\n", font);
                                pp.Add(phar);
                                PdfPCell cell541 = new PdfPCell(pp);
                                cell541.HorizontalAlignment = 2;
                                cell541.PaddingTop = 2.5f;
                                cell541.PaddingBottom = 2.5f;

                                table4.AddCell(cell541);
                                Paragraph pp12 = new Paragraph();
                                Phrase phar12 = new Phrase("" + (Convert.ToDecimal(data2[j].QD_Unit_Price) * Convert.ToDecimal(data2[j].QD_Quantity)).ToString("c2", CultureInfo.CreateSpecificCulture("de-DE")) + "\n", font);
                                pp12.Add(phar12);
                                PdfPCell cell54 = new PdfPCell(pp12);
                                cell54.HorizontalAlignment = 2;
                                cell54.PaddingTop = 2.5f;
                                cell54.PaddingBottom = 2.5f;

                                table4.AddCell(cell54);
                                total = total + ((Convert.ToDecimal(data2[j].QD_Unit_Price) * Convert.ToDecimal(data2[j].QD_Quantity)));


                            }
                                PdfPCell NetValCell = new PdfPCell();

                                NetValCell.AddElement(new Paragraph(new Chunk("Net Value", font)));
                                NetValCell.Colspan = 5;
                                NetValCell.PaddingTop = 2.5f;
                                NetValCell.PaddingBottom = 2.5f;

                                table4.AddCell(NetValCell);
                                Paragraph pp1 = new Paragraph();
                                Phrase phar1 = new Phrase("" + Convert.ToDecimal(total).ToString("c2", CultureInfo.CreateSpecificCulture("de-DE")) + "\n", font);
                                pp1.Add(phar1);
                                PdfPCell NetValCell1 = new PdfPCell(pp1);
                                NetValCell1.Colspan = 5;
                                NetValCell1.HorizontalAlignment = 2;
                                NetValCell1.VerticalAlignment = 2;
                                NetValCell1.PaddingTop = 2.5f;
                                NetValCell1.PaddingBottom = 2.5f;

                                table4.AddCell(NetValCell1);

                                if (data1[i].zipCode == 82 && data1[i].Q_Type!=1)
                                {
                                    PdfPCell inccell = new PdfPCell();

                                    inccell.AddElement(new Paragraph(new Chunk(Convert.ToDecimal(data1[i].VatPer).ToString("0") + "% VAT", font)));
                                    inccell.Colspan = 5;
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
                                    inccell.AddElement(new Paragraph(new Chunk(Convert.ToDecimal(data1[i].VatPer).ToString("0") + "% VAT", font)));
                                    inccell.Colspan = 5;
                                    inccell.PaddingTop = 2.5f;
                                    inccell.PaddingBottom = 2.5f;

                                    table4.AddCell(inccell);
                                    Paragraph pp1inc = new Paragraph();
                                    Phrase phar1inc = new Phrase("" + (Convert.ToDecimal(total * data1[i].VatPer / 100).ToString("C2", CultureInfo.CreateSpecificCulture("de-DE"))) + "\n", font);
                                    pp1inc.Add(phar1inc);
                                    PdfPCell inccell1 = new PdfPCell(pp1inc);
                                    inccell1.HorizontalAlignment = 2;
                                    inccell1.PaddingTop = 2.5f;
                                    inccell1.PaddingBottom = 2.5f;

                                    table4.AddCell(inccell1);

                                }

                                PdfPCell vataddcell = new PdfPCell();

                                vataddcell.AddElement(new Paragraph(new Chunk("Total", font)));
                                vataddcell.Colspan = 5;
                                vataddcell.PaddingTop = 2.5f;
                                vataddcell.PaddingBottom = 2.5f;

                                table4.AddCell(vataddcell);
                                if (data1[i].zipCode == 82 && data1[i].Q_Type != 1)
                                {
                                    Paragraph pp1vat = new Paragraph();
                                    Phrase phar1vat = new Phrase("" + ((Convert.ToDecimal(total + ((total * data1[i].VatPer / 100)))).ToString("C2", CultureInfo.CreateSpecificCulture("de-DE"))) + "\n", Fontsmaller);
                                    pp1vat.Add(phar1vat);
                                    PdfPCell vataddcell1 = new PdfPCell(pp1vat);
                                    vataddcell1.HorizontalAlignment = 2;
                                    vataddcell1.Colspan = 6;
                                    vataddcell1.PaddingTop = 2.5f;
                                    vataddcell1.PaddingBottom = 2.5f;

                                    table4.AddCell(vataddcell1);
                                }
                                else
                                {
                                    Paragraph pp1vat = new Paragraph();
                                    Phrase phar1vat = new Phrase("" + ((Convert.ToDecimal(total + ((total * data1[i].VatPer / 100)))).ToString("C2", CultureInfo.CreateSpecificCulture("de-DE"))) + "\n", Fontsmaller);
                                    pp1vat.Add(phar1vat);
                                    PdfPCell vataddcell1 = new PdfPCell(pp1vat);
                                    vataddcell1.HorizontalAlignment = 2;
                                    vataddcell1.Colspan = 5;
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

                                if (data1[i].PaymentTermsDescription != string.Empty)
                                {
                                    PdfPCell cell33 = new PdfPCell();
                                    cell33.AddElement(new Paragraph(new Chunk("Payment Terms: " + data1[i].PaymentTermsDescription + "", font)));
                                    cell33.PaddingTop = 2.5f;
                                    cell33.PaddingBottom = 2.5f;
                                    cell33.Border = 0;
                                    table7.AddCell(cell33);
                                }

                                if (data1[i].Q_DeliveryTerms != string.Empty)
                                {
                                    PdfPCell cell33d = new PdfPCell();
                                    cell33d.AddElement(new Paragraph(new Chunk("Delivery Terms: " + data1[i].Q_DeliveryTerms + "", font)));
                                    cell33d.PaddingTop = 2.5f;
                                    cell33d.PaddingBottom = 2.5f;
                                    cell33d.Border = 0;
                                    table7.AddCell(cell33d);
                                }

                                PdfPCell delitercell = new PdfPCell();
                                delitercell.PaddingTop = 2.5f;
                                delitercell.PaddingBottom = 2.5f;
                                delitercell.Border = 0;
                                table7.AddCell(delitercell);

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

                                //PdfPCell line2 = new PdfPCell();
                                //line2.AddElement(new Paragraph(new Chunk("Any quality/quantity variations have to be notified to us before using the goods but not later than 7 days on receipt of the goods.", font)));
                                //line2.PaddingTop = 2.5f;
                                //line2.PaddingBottom = 2.5f;
                                //line2.Border = 0;
                                //table7.AddCell(line2);

                                //PdfPCell line3 = new PdfPCell();
                                //line3.AddElement(new Paragraph(new Chunk("No claims will be accepted after washing/using the goods.", font)));
                                //line3.PaddingTop = 2.5f;
                                //line3.PaddingBottom = 2.5f;
                                //line3.Border = 0;
                                //table7.AddCell(line3);
                                doc1.Add(table7);
                                doc1.Close();
                            }
                    }
                    if (lang == "G")
                    {
                        for (int i = 0; i < data1.Count; i++)
                        {

                            var doc1 = new iTextSharp.text.Document(PageSize.A4, 20, 20, 130, 90);
                            string subPath = "~/Sales/PDFList/Quotation/" + data1[i].Q_Code + "/";
                            bool exists = System.IO.Directory.Exists(HttpContext.Server.MapPath(subPath));
                            if (!exists)
                                System.IO.Directory.CreateDirectory(HttpContext.Server.MapPath(subPath));
                            var output = new FileStream(Server.MapPath(subPath + data1[i].Q_Code + ".pdf"), FileMode.Create);
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
                            Phrase phraseConstant2 = new Phrase("" + data1[i].Q_CustomerName + "\n", font);
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
                            Phrase cuscode = new Phrase("Kunden Nr:", font);
                            PdfPCell cell26 = new PdfPCell(cuscode);
                            cell26.Border = 0;
                            cell26.HorizontalAlignment = 0;
                            cell26.PaddingBottom = 2.5f;
                            table2.AddCell(cell26);
                            Phrase cuscodeval = new Phrase("" + data1[i].CompanyCode + "", font);
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
                            Phrase Processby = new Phrase("Bearbeitet Von:", font);
                            PdfPCell cell26Processby = new PdfPCell(Processby);
                            cell26Processby.Border = 0;
                            cell26Processby.HorizontalAlignment = 0;
                            cell26Processby.PaddingBottom = 2.5f;
                            table2.AddCell(cell26Processby);
                            Phrase cuscodeval1 = new Phrase("" + data1[i].Q_SalesPersonName + "", font);
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
                            Phrase Datecell = new Phrase("Datum:", font);
                            PdfPCell cell26Datecell = new PdfPCell(Datecell);
                            cell26Datecell.Border = 0;
                            cell26Datecell.HorizontalAlignment = 0;
                            cell26Datecell.PaddingBottom = 2.5f;
                            table2.AddCell(cell26Datecell);
                            Phrase cuscodeval2 = new Phrase("" + data1[i].Q_Date.ToString("dd-MM-yyyy") + "", font);
                            PdfPCell cell26Processby2 = new PdfPCell(cuscodeval2);
                            cell26Processby2.Border = 0;
                            cell26Processby2.HorizontalAlignment = 2;
                            cell26Processby2.PaddingBottom = 2.5f;
                            table2.AddCell(cell26Processby2);
                            doc1.Add(table2);

                            PdfPTable contenttable = new PdfPTable(1);
                            contenttable.TotalWidth = 100f;
                            contenttable.WidthPercentage = 100f;
                            Paragraph pp1con = new Paragraph();
                            Phrase phar1con = new Phrase("", Fontsmaller1);
                            pp1con.Add(phar1con);
                            PdfPCell cell29Con = new PdfPCell(pp1con);

                            cell29Con.PaddingTop = 4.5f;
                            cell29Con.HorizontalAlignment = 1;
                            cell29Con.PaddingBottom = 5.5f;
                            cell29Con.Border = 0;
                            contenttable.AddCell(cell29Con);
                            PdfPCell cell29 = new PdfPCell();
                            cell29.AddElement(new Paragraph(new Chunk("Angebot Nr:" + data1[i].Q_Code + "", Fontsmaller)));
                            cell29.PaddingTop = 6.5f;
                            cell29.PaddingBottom = 20.5f;
                            cell29.Border = 0;
                            contenttable.AddCell(cell29);
                            doc1.Add(contenttable);
                            PdfPTable table4 = new PdfPTable(6);
                            float[] widths1 = new float[] { 8f, 12f, 10f, 30f, 20f, 20f };
                            table4.TotalWidth = 100f;
                            table4.WidthPercentage = 100f;
                            table4.HeaderRows = 1;
                            table4.SetWidths(widths1);

                            Phrase phraseConstantde1 = new Phrase("Pos", Fontsmaller);
                            PdfPCell cell41 = new PdfPCell(phraseConstantde1);

                            cell41.HorizontalAlignment = 1;
                            cell41.PaddingTop = 2.5f;
                            cell41.PaddingBottom = 2.5f;

                            table4.AddCell(cell41);
                            Phrase phraseConstantde2 = new Phrase("Menge", Fontsmaller);
                            PdfPCell cell42 = new PdfPCell(phraseConstantde2);
                            cell42.Colspan = 2;

                            cell42.HorizontalAlignment = 1;
                            cell42.PaddingTop = 2.5f;
                            cell42.PaddingBottom = 2.5f;

                            table4.AddCell(cell42);
                            Phrase phraseConstantde3 = new Phrase("Text", Fontsmaller);
                            PdfPCell cell421 = new PdfPCell(phraseConstantde3);
                            cell421.HorizontalAlignment = 1;

                            cell421.PaddingTop = 2.5f;
                            cell421.PaddingBottom = 2.5f;

                            table4.AddCell(cell421);
                            Phrase phraseConstantde4 = new Phrase("Einzelpreis(€)", Fontsmaller);
                            PdfPCell cell431 = new PdfPCell(phraseConstantde4);
                            cell431.HorizontalAlignment = 1;


                            cell431.PaddingTop = 2.5f;
                            cell431.PaddingBottom = 2.5f;

                            table4.AddCell(cell431);

                            Phrase phraseConstantde5 = new Phrase("Gesamtpreis(€)", Fontsmaller);
                            PdfPCell cell43 = new PdfPCell(phraseConstantde5);
                            cell43.HorizontalAlignment = 1;
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
                                Phrase qtyphar = new Phrase("" + Convert.ToDecimal(data2[j].QD_Quantity).ToString("N2", CultureInfo.CreateSpecificCulture("de-DE")) + "\n", font);
                                qtypp.Add(qtyphar);
                                PdfPCell cell52 = new PdfPCell(qtypp);
                                cell52.HorizontalAlignment = 2;
                                //  cell52.PaddingTop = 2.5f;
                                // cell52.PaddingBottom = 2.5f;
                                table4.AddCell(cell52);
                                //German Text for UOM
                                string uomText = data2[j].QD_UOM;
                                if (data2[j].QD_UOMCode == "1")
                                    uomText = "Kg";
                                else if (data2[j].QD_UOMCode == "2")
                                    uomText = "Mtrs";
                                else if (data2[j].QD_UOMCode == "3")
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


                                Paragraph Productpp = new Paragraph();
                                Phrase Productphar = new Phrase("" + data2[j].QD_ProductName + "\n", font);
                                Phrase Productphar1 = new Phrase("" + data2[j].QD_Description + "\n", font);
                                Productpp.Add(Productphar);
                                Productpp.Add(Productphar1);

                                PdfPCell cell53 = new PdfPCell(Productpp);
                                cell53.HorizontalAlignment = 0;
                                table4.AddCell(cell53);


                                Paragraph pp = new Paragraph();
                                Phrase phar = new Phrase("" + Convert.ToDecimal(data2[j].QD_Unit_Price).ToString("c2", CultureInfo.CreateSpecificCulture("de-DE")) + "\n", font);
                                pp.Add(phar);
                                PdfPCell cell541 = new PdfPCell(pp);

                                cell541.HorizontalAlignment = 2;
                                cell541.PaddingTop = 2.5f;
                                cell541.PaddingBottom = 2.5f;

                                table4.AddCell(cell541);
                                Paragraph pp12 = new Paragraph();
                                Phrase phar12 = new Phrase("" + (Convert.ToDecimal(data2[j].QD_Unit_Price) * Convert.ToDecimal(data2[j].QD_Quantity)).ToString("c2", CultureInfo.CreateSpecificCulture("de-DE")) + "\n", font);
                                pp12.Add(phar12);
                                PdfPCell cell54 = new PdfPCell(pp12);
                                cell54.HorizontalAlignment = 2;
                                cell54.PaddingTop = 2.5f;
                                cell54.PaddingBottom = 2.5f;

                                table4.AddCell(cell54);
                                total = total + ((Convert.ToDecimal(data2[j].QD_Unit_Price) * Convert.ToDecimal(data2[j].QD_Quantity)));


                            }


                            PdfPCell NetValCell = new PdfPCell();

                            NetValCell.AddElement(new Paragraph(new Chunk("Gesamt Netto", font)));
                            NetValCell.Colspan = 5;
                            NetValCell.PaddingTop = 2.5f;
                            NetValCell.PaddingBottom = 2.5f;

                            table4.AddCell(NetValCell);
                            Paragraph pp1 = new Paragraph();
                            Phrase phar1 = new Phrase("" + Convert.ToDecimal(total).ToString("c2", CultureInfo.CreateSpecificCulture("de-DE")) + "\n", font);
                            pp1.Add(phar1);
                            PdfPCell NetValCell1 = new PdfPCell(pp1);
                            NetValCell1.Colspan = 5;
                            NetValCell1.HorizontalAlignment = 2;
                            NetValCell1.VerticalAlignment = 2;
                            NetValCell1.PaddingTop = 2.5f;
                            NetValCell1.PaddingBottom = 2.5f;

                            table4.AddCell(NetValCell1);

                            if (data1[i].zipCode == 82 && data1[i].Q_Type != 1)
                            {
                                PdfPCell inccell = new PdfPCell();

                                inccell.AddElement(new Paragraph(new Chunk(Convert.ToDecimal(data1[i].VatPer).ToString("0") + "% USt.", font)));
                                inccell.Colspan = 5;
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
                                inccell.AddElement(new Paragraph(new Chunk(Convert.ToDecimal(data1[i].VatPer).ToString("0") + "% USt.", font)));
                                inccell.Colspan = 5;
                                inccell.PaddingTop = 2.5f;
                                inccell.PaddingBottom = 2.5f;

                                table4.AddCell(inccell);
                                Paragraph pp1inc = new Paragraph();
                                Phrase phar1inc = new Phrase("" + (Convert.ToDecimal(total * data1[i].VatPer / 100).ToString("C2", CultureInfo.CreateSpecificCulture("de-DE"))) + "\n", font);
                                pp1inc.Add(phar1inc);
                                PdfPCell inccell1 = new PdfPCell(pp1inc);
                                inccell1.HorizontalAlignment = 2;
                                inccell1.PaddingTop = 2.5f;
                                inccell1.PaddingBottom = 2.5f;

                                table4.AddCell(inccell1);

                            }

                            PdfPCell vataddcell = new PdfPCell();

                            vataddcell.AddElement(new Paragraph(new Chunk("Gesamtbetrag", font)));
                            vataddcell.Colspan = 5;
                            vataddcell.PaddingTop = 2.5f;
                            vataddcell.PaddingBottom = 2.5f;

                            table4.AddCell(vataddcell);
                            if (data1[i].zipCode == 82 && data1[i].Q_Type != 1)
                            {
                                Paragraph pp1vat = new Paragraph();
                                Phrase phar1vat = new Phrase("" + ((Convert.ToDecimal(total + ((total * data1[i].VatPer / 100)))).ToString("C2", CultureInfo.CreateSpecificCulture("de-DE"))) + "\n", Fontsmaller);
                                pp1vat.Add(phar1vat);
                                PdfPCell vataddcell1 = new PdfPCell(pp1vat);
                                vataddcell1.HorizontalAlignment = 2;
                                vataddcell1.Colspan = 6;
                                vataddcell1.PaddingTop = 2.5f;
                                vataddcell1.PaddingBottom = 2.5f;

                                table4.AddCell(vataddcell1);
                            }
                            else
                            {
                                Paragraph pp1vat = new Paragraph();
                                Phrase phar1vat = new Phrase("" + ((Convert.ToDecimal(total + ((total * data1[i].VatPer / 100)))).ToString("C2", CultureInfo.CreateSpecificCulture("de-DE"))) + "\n", Fontsmaller);
                                pp1vat.Add(phar1vat);
                                PdfPCell vataddcell1 = new PdfPCell(pp1vat);
                                vataddcell1.HorizontalAlignment = 2;
                                vataddcell1.Colspan = 5;
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

                            if (data1[i].PaymentTermsDescription != string.Empty)
                            {
                                PdfPCell cell33 = new PdfPCell();
                                //if (data2.Count > 11)
                                //{
                                //    doc1.NewPage();
                                //}
                                cell33.AddElement(new Paragraph(new Chunk("Zahlungsbedingung : " + data1[i].PaymentTermsDescription + "", font)));

                                cell33.PaddingTop = 2.5f;
                                cell33.PaddingBottom = 2.5f;
                                cell33.Border = 0;
                                table7.AddCell(cell33);
                            }

                            if (data1[i].Q_DeliveryTerms != string.Empty)
                            {
                                PdfPCell cell33d = new PdfPCell();
                                cell33d.AddElement(new Paragraph(new Chunk("Lieferbedingungen: " + data1[i].Q_DeliveryTerms + "", font)));
                                cell33d.PaddingTop = 2.5f;
                                cell33d.PaddingBottom = 2.5f;
                                cell33d.Border = 0;
                                table7.AddCell(cell33d);
                            }

                            PdfPCell delitercell = new PdfPCell();
                            delitercell.PaddingTop = 2.5f;
                            delitercell.PaddingBottom = 2.5f;
                            delitercell.Border = 0;
                            table7.AddCell(delitercell);

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

                            line1.AddElement(new Paragraph(new Chunk("Es gelten unsere Allgemeinen Geschäftsbedingungen.", font)));

                            line1.PaddingTop = 6.5f;
                            line1.PaddingBottom = 2.5f;
                            line1.Border = 0;
                            table7.AddCell(line1);
                            //PdfPCell line2 = new PdfPCell();
                            //line2.AddElement(new Paragraph(new Chunk("Jede allgemeine Abweichung muss uns vor der Ware, nicht aber 7 Tage nach Erhalt der Ware mitgeteilt werden.", font)));
                            //line2.PaddingTop = 2.5f;
                            //line2.PaddingBottom = 2.5f;
                            //line2.Border = 0;
                            //table7.AddCell(line2);

                            //PdfPCell line3 = new PdfPCell();
                            //line3.AddElement(new Paragraph(new Chunk("Nach dem Waschen / Verwenden der Ware werden keine Reklamationen akzeptiert.", font)));
                            //line3.PaddingTop = 2.5f;
                            //line3.PaddingBottom = 2.5f;
                            //line3.Border = 0;
                            //table7.AddCell(line3);
                            doc1.Add(table7);
                            doc1.Close();

                        }
                    }

                    return Json(data1, JsonRequestBehavior.AllowGet);
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
        public class pdffooterclass : PdfPageEventHelper
        {
            PdfTemplate headerTemplate, footerTemplate;
            BaseFont bf = null;
            PdfContentByte cb;
            Color FontColour = new Color(35, 31, 32);
            Font Fontbiggest = new Font(FontFactory.GetFont("Arial", 50, Font.BOLD, new Color(180)));
            Font Fontbigger = new Font(FontFactory.GetFont("Arial", 20, Font.BOLD));
            Font Fontsmaller = new Font(FontFactory.GetFont("Arial", 10, Font.BOLD));
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
            //Image logo = iTextSharp.text.Image.GetInstance("E:/Projects/Indra/Images/euro_logowithText.png");
            //Image logo = iTextSharp.text.Image.GetInstance("C:/Websites/Indra/Images/euro_logowithText.png");
            public override void OnStartPage(PdfWriter writer, Document document)
            {
                base.OnStartPage(writer, document);
                //logo.ScaleAbsoluteHeight(100);
                //logo.ScaleAbsoluteWidth(500);
                //string logoPath = "~/Images/euro_logo2912019.png";
                //string serverLogoPath = System.Web.HttpContext.Current.Server.MapPath(logoPath);
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
                // Phrase phraseConstant = new Phrase("EUROTEXTILES\n", Fontbiggest);
                // Phrase phraseConstant1 = new Phrase("Order Conformation", Fontbigger);
                //PdfPCell cell = new PdfPCell(phraseConstant);
                //cell.HorizontalAlignment = 0;
                //Paragraph pg1 = new Paragraph();
                //pg1.Add(phraseConstant);
                // pg1.Add(phraseConstant1);
                //PdfPCell cell12 = new PdfPCell(pg1);
                //cell12.HorizontalAlignment = 1;
                //cell12.Border = 0;
                //cell12.PaddingTop = 2.5f;
                //cell12.PaddingLeft = 2.5f;
                //cell12.PaddingBottom = 2.5f;
                //table0.AddCell(cell12);
                //cb.MoveTo(40, document.PageSize.Height - 100);
                //cb.LineTo(document.PageSize.Width - 40, document.PageSize.Height - 100);
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
        //Restore the deleted records
        public ActionResult ET_Master_Quotation_RestoreDelete(int id, bool type)
        {
            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                {
                    tbl_QuotationHeader deleted = dbcontext.tbl_QuotationHeader.Single(m => m.Q_ID == id);
                    deleted.DELETED = type;
                    deleted.DELETED_BY = id;
                    deleted.DELETED_DATE = DateTime.Now;
                    var result = dbcontext.SaveChanges();
                    var json = "Failed";
                    if (result != 0)
                    {
                        json = "Success";
                    }
                    else
                    {
                        objLOG.log_dockey = "8015";
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
        //Edit the records
        public ActionResult ET_Master_Quotation_Update_GetbyID(int id)
        {
            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                {
                    //TempData["CompaycontctID"] = id;
                    dbcontext.Configuration.ProxyCreationEnabled = false;
                    var data = dbcontext.tbl_QuotationHeader.Single(m => m.Q_ID == id);
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
        //get the quoation product details
        public ActionResult ET_Master_Quatation_Details(int id)
        {
            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                {
                    var data2 = (from c in dbcontext.tbl_QuotationDetails
                                 join a in dbcontext.Tbl_Product_Master on c.QD_ProductID equals a.P_ID
                                 join b in dbcontext.tbl_LookUp on a.P_UOM equals b.LU_Code
                                 into m
                                 from n in m.DefaultIfEmpty()
                                 where c.QD_PID == id && n.LU_Type == 2
                                 select new
                                 {
                                     QD_ProductID = a.P_ID,
                                     QD_ProductName = a.P_ShortName,
                                     QD_UOM = n.LU_Description,
                                     QD_Unit_Price = c.QD_Unit_Price,
                                     QD_Quantity = c.QD_Quantity,
                                     QD_Amount = c.QD_Amount,
                                     QD_Description = c.QD_Description,
                                     QD_CatalogPriceType = c.QD_CatalogPriceType
                                 }).ToList();
                    var modelItems = data2.Select((enquiryItem, index) => new { SO_Serial = index + 1, QD_ProductID = enquiryItem.QD_ProductID, QD_ProductName = enquiryItem.QD_ProductName, QD_UOM = enquiryItem.QD_UOM, QD_Unit_Price = enquiryItem.QD_Unit_Price, QD_Quantity = enquiryItem.QD_Quantity, QD_Amount = enquiryItem.QD_Amount, QD_Description = enquiryItem.QD_Description, QD_CatalogPriceType = enquiryItem.QD_CatalogPriceType}).ToList();
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
                    return Json("ERR" + errid.ToString(), JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return RedirectToAction("ET_SessionExpire", "ET_Login");
            }
        }
        //Get the enquiry product details 
        public ActionResult ET_Master_Enquiry_Details(int id,int priceType)
        {
            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                {
                    var data2 = (from c in dbcontext.tbl_EnquiryDetails
                                 join a in dbcontext.Tbl_Product_Master on c.ED_ProductID equals a.P_ID
                                 join b in dbcontext.tbl_LookUp on a.P_UOM equals b.LU_Code
                                 into m
                                 from n in m.DefaultIfEmpty()
                                 where c.ED_PID == id && n.LU_Type == 2
                                 select new
                                 {
                                     QD_ProductID = a.P_ID,
                                     QD_ProductName = a.P_ShortName,
                                     QD_UOM = n.LU_Description,
                                     QD_Unit_Price = (priceType == 1 ? (dbcontext.Tbl_ProductCatalog.FirstOrDefault(m => m.ACTIVE_STATUS == true && m.PRODUCT_ID == a.P_ID).UNIT_PRICE1) : priceType == 2 ? (dbcontext.Tbl_ProductCatalog.FirstOrDefault(m => m.ACTIVE_STATUS == true && m.PRODUCT_ID == a.P_ID).UNIT_PRICE2) : priceType == 3 ? (dbcontext.Tbl_ProductCatalog.FirstOrDefault(m => m.ACTIVE_STATUS == true && m.PRODUCT_ID == a.P_ID).UNIT_PRICE3) : (dbcontext.Tbl_ProductCatalog.FirstOrDefault(m => m.ACTIVE_STATUS == true && m.PRODUCT_ID == a.P_ID).UNIT_PRICE4)) != null ? (priceType == 1 ? (dbcontext.Tbl_ProductCatalog.FirstOrDefault(m => m.ACTIVE_STATUS == true && m.PRODUCT_ID == a.P_ID).UNIT_PRICE1) : priceType == 2 ? (dbcontext.Tbl_ProductCatalog.FirstOrDefault(m => m.ACTIVE_STATUS == true && m.PRODUCT_ID == a.P_ID).UNIT_PRICE2) : priceType == 3 ? (dbcontext.Tbl_ProductCatalog.FirstOrDefault(m => m.ACTIVE_STATUS == true && m.PRODUCT_ID == a.P_ID).UNIT_PRICE3) : (dbcontext.Tbl_ProductCatalog.FirstOrDefault(m => m.ACTIVE_STATUS == true && m.PRODUCT_ID == a.P_ID).UNIT_PRICE4)) : 0,
                                     QD_Quantity = c.ED_Quantity,
                                     QD_Amount = 0,
                                     QD_Description = c.ED_Description
                                 }).ToList();
                    var modelItems = data2.Select((enquiryItem, index) => new { SO_Serial = index + 1, QD_ProductID = enquiryItem.QD_ProductID, QD_ProductName = enquiryItem.QD_ProductName, QD_UOM = enquiryItem.QD_UOM, QD_Unit_Price = enquiryItem.QD_Unit_Price, QD_Quantity = enquiryItem.QD_Quantity, QD_Amount = enquiryItem.QD_Amount, QD_Description = enquiryItem.QD_Description }).ToList();
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
                    return Json("ERR" + errid.ToString(), JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return RedirectToAction("ET_SessionExpire", "ET_Login");
            }
        }
        [ValidateInput(false)]
        [HttpPost]
        public JsonResult SendMail(string formData,int id)
        {
            //Send the quation details
            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                {
                    int userid = Convert.ToInt32(Session["UserID"]);
                    var settings = dbcontext.Tbl_MailSettings.Where(a => a.MS_UserId == userid);
                   if(settings.Count() == 1)
                    {
                        var success = "Success";
                        try
                        {
                            MemoryStream workStream = new MemoryStream();
                            Response.ContentType = "application/pdf";
                            Response.AddHeader("content-disposition", "attachment;filename=Quotation.pdf");
                            Response.Cache.SetCacheability(HttpCacheability.NoCache);
                            Document pdfDoc;
                            pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
                            HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                            //PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
                            PdfWriter.GetInstance(pdfDoc, workStream).CloseStream = false;
                            pdfDoc.Open();
                            int com_key = Convert.ToInt32(Session["CompanyKey"].ToString());
                            var data1 = dbcontext.Tbl_SystemSetUp.Single(a => a.COMPANY_ID == com_key);
                            BaseFont bf = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                            Font font1 = new iTextSharp.text.Font(bf , 22, iTextSharp.text.Font.BOLD);
                            Chunk c1 = new Chunk(data1.COMPANY_NAME, font1);
                            var path = Server.MapPath("~/" + data1.COMPANY_LOGO);
                            iTextSharp.text.Image jpg;
                            jpg = iTextSharp.text.Image.GetInstance(path.ToString());



                            //Resize image depend upon your need

                            jpg.ScaleToFit(80f, 82f);

                            //Give space before image

                            jpg.SpacingBefore = 0f;

                            //Give some space after the image

                            jpg.SpacingAfter = 0f;

                            jpg.Alignment = Element.ALIGN_RIGHT;

                            PdfPTable table = new PdfPTable(3);

                            PdfPCell cell1 = new PdfPCell();
                            PdfPCell cell2 = new PdfPCell();
                            PdfPCell cell3 = new PdfPCell();
                            cell1.BorderWidth = 0;
                            cell2.BorderWidth = 0;
                            cell3.BorderWidth = 0;
                            Paragraph p1 = new Paragraph();
                            Paragraph p2 = new Paragraph(c1);
                            p1.Alignment = Element.ALIGN_LEFT;
                            p1.Alignment = Element.ALIGN_BOTTOM;
                            p2.Alignment = Element.ALIGN_CENTER;
                            p1.Add(new Chunk(jpg , 0 , -50));
                            cell1.AddElement(p1);
                            cell2.AddElement(p2);
                            table.AddCell(cell1);
                            table.AddCell(cell2);
                            table.AddCell(cell3);
                            table.WidthPercentage = 100;
                            pdfDoc.Add(table);

                            StyleSheet styles = new StyleSheet();
                            
                            StringWriter sw = new StringWriter();
                            HtmlTextWriter hw = new HtmlTextWriter(sw);
                            hw.WriteLine(formData);
                            StringReader sr = new StringReader(sw.ToString());
                            htmlparser.Parse(sr);
                            pdfDoc.Close();
                            byte[] byteInfo = workStream.ToArray();
                            workStream.Write(byteInfo, 0, byteInfo.Length);
                            workStream.Position = 0;

                            string usermail = settings.Select(a => a.MS_EmailID).FirstOrDefault();
                            decimal custid = dbcontext.tbl_QuotationHeader.Single(a => a.Q_ID == id).Q_CustomerID;
                            string custmail = dbcontext.Tbl_Master_CompanyDetails.Single(a => a.COM_ID == custid).COM_EMAIL;
                            MailMessage mm = new MailMessage(usermail, custmail);
                            mm.Subject = "Quotation regarding";
                            string newLine = Environment.NewLine;
                            string body = "Dear Sir/Madam, ";
                            body = body + newLine;
                            body = body + newLine;
                            body = body + "Thank you for enquiring us about our products.";
                            body = body + newLine;
                            body = body + "Kindly find attached our quotation corresponding to your enquiry about our products.All terms and conditions are included in the said quotation.";
                            body = body + newLine;
                            body = body + "Please review our quotation  and Please contact us if you have any clarification.";
                            body = body + newLine;
                            body = body + "We look forward  for your confirmed order.";
                            body = body + newLine;
                            body = body + newLine;
                            body = body + "Regards";
                            body = body + newLine;
                            body = body + newLine;
                            body = body + Session["UserName"].ToString();
                            mm.Body = string.Format(body);
                            mm.Attachments.Add(new Attachment(workStream, "Quotation.pdf"));

                            SmtpClient smtp = new SmtpClient();
                            smtp.Host = settings.Select(a => a.MS_OutGoingHostName).FirstOrDefault();
                            NetworkCredential NetworkCred = new NetworkCredential();
                            NetworkCred.UserName = settings.Select(a => a.MS_EmailID).FirstOrDefault();
                            NetworkCred.Password = settings.Select(a => a.MS_Password).FirstOrDefault();
                            smtp.UseDefaultCredentials = true;
                            smtp.Credentials = NetworkCred;
                            smtp.Port = settings.Select(a => a.MS_OutGoingPort).FirstOrDefault();
                            smtp.Send(mm);
                        }
                        catch(Exception ex)
                        {
                            success = "False";
                        }
                        return Json(success, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        var json = "Validation: Please Configure Your Mail Settings";
                        return Json(json, JsonRequestBehavior.AllowGet);
                    }
                }
                //catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
                //{
                //    Exception raise = dbEx;
                //    foreach (var validationErrors in dbEx.EntityValidationErrors)
                //    {
                //        foreach (var validationError in validationErrors.ValidationErrors)
                //        {
                //            string message = string.Format("{0}:{1}",
                //                validationErrors.Entry.Entity.ToString(),
                //                validationError.ErrorMessage);
                //            // raise a new exception nesting  
                //            // the current instance as InnerException  
                //            raise = new InvalidOperationException(message, raise);
                //        }
                //    }
                //    throw raise;
                //}
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