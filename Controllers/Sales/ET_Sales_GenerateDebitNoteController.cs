using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using BusinessEntity.EntityModels;
using BusinessLogic;
using BusinessLogic.Admin_BL;
using System.Web.Script.Serialization;
using BusinessEntity.CustomModels;

namespace Euro.Controllers.Sales
{
    public class ET_Sales_GenerateDebitNoteController : Controller
    {
        public static string prefix = "";
        public static bool automanual = false;
        BAL bal = new BAL();
        error_master objERR = new error_master();
        tbl_loginfo objLOG = new tbl_loginfo();
        EntityClasses dbcontext = new EntityClasses();
        // GET: ET_Sales_GenerateDebitNote
        public ActionResult ET_Sales_GenerateDebitNote(string type)
        {
            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                if (type != null)
                {
                    if (type == "Agency" || type == "Trading")
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
        private void AutoManual()
        {
            ViewBag.AutoManual = false;
            automanual = false;
            List<Tbl_Document_Master> ObjDoc = bal.AutoManual_BL(3009);
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

        public JsonResult GetPrivilages()
        {

            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                {
                    var privilagelist = bal.GetPrivilages_BL(Convert.ToInt32(Session["UserID"].ToString()), 3007);
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
        public JsonResult GetDebitNoteList(bool delete, int type)
        {
            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                {
                    int com_key = Convert.ToInt32(Session["Companykey"].ToString());

                    dbcontext.Configuration.ProxyCreationEnabled = false;
                    var data = (from a in dbcontext.Tbl_Shipment_Header
                                join c in dbcontext.Tbl_GenerateDebitNote on a.S_ID equals c.DN_ShipmentID into ord
                                from y in ord
                                join d1 in dbcontext.Tbl_Master_CompanyDetails on a.S_SupplierID equals d1.COM_ID into ord2
                                from y2 in ord2
                                join d2 in dbcontext.Tbl_Master_CompanyDetails on a.S_CustSup equals d2.COM_ID into ord3
                                from y3 in ord3
                                where a.DELETED == delete && a.COM_KEY == com_key && a.S_Type == type
                                select new Shipment_CM
                                {
                                    DN_ID=y.DN_ID,
                                    DN_Code=y.DN_Code,
                                    DN_FOBStatus=y.DN__FOBStatus,
                                    S_ID = a.S_ID,
                                    S_Code = a.S_Code,
                                    S_ETD = a.S_ETD,
                                    S_ETA = a.S_ETA,
                                    COM_DISPLAYNAME = y3.COM_DISPLAYNAME,
                                    SuppName = y2.COM_NAME,
                                    S_DeparturePort = a.S_DeparturePort,
                                    S_ArrivalPort = a.S_ArrivalPort,
                                    S_Type = a.S_Type,
                                    S_STATUS1 = (dbcontext.Tbl_CommissionRecieve.Where(m => m.CR_ShipmentID == a.S_ID).Count() > 0 ? "yes" : "No"),
                                    S_DebitNoteStatus = a.S_DebitNoteStatus,
                                    S_CommissionRecievedStatus = a.S_CommissionRecievedStatus,
                                    S_DebitNoteApprovalStatus = a.S_DebitNoteApprovalStatus,
                                    S_INV_AMT = a.S_INV_AMT,
                                    S_INV_NO = a.S_INV_NO,
                                    S_ContainerNo = a.S_ContainerNo,
                                    S_INV_DATE = a.S_INV_DATE,
                                    S_BL_NO = a.S_BL_NO,
                                    S_BL_DATE = a.S_BL_DATE,
                                    S_CommissionAmount = (dbcontext.Tbl_CommissionRecieve.Where(m => m.CR_ShipmentID == a.S_ID).FirstOrDefault().CR_CommissionAmount),
                                }).Distinct().OrderByDescending(m => m.S_ID).ToList();
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
       
        public JsonResult GetShipments(int id, string type, int invid)
        {

            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                {
                    if (type == "Submit")
                    {
                        int comkey = Convert.ToInt32(Session["CompanyKey"].ToString());
                        dbcontext.Configuration.ProxyCreationEnabled = false;
                        var data1 = dbcontext.Tbl_GenerateDebitNote.Select(m => m.DN_ShipmentID);
                        var data = dbcontext.Tbl_Shipment_Header.Where(m => m.COM_KEY == comkey && m.DELETED == false && m.S_Type == 1 && m.S_CustSup == id && !data1.Contains(m.S_ID)).ToList();
                        var json = new JavaScriptSerializer().Serialize(data);
                        return Json(json, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        var data1 = dbcontext.Tbl_GenerateDebitNote.Where(m => m.DN_ID == invid).Select(m => m.DN_ShipmentID);
                        dbcontext.Configuration.ProxyCreationEnabled = false;
                        var data = dbcontext.Tbl_Shipment_Header.Where(m => data1.Contains(m.S_ID)).ToList();
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
        public ActionResult GetDebitNoteDetails(int id)
        {
            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                {
                    dbcontext.Configuration.ProxyCreationEnabled = false;
                    var data = (from a in dbcontext.Tbl_Shipment_Details
                                where a.SD_PID == id
                                select new
                                {
                                    popupOrderNo = (from b in dbcontext.Tbl_Master_Order
                                                    join c in dbcontext.Tbl_Order_Details on b.SO_ID equals c.AGEN_TRAD_PO_ID
                                                    where c.ORDER_ID == a.SD_OrderDetailID
                                                    select
                                                    b.SO_Code).FirstOrDefault(),
                                    OrderAmt = (from b in dbcontext.Tbl_Master_Order
                                                join c in dbcontext.Tbl_Order_Details on b.SO_ID equals c.AGEN_TRAD_PO_ID
                                                where c.ORDER_ID == a.SD_OrderDetailID
                                                select (c.PRICE * a.SD_Quantity)).FirstOrDefault(),
                                });
                    var data1 = data.GroupBy(m => m.popupOrderNo).Select(a => new { ordNo = a.Select(m => m.popupOrderNo).FirstOrDefault(), ordAmt = a.Sum(m => m.OrderAmt) }).ToList();

                    var data2 = (from a in dbcontext.Tbl_Shipment_Header
                                        join b in dbcontext.Tbl_Master_CompanyDetails on a.S_SupplierID equals b.COM_ID
                                        join c in dbcontext.Tbl_Master_User on a.S_SalesPerson equals c.USER_ID
                                where a.S_ID == id
                                select new
                                {
                                    a.S_ID,
                                    b.COM_NAME,
                                    a.S_INV_AMT,
                                    a.S_INV_DATE,
                                    a.S_INV_NO,
                                    a.S_SalesPerson,
                                    c.DISPLAY_NAME,

                                }).ToList();
                    var result = new { data1, data2 };
                    var json = new JavaScriptSerializer().Serialize(result);
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
        public ActionResult GetDebitNoteDetailsForEdit(int id,int Status)
        {
            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                {
                    string popupFabAmt = (from b in dbcontext.Tbl_GenerateDebitNote where b.DN_ShipmentID == id select b.DN_FOBAmount).FirstOrDefault();
                    dbcontext.Configuration.ProxyCreationEnabled = false;
                    List<decimal> FabAmounts = new List<decimal>();
                    if (Status==1)
                    {
                        
                        if (popupFabAmt.Substring(popupFabAmt.Length - 1) == ",")
                        {
                            popupFabAmt = popupFabAmt.Substring(0, popupFabAmt.Length - 1);
                        }
                         FabAmounts = new List<decimal>(popupFabAmt.Split(',').Select(m => Convert.ToDecimal(m)).ToList()).ToList();
                    }
                    
                    var data = (from a in dbcontext.Tbl_Shipment_Details
                                where a.SD_PID == id
                                select new
                                {
                                    popupOrderNo = (from b in dbcontext.Tbl_Master_Order
                                                    join c in dbcontext.Tbl_Order_Details on b.SO_ID equals c.AGEN_TRAD_PO_ID
                                                    where c.ORDER_ID == a.SD_OrderDetailID
                                                    select
                                                    b.SO_Code).FirstOrDefault(),
                                    OrderAmt = (from b in dbcontext.Tbl_Master_Order
                                                join c in dbcontext.Tbl_Order_Details on b.SO_ID equals c.AGEN_TRAD_PO_ID
                                                where c.ORDER_ID == a.SD_OrderDetailID
                                                select (c.PRICE * a.SD_Quantity)).FirstOrDefault(),
                                });
                    var data1 = data.GroupBy(m => m.popupOrderNo).Select(a => new { ordNo = a.Select(m => m.popupOrderNo).FirstOrDefault(), ordAmt = a.Sum(m => m.OrderAmt) }).ToList();

                    var data2 = (from a in dbcontext.Tbl_Shipment_Header
                                 join d in dbcontext.Tbl_GenerateDebitNote on a.S_ID equals d.DN_ShipmentID
                                 join b in dbcontext.Tbl_Master_CompanyDetails on a.S_SupplierID equals b.COM_ID
                                 join c in dbcontext.Tbl_Master_User on a.S_SalesPerson equals c.USER_ID
                                 where a.S_ID == id
                                 select new
                                 {
                                     a.S_ID,
                                     b.COM_NAME,
                                     a.S_CustSup,
                                     d.DN_ID,
                                     d.DN_Code,
                                     a.S_Type,
                                     a.S_INV_AMT,
                                     a.S_INV_DATE,
                                     a.S_INV_NO,
                                     a.S_SalesPerson,
                                     c.DISPLAY_NAME,
                                     d.DN__FOBStatus,

                                 }).ToList();
                    var result = new { data1, data2, FabAmounts };
                    var json = new JavaScriptSerializer().Serialize(result);
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
        public ActionResult ET_Sales_Shipment_GenerateDebitNote(int id, string fabamount, decimal invamt, int fabstatus)
        {
            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                {
                    dbcontext.Configuration.ProxyCreationEnabled = false;
                    string code = "";
                    string[] FOBORD = new string[2];
                    if (fabstatus == 1)
                    {
                        FOBORD = fabamount.Split('|');
                    }
                    else
                    {
                        FOBORD[0] = "";
                        FOBORD[1] = "";
                    }
                    var DebitDetails = dbcontext.Tbl_GenerateDebitNote.Where(a => a.DN_ShipmentID == id).ToList();
                    if (DebitDetails.Count() == 0)
                    {
                        Tbl_GenerateDebitNote Objmc = new Tbl_GenerateDebitNote()
                        {
                            DN_ShipmentID = id,
                            DN_ShipmentAmount = invamt,
                            DN_FOBAmount = FOBORD[0],
                            DN_OrderCodes = FOBORD[1],
                            DN__FOBStatus = fabstatus,
                            DN_Status = 0,
                            CREATED_BY = Convert.ToInt32(Session["UserID"].ToString()),
                            CREATED_DATE = DateTime.Now,
                            DELETED = false,
                            COM_KEY = Convert.ToInt32(Session["CompanyKey"].ToString()),
                        };
                        dbcontext.Tbl_GenerateDebitNote.Add(Objmc);
                        dbcontext.SaveChanges();
                        int len = 10 - ("DBT" + Objmc.DN_ID).Length;
                        code = "DBT" + new String('0', len) + Objmc.DN_ID;
                        Tbl_GenerateDebitNote _Tbl_GenerateDebitNote = dbcontext.Tbl_GenerateDebitNote.Single(m => m.DN_ID == Objmc.DN_ID);
                        {
                            _Tbl_GenerateDebitNote.DN_Code = code;
                        };
                        dbcontext.SaveChanges();
                    }
                    else
                    {
                        code = DebitDetails[0].DN_Code;
                        decimal did = DebitDetails[0].DN_ID;
                        Tbl_GenerateDebitNote _Tbl_GenerateDebitNote = dbcontext.Tbl_GenerateDebitNote.Single(m => m.DN_ID == did);
                        {
                            _Tbl_GenerateDebitNote.DN_FOBAmount = FOBORD[0];
                            _Tbl_GenerateDebitNote.DN_OrderCodes = FOBORD[1];
                            _Tbl_GenerateDebitNote.DN_Status = 0;
                            _Tbl_GenerateDebitNote.DN__FOBStatus = fabstatus;
                            _Tbl_GenerateDebitNote.LAST_UPDATED_BY = Convert.ToInt32(Session["UserID"].ToString());
                            _Tbl_GenerateDebitNote.LAST_UPDATED_DATE = DateTime.Now;
                        };
                        dbcontext.SaveChanges();
                    }
                    dbcontext.Tbl_Shipment_Header.Single(m => m.S_ID == id).S_STATUS = 1;
                    dbcontext.Tbl_Shipment_Header.Single(m => m.S_ID == id).S_DebitNoteStatus = true;
                    dbcontext.Tbl_Shipment_Header.Single(m => m.S_ID == id).S_DebitNoteApprovalStatus = 1;
                    dbcontext.SaveChanges();
                    return Json(code, JsonRequestBehavior.AllowGet);
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
        public ActionResult GetCommissionDetails(int id)
        {
            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                {
                    dbcontext.Configuration.ProxyCreationEnabled = false;
                    var popupInvoiceAmt = (from b in dbcontext.Tbl_GenerateDebitNote where b.DN_ShipmentID == id select b.DN_ShipmentAmount);
                    var popupFabStatus = (from b in dbcontext.Tbl_GenerateDebitNote where b.DN_ShipmentID == id select b.DN__FOBStatus).FirstOrDefault();
                    if (popupFabStatus == 0)
                    {
                        var data = (from a in dbcontext.Tbl_Shipment_Details
                                    where a.SD_PID == id
                                    select new
                                    {
                                        scheduleCode = (from b in dbcontext.Tbl_Schedule where b.SH_ID == a.SD_ScheduleID select b.SH_Code).FirstOrDefault(),
                                        popupOrderNo = (from b in dbcontext.Tbl_Master_Order
                                                        join c in dbcontext.Tbl_Order_Details on b.SO_ID equals c.AGEN_TRAD_PO_ID
                                                        where c.ORDER_ID == a.SD_OrderDetailID
                                                        select
                                                        b.SO_Code).FirstOrDefault(),
                                        popupCommissionPer = (from b in dbcontext.Tbl_Master_Order
                                                              join c in dbcontext.Tbl_Order_Details on b.SO_ID equals c.AGEN_TRAD_PO_ID
                                                              where c.ORDER_ID == a.SD_OrderDetailID
                                                              select
                                                              b.SO_Commision).FirstOrDefault(),
                                        OrderAmt = (from b in dbcontext.Tbl_Master_Order
                                                    join c in dbcontext.Tbl_Order_Details on b.SO_ID equals c.AGEN_TRAD_PO_ID
                                                    where c.ORDER_ID == a.SD_OrderDetailID
                                                    select (c.PRICE * a.SD_Quantity)).FirstOrDefault(),
                                    }).GroupBy(m => m.popupOrderNo).Select(a => new { ordNo = a.Select(m => m.popupOrderNo).FirstOrDefault(), ordAmt = a.Sum(m => m.OrderAmt), commission = a.Select(m => m.popupCommissionPer).FirstOrDefault() }).ToList();
                        var popupCommissionAmt = (from a in data select (a.commission * a.ordAmt / 100)).Sum();
                        var result = new { popupInvoiceAmt, data, popupCommissionAmt, isfab = 0 };
                        var json = new JavaScriptSerializer().Serialize(result);
                        return Json(json, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        string popupFabAmt = (from b in dbcontext.Tbl_GenerateDebitNote where b.DN_ShipmentID == id select b.DN_FOBAmount).FirstOrDefault();
                        if (popupFabAmt.Substring(popupFabAmt.Length - 1) == ",")
                        {
                            popupFabAmt = popupFabAmt.Substring(0, popupFabAmt.Length - 1);
                        }
                        var FabAmounts = new HashSet<decimal>(popupFabAmt.Split(',').Select(m => Convert.ToDecimal(m)).ToList()).ToList();

                        var data = (from a in dbcontext.Tbl_Shipment_Details
                                    where a.SD_PID == id
                                    select new
                                    {
                                        scheduleCode = (from b in dbcontext.Tbl_Schedule where b.SH_ID == a.SD_ScheduleID select b.SH_Code).FirstOrDefault(),
                                        popupOrderNo = (from b in dbcontext.Tbl_Master_Order
                                                        join c in dbcontext.Tbl_Order_Details on b.SO_ID equals c.AGEN_TRAD_PO_ID
                                                        where c.ORDER_ID == a.SD_OrderDetailID
                                                        select
                                                        b.SO_Code).FirstOrDefault(),
                                        popupCommissionPer = (from b in dbcontext.Tbl_Master_Order
                                                              join c in dbcontext.Tbl_Order_Details on b.SO_ID equals c.AGEN_TRAD_PO_ID
                                                              where c.ORDER_ID == a.SD_OrderDetailID
                                                              select
                                                              b.SO_Commision).FirstOrDefault(),
                                    }).GroupBy(m => m.popupOrderNo).Select(a => new { ordNo = a.Select(m => m.popupOrderNo).FirstOrDefault(), commission = a.Select(m => m.popupCommissionPer).FirstOrDefault() }).ToList();
                        decimal popupCommissionAmt = 0;
                        for (int i = 0; i < data.Count(); i++)
                        {
                            popupCommissionAmt = popupCommissionAmt + (data[i].commission * FabAmounts[i] / 100) ?? 0;
                        }
                        var result = new { popupInvoiceAmt, data, popupCommissionAmt, isfab = 1, FabAmounts };
                        var json = new JavaScriptSerializer().Serialize(result);
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
                return RedirectToAction("ET_SessionExpire", "ET_Login");
            }
        }
        public ActionResult ET_Sales_Shipment_CommissionRecieve(int id, decimal fabamount, decimal invamt, decimal CommissionAmount, decimal CommissionRecieved, string CommissionDate)
        {
            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                {
                    dbcontext.Configuration.ProxyCreationEnabled = false;
                    DateTime date = DateTime.ParseExact(CommissionDate, "dd-MM-yyyy", null);
                    Tbl_CommissionRecieve Objmc = new Tbl_CommissionRecieve()
                    {
                        CR_ShipmentID = id,
                        CR_ShipmentAmount = invamt,
                        CR_FABAmount = fabamount,
                        CR_CommissionAmount = CommissionAmount,
                        CR_CommissionRecieved = CommissionRecieved,
                        CR_Date = date,
                        CREATED_BY = Convert.ToInt32(Session["UserID"].ToString()),
                        CREATED_DATE = DateTime.Now,
                        DELETED = false,
                        COM_KEY = Convert.ToInt32(Session["CompanyKey"].ToString()),
                    };
                    dbcontext.Tbl_CommissionRecieve.Add(Objmc);
                    dbcontext.SaveChanges();
                    int len = 10 - ("CMM" + Objmc.CR_ID).Length;
                    string code = "CMM" + new String('0', len) + Objmc.CR_ID;
                    Tbl_CommissionRecieve _Tbl_CommissionRecieve = dbcontext.Tbl_CommissionRecieve.Single(m => m.CR_ID == Objmc.CR_ID);
                    {
                        _Tbl_CommissionRecieve.CR_Code = code;
                    };
                    dbcontext.SaveChanges();
                    dbcontext.Tbl_Shipment_Header.Single(m => m.S_ID == id).S_CommissionRecievedStatus = true;
                    dbcontext.SaveChanges();
                    return Json(code, JsonRequestBehavior.AllowGet);
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
        public ActionResult DebitNote_Print(int id, string lang)
        {
            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                {
                    dbcontext.Configuration.ProxyCreationEnabled = false;
                    int com_key = Convert.ToInt32(Session["Companykey"]);
                    var popupFabStatus = (from b in dbcontext.Tbl_GenerateDebitNote where b.DN_ShipmentID == id select b.DN__FOBStatus).FirstOrDefault();
                    if (popupFabStatus == 0)
                    {

                        var data2 = (from a in dbcontext.Tbl_Shipment_Details
                                     join b in dbcontext.Tbl_Shipment_Header on a.SD_PID equals b.S_ID
                                     join c in dbcontext.Tbl_Master_CompanyDetails on b.S_SupplierID equals c.COM_ID into ord
                                     from x in ord
                                     where b.S_ID == id
                                     select new
                                     {
                                         scheduleCode = (from b in dbcontext.Tbl_Schedule where b.SH_ID == a.SD_ScheduleID select b.SH_Code).FirstOrDefault(),
                                         popupOrderNo = (from b in dbcontext.Tbl_Master_Order
                                                         join c in dbcontext.Tbl_Order_Details on b.SO_ID equals c.AGEN_TRAD_PO_ID
                                                         where c.ORDER_ID == a.SD_OrderDetailID
                                                         select
                                                         b.SO_Code).FirstOrDefault(),
                                         popupCommissionPer = (from b in dbcontext.Tbl_Master_Order
                                                               join c in dbcontext.Tbl_Order_Details on b.SO_ID equals c.AGEN_TRAD_PO_ID
                                                               where c.ORDER_ID == a.SD_OrderDetailID
                                                               select
                                                               b.SO_Commision).FirstOrDefault(),
                                         OrderAmt = (from b in dbcontext.Tbl_Master_Order
                                                     join c in dbcontext.Tbl_Order_Details on b.SO_ID equals c.AGEN_TRAD_PO_ID
                                                     where c.ORDER_ID == a.SD_OrderDetailID
                                                     select (c.PRICE * a.SD_Quantity)).FirstOrDefault()
                                     }).GroupBy(m => m.popupOrderNo).Select(a => new Shipment_CM { popupOrderNo = a.Select(m => m.popupOrderNo).FirstOrDefault(), OrderAmt = a.Sum(m => m.OrderAmt), popupCommissionPer = a.Select(m => m.popupCommissionPer).FirstOrDefault() }).ToList();
                        var popupCommissionAmt = (from a in data2 select (a.popupCommissionPer * a.OrderAmt / 100)).Sum();
                        var floor = Math.Floor(popupCommissionAmt ?? 0);
                        var intval = Convert.ToInt32(floor);
                        var floatval = popupCommissionAmt - intval;
                        var floatvalexa = floatval * 100;
                        //var floatint = Math.Floor(floatvalexa??0);
                        string AmtInWords = ConvertNumbertoWords(intval);
                        string FloatAmt = ConvertNumbertoWords(Convert.ToInt32(floatvalexa));
                        AmtInWords = AmtInWords + " AND " + FloatAmt;
                        var data1 = (from a in dbcontext.Tbl_Shipment_Details
                                     join b in dbcontext.Tbl_Shipment_Header on a.SD_PID equals b.S_ID
                                     join c in dbcontext.Tbl_Master_CompanyDetails on b.S_SupplierID equals c.COM_ID into ord
                                     from x in ord
                                     where b.S_ID == id
                                     select new Shipment_CM
                                     {
                                         S_ID = b.S_ID,
                                         S_Code = b.S_Code,
                                         S_INV_NO = b.S_INV_NO,
                                         S_INV_DATE = b.S_INV_DATE,
                                         COM_DISPLAYNAME = x.COM_DISPLAYNAME,
                                         CompanyCode = x.COM_CODE,
                                         CompanyName = x.COM_NAME,
                                         Street = x.COM_STREET,
                                         CityState = (x.COM_CITY + ", " + x.COM_STATE),
                                         USTID = x.COM_USTID,
                                         CountryZip = ((dbcontext.locations.Where(a => a.location_id == x.COM_COUNTRY).Select(a => a.name).FirstOrDefault()) + ", " + (x.COM_ZIP)),
                                         VatPer = (dbcontext.Tbl_SystemSetUp.Where(a => a.COMPANY_ID == com_key).Select(a => a.TAX).FirstOrDefault()),
                                         imgurl = (dbcontext.Tbl_SystemSetUp.Where(a => a.COMPANY_ID == com_key).Select(a => a.COMPANY_LOGO).FirstOrDefault()),
                                         SystemCompany = (dbcontext.Tbl_SystemSetUp.Where(a => a.COMPANY_ID == com_key).Select(a => a.COMPANY_NAME).FirstOrDefault()),
                                         DebitNoteCode = (dbcontext.Tbl_GenerateDebitNote.Where(z => z.DN_ShipmentID == id).Select(z => z.DN_Code).FirstOrDefault()),
                                         DebitNoteDate = (dbcontext.Tbl_GenerateDebitNote.Where(z => z.DN_ShipmentID == id).Select(z => z.CREATED_DATE ?? DateTime.Now).FirstOrDefault()),
                                         AmtInWords = AmtInWords,
                                         isfob = 0
                                     }).ToList();
                        ShipmentView_CM obj = new ShipmentView_CM();
                        obj.headerObj = data1;
                        obj.detailsObj = data2;
                        if (lang == "E")
                            return PartialView("/Views/Sales/ET_Sales_Shipment/ET_Sales_Shipment_Print.cshtml", obj);
                        else
                            return PartialView("/Views/Sales/ET_Sales_Shipment/ET_Sales_Shipment_Print_German.cshtml", obj);
                    }
                    else
                    {
                        string popupFabAmt = (from b in dbcontext.Tbl_GenerateDebitNote where b.DN_ShipmentID == id select b.DN_FOBAmount).FirstOrDefault();
                        if (popupFabAmt.Substring(popupFabAmt.Length - 1) == ",")
                        {
                            popupFabAmt = popupFabAmt.Substring(0, popupFabAmt.Length - 1);
                        }
                        var FabAmounts = new HashSet<decimal>(popupFabAmt.Split(',').Select(m => Convert.ToDecimal(m)).ToList()).ToList();
                        var data2 = (from a in dbcontext.Tbl_Shipment_Details
                                     join b in dbcontext.Tbl_Shipment_Header on a.SD_PID equals b.S_ID
                                     join c in dbcontext.Tbl_Master_CompanyDetails on b.S_SupplierID equals c.COM_ID into ord
                                     from x in ord
                                     where b.S_ID == id
                                     select new
                                     {
                                         scheduleCode = (from b in dbcontext.Tbl_Schedule where b.SH_ID == a.SD_ScheduleID select b.SH_Code).FirstOrDefault(),
                                         popupOrderNo = (from b in dbcontext.Tbl_Master_Order
                                                         join c in dbcontext.Tbl_Order_Details on b.SO_ID equals c.AGEN_TRAD_PO_ID
                                                         where c.ORDER_ID == a.SD_OrderDetailID
                                                         select
                                                         b.SO_Code).FirstOrDefault(),
                                         popupCommissionPer = (from b in dbcontext.Tbl_Master_Order
                                                               join c in dbcontext.Tbl_Order_Details on b.SO_ID equals c.AGEN_TRAD_PO_ID
                                                               where c.ORDER_ID == a.SD_OrderDetailID
                                                               select
                                                               b.SO_Commision).FirstOrDefault()
                                     }).GroupBy(m => m.popupOrderNo).Select(a => new Shipment_CM { popupOrderNo = a.Select(m => m.popupOrderNo).FirstOrDefault(), popupCommissionPer = a.Select(m => m.popupCommissionPer).FirstOrDefault() }).ToList();
                        for (int i = 0; i < data2.Count(); i++)
                        {
                            data2[i].OrderAmt = FabAmounts[i];
                        }
                        var popupCommissionAmt = (from a in data2 select (a.popupCommissionPer * a.OrderAmt / 100)).Sum();
                        var floor = Math.Floor(popupCommissionAmt ?? 0);
                        var intval = Convert.ToInt32(floor);
                        var floatval = popupCommissionAmt - intval;
                        var floatvalexa = floatval * 100;
                        string AmtInWords = ConvertNumbertoWords(intval);
                        string FloatAmt = ConvertNumbertoWords(Convert.ToInt32(floatvalexa));
                        AmtInWords = AmtInWords + " AND " + FloatAmt;
                        var data1 = (from a in dbcontext.Tbl_Shipment_Details
                                     join b in dbcontext.Tbl_Shipment_Header on a.SD_PID equals b.S_ID
                                     join c in dbcontext.Tbl_Master_CompanyDetails on b.S_SupplierID equals c.COM_ID into ord
                                     from x in ord
                                     where b.S_ID == id
                                     select new Shipment_CM
                                     {
                                         S_ID = b.S_ID,
                                         S_Code = b.S_Code,
                                         S_INV_NO = b.S_INV_NO,
                                         S_INV_DATE = b.S_INV_DATE,
                                         COM_DISPLAYNAME = x.COM_DISPLAYNAME,
                                         CompanyCode = x.COM_CODE,
                                         CompanyName = x.COM_NAME,
                                         Street = x.COM_STREET,
                                         CityState = (x.COM_CITY + ", " + x.COM_STATE),
                                         USTID = x.COM_USTID,
                                         CountryZip = ((dbcontext.locations.Where(a => a.location_id == x.COM_COUNTRY).Select(a => a.name).FirstOrDefault()) + ", " + (x.COM_ZIP)),
                                         VatPer = (dbcontext.Tbl_SystemSetUp.Where(a => a.COMPANY_ID == com_key).Select(a => a.TAX).FirstOrDefault()),
                                         imgurl = (dbcontext.Tbl_SystemSetUp.Where(a => a.COMPANY_ID == com_key).Select(a => a.COMPANY_LOGO).FirstOrDefault()),
                                         SystemCompany = (dbcontext.Tbl_SystemSetUp.Where(a => a.COMPANY_ID == com_key).Select(a => a.COMPANY_NAME).FirstOrDefault()),
                                         DebitNoteCode = (dbcontext.Tbl_GenerateDebitNote.Where(z => z.DN_ShipmentID == id).Select(z => z.DN_Code).FirstOrDefault()),
                                         DebitNoteDate = (dbcontext.Tbl_GenerateDebitNote.Where(z => z.DN_ShipmentID == id).Select(z => z.CREATED_DATE ?? DateTime.Now).FirstOrDefault()),
                                         AmtInWords = AmtInWords,
                                         isfob = 1
                                     }).ToList();
                        ShipmentView_CM obj = new ShipmentView_CM();
                        obj.headerObj = data1;
                        obj.detailsObj = data2;
                        if (lang == "E")
                            return PartialView("/Views/Sales/ET_Sales_Shipment/ET_Sales_Shipment_Print.cshtml", obj);
                        else
                            return PartialView("/Views/Sales/ET_Sales_Shipment/ET_Sales_Shipment_Print_German.cshtml", obj);
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
                return RedirectToAction("ET_SessionExpire", "ET_Login");
            }
        }
        public string ConvertNumbertoWords(int number)
        {
            if (number == 0)
                return "ZERO";
            if (number < 0)
                return "minus " + ConvertNumbertoWords(Math.Abs(number));
            string words = "";
            if ((number / 1000000) > 0)
            {
                words += ConvertNumbertoWords(number / 1000000) + " MILLION ";
                number %= 1000000;
            }
            if ((number / 1000) > 0)
            {
                words += ConvertNumbertoWords(number / 1000) + " THOUSAND ";
                number %= 1000;
            }
            if ((number / 100) > 0)
            {
                words += ConvertNumbertoWords(number / 100) + " HUNDRED ";
                number %= 100;
            }
            if (number > 0)
            {
                if (words != "")
                    words += "AND ";
                var unitsMap = new[] { "ZERO", "ONE", "TWO", "THREE", "FOUR", "FIVE", "SIX", "SEVEN", "EIGHT", "NINE", "TEN", "ELEVEN", "TWELVE", "THIRTEEN", "FOURTEEN", "FIFTEEN", "SIXTEEN", "SEVENTEEN", "EIGHTEEN", "NINETEEN" };
                var tensMap = new[] { "ZERO", "TEN", "TWENTY", "THIRTY", "FORTY", "FIFTY", "SIXTY", "SEVENTY", "EIGHTY", "NINETY" };

                if (number < 20)
                    words += unitsMap[Convert.ToInt32(number)];
                else
                {
                    words += tensMap[Convert.ToInt32(number) / 10];
                    if ((number % 10) > 0)
                        words += " " + unitsMap[Convert.ToInt32(number) % 10];
                }
            }
            return words;
        }
    }
}