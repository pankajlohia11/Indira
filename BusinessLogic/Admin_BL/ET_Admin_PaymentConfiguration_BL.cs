using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessEntity.CustomModels;
using BusinessEntity.EntityModels;
using DataAccess.Admin_DA;
using DataAccess;

namespace BusinessLogic.Admin_BL
{
    public class ET_Admin_PaymentConfiguration_BL
    {
        ET_Admin_PaymentConfiguration_DL ObjDL = new ET_Admin_PaymentConfiguration_DL();
        public List<Tbl_Payment_Config> ET_Admin_PaymentConfigurationlist_BL(int com_key)
        {
            return ObjDL.ET_Admin_PaymentConfigurationlist_DL(com_key);
        }
        public string CheckDuplicateCode_BL(int PaymentID, string PaymentCode)
        {
            return ObjDL.CheckDuplicateCode_DL(PaymentID, PaymentCode);
        }
        public decimal ET_Admin_PaymentConfiguration_Add_BL(Tbl_Payment_Config obj, string prefix, bool automanual)
        {
            return ObjDL.ET_Admin_PaymentConfiguration_Add_DL(obj, prefix, automanual);
        }

        public Tbl_Payment_Config ET_Admin_PaymentConfig_Update_GetbyID_BL(int id)
        {
            return ObjDL.ET_Admin_PaymentConfig_Update_GetbyID_DL(id);
        }
        public int ET_Admin_PaymentConfig_Delete_BL(int id, decimal deleteby)
        {
            return ObjDL.ET_Admin_PaymentConfig_Delete_DL(id, deleteby);
        }

        public List<Tbl_Payment_Config> ET_Admin_PaymentConfig_Restore_BL()
        {
            return ObjDL.ET_Admin_PaymentConfig_Restore_DL();
        }
        public int ET_Admin_PaymentConfig_Restore_Insert_BL(int id, string Updatedby)
        {
            return ObjDL.ET_Admin_PaymentConfig_Restore_Insert_DL(id, Updatedby);
        }
        public Tbl_Payment_Config ET_Admin_PaymentConfig_View_BL(int id)
        {
            return ObjDL.ET_Admin_PaymentConfig_View_DL(id);
        }

        }
    }
