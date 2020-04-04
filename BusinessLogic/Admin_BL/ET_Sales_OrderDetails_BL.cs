using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess;
using DataAccess.Admin_DA;
using BusinessEntity.EntityModels;
using BusinessLogic.Admin_BL;
using System.Data;
using BusinessEntity.CustomModels;

namespace BusinessLogic.Admin_BL
{
    public class ET_Sales_OrderDetails_BL
    {
        ET_Sales_OrderDetails_DL ObjDL = new ET_Sales_OrderDetails_DL();

        //public List<Tbl_Order_Details_CM> Tbl_OrderList_BL(int Comkey, bool type)
        //{
        //    return ObjDL.Tbl_OrderList_DL(Comkey, type);
        //}
        public List<Tbl_Master_User> Sales_person_dropdown_BL(int com_key)
        {
            return ObjDL.Sales_person_dropdown_DL(com_key);
        }

        public List<Tbl_Payment_Terms> Payment_terms_dropdown_BL(int com_key)
        {
            return ObjDL.Payment_terms_dropdown_DL(com_key);
        }
        public DataTable OrgEmp_BL(int com_key,decimal orgtype)
        {
            return ObjDL.OrgEmp_DL(com_key,orgtype);
        }

        public decimal ET_Sales_OrderDetails_Add_BL(Tbl_Master_Order obj, string prefix, bool automanual, string Orderdetails,out string OrdCode)
        {
            return ObjDL.ET_Sales_OrderDetails_Add_DL(obj, prefix, automanual, Orderdetails,out OrdCode);
        }

        public decimal ET_Sales_CloneOrder_BL(int orderId,string prefix, out string orderCode)
        {
            return ObjDL.ET_Sales_CloneOrder_DL(orderId, prefix, out orderCode);
        }

        public Tbl_Master_Order ET_Sales_OrderDetails_Update_GetbyID_BL(int id)
        {
            return ObjDL.ET_Sales_OrderDetails_Update_GetbyID_DL(id);
        }

        public int ET_Sales_OrderDetails_Restore_Delete_BL(int id, decimal Updatedby, bool type)
        {
            return ObjDL.ET_Sales_OrderDetails_Restore_Delete_DL(id, Updatedby, type);
        }
        public Tbl_Order_Details_CM ET_General_Order_Details_View_BL(int id)
        {
            return ObjDL.ET_General_Order_Details_View_DL(id);
        }
        public List<Tbl_Order_Details_CM> ET_Sales_OrderDetails_Update_Childtable_BL(int id)
        {
            return ObjDL.ET_Sales_OrderDetails_Update_Childtable_DL(id);
        }
        public List<Tbl_Offer_Details_CM> ET_General_Order_SuplierProduct_Offer_BL(int id, int pid)
        {
            return ObjDL.ET_General_Order_SuplierProduct_Offer_DL(id, pid);
        }

        public string CheckDuplicateCode_BL(decimal id, string OrderCode)
        {
            return ObjDL.CheckDuplicateCode_DL(id, OrderCode);
        }
    }
}
