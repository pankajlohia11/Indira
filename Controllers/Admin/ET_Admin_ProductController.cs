using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessEntity.CustomModels;
using BusinessEntity.EntityModels;
using BusinessLogic.Admin_BL;
using BusinessLogic;
using System.Web.Script.Serialization;

namespace Euro.Controllers.Admin
{
    public class ET_Admin_ProductController : Controller
    {
        // GET: ET_Admin_Product
        EntityClasses dbcontext = new EntityClasses();
        public static string Catprefix = "";
        public static string Proprefix = "";
        public static bool Catautomanual = false;
        public static bool Proautomanual = false;
        BAL bal = new BAL();
        error_master objERR = new error_master();
        tbl_loginfo objLOG = new tbl_loginfo();
        ET_Admin_Product_BL ObjBL = new ET_Admin_Product_BL();
        public ActionResult ET_Admin_Product()
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
        //Checking privilage
        public JsonResult GetPrivilages(int id)
        {

            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                {
                    using (EntityClasses dbcontext = new EntityClasses())
                    {
                        var privilagelist = bal.GetPrivilages_BL(Convert.ToInt32(Session["UserID"].ToString()),id);
                        var json = new JavaScriptSerializer().Serialize(privilagelist);
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
        //get uom list
        public JsonResult GetUOM()
        {

            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                {
                    using (EntityClasses dbcontext = new EntityClasses())
                    {
                        var uomlist = ObjBL.ET_Admin_ProductGroup_GetUOM_BL();
                        var json = new JavaScriptSerializer().Serialize(uomlist);
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
        //Checking Code Auto/Manual
        private void AutoManual()
        {
            ViewBag.CategoryAutoManual = false;
            Catautomanual = false;
            List<Tbl_Document_Master> ObjDoc = bal.AutoManual_BL(3013);
            if (ObjDoc.Count > 0)
            {
                foreach (var item in ObjDoc)
                {
                    if (item.autogen_type == "Automatic")
                    {
                        ViewBag.CategoryAutoManual = true;
                        Catprefix = item.autogen_prefix;
                        Catautomanual = true;
                    }
                    else
                    {
                        ViewBag.CategoryRequired = "required";
                    }
                }

            }

            ViewBag.ProductAutoManual = false;
            Proautomanual = false;
            List<Tbl_Document_Master> ObjDocpro = bal.AutoManual_BL(5014);
            if (ObjDocpro.Count > 0)
            {
                foreach (var item in ObjDocpro)
                {
                    if (item.autogen_type == "Automatic")
                    {
                        ViewBag.ProductAutoManual = true;
                        Proprefix = item.autogen_prefix;
                        Proautomanual = true;
                    }
                    else
                    {
                        ViewBag.ProductRequired = "required";
                    }
                }

            }
        }
        //Bind the tree data
        public JsonResult BindTree()
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
                        var parentgroups = dbcontext.Tbl_ProductGroup.Where(x=>x.COM_KEY == com_key && x.DELETED==false && x.PG_TYPE==1 ).ToList();
                        string s = "";
                        for (int i = 0; i < parentgroups.Count(); i++)
                        {
                            if (i == 0)
                            {
                                s = s + " [{'text':'" + parentgroups[i].PG_NAME + "','id':'" + parentgroups[i].PG_ID + "'";
                            }
                            else
                            {
                                s = s + ", {'text':'" + parentgroups[i].PG_NAME + "','id':'" + parentgroups[i].PG_ID + "'";
                            }
                            //get all the child groups of this parent
                            decimal parentid = parentgroups[i].PG_ID;
                            var childgroups = dbcontext.Tbl_ProductGroup.Where(x => x.COM_KEY == com_key && x.DELETED == false && x.PG_TYPE == 2 && x.PG_PARENT_ID==parentid).ToList();
                            if (childgroups.Count() > 0)
                            {
                                s = s + ",'children':[";
                            }
                            for (int j = 0; j < childgroups.Count(); j++)
                            {
                                if (j == 0)
                                {
                                    s = s + " {'text':'" + childgroups[j].PG_NAME + "','id':'" + childgroups[j].PG_ID + "'";
                                }
                                else
                                {
                                    s = s + ", {'text':'" + childgroups[j].PG_NAME + "','id':'" + childgroups[j].PG_ID + "'";
                                }
                                //get all the products of this parent
                                decimal childid = childgroups[j].PG_ID;
                                var products = dbcontext.Tbl_Product_Master.Where(x => x.COM_KEY == com_key && x.DELETED == false && x.P_SubcategoryID == childid).ToList();
                                if (products.Count() > 0)
                                {
                                    s = s + ",'children':[";
                                }
                                for (int k = 0; k < products.Count(); k++)
                                {
                                    if (k == 0)
                                    {
                                        s = s + " {'text':'" + products[k].P_Name + "','id':'P_" + products[k].P_ID + "','icon' : 'jstree-file'}";
                                    }
                                    else
                                    {
                                        s = s + ", {'text':'" + products[k].P_Name + "','id':'P_" + products[k].P_ID + "','icon' : 'jstree-file'}";
                                    }
                                }
                                if (products.Count() > 0)
                                {
                                    s = s + "]";
                                }
                                s = s + "}";
                            }
                            
                            decimal productid = parentgroups[i].PG_ID;
                            var products1 = dbcontext.Tbl_Product_Master.Where(x => x.COM_KEY == com_key && x.DELETED == false && x.P_SubcategoryID == productid).ToList();
                            if (childgroups.Count() == 0)
                            {
                                if (products1.Count() > 0)
                                {
                                    s = s + ",'children':[";
                                }
                                for (int k = 0; k < products1.Count(); k++)
                                {
                                    if (k == 0)
                                    {
                                        s = s + " {'text':'" + products1[k].P_Name + "','id':'P_" + products1[k].P_ID + "','icon' : 'jstree-file'}";
                                    }
                                    else
                                    {
                                        s = s + ", {'text':'" + products1[k].P_Name + "','id':'P_" + products1[k].P_ID + "','icon' : 'jstree-file'}";
                                    }
                                }
                                if (products1.Count() > 0)
                                {
                                    s = s + "]";
                                }
                            }
                            else
                            {
                                if (products1.Count() > 0)
                                {
                                    for (int k = 0; k < products1.Count(); k++)
                                    {
                                        s = s + ", {'text':'" + products1[k].P_Name + "','id':'P_" + products1[k].P_ID + "','icon' : 'jstree-file'}";
                                    }
                                    s = s + "]";
                                }
                            }
                            if (childgroups.Count() > 0 && products1.Count()==0)
                            {
                                s = s + "]";
                            }
                            s = s + "}";
                        }
                        if(parentgroups.Count()>0)
                            s = s + "]";
                        string s5= s.Replace("\"","~");
                        s5 = s5.Replace("'", "\"");
                        s5 = s5.Replace("~","'");
                        
                        return Json(s5, JsonRequestBehavior.AllowGet);
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

     //Server side validation
        private string validations(string GroupName, decimal GroupID, string GroupCode, string Initial, decimal Parent)
        {
            if (!Catautomanual && GroupCode == "")
            {
                return "Enter the Product Group Code";
            }
            if (GroupName == "")
            {
                return "Enter the Group Name";
            }
            if (Parent == 0)
            {
                if (Initial == "")
                {
                    return "Enter Initial";
                }
            }
            if (!Catautomanual)
            {
                string valid = ObjBL.CheckDuplicateCode_BL(GroupID, GroupCode);
                if (valid != "")
                {
                    return "Group Code Already Exist";
                }
            }
            {
                string valid = ObjBL.CheckDuplicateGroupName_BL(GroupID, GroupName);
                if (valid != "")
                {
                    return "Group Name Already Exist";
                }
            }

            return "";
        }
        //Insert/update the product Group
        [HttpPost]
        public JsonResult ET_Admin_ProductGroup_Add(decimal GroupID, string GroupCode, string GroupName,  decimal Parent,string Initial)
        {
            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                {
                    string valid = validations(GroupName, GroupID, GroupCode, Initial, Parent);
                    if (valid == "")
                    {
                        decimal ParentType = 2;
                        if (Parent == 0)
                        {
                            ParentType = 1;
                            Initial = Initial.ToUpper();
                        }
                        Tbl_ProductGroup obj = new Tbl_ProductGroup()
                        {
                            PG_ID = GroupID,
                            PG_NAME = GroupName,
                            PG_CODE = GroupCode,
                            PG_PARENT_ID = Parent,
                            PG_TYPE = ParentType,
                            PG_Initial= Initial,
                            CREATED_BY = Convert.ToInt64(Session["UserID"]),
                            CREATED_DATE = DateTime.Now,
                            LAST_UPDATED_DATE = DateTime.Now,
                            DELETED = false,
                            LAST_UPDATED_BY = Convert.ToInt64(Session["UserID"]),
                            DELETED_BY = Convert.ToInt64(Session["UserID"]),
                            COM_KEY = Convert.ToInt32(Session["CompanyKey"])
                        };

                        Tbl_ProductGroup d = ObjBL.ET_Admin_ProductGroup_Add_BL(obj, Catautomanual, Catprefix);
                        if (d.PG_ID != 0)
                        {
                            objLOG.log_dockey = "3013";
                            objLOG.log_userid = Session["UserID"].ToString();
                            objLOG.log_recordkey = d.PG_ID.ToString();
                            if (GroupID == 0)
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
                        return Json(d.PG_ID+"_"+d.PG_CODE, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        var json = "Validation:" + valid;
                        return Json(json, JsonRequestBehavior.AllowGet);
                    }
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
        //Edit the product group
        [HttpPost]
        public JsonResult ET_Admin_ProductGroup_Update_GetbyID(int id)
        {
            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                {
                    var data1 = (from a in dbcontext.Tbl_ProductGroup
                                 join b in dbcontext.Tbl_ProductGroup on a.PG_PARENT_ID equals b.PG_ID
                                 into t
                                 from rt in t.DefaultIfEmpty()
                                 where a.PG_ID==id
                                 select new
                                 {
                                     a.PG_ID,
                                     a.PG_CODE,
                                     a.PG_NAME,
                                     a.PG_PARENT_ID,
                                     PG_Parent = rt.PG_NAME,
                                     a.PG_Initial
                                 }
                               ).ToList();
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
        //Delete the product group
        public JsonResult ET_Admin_ProductGroup_Delete(int id,string type)
        {
            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                {
                    var deleteby = Convert.ToInt64(Session["UserID"]);
                    int i = ObjBL.ET_Admin_ProductGroup_Delete_BL(id, deleteby, type);
                    if (i == 1)
                    {
                        objLOG.log_dockey = "3013";
                        objLOG.log_operation = "Delete";
                        objLOG.log_userid = Session["UserID"].ToString();
                        objLOG.log_recordkey = id.ToString();
                        objLOG.log_Remarks = "Record Deleted Successfully";
                        bal.OperationInsertLogs_BL(objLOG);
                    }
                    return Json(i.ToString(), JsonRequestBehavior.AllowGet);
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
        //Validate the product
        private string ProductValidations(decimal ID, string Code, string ArticleNo, decimal CategoryID, decimal SubCategoryID, string Name, string ShortName, string Description, string UOM, decimal PackingQuantity)
        {
            if (!Proautomanual && Code == "")
            {
                return "Enter the Product Code";
            }
            if (Name == "")
            {
                return "Enter the Product Name";
            }
            if (ShortName == "")
            {
                return "Enter the Short Name";
            }
            if (Description == "")
            {
                return "Enter the Description";
            }
            if (UOM == "")
            {
                return "Enter the UOM";
            }
            if (PackingQuantity == 0)
            {
                return "Enter the Packing Quantity";
            }

            if (!Proautomanual)
            {
                string valid = ObjBL.CheckDuplicateProCode_BL(ID, Code);
                if (valid != "")
                {
                    return "Product Code Already Exist";
                }
            }
            {
                string valid = ObjBL.CheckDuplicateProName_BL(ID, Name);
                if (valid != "")
                {
                    return "Product Name Already Exist";
                }
            }
            return "";
        }
        //insert the product
        public ActionResult ET_Admin_Product_Add(decimal ID, string Code,string ArticleNo, decimal CategoryID, decimal SubCategoryID, string Name, string ShortName, string Description, string UOM, decimal PackingQuantity, string Remark1, string Remark2, string Remark3,string PackingQuantityUOM,bool IsStock,int P_ROL, int P_MSL)
        {
            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                {
                    string valid = ProductValidations(ID, Code, ArticleNo, CategoryID, SubCategoryID, Name, ShortName, Description, UOM, PackingQuantity);
                    if (valid == "")
                    {
                        Tbl_Product_Master obj = new Tbl_Product_Master()
                        {
                            P_ID = ID,
                            P_Code = Code,
                            P_ArticleNo = ArticleNo,
                            P_CategoryID = CategoryID,
                            P_SubcategoryID = SubCategoryID,
                            P_Name = Name,
                            P_ShortName = ShortName,
                            P_Description = Description,
                            P_UOM = UOM,
                            P_PackingQuantity = PackingQuantity,
                            P_Remark1 = Remark1,
                            P_Remark2 = Remark2,
                            P_Remark3 = Remark3,
                            P_PackingQuantityUOM = PackingQuantityUOM,
                            P_IsStock = IsStock,
                            P_ROL = P_ROL,
                            P_MSL = P_MSL,
                            CREATED_BY = Convert.ToInt64(Session["UserID"]),
                            LAST_UPDATED_BY = Convert.ToInt64(Session["UserID"]),
                            COM_KEY= Convert.ToInt32(Session["CompanyKey"])
                        };
                        var data = ObjBL.ET_Admin_Product_Add_BL(obj, Proautomanual, Proprefix);
                        if(data.P_ID!=0)
                        {
                            objLOG.log_dockey = "5014";
                            objLOG.log_userid = Session["UserID"].ToString();
                            objLOG.log_recordkey = data.P_ID.ToString();
                            if (ID == 0)
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
                        return Json(data.P_ID + "_" + data.P_Code + "_" + data.P_ArticleNo, JsonRequestBehavior.AllowGet);
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
                return RedirectToAction("ET_SessionExpire", "ET_Login");
            }
        }
        //Delete the product
        public ActionResult ET_Admin_Product_Delete(int id)
        {
            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                {
                    var deleteby = Convert.ToInt64(Session["UserID"]);
                    int i = ObjBL.ET_Admin_Product_Deleted_BL(id, deleteby);
                    if (i!=0)
                    {
                        objLOG.log_dockey = "5014";
                        objLOG.log_operation = "Delete";
                        objLOG.log_userid = Session["UserID"].ToString();
                        objLOG.log_recordkey = id.ToString();
                        objLOG.log_Remarks = "Record Deleted Successfully";
                        bal.OperationInsertLogs_BL(objLOG);
                    }
                    return Json(i.ToString(), JsonRequestBehavior.AllowGet);
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
        //Edit the product 
        public ActionResult ET_Admin_Product_Update_GetbyID(int id)
        {
            bool val = Session["UserID"] == null ? false : true;
            if (val)
            {
                try
                {
                    var data1 = (from a in dbcontext.Tbl_Product_Master
                                 where a.P_ID == id
                                 select new
                                 {
                                     a.P_ID,
                                     a.P_Code,
                                     a.P_ArticleNo,
                                     a.P_CategoryID,
                                     category=(dbcontext.Tbl_ProductGroup.FirstOrDefault(x=>x.PG_ID== a.P_CategoryID).PG_NAME),
                                     a.P_SubcategoryID,
                                     subcategory = (dbcontext.Tbl_ProductGroup.FirstOrDefault(x => x.PG_ID == a.P_SubcategoryID).PG_NAME),
                                     a.P_Name,
                                     a.P_ShortName,
                                     a.P_Description,
                                     a.P_UOM,
                                     a.P_PackingQuantity,
                                     a.P_Remark1,
                                     a.P_Remark2,
                                     a.P_Remark3,
                                     a.P_PackingQuantityUOM,
                                     a.P_IsStock,
                                     a.P_ROL,
                                     a.P_MSL
                                 }
                             ).ToList();
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
