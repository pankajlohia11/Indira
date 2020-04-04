using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using BusinessEntity.EntityModels;
using BusinessEntity.CustomModels;

namespace DataAccess
{
    public class DAL
    {
        EntityClasses dbcontext = new EntityClasses();

        public List<Tbl_Master_User> Authenticate_DA(Tbl_Master_User objBE)
        {
            var result = (dbcontext.Tbl_Master_User.Where(m => m.USER_NAME == objBE.USER_NAME && m.USER_PASSWORD == objBE.USER_PASSWORD)).ToList();
            return result;
        }
        public int ExceptionInsertLogs_DA(error_master objBE)
        {
            int s = 0;
            try
            {
                error_master Objtmr = new error_master()
                {
                    err_title = objBE.err_title,
                    err_message = objBE.err_message,
                    err_details = objBE.err_details,
                    err_datetime=DateTime.Now
                };
                dbcontext.error_master.Add(Objtmr);
                dbcontext.SaveChanges();
                s = Objtmr.error_master_id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
                return s;
        }
        public int OperationInsertLogs_DA(tbl_loginfo objBE)
        {
            int s = 0;
            try
            {
                tbl_loginfo Objtmr = new tbl_loginfo()
                {
                    log_dockey = objBE.log_dockey,
                    log_operation = objBE.log_operation,
                    log_userid = objBE.log_userid,
                    log_date = DateTime.Now,
                    log_recordkey = objBE.log_recordkey,
                    log_Remarks = objBE.log_Remarks,
                   
                };
                dbcontext.tbl_loginfo.Add(Objtmr);
                dbcontext.SaveChanges();
                s = Objtmr.log_id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return s;
        }
        public List<Tbl_AccessPermissions> GetPrivilages_DA(int userid, int dockey)
        {
            decimal roleid = Convert.ToDecimal((from a in dbcontext.Tbl_Master_User
                                                where a.USER_ID == userid
                                                select a.ROLE_ID).Single());
            var data = dbcontext.Tbl_AccessPermissions.Where(m => m.ROLE_ID == roleid & m.FORMS == dockey.ToString()).ToList();

            return data;
        }

        public  bool CheckIfAdminUser_DA(int userid)
        {
            decimal roleid = Convert.ToDecimal((from a in dbcontext.Tbl_Master_User
                                                where a.USER_ID == userid
                                                select a.ROLE_ID).Single());

            if (roleid == 1)
                return true;

            return false;
        }

        public List<location> Binddropdown_Country_DL()
        {
            try
            {
                var Country = dbcontext.locations.Where(m => m.location_type == 0).ToList();
                //  dbc.emp.Select(m => new SelectListItem { Text = m.Dept, Value = m.empid.ToString() }).Distinct().ToList();
                return Country;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<location> Binddropdown_State_DL(int id)
        {
            try
            {
                var State = dbcontext.locations.Where(m => m.location_type == 1 && m.parent_id == id).ToList();
                return State;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<location> Binddropdown_City_DL(int id)
        {
            try
            {
                var City = dbcontext.locations.Where(m => m.location_type == 2 && m.parent_id == id).ToList();
                return City;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<Tbl_Document_Master> AutoManual_DA(int dockey)
        {
            var data = dbcontext.Tbl_Document_Master.Where(m => m.auto_key == dockey).ToList();
            return data;
        }

        public List<Tbl_Master_CompanyContacts> Dropdown_ContactPerson_DL(int id)
        {
            dbcontext.Configuration.ProxyCreationEnabled = false;
            try
            {
                //var Contactname = (from name in dbcontext.Tbl_Master_CompanyContacts
                //                   join offer in dbcontext.Tbl_Offers_Master on name.COM_ID equals offer.COM_ID
                //                  // where name.COM_ID == id && name.DELETED == false && offer.COM_KEY == name.COM_KEY
                //                   select new Tbl_Master_CompanyContacts
                //                   {
                //                       FIRST_NAME = name.FIRST_NAME,
                //                       COM_ID = name.COM_ID,
                //                       LAST_NAME = name.LAST_NAME
                //                   }).ToList();
                var Contactname = dbcontext.Tbl_Master_CompanyContacts.Where(m => m.COM_ID == id).ToList();
                return Contactname;
            }
            catch (Exception exe)
            {
                throw exe;
            }
        }

        public List<Tbl_Master_CompanyDetails> Bind_dropdown_Customer_DL(decimal companykey)
        {
            dbcontext.Configuration.ProxyCreationEnabled = false;
            try
            {
                // var Customername = dbcontext.Tbl_Master_CompanyDetails.Where(m => m.Cust_Supp != 1).Select(m => m.COM_ID && m.COM_NAME).ToList();
                var Customername = dbcontext.Tbl_Master_CompanyDetails.Where(m => m.Cust_Supp != 1 && m.COM_KEY == companykey).OrderBy(m => m.COM_NAME).ToList();
                return Customername;
            }
            catch (Exception exe)
            {
                throw exe;
            }
        }
        public List<Tbl_Master_CompanyDetails> Bind_dropdown_Supplier_DL(decimal companykey)
        {
            dbcontext.Configuration.ProxyCreationEnabled = false;
            try
            {
                var Suppliername = dbcontext.Tbl_Master_CompanyDetails.Where(m => m.Cust_Supp != 0 && m.COM_KEY == companykey).OrderBy(m=>m.COM_NAME).ToList();
                return Suppliername;
            }
            catch (Exception exe)
            {
                throw exe;
            }
        }
        public List<Tbl_Currency_Master> Bind_Currency_DL()
        {
            try
            {
                var Currency = dbcontext.Tbl_Currency_Master.ToList();
                return Currency;
            }
            catch (Exception exe)
            {
                throw exe;
            }
        }

        public List<Tbl_Master_Category> Dropdown_Category_DL(decimal companykey)
        {
            try
            {
                dbcontext.Configuration.ProxyCreationEnabled = false;
                var Category = dbcontext.Tbl_Master_Category.Where(m => m.CAT_CODE != 0 && m.COM_KEY == companykey && m.DELETED == false).ToList();
                return Category;
            }
            catch (Exception exe)
            {
                throw exe;
            }
        }
        public List<Tbl_Product_Master> Dropdown_Products_DL(decimal companykey)
        {
            try
            {
                dbcontext.Configuration.ProxyCreationEnabled = false;
                var product_details = dbcontext.Tbl_Product_Master.Where(m => m.P_ID != 0 && m.DELETED == false && m.COM_KEY == companykey).ToList();
                return product_details;
            }
            catch (Exception exe)
            {
                throw exe;
            }
        }

        public List<Tbl_LookUp_CM> Bind_Packing_DL()
        {
            try
            {
                var data = (from lt in dbcontext.Tbl_LookUpType
                            join lu in dbcontext.tbl_LookUp on lt.LT_ID equals lu.LU_Type
                            select new Tbl_LookUp_CM
                            {
                                LU_ID = lu.LU_ID,
                                LU_Type = lu.LU_Type,
                                LU_Code = lu.LU_Code,
                                LU_Description = lu.LU_Description,
                                LT_Description = lt.LT_Description
                            }).ToList();
                return data;
            }
            catch (Exception exe)
            {
                throw exe;
            }
        }
        public int InsertNotification_DAO(tbl_Notifications obj)
        {
            try
            {
                dbcontext.tbl_Notifications.Add(obj);
                return dbcontext.SaveChanges();
            }
            catch (Exception exe)
            {
                throw exe;
            }
        }
    }
}
