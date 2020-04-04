using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Admin_DA;
using BusinessEntity.EntityModels;
using BusinessEntity.CustomModels;

namespace BusinessLogic.Admin_BL
{
    public class ET_Agency_Enquiry_BL
    {
        EntityClasses dbcontext = new EntityClasses();
        ET_Agency_Enquiry_DL objDL = new ET_Agency_Enquiry_DL();
        public List<Enquiry_CM> ET_Admin_EnquiryList_BL(bool deleted, int com_key, int type)
        {
            return objDL.ET_Admin_EnquiryList_DL(deleted, com_key, type);
        }

        public decimal ET_Admin_Enquiry_Add_BL(tbl_EnquiryHeader obj, bool automanual, string prefix, string EnquiryDetails,out string EnqCode)
        {
            return objDL.ET_Admin_Enquiry_Add_DL(obj, automanual, prefix, EnquiryDetails,out EnqCode);
        }

        public int ET_Admin_Enquiry_DeletRestore_BL(int id, bool delete, int uid)
        {
            return objDL.ET_Admin_Enquiry_DeletRestore_DL(id, delete, uid);
        }

        public tbl_EnquiryHeader ET_Admin_Enquiry_Update_GetbyID_BL(int id)
        {
            return objDL.ET_Admin_Enquiry_Update_GetbyID_DL(id);
        }

        public List<tbl_EnquiryDetails> ET_Admin_Enquiry_Details_BL(int id)
        {
            return objDL.ET_Admin_Enquiry_Details_DL(id);
        }
        
        public string CheckDuplicateCode_BL(int ID, string Code)
        {
            return objDL.CheckDuplicateCode_DL(ID, Code);
        }
        public List<Tbl_Master_CompanyDetails> ET_Admin_CustomerList_BL(int comkey)
        {
            return objDL.ET_Admin_CustomerList_DL(comkey);
        }
        public List<Tbl_Master_CompanyContacts> ET_Admin_ContactList_BL(int customerid)
        {
            return objDL.ET_Admin_ContactList_DL(customerid);
        }
        public List<Tbl_Master_User> ET_Admin_SalesPersonList_BL(int comkey)
        {
            return objDL.ET_Admin_SalesPersonList_DL(comkey);
        }
    }
}

