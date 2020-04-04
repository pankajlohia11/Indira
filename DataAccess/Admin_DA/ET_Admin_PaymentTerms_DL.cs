using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Admin_DA;
using BusinessEntity.EntityModels;

namespace DataAccess.Admin_DA
{
    public class ET_Admin_PaymentTerms_DL
    {
        EntityClasses dbcontext = new EntityClasses();
        public List<Tbl_Payment_Terms> GetPayment_Terms_List(bool deleted)
        {
            try
            {
                var data = dbcontext.Tbl_Payment_Terms.Where(m => m.PT_ID != 0 && m.DELETED == deleted).ToList();
                return data;
            }
            catch (Exception exe)
            {
                throw exe;
            }
        }
        public decimal ET_Admin_PaymentTerms_Add_DL(Tbl_Payment_Terms obj, string prefix, bool automanual,out string PayCode)
        {
            Tbl_Payment_Terms gltpt = new Tbl_Payment_Terms();
            try
            {
                if (obj.PT_ID == 0)
                {
                    Tbl_Payment_Terms objtpt = new Tbl_Payment_Terms()
                    {
                        PT_ID = obj.PT_ID,
                        PT_Code = obj.PT_Code,
                        PT_Name = obj.PT_Name,
                        PT_DiscountType = obj.PT_DiscountType,
                        PT_AdvanceType = obj.PT_AdvanceType,
                        PT_Details = obj.PT_Details,
                        DELETED = false,
                        CREATED_DATE = DateTime.Now,
                        CREATED_BY = obj.CREATED_BY,
                        LAST_UPDATED_DATE = DateTime.Now,
                        LAST_UPDATED_BY = obj.LAST_UPDATED_BY,
                        COM_KEY = obj.COM_KEY
                    };
                    dbcontext.Tbl_Payment_Terms.Add(objtpt);
                    dbcontext.SaveChanges();

                    if (automanual == true)
                    {
                        string code = prefix + "0" + objtpt.PT_ID;
                        Tbl_Payment_Terms Tbl_Payment_Terms = dbcontext.Tbl_Payment_Terms.Single(m => m.PT_ID == objtpt.PT_ID);
                        {
                            Tbl_Payment_Terms.PT_Code = code;
                        };
                        dbcontext.SaveChanges();
                    }
                    gltpt.PT_ID = objtpt.PT_ID;
                    PayCode= objtpt.PT_Code;
                }
                else
                {
                    Tbl_Payment_Terms objtpt = dbcontext.Tbl_Payment_Terms.Single(m => m.PT_ID == obj.PT_ID);
                    {
                        objtpt.PT_ID = obj.PT_ID;
                        objtpt.PT_Code = obj.PT_Code;
                        objtpt.PT_Name = obj.PT_Name;
                        objtpt.PT_DiscountType = obj.PT_DiscountType;
                        objtpt.PT_AdvanceType = obj.PT_AdvanceType;
                        objtpt.PT_Details = obj.PT_Details;
                        objtpt.DELETED = false;
                        objtpt.CREATED_DATE = DateTime.Now;
                        objtpt.CREATED_BY = obj.CREATED_BY;
                        objtpt.LAST_UPDATED_DATE = DateTime.Now;
                        objtpt.LAST_UPDATED_BY = obj.LAST_UPDATED_BY;
                    };
                    dbcontext.SaveChanges();
                    gltpt.PT_ID = objtpt.PT_ID;
                    PayCode = objtpt.PT_Code;
                }
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                        // raise a new exception nesting  
                        // the current instance as InnerException  
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                throw raise;
            }
            catch (Exception exe)
            {
                throw exe;
            }
            return gltpt.PT_ID;
        }

        public Tbl_Payment_Terms ET_Admin_PaymentTerms_Update_GetbyID_DL(int id)
        {
            try
            {
                return dbcontext.Tbl_Payment_Terms.Single(m => m.PT_ID == id);
            }
            catch (Exception exe)
            {
                throw exe;
            }
        }
        public int ET_Admin_PaymentTerms_Restore_Delete_DL(int id, decimal Updatedby, bool type)
        {
            int result = 0;
            try
            {
                Tbl_Payment_Terms obj = dbcontext.Tbl_Payment_Terms.Single(m => m.PT_ID == id);
                {
                    obj.DELETED = type;
                    obj.LAST_UPDATED_DATE = DateTime.Now;
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

        public Tbl_Payment_Terms ET_Admin_PaymentTerms_View_DL(int id)
        {
            try
            {
                return dbcontext.Tbl_Payment_Terms.Single(m => m.PT_ID == id);
            }
            catch (Exception exe)
            {
                throw exe;
            }
        }

        public string CheckDuplicateCode_DL(decimal id, string txtpamentCode)
        {
            try
            {
                if (id == 0)
                {
                    var count = dbcontext.Tbl_Payment_Terms.Where(m => m.PT_ID == id).Count();
                    if (count > 0)
                    {
                        return "Exist";
                    }
                }
                else
                {
                    var count = dbcontext.Tbl_Payment_Terms.Where(m => m.PT_ID != id && m.PT_Code == txtpamentCode).Count();
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
