using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessEntity.EntityModels;
using DataAccess.Admin_DA;
/*
    Author=Manoj
    Date = 16th Apr 2018
    Sales Target Business Logic 
 */
namespace BusinessLogic.Admin_BL
{
    public class ET_Admin_SalesGroup_Target_BL
    {
        // Creating Object for Sales Organization Data Access
        ET_Admin_SalesGroup_Target_DL objDA = new ET_Admin_SalesGroup_Target_DL();

        //Sales Group Target List View
        public List<Tbl_SalesGroup_Target> ET_Admin_SalesGroupTargetList_BL()
        {
            return objDA.ET_Admin_SalesGroupTargetList_DL();
        }

        //Bind Dropdown Sales Organization List
        public List<Tbl_Sales_Organization> Binddropdown_Organization_BL()
        {
            return objDA.Binddropdown_Organization_DL();
        }


        //Check Financial Year and Organization Exists
        public string CheckDuplicateTarget_BL(int GroupTargetID, int FinYear, int Salesorg)
        {
            return objDA.CheckDuplicateTarget_DL(GroupTargetID, FinYear, Salesorg);
        }

        //Insert / Update Sales Group Target
        public decimal Tbl_SalesGroup_Target_Add(Tbl_SalesGroup_Target tbl_Sales_group_target)
        {
            return objDA.Tbl_SalesGroup_Target_Add(tbl_Sales_group_target);
        }

        //Edit Sales Organization Record Details
        public List<Tbl_Sales_Target> ET_Admin_SalesTarget_Update_Get(int id)
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

     

        

    }
}
