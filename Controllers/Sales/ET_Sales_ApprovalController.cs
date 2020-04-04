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

namespace Euro.Controllers.Sales
{
    public class ET_Sales_ApprovalController : Controller
    {
        public static string prefix = "";
        public static bool automanual = false;
        BAL bal = new BAL();
        error_master objERR = new error_master();
        tbl_loginfo objLOG = new tbl_loginfo();
        EntityClasses dbcontext = new EntityClasses();
        // GET: Approval
        public ActionResult ET_Sales_Approval()
        { bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                ViewBag.Login_Name = Session["DisplayName"].ToString();
                return View();
            }
            else
            {
                return RedirectToAction("ET_SessionExpire", "ET_Login");
            }
        }
        public JsonResult GetPendingRecords()
        {

            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                {


                    int com_key = Convert.ToInt32(Session["Companykey"].ToString());
                    int UserID = Convert.ToInt32(Session["UserID"].ToString());
                    dbcontext.Configuration.ProxyCreationEnabled = false;
                    List<Approval_CM> obj1 = (from a in dbcontext.tbl_QuotationHeader
                                              join d in dbcontext.Tbl_Master_User on a.Q_Sales_Person equals d.USER_ID into user
                                              from z in user
                                              where a.Q_ApprovedStatus == 0 && a.COM_KEY == com_key && a.DELETED == false && UserID == a.Q_Approver
                                              select new Approval_CM
                                              {
                                                  ID = a.Q_ID,
                                                  Code = a.Q_Code,
                                                  CreatedBy = (dbcontext.Tbl_Master_User.FirstOrDefault(m => m.USER_ID == a.CREATED_BY).DISPLAY_NAME),
                                                  SalesPersonName = z.USER_NAME,
                                                  Date = ( a.LAST_UPDATED_DATE == null ? a.CREATED_DATE??DateTime.Now:a.LAST_UPDATED_DATE ?? DateTime.Now),
                                                  Form = "Quotation"

                                              }).ToList();
                    List<Approval_CM> obj2 = (from a in dbcontext.Tbl_Master_Order
                                              join d in dbcontext.Tbl_Master_User on a.SO_SalesPersonID equals d.USER_ID into user
                                              from z in user
                                              where a.SO_Approval == 0 && a.COM_KEY == com_key && a.DELETED == false && UserID == a.SO_Approver
                                              select new Approval_CM
                                              {
                                                  ID = a.SO_ID,
                                                  Code = a.SO_Code,
                                                  CreatedBy = (dbcontext.Tbl_Master_User.FirstOrDefault(m => m.USER_ID == a.CREATED_BY).DISPLAY_NAME),
                                                  SalesPersonName = z.USER_NAME,
                                                  Date = (a.LAST_UPDATED_DATE == null ? a.CREATED_DATE ?? DateTime.Now : a.LAST_UPDATED_DATE ?? DateTime.Now),
                                                  Form = "Order"
                                              }).ToList();
                    List<Approval_CM> obj3 = (from a in dbcontext.Tbl_Shipment_Header
                                              join d in dbcontext.Tbl_Master_User on a.S_SalesPerson equals d.USER_ID into user
                                              from z in user
                                              where a.S_STATUS == 0 && a.COM_KEY == com_key && a.DELETED == false && UserID == a.S_Approver
                                              select new Approval_CM
                                              {
                                                  ID = a.S_ID,
                                                  Code = a.S_Code,
                                                  CreatedBy = (dbcontext.Tbl_Master_User.FirstOrDefault(m => m.USER_ID == a.CREATED_BY).DISPLAY_NAME),
                                                  SalesPersonName = z.USER_NAME,
                                                  Date = (a.LAST_UPDATED_DATE == null ? a.CREATED_DATE ?? DateTime.Now : a.LAST_UPDATED_DATE ?? DateTime.Now),
                                                  Form = "Shipment"
                                              }).ToList();
                    List<Approval_CM> obj4 = (from a in dbcontext.Tbl_Shipment_Header
                                              join d in dbcontext.Tbl_Master_User on a.S_SalesPerson equals d.USER_ID into user
                                              from z in user
                                              where a.S_DebitNoteApprovalStatus == 0 && a.COM_KEY == com_key && a.DELETED == false && UserID == a.S_Approver && a.S_Type == 1 && a.S_STATUS == 1 && a.S_DebitNoteStatus == true
                                              select new Approval_CM
                                              {
                                                  ID = a.S_ID,
                                                  Code = a.S_Code,
                                                  CreatedBy = (dbcontext.Tbl_Master_User.FirstOrDefault(m => m.USER_ID == a.CREATED_BY).DISPLAY_NAME),
                                                  SalesPersonName = z.USER_NAME,
                                                  Date = (a.LAST_UPDATED_DATE == null ? a.CREATED_DATE ?? DateTime.Now : a.LAST_UPDATED_DATE ?? DateTime.Now),
                                                  Form = "DebitNote"
                                              }).ToList();

                    List<Approval_CM> obj5 = (from a in dbcontext.tbl_PurchaseOrderHeader
                                             
                                              where a.Po_ApprovalStatus == 0 && a.COM_KEY == com_key && a.DELETED == false && UserID == a.PO_Approver
                                              select new Approval_CM
                                              {
                                                  ID = a.PP_ID,
                                                  Code = a.PO_Code,
                                                  CreatedBy = (dbcontext.Tbl_Master_User.FirstOrDefault(m => m.USER_ID == a.CREATED_BY).DISPLAY_NAME),
                                                  SalesPersonName = "",
                                                  Date = (a.LAST_UPDATED_DATE == null ? a.CREATED_DATE ?? DateTime.Now : a.LAST_UPDATED_DATE ?? DateTime.Now),
                                                  Form = "PO"

                                              }).ToList();
                    List<Approval_CM> obj6 = (from a in dbcontext.Tbl_OneToOneInvoice

                                              where a.OTOI_Approval == 0 && a.COM_KEY == com_key && a.DELETED == false && UserID == a.OTOI_Approver
                                              select new Approval_CM
                                              {
                                                  ID = a.OTOI_ID,
                                                  Code = a.OTOI_Code,
                                                  CreatedBy = (dbcontext.Tbl_Master_User.FirstOrDefault(m => m.USER_ID == a.CREATED_BY).DISPLAY_NAME),
                                                  SalesPersonName = "",
                                                  Date = (a.LAST_UPDATED_DATE == null ? a.CREATED_DATE ?? DateTime.Now : a.LAST_UPDATED_DATE ?? DateTime.Now),
                                                  Form = "OneToOneInvoice"

                                              }).ToList();
                    List<Approval_CM> obj7 = (from a in dbcontext.Tbl_OneToManyInvoice

                                              where a.OTMI_Approval == 0 && a.COM_KEY == com_key && a.DELETED == false && UserID == a.OTMI_Approver
                                              select new Approval_CM
                                              {
                                                  ID = a.OTMI_ID,
                                                  Code = a.OTMI_Code,
                                                  CreatedBy = (dbcontext.Tbl_Master_User.FirstOrDefault(m => m.USER_ID == a.CREATED_BY).DISPLAY_NAME),
                                                  SalesPersonName = "",
                                                  Date = (a.LAST_UPDATED_DATE == null ? a.CREATED_DATE ?? DateTime.Now : a.LAST_UPDATED_DATE ?? DateTime.Now),
                                                  Form = "OneToManyInvoice"

                                              }).ToList();
                    List<Approval_CM> obj = new List<Approval_CM>();
                    obj.AddRange(obj1);
                    obj.AddRange(obj2);
                    obj.AddRange(obj3);
                    obj.AddRange(obj4);
                    obj.AddRange(obj5);
                    obj.AddRange(obj6);
                    obj.AddRange(obj7);
                    var json = new JavaScriptSerializer().Serialize(obj);
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
        public JsonResult approverejectRecords(int ID, string FormName, int Status)
        {
            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                {
                    var json = "Success";
                    try
                    {
                        if (FormName == "Quotation")
                        {
                            tbl_QuotationHeader Obj_tbl_QuotationHeader = dbcontext.tbl_QuotationHeader.Single(m => m.Q_ID == ID);
                            {

                                Obj_tbl_QuotationHeader.Q_ApprovedStatus = Status;
                            };
                            dbcontext.SaveChanges();
                        }
                        else if (FormName == "Order")
                        {
                            Tbl_Master_Order Obj_Tbl_Master_Order = dbcontext.Tbl_Master_Order.Single(m => m.SO_ID == ID);
                            {

                                Obj_Tbl_Master_Order.SO_Approval = Status;
                            };
                            dbcontext.SaveChanges();
                        }
                        else if (FormName == "Shipment")
                        {
                            Tbl_Shipment_Header Obj_Tbl_Shipment_Header = dbcontext.Tbl_Shipment_Header.Single(m => m.S_ID == ID);
                            {

                                Obj_Tbl_Shipment_Header.S_STATUS = Status;
                            };
                            dbcontext.SaveChanges();
                        }
                        else if (FormName == "DebitNote")
                        {
                            Tbl_Shipment_Header Obj_Tbl_Shipment_Header = dbcontext.Tbl_Shipment_Header.Single(m => m.S_ID == ID);
                            {

                                Obj_Tbl_Shipment_Header.S_DebitNoteApprovalStatus = Status;
                            };
                            dbcontext.SaveChanges();
                        }
                        else if (FormName == "PO")
                        {
                            tbl_PurchaseOrderHeader Obj_tbl_PurchaseOrderHeader = dbcontext.tbl_PurchaseOrderHeader.Single(m => m.PP_ID == ID);
                            {

                                Obj_tbl_PurchaseOrderHeader.Po_ApprovalStatus = Status;
                            };
                            dbcontext.SaveChanges();
                        }
                        else if(FormName== "OneToOneInvoice")
                        {
                            Tbl_OneToOneInvoice Obj_tbl_OneToOneInvoice = dbcontext.Tbl_OneToOneInvoice.Single(m => m.OTOI_ID == ID);
                            {

                                Obj_tbl_OneToOneInvoice.OTOI_Approval = Status;
                            };
                            dbcontext.SaveChanges();
                        }
                        else if (FormName == "OneToManyInvoice")
                        {
                            Tbl_OneToManyInvoice Obj_tbl_OneToManyInvoice = dbcontext.Tbl_OneToManyInvoice.Single(m => m.OTMI_ID == ID);
                            {

                                Obj_tbl_OneToManyInvoice.OTMI_Approval = Status;
                            };
                            dbcontext.SaveChanges();
                        }
                    }
                    catch
                    {
                        json = "Failed";

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
                return Json("Expired", JsonRequestBehavior.AllowGet);
            }
        }

    }

}    
