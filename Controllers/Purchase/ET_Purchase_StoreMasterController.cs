using BusinessEntity.EntityModels;
using BusinessEntity.CustomModels;
using BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Euro.Controllers.Purchase
{
    public class ET_Purchase_StoreMasterController : Controller
    {
        // GET: ET_Purchase_StoreMaster
        public static string prefix = "";
        public static bool automanual = false;
        BAL bal = new BAL();
        error_master objERR = new error_master();
        tbl_loginfo objLOG = new tbl_loginfo();
        EntityClasses dbcontext = new EntityClasses();
        public ActionResult ET_Purchase_StoreMaster()
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
        //Checking Code Auto/Manual
        private void AutoManual()
        {
            ViewBag.AutoManual = false;
            automanual = false;
            List<Tbl_Document_Master> ObjDoc = bal.AutoManual_BL(8014);
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
        //Checking privilage
        public JsonResult GetPrivilages()
        {

            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                {
                    var privilagelist = bal.GetPrivilages_BL(Convert.ToInt32(Session["UserID"].ToString()), 8014);
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
        //Get the store list
        public JsonResult GetStoreList(bool delete)
        {

            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                {
                    int comp_key = Convert.ToInt32(Session["CompanyKey"].ToString());
                    dbcontext.Configuration.ProxyCreationEnabled = false;
                    var data = (from a in dbcontext.tbl_StoreMaster
                                join b in dbcontext.Tbl_Master_User on a.SM_Store_User equals b.USER_ID into comp
                                from x in comp                                 
                                where a.SM_DeleteStatus == delete && a.SM_CompanyKey == comp_key
                                select new 
                                {
                                    ID=a.SM_Id,
                                    Code= a.SM_Code,
                                    Name = a.SM_Name,
                                    StoreUser = x.DISPLAY_NAME,
                                    AddressLine1 = a.SM_Addressline1,
                                    Status = a.SM_Activeflag
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
        //get the store user list
        public JsonResult GetStoreUserList()
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
                        var StoreUser = dbcontext.Tbl_Master_User.Where(m => m.COM_KEY == comp_key).ToList();
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
        //Get the country list
        public JsonResult GetCountryList()
        {
            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                {
                    using (EntityClasses dbcontext = new EntityClasses())
                    {
                        var Country = dbcontext.locations.Where(m => m.location_type == 0).ToList();
                        var json = new JavaScriptSerializer().Serialize(Country);
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
        public JsonResult BindStates(int id)
        {
            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                {
                    using (EntityClasses dbcontext = new EntityClasses())
                    {
                        var State = dbcontext.locations.Where(m => m.location_type == 1 && m.parent_id == id).ToList();
                        return Json(State, JsonRequestBehavior.AllowGet);
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

        // Bind Dropdown City Based on State 
        public JsonResult BindCities(int id)
        {
            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                {
                    using (EntityClasses dbcontext = new EntityClasses())
                    {
                        var City = dbcontext.locations.Where(m => m.location_type == 2 && m.parent_id == id).ToList();
                        return Json(City, JsonRequestBehavior.AllowGet);
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
        //Get all the prosuct
        public JsonResult GetProducts()
        {

            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                {
                    dbcontext.Configuration.ProxyCreationEnabled = false;
                    int custkey = Convert.ToInt32(Session["CompanyKey"]);
                    var data = dbcontext.Tbl_Product_Master.Where(m => m.COM_KEY == custkey).ToList();
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
        //Get the particular product details
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
                                     uom = n.LU_Description, 
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
        //Validate the data
        private string Validations(int SM_Id, string SM_Code, string SM_Name, decimal SM_Store_User, string SM_Addressline1,string EnquiryDetails)
        {
            if (!automanual && SM_Code == "")
            {
                return "Enter Store Code";
            }
            if (SM_Name == "")
            {
                return "Enter Store Name";
            }
            if (SM_Store_User == 0)
            {
                return "Select Store User";
            } 
            if (SM_Addressline1 == "")
            {
                return "Enter Address Line 1";
            }
            if (!automanual)
            {
                if (SM_Id == 0)
                {
                    var count = dbcontext.tbl_StoreMaster.Where(m => m.SM_Code == SM_Code).ToList().Count();
                    if (count > 0)
                    {
                        return "Store Code Already Exist";
                    }
                }
                else
                {
                    var count = dbcontext.tbl_StoreMaster.Where(m => m.SM_Id != SM_Id && m.SM_Code == SM_Code).ToList().Count();
                    if (count > 0)
                    {
                        return "Store Code Already Exist";
                    }
                }  
            }
            try
            {
                string[] ChildRow = EnquiryDetails.Split('|');
                string[] tableColumns = new string[ChildRow.Length];
                for (int i = 0; i < ChildRow.Length - 1; i++)
                {
                    string[] ChildRecord = ChildRow[i].Split('}');
                    if (tableColumns.Contains(Convert.ToDecimal(ChildRecord[0]).ToString()))
                    {
                        return "Product is repeated at row :" + (i + 1);
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
        //Insert/update the store details
        [HttpPost]
        public JsonResult ET_Master_Store_Add(int SM_Id, string SM_Code, string SM_Name, decimal SM_Store_User, string SM_Addressline1, string SM_Addressline2, string SM_Country, string SM_State, string SM_City, string SM_Zipcode, bool SM_Activeflag, string EnquiryDetails)
        {
            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                {
                    string valid = Validations(SM_Id, SM_Code, SM_Name, SM_Store_User, SM_Addressline1, EnquiryDetails);
                    if (valid == "")
                    {
                        var Username = Session["UserID"].ToString();

                        tbl_StoreMaster Objmc = new tbl_StoreMaster()
                        {
                            SM_Id = SM_Id,
                            SM_Code = SM_Code,
                            SM_Name = SM_Name,
                            SM_Store_User = SM_Store_User,
                            SM_Addressline1 = SM_Addressline1,
                            SM_Addressline2 = SM_Addressline2,
                            SM_Country = SM_Country,
                            SM_State = SM_State,
                            SM_City = SM_City,
                            SM_Zipcode = SM_Zipcode,
                            SM_Activeflag = SM_Activeflag,
                            SM_EnteredBy = Session["UserID"].ToString(),
                            SM_UpdatedBy = Session["UserID"].ToString(),
                            SM_EnteredDate = DateTime.Now,
                            SM_DeleteStatus = false,
                            SM_CompanyKey = Convert.ToInt32(Session["CompanyKey"])
                        };

                        decimal k = 0;
                        var context = new EntityClasses();
                        var transaction = context.Database.BeginTransaction();
                        bool success = true;
                        try
                        {
                            if (SM_Id == 0)
                            {
                                context.tbl_StoreMaster.Add(Objmc);
                                if (context.SaveChanges() == 0)
                                {
                                    success = false;
                                }
                                k = Objmc.SM_Id;
                                if (automanual == true)
                                {
                                    int len = 10 - (prefix + Objmc.SM_Id).Length;
                                    string code = prefix + new String('0', len) + Objmc.SM_Id;
                                    tbl_StoreMaster Obj_tbl_StoreMaster = context.tbl_StoreMaster.Single(m => m.SM_Id == Objmc.SM_Id);
                                    {
                                        Obj_tbl_StoreMaster.SM_Code = code;
                                    };
                                    SM_Code = code;
                                    if (context.SaveChanges() == 0)
                                    {
                                        success = false;
                                    }
                                }
                            }
                            else
                            {
                                tbl_StoreMaster Obj_tbl_StoreMaster = context.tbl_StoreMaster.Single(m => m.SM_Id == Objmc.SM_Id);
                                {
                                    Obj_tbl_StoreMaster.SM_Code = Objmc.SM_Code;
                                    Obj_tbl_StoreMaster.SM_Name = Objmc.SM_Name;
                                    Obj_tbl_StoreMaster.SM_Store_User = Objmc.SM_Store_User;
                                    Obj_tbl_StoreMaster.SM_Addressline1 = Objmc.SM_Addressline1;
                                    Obj_tbl_StoreMaster.SM_Addressline2 = Objmc.SM_Addressline2;
                                    Obj_tbl_StoreMaster.SM_Country = Objmc.SM_Country;
                                    Obj_tbl_StoreMaster.SM_State = Objmc.SM_State;
                                    Obj_tbl_StoreMaster.SM_City = Objmc.SM_City;
                                    Obj_tbl_StoreMaster.SM_Zipcode = Objmc.SM_Zipcode;
                                    Obj_tbl_StoreMaster.SM_Activeflag = Objmc.SM_Activeflag;
                                    Obj_tbl_StoreMaster.SM_DeleteStatus = Objmc.SM_DeleteStatus;
                                    Obj_tbl_StoreMaster.SM_CompanyKey = Objmc.SM_CompanyKey;
                                    Obj_tbl_StoreMaster.SM_EnteredDate = DateTime.Now;
                                    Obj_tbl_StoreMaster.SM_EnteredBy = Objmc.SM_EnteredBy;
                                };
                                k = Objmc.SM_Id;
                                if (context.SaveChanges() == 0)
                                {
                                    success = false;
                                }
                            }
                            // Delete previous contact data
                            tbl_StoreDetails objdeletecontact = new tbl_StoreDetails();
                            context.tbl_StoreDetails.RemoveRange(context.tbl_StoreDetails.Where(m => m.SD_SM_ID == Objmc.SM_Id));
                            context.SaveChanges();

                            // Insert new contacts data
                            string[] ChildRow = EnquiryDetails.Split('|');
                            for (int i = 0; i < ChildRow.Length - 1; i++)
                            {
                                string[] ChildRecord = ChildRow[i].Split('}');
                                tbl_StoreDetails objStoredetails = new tbl_StoreDetails()
                                {
                                    SD_SM_ID = Convert.ToInt32(Objmc.SM_Id),
                                    SD_Itemcode = Convert.ToInt32(ChildRecord[0]),
                                    SD_ItemDescription = ChildRecord[1],
                                    SD_UOM = ChildRecord[3],
                                    SD_OpeningStock = Convert.ToDecimal(ChildRecord[4]),
                                    SD_OpeningRate = Convert.ToDecimal(ChildRecord[5]),
                                };
                                context.tbl_StoreDetails.Add(objStoredetails);
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
                        var json = "Success:"+ SM_Code;
                        if (k == 0)
                        {
                            json = "Failed";
                        }
                        else
                        {
                            objLOG.log_dockey = "8014";
                            objLOG.log_userid = Session["UserID"].ToString();
                            objLOG.log_recordkey = k.ToString();
                            if (SM_Id == 0)
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
        public ActionResult ET_Master_Store_View(int id)
        {
            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                {
                    dbcontext.Configuration.ProxyCreationEnabled = false;
                    var data1 = (from a in dbcontext.tbl_StoreMaster
                                join b in dbcontext.Tbl_Master_User on a.SM_Store_User equals b.USER_ID into comp
                                from x in comp
                                where a.SM_Id == id
                                select new StoreMaster
                                {
                                    SM_Id = a.SM_Id,
                                    SM_Code = a.SM_Code,
                                    SM_Name = a.SM_Name,
                                    SM_Store_User=a.SM_Store_User,
                                    SM_Addressline2=a.SM_Addressline2,
                                    SM_Addressline1 = a.SM_Addressline1,
                                    SM_Country=a.SM_Country,
                                    SM_State=a.SM_State,
                                    SM_City=a.SM_City,
                                    SM_Zipcode=a.SM_Zipcode,
                                    SM_Activeflag = a.SM_Activeflag
                                }
                                ).ToList();
                    var data2 = (from c in dbcontext.tbl_StoreDetails
                                 join a in dbcontext.Tbl_Product_Master on c.SD_Itemcode equals a.P_ID
                                 join b in dbcontext.tbl_LookUp on a.P_UOM equals b.LU_Code
                                 into m
                                 from n in m.DefaultIfEmpty()
                                 where c.SD_SM_ID == id && n.LU_Type == 2
                                 select new StoreMaster
                                 {
                                     Product_name = a.P_ShortName,
                                     SD_ItemDescription = c.SD_ItemDescription,
                                     SD_UOM = c.SD_UOM,
                                     SD_OpeningStock = c.SD_OpeningStock,
                                     SD_OpeningRate = c.SD_OpeningRate
                                 }).ToList();
                    Store_View_CM obj = new Store_View_CM();
                    obj.storeHeader = data1;
                    obj.storeChild = data2;

                    return PartialView("/Views/Purchase/ET_Purchase_StoreMaster/ET_Purchase_StoreMaster_View.cshtml", obj);
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
        //Restore the delete data
        public ActionResult ET_Master_Store_RestoreDelete(int id, bool type)
        {
            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                {
                    int i;
                    tbl_StoreMaster deleted = dbcontext.tbl_StoreMaster.Single(m => m.SM_Id == id);
                    deleted.SM_DeleteStatus = type;
                    i = dbcontext.SaveChanges();
                    var json = "Failed";
                    if (i != 0)
                    {
                        json = "Success";
                    }
                    else
                    {
                        objLOG.log_dockey = "8014";
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
        public ActionResult ET_Master_Store_Update_GetbyID(int id)
        {
            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                { 
                       var data= dbcontext.tbl_StoreMaster.Single(m => m.SM_Id == id);
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
        //get the peoduct details
        public ActionResult ET_Master_Store_Details_Load(int id)
        {
            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                {
                    var data1 = (from c in dbcontext.tbl_StoreDetails
                                 join a in dbcontext.Tbl_Product_Master on c.SD_Itemcode equals a.P_ID
                                 join b in dbcontext.tbl_LookUp on a.P_UOM equals b.LU_Code
                                 into m
                                 from n in m.DefaultIfEmpty()
                                 where c.SD_SM_ID == id && n.LU_Type == 2
                                 select new
                                 {
                                     productid=a.P_ID,
                                     name = a.P_ShortName,
                                     uom = n.LU_Description,
                                     openingQty = c.SD_OpeningStock,
                                     openingrate = c.SD_OpeningRate
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
        
    }
}