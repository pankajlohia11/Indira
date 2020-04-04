using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessEntity.EntityModels;

namespace DataAccess
{
    public class ET_Admin_Company_DL
    {
        EntityClasses dbcontext = new EntityClasses();
        // DbContext.Configuration.ProxyCreationEnabled = false;
        public List<Tbl_Master_CompanyDetails> ET_Admin_Companylist_DL(bool deleted, int com_key)
        {
            try
            {
                dbcontext.Configuration.ProxyCreationEnabled = false;
                var data = dbcontext.Tbl_Master_CompanyDetails.Where(m => m.DELETED == deleted && m.COM_KEY == com_key).ToList();
                //data = data.Select(x => { x.COM_COUNTRY_DISPLAY = GetCountryDisplay(x.COM_COUNTRY.Value);return x; }).ToList();
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GetCountryDisplay(int countryCode)
        {
            var countryName = (dbcontext.locations.Where(m => m.location_id == countryCode).Select(m => m.name)).FirstOrDefault();
            return countryName;
        }
        public decimal ET_Master_Company_Add_DL(Tbl_Master_CompanyDetails obj, bool automanual, string prefix, string Contactdata, string Bankdata, out string compcode)
        {
            decimal k = 0;
            var context = new EntityClasses();
            var transaction = context.Database.BeginTransaction();
           
            bool success = true;
            try
            {
                
                if (obj.COM_ID == 0)
                {
                    context.Tbl_Master_CompanyDetails.Add(obj);
                    if (context.SaveChanges() == 0)
                    {
                        success = false;
                    }
                    if (automanual == true)
                    {
                        string code = prefix + "0" + obj.COM_ID;
                        Tbl_Master_CompanyDetails _Tbl_Master_CompanyDetails = context.Tbl_Master_CompanyDetails.Single(m => m.COM_ID == obj.COM_ID);
                        {
                            _Tbl_Master_CompanyDetails.COM_CODE = code;
                        };
                        if (context.SaveChanges() == 0)
                        {
                            success = false;
                        }
                    }
                    k= obj.COM_ID;
                    compcode = obj.COM_CODE;

                }
                else
                {
                    Tbl_Master_CompanyDetails ObjTbl_Master_CompanyDetails = context.Tbl_Master_CompanyDetails.Single(m => m.COM_ID == obj.COM_ID);
                    {
                        ObjTbl_Master_CompanyDetails.COM_CODE = obj.COM_CODE;
                        ObjTbl_Master_CompanyDetails.COM_KEY = obj.COM_KEY;
                        ObjTbl_Master_CompanyDetails.COM_NAME = obj.COM_NAME;
                        ObjTbl_Master_CompanyDetails.COM_USTID = obj.COM_USTID;
                        ObjTbl_Master_CompanyDetails.COM_DISPLAYNAME = obj.COM_DISPLAYNAME;
                        ObjTbl_Master_CompanyDetails.Tax_No = obj.Tax_No;
                        ObjTbl_Master_CompanyDetails.COM_PHONE = obj.COM_PHONE;
                        ObjTbl_Master_CompanyDetails.COM_MOBILE = obj.COM_MOBILE;
                        ObjTbl_Master_CompanyDetails.COM_FAX = obj.COM_FAX;
                        ObjTbl_Master_CompanyDetails.COM_ZIP = obj.COM_ZIP;
                        ObjTbl_Master_CompanyDetails.Cust_Supp = obj.Cust_Supp;
                        ObjTbl_Master_CompanyDetails.COM_EMAIL = obj.COM_EMAIL;
                        ObjTbl_Master_CompanyDetails.COM_REMARKS = obj.COM_REMARKS;
                        ObjTbl_Master_CompanyDetails.DELETED = obj.DELETED;
                        ObjTbl_Master_CompanyDetails.IPADDRESS = obj.IPADDRESS;
                        ObjTbl_Master_CompanyDetails.COM_Sales_Person = obj.COM_Sales_Person;
                        ObjTbl_Master_CompanyDetails.COM_CITY = obj.COM_CITY;
                        ObjTbl_Master_CompanyDetails.COM_STATE = obj.COM_STATE;
                        ObjTbl_Master_CompanyDetails.COM_COUNTRY = obj.COM_COUNTRY;
                        ObjTbl_Master_CompanyDetails.COM_CreditDays = obj.COM_CreditDays;
                        ObjTbl_Master_CompanyDetails.LAST_UPDATED_DATE = DateTime.Now;
                        ObjTbl_Master_CompanyDetails.LAST_UPDATED_BY = obj.LAST_UPDATED_BY;
                    };
                    if (context.SaveChanges() == 0)
                    {
                        success = false;
                    }
                    k = ObjTbl_Master_CompanyDetails.COM_ID;
                    compcode = ObjTbl_Master_CompanyDetails.COM_CODE;
                    // obj.COM_ID = k;
                }

                // Delete previous contact data
                Tbl_Master_CompanyContacts objdeletecontact = new Tbl_Master_CompanyContacts();
                context.Tbl_Master_CompanyContacts.RemoveRange(context.Tbl_Master_CompanyContacts.Where(m => m.COM_ID == obj.COM_ID));
                dbcontext.SaveChanges();

                // Delete previous bank data
                Tbl__Master_CompanyBank objdeletebank = new Tbl__Master_CompanyBank();
                context.Tbl__Master_CompanyBank.RemoveRange(context.Tbl__Master_CompanyBank.Where(m => m.B_PID == obj.COM_ID));
                dbcontext.SaveChanges();

                // Insert new contacts data
                string[] ChildRow = Contactdata.Split('|');
                for (int i = 0; i < ChildRow.Length - 1; i++)
                {
                    string[] ChildRecord = ChildRow[i].Split('}');
                    Tbl_Master_CompanyContacts objcontact = new Tbl_Master_CompanyContacts()
                    {
                        COM_ID = obj.COM_ID,
                        DELETED = Convert.ToBoolean(0),
                        TITLE = ChildRecord[0].ToString(),
                        FIRST_NAME = ChildRecord[1].ToString(),
                        LAST_NAME = ChildRecord[2].ToString(),
                        JOB_TITLE = ChildRecord[3].ToString(),
                        PHONE = ChildRecord[4].ToString(),
                        FAX = ChildRecord[5].ToString(),
                        MOBILE = ChildRecord[6].ToString(),
                        EMAIL = ChildRecord[7].ToString(),
                        REMARKS = ChildRecord[8].ToString(),
                        CREATED_BY = obj.CREATED_BY,
                        CREATED_DATE = DateTime.Now,
                        LAST_UPDATED_BY = obj.LAST_UPDATED_BY,
                        LAST_UPDATED_DATE = DateTime.Now,
                    };
                    context.Tbl_Master_CompanyContacts.Add(objcontact);
                    if (context.SaveChanges() == 0)
                    {
                        success = false;
                    }
                    k = (int)objcontact.COM_ID;
                }
                // Insert new bank data
                string[] ChildRow1 = Bankdata.Split('|');
                for (int i = 0; i < ChildRow1.Length - 1; i++)
                {
                    string[] ChildRecord1 = ChildRow1[i].Split('}');
                    if(ChildRecord1[1].ToString()!="")
                    {
                        Tbl__Master_CompanyBank objbank = new Tbl__Master_CompanyBank()
                        {
                            B_PID = obj.COM_ID,
                            DELETED = Convert.ToBoolean(0),
                            B_NAME = ChildRecord1[0].ToString(),
                            Address = ChildRecord1[1].ToString(),
                            B_AccountNo = ChildRecord1[2].ToString(),
                            SWIFT = ChildRecord1[3].ToString(),
                            BLZ = ChildRecord1[4].ToString(),
                            BIC = ChildRecord1[5].ToString(),
                            IBAN = ChildRecord1[6].ToString(),
                            CREATED_BY = obj.CREATED_BY,
                            CREATED_DATE = DateTime.Now,
                            LAST_UPDATED_BY = obj.LAST_UPDATED_BY,
                            LAST_UPDATED_DATE = DateTime.Now,
                        };
                        context.Tbl__Master_CompanyBank.Add(objbank);
                        if (context.SaveChanges() == 0)
                        {
                            success = false;
                        }
                        k = (int)objbank.B_PID;
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
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                k = 0;
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}", validationErrors.Entry.Entity.ToString(), validationError.ErrorMessage);
                        //raise a new exception inserting the current one as the InnerException
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                throw raise;
            }
            catch (Exception ex)
            {
                k = 0;
                throw ex;
            }
            finally
            {
                transaction.Dispose();
                context.Dispose();
            }
            return k;
        }

        public int ET_Master_Company_Deleted_DL(int id)
        {
            int i;
            try
            {
                var find = dbcontext.Tbl_Master_CompanyDetails.Where(m => m.COM_ID == id).ToList();
                if (find.Count() != 0)
                {
                    Tbl_Master_CompanyDetails delete = dbcontext.Tbl_Master_CompanyDetails.Single(m => m.COM_ID == id);
                    delete.DELETED = true;
                    delete.DELETED_BY = 1;
                    delete.DELETED_DATE = DateTime.Now;
                    //dbcontext.Tbl_Master_Role.Add(isdelete);
                };
                i = dbcontext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return i;
        }

        public List<location> Binddropdown_Country_DL()
        {
            try
            {
                var Country = dbcontext.locations.Where(m => m.location_type == 0).ToList();
                return Country;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Tbl_Master_CompanyDetails> ET_Master_Company_Restore_DL()
        {
            try
            {
                var data = dbcontext.Tbl_Master_CompanyDetails.Where(m => m.DELETED == true).ToList();
                return data;
            }
            catch (Exception ex)
            { throw ex; }
        }

        public int ET_Master_Company_Restore_Insert_DL(int id, bool type)
        {
            int result;
            try
            {
                Tbl_Master_CompanyDetails obj = dbcontext.Tbl_Master_CompanyDetails.Single(m => m.COM_ID == id );
                {
                    obj.CREATED_DATE = DateTime.Now;
                    obj.DELETED = type;
                    obj.LAST_UPDATED_DATE = DateTime.Now;
                    obj.LAST_UPDATED_BY = 1;
                };
                result = dbcontext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
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

        public List<Tbl_Master_CompanyDetails> ET_Master_Company_View_DL(int id)
        {
            try
            {
                return dbcontext.Tbl_Master_CompanyDetails.Where(m => m.COM_ID == id).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Tbl_Master_CompanyDetails ET_Master_Company_Update_GetbyID(int id)
        {
            dbcontext.Configuration.ProxyCreationEnabled = false;
            try
            {
                //  dbcontext.Configuration.ProxyCreationEnabled = false;
                return dbcontext.Tbl_Master_CompanyDetails.Single(m => m.COM_ID == id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Tbl_Master_CompanyContacts> ET_Master_Company_Contactsedit_BL(int id)
        {
            try
            {
                var data = dbcontext.Tbl_Master_CompanyContacts.Where(m => m.DELETED == false && m.COM_ID == id).ToList();
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<Tbl_Master_CompanyContacts> ET_Master_Company_Contacts_DL(int id)
        {
            try
            {
                var data = dbcontext.Tbl_Master_CompanyContacts.Where(m => m.DELETED == false && m.COM_ID == id).ToList();
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Tbl__Master_CompanyBank> ET_Master_Company_Bank_DL(int id)
        {
            try
            {
                var data = dbcontext.Tbl__Master_CompanyBank.Where(m => m.DELETED == false && m.B_PID == id).ToList();
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public decimal ET_Admin_Contact_Add_DL(Tbl_Master_CompanyContacts obj)
        {
            try
            {
                if (obj.CONTACT_ID == 0)
                {
                    dbcontext.Tbl_Master_CompanyContacts.Add(obj);
                    dbcontext.SaveChanges();
                }
                else
                {
                    Tbl_Master_CompanyContacts ObjTbl_Master_CompanyContacts = dbcontext.Tbl_Master_CompanyContacts.Single(m => m.COM_ID == obj.COM_ID);
                    {
                        ObjTbl_Master_CompanyContacts.CREATED_BY = obj.CREATED_BY;
                        ObjTbl_Master_CompanyContacts.CREATED_DATE = DateTime.Now;
                        ObjTbl_Master_CompanyContacts.DELETED = false;
                        ObjTbl_Master_CompanyContacts.EMAIL = obj.EMAIL;
                        ObjTbl_Master_CompanyContacts.FAX = obj.FAX;
                        ObjTbl_Master_CompanyContacts.FIRST_NAME = obj.FIRST_NAME;
                        ObjTbl_Master_CompanyContacts.LAST_NAME = obj.LAST_NAME;
                        ObjTbl_Master_CompanyContacts.TITLE = obj.TITLE;
                        ObjTbl_Master_CompanyContacts.PHONE = obj.PHONE;
                        ObjTbl_Master_CompanyContacts.JOB_TITLE = obj.JOB_TITLE;
                        ObjTbl_Master_CompanyContacts.MOBILE = obj.MOBILE;
                        ObjTbl_Master_CompanyContacts.REMARKS = obj.REMARKS;
                        ObjTbl_Master_CompanyContacts.IPADDRESS = obj.IPADDRESS;
                        ObjTbl_Master_CompanyContacts.LAST_UPDATED_BY = obj.LAST_UPDATED_BY;
                        ObjTbl_Master_CompanyContacts.LAST_UPDATED_DATE = DateTime.Now;
                    };
                    int i = dbcontext.SaveChanges();
                    obj.COM_ID = i;
                }
            }
            catch (Exception ex)
            { throw ex; }

            return obj.COM_ID;
        }

        public List<Tbl_BankMaster> Bind_dropdown_Bankname_DL(int com_key)
        {
            try
            {
                var data = dbcontext.Tbl_BankMaster.Where(m => m.DELETED == false && m.COM_KEY == com_key).ToList();
                return data;
            }
            catch (Exception exe)
            {
                throw exe;
            }
        }

            public string CheckDuplicateCode_DL(int ComID, string CompanyCode)
        {
            if (ComID == 0)
            {
                var count = dbcontext.Tbl_Master_CompanyDetails.Where(m => m.COM_CODE == CompanyCode).ToList().Count();
                if (count > 0)
                {
                    return "exist";
                }
            }
            else
            {
                var count = dbcontext.Tbl_Master_CompanyDetails.Where(m => m.COM_ID != ComID && m.COM_CODE == CompanyCode).ToList().Count();
                if (count > 0)
                {
                    return "exist";
                }
            }
            return "";
        }
        public string CheckDuplicateCompanyName_DL(int ComID, string CompanyName)
        {
            if (ComID == 0)
            {
                var count = dbcontext.Tbl_Master_CompanyDetails.Where(m => m.COM_NAME == CompanyName).ToList().Count();
                if (count > 0)
                {
                    return "exist";
                }
            }
            else
            {
                var count = dbcontext.Tbl_Master_CompanyDetails.Where(m => m.COM_ID != ComID && m.COM_NAME == CompanyName).ToList().Count();
                if (count > 0)
                {
                    return "exist";
                }
            }
            return "";
        }
        public string CheckDuplicateDisplayName_DL(int ComID, string DisplayName)
        {
            if (ComID == 0)
            {
                var count = dbcontext.Tbl_Master_CompanyDetails.Where(m => m.COM_DISPLAYNAME == DisplayName).ToList().Count();
                if (count > 0)
                {
                    return "exist";
                }
            }
            else
            {
                var count = dbcontext.Tbl_Master_CompanyDetails.Where(m => m.COM_ID != ComID && m.COM_DISPLAYNAME == DisplayName).ToList().Count();
                if (count > 0)
                {
                    return "exist";
                }
            }
            return "";
        }

        public string CheckDuplicateEmail_DL(int ComID, string Email)
        {
            if (ComID == 0)
            {
                var count = dbcontext.Tbl_Master_CompanyDetails.Where(m => m.COM_EMAIL == Email).ToList().Count();
                if (count > 0)
                {
                    return "exist";
                }
            }
            else
            {
                var count = dbcontext.Tbl_Master_CompanyDetails.Where(m => m.COM_ID != ComID && m.COM_EMAIL == Email).ToList().Count();
                if (count > 0)
                {
                    return "exist";
                }
            }
            return "";
        }
    }
}
