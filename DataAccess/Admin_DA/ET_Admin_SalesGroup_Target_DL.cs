using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessEntity.EntityModels;
/*
    Author=Manoj
    Date = 19th Apr 2018
    Sales Target Data Access 
 */
namespace DataAccess.Admin_DA
{
    public class ET_Admin_SalesGroup_Target_DL
    {
        // Creating Database Object for EntityClasses
        EntityClasses dbcontext = new EntityClasses();

        //Sales Target List View
        public List<Tbl_SalesGroup_Target> ET_Admin_SalesGroupTargetList_DL()
        {
            try
            {
                var data = dbcontext.Tbl_SalesGroup_Target.Where(m => m.DELETED != true).ToList();

                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        //Bind Dropdown Sales Organization List
        public List<Tbl_Sales_Organization> Binddropdown_Organization_DL()
        {
            try
            {
                dbcontext.Configuration.ProxyCreationEnabled = false;
                var orglist = dbcontext.Tbl_Sales_Organization.Where(m => m.DELETED == false).ToList();
                return orglist;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        //Check Financial Year and Organization Exists
        public string CheckDuplicateTarget_DL(int GroupTargetID, int FinYear, int Salesorg)
        {
            if (GroupTargetID == 0)
            {
                int count = dbcontext.Tbl_SalesGroup_Target.Where(m => m.SGT_FIN_YEAR == FinYear && m.SGT_GROUP_ID== Salesorg).Count();
                if (count > 0)
                {
                    return "exist";
                }
            }
            else
            {
                int count = dbcontext.Tbl_SalesGroup_Target.Where(m => m.SGT_FIN_YEAR == FinYear && m.SGT_GROUP_ID == Salesorg && m.SGT_ID != GroupTargetID).Count();
                if (count > 0)
                {
                    return "exist";
                }
            }

            return "";
        }

        //Insert / Update Sales Group Target
        public decimal Tbl_SalesGroup_Target_Add(Tbl_SalesGroup_Target tbl_Sales_group_target)
        {
            Tbl_SalesGroup_Target tst = new Tbl_SalesGroup_Target();
            try
            {
                if (tbl_Sales_group_target.SGT_ID == 0)
                {

                    Tbl_SalesGroup_Target Objtst = new Tbl_SalesGroup_Target()
                    {
                        SGT_GROUP_ID = tbl_Sales_group_target.SGT_GROUP_ID,
                        SGT_FIN_YEAR = tbl_Sales_group_target.SGT_FIN_YEAR,
                        SGT_TARGET = tbl_Sales_group_target.SGT_TARGET,
                        CREATED_BY = tbl_Sales_group_target.CREATED_BY,
                        CREATED_DATE = DateTime.Now,
                        DELETED = false
                    };
                    dbcontext.Tbl_SalesGroup_Target.Add(Objtst);
                    dbcontext.SaveChanges();
                   
                    tst.SGT_ID = Objtst.SGT_ID;
                }
                else
                {
                    Tbl_SalesGroup_Target Objtst = dbcontext.Tbl_SalesGroup_Target.Single(m => m.SGT_ID == tbl_Sales_group_target.SGT_ID);
                    {
                        Objtst.SGT_GROUP_ID = tbl_Sales_group_target.SGT_GROUP_ID;
                        Objtst.SGT_FIN_YEAR = tbl_Sales_group_target.SGT_FIN_YEAR;
                        Objtst.SGT_TARGET = tbl_Sales_group_target.SGT_TARGET;
                        Objtst.LAST_UPDATED_BY = tbl_Sales_group_target.CREATED_BY;
                        Objtst.LAST_UPDATED_DATE = DateTime.Now;
                        
                    };
                    tst.SGT_ID = Objtst.SGT_ID;
                    dbcontext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return tst.SGT_ID;
        }

        //Edit Sales Organization Record Details
        public List<Tbl_Sales_Target> ET_Admin_SalesTarget_Update_Get(int id)
        {
            try
            {
                return dbcontext.Tbl_Sales_Target.Where(m => m.ORG_ID == id).ToList();
                // return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Sales Organizatioin View Record Details
        public List<Tbl_Sales_Organization> ET_Admin_SalesOrganization_View(int id)
        {
            return dbcontext.Tbl_Sales_Organization.Where(m => m.ORG_ID == id).ToList();
            // return data;
        }

        // Delete Record
        public int ET_Admin_SalesOrganization_Deleted_DL(int id)
        {
            int result = 0;
            //int i = 1;
            try
            {
                var find = dbcontext.Tbl_Sales_Organization.Where(m => m.ORG_ID == id).ToList();
                if (find.Count() != 0)
                {
                    Tbl_Sales_Organization delete = dbcontext.Tbl_Sales_Organization.Single(m => m.ORG_ID == id);
                    delete.DELETED = true;
                    delete.DELETED_BY = 1;
                    delete.DELETED_DATE = DateTime.Now;

                    result = dbcontext.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        //View List Of Deleted Records
        public List<Tbl_Sales_Organization> ET_Admin_SalesOrganization_Restore_DL()
        {
            try
            {
                var data = dbcontext.Tbl_Sales_Organization.Where(m => m.DELETED == true).ToList();
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // Restore Deleted Record
        public decimal ET_Admin_SalesOrganization_Restore_Insert_DL(int id)
        {
            int result = 0;
            try
            {
                Tbl_Sales_Organization tmr = dbcontext.Tbl_Sales_Organization.Single(m => m.ORG_ID == id);
                {
                    tmr.LAST_UPDATED_DATE = DateTime.Now;
                    tmr.DELETED = false;
                }
                result = dbcontext.SaveChanges();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        

        

    }
}
