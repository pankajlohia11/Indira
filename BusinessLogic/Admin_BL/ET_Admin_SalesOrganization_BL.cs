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
    Sales Organization Business Logic 
 */
namespace BusinessLogic.Admin_BL
{
    public class ET_Admin_SalesOrganization_BL
    {
        // Creating Object for Sales Organization Data Access
        ET_Admin_SalesOrganization_DL objDA = new ET_Admin_SalesOrganization_DL();

        //Sales Organization List View
        public List<Tbl_Sales_Organization> ET_Admin_SalesOrganizationList_BL(bool deleted,int com_key)
        {
            return objDA.ET_Admin_SalesOrganizationList_DL(deleted,com_key);
        }

        //Bind Dropdown User List
        public List<Tbl_Master_User> Binddropdown_User_BL(int com_key, string type, int id,decimal orgtype)
        {
            return objDA.Binddropdown_User_DL(com_key, type, id, orgtype);
        }

        //Bind Dropdown Employees Based on User Selection
        public List<Tbl_Master_User> Binddropdown_Employees_BL(int pid,int com_key, string type, int id)
        {
            return objDA.Binddropdown_Employees_DL(pid, com_key, type, id);
        }

        //Bind Dropdown Sales Organization List
        public List<Tbl_Sales_Organization> Binddropdown_ORGParent_BL(int id, int com_key, string type)
        {
            return objDA.Binddropdown_ORGParent_DL(id, com_key, type);
        }

        //Insert / Update Sales Organization
        public decimal Tbl_Sales_Organization_Add(Tbl_Sales_Organization tbl_Sales_org, bool automanual, string prefix,out string orgcode)
        {
            return objDA.Tbl_Sales_Organization_Add(tbl_Sales_org, automanual, prefix,out orgcode);
        }

        //Edit Sales Organization Record Details
        public Tbl_Sales_Organization ET_Admin_SalesOrganization_Update_Get(int id)
        {
            Tbl_Sales_Organization obj = objDA.ET_Admin_SalesOrganization_Update_Get(id);
            return obj;
        }

        //Sales Organizatioin View Record Details
        public List<Tbl_Sales_Organization> ET_Admin_SalesOrganization_View(int id)
        {
            return objDA.ET_Admin_SalesOrganization_View(id);
        }

        // Delete Record
        public int ET_Admin_SalesOrganization_Deleted_BL(int id, bool type, int uid)
        {
            return objDA.ET_Admin_SalesOrganization_Deleted_DL(id,type,uid);
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

        //Checking Duplicate code in Sales Organization
        public string CheckDuplicateCode_BL(int id, string code)
        {
            return objDA.CheckDuplicateCode_DA(id, code);
        }

        //Checking Duplicate Organizationname in Sales Organization
        public string CheckDuplicateOrganizationName_BL(int id, string name)
        {
            return objDA.CheckDuplicateOrganizationName_DL(id, name);
        }

    }
}
