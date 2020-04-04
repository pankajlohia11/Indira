using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessEntity.EntityModels;
using BusinessEntity.CustomModels;

namespace DataAccess.Admin_DA
{
    public class ET_Admin_Material_DL
    {
        EntityClasses dbcontext = new EntityClasses();

        public List<Tbl_Material_Master> ET_Admin_Material_List_DL()
        {
            try
            {
                var data = dbcontext.Tbl_Material_Master.Where(m => m.DELETED == false).ToList();
                return data;
            }
            catch (Exception exe)
            {
                throw exe;
            }
        }

        public decimal ET_Admin_Material_Add_DL(Tbl_Material_Master obj, bool automanual, string prefix)
        {
            try
            {
                if (obj.MATERIAL_ID == 0)
                {
                    Tbl_Material_Master objtmm = new Tbl_Material_Master()
                    {
                        MATERIAL_NAME = obj.MATERIAL_NAME,
                        MATERIAL_CODE = obj.MATERIAL_CODE,
                        MATERIAL_DESCRIPTION = obj.MATERIAL_DESCRIPTION,
                        COTTON_PER = obj.COTTON_PER,
                        DELETED = false,
                        CREATED_DATE = DateTime.Now,
                        CREATED_BY = obj.CREATED_BY,
                        COM_KEY=obj.COM_KEY
                    };
                    dbcontext.Tbl_Material_Master.Add(objtmm);
                    dbcontext.SaveChanges();
                    obj.MATERIAL_ID = objtmm.MATERIAL_ID;
                    if (automanual == true)
                    {
                        int len = 10 - (prefix + obj.MATERIAL_ID).Length;
                        string code = prefix + new String('0', len) + obj.MATERIAL_ID;
                        Tbl_Material_Master objTbl_MaterialMaster = dbcontext.Tbl_Material_Master.Single(m => m.MATERIAL_ID == obj.MATERIAL_ID);
                        {
                            objTbl_MaterialMaster.MATERIAL_CODE = code;
                        };
                        dbcontext.SaveChanges();
                    }
                }
                else
                {
                    Tbl_Material_Master objtmm = dbcontext.Tbl_Material_Master.Single(m => m.MATERIAL_ID == obj.MATERIAL_ID);
                    {
                        objtmm.MATERIAL_CODE = obj.MATERIAL_CODE;
                        objtmm.MATERIAL_NAME = obj.MATERIAL_NAME;
                        objtmm.MATERIAL_DESCRIPTION = obj.MATERIAL_DESCRIPTION;
                        objtmm.COTTON_PER = obj.COTTON_PER;
                        objtmm.LAST_UPDATED_DATE = DateTime.Now;
                        objtmm.LAST_UPDATED_BY = obj.LAST_UPDATED_BY;
                    };
                    obj.MATERIAL_ID = dbcontext.SaveChanges();
                    
                }
                return obj.MATERIAL_ID;
            }
            catch (Exception exe)
            {
                throw exe;
            }
        }
        public string CheckDuplicateCode_DA(int id, string code)
        {
            if (id == 0)
            {
                int count = dbcontext.Tbl_Material_Master.Where(m => m.MATERIAL_CODE == code).Count();
                if (count > 0)
                {
                    return "exist";
                }
            }
            else
            {
                int count = dbcontext.Tbl_Material_Master.Where(m => m.MATERIAL_CODE == code && m.MATERIAL_ID != id).Count();
                if (count > 0)
                {
                    return "exist";
                }
            }

            return "";
        }
        public string CheckDuplicateMaterialName_DA(int id, string name)
        {
            if (id == 0)
            {
                int count = dbcontext.Tbl_Material_Master.Where(m => m.MATERIAL_NAME == name).Count();
                if (count > 0)
                {
                    return "exist";
                }
            }
            else
            {
                int count = dbcontext.Tbl_Material_Master.Where(m => m.MATERIAL_NAME == name && m.MATERIAL_ID != id).Count();
                if (count > 0)
                {
                    return "exist";
                }
            }

            return "";
        }
    }
}
