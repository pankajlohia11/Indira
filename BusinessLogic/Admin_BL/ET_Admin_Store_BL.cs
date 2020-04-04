using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessEntity.EntityModels;
using BusinessEntity.CustomModels;
using DataAccess.Admin_DA;

namespace BusinessLogic.Admin_BL
{

    public class ET_Admin_Store_BL
    {
        ET_Admin_Store_DL ObjDL = new ET_Admin_Store_DL();
        public List<Tbl_Locations_Master> ET_Admin_Store(int com_key)
        {
            return ObjDL.ET_Admin_Store(com_key);
        }

        public decimal ET_Admin_Store_Add_BL(Tbl_Locations_Master obj, string prefix, bool automanual)
        {
            return ObjDL.ET_Admin_Store_Add_DL(obj, prefix, automanual);
        }
        public Tbl_Locations_Master ET_Admin_Store_Update_GetbyID_BL(int id)
        {
            return ObjDL.ET_Admin_Store_Update_GetbyID_DL(id);
        }

        public int ET_Admin_Store_Delete_BL(int id,decimal deleteby)
        {
            return ObjDL.ET_Admin_Store_Delete_DL(id, deleteby);
        }
        public List<Tbl_Locations_Master> ET_Admin_Store_Restore_BL()
        {
            return ObjDL.ET_Admin_Store_Restore_DL();
        }
        public int ET_Admin_Store_Restore_Insert_BL(int id, decimal Updatedby)
        {
            return ObjDL.ET_Admin_Store_Restore_Insert_DL(id, Updatedby);
        }

        public Tbl_Locations_Master ET_Admin_Store_View_BL(int id)
        {
            return ObjDL.ET_Admin_Store_View_DL(id);
        }
        public string CheckDuplicateCode_BL(int id, string Storecode)
        {
            return ObjDL.CheckDuplicateCode_DL(id, Storecode);
        }
        public string CheckDuplicateDisplayName_BL(int id, string DisplayName)
        {
            return ObjDL.CheckDuplicateStoreName_DL(id, DisplayName);
        }
        public string CheckDuplicateStoreName_BL(int id, string StoreName)
        {
            return ObjDL.CheckDuplicateStoreName_DL(id, StoreName);
        }
        public string CheckDuplicateStoreKeeper_BL(int id, string StoreName)
        {
            return ObjDL.CheckDuplicateStoreKeeper_DL(id, StoreName);
        }


    }
}

