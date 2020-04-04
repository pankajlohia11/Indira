using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessEntity.CustomModels;
using BusinessEntity.EntityModels;
using DataAccess.Admin_DA;
using DataAccess;


namespace DataAccess.Admin_DA
{
    public class ET_Admin_LC_Details_DL
    {
        EntityClasses dbcontext = new EntityClasses();

        public List<ET_TBL_LC_DETAILS_CM> ET_Admin_LC_Detailslist_DL(bool deleted)
        {
            try
            {
                // var data = dbcontext.ET_TBL_LC_DETAILS.Where(m => m.DELETED == deleted).ToList();
            var data = (from Company in dbcontext.Tbl_Master_CompanyDetails
                            join lcd in dbcontext.ET_TBL_LC_DETAILS on Company.COM_ID equals lcd.CUST_ID
                            where lcd.DELETED == deleted
                            select new ET_TBL_LC_DETAILS_CM
                              {
                                LC_NO = lcd.LC_NO,
                                LC_ID = lcd.LC_ID,
                                COM_NAME = Company.COM_NAME,
                                LC_DATE = lcd.LC_DATE,
                                LC_EXPIRYDATE = lcd.LC_EXPIRYDATE
                            }).ToList();
                return data;
            }
            catch (Exception exe)
            {
                throw exe;
            }
        }

        public decimal ET_Admin_LC_Details_Add_DL(ET_TBL_LC_DETAILS obj, string prefix, bool automanual)
        {
            ET_TBL_LC_DETAILS objlcgl = new ET_TBL_LC_DETAILS();
            try
            {
                if (obj.LC_ID == 0)
                {
                    var bankid = dbcontext.Tbl_Master_CompanyDetails.Where(m => m.COM_ID == obj.CUST_ID).ToList().FirstOrDefault().BANK_ID;

                    var bankname = "";
                    if (bankid != null)
                        bankname = dbcontext.Tbl_BankMaster.Single(m => m.BANK_ID == bankid).BANK_NAME;

                    ET_TBL_LC_DETAILS objlc = new ET_TBL_LC_DETAILS()
                    {
                        LC_CODE = obj.LC_CODE,
                        COM_KEY = obj.COM_KEY,
                        LC_ID = obj.LC_ID,
                        CUST_ID = obj.CUST_ID,
                        CUST_SUPP = obj.CUST_SUPP,
                        CURRENCY_ID = obj.CURRENCY_ID,
                        LC_NO = obj.LC_NO,
                        LC_DATE = obj.LC_DATE,
                        LC_EXPIRYDATE = obj.LC_EXPIRYDATE,
                        LC_AMOUNT = obj.LC_AMOUNT,
                        LC_BAL_AMT = obj.LC_BAL_AMT,
                        LC_CLOSE_AMT = obj.LC_CLOSE_AMT,
                        LC_BANK_NAME = bankname,
                        LC_BANK_ID = bankid,
                        DELETED = false,
                        CREATED_DATE = DateTime.Now,
                        CREATED_BY = obj.CREATED_BY
                    };
                    dbcontext.ET_TBL_LC_DETAILS.Add(objlc);
                    dbcontext.SaveChanges();

                    if (automanual == true)
                    {
                        string code = prefix +"0" + objlc.LC_ID;
                        ET_TBL_LC_DETAILS ET_TBL_LC_DETAILS = dbcontext.ET_TBL_LC_DETAILS.Single(m => m.LC_ID == objlc.LC_ID);
                        {
                            ET_TBL_LC_DETAILS.LC_CODE = code;
                        };
                        dbcontext.SaveChanges();
                    }
                    objlcgl.LC_ID = objlc.LC_ID;
                }
                else
                {
                    ET_TBL_LC_DETAILS objlc = dbcontext.ET_TBL_LC_DETAILS.Single(m => m.LC_ID == obj.LC_ID);
                    {
                        var bankid = dbcontext.Tbl_Master_CompanyDetails.Where(m => m.COM_ID == obj.CUST_ID).ToList().FirstOrDefault().BANK_ID;

                        var bankname = "";
                        if (bankid != null)
                            bankname = dbcontext.Tbl_BankMaster.Single(m => m.BANK_ID == bankid).BANK_NAME;
                        objlc.LC_CODE = obj.LC_CODE;
                        objlc.LC_ID = obj.LC_ID;
                        objlc.CUST_ID = obj.CUST_ID;
                        objlc.CUST_SUPP = obj.CUST_SUPP;
                        objlc.CURRENCY_ID = obj.CURRENCY_ID;
                        objlc.LC_NO = obj.LC_NO;
                        objlc.LC_DATE = obj.LC_DATE;
                        objlc.LC_EXPIRYDATE = obj.LC_EXPIRYDATE;
                        objlc.LC_AMOUNT = obj.LC_AMOUNT;
                        objlc.LC_BAL_AMT = obj.LC_BAL_AMT;
                        objlc.LC_CLOSE_AMT = obj.LC_CLOSE_AMT;
                        objlc.LC_BANK_NAME = bankname;
                        objlc.LC_BANK_ID = bankid;
                        objlc.LAST_UPDATED_BY = obj.LAST_UPDATED_BY;
                        objlc.LAST_UPDATED_DATE = DateTime.Now;
                    };
                    objlcgl.LC_ID = objlc.LC_ID;
                    dbcontext.SaveChanges();
                }
            }
            catch (Exception exe)
            {
                throw exe;
            }
            return objlcgl.LC_ID;
        }

        public ET_TBL_LC_DETAILS ET_Admin_LC_Details_Update_GetbyID_DL(int id)
        {
            try
            {
                return dbcontext.ET_TBL_LC_DETAILS.Single(m => m.LC_ID == id);
            }
            catch (Exception exe)
            {
                throw exe;
            }
        }
        public int ET_Admin_LC_Details_Restore_Insert_DL(int id, decimal Updatedby, bool type)
        {
            int result = 0;
            try
            {
                ET_TBL_LC_DETAILS obj = dbcontext.ET_TBL_LC_DETAILS.Single(m => m.LC_ID == id);
                {
                    obj.DELETED = type;
                    obj.DELETED_DATE = DateTime.Now;
                    obj.LAST_UPDATED_BY = Updatedby;
                }
                result = dbcontext.SaveChanges();                
            }
            catch (Exception exe)
            {
                throw exe;
            }
            return result;
        }
        public ET_TBL_LC_DETAILS ET_Admin_LC_Details_View_DL(int id)
        {
            try
            {
                return dbcontext.ET_TBL_LC_DETAILS.Single(m => m.LC_ID == id);
            }
            catch (Exception exe)
            {
                throw exe;
            }
        }      
        public List<Tbl_BankMaster> Bind_dropdown_Bankname_DL(int id)
        {
            dbcontext.Configuration.ProxyCreationEnabled = false;
            try
            {
                 var Bankname = dbcontext.Tbl_BankMaster.Where(m => m.BANK_ID == id).ToList();
                //var Bankname = (from bank in dbcontext.Tbl_BankMaster
                //                join tmc in dbcontext.Tbl_Master_CompanyDetails on bank.BANK_ID equals tmc.BANK_ID
                //                where tmc.Cust_Supp == id 
                //                select new
                //                {
                //                    tmc.BANK_ID,
                //                    bank.BANK_NAME,
                //                    tmc.COM_ID,
                //                    tmc.COM_NAME
                //                }).ToString();
                return Bankname;
            }
            catch (Exception exe)
            {
                throw exe;
            }
        }    
        public string CheckDuplicateCode_DL(decimal id, string LCCode)
        {
            try
            {
                if (id == 0)
                {
                    var count = dbcontext.ET_TBL_LC_DETAILS.Where(m => m.LC_CODE == LCCode).Count();
                    if (count > 0)
                    {
                        return "Exist";
                    }
                }
                else
                {
                    var count = dbcontext.ET_TBL_LC_DETAILS.Where(m => m.LC_ID != id && m.LC_CODE == LCCode).Count();
                    if (count > 0)
                    {
                        return "Exist";
                    }
                }
                return "";
            }
            catch (Exception exe)
            {
                throw exe;
            }
        }      
        public string CheckDuplicateLCNo_DL(decimal id, string LCNo)
        {
            try
            {
                if (id == 0)
                {
                    var count = dbcontext.ET_TBL_LC_DETAILS.Where(m => m.LC_NO == LCNo).Count();
                    if (count > 0)
                    {
                        return "Exist";
                    }
                }
                else
                {
                    var count = dbcontext.ET_TBL_LC_DETAILS.Where(m => m.LC_ID != id && m.LC_NO == LCNo).Count();
                    if (count > 0)
                    {
                        return "Exist";
                    }
                }
                return "";
            }
            catch (Exception exe)
            {
                throw exe;
            }
        }
        public string CheckDuplicateLCdate_DL(decimal id, string LCdate)
        {
            DateTime date = Convert.ToDateTime(LCdate);
            try
            {
                if (id == 0)
                {
                    var count = dbcontext.ET_TBL_LC_DETAILS.Where(m => m.LC_DATE == date).Count();
                    if (count > 0)
                    {
                        return "Exist";
                    }
                }
                else
                {
                    var count = dbcontext.ET_TBL_LC_DETAILS.Where(m => m.LC_ID != id && m.LC_DATE == date).Count();
                    if (count > 0)
                    {
                        return "Exist";
                    }
                }
                return "";
            }
            catch (Exception exe)
            {
                throw exe;
            }
        }

    }
}
