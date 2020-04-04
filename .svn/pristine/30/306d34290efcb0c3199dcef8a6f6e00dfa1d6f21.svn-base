using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessEntity.EntityModels;

namespace DataAccess.Admin_DA
{
    public class Admin_Bank_DL
    {
        EntityClasses dbcontext = new EntityClasses();

        public List<Tbl_BankMaster> ET_Admin_Bank_DL(int com_key)
        {
            try
            {
                var data = dbcontext.Tbl_BankMaster.Where(m => m.DELETED == false && m.COM_KEY== com_key).ToList();
                return data;
            }
            catch (Exception exe)
            {
                throw exe;
            }
        }

        public decimal ET_Admin_Bank_Add_DL(Tbl_BankMaster obj,string prefix, bool automanual,out string bankcode)
        {
            Tbl_BankMaster objgl = new Tbl_BankMaster();
            try
            {
                if (obj.BANK_ID == 0)
                {
                    Tbl_BankMaster objtmb = new Tbl_BankMaster()
                    {
                        BANK_CODE = obj.BANK_CODE,
                        BANK_ID = obj.BANK_ID,
                        BANK_NAME = obj.BANK_NAME,
                        DISPLAY_NAME = obj.DISPLAY_NAME,
                        STREET = obj.STREET,
                        STATE = obj.STATE,
                        CITY = obj.CITY,
                        COUNTRY = obj.COUNTRY,
                        ZIP = obj.ZIP,
                        BLZ = obj.BLZ,
                        BIC = obj.BIC,
                        CONTACT_NAME = obj.CONTACT_NAME,
                        CONTACT_NO = obj.CONTACT_NO,
                        IBAN = obj.IBAN,
                        SWIFT = obj.SWIFT,
                        REMARKS = obj.REMARKS,
                        DELETED = false,
                        ACCOUNT = obj.ACCOUNT,
                        CREATED_BY = obj.CREATED_BY,
                        CREATED_DATE = DateTime.Now,
                        COM_KEY=obj.COM_KEY
                    };
                    dbcontext.Tbl_BankMaster.Add(objtmb);
                    dbcontext.SaveChanges();

                    if (automanual == true)
                    {
                        string code = prefix +"0"+ objtmb.BANK_ID;
                        Tbl_BankMaster objTbl_BankMaster = dbcontext.Tbl_BankMaster.Single(m => m.BANK_ID == objtmb.BANK_ID);
                        {
                            objTbl_BankMaster.BANK_CODE = code;
                        };
                        dbcontext.SaveChanges();
                    }

                    objgl.BANK_ID = objtmb.BANK_ID;
                    bankcode= objtmb.BANK_CODE;
                }
                else
                {
                    Tbl_BankMaster objtmb = dbcontext.Tbl_BankMaster.Single(m => m.BANK_ID == obj.BANK_ID);
                    {
                        objtmb.BANK_CODE = obj.BANK_CODE;
                        objtmb.BANK_ID = obj.BANK_ID;
                        objtmb.BANK_NAME = obj.BANK_NAME;
                        objtmb.DISPLAY_NAME = obj.DISPLAY_NAME;
                        objtmb.STREET = obj.STREET;
                        objtmb.CITY = obj.CITY;
                        objtmb.COUNTRY = obj.COUNTRY;
                        objtmb.ZIP = obj.ZIP;
                        objtmb.BLZ = obj.BLZ;
                        objtmb.BIC = obj.BIC;
                        objtmb.CONTACT_NAME = obj.CONTACT_NAME;
                        objtmb.CONTACT_NO = obj.CONTACT_NO;
                        objtmb.IBAN = obj.IBAN;
                        objtmb.SWIFT = obj.SWIFT;
                        objtmb.REMARKS = obj.REMARKS;
                        objtmb.DELETED = false;
                        objtmb.ACCOUNT = obj.ACCOUNT;
                        objtmb.LAST_UPDATED_BY = obj.LAST_UPDATED_BY;
                        objtmb.LAST_UPDATED_DATE = DateTime.Now;
                    };
                    dbcontext.SaveChanges();
                    objgl.BANK_ID = objtmb.BANK_ID;
                    bankcode = objtmb.BANK_CODE;
                }
            }
            catch (Exception exe)
            {
                throw exe;
            }
            return objgl.BANK_ID;
        }

        public int ET_Admin_Roles_Deleted_DL(int id, decimal deleteby)
        {
            int i = 0;
            try
            {
                Tbl_BankMaster obj = dbcontext.Tbl_BankMaster.Single(m => m.BANK_ID == id && m.DELETED == false);
                {
                    obj.DELETED = true;
                    obj.DELETED_DATE = DateTime.Now;
                    obj.DELETED_BY = deleteby;
                };
                i = dbcontext.SaveChanges();
            }
            catch (Exception exe)
            { throw exe; }
            return i;
        }

        public int ET_Admin_Bank_Restore_Insert_DL(int id, decimal deleteby)
        {
            int i = 0;
            try
            {
                Tbl_BankMaster obj = dbcontext.Tbl_BankMaster.Single(m => m.BANK_ID == id && m.DELETED == true);
                {
                    obj.DELETED = false;
                    obj.DELETED_DATE = DateTime.Now;
                    obj.DELETED_BY = deleteby;
                };
                i = dbcontext.SaveChanges();
            }
            catch (Exception exe)
            {
                throw exe;
            }
            return i;
        }

        public List<Tbl_BankMaster> ET_Admin_Bank_Restore_DL()
        {
            try
            {
                var data = dbcontext.Tbl_BankMaster.Where(m => m.DELETED == true).ToList();
                return data;
            }
            catch (Exception exe)
            {
                throw exe;
            }
        }

        public Tbl_BankMaster ET_Admin_Bank_View_DL(int id)
        {
            try
            {

                var data = dbcontext.Tbl_BankMaster.Single(m => m.BANK_ID == id);
                return data;
            }
            catch (Exception exe)
            {
                throw exe;
            }
        }

        public Tbl_BankMaster ET_Admin_Bank_Update_GetbyID_DL(int id)
        {
            try
            {
                return dbcontext.Tbl_BankMaster.Single(m => m.BANK_ID == id);
            }
            catch (Exception exe)
            {
                throw exe;
            }
        }

        public string CheckDuplicateCode_DL(int id, string Bankcode)
        {
            try
            {
                if (id == 0)
                {
                    var Count = dbcontext.Tbl_BankMaster.Where(m => m.BANK_CODE == Bankcode).ToList().Count();
                    if (Count > 0)
                    {
                        return "Exist";
                    }
                }
                else
                {
                    var Count = dbcontext.Tbl_BankMaster.Where(m => m.BANK_ID != id && m.BANK_CODE == Bankcode).ToList().Count();
                    if (Count > 0)
                    {
                        return "Exist";
                    }
                }
            }
            catch (Exception exe)
            {
                throw exe;
            }
            return "";
        }


        public string CheckDuplicateBankName_DL(int id, string BankName)
        {
            try
            {
                if (id == 0)
                {
                    var Count = dbcontext.Tbl_BankMaster.Where(m => m.BANK_NAME == BankName).ToList().Count();
                    if (Count > 0)
                    {
                        return "Exist";
                    }
                }
                else
                {
                    var Count = dbcontext.Tbl_BankMaster.Where(m => m.BANK_ID != id && m.BANK_NAME == BankName).ToList().Count();
                    if (Count > 0)
                    {
                        return "Exist";
                    }
                }
            }
            catch (Exception exe)
            {
                throw exe;
            }
            return "";
        }

        public string CheckDuplicateDisplayName_DL(int id, string DisplayName)
        {
            try
            {
                if (id == 0)
                {
                    var Count = dbcontext.Tbl_BankMaster.Where(m => m.DISPLAY_NAME == DisplayName).ToList().Count();
                    if (Count > 0)
                    {
                        return "Exist";
                    }
                }
                else
                {
                    var Count = dbcontext.Tbl_BankMaster.Where(m => m.BANK_ID != id && m.DISPLAY_NAME == DisplayName).ToList().Count();
                    if (Count > 0)
                    {
                        return "Exist";
                    }
                }
            }
            catch (Exception exe)
            {
                throw exe;
            }
            return "";
        }

        public string CheckDuplicateAccountno_DL(int id, string AccountNo)
        {
            try
            {
                if (id == 0)
                {
                    var Count = dbcontext.Tbl_BankMaster.Where(m => m.ACCOUNT == AccountNo).ToList().Count();
                    if (Count > 0)
                    {
                        return "Exist";
                    }
                }
                else
                {
                    var Count = dbcontext.Tbl_BankMaster.Where(m => m.BANK_ID != id && m.ACCOUNT == AccountNo).ToList().Count();
                    if (Count > 0)
                    {
                        return "Exist";
                    }
                }
            }
            catch (Exception exe)
            {
                throw;
            }
            return "";
        }
    }
}
