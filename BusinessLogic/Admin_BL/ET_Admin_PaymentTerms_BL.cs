using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessEntity.EntityModels;
using BusinessLogic.Admin_BL;
using BusinessLogic;
using DataAccess;
using DataAccess.Admin_DA;

namespace BusinessLogic.Admin_BL
{
    public class ET_Admin_PaymentTerms_BL
    {
        ET_Admin_PaymentTerms_DL ObjDL = new ET_Admin_PaymentTerms_DL();
        public List<Tbl_Payment_Terms> GetPayment_Terms_List(bool deleted)
        {
            return ObjDL.GetPayment_Terms_List(deleted);
        }
        public decimal ET_Admin_PaymentTerms_Add_BL(Tbl_Payment_Terms obj, string prefix, bool automanual,out string PayCode)
        {
            return ObjDL.ET_Admin_PaymentTerms_Add_DL(obj, prefix, automanual,out PayCode);
        }
        public int ET_Admin_PaymentTerms_Restore_Delete_BL(int id, decimal Updatedby, bool type)
        {
            return ObjDL.ET_Admin_PaymentTerms_Restore_Delete_DL(id, Updatedby, type);
        }
        public Tbl_Payment_Terms ET_Admin_PaymentTerms_Update_GetbyID_BL(int id)
        {
            return ObjDL.ET_Admin_PaymentTerms_Update_GetbyID_DL(id);
        }
        public Tbl_Payment_Terms ET_Admin_PaymentTerms_View_BL(int id)
        {
            return ObjDL.ET_Admin_PaymentTerms_View_DL(id);
        }

        public string CheckDuplicateCode_BL(decimal id, string txtpamentCode)
        {
            return ObjDL.CheckDuplicateCode_DL(id, txtpamentCode);
        }
    }
}
