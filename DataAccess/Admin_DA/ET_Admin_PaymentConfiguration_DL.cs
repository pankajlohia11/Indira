using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessEntity.EntityModels;
using BusinessEntity.CustomModels;

namespace DataAccess.Admin_DA
{
    public class ET_Admin_PaymentConfiguration_DL
    {
        EntityClasses dbcontext = new EntityClasses();

        public List<Tbl_Payment_Config> ET_Admin_PaymentConfigurationlist_DL(int com_key)
        {
            try
            {
                var data = dbcontext.Tbl_Payment_Config.Where(m => m.Deleted == false && m.COM_KEY== com_key).ToList();
                return data;
            }
            catch (Exception exe)
            {
                throw exe;
            }
        }

        public decimal ET_Admin_PaymentConfiguration_Add_DL(Tbl_Payment_Config obj, string prefix, bool automanual)
        {
            Tbl_Payment_Config objgl = new Tbl_Payment_Config();
            try
            {
                if (obj.Payment_Config_Id == 0)
                {
                    Tbl_Payment_Config objtpc = new Tbl_Payment_Config()
                    {
                        Payment_Config_Code = obj.Payment_Config_Code,
                        Payment_From = obj.Payment_From,
                        Payment_To = obj.Payment_To,
                        Payment_Discount_Type = obj.Payment_Discount_Type,
                        Payment_Discount_Amount = obj.Payment_Discount_Amount,
                        Payment_Discount_Percentage = obj.Payment_Discount_Percentage,
                        IPAddress = obj.IPAddress,
                        Deleted = false,
                        Created_By = obj.Created_By,
                        Created_Date = DateTime.Now,
                        Last_Updated_By = obj.Last_Updated_By,
                        Last_Updated_Date = DateTime.Now,
                        COM_KEY=obj.COM_KEY
                    };
                    dbcontext.Tbl_Payment_Config.Add(objtpc);
                    dbcontext.SaveChanges();

                    if (automanual == true)
                    {
                        string code = prefix + "0" + objtpc.Payment_Config_Id;
                        Tbl_Payment_Config Tbl_Payment_Config = dbcontext.Tbl_Payment_Config.Single(m => m.Payment_Config_Id == objtpc.Payment_Config_Id);
                        {
                            Tbl_Payment_Config.Payment_Config_Code = code;
                        };
                        dbcontext.SaveChanges();
                        objgl.Payment_Config_Id = objtpc.Payment_Config_Id;
                    }
                }
                else
                {
                    Tbl_Payment_Config objtpc = dbcontext.Tbl_Payment_Config.Single(m => m.Payment_Config_Id == obj.Payment_Config_Id);
                    {
                        objtpc.Payment_Config_Code = obj.Payment_Config_Code;
                        objtpc.Payment_From = obj.Payment_From;
                        objtpc.Payment_To = obj.Payment_To;
                        objtpc.Payment_Discount_Type = obj.Payment_Discount_Type;
                        objtpc.Payment_Discount_Amount = obj.Payment_Discount_Amount;
                        objtpc.Payment_Discount_Percentage = obj.Payment_Discount_Percentage;
                        objtpc.IPAddress = obj.IPAddress;
                        objtpc.Last_Updated_By = obj.Last_Updated_By;
                        objtpc.Last_Updated_Date = DateTime.Now;
                    };
                    objgl.Payment_Config_Id = dbcontext.SaveChanges();
                }
            }
            catch (Exception exe)
            {
                throw exe;
            }
            return objgl.Payment_Config_Id;
        }

        public string CheckDuplicateCode_DL(int PaymentID, string PaymentCode)
        {
            try
            {
                if (PaymentID == 0)
                {
                    var count = dbcontext.Tbl_Payment_Config.Where(m => m.Payment_Config_Code == PaymentCode).Count();
                    if (count > 0)
                    {
                        return "Exist";
                    }
                }
                else
                {
                    var count = dbcontext.Tbl_Payment_Config.Where(m => m.Payment_Config_Id != PaymentID && m.Payment_Config_Code == PaymentCode).Count();
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


        public Tbl_Payment_Config ET_Admin_PaymentConfig_Update_GetbyID_DL(int id)
        {
            try
            {
                return dbcontext.Tbl_Payment_Config.Single(m => m.Payment_Config_Id == id);
            }
            catch (Exception exe)
            {
                throw exe;
            }
        }
        public int ET_Admin_PaymentConfig_Delete_DL(int id, decimal deleteby)
        {
            try
            {
                Tbl_Payment_Config obj = dbcontext.Tbl_Payment_Config.Single(m => m.Payment_Config_Id == id && m.Deleted == false);
                {
                    obj.Deleted = true;
                    obj.Deleted_date = DateTime.Now;
                    obj.Deleted_By = deleteby;
                }
                int i = dbcontext.SaveChanges();
                return i;
            }
            catch (Exception exe)
            {
                throw exe;
            }
        }
        public List<Tbl_Payment_Config> ET_Admin_PaymentConfig_Restore_DL()
        {
            try
            {
                var data = dbcontext.Tbl_Payment_Config.Where(m => m.Deleted == true).ToList();
                return data;
            }
            catch (Exception exe)
            {
                throw exe;
            }
        }
        public int ET_Admin_PaymentConfig_Restore_Insert_DL(int id, string Updatedby)
        {
            try
            {
                Tbl_Payment_Config obj = dbcontext.Tbl_Payment_Config.Single(m => m.Payment_Config_Id == id && m.Deleted == true);
                {
                    obj.Deleted = false;
                    obj.Deleted_date = DateTime.Now;
                    obj.Last_Updated_By = Updatedby;
                }
                int i = dbcontext.SaveChanges();
                return i;
            }
            catch (Exception exe)
            {
                throw exe;
            }
        }
        public Tbl_Payment_Config ET_Admin_PaymentConfig_View_DL(int id)
        {
            try
            {
                return dbcontext.Tbl_Payment_Config.Single(m => m.Payment_Config_Id == id);
            }
            catch (Exception exe)
            {
                throw exe;
            }
        }
    }
}
