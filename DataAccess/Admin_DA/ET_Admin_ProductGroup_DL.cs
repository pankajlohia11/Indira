using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess;
using DataAccess.Admin_DA;
using BusinessEntity.CustomModels;
using BusinessEntity.EntityModels;

namespace DataAccess.Admin_DA
{
    public class ET_Admin_ProductGroup_DL
    {
        EntityClasses dbcontext = new EntityClasses();
        public List<Tbl_ProductGroup> ET_Admin_ProductGrouplist_DL(int com_key)
        {
                var data = dbcontext.Tbl_ProductGroup.Where(m => m.DELETED != true && m.COM_KEY == com_key).ToList();
                return data;
        }

        public decimal ET_Admin_ProductGroup_Add_DL(Tbl_ProductGroup obj, bool automanual, string prefix)
        {
            Tbl_ProductGroup objgl = new Tbl_ProductGroup();
            try
            {
                if (obj.PG_ID == 0)
                {

                    Tbl_ProductGroup objtpg = new Tbl_ProductGroup()
                    {
                        PG_NAME = obj.PG_NAME,
                        PG_CODE = obj.PG_CODE,
                        PG_PARENT_ID = obj.PG_PARENT_ID,
                        PG_TYPE = obj.PG_TYPE,
                        CREATED_BY = obj.CREATED_BY,
                        CREATED_DATE = DateTime.Now,
                        LAST_UPDATED_DATE = DateTime.Now,
                        LAST_UPDATED_BY = obj.LAST_UPDATED_BY,
                        DELETED = false,
                        COM_KEY=obj.COM_KEY
                    };
                    dbcontext.Tbl_ProductGroup.Add(objtpg);
                    dbcontext.SaveChanges();
                    if (automanual == true)
                    {
                        string code = prefix + "0" + objtpg.PG_ID;
                        Tbl_ProductGroup objcode = dbcontext.Tbl_ProductGroup.Single(m => m.PG_ID == objtpg.PG_ID);
                        {
                            objcode.PG_CODE = code;
                        };
                        dbcontext.SaveChanges();
                    }
                    objgl.PG_ID = objtpg.PG_ID;
                }
                else
                {
                    Tbl_ProductGroup objtpg = dbcontext.Tbl_ProductGroup.Single(m => m.PG_ID == obj.PG_ID);
                    {
                        objtpg.PG_NAME = obj.PG_NAME;
                        objtpg.PG_CODE = obj.PG_CODE;
                        objtpg.PG_PARENT_ID = obj.PG_PARENT_ID;
                        objtpg.PG_TYPE = obj.PG_TYPE;
                        objtpg.LAST_UPDATED_DATE = DateTime.Now;
                        objtpg.LAST_UPDATED_BY = obj.LAST_UPDATED_BY;
                        objtpg.DELETED = obj.DELETED;
                    };
                    objgl.PG_ID = dbcontext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return objgl.PG_ID;
        }

        public Tbl_ProductGroup ET_Admin_ProductGroup_Update_GetbyID_DL(int id)
        {
            try
            {
                return dbcontext.Tbl_ProductGroup.Single(m => m.PG_ID == id);
            }
            catch (Exception exe)
            {
                throw exe;
            }
        }

        public int ET_Admin_ProductGroup_Delete_DL(int id, decimal deleteby)
        {
            try
            {
                Tbl_ProductGroup obj = dbcontext.Tbl_ProductGroup.Single(m => m.PG_ID == id && m.DELETED == false);
                {
                    obj.DELETED = true;
                    obj.DELETED_DATE = DateTime.Now;
                    obj.DELETED_BY = deleteby;
                }
                int i = dbcontext.SaveChanges();
                return i;
            }
            catch (Exception exe)
            {
                throw exe;
            }
        }

        public List<Tbl_ProductGroup> ET_Admin_ProductGroup_Restore_DL()
        {
            try
            {
                var data = dbcontext.Tbl_ProductGroup.Where(m => m.DELETED == true).ToList();
                return data;
            }
            catch (Exception exe)
            {
                throw exe;
            }
        }

        public int ET_Admin_ProductGroup_Restore_Insert_DL(int id, decimal Updatedby)
        {
            try
            {
                Tbl_ProductGroup obj = dbcontext.Tbl_ProductGroup.Single(m => m.PG_ID == id && m.DELETED == true);
                {
                    obj.DELETED = false;
                    obj.DELETED_DATE = DateTime.Now;
                    obj.LAST_UPDATED_BY = Updatedby;
                }
                int i = dbcontext.SaveChanges();
                return i;
            }
            catch (Exception exe)
            {
                throw exe;
            }
        }

        public Tbl_ProductGroup ET_Admin_ProductGroup_View_DL(int id)
        {
            try
            {
                return dbcontext.Tbl_ProductGroup.Single(m => m.PG_ID == id);
            }
            catch (Exception exe)
            {
                throw exe;
            }
        }
        public List<Tbl_ProductGroup> BindDropdown_DL(int id)
        {
            try
            {
                var product = dbcontext.Tbl_ProductGroup.Where(m => m.PG_NAME != null && m.PG_ID != id).ToList();
                return product;
            }
            catch (Exception exe)
            {
                throw exe;
            }
        }

        public string CheckDuplicateCode_DL(decimal id, string GroupCode)
        {
            try
            {
                if (id == 0)
                {
                    var count = dbcontext.Tbl_ProductGroup.Where(m => m.PG_CODE == GroupCode).Count();
                    if (count > 0)
                    {
                        return "Exist";
                    }
                }
                else
                {
                    var count = dbcontext.Tbl_ProductGroup.Where(m => m.PG_ID != id && m.PG_CODE == GroupCode).Count();
                    if (count > 0)
                    {
                        return "Exist";
                    }
                }
                return "";
            }
            catch (Exception exe)
            {
                throw exe;
            }
        }

        public string CheckDuplicateGroupName_DL(decimal id, string GroupName)
        {
            try
            {
                if (id == 0)
                {
                    var count = dbcontext.Tbl_ProductGroup.Where(m => m.PG_NAME == GroupName).Count();
                    if (count > 0)
                    {
                        return "Exist";
                    }
                }
                else
                {
                    var count = dbcontext.Tbl_ProductGroup.Where(m => m.PG_ID != id && m.PG_NAME == GroupName).Count();
                    if (count > 0)
                    {
                        return "Exist";
                    }
                }
                return "";
            }
            catch (Exception exe)
            {
                throw exe;
            }
        }
    }
}
