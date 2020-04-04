using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessEntity.EntityModels;
using DataAccess;
using BusinessEntity.CustomModels;

namespace BusinessLogic
{
    public class BAL
    {
        DAL dal = new DAL();
        public List<Tbl_Master_User> Authenticate_BL(Tbl_Master_User objBE)
        {
            return dal.Authenticate_DA(objBE);
        }
        public int ExceptionInsertLogs_BL(error_master objBE)
        {
            return dal.ExceptionInsertLogs_DA(objBE);
        }
        public int OperationInsertLogs_BL(tbl_loginfo objBE)
        {
            return dal.OperationInsertLogs_DA(objBE);
        }
        public List<Tbl_AccessPermissions> GetPrivilages_BL(int userid, int dockey)
        {
            return dal.GetPrivilages_DA(userid, dockey);
        }
        public bool CheckIfAdminUser_BL(int userid)
        {
            return dal.CheckIfAdminUser_DA(userid);
        }

        public List<location> Binddropdown_County_BL()
        {
            return dal.Binddropdown_Country_DL();
        }

        public List<location> Binddropdown_State_BL(int id)
        {
            return dal.Binddropdown_State_DL(id);
        }

        public List<location> Binddropdown_City_BL(int id)
        {
            return dal.Binddropdown_City_DL(id);
        }

        public List<Tbl_Document_Master> AutoManual_BL(int dockey)
        {
            return dal.AutoManual_DA(dockey);
        }
        public List<Tbl_Master_CompanyContacts> Dropdown_ContactPerson_BL(int id)
        {
            return dal.Dropdown_ContactPerson_DL(id);
        }
        public List<Tbl_Master_CompanyDetails> Bind_dropdown_Customer_BL(decimal companykey)
        {
            return dal.Bind_dropdown_Customer_DL(companykey);
        }
        public List<Tbl_Master_CompanyDetails> Bind_dropdown_Supplier_BL(decimal companykey)
        {
            return dal.Bind_dropdown_Supplier_DL(companykey);
        }
        public List<Tbl_Currency_Master> Bind_Currency_BL()
        {
            return dal.Bind_Currency_DL();
        }
        public List<Tbl_Master_Category> Dropdown_Category_BL(decimal companykey)
        {
            return dal.Dropdown_Category_DL(companykey);
        }
        public List<Tbl_Product_Master> Dropdown_Products_BL(decimal companykey)
        {
            return dal.Dropdown_Products_DL(companykey);
        }
        public List<Tbl_LookUp_CM> Bind_Packing_BL()
        {
            return dal.Bind_Packing_DL();
        }
        public int InsertNotification_BO(tbl_Notifications obj)
        {
            return dal.InsertNotification_DAO(obj);
        }
    }
}
