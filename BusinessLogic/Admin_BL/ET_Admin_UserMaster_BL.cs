using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessEntity.EntityModels;
using DataAccess.Admin_DA;
/*
    Author=Manoj
    Date = 12th Apr 2018
    User Master Business Logic 
 */
namespace BusinessLogic.Admin_BL
{
    public class ET_Admin_UserMaster_BL
    {
        // Creating Object for User Master Data Access
        ET_Admin_UserMaster_DL objDA = new ET_Admin_UserMaster_DL();

        //User Master List
        public List<Tbl_Master_User> ET_Admin_UserList_BL(bool delete,int com_key)
        {
            return objDA.ET_Admin_UserList_DL(delete, com_key);
        }

        //Bind Dropdown Roles
        public List<Tbl_Master_Role> Binddropdown_Role_BL(int companykey)
        {
            return objDA.Binddropdown_Role_DL(companykey);
        }

        // Insert/Update User Master
        public decimal Tbl_Master_User_Add(Tbl_Master_User _tbl_Master_User, bool automanual, string prefix, out string usercode)
        {
            return objDA.Tbl_Master_User_Add(_tbl_Master_User, automanual, prefix,out usercode);
        }

        //Get User Master Record while Edit
        public Tbl_Master_User ET_Admin_User_Update_Get(int id)
        {
            Tbl_Master_User obj= objDA.ET_Admin_User_Update_Get(id);
            obj.USER_PASSWORD =new  BALCrypto().Decrypting(obj.USER_PASSWORD, "12345");
            return obj;
        }

        //User Master Record View
        public List<Tbl_Master_User> ET_Admin_User_View(int id)
        {
            return objDA.ET_Admin_User_View(id);
        }

        // Delete User Master Record
        public int ET_Admin_User_Deleted_BL(int id)
        {
            return objDA.ET_Admin_Users_Deleted_DL(id);
        }

        //Restore Page Deleted records List
        public List<Tbl_Master_User> ET_Admin_User_Restore_BL()
        {
            return objDA.ET_Admin_User_Restore_DL();
        }

        //Restore User Master Record
        public decimal ET_Admin_User_Restore_Insert_BL(int id, bool type,int uid)
        {
            return objDA.ET_Admin_User_Restore_Insert_DL(id, type,uid);

        }

        //Checking Duplicate code in Usermaster
        public string CheckDuplicateCode_BL(int id, string code)
        {
            return objDA.CheckDuplicateCode_DA(id, code);
        }

        //Checking Duplicate Username in Usermaster
        public string CheckDuplicateUserName_BL(int id,string name)
        {
            return objDA.CheckDuplicateUserName_DL(id, name);
        }

    }
}
