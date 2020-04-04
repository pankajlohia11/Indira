using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessEntity.EntityModels;
using DataAccess.Admin_DA;

namespace BusinessLogic.Admin_BL
{
    public class Admin_Bank_BL
    {
        Admin_Bank_DL ObjDL = new Admin_Bank_DL();

        public List<Tbl_BankMaster> ET_Admin_Bank_BL(int com_key)
        {
            return ObjDL.ET_Admin_Bank_DL(com_key);
        }

        public decimal ET_Admin_Bank_Add_BL(Tbl_BankMaster obj, string prefix, bool automanual,out string bankcode)
        {
            return ObjDL.ET_Admin_Bank_Add_DL(obj, prefix, automanual,out bankcode);
        }

        public int ET_Admin_Roles_Deleted_BL(int id, decimal deleteby)
        {
            return ObjDL.ET_Admin_Roles_Deleted_DL(id, deleteby);
        }

        public int ET_Admin_Bank_Restore_Insert_BL(int id, decimal deleteby)
        {
            return ObjDL.ET_Admin_Bank_Restore_Insert_DL(id, deleteby);
        }
        public List<Tbl_BankMaster> ET_Admin_Bank_Restore_BL()
        {
            return ObjDL.ET_Admin_Bank_Restore_DL();
        }
        public Tbl_BankMaster ET_Admin_Bank_View_BL(int id)
        {
            return ObjDL.ET_Admin_Bank_View_DL(id);
        }

        public Tbl_BankMaster ET_Admin_Bank_Update_GetbyID_BL(int id)
        {
            return ObjDL.ET_Admin_Bank_Update_GetbyID_DL(id);
        }

        public string CheckDuplicateCode_BL(int id, string Bankcode)
        {
            return ObjDL.CheckDuplicateCode_DL(id, Bankcode);
        }
        public string CheckDuplicateBankName_BL(int id, string BankName)
        {
            return ObjDL.CheckDuplicateBankName_DL(id, BankName);
        }
        public string CheckDuplicateDisplayName_BL(int id, string DisplayName)
        {
            return ObjDL.CheckDuplicateDisplayName_DL(id, DisplayName);
        }

        public string CheckDuplicateAccountno_BL(int id, string AccountNo)
        {
            return ObjDL.CheckDuplicateAccountno_DL(id, AccountNo);
        }
    }
    }
