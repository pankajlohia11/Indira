using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessEntity.EntityModels;
using BusinessEntity.CustomModels;
using DataAccess.Admin_DA;
using DataAccess;

namespace BusinessLogic.Admin_BL
{
    public class ET_Admin_Material_BL
    {
        ET_Admin_Material_DL ObjDL = new ET_Admin_Material_DL();
        public List<Tbl_Material_Master> ET_Admin_Material_List_BL()
        {
            return ObjDL.ET_Admin_Material_List_DL();
        }

        public decimal ET_Admin_Material_Add_BL(Tbl_Material_Master obj, bool automanual, string prefix)
        {
            return ObjDL.ET_Admin_Material_Add_DL(obj, automanual, prefix);
        }
        public string CheckDuplicateCode_BL(int id, string code)
        {
            return ObjDL.CheckDuplicateCode_DA(id,code);
        }
        public string CheckDuplicateMaterialName_BL(int id, string code)
        {
            return ObjDL.CheckDuplicateMaterialName_DA(id, code);
        }
    }
}
