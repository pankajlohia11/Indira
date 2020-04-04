using BusinessEntity.CustomModels;
using BusinessEntity.EntityModels;
using BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using BusinessLogic.Admin_BL;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.IO;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.Globalization;
//using System.Data.Entity;
using Euro.Models;
using ExcelDataReader;
using System.Web;
using System.Data;
using System.Reflection;
using System.ComponentModel;

namespace Euro.Controllers.Trading
{
    public class ET_Trading_PackingListController : Controller
    {
        // GET: ET_Trading_PackingList
        public static string prefix = "";
        public static bool automanual = false;
        BAL bal = new BAL();
        error_master objERR = new error_master();
        tbl_loginfo objLOG = new tbl_loginfo();
        EntityClasses dbcontext = new EntityClasses();
        public ActionResult ET_Trading_PackingList()
        {
            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                {
                    AutoManual();
                    ViewBag.Login_Name = Session["DisplayName"].ToString();
                    //return View("/Views/Trading/ET_Trading_PackingList/ET_Trading_ShipmentToPackingList.cshtml");
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

        public ActionResult ET_Trading_ShipmentToPackingList()
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
        //check the auto/manual
        private void AutoManual()
        {
            ViewBag.AutoManual = false;
            automanual = false;
            List<Tbl_Document_Master> ObjDoc = bal.AutoManual_BL(8021);
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
        //Check the privillages
        public JsonResult GetPrivilages()
        {

            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                {
                    var privilagelist = bal.GetPrivilages_BL(Convert.ToInt32(Session["UserID"].ToString()), 8021);
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
        //get the packing list
        public JsonResult GetPackingList(bool delete)
        {

            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                {
                    int comp_key = Convert.ToInt32(Session["CompanyKey"].ToString());

                    dbcontext.Configuration.ProxyCreationEnabled = false;
                    var data = (from a in dbcontext.Tbl_Packing_List
                                join b in dbcontext.Tbl_Master_CompanyDetails on a.PL_CustomerID equals b.COM_ID into comp
                                from x in comp
                                where a.DELETED == delete && a.COM_KEY == comp_key
                                select new
                                {
                                    ID = a.PL_ID,
                                    Code = a.PL_Code,
                                    Date = a.PL_Date,
                                    remarks = a.PL_Remarks,
                                    customer= x.COM_NAME
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

        //public JsonResult Upload(HttpPostedFileBase upload)
        //{
        //    if (ModelState.IsValid)
        //    {

        //        if (upload != null && upload.ContentLength > 0)
        //        {
        //            // ExcelDataReader works with the binary Excel file, so it needs a FileStream
        //            // to get started. This is how we avoid dependencies on ACE or Interop:
        //            Stream stream = upload.InputStream;

        //            // We return the interface, so that
        //            IExcelDataReader reader = null;


        //            if (upload.FileName.EndsWith(".xls"))
        //            {
        //                reader = ExcelReaderFactory.CreateBinaryReader(stream);
        //            }
        //            else if (upload.FileName.EndsWith(".xlsx"))
        //            {
        //                reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
        //            }
        //            else
        //            {
        //                ModelState.AddModelError("File", "This file format is not supported");
        //                return View();
        //            }

        //            reader.IsFirstRowAsColumnNames = true;

        //            DataSet result = reader.AsDataSet();
        //            reader.Close();

        //            return View(result.Tables[0]);
        //        }
        //        else
        //        {
        //            ModelState.AddModelError("File", "Please Upload Your file");
        //        }
        //    }
        //    return View();
        //}

        public JsonResult GetCustomersForShipment(string shipmentId)
        {
            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                {
                    decimal shipment = 0;
                    if (shipmentId == "")
                        shipment = 0;
                    else
                        shipment = Convert.ToDecimal(shipmentId);
                    int comkey = Convert.ToInt32(Session["CompanyKey"].ToString());
                    dbcontext.Configuration.ProxyCreationEnabled = false;
                    var customer = dbcontext.Tbl_Shipment_Header.Where(ship => ship.S_ID == shipment).Select(t => t.S_CustSup);
                    var json = new JavaScriptSerializer().Serialize(customer);
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
        //Get the customer list
        public JsonResult GetCustomers(int id)
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
                        var data = dbcontext.Tbl_Master_CompanyDetails.Where(m => m.COM_KEY == comkey && m.Cust_Supp != 1 && m.DELETED == false).ToList();
                        var json = new JavaScriptSerializer().Serialize(data);
                        return Json(json, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        var custs = dbcontext.Tbl_Packing_List.Single(a => a.PL_ID == id && a.PL_Type == 2).PL_CustomerID;
                        var data = dbcontext.Tbl_Master_CompanyDetails.Where(m => m.COM_KEY == comkey && m.Cust_Supp != 1 && (m.DELETED == false || m.COM_ID == custs)).ToList();
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
        //Get the order list
        public JsonResult GetOrderList(int custid)
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
                        var StoreUser = dbcontext.Tbl_Master_Order.Where(m => m.COM_KEY == comp_key && m.SO_OrderType == 2 && m.SO_CutomerID == custid).ToList();
                        var json = new JavaScriptSerializer().Serialize(StoreUser);
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
        public JsonResult GetOrderForShipment(string shipment)
        {
            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                decimal shipmentId = 0;
                if (shipment == "")
                    shipmentId = 0;
                else
                    shipmentId = Convert.ToDecimal(shipment);
                try
                {
                    int comkey = Convert.ToInt32(Session["CompanyKey"].ToString());
                    dbcontext.Configuration.ProxyCreationEnabled = false;
                    var data = (from OTOI in dbcontext.Tbl_Shipment_Header
                                join sc in dbcontext.Tbl_Schedule on OTOI.S_ScheduleID equals sc.SH_Code
                                join e in dbcontext.Tbl_Master_Order on sc.SH_OrderID equals e.SO_ID
                                where OTOI.S_ID == shipmentId &&  OTOI.COM_KEY == comkey && OTOI.DELETED == false
                                select e.SO_ID);
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
        public JsonResult GetPackingTypeForOrder(int orderId)
        {
            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                {
                    int comkey = Convert.ToInt32(Session["CompanyKey"].ToString());
                    dbcontext.Configuration.ProxyCreationEnabled = false;
                    var data = dbcontext.Tbl_Master_Order.Where(od => od.SO_ID == orderId).Select(a => a.SO_ProductType);
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
        public JsonResult GetShipmentForPacking(int customerId,int orderId)
        {

            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                {
                        int comkey = Convert.ToInt32(Session["CompanyKey"].ToString());
                        dbcontext.Configuration.ProxyCreationEnabled = false;
                        var data = (from OTOI in dbcontext.Tbl_Shipment_Header
                                     join sc in dbcontext.Tbl_Schedule on OTOI.S_ScheduleID equals sc.SH_Code
                                     join e in dbcontext.Tbl_Master_Order on sc.SH_OrderID equals e.SO_ID
                                     where OTOI.S_CustSup == customerId && e.SO_ID == orderId && OTOI.COM_KEY == comkey && OTOI.DELETED == false
                                     select OTOI.S_Code);
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

        //Get the product list
        public JsonResult GetProducts(string productCategory)
        {

            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                {
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
                    var pgData = 1;
                    var yarnCat = 1;
                    var fabCat = 1;
                    if (masterCategory != 0)
                    {
                        pgData = (int)(dbcontext.Tbl_ProductGroup.Where(m => m.PG_TYPE == 1 && m.PG_NAME == productGroupName).Select(a => a.PG_ID)).SingleOrDefault();
                        masterCategory = Convert.ToInt32(pgData);
                    }
                    else
                    {
                        yarnCat = (int)(dbcontext.Tbl_ProductGroup.Where(m => m.PG_TYPE == 1 && m.PG_NAME == "YARN").Select(a => a.PG_ID)).SingleOrDefault();
                        fabCat = (int)(dbcontext.Tbl_ProductGroup.Where(m => m.PG_TYPE == 1 && m.PG_NAME == "FABRIC").Select(a => a.PG_ID)).SingleOrDefault();
                    }

                    dbcontext.Configuration.ProxyCreationEnabled = false;
                    int custkey = Convert.ToInt32(Session["CompanyKey"]);
                    //var data = dbcontext.Tbl_Product_Master.Where(m => m.COM_KEY == custkey).ToList();
                    var data = (from PM in dbcontext.Tbl_Product_Master
                                    //join CG in dbcontext.Tbl_Master_Category on PM.P_CategoryID equals CG.CAT_CODE
                                    where PM.COM_KEY == custkey && (PM.DELETED == false) && (PM.P_CategoryID == masterCategory || (masterCategory == 0 && PM.P_CategoryID != yarnCat && PM.P_CategoryID != fabCat))
                                    select new
                                    {
                                        PM.P_ID,
                                        PM.P_CategoryID,
                                        PM.P_ArticleNo,
                                        PM.P_ShortName,
                                        PM.P_Name
                                    }).Distinct().OrderBy(m => m.P_ArticleNo).ToList();
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
        //public JsonResult ET_Master_PackingListPrint(int id, string lang)
        //{
        //    bool val = Session["UserID"] == null ? false : true;
        //    var context = new EntityClasses();
        //    var transaction = context.Database.BeginTransaction();
        //    try
        //    {
        //        dbcontext.Configuration.ProxyCreationEnabled = false;
        //        int com_key = Convert.ToInt32(Session["Companykey"]);
        //        var OrderIDs = new HashSet<decimal>(dbcontext.Tbl_Packing_List.Single(f => f.PL_ID == id).PL_OrderNo.Split(',').Select(m => Convert.ToDecimal(m)).ToList());
        //        var OrderCodes = string.Join(",", (from m in dbcontext.Tbl_Master_Order where OrderIDs.Contains(m.SO_ID) select m.SO_Code).ToList());
        //        var data1 = (from a in dbcontext.Tbl_Packing_List
        //                     join b in dbcontext.Tbl_Master_CompanyDetails on a.PL_CustomerID equals b.COM_ID into comp
        //                     from x in comp
        //                     where a.PL_ID == id
        //                     select new PackingList_ViewCM
        //                     {
        //                         PL_ID = a.PL_ID,
        //                         PL_Code = a.PL_Code,
        //                         PL_Date = a.PL_Date,
        //                         PL_Remarks = a.PL_Remarks,
        //                         CustomerName = x.COM_DISPLAYNAME,
        //                         Orders = OrderCodes,
        //                         CompanyCode = x.COM_CODE,
        //                         CompanyName = x.COM_NAME,
        //                         Street = x.COM_STREET,
        //                         CityState = (x.COM_CITY + ", " + x.COM_STATE),
        //                         USTID = x.COM_USTID,
        //                         CountryZip = ((dbcontext.locations.Where(a => a.location_id == x.COM_COUNTRY).Select(a => a.name).FirstOrDefault()) + ", " + (x.COM_ZIP)),
        //                         VatPer = (dbcontext.Tbl_SystemSetUp.Where(a => a.COMPANY_ID == com_key).Select(a => a.TAX).FirstOrDefault()),
        //                         imgurl = (dbcontext.Tbl_SystemSetUp.Where(a => a.COMPANY_ID == com_key).Select(a => a.COMPANY_LOGO).FirstOrDefault()),
        //                         SystemCompany = (dbcontext.Tbl_SystemSetUp.Where(a => a.COMPANY_ID == com_key).Select(a => a.COMPANY_NAME).FirstOrDefault()),
        //                     }
        //                ).ToList();
        //        var data2 = (from c in dbcontext.Tbl_Packing_ListDetails
        //                     join a in dbcontext.Tbl_Product_Master on c.PD_ProductID equals a.P_ID
        //                     join b in dbcontext.tbl_LookUp on a.P_UOM equals b.LU_Code
        //                     into m
        //                     from n in m.DefaultIfEmpty()
        //                     where c.PD_PID == id && n.LU_Type == 2
        //                     select new PackingList_ViewCM
        //                     {
        //                         PD_ArticleNo = a.P_ArticleNo,
        //                         PD_ProductID = a.P_ID,
        //                         ProductName = a.P_ShortName,
        //                         PD_PalletNo = c.PD_PalletNo,
        //                         PD_DesignNo = c.PD_DesignNo,
        //                         PD_NoOfPieces = c.PD_NoOfPieces,
        //                         PD_TotalMeters = c.PD_TotalMeters,
        //                         PD_NwtinKGS = c.PD_NwtinKGS,
        //                         PD_GwtinKGS = c.PD_GwtinKGS,
        //                         PD_IndividualPieceLength = c.PD_IndividualPieceLength
        //                     }).OrderBy(m => m.PD_ProductID).ToList();
        //        string path = "";
        //        if (lang == "E")
        //        {
        //            for (int i = 0; i < data1.Count; i++)
        //            {

        //                var doc1 = new iTextSharp.text.Document(PageSize.A4, 30, 25, 130, 90);
        //                string subPath = "~/Sales/PDFList/PackingList/" + data1[i].PL_Code + "/";
        //                bool exists = System.IO.Directory.Exists(HttpContext.Server.MapPath(subPath));
        //                if (!exists)
        //                    System.IO.Directory.CreateDirectory(HttpContext.Server.MapPath(subPath));
        //                var output = new FileStream(Server.MapPath(subPath + data1[i].PL_Code + ".pdf"), FileMode.Create);
        //                var writer = PdfWriter.GetInstance(doc1, output);
        //                writer.PageEvent = new pdffooterclass();
        //                //PdfWriter.GetInstance(doc1, new FileStream(Request.PhysicalApplicationPath + "\\Invoice_Statement.pdf", FileMode.Create));
        //                doc1.Open();
        //                path = output.Name;
        //                //font size change from default  added by gv on 12/12/18
        //                FontFactory.RegisterDirectories();
        //                Font font = new Font(FontFactory.GetFont("Arial", 10, Font.NORMAL));
        //                Font fontsmall = new Font(FontFactory.GetFont("Arial", 9, Font.NORMAL));
        //                Font fontsmall1 = new Font(FontFactory.GetFont("Arial", 10, Font.NORMAL | Font.UNDERLINE));
        //                Font Fontbigger = new Font(FontFactory.GetFont("Arial", 20, Font.BOLD));
        //                Font Fontsmaller = new Font(FontFactory.GetFont("Arial", 10, Font.BOLD));
        //                Font Fontsmaller1 = new Font(FontFactory.GetFont("Arial", 12, Font.BOLD));

        //                PdfPTable table1 = new PdfPTable(1);
        //                table1.DefaultCell.Border = 0;
        //                table1.WidthPercentage = 100f;

        //                PdfPCell Title = new PdfPCell();
        //                Title.Border = 0;

        //                Title.AddElement(new Paragraph(new Chunk("Eurotextiles GmbH & Co. KG, Mammolshainer Weg 14, 61462 Königstein Ts.", fontsmall1)));
        //                Title.VerticalAlignment = 2;
        //                Title.PaddingTop = 1.0f;
        //                Title.PaddingBottom = 3.0f;
        //                table1.AddCell(Title);
        //                Paragraph pg2 = new Paragraph();
        //                Phrase phraseConstant2 = new Phrase("" + data1[i].CustomerName + "\n", font);
        //                Phrase phraseConstant3 = new Phrase("" + data1[i].Street + "\n", font);
        //                Phrase phraseConstant4 = new Phrase("" + data1[i].CityState + "\n", font);
        //                Phrase phraseConstant5 = new Phrase("" + data1[i].CountryZip + "\n", font);
        //                pg2.Add(phraseConstant2);
        //                pg2.Add(phraseConstant3);
        //                pg2.Add(phraseConstant4);
        //                pg2.Add(phraseConstant5);
        //                PdfPCell cell21 = new PdfPCell(pg2);
        //                cell21.HorizontalAlignment = 0;
        //                cell21.PaddingTop = 1.0f;
        //                cell21.PaddingLeft = 1.0f;
        //                cell21.Border = 0;
        //                table1.AddCell(cell21);
        //                doc1.Add(table1);

        //                PdfPTable table2 = new PdfPTable(3);
        //                table2.WidthPercentage = 100f;
        //                float[] widthsvalforcus = new float[] { 13f, 4f, 5f };
        //                table2.TotalWidth = 100f;
        //                table2.WidthPercentage = 100f;
        //                table2.SetWidths(widthsvalforcus);

        //                Phrase emp1 = new Phrase("", font);
        //                PdfPCell cell26emp = new PdfPCell(emp1);
        //                cell26emp.Border = 0;
        //                cell26emp.HorizontalAlignment = 2;
        //                cell26emp.PaddingBottom = 2.5f;
        //                table2.AddCell(cell26emp);
        //                Phrase cuscode = new Phrase("Customer Code:", font);
        //                PdfPCell cell26 = new PdfPCell(cuscode);
        //                cell26.Border = 0;
        //                cell26.HorizontalAlignment = 0;
        //                cell26.PaddingBottom = 2.5f;
        //                table2.AddCell(cell26);
        //                Phrase cuscodeval = new Phrase("" + data1[i].PL_CustomerID + "", font);
        //                PdfPCell cell26val = new PdfPCell(cuscodeval);
        //                cell26val.Border = 0;
        //                cell26val.HorizontalAlignment = 0;
        //                cell26val.PaddingBottom = 2.5f;
        //                table2.AddCell(cell26val);

        //                Phrase emp2 = new Phrase("", font);
        //                PdfPCell cell26emp1 = new PdfPCell(emp2);
        //                cell26emp1.Border = 0;
        //                cell26emp1.HorizontalAlignment = 0;
        //                cell26emp1.PaddingBottom = 2.5f;
        //                table2.AddCell(cell26emp1);
        //                Phrase Processby = new Phrase("Processed By:", font);
        //                PdfPCell cell26Processby = new PdfPCell(Processby);
        //                cell26Processby.Border = 0;
        //                cell26Processby.HorizontalAlignment = 0;
        //                cell26Processby.PaddingBottom = 2.5f;
        //                table2.AddCell(cell26Processby);
        //                Phrase cuscodeval1 = new Phrase("" + data1[i].CompanyName + "", font);
        //                PdfPCell cell26Processby1 = new PdfPCell(cuscodeval1);
        //                cell26Processby1.Border = 0;
        //                cell26Processby1.HorizontalAlignment = 0;
        //                cell26Processby1.PaddingBottom = 2.5f;
        //                table2.AddCell(cell26Processby1);

        //                Phrase emp3 = new Phrase("", font);
        //                PdfPCell cell26emp2 = new PdfPCell(emp3);
        //                cell26emp2.Border = 0;
        //                cell26emp2.HorizontalAlignment = 2;
        //                cell26emp2.PaddingBottom = 2.5f;
        //                table2.AddCell(cell26emp2);
        //                Phrase Datecell = new Phrase("Date:", font);
        //                PdfPCell cell26Datecell = new PdfPCell(Datecell);
        //                cell26Datecell.Border = 0;
        //                cell26Datecell.HorizontalAlignment = 0;
        //                cell26Datecell.PaddingBottom = 2.5f;
        //                table2.AddCell(cell26Datecell);
        //                Phrase cuscodeval2 = new Phrase("" + (data1[i].PL_Date ?? DateTime.Now).ToString("dd-MM-yyyy") + "", font);
        //                PdfPCell cell26Processby2 = new PdfPCell(cuscodeval2);
        //                cell26Processby2.Border = 0;
        //                cell26Processby2.HorizontalAlignment = 0;
        //                cell26Processby2.PaddingBottom = 2.5f;
        //                table2.AddCell(cell26Processby2);

        //                doc1.Add(table2);
        //                PdfPTable contenttable = new PdfPTable(1);
        //                contenttable.TotalWidth = 100f;
        //                contenttable.WidthPercentage = 100f;
        //                Paragraph pp1con = new Paragraph();
        //                Phrase phar1con = new Phrase("Order confirmation", Fontsmaller1);
        //                pp1con.Add(phar1con);
        //                PdfPCell cell29Con = new PdfPCell(pp1con);
        //                //cell29Con.AddElement(new Paragraph(new Chunk("" , Fontsmaller)));
        //                cell29Con.PaddingTop = 4.5f;
        //                cell29Con.HorizontalAlignment = 1;
        //                cell29Con.PaddingBottom = 5.5f;
        //                cell29Con.Border = 0;
        //                contenttable.AddCell(cell29Con);
        //                PdfPCell cell29 = new PdfPCell();
        //                cell29.AddElement(new Paragraph(new Chunk("Order confirmation No:" + data1[i].SO_Code + "", Fontsmaller)));
        //                cell29.PaddingTop = 6.5f;
        //                cell29.PaddingBottom = 20.5f;
        //                cell29.Border = 0;
        //                contenttable.AddCell(cell29);
        //                doc1.Add(contenttable);
        //                PdfPTable table4 = new PdfPTable(7);
        //                float[] widths1 = new float[] { 5f, 10f, 10f, 40f, 10f, 10f, 13f };
        //                table4.TotalWidth = 100f;
        //                table4.WidthPercentage = 100f;
        //                table4.HeaderRows = 1;
        //                table4.SetWidths(widths1);

        //                Phrase phraseConstantde1 = new Phrase("S.No", Fontsmaller);
        //                PdfPCell cell41 = new PdfPCell(phraseConstantde1);

        //                // cell41.AddElement(new Paragraph(new Chunk("Item", Fontsmaller)));

        //                cell41.HorizontalAlignment = 1;
        //                cell41.PaddingTop = 2.5f;
        //                cell41.PaddingBottom = 2.5f;

        //                table4.AddCell(cell41);
        //                Phrase phraseConstantde2 = new Phrase("Quantity", Fontsmaller);
        //                PdfPCell cell42 = new PdfPCell(phraseConstantde2);
        //                cell42.Colspan = 2;
        //                //cell42.AddElement(new Paragraph(new Chunk("Qty", Fontsmaller)));
        //                cell42.HorizontalAlignment = 1;
        //                cell42.PaddingTop = 2.5f;
        //                cell42.PaddingBottom = 2.5f;

        //                table4.AddCell(cell42);
        //                Phrase phraseConstantde3 = new Phrase("Product Description", Fontsmaller);
        //                PdfPCell cell421 = new PdfPCell(phraseConstantde3);
        //                cell421.HorizontalAlignment = 1;
        //                //cell421.AddElement(new Paragraph(new Chunk("UOM", Fontsmaller)));
        //                cell421.PaddingTop = 2.5f;
        //                cell421.PaddingBottom = 2.5f;

        //                table4.AddCell(cell421);
        //                Phrase phraseConstantde4 = new Phrase("Price", Fontsmaller);
        //                PdfPCell cell431 = new PdfPCell(phraseConstantde4);
        //                cell431.HorizontalAlignment = 1;
        //                // cell431.Colspan = 2;
        //                //cell431.AddElement(new Paragraph(new Chunk("Product Name", Fontsmaller)));

        //                cell431.PaddingTop = 2.5f;
        //                cell431.PaddingBottom = 2.5f;

        //                table4.AddCell(cell431);
        //                Phrase phraseConstantde4d = new Phrase("Discount(%)", Fontsmaller);
        //                PdfPCell cell431d = new PdfPCell(phraseConstantde4d);
        //                cell431d.HorizontalAlignment = 1;
        //                // cell431d.Colspan = 2;
        //                //cell431.AddElement(new Paragraph(new Chunk("Product Name", Fontsmaller)));

        //                cell431d.PaddingTop = 2.5f;
        //                cell431d.PaddingBottom = 2.5f;

        //                table4.AddCell(cell431d);
        //                Phrase phraseConstantde5 = new Phrase("Total", Fontsmaller);
        //                PdfPCell cell43 = new PdfPCell(phraseConstantde5);
        //                cell43.HorizontalAlignment = 1;

        //                // cell43.AddElement(new Paragraph(new Chunk("Description", Fontsmaller)));

        //                cell43.PaddingTop = 2.5f;
        //                cell43.PaddingBottom = 2.5f;

        //                table4.AddCell(cell43);
        //                decimal? total = 0;
        //                for (int j = 0; j < data2.Count; j++)
        //                {
        //                    Paragraph Snopp = new Paragraph();
        //                    Phrase Snophar = new Phrase("" + (j + 1) + "\n", font);
        //                    Snopp.Add(Snophar);
        //                    PdfPCell cell51 = new PdfPCell(Snopp);
        //                    cell51.HorizontalAlignment = 1;
        //                    //cell51.AddElement(new Paragraph(new Chunk("" + (j + 1) + "", font)));

        //                    // cell51.PaddingTop = 2.5f;
        //                    // cell51.PaddingBottom = 2.5f;

        //                    table4.AddCell(cell51);
        //                    Paragraph qtypp = new Paragraph();
        //                    Phrase qtyphar = new Phrase("" + Convert.ToDecimal(data2[j].PD_NoOfPieces).ToString("N2", CultureInfo.CreateSpecificCulture("de-DE")) + "\n", font);
        //                    qtypp.Add(qtyphar);
        //                    PdfPCell cell52 = new PdfPCell(qtypp);
        //                    cell52.HorizontalAlignment = 2;
        //                    //  cell52.PaddingTop = 2.5f;
        //                    // cell52.PaddingBottom = 2.5f;

        //                    table4.AddCell(cell52);
        //                    Paragraph uompp = new Paragraph();
        //                    Phrase uomphar = new Phrase("" + data2[j].PD_DesignNo + "\n", font);
        //                    uompp.Add(uomphar);
        //                    PdfPCell uomcell = new PdfPCell(uompp);

        //                    // uomcell.AddElement(new Paragraph(new Chunk("" + data2[j].UOM_NAME + "", font)));
        //                    uomcell.HorizontalAlignment = 0;
        //                    // uomcell.PaddingTop = 2.5f;
        //                    // uomcell.PaddingBottom = 2.5f;

        //                    table4.AddCell(uomcell);

        //                    if (data1[i].SO_OrderType == 2)
        //                    {
        //                        Paragraph Productpp = new Paragraph();
        //                        Phrase Productphar = new Phrase("" + data2[j].PRODUCT_Name + "\n", font);
        //                        Phrase Productphar1 = new Phrase("" + data2[j].DesignDetail + "\n", font);
        //                        Phrase Productphar2 = new Phrase("" + data2[j].CustomerDesc + "\n", font);
        //                        Productpp.Add(Productphar);
        //                        Productpp.Add(Productphar1);
        //                        Productpp.Add(Productphar2);
        //                        PdfPCell cell53 = new PdfPCell(Productpp);
        //                        // cell53.AddElement(new Paragraph(new Chunk("" + data2[j].PRODUCT_Name + "\n", font)));
        //                        // cell53.AddElement(new Paragraph(new Chunk("" + data2[j].DesignDetail + "\n", font)));
        //                        cell53.HorizontalAlignment = 0;
        //                        cell53.PaddingTop = 0f;
        //                        // cell53.PaddingBottom = 2.5f;

        //                        table4.AddCell(cell53);
        //                    }
        //                    else
        //                    {
        //                        Paragraph Productpp = new Paragraph();
        //                        Phrase Productphar = new Phrase("" + data2[j].PRODUCT_Name + "\n", font);
        //                        Productpp.Add(Productphar);
        //                        PdfPCell cell53 = new PdfPCell(Productpp);

        //                        // cell53.AddElement(new Paragraph(new Chunk("" + data2[j].PRODUCT_Name + "", font)));
        //                        cell53.HorizontalAlignment = 0;
        //                        cell53.PaddingTop = 0f;
        //                        //cell53.PaddingTop = 2.5f;
        //                        // cell53.PaddingBottom = 2.5f;

        //                        table4.AddCell(cell53);
        //                    }

        //                    ////if (data1[i].SO_OrderType == 2 || data1[i].SO_OrderType == 3)
        //                    ////{
        //                    ////    Paragraph pp = new Paragraph();
        //                    ////    Phrase phar = new Phrase("" + Convert.ToDecimal(data2[j].PRICE).ToString("c2", CultureInfo.CreateSpecificCulture("de-DE")) + "\n", font);
        //                    ////    pp.Add(phar);
        //                    ////    PdfPCell cell541 = new PdfPCell(pp);
        //                    ////    //cell541.AddElement(pp);
        //                    ////    // cell541.Colspan = 2;
        //                    ////    cell541.HorizontalAlignment = 2;
        //                    ////    cell541.PaddingTop = 2.5f;
        //                    ////    cell541.PaddingBottom = 2.5f;

        //                    ////    table4.AddCell(cell541);
        //                    ////    decimal? DisPer = 0;
        //                    ////    if (data2[j].DiscountPer == 0)
        //                    ////    {
        //                    ////        DisPer = 0;
        //                    ////    }
        //                    ////    else
        //                    ////    {
        //                    ////        DisPer = data2[j].DiscountPer;
        //                    ////    }
        //                    ////    Paragraph ppd = new Paragraph();
        //                    ////    Phrase phard = new Phrase("" + DisPer + "\n", font);
        //                    ////    ppd.Add(phard);
        //                    ////    PdfPCell cell541d = new PdfPCell(ppd);
        //                    ////    //cell541.AddElement(pp);
        //                    ////    //cell541.Colspan = 2;
        //                    ////    cell541d.HorizontalAlignment = 2;
        //                    ////    cell541d.PaddingTop = 2.5f;
        //                    ////    cell541d.PaddingBottom = 2.5f;

        //                    ////    table4.AddCell(cell541d);
        //                    ////    decimal discountAmt = ((Convert.ToDecimal(data2[j].PRICE) * Convert.ToDecimal(data2[j].QUANTITY)) * Convert.ToDecimal(DisPer) / 100);
        //                    ////    Paragraph pp12 = new Paragraph();
        //                    ////    Phrase phar12 = new Phrase("" + ((Convert.ToDecimal(data2[j].PRICE) * Convert.ToDecimal(data2[j].QUANTITY)) - discountAmt).ToString("c2", CultureInfo.CreateSpecificCulture("de-DE")) + "\n", font);
        //                    ////    pp12.Add(phar12);
        //                    ////    PdfPCell cell54 = new PdfPCell(pp12);
        //                    ////    cell54.HorizontalAlignment = 2;
        //                    ////    cell54.PaddingTop = 2.5f;
        //                    ////    cell54.PaddingBottom = 2.5f;

        //                    ////    table4.AddCell(cell54);
        //                    ////    total = total + ((Convert.ToDecimal(data2[j].PRICE) * Convert.ToDecimal(data2[j].QUANTITY)) - discountAmt);
        //                    ////}
        //                    ////else
        //                    ////{
        //                    ////    Paragraph pp = new Paragraph();
        //                    ////    Phrase phar = new Phrase("" + Convert.ToDecimal(data2[j].PRICE).ToString("c2", CultureInfo.CreateSpecificCulture("de-DE")) + "\n", font);
        //                    ////    pp.Add(phar);
        //                    ////    PdfPCell cell541 = new PdfPCell(pp);
        //                    ////    //cell541.AddElement(pp);
        //                    ////    cell541.Colspan = 2;
        //                    ////    cell541.HorizontalAlignment = 2;
        //                    ////    cell541.PaddingTop = 2.5f;
        //                    ////    cell541.PaddingBottom = 2.5f;

        //                    ////    table4.AddCell(cell541);

        //                    ////    Paragraph pp12 = new Paragraph();
        //                    ////    Phrase phar12 = new Phrase("" + (Convert.ToDecimal(data2[j].PRICE) * Convert.ToDecimal(data2[j].QUANTITY)).ToString("c2", CultureInfo.CreateSpecificCulture("de-DE")) + "\n", font);
        //                    ////    pp12.Add(phar12);
        //                    ////    PdfPCell cell54 = new PdfPCell(pp12);
        //                    ////    cell54.HorizontalAlignment = 2;
        //                    ////    cell54.PaddingTop = 2.5f;
        //                    ////    cell54.PaddingBottom = 2.5f;

        //                    ////    table4.AddCell(cell54);
        //                    ////    total = total + (Convert.ToDecimal(data2[j].PRICE) * Convert.ToDecimal(data2[j].QUANTITY));
        //                    ////}


        //                }

        //                PdfPCell NetValCell = new PdfPCell();

        //                NetValCell.AddElement(new Paragraph(new Chunk("Net Value", font)));
        //                NetValCell.Colspan = 6;
        //                NetValCell.PaddingTop = 2.5f;
        //                NetValCell.PaddingBottom = 2.5f;

        //                table4.AddCell(NetValCell);
        //                Paragraph pp1 = new Paragraph();
        //                Phrase phar1 = new Phrase("" + Convert.ToDecimal(total).ToString("c2", CultureInfo.CreateSpecificCulture("de-DE")) + "\n", font);
        //                pp1.Add(phar1);
        //                PdfPCell NetValCell1 = new PdfPCell(pp1);
        //                NetValCell1.Colspan = 6;
        //                NetValCell1.HorizontalAlignment = 2;
        //                NetValCell1.VerticalAlignment = 2;
        //                NetValCell1.PaddingTop = 2.5f;
        //                NetValCell1.PaddingBottom = 2.5f;

        //                table4.AddCell(NetValCell1);

        //                if (data1[i].Zipcode == 82)
        //                {
        //                    PdfPCell inccell = new PdfPCell();

        //                    inccell.AddElement(new Paragraph(new Chunk(Convert.ToDecimal(data1[i].VatPer).ToString("0") + " % Value", font)));
        //                    inccell.Colspan = 6;
        //                    inccell.PaddingTop = 2.5f;
        //                    inccell.PaddingBottom = 2.5f;

        //                    table4.AddCell(inccell);
        //                    Paragraph pp1inc = new Paragraph();
        //                    Phrase phar1inc = new Phrase("" + (Convert.ToDecimal(total * data1[i].VatPer / 100).ToString("C2", CultureInfo.CreateSpecificCulture("de-DE"))) + "\n", font);
        //                    pp1inc.Add(phar1inc);
        //                    PdfPCell inccell1 = new PdfPCell(pp1inc);
        //                    inccell1.HorizontalAlignment = 2;
        //                    inccell1.VerticalAlignment = 2;
        //                    inccell1.PaddingTop = 2.5f;
        //                    inccell1.PaddingBottom = 2.5f;

        //                    table4.AddCell(inccell1);
        //                }
        //                else
        //                {

        //                    PdfPCell inccell = new PdfPCell();
        //                    inccell.AddElement(new Paragraph(new Chunk("0% VAT", font)));
        //                    inccell.Colspan = 6;
        //                    inccell.PaddingTop = 2.5f;
        //                    inccell.PaddingBottom = 2.5f;

        //                    table4.AddCell(inccell);
        //                    Paragraph pp1inc = new Paragraph();
        //                    Phrase phar1inc = new Phrase("" + (Convert.ToDecimal(total * 0 / 100).ToString("C2", CultureInfo.CreateSpecificCulture("de-DE"))) + "\n", font);
        //                    pp1inc.Add(phar1inc);
        //                    PdfPCell inccell1 = new PdfPCell(pp1inc);
        //                    inccell1.HorizontalAlignment = 2;
        //                    inccell1.PaddingTop = 2.5f;
        //                    inccell1.PaddingBottom = 2.5f;

        //                    table4.AddCell(inccell1);

        //                }
        //                decimal Discountamt = 0;

        //                if (data1[i].Zipcode == 82)
        //                {

        //                    var Vatamount = (Convert.ToDecimal(total + ((total * data1[i].VatPer / 100))));

        //                    if (data1[i].Discount != 0)
        //                    {
        //                        Discountamt = (Convert.ToDecimal(total * data1[i].Discount / 100));
        //                        PdfPCell AllDiscountcell = new PdfPCell();

        //                        AllDiscountcell.AddElement(new Paragraph(new Chunk("Discount(%)", font)));
        //                        AllDiscountcell.Colspan = 6;
        //                        AllDiscountcell.PaddingTop = 2.5f;
        //                        AllDiscountcell.PaddingBottom = 2.5f;

        //                        table4.AddCell(AllDiscountcell);
        //                        PdfPCell AllDiscountcellde = new PdfPCell();

        //                        AllDiscountcellde.AddElement(new Paragraph(new Chunk("" + Discountamt.ToString("C2", CultureInfo.CreateSpecificCulture("de-DE"))) + "", font));
        //                        AllDiscountcellde.Colspan = 6;
        //                        AllDiscountcellde.PaddingTop = 2.5f;
        //                        AllDiscountcellde.PaddingBottom = 2.5f;

        //                        table4.AddCell(AllDiscountcellde);
        //                    }
        //                    PdfPCell vataddcell = new PdfPCell();

        //                    vataddcell.AddElement(new Paragraph(new Chunk("Total", font)));
        //                    vataddcell.Colspan = 6;
        //                    vataddcell.PaddingTop = 2.5f;
        //                    vataddcell.PaddingBottom = 2.5f;

        //                    table4.AddCell(vataddcell);
        //                    Paragraph pp1vat = new Paragraph();
        //                    Phrase phar1vat = new Phrase("" + ((Convert.ToDecimal(total - Discountamt)).ToString("C2", CultureInfo.CreateSpecificCulture("de-DE"))) + "\n", Fontsmaller);
        //                    pp1vat.Add(phar1vat);
        //                    PdfPCell vataddcell1 = new PdfPCell(pp1vat);
        //                    vataddcell1.HorizontalAlignment = 2;
        //                    vataddcell1.Colspan = 6;
        //                    vataddcell1.PaddingTop = 2.5f;
        //                    vataddcell1.PaddingBottom = 2.5f;

        //                    table4.AddCell(vataddcell1);

        //                }
        //                else
        //                {
        //                    var Vatamount = (Convert.ToDecimal(total + ((total * 0 / 100))));

        //                    if (data1[i].Discount != 0)
        //                    {
        //                        Discountamt = (Convert.ToDecimal(total * data1[i].Discount / 100));
        //                        PdfPCell AllDiscountcell = new PdfPCell();

        //                        AllDiscountcell.AddElement(new Paragraph(new Chunk("Discount(%)", font)));
        //                        AllDiscountcell.Colspan = 6;
        //                        AllDiscountcell.PaddingTop = 2.5f;
        //                        AllDiscountcell.PaddingBottom = 2.5f;

        //                        table4.AddCell(AllDiscountcell);
        //                        PdfPCell AllDiscountcellde = new PdfPCell();

        //                        AllDiscountcellde.AddElement(new Paragraph(new Chunk("" + Discountamt.ToString("C2", CultureInfo.CreateSpecificCulture("de-DE"))) + "", font));
        //                        AllDiscountcellde.Colspan = 6;
        //                        AllDiscountcellde.PaddingTop = 2.5f;
        //                        AllDiscountcellde.PaddingBottom = 2.5f;

        //                        table4.AddCell(AllDiscountcellde);
        //                    }
        //                    PdfPCell vataddcell = new PdfPCell();

        //                    vataddcell.AddElement(new Paragraph(new Chunk("Total", font)));
        //                    vataddcell.Colspan = 6;
        //                    vataddcell.PaddingTop = 2.5f;
        //                    vataddcell.PaddingBottom = 2.5f;

        //                    table4.AddCell(vataddcell);
        //                    Paragraph pp1vat = new Paragraph();
        //                    Phrase phar1vat = new Phrase("" + ((Convert.ToDecimal(total - Discountamt)).ToString("C2", CultureInfo.CreateSpecificCulture("de-DE"))) + "\n", Fontsmaller);
        //                    pp1vat.Add(phar1vat);
        //                    PdfPCell vataddcell1 = new PdfPCell(pp1vat);
        //                    vataddcell1.HorizontalAlignment = 2;
        //                    vataddcell1.Colspan = 6;
        //                    vataddcell1.PaddingTop = 2.5f;
        //                    vataddcell1.PaddingBottom = 2.5f;

        //                    table4.AddCell(vataddcell1);

        //                }
        //                doc1.Add(table4);

        //                PdfPTable table6 = new PdfPTable(1);
        //                PdfPCell cell31 = new PdfPCell();
        //                cell31.Border = 0;
        //                cell31.AddElement(new Paragraph(new Chunk("", font)));
        //                cell31.PaddingTop = 20.5f;
        //                cell31.PaddingBottom = 2.5f;
        //                table6.AddCell(cell31);

        //                PdfPCell cell39 = new PdfPCell();
        //                cell39.Border = 0;
        //                cell39.AddElement(new Paragraph(""));
        //                //cell39.PaddingTop = 280.0f;
        //                cell39.PaddingBottom = 2.5f;
        //                table6.AddCell(cell39);
        //                doc1.Add(table6);

        //                PdfPTable table7 = new PdfPTable(1);
        //                table7.WidthPercentage = 100f;
        //                PdfPCell cell33 = new PdfPCell();
        //                //if (data2.Count > 11)
        //                //{
        //                //    doc1.NewPage();
        //                //}
        //                cell33.AddElement(new Paragraph(new Chunk("Payment Terms: " + data1[i].ProductName + "", font)));

        //                cell33.PaddingTop = 2.5f;
        //                cell33.PaddingBottom = 2.5f;
        //                cell33.Border = 0;
        //                table7.AddCell(cell33);
        //                PdfPCell delitercell = new PdfPCell();

        //                delitercell.AddElement(new Paragraph(new Chunk("Delivery Terms: " + data1[i].PL_Remarks + "", font)));

        //                delitercell.PaddingTop = 2.5f;
        //                delitercell.PaddingBottom = 2.5f;
        //                delitercell.Border = 0;
        //                table7.AddCell(delitercell);
        //                PdfPCell Linecell = new PdfPCell();

        //                Linecell.AddElement(new Paragraph(new Chunk("___________________________", font)));

        //                Linecell.PaddingTop = 45.5f;
        //                Linecell.PaddingBottom = 0f;
        //                Linecell.Border = 0;
        //                table7.AddCell(Linecell);
        //                PdfPCell signcell = new PdfPCell();

        //                signcell.AddElement(new Paragraph(new Chunk("Eurotextiles GmbH & Co. KG", font)));

        //                signcell.PaddingTop = 0f;
        //                signcell.PaddingBottom = 2.5f;
        //                signcell.Border = 0;
        //                table7.AddCell(signcell);
        //                PdfPCell line1 = new PdfPCell();

        //                line1.AddElement(new Paragraph(new Chunk("Es gelten die allgemeinen Geschäftsbedingungen der Eurotextiles GmbH & Co. KG.Abweichungen von Qualität / Stückzahl der Ware müssen uns vor Benutzung, aber in jedem Fall innerhalb von 7 Tagen nach Erhalt der Ware, schriftlich mitgeteilt werden.Ware die bereits gewaschen oder benutzt wurde ist von der Reklamation ausgeschlossen.", font)));

        //                line1.PaddingTop = 6.5f;
        //                line1.PaddingBottom = 2.5f;
        //                line1.Border = 0;
        //                table7.AddCell(line1);
        //                PdfPCell line2 = new PdfPCell();

        //                line2.AddElement(new Paragraph(new Chunk("Hinweis nach Bundesdatenschutzgesetz: Wir arbeiten mit EDV und haben Ihren Namen und die sonst im Rahmen der Angelegenheit benötigten Daten gespeichert. Eine Übermittlung an Dritte findet nicht statt.", font)));

        //                line2.PaddingTop = 2.5f;
        //                line2.PaddingBottom = 2.5f;
        //                line2.Border = 0;
        //                table7.AddCell(line2);
        //                PdfPCell line3 = new PdfPCell();

        //                //line3.AddElement(new Paragraph(new Chunk("No claims will be accepted after washing/using the goods.", font)));

        //                //line3.PaddingTop = 2.5f;
        //                //line3.PaddingBottom = 2.5f;
        //                //line3.Border = 0;
        //                //table7.AddCell(line3);
        //                doc1.Add(table7);

        //                doc1.Close();

        //            }
        //        }

        //        if (lang == "G")
        //        {
        //            for (int i = 0; i < data1.Count; i++)
        //            {

        //                var doc1 = new iTextSharp.text.Document(PageSize.A4, 20, 20, 130, 90);
        //                string subPath = "~/Sales/PDFList/PackingList/" + data1[i].PL_Code + "/";
        //                bool exists = System.IO.Directory.Exists(HttpContext.Server.MapPath(subPath));
        //                if (!exists)
        //                    System.IO.Directory.CreateDirectory(HttpContext.Server.MapPath(subPath));
        //                var output = new FileStream(Server.MapPath(subPath + data1[i].PL_Code + ".pdf"), FileMode.Create);
        //                var writer = PdfWriter.GetInstance(doc1, output);
        //                writer.PageEvent = new pdffooterclass();
        //                //PdfWriter.GetInstance(doc1, new FileStream(Request.PhysicalApplicationPath + "\\Invoice_Statement.pdf", FileMode.Create));
        //                doc1.Open();
        //                path = output.Name;
        //                //font size change from default  added by gv on 12/12/18
        //                FontFactory.RegisterDirectories();
        //                Font font = new Font(FontFactory.GetFont("Arial", 10, Font.NORMAL));
        //                Font fontsmall = new Font(FontFactory.GetFont("Arial", 9, Font.NORMAL));
        //                Font fontsmall1 = new Font(FontFactory.GetFont("Arial", 10, Font.NORMAL | Font.UNDERLINE));
        //                Font Fontbiggest = new Font(FontFactory.GetFont("Arial", 50, Font.BOLD, Color.BLUE));
        //                Font Fontbigger = new Font(FontFactory.GetFont("Arial", 20, Font.BOLD));
        //                Font Fontsmaller = new Font(FontFactory.GetFont("Arial", 10, Font.BOLD));
        //                Font Fontsmaller1 = new Font(FontFactory.GetFont("Arial", 12, Font.BOLD));



        //                PdfPTable table1 = new PdfPTable(1);
        //                table1.DefaultCell.Border = 0;
        //                table1.WidthPercentage = 100f;

        //                PdfPCell Title = new PdfPCell();
        //                Title.Border = 0;
        //                Title.AddElement(new Paragraph(new Chunk("Eurotextiles GmbH & Co. KG, Mammolshainer Weg 14, 61462 Königstein", fontsmall1)));
        //                Title.VerticalAlignment = 2;
        //                Title.PaddingTop = 1.0f;
        //                Title.PaddingBottom = 3.0f;
        //                table1.AddCell(Title);
        //                Paragraph pg2 = new Paragraph();
        //                Phrase phraseConstant2 = new Phrase("" + data1[i].CustomerName + "\n", font);
        //                Phrase phraseConstant3 = new Phrase("" + data1[i].Street + "\n", font);
        //                Phrase phraseConstant4 = new Phrase("" + data1[i].CityState + "\n", font);
        //                Phrase phraseConstant5 = new Phrase("" + data1[i].CountryZip + "\n", font);
        //                pg2.Add(phraseConstant2);
        //                pg2.Add(phraseConstant3);
        //                pg2.Add(phraseConstant4);
        //                pg2.Add(phraseConstant5);
        //                PdfPCell cell21 = new PdfPCell(pg2);
        //                cell21.HorizontalAlignment = 0;
        //                cell21.PaddingTop = 1.0f;
        //                cell21.PaddingLeft = 1.0f;
        //                cell21.Border = 0;
        //                table1.AddCell(cell21);
        //                doc1.Add(table1);

        //                PdfPTable table2 = new PdfPTable(3);
        //                table2.WidthPercentage = 100f;
        //                float[] widthsvalforcus = new float[] { 13f, 4f, 5f };
        //                table2.TotalWidth = 100f;
        //                table2.WidthPercentage = 100f;
        //                table2.SetWidths(widthsvalforcus);

        //                Phrase emp1 = new Phrase("", font);
        //                PdfPCell cell26emp = new PdfPCell(emp1);
        //                cell26emp.Border = 0;
        //                cell26emp.HorizontalAlignment = 2;
        //                cell26emp.PaddingBottom = 2.5f;
        //                table2.AddCell(cell26emp);
        //                Phrase cuscode = new Phrase("Customer Code:", font);
        //                PdfPCell cell26 = new PdfPCell(cuscode);
        //                cell26.Border = 0;
        //                cell26.HorizontalAlignment = 0;
        //                cell26.PaddingBottom = 2.5f;
        //                table2.AddCell(cell26);
        //                Phrase cuscodeval = new Phrase("" + data1[i].PL_CustomerID + "", font);
        //                PdfPCell cell26val = new PdfPCell(cuscodeval);
        //                cell26val.Border = 0;
        //                cell26val.HorizontalAlignment = 0;
        //                cell26val.PaddingBottom = 2.5f;
        //                table2.AddCell(cell26val);

        //                Phrase emp2 = new Phrase("", font);
        //                PdfPCell cell26emp1 = new PdfPCell(emp2);
        //                cell26emp1.Border = 0;
        //                cell26emp1.HorizontalAlignment = 2;
        //                cell26emp1.PaddingBottom = 2.5f;
        //                table2.AddCell(cell26emp1);
        //                Phrase Processby = new Phrase("Processed By:", font);
        //                PdfPCell cell26Processby = new PdfPCell(Processby);
        //                cell26Processby.Border = 0;
        //                cell26Processby.HorizontalAlignment = 0;
        //                cell26Processby.PaddingBottom = 2.5f;
        //                table2.AddCell(cell26Processby);
        //                Phrase cuscodeval1 = new Phrase("" + data1[i].CompanyName + "", font);
        //                PdfPCell cell26Processby1 = new PdfPCell(cuscodeval1);
        //                cell26Processby1.Border = 0;
        //                cell26Processby1.HorizontalAlignment = 0;
        //                cell26Processby1.PaddingBottom = 2.5f;
        //                table2.AddCell(cell26Processby1);

        //                Phrase emp3 = new Phrase("", font);
        //                PdfPCell cell26emp2 = new PdfPCell(emp3);
        //                cell26emp2.Border = 0;
        //                cell26emp2.HorizontalAlignment = 0;
        //                cell26emp2.PaddingBottom = 2.5f;
        //                table2.AddCell(cell26emp2);
        //                Phrase Datecell = new Phrase("Date:", font);
        //                PdfPCell cell26Datecell = new PdfPCell(Datecell);
        //                cell26Datecell.Border = 0;
        //                cell26Datecell.HorizontalAlignment = 0;
        //                cell26Datecell.PaddingBottom = 2.5f;
        //                table2.AddCell(cell26Datecell);
        //                Phrase cuscodeval2 = new Phrase("" + (data1[i].PL_Date ?? DateTime.Now).ToString("dd-MM-yyyy") + "", font);
        //                PdfPCell cell26Processby2 = new PdfPCell(cuscodeval2);
        //                cell26Processby2.Border = 0;
        //                cell26Processby2.HorizontalAlignment = 0;
        //                cell26Processby2.PaddingBottom = 2.5f;
        //                table2.AddCell(cell26Processby2);

        //                doc1.Add(table2);
        //                PdfPTable contenttable = new PdfPTable(1);
        //                contenttable.TotalWidth = 100f;
        //                contenttable.WidthPercentage = 100f;
        //                Paragraph pp1con = new Paragraph();
        //                Phrase phar1con = new Phrase("Order confirmation", Fontsmaller1);
        //                pp1con.Add(phar1con);
        //                PdfPCell cell29Con = new PdfPCell(pp1con);
        //                //cell29Con.AddElement(new Paragraph(new Chunk("" , Fontsmaller)));
        //                cell29Con.PaddingTop = 4.5f;
        //                cell29Con.HorizontalAlignment = 1;
        //                cell29Con.PaddingBottom = 5.5f;
        //                cell29Con.Border = 0;
        //                contenttable.AddCell(cell29Con);
        //                PdfPCell cell29 = new PdfPCell();
        //                cell29.AddElement(new Paragraph(new Chunk("Order confirmation No:" + data1[i].PL_Code + "", Fontsmaller)));
        //                cell29.PaddingTop = 6.5f;
        //                cell29.PaddingBottom = 20.5f;
        //                cell29.Border = 0;
        //                contenttable.AddCell(cell29);
        //                doc1.Add(contenttable);
        //                PdfPTable table4 = new PdfPTable(7);
        //                float[] widths1 = new float[] { 8f, 10f, 10f, 40f, 5f, 7f, 16f };
        //                table4.TotalWidth = 100f;
        //                table4.WidthPercentage = 100f;
        //                table4.HeaderRows = 1;
        //                table4.SetWidths(widths1);

        //                Phrase phraseConstantde1 = new Phrase("S.No", Fontsmaller);
        //                PdfPCell cell41 = new PdfPCell(phraseConstantde1);

        //                // cell41.AddElement(new Paragraph(new Chunk("Item", Fontsmaller)));

        //                cell41.HorizontalAlignment = 1;
        //                cell41.PaddingTop = 2.5f;
        //                cell41.PaddingBottom = 2.5f;

        //                table4.AddCell(cell41);
        //                Phrase phraseConstantde2 = new Phrase("Quantity", Fontsmaller);
        //                PdfPCell cell42 = new PdfPCell(phraseConstantde2);
        //                cell42.Colspan = 2;
        //                //cell42.AddElement(new Paragraph(new Chunk("Qty", Fontsmaller)));
        //                cell42.HorizontalAlignment = 1;
        //                cell42.PaddingTop = 2.5f;
        //                cell42.PaddingBottom = 2.5f;

        //                table4.AddCell(cell42);
        //                Phrase phraseConstantde3 = new Phrase("Product Description", Fontsmaller);
        //                PdfPCell cell421 = new PdfPCell(phraseConstantde3);
        //                cell421.HorizontalAlignment = 1;
        //                //cell421.AddElement(new Paragraph(new Chunk("UOM", Fontsmaller)));
        //                cell421.PaddingTop = 2.5f;
        //                cell421.PaddingBottom = 2.5f;

        //                table4.AddCell(cell421);
        //                Phrase phraseConstantde4 = new Phrase("Price", Fontsmaller);
        //                PdfPCell cell431 = new PdfPCell(phraseConstantde4);
        //                cell431.HorizontalAlignment = 1;
        //                cell431.Colspan = 2;
        //                //cell431.AddElement(new Paragraph(new Chunk("Product Name", Fontsmaller)));

        //                cell431.PaddingTop = 2.5f;
        //                cell431.PaddingBottom = 2.5f;

        //                table4.AddCell(cell431);
        //                Phrase phraseConstantde5 = new Phrase("Total", Fontsmaller);
        //                PdfPCell cell43 = new PdfPCell(phraseConstantde5);
        //                cell43.HorizontalAlignment = 1;

        //                // cell43.AddElement(new Paragraph(new Chunk("Description", Fontsmaller)));

        //                cell43.PaddingTop = 2.5f;
        //                cell43.PaddingBottom = 2.5f;

        //                table4.AddCell(cell43);
        //                decimal? total = 0;
        //                for (int j = 0; j < data2.Count; j++)
        //                {
        //                    Paragraph Snopp = new Paragraph();
        //                    Phrase Snophar = new Phrase("" + (j + 1) + "\n", font);
        //                    Snopp.Add(Snophar);

        //                    PdfPCell cell51 = new PdfPCell(Snopp);
        //                    cell51.HorizontalAlignment = 1;
        //                    table4.AddCell(cell51);
        //                    Paragraph qtypp = new Paragraph();
        //                    Phrase qtyphar = new Phrase("" + Convert.ToDecimal(data2[j].PD_NoOfPieces).ToString("N2", CultureInfo.CreateSpecificCulture("de-DE")) + "\n", font);
        //                    qtypp.Add(qtyphar);
        //                    PdfPCell cell52 = new PdfPCell(qtypp);
        //                    cell52.HorizontalAlignment = 2;
        //                    cell52.PaddingTop = 2.5f;
        //                    cell52.PaddingBottom = 2.5f;

        //                    table4.AddCell(cell52);
        //                    PdfPCell uomcell = new PdfPCell();

        //                    uomcell.AddElement(new Paragraph(new Chunk("" + data2[j].ProductName + "", font)));

        //                    //uomcell.PaddingTop = 2.5f;
        //                    // uomcell.PaddingBottom = 2.5f;

        //                    table4.AddCell(uomcell);

        //                    //if (data1[i].SO_OrderType == 2)
        //                    //{
        //                    //    Paragraph Productpp = new Paragraph();
        //                    //    Phrase Productphar = new Phrase("" + data2[j].PRODUCT_Name + "\n", font);
        //                    //    Phrase Productphar1 = new Phrase("" + data2[j].DesignDetail + "\n", font);
        //                    //    Productpp.Add(Productphar);
        //                    //    Productpp.Add(Productphar1);
        //                    //    PdfPCell cell53 = new PdfPCell(Productpp);
        //                    //    // cell53.AddElement(new Paragraph(new Chunk("" + data2[j].PRODUCT_Name + "\n", font)));
        //                    //    // cell53.AddElement(new Paragraph(new Chunk("" + data2[j].DesignDetail + "\n", font)));
        //                    //    cell53.HorizontalAlignment = 0;
        //                    //    cell53.PaddingTop = 0f;
        //                    //    // cell53.PaddingBottom = 2.5f;

        //                    //    table4.AddCell(cell53);
        //                    //}
        //                    //else
        //                    //{
        //                    //    Paragraph Productpp = new Paragraph();
        //                    //    Phrase Productphar = new Phrase("" + data2[j].PRODUCT_Name + "\n", font);
        //                    //    Productpp.Add(Productphar);
        //                    //    PdfPCell cell53 = new PdfPCell(Productpp);

        //                    //    // cell53.AddElement(new Paragraph(new Chunk("" + data2[j].PRODUCT_Name + "", font)));
        //                    //    cell53.HorizontalAlignment = 0;
        //                    //    cell53.PaddingTop = 0f;
        //                    //    //cell53.PaddingTop = 2.5f;
        //                    //    // cell53.PaddingBottom = 2.5f;

        //                    //    table4.AddCell(cell53);
        //                    //}

        //                    Paragraph pp = new Paragraph();
        //                    Phrase phar = new Phrase("" + Convert.ToDecimal(data2[j].PD_DesignNo).ToString("c2", CultureInfo.CreateSpecificCulture("de-DE")) + "\n", font);
        //                    pp.Add(phar);
        //                    PdfPCell cell541 = new PdfPCell(pp);
        //                    //cell541.AddElement(pp);
        //                    cell541.Colspan = 2;
        //                    cell541.HorizontalAlignment = 2;
        //                    cell541.PaddingTop = 2.5f;
        //                    cell541.PaddingBottom = 2.5f;

        //                    table4.AddCell(cell541);
        //                    Paragraph pp12 = new Paragraph();
        //                  //  Phrase phar12 = new Phrase("" + (Convert.ToDecimal(data2[j].PRICE) * Convert.ToDecimal(data2[j].QUANTITY)).ToString("c2", CultureInfo.CreateSpecificCulture("de-DE")) + "\n", font);
        //                    pp12.Add(phar12);
        //                    PdfPCell cell54 = new PdfPCell(pp12);
        //                    cell54.HorizontalAlignment = 2;
        //                    cell54.PaddingTop = 2.5f;
        //                    cell54.PaddingBottom = 2.5f;

        //                    table4.AddCell(cell54);
        //                   // total = total + (Convert.ToDecimal(data2[j].PRICE) * Convert.ToDecimal(data2[j].QUANTITY));
        //                }

        //                PdfPCell NetValCell = new PdfPCell();

        //                NetValCell.AddElement(new Paragraph(new Chunk("Gesamt Netto", font)));
        //                NetValCell.Colspan = 6;
        //                NetValCell.PaddingTop = 2.5f;
        //                NetValCell.PaddingBottom = 2.5f;

        //                table4.AddCell(NetValCell);
        //                Paragraph pp1 = new Paragraph();
        //                Phrase phar1 = new Phrase("" + Convert.ToDecimal(total).ToString("c2", CultureInfo.CreateSpecificCulture("de-DE")) + "\n", font);
        //                pp1.Add(phar1);
        //                PdfPCell NetValCell1 = new PdfPCell(pp1);
        //                NetValCell1.Colspan = 6;
        //                NetValCell1.HorizontalAlignment = 2;
        //                NetValCell1.PaddingTop = 2.5f;
        //                NetValCell1.PaddingBottom = 2.5f;

        //                table4.AddCell(NetValCell1);
        //                //if (data1[i].Zipcode == 82)
        //                //{
        //                //    PdfPCell inccell = new PdfPCell();

        //                //    inccell.AddElement(new Paragraph(new Chunk(Convert.ToDecimal(data1[i].VatPer).ToString("0") + " % Value", font)));
        //                //    inccell.Colspan = 6;
        //                //    inccell.PaddingTop = 2.5f;
        //                //    inccell.PaddingBottom = 2.5f;

        //                //    table4.AddCell(inccell);
        //                //    Paragraph pp1inc = new Paragraph();
        //                //    Phrase phar1inc = new Phrase("" + (Convert.ToDecimal(total * data1[i].VatPer / 100).ToString("C2", CultureInfo.CreateSpecificCulture("de-DE"))) + "\n", font);
        //                //    pp1inc.Add(phar1inc);
        //                //    PdfPCell inccell1 = new PdfPCell(pp1inc);
        //                //    inccell1.VerticalAlignment = 2;
        //                //    inccell1.PaddingTop = 2.5f;
        //                //    inccell1.PaddingBottom = 2.5f;

        //                //    table4.AddCell(inccell1);
        //                //}
        //                //else
        //                //{

        //                //    PdfPCell inccell = new PdfPCell();
        //                //    inccell.AddElement(new Paragraph(new Chunk("0% VAT", font)));
        //                //    inccell.Colspan = 6;
        //                //    inccell.PaddingTop = 2.5f;
        //                //    inccell.PaddingBottom = 2.5f;

        //                //    table4.AddCell(inccell);
        //                //    Paragraph pp1inc = new Paragraph();
        //                //    Phrase phar1inc = new Phrase("" + (Convert.ToDecimal(total * 0 / 100).ToString("C2", CultureInfo.CreateSpecificCulture("de-DE"))) + "\n", font);
        //                //    pp1inc.Add(phar1inc);
        //                //    PdfPCell inccell1 = new PdfPCell(pp1inc);
        //                //    inccell1.VerticalAlignment = 2;
        //                //    inccell1.PaddingTop = 2.5f;
        //                //    inccell1.PaddingBottom = 2.5f;

        //                //    table4.AddCell(inccell1);

        //                //}

        //                //;
        //                //decimal Discountamt = 0;
        //                //if (data1[i].Zipcode == 82)
        //                //{

        //                //    var Vatamount = (Convert.ToDecimal(total + ((total * data1[i].VatPer / 100))));

        //                //    if (data1[i].Discount != 0)
        //                //    {
        //                //        Discountamt = (Convert.ToDecimal(total * data1[i].Discount / 100));
        //                //        PdfPCell AllDiscountcell = new PdfPCell();

        //                //        AllDiscountcell.AddElement(new Paragraph(new Chunk("Discount(%)", font)));
        //                //        AllDiscountcell.Colspan = 6;
        //                //        AllDiscountcell.PaddingTop = 2.5f;
        //                //        AllDiscountcell.PaddingBottom = 2.5f;

        //                //        table4.AddCell(AllDiscountcell);
        //                //        PdfPCell AllDiscountcellde = new PdfPCell();

        //                //        AllDiscountcellde.AddElement(new Paragraph(new Chunk("" + Discountamt.ToString("C2", CultureInfo.CreateSpecificCulture("de-DE"))) + "", font));
        //                //        AllDiscountcellde.Colspan = 6;
        //                //        AllDiscountcellde.PaddingTop = 2.5f;
        //                //        AllDiscountcellde.PaddingBottom = 2.5f;

        //                //        table4.AddCell(AllDiscountcellde);
        //                //    }
        //                //    PdfPCell vataddcell = new PdfPCell();

        //                //    vataddcell.AddElement(new Paragraph(new Chunk("Total", font)));
        //                //    vataddcell.Colspan = 6;
        //                //    vataddcell.PaddingTop = 2.5f;
        //                //    vataddcell.PaddingBottom = 2.5f;

        //                //    table4.AddCell(vataddcell);
        //                //    Paragraph pp1vat = new Paragraph();
        //                //    Phrase phar1vat = new Phrase("" + ((Convert.ToDecimal(total - Discountamt)).ToString("C2", CultureInfo.CreateSpecificCulture("de-DE"))) + "\n", Fontsmaller);
        //                //    pp1vat.Add(phar1vat);
        //                //    PdfPCell vataddcell1 = new PdfPCell(pp1vat);
        //                //    vataddcell1.HorizontalAlignment = 2;
        //                //    vataddcell1.Colspan = 6;
        //                //    vataddcell1.PaddingTop = 2.5f;
        //                //    vataddcell1.PaddingBottom = 2.5f;

        //                //    table4.AddCell(vataddcell1);

        //                //}
        //                //else
        //                //{
        //                //    var Vatamount = (Convert.ToDecimal(total + ((total * 0 / 100))));

        //                //    if (data1[i].Discount != 0)
        //                //    {
        //                //        Discountamt = (Convert.ToDecimal(total * data1[i].Discount / 100));
        //                //        PdfPCell AllDiscountcell = new PdfPCell();

        //                //        AllDiscountcell.AddElement(new Paragraph(new Chunk("Discount(%)", font)));
        //                //        AllDiscountcell.Colspan = 6;
        //                //        AllDiscountcell.PaddingTop = 2.5f;
        //                //        AllDiscountcell.PaddingBottom = 2.5f;

        //                //        table4.AddCell(AllDiscountcell);
        //                //        PdfPCell AllDiscountcellde = new PdfPCell();

        //                //        AllDiscountcellde.AddElement(new Paragraph(new Chunk("" + Discountamt.ToString("C2", CultureInfo.CreateSpecificCulture("de-DE"))) + "", font));
        //                //        AllDiscountcellde.Colspan = 6;
        //                //        AllDiscountcellde.PaddingTop = 2.5f;
        //                //        AllDiscountcellde.PaddingBottom = 2.5f;

        //                //        table4.AddCell(AllDiscountcellde);
        //                //    }
        //                    PdfPCell vataddcell = new PdfPCell();

        //                    vataddcell.AddElement(new Paragraph(new Chunk("Total", font)));
        //                    vataddcell.Colspan = 6;
        //                    vataddcell.PaddingTop = 2.5f;
        //                    vataddcell.PaddingBottom = 2.5f;

        //                    table4.AddCell(vataddcell);
        //                    Paragraph pp1vat = new Paragraph();
        //                    Phrase phar1vat = new Phrase("" + ((Convert.ToDecimal(total - Discountamt)).ToString("C2", CultureInfo.CreateSpecificCulture("de-DE"))) + "\n", Fontsmaller);
        //                    pp1vat.Add(phar1vat);
        //                    PdfPCell vataddcell1 = new PdfPCell(pp1vat);
        //                    vataddcell1.HorizontalAlignment = 2;
        //                    vataddcell1.Colspan = 6;
        //                    vataddcell1.PaddingTop = 2.5f;
        //                    vataddcell1.PaddingBottom = 2.5f;

        //                    table4.AddCell(vataddcell1);

        //                }
        //                doc1.Add(table4);

        //                PdfPTable table6 = new PdfPTable(1);
        //                PdfPCell cell31 = new PdfPCell();
        //                cell31.Border = 0;
        //                cell31.AddElement(new Paragraph(new Chunk("", font)));
        //                cell31.PaddingTop = 20.5f;
        //                cell31.PaddingBottom = 2.5f;
        //                table6.AddCell(cell31);

        //                PdfPCell cell39 = new PdfPCell();
        //                cell39.Border = 0;
        //                cell39.AddElement(new Paragraph(""));
        //                //cell39.PaddingTop = 280.0f;
        //                cell39.PaddingBottom = 2.5f;
        //                table6.AddCell(cell39);
        //                doc1.Add(table6);

        //                PdfPTable table7 = new PdfPTable(1);
        //                table7.WidthPercentage = 100f;
        //                PdfPCell cell33 = new PdfPCell();
        //                //if (data2.Count > 11)
        //                //{
        //                //    doc1.NewPage();
        //                //}
        //                cell33.AddElement(new Paragraph(new Chunk("Payment Terms: " + data1[i].PaymentTermsDescription + "", font)));

        //                cell33.PaddingTop = 2.5f;
        //                cell33.PaddingBottom = 2.5f;
        //                cell33.Border = 0;
        //                table7.AddCell(cell33);
        //                PdfPCell delitercell = new PdfPCell();

        //                delitercell.AddElement(new Paragraph(new Chunk("Delivery Terms: " + data1[i].SO_CusDeliveryTerms + "", font)));

        //                delitercell.PaddingTop = 2.5f;
        //                delitercell.PaddingBottom = 2.5f;
        //                delitercell.Border = 0;
        //                table7.AddCell(delitercell);
        //                PdfPCell signcell = new PdfPCell();

        //                signcell.AddElement(new Paragraph(new Chunk("Eurotextiles GmbH & Co. KG", font)));

        //                signcell.PaddingTop = 45.5f;
        //                signcell.PaddingBottom = 2.5f;
        //                signcell.Border = 0;
        //                table7.AddCell(signcell);
        //                PdfPCell line1 = new PdfPCell();

        //                line1.AddElement(new Paragraph(new Chunk("Es gelten die Allgemeinen Geschäftsbedingungen (AGB) der Eurotextiles GmbH & Co. KG.", font)));

        //                line1.PaddingTop = 6.5f;
        //                line1.PaddingBottom = 2.5f;
        //                line1.Border = 0;
        //                table7.AddCell(line1);
        //                PdfPCell line2 = new PdfPCell();

        //                line2.AddElement(new Paragraph(new Chunk("Abweichungen von Qualität/Stückzahl der Ware müssen uns vor Benutzung, aber in jedem Fall innerhalb von 7 Tagen nach Erhalt der Ware, schriftlich mitgeteilt werden.", font)));

        //                line2.PaddingTop = 2.5f;
        //                line2.PaddingBottom = 2.5f;
        //                line2.Border = 0;
        //                table7.AddCell(line2);
        //                PdfPCell line3 = new PdfPCell();

        //                line3.AddElement(new Paragraph(new Chunk("Ware die bereits gewaschen oder benutzt wurde ist von der Reklamation ausgeschlossen.", font)));

        //                line3.PaddingTop = 2.5f;
        //                line3.PaddingBottom = 2.5f;
        //                line3.Border = 0;
        //                table7.AddCell(line3);
        //                doc1.Add(table7);

        //                doc1.Close();

        //            }
        //        }

        //        return Json(data1, JsonRequestBehavior.AllowGet);
        //        //return path;
        //    }
        //    catch (Exception exe)
        //    {
        //        transaction.Rollback();
        //        string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
        //        string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
        //        objERR.err_title = controllerName + "-" + controllerName;
        //        objERR.err_message = "Sorry for the inconvience. Some error has been occured.";
        //        objERR.err_details = exe.Message.Replace("'", "");
        //        int errid = bal.ExceptionInsertLogs_BL(objERR);
        //        return Json("ERR" + errid.ToString(), JsonRequestBehavior.AllowGet);
        //        // return "ERR" + errid.ToString();
        //    }
        //    finally
        //    {
        //        transaction.Dispose();
        //        context.Dispose();
        //    }

        //}
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
        //Validate the product
        private string Validations(int PL_ID, string PL_Code, string PL_Date, string PL_OrderNo, string PL_Remarks,string packingType,string shipmentCode, string EnquiryDetails)
        {
            if (!automanual && PL_Code == "")
            {
                return "Enter Packing list Code";
            }
            if (PL_Date == "")
            {
                return "Enter Packing list Code";
            }
            if (PL_OrderNo == "")
            {
                return "Select Order No";
            }
            if (PL_Remarks == "")
            {
                return "Enter Remarks";
            }
            if(shipmentCode  == "")
            {
                return "Shipment Code is Empty.";
            }
            if (!automanual)
            {
                if (PL_ID == 0)
                {
                    var count = dbcontext.Tbl_Packing_List.Where(m => m.PL_Code == PL_Code && m.PL_Type == 2).ToList().Count();
                    if (count > 0)
                    {
                        return "Packing list Code Already Exist";
                    }
                }
                else
                {
                    var count = dbcontext.Tbl_Packing_List.Where(m => m.PL_ID != PL_ID && m.PL_Code == PL_Code &&  m.PL_Type == 2).ToList().Count();
                    if (count > 0)
                    {
                        return "Packing list Code Already Exist";
                    }
                }
            }
            return "";
        }
        //Insert/update the packing list
        [HttpPost]
        public JsonResult ET_Trading_PackingList_Add(int PL_ID, string PL_Code, string PL_Date,int PL_CustomerID, string PL_OrderNo, string PL_Remarks, string packingType,string shipmentCode,string packingSummary,string EnquiryDetails)
        {
            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                {
                    var packingListType = 1;
                    if (packingType == "1")
                        packingListType = 1;
                    else if (packingType == "2")
                        packingListType = 2;
                    else
                        packingListType = 3;

                    string valid = Validations(PL_ID, PL_Code, PL_Date, PL_OrderNo, PL_Remarks, packingType, shipmentCode, EnquiryDetails);
                    if (valid == "")
                    {
                        var Username = Session["UserID"].ToString();
                        DateTime PLDate = DateTime.ParseExact(PL_Date,"dd-MM-yyyy",null);
                        Tbl_Packing_List Objmc = new Tbl_Packing_List()
                        {
                            PL_ID = PL_ID,
                            PL_Code = PL_Code,
                            PL_Date = PLDate,
                            PL_CustomerID = PL_CustomerID,
                            PL_OrderNo = PL_OrderNo,
                            PL_Remarks = PL_Remarks,
                            CREATED_BY = Convert.ToInt32(Session["UserID"].ToString()),
                            LAST_UPDATED_BY = Convert.ToInt32(Session["UserID"].ToString()),
                            CREATED_DATE = DateTime.Now,
                            DELETED = false,
                            COM_KEY = Convert.ToInt32(Session["CompanyKey"]),
                            PL_Type = packingListType,
                            PL_ShipmentCode = shipmentCode,
                            PL_Summary = packingSummary
                        };

                        decimal k = 0;
                        var context = new EntityClasses();
                        var transaction = context.Database.BeginTransaction();
                        bool success = true;
                        try
                        {
                            if (PL_ID == 0)
                            {
                                context.Tbl_Packing_List.Add(Objmc);
                                if (context.SaveChanges() == 0)
                                {
                                    success = false;
                                }
                                k = Objmc.PL_ID;
                                if (automanual == true)
                                {
                                    int len = 10 - (prefix + Objmc.PL_ID).Length;
                                    string code = prefix + new String('0', len) + Objmc.PL_ID;
                                    Tbl_Packing_List Obj_Tbl_Packing_List = context.Tbl_Packing_List.Single(m => m.PL_ID == Objmc.PL_ID);
                                    {
                                        Obj_Tbl_Packing_List.PL_Code = code;
                                    };
                                    if (context.SaveChanges() == 0)
                                    {
                                        success = false;
                                    }
                                    PL_Code = code;
                                }
                            }
                            else
                            {
                                Tbl_Packing_List Obj_Tbl_Packing_List = context.Tbl_Packing_List.Single(m => m.PL_ID == Objmc.PL_ID);
                                {
                                    Obj_Tbl_Packing_List.PL_Code = Objmc.PL_Code;
                                    Obj_Tbl_Packing_List.PL_Date = Objmc.PL_Date;
                                    Obj_Tbl_Packing_List.PL_CustomerID = Objmc.PL_CustomerID;
                                    Obj_Tbl_Packing_List.PL_OrderNo = Objmc.PL_OrderNo;
                                    Obj_Tbl_Packing_List.PL_Remarks = Objmc.PL_Remarks;
                                    Obj_Tbl_Packing_List.DELETED = Objmc.DELETED;
                                    Obj_Tbl_Packing_List.COM_KEY = Objmc.COM_KEY;
                                    Obj_Tbl_Packing_List.LAST_UPDATED_DATE = DateTime.Now;
                                    Obj_Tbl_Packing_List.LAST_UPDATED_BY = Objmc.LAST_UPDATED_BY;
                                    Obj_Tbl_Packing_List.PL_Type = Objmc.PL_Type;
                                    Obj_Tbl_Packing_List.PL_ShipmentCode = Objmc.PL_ShipmentCode;
                                    Obj_Tbl_Packing_List.PL_Summary = Objmc.PL_Summary;
                                };
                                if (context.SaveChanges() == 0)
                                {
                                    success = false;
                                }
                                k = Objmc.PL_ID;
                            }
                            // Delete previous contact data
                            Tbl_Packing_ListDetails objdeletecontact = new Tbl_Packing_ListDetails();
                            context.Tbl_Packing_ListDetails.RemoveRange(context.Tbl_Packing_ListDetails.Where(m => m.PD_PID == Objmc.PL_ID));
                            context.SaveChanges();

                            string[] ChildRow = EnquiryDetails.Split('|');

                            
                            // Insert new contacts data
                            
                            for (int i = 0; i < ChildRow.Length - 1; i++)
                            {
                                string[] ChildRecord = ChildRow[i].Split(',');
                                Tbl_Packing_ListDetails objStoredetails;
                                if (packingType == "1")
                                {
                                    objStoredetails = new Tbl_Packing_ListDetails()
                                    {
                                        PD_PID = Convert.ToInt32(Objmc.PL_ID),
                                        PD_ArticleNo = ChildRecord[1],
                                        PD_ProductID = Convert.ToDecimal(ChildRecord[0]),
                                        PD_PalletNo = ChildRecord[3],
                                        PD_NoOfCones = ChildRecord[4],
                                        PD_LotNo = ChildRecord[5],
                                        PD_GwtinKGS = ChildRecord[6],
                                        PD_NwtinKGS = ChildRecord[7],
                                        PD_Remarks = ChildRecord[8]
                                    };
                                }
                                else if(packingType == "2")
                                {
                                    objStoredetails = new Tbl_Packing_ListDetails()
                                    {
                                        PD_PID = Convert.ToInt32(Objmc.PL_ID),
                                        PD_ArticleNo = ChildRecord[1],
                                        PD_ProductID = Convert.ToDecimal(ChildRecord[0]),
                                        PD_PalletNo = ChildRecord[3],
                                        PD_DesignNo = ChildRecord[4],
                                        PD_NoOfPieces = ChildRecord[5],
                                        PD_IndividualPieceLength = ChildRecord[6],
                                        PD_TotalMeters = ChildRecord[7],
                                        PD_GwtinKGS = ChildRecord[8],
                                        PD_NwtinKGS = ChildRecord[9],
                                        PD_Remarks = ChildRecord[10]
                                    };
                                }
                                else
                                {
                                    objStoredetails = new Tbl_Packing_ListDetails()
                                    {
                                        PD_PID = Convert.ToInt32(Objmc.PL_ID),
                                        PD_ArticleNo = ChildRecord[1],
                                        PD_ProductID = Convert.ToDecimal(ChildRecord[0]),
                                        PD_NoOfPieces = ChildRecord[3],
                                        PD_PackingUnits = ChildRecord[4],
                                        PD_GwtinKGS = ChildRecord[5],
                                        PD_NwtinKGS = ChildRecord[6],
                                        PD_Remarks = ChildRecord[7]
                                    };
                                }
                                context.Tbl_Packing_ListDetails.Add(objStoredetails);
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
                                k = 0;
                                transaction.Rollback();
                            }
                        }
                        catch (Exception ex)
                        {
                            k = 0;
                            transaction.Rollback();
                            throw ex;
                        }
                        finally
                        {
                            transaction.Dispose();
                            context.Dispose();
                        }
                        var json = "Success:"+ PL_Code;
                        if (k == 0)
                        {
                            json = "Failed";
                        }
                        else
                        {
                            objLOG.log_dockey = "8014";
                            objLOG.log_userid = Session["UserID"].ToString();
                            objLOG.log_recordkey = k.ToString();
                            if (PL_ID == 0)
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
        //View: Popup view
        public ActionResult ET_Trading_PackingList_View(int id)
        {
            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                {
                    dbcontext.Configuration.ProxyCreationEnabled = false;
                    var OrderIDs = new HashSet<decimal>(dbcontext.Tbl_Packing_List.Single(f => f.PL_ID == id).PL_OrderNo.Split(',').Select(m => Convert.ToDecimal(m)).ToList());
                    var OrderCodes = string.Join(",", (from m in dbcontext.Tbl_Master_Order where OrderIDs.Contains(m.SO_ID) select m.SO_Code).ToList());
                    var data1 = (from a in dbcontext.Tbl_Packing_List
                                 join b in dbcontext.Tbl_Master_CompanyDetails on a.PL_CustomerID equals b.COM_ID into comp
                                 from x in comp
                                 where a.PL_ID == id
                                 select new PackingList_ViewCM
                                 {
                                     PL_ID = a.PL_ID,
                                     PL_Code = a.PL_Code,
                                     PL_Date = a.PL_Date,
                                     Customer = x.COM_NAME,
                                     PL_CustomerID = a.PL_CustomerID,
                                     Orders = OrderCodes,
                                     PL_Remarks = a.PL_Remarks,
                                     PL_Type = a.PL_Type,
                                     OTOI_ShipmentID = dbcontext.Tbl_Shipment_Header.Where(s => s.S_Code == a.PL_ShipmentCode).Select(a => a.S_ID).FirstOrDefault(),
                                 }
                            ).ToList();
                    var data2 = (from c in dbcontext.Tbl_Packing_ListDetails
                                 join a in dbcontext.Tbl_Product_Master on c.PD_ProductID equals a.P_ID
                                 join b in dbcontext.tbl_LookUp on a.P_UOM equals b.LU_Code
                                 into m
                                 from n in m.DefaultIfEmpty()
                                 where c.PD_PID == id && n.LU_Type == 2
                                 select new PackingList_ViewCM
                                 {
                                     PD_ArticleNo = a.P_ArticleNo,
                                     PD_ProductID = a.P_ID,
                                     ProductName = a.P_ShortName,
                                     PD_PalletNo = c.PD_PalletNo,
                                     PD_LotNr = c.PD_LotNo,
                                     PD_ConesNr = c.PD_NoOfCones,
                                     PD_PackingUnits = c.PD_PackingUnits,
                                     PD_DesignNo = c.PD_DesignNo,
                                     PD_NoOfPieces = c.PD_NoOfPieces,
                                     PD_TotalMeters = c.PD_TotalMeters,
                                     PD_NwtinKGS = c.PD_NwtinKGS,
                                     PD_GwtinKGS = c.PD_GwtinKGS,
                                     PD_IndividualPieceLength = c.PD_IndividualPieceLength,
                                     PL_Remarks = c.PD_Remarks
                                 }).ToList();
                    Paclistdetails_View_CM obj = new Paclistdetails_View_CM();
                    obj.QHeader = data1;
                    obj.QChild = data2;
                    return PartialView("/Views/Trading/ET_Trading_PackingList/ET_Trading_PackingListView.cshtml", obj);
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
        //Restore the data
        public ActionResult ET_Trading_PackingList_RestoreDelete(int id, bool type)
        {
            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                {
                    int i;
                    Tbl_Packing_List deleted = dbcontext.Tbl_Packing_List.Single(m => m.PL_ID == id && m.PL_Type ==2);
                    deleted.DELETED = type;
                    i = dbcontext.SaveChanges();
                    var json = "Failed";
                    if (i != 0)
                    {
                        json = "Success";
                    }
                    else
                    {
                        objLOG.log_dockey = "8021";
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
        //Edit the data
        public ActionResult ET_Trading_PackingList_Update_GetbyID(int id)
        {
            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                {
                    var data = dbcontext.Tbl_Packing_List.Single(m => m.PL_ID == id);
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
        //Get the product details for product
        public ActionResult ET_Trading_Packing_Details_Load(int id)
        {
            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                {
                    var data1 = (from c in dbcontext.Tbl_Packing_ListDetails
                                 join a in dbcontext.Tbl_Product_Master on c.PD_ProductID equals a.P_ID
                                 join b in dbcontext.tbl_LookUp on a.P_UOM equals b.LU_Code
                                 into m
                                 from n in m.DefaultIfEmpty()
                                 where c.PD_PID == id && n.LU_Type == 2
                                 select new
                                 {
                                     productid = a.P_ID,
                                     name = a.P_ShortName,
                                     palletno = c.PD_PalletNo,
                                     designno = c.PD_DesignNo,
                                     noofcones = c.PD_NoOfCones,
                                     lotno = c.PD_LotNo,
                                     remarks = c.PD_Remarks,
                                     packingunits = c.PD_PackingUnits,
                                     noofpeices = c.PD_NoOfPieces,
                                     totalmeters = c.PD_TotalMeters,
                                     nwtinkgs = c.PD_NwtinKGS,
                                     gwtinkgs = c.PD_GwtinKGS,
                                     individualpiecelemgth = c.PD_IndividualPieceLength
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
                return RedirectToAction("ET_SessionExpire", "ET_Login");
            }
        }
        //Get the product details
        public JsonResult GetProductDetailsByID(int id)
        {

            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                {
                    dbcontext.Configuration.ProxyCreationEnabled = false;
                    var data1 = (from a in dbcontext.Tbl_Product_Master
                                 join b in dbcontext.tbl_LookUp on a.P_UOM equals b.LU_Code
                                 into m
                                 from n in m.DefaultIfEmpty()
                                 where a.P_ID == id && n.LU_Type == 2
                                 select new
                                 {
                                     name = a.P_ShortName,
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
        //Print the packing list
        public ActionResult ET_Master_PackingList_Print(int id, string lang)
        {
            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                {
                    dbcontext.Configuration.ProxyCreationEnabled = false;
                    int com_key = Convert.ToInt32(Session["Companykey"]);
                    var OrderIDs = new HashSet<decimal>(dbcontext.Tbl_Packing_List.Single(f => f.PL_ID == id).PL_OrderNo.Split(',').Select(m => Convert.ToDecimal(m)).ToList());
                    var OrderCodes = string.Join(",", (from m in dbcontext.Tbl_Master_Order where OrderIDs.Contains(m.SO_ID) select m.SO_Code).ToList());
                    var packingItem = dbcontext.Tbl_Packing_List.Where(a => a.PL_ID == id).FirstOrDefault();
                    var data1 = (from a in dbcontext.Tbl_Packing_List
                                 join b in dbcontext.Tbl_Master_CompanyDetails on a.PL_CustomerID equals b.COM_ID into comp
                                 from x in comp
                                 where a.PL_ID == id
                                 select new PackingList_ViewCM
                                 {
                                     PL_ID = a.PL_ID,
                                     PL_Code = a.PL_Code,
                                     PL_Date = a.PL_Date,
                                     PL_Remarks = a.PL_Remarks,
                                     PL_Type = a.PL_Type,
                                     PL_CustomerID = a.PL_CustomerID,
                                     CustomerName = x.COM_DISPLAYNAME,
                                     Orders = OrderCodes,
                                     CompanyCode = x.COM_CODE,
                                     CompanyName = x.COM_NAME,
                                     Street = x.COM_STREET,
                                     CityState = (x.COM_CITY + ", " + x.COM_STATE),
                                     USTID = x.COM_USTID,
                                     CountryZip = ((dbcontext.locations.Where(a => a.location_id == x.COM_COUNTRY).Select(a => a.name).FirstOrDefault()) + ", " + (x.COM_ZIP)),
                                     VatPer = (dbcontext.Tbl_SystemSetUp.Where(a => a.COMPANY_ID == com_key).Select(a => a.TAX).FirstOrDefault()),
                                     imgurl = (dbcontext.Tbl_SystemSetUp.Where(a => a.COMPANY_ID == com_key).Select(a => a.COMPANY_LOGO).FirstOrDefault()),
                                     SystemCompany = (dbcontext.Tbl_SystemSetUp.Where(a => a.COMPANY_ID == com_key).Select(a => a.COMPANY_NAME).FirstOrDefault()),
                                     OTOI_ShipmentID = dbcontext.Tbl_Shipment_Header.Where(s=>s.S_Code == packingItem.PL_ShipmentCode).Select(a=>a.S_ID).FirstOrDefault(),
                                     OTOI_ContainerNo = dbcontext.Tbl_Shipment_Header.Where(s => s.S_Code == packingItem.PL_ShipmentCode).Select(a => a.S_ContainerNo).FirstOrDefault(),
                                 }
                            ).ToList();

                    var data2 = (from c in dbcontext.Tbl_Packing_ListDetails
                                 join a in dbcontext.Tbl_Product_Master on c.PD_ProductID equals a.P_ID
                                 join b in dbcontext.tbl_LookUp on a.P_UOM equals b.LU_Code
                                 into m
                                 from n in m.DefaultIfEmpty()
                                 where c.PD_PID == id && n.LU_Type == 2
                                 select new PackingList_ViewCM
                                 {
                                     PD_ArticleNo = a.P_ArticleNo,
                                     PD_ProductID = a.P_ID,
                                     ProductName=a.P_ShortName,
                                     PD_DesignNo = c.PD_DesignNo,
                                     PD_PalletNo = c.PD_PalletNo,
                                     PD_NoOfPieces = c.PD_NoOfPieces,
                                     PD_TotalMeters = c.PD_TotalMeters,
                                     PD_IndividualPieceLength = c.PD_IndividualPieceLength,
                                     PD_ConesNr = c.PD_NoOfCones,
                                     PD_LotNr = c.PD_LotNo,
                                     PD_PackingUnits = c.PD_PackingUnits,
                                     PD_NwtinKGS = c.PD_NwtinKGS,
                                     PD_GwtinKGS = c.PD_GwtinKGS,
                                     PL_Remarks = c.PD_Remarks,
                                 }).OrderBy(m=>m.PD_ProductID).ToList();
                    Paclistdetails_View_CM obj = new Paclistdetails_View_CM();
                    obj.QHeader = data1;
                    obj.QChild = data2;
                    if (lang == "E")
                        return PartialView("/Views/Trading/ET_Trading_PackingList/ET_Trading_PackingList_Print.cshtml", obj);
                    else
                        return PartialView("/Views/Trading/ET_Trading_PackingList/ET_Trading_PackingList_Print_German.cshtml", obj);
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

        [HttpPost]
        public ActionResult ReadExcel()
        {
            object packingList = null;
            string packingType = "1";
            if (ModelState.IsValid)
            {
                bool val = Session["UserID"] == null ? false : true;
                if (val)
                {

                    string filePath = string.Empty;
                    if (Request != null)
                    {
                        HttpPostedFileBase file = Request.Files["file"];
                        packingType = Request.Form.GetValues("packingType")[0].ToString();
                        if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
                        {

                            string fileName = file.FileName;
                            string fileContentType = file.ContentType;
                            string path = Server.MapPath("~/Uploads/");
                            if (!Directory.Exists(path))
                            {
                                Directory.CreateDirectory(path);
                            }
                            filePath = path + Path.GetFileName(file.FileName);
                            string extension = Path.GetExtension(file.FileName);
                            file.SaveAs(filePath);
                            Stream stream = file.InputStream;
                            // We return the interface, so that
                            IExcelDataReader reader = null;
                            if (file.FileName.EndsWith(".xls"))
                            {
                                reader = ExcelReaderFactory.CreateBinaryReader(stream);
                            }
                            else if (file.FileName.EndsWith(".xlsx"))
                            {
                                reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                            }
                            else
                            {
                                ModelState.AddModelError("File", "This file format is not supported");
                                return RedirectToAction("Index");
                            }

                            var result = reader.AsDataSet(new ExcelDataSetConfiguration()
                            {
                                ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                                {
                                    UseHeaderRow = true
                                }
                            });

                            //reader.IsFirstRowAsColumnNames = true;
                            //DataSet result = reader.AsDataSet();
                            reader.Close();
                            //delete the file from physical path after reading 
                            string filedetails = path + fileName;
                            FileInfo fileinfo = new FileInfo(filedetails);
                            if (fileinfo.Exists)
                            {
                                fileinfo.Delete();
                            }
                            DataTable dt = result.Tables["PackingList"];
                            if(dt == null)
                            {
                                return Json("ERR:Invalid PackingList Excel Selected", JsonRequestBehavior.AllowGet);
                            }

                            if (this.ValidatePackingListExcel(dt,packingType))
                            {
                                if (packingType == "1")
                                {
                                    List<YarnPackingListModel> yarnPackingList = new List<YarnPackingListModel>();
                                    yarnPackingList = ConvertDataTable<YarnPackingListModel>(dt);
                                    TempData["PackingList"] = yarnPackingList;
                                    packingList = yarnPackingList;
                                }
                                else if(packingType == "2")
                                {
                                    List<FabricPackingListModel> fabricPackingList = new List<FabricPackingListModel>();
                                    fabricPackingList = ConvertDataTable<FabricPackingListModel>(dt);
                                    TempData["PackingList"] = fabricPackingList;
                                    packingList = fabricPackingList;
                                }
                                else
                                {
                                    List<FGPackingListModel> fgPackingList = new List<FGPackingListModel>();
                                    fgPackingList = ConvertDataTable<FGPackingListModel>(dt);
                                    TempData["PackingList"] = fgPackingList;
                                    packingList = fgPackingList;
                                }
                            }
                            else
                            {
                                string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                                string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                                string packingCaption = string.Empty;
                                if (packingType == "1")
                                    packingCaption = "Yarn";
                                else if (packingType == "2")
                                    packingCaption = "Fabric";
                                else
                                    packingCaption = "Finished Goods";

                                objERR.err_title = controllerName + "-" + controllerName;
                                objERR.err_message = "PackingList Schema/Columns does not match for Selected Packing type:" + packingCaption;
                                objERR.err_details = "PackingList File Selected is Invalid/Incorrect";
                                //int errid = bal.ExceptionInsertLogs_BL(objERR);
                                return Json("ERR:" + objERR.err_message, JsonRequestBehavior.AllowGet);
                            }
                        }
                        else
                        {
                            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                            objERR.err_title = controllerName + "-" + controllerName;
                            objERR.err_message = "Invalid PackingList File";
                            objERR.err_details = "PackingList File Selected is Invalid/Incorrect";
                            int errid = bal.ExceptionInsertLogs_BL(objERR);
                            return Json("ERR" + errid.ToString(), JsonRequestBehavior.AllowGet);
                        }
                    }
                }
                else
                {
                    return Json("Expired", JsonRequestBehavior.AllowGet);
                }
            }
            // var files = Request.Files;

            return new JsonResult { Data = packingList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        private bool ValidatePackingListExcel(DataTable dt, string packingType)
        {
            bool isValidPackingList = false;
            string yarnColumnSet = "S. No.|Pallet No.|No.of Cones|Lot No.|Nwt. in Kgs.|GWt. in Kgs.|Remarks" ;
            string fabricColumnSet = "S. No.|Pallet No.|Design No.|No.of pieces|Total Meters|Nwt. in Kgs.|GWt. in Kgs.|Individual piece lengths|Remarks";
            string fgColumnSet = "S. No.|No. of Pieces|Packing Units|Nwt. in Kgs.|GWt. in Kgs.|Remarks";
            string validatingColumnSet = string.Empty;
            for (var colIndex = 0; colIndex < dt.Columns.Count; colIndex++)
            {
                if(validatingColumnSet == string.Empty)
                    validatingColumnSet = string.Concat(validatingColumnSet, dt.Columns[colIndex].ColumnName);
                else
                    validatingColumnSet = string.Concat(validatingColumnSet,"|", dt.Columns[colIndex].ColumnName);
            }
            switch(packingType)
            {
                case "1":
                    if (String.Equals(yarnColumnSet.ToString(), validatingColumnSet, StringComparison.InvariantCultureIgnoreCase))
                        isValidPackingList = true;
                    break;
                case "2":
                    if (String.Equals(fabricColumnSet.ToString(), validatingColumnSet, StringComparison.InvariantCultureIgnoreCase))
                        isValidPackingList = true;
                    break;
                case "3":
                    if (String.Equals(fgColumnSet.ToString(), validatingColumnSet, StringComparison.InvariantCultureIgnoreCase))
                        isValidPackingList = true;
                    break;
            }
            return isValidPackingList;
        }

        private static List<T> ConvertDataTable<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            for (var i = 0; i < dt.Rows.Count; i++)
            {
                DataRow row = dt.Rows[i];
                if (!String.IsNullOrEmpty(row[1].ToString()))
                {
                    T item = GetItem<T>(row);
                    data.Add(item);
                }
            }
            //foreach (DataRow row in dt.Rows)
            //{
            //    T item = GetItem<T>(row);
            //    data.Add(item);
            //}
            return data;
        }

        private static T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    var displayNameAttribute = pro.GetCustomAttributes(typeof(DisplayNameAttribute), false);
                    var displayName = (displayNameAttribute[0] as DisplayNameAttribute).DisplayName;
                    if (displayName == column.ColumnName)
                    {
                        object columnValue = string.Empty;
                        if (!String.IsNullOrEmpty(dr[column.ColumnName].ToString()))
                        {
                            if (pro.Name == "Serial")
                            {
                                columnValue = Convert.ToInt32(dr[column.ColumnName]);
                            }
                            else
                            {
                                columnValue = Convert.ToString(dr[column.ColumnName]);
                            }
                            pro.SetValue(obj, columnValue, null);
                        }
                    }
                    else
                        continue;
                }
            }
            return obj;
        }
    }


}