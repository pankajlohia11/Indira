using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization; 
using System.Web.Routing;
using BusinessEntity.EntityModels;
using BusinessEntity.CustomModels;
using BusinessLogic.Admin_BL;
using BusinessLogic;
using System.IO;
namespace Euro.Controllers.Admin
{
    public class ET_Admin_CustomerModuleController : Controller
    {
        // GET: ET_Admin_CompanyModule
        ET_Admin_CustomerModule_BL objBL = new ET_Admin_CustomerModule_BL();
        error_master objERR = new error_master();
        BAL bal = new BAL();
        tbl_loginfo objLOG = new tbl_loginfo();
        EntityClasses dbcontext = new EntityClasses();
        public ActionResult ET_Admin_CustomerModule()
        {
            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                {
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
        //Get customer list
        public ActionResult GetCustomerList()

        {
            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                {
                    
                    CompanyModule__CM objMaster = new CompanyModule__CM();
                    objMaster.CustomerMasterList = objBL.GetCustomers_BL();
                    //var data = dbcontext.Tbl_Master_CompanyDetails.Where(m => m.Cust_Supp != 1);
                    var json = new JavaScriptSerializer().Serialize(objMaster.CustomerMasterList);
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
        //Get customer info
        public ActionResult GetCustomerInfo(int id)
        {
            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                {
                    dbcontext.Configuration.ProxyCreationEnabled = false;
                    
                    var data1 = (from a in dbcontext.Tbl_Master_CompanyDetails
                                 where a.COM_ID == id
                                 select new
                                 {
                                     a.COM_DISPLAYNAME,
                                     a.COM_NAME,
                                     a.COM_MOBILE,
                                     a.COM_EMAIL,
                                     a.COM_CODE,
                                     a.COM_USTID,
                                     a.Tax_No,
                                     a.COM_STREET,
                                     a.COM_ZIP,
                                    CustType =
                                    (
                                    a.Cust_Supp == 0 ? "Customer" :
                                    a.Cust_Supp == 1 ? "Supervisor" : "Both"
                                    ),
                                     a.CREATED_DATE,
                                     activeStatus = a.DELETED == false ? "Active" : "InActive",
                                     country =(dbcontext.locations.Where(m=>m.location_id == a.COM_COUNTRY).Select(m=>m.name)),
                                     city = a.COM_CITY,
                                     state = a.COM_STATE
                                 });
                    var json1 = new JavaScriptSerializer().Serialize(data1);
                    var data2 = dbcontext.Tbl_Master_CompanyContacts.Where(m => m.COM_ID == id);
                    var json2 = new JavaScriptSerializer().Serialize(data2);
                    int com_key = Convert.ToInt32(Session["Companykey"]);
                    var data3 = (from Ol in dbcontext.Tbl_Master_Order
                                join Mu in dbcontext.Tbl_Master_User on Ol.SO_SalesPersonID equals Mu.USER_ID
                                where Ol.DELETED == false && Ol.COM_KEY == com_key && Ol.SO_CutomerID == id
                                select new
                                {
                                    SO_ID = Ol.SO_ID,
                                    SO_CODE = Ol.SO_Code,
                                    USER_NAME = Mu.USER_NAME,
                                    Ol.SO_OrderDate,
                                    Ol.SO_CusPONO,
                                    Ol.SO_CusPODate,
                                    Type =
                                    (
                                    Ol.SO_OrderType == 1 ? "Agency" :
                                    Ol.SO_OrderType == 2 ? "Trading" : "Store"
                                    ),
                                    Status = Ol.SO_Approval == 1 ? "Approved" : "Unapproved"
                                }).ToList();
                    var json3 = new JavaScriptSerializer().Serialize(data3);
                    //changesNeeded
                    var data4 = (from a in dbcontext.Tbl_Shipment_Header
                                join c in dbcontext.Tbl_Master_CompanyDetails on a.S_CustSup equals c.COM_ID into ord
                                from y in ord
                                where a.DELETED == false && a.COM_KEY == com_key && a.S_CustSup == id && a.S_Type == 2
                                select new Shipment_CM
                                {
                                    S_ID = a.S_ID,
                                    S_Code = a.S_Code,
                                    S_ETD = a.S_ETD,
                                    S_ETA = a.S_ETA,
                                    COM_DISPLAYNAME = y.COM_DISPLAYNAME,
                                    S_DeparturePort = a.S_DeparturePort,
                                    S_ArrivalPort = a.S_ArrivalPort,
                                    S_Type = a.S_Type,
                                    S_STATUS = a.S_STATUS,
                                    S_DebitNoteStatus = a.S_DebitNoteStatus,
                                    S_CommissionRecievedStatus = a.S_CommissionRecievedStatus,
                                    S_DebitNoteApprovalStatus = a.S_DebitNoteApprovalStatus,
                                    S_INV_AMT = a.S_INV_AMT
                                }).ToList();
                    var json4 = new JavaScriptSerializer().Serialize(data4);
                    var data9 = (from a in dbcontext.Tbl_DespatchHeader
                                join c in dbcontext.Tbl_Master_CompanyDetails on a.D_CustomerID equals c.COM_ID into ord
                                from y in ord
                                where a.DELETED == false && a.COM_KEY == com_key && a.D_CustomerID == id 
                                select new 
                                {
                                    D_ID = a.D_ID,
                                    D_Code = a.D_Code,
                                    D_DespatchDate=a.D_DespatchDate,
                                    COM_DISPLAYNAME = y.COM_DISPLAYNAME,
                                    D_DeliveryFrom = a.D_DeliveryFrom,
                                    D_DeliveryTo = a.D_DeliveryTo,
                                   
                                }).ToList();
                    var json9 = new JavaScriptSerializer().Serialize(data9);
                    var data10 = (from a in dbcontext.Tbl_Shipment_Header
                                join c in dbcontext.Tbl_Master_CompanyDetails on a.S_CustSup equals c.COM_ID into ord
                                from y in ord
                                join d in dbcontext.Tbl_Master_CompanyDetails on a.S_SupplierID equals d.COM_ID into ord1
                                from y1 in ord1
                                where a.DELETED == false && a.COM_KEY == com_key && a.S_Type == 1 && a.S_CustSup==id
                                select new Shipment_CM
                                {
                                    S_ID = a.S_ID,
                                    S_Code = a.S_Code,
                                    S_ETD = a.S_ETD,
                                    S_ETA = a.S_ETA,
                                    COM_DISPLAYNAME = y.COM_DISPLAYNAME,
                                    SuppName = y1.COM_NAME,
                                    S_DeparturePort = a.S_DeparturePort,
                                    S_ArrivalPort = a.S_ArrivalPort,
                                    S_Type = a.S_Type,
                                    S_STATUS = a.S_STATUS,
                                    S_DebitNoteStatus = a.S_DebitNoteStatus,
                                    S_CommissionRecievedStatus = a.S_CommissionRecievedStatus,
                                    S_DebitNoteApprovalStatus = a.S_DebitNoteApprovalStatus,
                                    S_INV_AMT = a.S_INV_AMT
                                }).ToList();
                    var json10 = new JavaScriptSerializer().Serialize(data10);
                    var data5 = (from a in dbcontext.Tbl_Schedule
                                join b in dbcontext.Tbl_Product_Master on a.SH_ProductID equals b.P_ID into comp
                                from x in comp
                                join c in dbcontext.Tbl_Master_Order on a.SH_OrderID equals c.SO_ID into ord
                                from y in ord
                                where a.DELETED == false && a.COM_KEY == com_key && y.SO_CutomerID == id
                                select new Schedule_CM
                                {
                                    SH_ID = a.SH_ID,
                                    SH_Code = a.SH_Code,
                                    SH_DATE = a.SH_DATE,
                                    SO_Code = y.SO_Code,
                                    SH_Status = a.SH_Status,
                                    SH_Type = a.SH_Type,
                                    SalesPerson = (from d in dbcontext.Tbl_Master_User where d.USER_ID == a.SH_SalesPerson select d.USER_NAME).FirstOrDefault()
                                }
                                ).ToList().GroupBy(x => x.SH_Code).Select(x => x.FirstOrDefault()); ;
                    var json5 = new JavaScriptSerializer().Serialize(data5);
                    var data6 = (from a in dbcontext.Tbl_OneToOneInvoice
                                join b in dbcontext.Tbl_Shipment_Header on a.OTOI_ShipmentID equals b.S_ID into comp
                                from x in comp
                                join d in dbcontext.Tbl_Master_User on a.OTOI_SalesPerson equals d.USER_ID into user
                                from z in user
                                join w in dbcontext.Tbl_Master_CompanyDetails on a.OTOI_CustomerID equals w.COM_ID into p
                                from enq in p.DefaultIfEmpty()
                                where a.DELETED == false && a.COM_KEY == com_key && a.OTOI_CustomerID == id
                                select new
                                {
                                    OTOI_ID = a.OTOI_ID,
                                    OTOI_Code = a.OTOI_Code,
                                    OTOI_InvoiceDate = a.OTOI_InvoiceDate.ToString(),
                                    S_Code = x.S_Code,
                                    COM_DISPLAYNAME = enq.COM_DISPLAYNAME,
                                    USER_NAME = z.USER_NAME,
                                    salesPerson=a.OTOI_SalesPerson
                                   
                                }).ToList();
                    var json6 = new JavaScriptSerializer().Serialize(data6);
                    var data7 = (from a in dbcontext.Tbl_OneToManyInvoice
                                join d in dbcontext.Tbl_Master_User on a.OTMI_SalesPerson equals d.USER_ID into user
                                from z in user
                                join w in dbcontext.Tbl_Master_CompanyDetails on a.OTMI_CustomerID equals w.COM_ID into p
                                from enq in p.DefaultIfEmpty()
                                where a.DELETED == false && a.COM_KEY == com_key && a.OTMI_CustomerID == id
                                select new
                                {
                                    OTMI_ID = a.OTMI_ID,
                                    OTMI_Code = a.OTMI_Code,
                                    OTMI_InvoiceDate = a.OTMI_InvoiceDate.ToString(),
                                    COM_DISPLAYNAME = enq.COM_DISPLAYNAME,
                                    USER_NAME = z.USER_NAME
                                }).ToList();
                    var json7 = new JavaScriptSerializer().Serialize(data7);
                    var data8 = (from Cr in dbcontext.Tbl_CommissionRecieve
                                 join Sh in dbcontext.Tbl_Shipment_Header on Cr.CR_ShipmentID equals Sh.S_ID
                                 where Cr.DELETED == false && Cr.COM_KEY == com_key && Sh.S_CustSup == id && Sh.S_Type == 1 && Sh.S_CustSup == id
                                 select new
                                 {
                                     SH_CODE = Sh.S_Code,
                                     CR_Code = Cr.CR_Code,
                                     Cr.CR_ShipmentAmount,
                                     Cr.CR_FABAmount,
                                     Cr.CR_CommissionAmount,
                                     Cr.CR_CommissionRecieved
                                 }).ToList();
                    var json8 = new JavaScriptSerializer().Serialize(data8);

                    var result = new { Result = json1, Contact = json2, Order = json3, Shipment = json4, Schedule = json5, OneToOne = json6, OneToMany = json7, Payment = json8 , Despatch =json9,ShipmentAgency=json10};
                    return Json(result, JsonRequestBehavior.AllowGet);
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
        private string validations(string SUBJECT)
        {

            if (SUBJECT == "")
            {
                return "Subject";
            }
            return "";
        }
        //Company commnuication insert/update
        public JsonResult ET_Company_Comm_Add(string SUBJECT, string DATE_COMM, string CONVERSATION, bool TYPE,decimal PID)
        {
            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                {
                    string valid = validations(SUBJECT);
                    if (valid == "")
                    {
                        //FILE_NAME = FILE_NAME,
                        //Tbl_Company_Communication.FILE_NAME = FILE_NAME,
                        decimal data = 0;
                        string urlpath = "";
                        DateTime Comm_date;
                        if (DATE_COMM != "") { Comm_date = DateTime.ParseExact(DATE_COMM, "dd-MM-yyyy", null); } else { Comm_date = DateTime.Now; }

                        if (Request.Files.Count > 0)
                        {
                            HttpPostedFileBase file = Request.Files[0]; //Uploaded file
                                                                        //Use the following properties to get file's name, size and MIMEType

                            int fileSize = file.ContentLength;
                            string fileName = file.FileName;
                            string mimeType = file.ContentType;
                            System.IO.Stream fileContent = file.InputStream;

                            //To save file, use SaveAs method
                            string subPath = "~/conversationFile/";
                            bool exists = System.IO.Directory.Exists(Server.MapPath(subPath));
                            if (!exists)
                                System.IO.Directory.CreateDirectory(Server.MapPath(subPath));

                            string path1 = System.Web.Hosting.HostingEnvironment.MapPath(subPath) + "\\";
                            string path = System.Web.Hosting.HostingEnvironment.MapPath(subPath) + "\\" + fileName;
                            string url = subPath.Replace("~", "") + fileName;

                            FileInfo file1 = new FileInfo(path);
                            int k = 1;
                            while (file1.Exists)
                            {
                                path = path1 + "Copy" + k + fileName;
                                url = subPath.Replace("~", "") + "Copy" + k + fileName;
                                k++;
                                file1 = new FileInfo(path);
                            }
                            urlpath = url;
                            file.SaveAs(path); //File will be saved in application root
                        }

                            Tbl_Company_Communication Objmc = new Tbl_Company_Communication()
                            {
                                CC_PID = PID,
                                SUBJECT = SUBJECT,
                                DATE_COMM = Comm_date,
                                CONVERSATION = CONVERSATION,
                                TYPE = TYPE,
                                FILE_NAME = urlpath,
                                MS_UserId = Convert.ToInt32(Session["UserID"].ToString()),
                                CREATED_BY = Convert.ToInt32(Session["UserID"].ToString()),
                                CREATED_DATE = DateTime.Now,
                                DELETED = false
                            };
                            dbcontext.Tbl_Company_Communication.Add(Objmc);
                            dbcontext.SaveChanges();
                            data = Objmc.CC_ID;
                            
                        
                        
                        if (data != 0)
                        {
                            objLOG.log_dockey = "8019";
                            objLOG.log_userid = Session["UserID"].ToString();
                            objLOG.log_recordkey = data.ToString();
                            //if (CC_ID == 0)
                            //{
                                objLOG.log_operation = "Insert";
                                objLOG.log_Remarks = "Record Inserted Successfully";
                            //}
                            //else
                            //{
                            //    objLOG.log_operation = "Update";
                            //    objLOG.log_Remarks = "Record Updated Successfully";
                            //}
                            bal.OperationInsertLogs_BL(objLOG);
                        }
                        var json = new JavaScriptSerializer().Serialize(data);
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
                var json = "Session Expired:";
                return Json(json, JsonRequestBehavior.AllowGet);
            }
        }
        //Get communication list
        public ActionResult GetCompanyCommList(int id)
        {
            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                {
                    dbcontext.Configuration.ProxyCreationEnabled = false;

                    
                    int com_key = Convert.ToInt32(Session["Companykey"]);
                    var dataComm = (from Ol in dbcontext.Tbl_Company_Communication
                                    where Ol.DELETED == false && Ol.CC_PID == id
                                 select new
                                 {
                                     Ol.CC_ID,
                                     Ol.CC_PID,
                                     Ol.SUBJECT,
                                     Ol.DATE_COMM,
                                     Ol.FILE_NAME,
                                     Ol.CONVERSATION,
                                     Type = Ol.TYPE == false ? "Offline" : "Tele"
                                 }).ToList();
                    var jsonComm = new JavaScriptSerializer().Serialize(dataComm);
                    
//                    var result = new { Result = jsonComm };
                    return Json(jsonComm, JsonRequestBehavior.AllowGet);
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

        public JsonResult GetCustomerSalesForBarchart(int id,int startYear,int endYear)
        {
            int com_key = Convert.ToInt32(Session["Companykey"]);
            if (id != 0)
            {
                List<ShipmentReport_CM> shipmentdetails = new List<ShipmentReport_CM>();
                List<ShipmentReport_CM> despatchDetails = new List<ShipmentReport_CM>();
                shipmentdetails = (from shpheader in dbcontext.Tbl_Shipment_Header
                                   //join comp in dbcontext.Tbl_Master_CompanyDetails on shpheader.COM_KEY equals comp.COM_KEY
                                   join commission in dbcontext.Tbl_OneToOneInvoice on shpheader.S_ID equals commission.OTOI_ShipmentID
                                   where (commission.OTOI_InvoiceDate.Year == startYear || commission.OTOI_InvoiceDate.Year == endYear) && shpheader.S_CustSup == id && shpheader.COM_KEY == com_key
                                   select new ShipmentReport_CM
                                   {
                                       salespersonID = shpheader.S_SalesPerson,
                                       salespersonName = (dbcontext.Tbl_Master_User.Where(x => x.USER_ID == shpheader.S_SalesPerson).Select(x => x.USER_NAME).FirstOrDefault()),
                                       CommissionAmt = commission.OTOI_InvoiceAmount,
                                       CREATED_DATE = commission.OTOI_InvoiceDate,
                                       //monthname = commission.OTOI_InvoiceDate.Value.Month
                                   }).Distinct().ToList();
                despatchDetails = (from shpheader in dbcontext.Tbl_DespatchHeader
                                   //join comp in dbcontext.Tbl_Master_CompanyDetails on shpheader.COM_KEY equals comp.COM_KEY
                                   join commission in dbcontext.Tbl_OneToManyInvoice on shpheader.D_ID equals commission.OTMI_DespatchIDs
                                   where (commission.OTMI_InvoiceDate.Year == startYear || commission.OTMI_InvoiceDate.Year == endYear) && shpheader.D_CustomerID == id && shpheader.COM_KEY == com_key
                                   select new ShipmentReport_CM
                                   {
                                       salespersonID = shpheader.D_SalesPerson,
                                       salespersonName = (dbcontext.Tbl_Master_User.Where(x => x.USER_ID == shpheader.D_SalesPerson).Select(x => x.USER_NAME).FirstOrDefault()),
                                       CommissionAmt = commission.OTMI_InvoiceAmount,
                                       CREATED_DATE = commission.OTMI_InvoiceDate,
                                       //monthname = commission.OTOI_InvoiceDate.Value.Month
                                   }).ToList();

                shipmentdetails.AddRange(despatchDetails);

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
                                       //join comp in dbcontext.Tbl_Master_CompanyDetails on shpheader.COM_KEY equals comp.COM_KEY
                                   join commission in dbcontext.Tbl_OneToOneInvoice on shpheader.S_ID equals commission.OTOI_ShipmentID
                                   where (commission.OTOI_InvoiceDate.Year == (startYear) || commission.OTOI_InvoiceDate.Year == (endYear)) && shpheader.COM_KEY == com_key
                                   select new ShipmentReport_CM
                                   {
                                       salespersonID = shpheader.S_SalesPerson,
                                       salespersonName = (dbcontext.Tbl_Master_User.Where(x => x.USER_ID == shpheader.S_SalesPerson).Select(x => x.USER_NAME).FirstOrDefault()),
                                       CommissionAmt = commission.OTOI_InvoiceAmount,
                                       CREATED_DATE = commission.OTOI_InvoiceDate,
                                       //monthname = commission.OTOI_InvoiceDate.Value.Month
                                   }).Distinct().ToList();

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

        /// <summary>
        /// Get Product wise Sales for Bar Chart.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="startYear">Current year</param>
        /// <param name="endYear">Current year</param>
        /// <returns></returns>
        public JsonResult GetProductWiseSalesForBarchart(int id, int startYear, int endYear)
        {
            int com_key = Convert.ToInt32(Session["Companykey"]);
            if (id != 0)
            {
                List<ShipmentReport_CM> shipmentdetails = new List<ShipmentReport_CM>();
                List<ShipmentReport_CM> despatchDetails = new List<ShipmentReport_CM>();
                var shipmentData = (from shpheader in dbcontext.Tbl_Shipment_Header
                                   join shipmentDet in dbcontext.Tbl_Shipment_Details on shpheader.S_ID equals shipmentDet.SD_PID
                                   //join comp in dbcontext.Tbl_Master_CompanyDetails on shpheader.COM_KEY equals comp.COM_KEY
                                   join commission in dbcontext.Tbl_OneToOneInvoice on shpheader.S_ID equals commission.OTOI_ShipmentID
                                   where (commission.OTOI_InvoiceDate.Year == startYear || commission.OTOI_InvoiceDate.Year == endYear) && shpheader.S_CustSup == id && shpheader.COM_KEY == com_key
                                   select new ShipmentReport_CM
                                   {
                                       salespersonID = shpheader.S_SalesPerson,
                                       salespersonName = (dbcontext.Tbl_Master_User.Where(x => x.USER_ID == shpheader.S_SalesPerson).Select(x => x.USER_NAME).FirstOrDefault()),
                                       CommissionAmt = commission.OTOI_InvoiceAmount,
                                       CREATED_DATE = commission.OTOI_InvoiceDate,
                                       ShipmenPGName = (from c in dbcontext.Tbl_Product_Master
                                                        join pg in dbcontext.Tbl_ProductGroup on c.P_CategoryID equals pg.PG_PARENT_ID
                                                        where shipmentDet.SD_ProductID == c.P_ID
                                                        select pg.PG_NAME
                                                       ).FirstOrDefault(),
                                       ShipmenPGParent = (from c in dbcontext.Tbl_Product_Master
                                                          join pg in dbcontext.Tbl_ProductGroup on c.P_CategoryID equals pg.PG_PARENT_ID
                                                          where shipmentDet.SD_ProductID == c.P_ID
                                                          select pg.PG_PARENT_ID.Value
                                                       ).FirstOrDefault(),
                                       ShipmenPGParentType = (from c in dbcontext.Tbl_Product_Master
                                                              join pg in dbcontext.Tbl_ProductGroup on c.P_CategoryID equals pg.PG_PARENT_ID
                                                              where shipmentDet.SD_ProductID == c.P_ID
                                                              select pg.PG_TYPE
                                                       ).FirstOrDefault()
                                       //monthname = commission.OTOI_InvoiceDate.Value.Month
                                   }).Distinct();
                var dataCount = shipmentData.Count();

                shipmentdetails = shipmentData.AsEnumerable().Select(x => new ShipmentReport_CM { salespersonID = x.salespersonID, salespersonName = x.salespersonName, CommissionAmt = x.CommissionAmt, CREATED_DATE = x.CREATED_DATE, ShipmentProductType = GetPGName(x.ShipmenPGParent, x.ShipmenPGParentType, x.ShipmenPGName) }).ToList();

                var despatchData = (from shpheader in dbcontext.Tbl_DespatchHeader
                                    join shipmentDet in dbcontext.Tbl_DespatchDetails on shpheader.D_ID equals shipmentDet.DD_ID
                                    //join comp in dbcontext.Tbl_Master_CompanyDetails on shpheader.COM_KEY equals comp.COM_KEY
                                    join commission in dbcontext.Tbl_OneToManyInvoice on shpheader.D_ID equals commission.OTMI_DespatchIDs
                                    where (commission.OTMI_InvoiceDate.Year == startYear || commission.OTMI_InvoiceDate.Year == endYear) && shpheader.D_CustomerID == id && shpheader.COM_KEY == com_key
                                    select new ShipmentReport_CM
                                    {
                                        salespersonID = shpheader.D_SalesPerson,
                                        salespersonName = (dbcontext.Tbl_Master_User.Where(x => x.USER_ID == shpheader.D_SalesPerson).Select(x => x.USER_NAME).FirstOrDefault()),
                                        CommissionAmt = commission.OTMI_InvoiceAmount,
                                        CREATED_DATE = commission.OTMI_InvoiceDate,
                                        ShipmenPGName = (from c in dbcontext.Tbl_Product_Master
                                                         join pg in dbcontext.Tbl_ProductGroup on c.P_CategoryID equals pg.PG_PARENT_ID
                                                         where shipmentDet.DD_ProductID == c.P_ID
                                                         select pg.PG_NAME
                                                        ).FirstOrDefault(),
                                        ShipmenPGParent = (from c in dbcontext.Tbl_Product_Master
                                                           join pg in dbcontext.Tbl_ProductGroup on c.P_CategoryID equals pg.PG_PARENT_ID
                                                           where shipmentDet.DD_ProductID == c.P_ID
                                                           select pg.PG_PARENT_ID.Value
                                                        ).FirstOrDefault(),
                                        ShipmenPGParentType = (from c in dbcontext.Tbl_Product_Master
                                                               join pg in dbcontext.Tbl_ProductGroup on c.P_CategoryID equals pg.PG_PARENT_ID
                                                               where shipmentDet.DD_ProductID == c.P_ID
                                                               select pg.PG_TYPE
                                                        ).FirstOrDefault()
                                        //monthname = commission.OTOI_InvoiceDate.Value.Month
                                    }).Distinct();
                despatchDetails = despatchData.AsEnumerable().Select(x => new ShipmentReport_CM { salespersonID = x.salespersonID, salespersonName = x.salespersonName, CommissionAmt = x.CommissionAmt, CREATED_DATE = x.CREATED_DATE, ShipmentProductType = GetPGName(x.ShipmenPGParent, x.ShipmenPGParentType, x.ShipmenPGName) }).ToList();

                shipmentdetails.AddRange(despatchDetails);
                List<ShipmentReport_CM> salespersonWise = shipmentdetails.GroupBy(_ => new { _.ShipmentProductType})
                                                            .Select(g => new ShipmentReport_CM
                                                            {
                                                                //salespersonID = g.Key.salespersonID,
                                                                CREATED_DATE = g.FirstOrDefault().CREATED_DATE,
                                                                salespersonName = g.FirstOrDefault().salespersonName,
                                                                ShipmentProductType = g.FirstOrDefault().ShipmentProductType,
                                                                CommissionAmt = g.Where(_ => _.CommissionAmt != 0).Sum(_ => _.CommissionAmt),
                                                            }).ToList();
                var f = 0;
                string s = "";
                if (salespersonWise.Count() <= 0)
                {

                    s = s + "['0','0','0']";
                }
                else
                {
                    for (int i = 0; i < salespersonWise.Count(); i++)
                    {
                        if (f == 0)
                        {
                            s = s + "['" + salespersonWise[i].CommissionAmt + "'";
                        }
                        else
                        {
                            s = s + ",'" + salespersonWise[i].CommissionAmt + "'";
                        }
                        f++;
                    }

                    s = s + "]";
                }
                string s5 = s.Replace("\"", "~");
                s5 = s5.Replace("'", "\"");
                s5 = s5.Replace("~", "'");
                return Json(s5, JsonRequestBehavior.AllowGet);
            }
            else
            {
                string s = "";
                s = s + "['0','0','0']";
                string s5 = s.Replace("\"", "~");
                s5 = s5.Replace("'", "\"");
                s5 = s5.Replace("~", "'");
                return Json(s5, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetPendingShipmentDetails(int customer)
        {
            List<PendingShipmentReport_CM> PendingShipmentDetails = new List<PendingShipmentReport_CM>();
            string s = "";
            string s1 = "";
            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                {
                    var ShipmentSchdule = dbcontext.Tbl_Shipment_Header.Where(m => m.DELETED == false).ToList();
                    for (int i = 0; i < ShipmentSchdule.Count; i++)
                    {
                        if (i == 0)
                        {
                            s = ShipmentSchdule[i].S_ScheduleID.ToString();
                        }
                        else
                        {
                            s = s + "," + ShipmentSchdule[i].S_ScheduleID.ToString();
                        }

                    }
                    var UID = new HashSet<string>(s.Split(',').ToList());
                    var schduleCode = dbcontext.Tbl_Schedule.Where(m => m.DELETED == false && !UID.Contains(m.SH_Code)).Select(m => m.SH_Code).Distinct().ToList();
                   PendingShipmentDetails = (from shipment in dbcontext.Tbl_Shipment_Header
                                                 //from order1 in dbcontext.Tbl_Master_Order
                                                 //join OrderDetails in dbcontext.Tbl_Order_Details on order1.SO_ID equals OrderDetails.AGEN_TRAD_PO_ID
                                                 // join order in dbcontext.Tbl_Schedule on order1.SO_ID equals order.SH_OrderID
                                                 //join SD in dbcontext.Tbl_Shipment_Details on order.SH_ID equals SD.SD_ScheduleID
                                                 //join shipment in dbcontext.Tbl_Shipment_Header on SD.SD_PID equals shipment.S_ID
                                                 // where schduleCode.Contains(order.SH_Code)
                                             where (customer != 0 && shipment.S_CustSup == customer) && shipment.S_STATUS == 0
                                              select new PendingShipmentReport_CM
                                              {
                                                  ShipmentCode = shipment.S_Code,
                                                  //SchduleID = order1.SO_ID,
                                                  //SchduleCode = order.SH_Code,
                                                  //Sc_No = order1.SO_CusPONO,
                                                  //Sc_Date = order1.SO_CusPODate ?? DateTime.Now,
                                                  //ETD = order.SH_DATE,
                                                  ETA=shipment.S_ETA.Value,
                                                  //salespersonID = order1.SO_SalesPersonID,
                                                  salespersonName = (dbcontext.Tbl_Master_User.Where(x => x.USER_ID == shipment.S_SalesPerson).Select(x => x.USER_NAME).FirstOrDefault()),
                                                  //customerID = order1.SO_CutomerID,
                                                  Status=shipment.S_STATUS,
                                                  //supplierID = order1.SO_CutomerID,
                                                  customerName = (dbcontext.Tbl_Master_CompanyDetails.Where(x => x.COM_ID == shipment.S_CustSup).Select(x => x.COM_NAME).FirstOrDefault()),
                                                 //supplierName = (dbcontext.Tbl_Master_CompanyDetails.Where(x => x.COM_ID == order1.SO_SupplierID).Select(x => x.COM_NAME).FirstOrDefault()),
                                                  Shipment_Type = shipment.S_Type,
                                                  Shipment_TypeString = ( shipment.S_Type == 1 ? "Agency" :
                                                                          shipment.S_Type == 2 ? "Trading" : "Store"),
                                                  // ProductId = OrderDetails.PRODUCT_ID,
                                                  //ProductName = (dbcontext.Tbl_Product_Master.Where(x => x.P_ID == OrderDetails.PRODUCT_ID).Select(x => x.P_ShortName).FirstOrDefault()),
                                                  //QUANTITY = OrderDetails.QUANTITY,
                                              }).Distinct().ToList();
                    var PendingDespatchDetails = (from despatch in dbcontext.Tbl_DespatchHeader
                                              where ((customer != 0 && despatch.D_CustomerID == customer)) && despatch.D_Status == 0
                                              select new PendingShipmentReport_CM
                                              {
                                                  ShipmentCode = despatch.D_Code,
                                                  //SchduleID = order1.SO_ID,
                                                  //SchduleCode = order.SH_Code,
                                                  //Sc_No = order1.SO_CusPONO,
                                                  //Sc_Date = order1.SO_CusPODate ?? DateTime.Now,
                                                  Sc_Date = despatch.D_DespatchDate,
                                                  //ETD = order.SH_DATE,
                                                  ETA = despatch.D_DespatchDate,
                                                  //salespersonID = order1.SO_SalesPersonID,
                                                  salespersonName = (dbcontext.Tbl_Master_User.Where(x => x.USER_ID == despatch.D_SalesPerson).Select(x => x.USER_NAME).FirstOrDefault()),
                                                  //customerID = order1.SO_CutomerID,
                                                  //Status = despatch.D_Status.Value,
                                                  //supplierID = order1.SO_CutomerID,
                                                  customerName = (dbcontext.Tbl_Master_CompanyDetails.Where(x => x.COM_ID == despatch.D_CustomerID).Select(x => x.COM_NAME).FirstOrDefault()),
                                                  //supplierName = (dbcontext.Tbl_Master_CompanyDetails.Where(x => x.COM_ID == despatch.su).Select(x => x.COM_NAME).FirstOrDefault()),
                                                  Shipment_Type = 3,
                                                  Shipment_TypeString = "Store",
                                                  // ProductId = OrderDetails.PRODUCT_ID,
                                                  //ProductName = (dbcontext.Tbl_Product_Master.Where(x => x.P_ID == OrderDetails.PRODUCT_ID).Select(x => x.P_ShortName).FirstOrDefault()),
                                                  //QUANTITY = OrderDetails.QUANTITY,
                                              }).Distinct().ToList();

                    PendingShipmentDetails.AddRange(PendingDespatchDetails);
                    var modelItems = PendingShipmentDetails.Select((pendingItem, index) => new{ SO_Serial = index + 1, ShipmentCode = pendingItem.ShipmentCode, Shipment_Type = pendingItem.Shipment_Type, ETA = pendingItem.ETA , salespersonName = pendingItem.salespersonName, Shipment_TypeString  = pendingItem.Shipment_TypeString }).ToList();
                    var pendingJson = Newtonsoft.Json.JsonConvert.SerializeObject(modelItems);
                    return Json(pendingJson, JsonRequestBehavior.AllowGet);
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
        /// Get PG Name.
        /// </summary>
        /// <param name="parentId"></param>
        /// <param name="pgType"></param>
        /// <param name="pgName"></param>
        /// <returns></returns>
        private string GetPGName(decimal parentId,decimal pgType, string pgName)
        {
            Tbl_ProductGroup pgRoot;
           
            //if (parentId == 0 && pgType == 1)
            //{
            //    pgRoot = dbcontext.Tbl_ProductGroup.Where(a => a.PG_ID == 1).FirstOrDefault();
            //    return pgRoot.PG_NAME;
            //}
            //else if(parentId != 0 && pgType == 1)
            //{
            //    pgRoot = dbcontext.Tbl_ProductGroup.Where(a => a.PG_ID == parentId).FirstOrDefault();
            //    return pgRoot.PG_NAME;
            //}
            pgRoot = dbcontext.Tbl_ProductGroup.Where(a => a.PG_ID == parentId).FirstOrDefault();
            var rootId = pgRoot.PG_PARENT_ID;
            while (rootId > 0)
            {
                pgRoot = dbcontext.Tbl_ProductGroup.Where(a => a.PG_ID == pgRoot.PG_PARENT_ID).FirstOrDefault();
                rootId = pgRoot.PG_PARENT_ID;
            }
            return pgRoot.PG_NAME;
        }
        public JsonResult GetPendingInvoicelist(int customer)
        {
            List<PendingInvoiceReport_CM> PendingOTOInvoiceDetails = new List<PendingInvoiceReport_CM>();
            List<PendingInvoiceReport_CM> PendingOTMInvoiceDetails = new List<PendingInvoiceReport_CM>();
            string s = "";
            string s1 = "";
            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                {
                    var ShipmentSchdule = dbcontext.Tbl_Shipment_Header.Where(m => m.DELETED == false).ToList();
                    for (int i = 0; i < ShipmentSchdule.Count; i++)
                    {
                        if (i == 0)
                        {
                            s = ShipmentSchdule[i].S_ScheduleID.ToString();
                        }
                        else
                        {
                            s = s + "," + ShipmentSchdule[i].S_ScheduleID.ToString();
                        }

                    }
                    var UID = new HashSet<string>(s.Split(',').ToList());
                    var schduleCode = dbcontext.Tbl_Schedule.Where(m => m.DELETED == false && !UID.Contains(m.SH_Code)).Select(m => m.SH_Code).Distinct().ToList();
                   PendingOTOInvoiceDetails = (
                                              //from order1 in dbcontext.Tbl_Master_Order
                                              //join OrderDetails in dbcontext.Tbl_Order_Details on order1.SO_ID equals OrderDetails.AGEN_TRAD_PO_ID
                                              //join order in dbcontext.Tbl_Schedule on order1.SO_ID equals order.SH_OrderID
                                              from invoice in dbcontext.Tbl_OneToOneInvoice
                                              join shipment in dbcontext.Tbl_Shipment_Header on invoice.OTOI_ShipmentID equals shipment.S_ID
                                              where invoice.OTOI_Approval == 0 && invoice.OTOI_CustomerID == customer
                                              select new PendingInvoiceReport_CM
                                              {
                                                  InvoiceCode = invoice.OTOI_Code,
                                                  ShipmentCode = shipment.S_Code,
                                                  //SchduleID = order1.SO_ID,
                                                  //SchduleCode = order.SH_Code,
                                                  //Sc_No = order1.SO_CusPONO,
                                                  //Sc_Date = order1.SO_CusPODate ?? DateTime.Now,
                                                  ETA = invoice.OTOI_InvoiceDate,
                                                  //salespersonID = order1.SO_SalesPersonID,
                                                  salespersonName = (dbcontext.Tbl_Master_User.Where(x => x.USER_ID == shipment.S_SalesPerson).Select(x => x.USER_NAME).FirstOrDefault()),
                                                  //customerID = order1.SO_CutomerID,
                                                  Status = shipment.S_STATUS,
                                                  //supplierID = order1.SO_CutomerID,
                                                  //customerName = (dbcontext.Tbl_Master_CompanyDetails.Where(x => x.COM_ID == order1.SO_CutomerID).Select(x => x.COM_NAME).FirstOrDefault()),
                                                  //supplierName = (dbcontext.Tbl_Master_CompanyDetails.Where(x => x.COM_ID == order1.SO_SupplierID).Select(x => x.COM_NAME).FirstOrDefault()),
                                                  // ProductId = OrderDetails.PRODUCT_ID,
                                                  //ProductName = (dbcontext.Tbl_Product_Master.Where(x => x.P_ID == OrderDetails.PRODUCT_ID).Select(x => x.P_ShortName).FirstOrDefault()),
                                                  //QUANTITY = OrderDetails.QUANTITY,
                                              }).Distinct().ToList();
                    PendingOTMInvoiceDetails = (
                                            //from order1 in dbcontext.Tbl_Master_Order
                                            //join OrderDetails in dbcontext.Tbl_Order_Details on order1.SO_ID equals OrderDetails.AGEN_TRAD_PO_ID
                                            //join order in dbcontext.Tbl_Schedule on order1.SO_ID equals order.SH_OrderID
                                            from invoice in dbcontext.Tbl_OneToManyInvoice 
                                            join despatch in dbcontext.Tbl_DespatchHeader on invoice.OTMI_DespatchIDs equals despatch.D_ID
                                            where invoice.OTMI_Approval == 0 && invoice.OTMI_CustomerID == customer
                                             select new PendingInvoiceReport_CM
                                             {
                                                 InvoiceCode = invoice.OTMI_Code,
                                                 ShipmentCode = despatch.D_Code,
                                                 //SchduleID = order1.SO_ID,
                                                 //SchduleCode = order.SH_Code,
                                                 //Sc_No = order1.SO_CusPONO,
                                                 //Sc_Date = order1.SO_CusPODate ?? DateTime.Now,
                                                 ETA = invoice.OTMI_InvoiceDate,
                                                 //salespersonID = order1.SO_SalesPersonID,
                                                 salespersonName = (dbcontext.Tbl_Master_User.Where(x => x.USER_ID == despatch.D_SalesPerson).Select(x => x.USER_NAME).FirstOrDefault()),
                                                 //customerID = order1.SO_CutomerID,
                                                 Status = despatch.D_Status.Value,
                                                 //supplierID = order1.SO_CutomerID,
                                                 //customerName = (dbcontext.Tbl_Master_CompanyDetails.Where(x => x.COM_ID == order1.SO_CutomerID).Select(x => x.COM_NAME).FirstOrDefault()),
                                                 //supplierName = (dbcontext.Tbl_Master_CompanyDetails.Where(x => x.COM_ID == order1.SO_SupplierID).Select(x => x.COM_NAME).FirstOrDefault()),
                                                 // ProductId = OrderDetails.PRODUCT_ID,
                                                 //ProductName = (dbcontext.Tbl_Product_Master.Where(x => x.P_ID == OrderDetails.PRODUCT_ID).Select(x => x.P_ShortName).FirstOrDefault()),
                                                 //QUANTITY = OrderDetails.QUANTITY,
                                             }).Distinct().ToList();

                    var pendingInvoiceComplete = PendingOTOInvoiceDetails.Concat(PendingOTMInvoiceDetails);

                    var modelItems = pendingInvoiceComplete.Select((pendingItem, index) => new { SO_Serial = index + 1, InvoiceCode = pendingItem.InvoiceCode, ShipmentCode = pendingItem.ShipmentCode, ETA = pendingItem.ETA, salespersonName = pendingItem.salespersonName }).ToList();
                    var pendingJson = Newtonsoft.Json.JsonConvert.SerializeObject(modelItems);
                    return Json(pendingJson, JsonRequestBehavior.AllowGet);
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