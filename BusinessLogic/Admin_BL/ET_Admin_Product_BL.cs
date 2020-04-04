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
    public class ET_Admin_Product_BL
    {
        ET_Admin_Product_DL ObjDL = new ET_Admin_Product_DL();

       
        public Tbl_Product_Master ET_Admin_Product_Add_BL(Tbl_Product_Master obj, bool automanual, string prefix)
        {
            return ObjDL.ET_Admin_Product_Add_DL(obj, automanual, prefix);
        }
       
        public List<Tbl_Master_Products> Master_Products_list()
        {
            return ObjDL.Master_Products_list();
        }

        public int ET_Admin_Product_Deleted_BL(int id, decimal deleteby)
        {
            return ObjDL.ET_Admin_Product_Deleted_DL(id, deleteby);
        }

     
        public Tbl_Product_Master ET_Admin_Product_Update_GetbyID_BL(int id)
        {
            return ObjDL.ET_Admin_Product_Update_GetbyID_DL(id);
        }
        public List<string> Sub_Categorylist_BL()
        {
            return ObjDL.Sub_Categorylist_DL();
        }
        public List<Tbl_Master_UOM> UOM_BL()
        {
            return ObjDL.UOM_DL();
        }
        public Tbl_ProductGroup ET_Admin_ProductGroup_Add_BL(Tbl_ProductGroup obj, bool automanual, string prefix)
        {
            return ObjDL.ET_Admin_ProductGroup_Add_DL(obj, automanual, prefix);
        }
        public string CheckDuplicateGroupName_BL(decimal id, string GroupName)
        {
            return ObjDL.CheckDuplicateGroupName_DL(id, GroupName);
        }
        public string CheckDuplicateCode_BL(decimal id, string GroupCode)
        {
            return ObjDL.CheckDuplicateCode_DL(id, GroupCode);
        }
        public Tbl_ProductGroup ET_Admin_ProductGroup_Update_GetbyID_BL(int id)
        {
            return ObjDL.ET_Admin_ProductGroup_Update_GetbyID_DL(id);
        }
        public int ET_Admin_ProductGroup_Delete_BL(int id, decimal deleteby,string type)
        {
            return ObjDL.ET_Admin_ProductGroup_Delete_DL(id, deleteby, type);
        }
        public List<tbl_LookUp> ET_Admin_ProductGroup_GetUOM_BL()
        {
            return ObjDL.ET_Admin_ProductGroup_GetUOM_DL();
        }
        public string CheckDuplicateProName_BL(decimal id, string Name)
        {
            return ObjDL.CheckDuplicateProName_DL(id, Name);
        }
        public string CheckDuplicateProCode_BL(decimal id, string Code)
        {
            return ObjDL.CheckDuplicateProCode_DL(id, Code);
        }
    }
}
