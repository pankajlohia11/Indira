using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic;
using BusinessEntity.EntityModels;
using DataAccess;

namespace BusinessLogic
{
    public class ET_Admin_Company_BL
    {
        ET_Admin_Company_DL ObjDL = new ET_Admin_Company_DL();

        public List<Tbl_Master_CompanyDetails> ET_Admin_Companylist_BL(bool deleted, int com_key)
        {
            return ObjDL.ET_Admin_Companylist_DL(deleted,com_key);
        }

        public decimal ET_Master_Company_Add_BL(Tbl_Master_CompanyDetails Obj, bool automanual, string prefix, string Contactdata,string Bankdata, out string compcode)
        {
            return ObjDL.ET_Master_Company_Add_DL(Obj, automanual, prefix, Contactdata, Bankdata, out compcode);
        }

        public int ET_Master_Company_Deleted_BL(int id)
        {
            return ObjDL.ET_Master_Company_Deleted_DL(id);
        }

        public List<location> Binddropdown_County_BL()
        {
            return ObjDL.Binddropdown_Country_DL();
        }

        public List<location> Binddropdown_State_BL(int id)
        {
            return ObjDL.Binddropdown_State_DL(id);
        }

        public List<location> Binddropdown_City_BL(int id)
        {
            return ObjDL.Binddropdown_City_DL(id);
        }

        public List<Tbl_Master_CompanyDetails> ET_Master_Company_View_BL(int id)
        {
            return ObjDL.ET_Master_Company_View_DL(id);
        }

        public List<Tbl_Master_CompanyDetails> ET_Master_Company_Restore_BL()
        {
            return ObjDL.ET_Master_Company_Restore_DL();
        }

        public int ET_Master_Company_Restore_Insert_BL(int id, bool type)
        {
            return ObjDL.ET_Master_Company_Restore_Insert_DL(id,type);
        }
        public Tbl_Master_CompanyDetails ET_Master_Company_Update_GetbyID(int id)
        {
            return ObjDL.ET_Master_Company_Update_GetbyID(id);
        }

        public List<Tbl_Master_CompanyContacts> ET_Master_Company_Contacts_BL(int id)
        {
            return ObjDL.ET_Master_Company_Contacts_DL(id);
        }
        public List<Tbl__Master_CompanyBank> ET_Master_Company_Bank_BL(int id)
        {
            return ObjDL.ET_Master_Company_Bank_DL(id);
        }
        public List<Tbl_Master_CompanyContacts> ET_Master_Company_Contactsedit_BL(int id)
        {
            return ObjDL.ET_Master_Company_Contactsedit_BL(id);
        }
        public decimal ET_Admin_Contact_Add_BL(Tbl_Master_CompanyContacts obj)
        {
            return ObjDL.ET_Admin_Contact_Add_DL(obj);
        }

        public string CheckDuplicateCode_BL(int ComID, string CompanyCode)
        {
            return ObjDL.CheckDuplicateCode_DL(ComID, CompanyCode);
        }
        public string CheckDuplicateCompanyName_BL(int ComID, string CompanyName)
        {
            return ObjDL.CheckDuplicateCompanyName_DL(ComID, CompanyName);
        }
        public string CheckDuplicateDisplayName_BL(int ComID, string DisplayName)
        {
            return ObjDL.CheckDuplicateDisplayName_DL(ComID, DisplayName);
        }

        public string CheckDuplicateEmail_BL(int ComID, string Email)
        {
            return ObjDL.CheckDuplicateCode_DL(ComID, Email);
        }

        public List<Tbl_BankMaster> Bind_dropdown_Bankname_BL(int com_key)
        {
            return ObjDL.Bind_dropdown_Bankname_DL(com_key);
        }
    }
}
