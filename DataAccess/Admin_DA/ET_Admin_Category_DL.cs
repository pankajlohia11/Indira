using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Threading.Tasks;
using BusinessEntity.EntityModels;
using BusinessEntity.CustomModels;


namespace DataAccess
{
    public class ET_Admin_Category_DL
    {
        EntityClasses dbcontext = new EntityClasses();
        public List<Tbl_Category_Details> ET_Admin_Category_List_DL(int com_key)
        {
            try
            {
                var data = dbcontext.Tbl_Category_Details.Where(m => m.DELETED == false && m.COM_KEY==com_key).ToList();
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    

        public int ET_Admin_Category_Deleted(int id)
        {
            int result = 0;
            try
            {
                var find = dbcontext.Tbl_Category_Details.Where(m => m.CAT_DETAILS_ID == id).ToList();
                if (find.Count() != 0)
                {
                    Tbl_Category_Details delete = dbcontext.Tbl_Category_Details.Single(m => m.CAT_DETAILS_ID == id);
                    delete.DELETED = true;
                    delete.DELETED_BY = 1;
                };
                result = dbcontext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public Tbl_Category_Details ET_Admin_Category_Update_Get_DL(int id)
        {
            try
            {
                return dbcontext.Tbl_Category_Details.Single(m => m.CAT_DETAILS_ID == id);
            }
            catch (Exception exe)
            {
                throw exe;
            }
        }

        public decimal Tbl_Category_Details_Add_DL(Tbl_Category_Details obj, string prefix, bool automanual)
        {
            Tbl_Category_Details objgl = new Tbl_Category_Details();
            try
            {
                if (obj.CAT_DETAILS_ID == 0)
                {
                    Tbl_Category_Details objtcd = new Tbl_Category_Details()
                    {
                        CATEGORY = obj.CATEGORY,
                        CATEGORY_CODE = obj.CATEGORY_CODE,
                        SUB_CATEGORY = obj.SUB_CATEGORY,
                        DESCRIPTION = obj.DESCRIPTION,
                        CREATED_DATE = DateTime.Now,
                        CREATED_BY = obj.CREATED_BY,
                        DELETED = false,
                        COM_KEY=obj.COM_KEY

                    };
                    dbcontext.Tbl_Category_Details.Add(objtcd);
                    obj.CAT_DETAILS_ID = dbcontext.SaveChanges();

                    if (automanual == true)
                    {
                        string code = prefix + "0" + objtcd.CAT_DETAILS_ID;
                        Tbl_Category_Details Tbl_Category_Details = dbcontext.Tbl_Category_Details.Single(m => m.CAT_DETAILS_ID == objtcd.CAT_DETAILS_ID);
                        {
                            Tbl_Category_Details.CATEGORY_CODE = code;
                        };
                        dbcontext.SaveChanges();
                    }
                    objgl.CAT_DETAILS_ID = objtcd.CAT_DETAILS_ID;
                }
                else
                {
                    Tbl_Category_Details objtcd = dbcontext.Tbl_Category_Details.Single(m => m.CAT_DETAILS_ID == obj.CAT_DETAILS_ID);
                    {
                        objtcd.CATEGORY = obj.CATEGORY;
                        objtcd.SUB_CATEGORY = obj.SUB_CATEGORY;
                        objtcd.DESCRIPTION = obj.DESCRIPTION;
                        objtcd.LAST_UPDATED_BY = obj.LAST_UPDATED_BY;
                        objtcd.CATEGORY_CODE = obj.CATEGORY_CODE;
                        objtcd.LAST_UPDATED_DATE = DateTime.Now;
                    };
                    objgl.CAT_DETAILS_ID = dbcontext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return objgl.CAT_DETAILS_ID;
        }       

        public string CheckDuplicateCode_DL(int CategoryID, string CategoryCode)
        {
            try
            {
                if (CategoryID == 0)
                {
                    var count = dbcontext.Tbl_Category_Details.Where(m => m.CATEGORY_CODE == CategoryCode).Count();
                    if (count > 0)
                    {
                        return "Exist";
                    }
                }
                else
                {
                    var count = dbcontext.Tbl_Category_Details.Where(m => m.CAT_DETAILS_ID != CategoryID && m.CATEGORY_CODE == CategoryCode).Count();
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

        public List<Tbl_Category_Details> ET_Admin_Category_Restore_DL()
        {
            try
            {
                var data = dbcontext.Tbl_Category_Details.Where(m => m.DELETED == true).ToList();
                return data;
            }
            catch (Exception exe)
            {
                throw exe;
            }
        }

        public int ET_Admin_Category_Restore_Insert_DL(int id,decimal updatedby)
        {
            int i = 0;
            try
            {                
                Tbl_Category_Details obj = dbcontext.Tbl_Category_Details.Single(m => m.CAT_DETAILS_ID == id && m.DELETED == true);
                {
                    obj.DELETED = false;
                    obj.DELETED_BY = updatedby;
                    obj.LAST_UPDATED_DATE = DateTime.Now;
                };
                i = dbcontext.SaveChanges();
            }
            catch (Exception exe)
            {
                throw exe;
            }
            return i;
        }

        public Tbl_Category_Details ET_Admin_Category_View_DL(int id)
        {
            try
            {
                return dbcontext.Tbl_Category_Details.Single(m => m.CAT_DETAILS_ID == id);
            }
            catch (Exception exe)
            {
                throw exe;
            }
        }
    }
}
