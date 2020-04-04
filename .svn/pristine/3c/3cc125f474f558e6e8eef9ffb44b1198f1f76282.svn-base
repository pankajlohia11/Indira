using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessEntity.EntityModels;
using BusinessEntity.CustomModels;
using DataAccess.Admin_DA;
/*
    Author=Manoj
    Date = 16th Apr 2018
    Sales Target Business Logic 
 */
namespace BusinessLogic.Admin_BL
{
    public class ET_Admin_SalesTarget_BL
    {
        // Creating Object for Sales Organization Data Access
        ET_Admin_SalesTarget_DL objDA = new ET_Admin_SalesTarget_DL();

        //Sales Organization List View
        public List<Tbl_Sales_Target> ET_Admin_SalesTargetList_BL()
        {
            return objDA.ET_Admin_SalesTargetList_DL();
        }

        //Bind Dropdown Sales Organization List
        public List<Tbl_Sales_Organization> Binddropdown_Organization_BL(int com_key)
        {
            return objDA.Binddropdown_Organization_DL(com_key);
        }

        //Bind Row User List
        public List<SalesTarget_CM> BindRow_Employees_BL(int id, int com_key,int FIN_YEAR)
        {
            return objDA.BindRow_Employees_DL(id,com_key, FIN_YEAR);
        }

        //Check Financial Year and Organization Exists
        public string CheckFinancialORGExists_BL(int TargetID, int FinYear, int ORG_ID)
        {
            return objDA.CheckFinancialORGExists_DL(TargetID, FinYear, ORG_ID);
        }

        //Insert / Update Sales Target
        public decimal Tbl_Sales_Target_Add(SalesTarget_CM tbl_Sales_target)
        {
            return objDA.Tbl_Sales_Target_Add(tbl_Sales_target);
        }

        //Edit Sales Organization Record Details
        public List<Tbl_SalesGroup_Target> ET_Admin_SalesTarget_Update_Get(int id)
        {
            return objDA.ET_Admin_SalesTarget_Update_Get(id);
          
        }

        //Sales Organizatioin View Record Details
        public List<Tbl_Sales_Organization> ET_Admin_SalesOrganization_View(int id)
        {
            return objDA.ET_Admin_SalesOrganization_View(id);
        }

        // Delete Record
        public int ET_Admin_SalesOrganization_Deleted_BL(int id)
        {
            return objDA.ET_Admin_SalesOrganization_Deleted_DL(id);
        }

        //View List Of Deleted Records
        public List<Tbl_Sales_Organization> ET_Admin_SalesOrganization_Restore_BL()
        {
            return objDA.ET_Admin_SalesOrganization_Restore_DL();
        }

        // Restore Deleted Record
        public decimal ET_Admin_SalesOrganization_Restore_Insert_BL(int id)
        {
            return objDA.ET_Admin_SalesOrganization_Restore_Insert_DL(id);

        }
        public int ET_Admin_SalesTarget_Deleted_BL(int id, bool type, int uid)
        {
            return objDA.ET_Admin_SalesTarget_Deleted_DL(id, type, uid);
        }




    }
}

