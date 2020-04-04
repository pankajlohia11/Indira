using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessEntity.EntityModels;
using BusinessEntity.CustomModels;

namespace DataAccess.Admin_DA
{
    public class ET_Admin_Store_DL
    {
        EntityClasses dbcontext = new EntityClasses();
        public List<Tbl_Locations_Master> ET_Admin_Store(int com_key)
        {
            try
            {
                var data = dbcontext.Tbl_Locations_Master.Where(m => m.Deleted == false && m.COM_KEY == com_key).ToList();
                return data;
            }
            catch (Exception exe)
            {
                throw exe;
            }
        }

        public decimal ET_Admin_Store_Add_DL(Tbl_Locations_Master obj, string prefix, bool automanual)
        {
            Tbl_Locations_Master objgl = new Tbl_Locations_Master();
            try
            {
                if (obj.Store_ID == 0)
                {
                    Tbl_Locations_Master objtlm = new Tbl_Locations_Master()
                    {
                        Store_Code = obj.Store_Code,
                        Store_Name = obj.Store_Name,
                        Display_Name = obj.Display_Name,
                        Store_KeeperName = obj.Store_KeeperName,
                        Country = obj.Country,
                        State = obj.State,
                        City = obj.City,
                        Street = obj.Street,
                        Zip = obj.Zip,
                        Deleted = false,
                        ContactNo = obj.ContactNo,
                        Created_Date = DateTime.Now,
                        LAST_UPDATED_DATE = DateTime.Now,
                        COM_KEY=obj.COM_KEY
                    };
                    dbcontext.Tbl_Locations_Master.Add(objtlm);
                    dbcontext.SaveChanges();

                    if (automanual == true)
                    {
                        string code = prefix +"0"+ objtlm.Store_ID;
                        Tbl_Locations_Master Tbl_Locations_Master = dbcontext.Tbl_Locations_Master.Single(m => m.Store_ID == objtlm.Store_ID);
                        {
                            Tbl_Locations_Master.Store_Code = code;
                        };
                        dbcontext.SaveChanges();
                    }
                    objgl.Store_ID = objtlm.Store_ID;
                }
                else
                {
                    Tbl_Locations_Master objtlmu = dbcontext.Tbl_Locations_Master.Single(m => m.Store_ID == obj.Store_ID);
                    {
                        objtlmu.Store_Code = obj.Store_Code;
                        objtlmu.Store_Name = obj.Store_Name;
                        objtlmu.Display_Name = obj.Display_Name;
                        objtlmu.Store_KeeperName = obj.Store_KeeperName;
                        objtlmu.Country = obj.Country;
                        objtlmu.State = obj.State;
                        objtlmu.City = obj.City;
                        objtlmu.Street = obj.Street;
                        objtlmu.Zip = obj.Zip;
                        objtlmu.ContactNo = obj.ContactNo;
                        objtlmu.LAST_UPDATED_DATE = DateTime.Now;
                    };
                    objgl.Store_ID = dbcontext.SaveChanges();
                }
            }
            catch (Exception exe)
            {
                throw exe;
            }
            return objgl.Store_ID;
        }

        public Tbl_Locations_Master ET_Admin_Store_Update_GetbyID_DL(int id)
        {
            try
            {
                return dbcontext.Tbl_Locations_Master.Single(m => m.Store_ID == id);
            }
            catch (Exception exe)
            {
                throw exe;
            }
        }

        public int ET_Admin_Store_Delete_DL(int id, decimal deleteby)
        {
            try
            {
                Tbl_Locations_Master obj = dbcontext.Tbl_Locations_Master.Single(m => m.Store_ID == id && m.Deleted == false);
                {
                    obj.Deleted = true;
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

        public List<Tbl_Locations_Master> ET_Admin_Store_Restore_DL()
        {
            try
            {
                var data = dbcontext.Tbl_Locations_Master.Where(m => m.Deleted == true).ToList();
                return data;
            }
            catch (Exception exe)
            {
                throw exe;
            }
        }

        public int ET_Admin_Store_Restore_Insert_DL(int id, decimal Updatedby)
        {
            try
            {
                Tbl_Locations_Master obj = dbcontext.Tbl_Locations_Master.Single(m => m.Store_ID == id && m.Deleted == true);
                {
                    obj.Deleted = false;
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

        public Tbl_Locations_Master ET_Admin_Store_View_DL(int id)
        {
            try
            {
                return dbcontext.Tbl_Locations_Master.Single(m => m.Store_ID == id);
            }
            catch (Exception exe)
            {
                throw exe;
            }
        }

        public string CheckDuplicateCode_DL(int id, string Storecode)
        {
            try
            {
                if (id == 0)
                {
                    var count = dbcontext.Tbl_Locations_Master.Where(m => m.Store_Code == Storecode).Count();
                    if (count > 0)
                    {
                        return "Exist";
                    }
                }
                else
                {
                    var count = dbcontext.Tbl_Locations_Master.Where(m => m.Store_ID != id && m.Store_Code == Storecode).Count();
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

        public string CheckDuplicateStoreName_DL(int id, string StoreName)
        {
            try
            {
                if (id == 0)
                {
                    var count = dbcontext.Tbl_Locations_Master.Where(m => m.Store_Name == StoreName).Count();
                    if (count > 0)
                    {
                        return "Exist";
                    }
                }
                else
                {
                    var count = dbcontext.Tbl_Locations_Master.Where(m => m.Store_ID != id && m.Store_Name == StoreName).Count();
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

        public string CheckDuplicateDisplayName_DL(int id, string DisplayName)
        {
            try
            {
                if (id == 0)
                {
                    var count = dbcontext.Tbl_Locations_Master.Where(m => m.Display_Name == DisplayName).Count();
                    if (count > 0)
                    {
                        return "Exist";
                    }
                }
                else
                {
                    var count = dbcontext.Tbl_Locations_Master.Where(m => m.Store_ID != id && m.Display_Name == DisplayName).Count();
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

        public string CheckDuplicateStoreKeeper_DL(int id, string StoreKeeper)
        {
            try
            {
                if (id == 0)
                {
                    var count = dbcontext.Tbl_Locations_Master.Where(m => m.Store_KeeperName == StoreKeeper).Count();
                    if (count > 0)
                    {
                        return "Exist";
                    }
                }
                else
                {
                    var count = dbcontext.Tbl_Locations_Master.Where(m => m.Store_ID != id && m.Store_KeeperName == StoreKeeper).Count();
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
