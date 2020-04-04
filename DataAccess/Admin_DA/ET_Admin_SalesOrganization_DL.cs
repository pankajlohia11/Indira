using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessEntity.EntityModels;
/*
    Author=Manoj
    Date = 17th Apr 2018
    Sales Organization Data Access 
 */
namespace DataAccess.Admin_DA
{
    public class ET_Admin_SalesOrganization_DL
    {
        // Creating Database Object for EntityClasses
        EntityClasses dbcontext = new EntityClasses();

        //Sales Organization List View
        public List<Tbl_Sales_Organization> ET_Admin_SalesOrganizationList_DL(bool deleted, int com_key)
        {
            try
            {
                var data = dbcontext.Tbl_Sales_Organization.Where(m => m.DELETED ==deleted && m.COM_KEY==com_key).ToList();
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        //Bind Dropdown User List
        public List<Tbl_Master_User> Binddropdown_User_DL(int com_key, string type, int id,decimal orgtype)
        {
            try
            {
                dbcontext.Configuration.ProxyCreationEnabled = false;
                if (type == "Submit")
                {
                    decimal[] emps = (from org in dbcontext.Tbl_Sales_Organization where org.sales_Organization==orgtype select org.ORG_HEAD_ID).ToArray();
                   // string[] emps = (from org in dbcontext.Tbl_Sales_Organization where org.sales_Organization==orgtype select org.ORG_EMPLOYEE_IDS).ToArray();
                    decimal[] employees = new decimal[dbcontext.Tbl_Master_User.Count()];
                    int k = 0;
                    //for (int i = 0; i < emps.Length; i++)
                    //{
                    //    string[] employees1 = emps[i].Split(',').ToArray();
                    //    for (int j = 0; j < emps.Length; j++)
                    //    {
                    //        employees[k] = Convert.ToDecimal(employees1[j].ToString());
                    //        k++;
                    //    }
                    //}
                    var data = (from user in dbcontext.Tbl_Master_User
                                join org in dbcontext.Tbl_Sales_Organization on user.USER_ID equals org.ORG_HEAD_ID into t
                                from rt in t.DefaultIfEmpty()
                                where user.DELETED == false && user.COM_KEY == com_key && !(emps).Contains(user.USER_ID)
                                //where user.DELETED == false && user.COM_KEY == com_key && rt.ORG_HEAD_ID == null && !(employees).Contains(user.USER_ID)
                                select user).ToList();
                    return data;
                }
                else
                {
                    decimal[] emps = (from org in dbcontext.Tbl_Sales_Organization where org.sales_Organization == orgtype select org.ORG_HEAD_ID).ToArray();
                    //string[] emps = (from org in dbcontext.Tbl_Sales_Organization where org.sales_Organization == orgtype select org.ORG_EMPLOYEE_IDS).ToArray();
                    decimal[] employees = new decimal[dbcontext.Tbl_Master_User.Count()];
                    int k = 0;
                    //for (int i = 0; i < emps.Length; i++)
                    //{
                    //    string[] employees1 = emps[i].Split(',').ToArray();
                    //    for (int j = 0; j < employees1.Length; j++)
                    //    {
                    //        employees[k] = Convert.ToDecimal(employees1[j].ToString());
                    //        k++;
                    //    }
                    //}
                    var data = (from user in dbcontext.Tbl_Master_User
                                join org in dbcontext.Tbl_Sales_Organization on user.USER_ID equals org.ORG_HEAD_ID into t
                                from rt in t.DefaultIfEmpty()
                                where user.DELETED == false && user.COM_KEY == com_key && (rt.ORG_HEAD_ID == null || rt.ORG_ID == id) /*&& !(employees).Contains(user.USER_ID)*/
                                // where user.DELETED == false && user.COM_KEY == com_key && (rt.ORG_HEAD_ID == null || rt.ORG_ID == id) && !(employees).Contains(user.USER_ID)
                                select user).ToList();
                    return data;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Bind Dropdown Employees Based on User Selection
        public List<Tbl_Master_User> Binddropdown_Employees_DL(int pid,int com_key, string type, int id)
        {
            try
            {
                dbcontext.Configuration.ProxyCreationEnabled = false;
                if (type == "Submit")
                {

                    string[] emps = (from org in dbcontext.Tbl_Sales_Organization where org.COM_KEY == com_key select org.ORG_EMPLOYEE_IDS).ToArray();
                    decimal[] employees = new decimal[dbcontext.Tbl_Master_User.Count()];
                    int k = 0;
                    //for (int i = 0; i < emps.Length; i++)
                    //{
                    //    string[] employees1 = emps[i].Split(',').ToArray();
                    //    for (int j = 0; j < employees1.Length; j++)
                    //    {
                    //        employees[k] = Convert.ToDecimal(employees1[j].ToString());
                    //        k++;
                    //    }
                    //}
                   // employees[k] = pid;
                    var data = (from user in dbcontext.Tbl_Master_User
                                //join org in dbcontext.Tbl_Sales_Organization on user.USER_ID equals org.ORG_HEAD_ID into t
                                //from rt in t.DefaultIfEmpty()
                                where user.DELETED == false && user.COM_KEY == com_key 
                                //where user.DELETED == false && user.COM_KEY == com_key && rt.ORG_HEAD_ID == null && !(employees).Contains(user.USER_ID)
                                select user).ToList();
                    return data;
                }
                else
                {
                    string[] emps = (from org in dbcontext.Tbl_Sales_Organization where org.COM_KEY == com_key && org.ORG_ID!= id select org.ORG_EMPLOYEE_IDS).ToArray();
                    decimal[] employees = new decimal[dbcontext.Tbl_Master_User.Count()];
                    int k = 0;
                    ////for (int i = 0; i < emps.Length; i++)
                    ////{
                    ////    string[] employees1 = emps[i].Split(',').ToArray();
                    ////    for (int j = 0; j < employees1.Length; j++)
                    ////    {
                    ////        employees[k] = Convert.ToDecimal(employees1[j].ToString());
                    ////        k++;
                    ////    }
                    ////}
                    //employees[k] = pid;
                    var data = (from user in dbcontext.Tbl_Master_User
                                //join org in dbcontext.Tbl_Sales_Organization on user.USER_ID equals org.ORG_HEAD_ID into t
                                //from rt in t.DefaultIfEmpty()
                                    //where user.DELETED == false && user.COM_KEY == com_key && (rt.ORG_HEAD_ID == null || rt.ORG_ID == id) && !(employees).Contains(user.USER_ID)
                                where user.DELETED == false && user.COM_KEY == com_key
                                select user).ToList();
                    return data;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Bind Dropdown Sales Organization List
        public List<Tbl_Sales_Organization> Binddropdown_ORGParent_DL(int id, int com_key, string type)
        {
            try
            {
                dbcontext.Configuration.ProxyCreationEnabled = false;
                if (type == "Submit")
                {
                    var orglist = dbcontext.Tbl_Sales_Organization.Where(m => m.DELETED == false && m.COM_KEY == com_key).ToList();
                    return orglist;
                }
                else
                {
                    var orglist = dbcontext.Tbl_Sales_Organization.Where(m => m.DELETED == false && m.COM_KEY == com_key && m.ORG_ID!=id).ToList();
                    return orglist;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Insert / Update Sales Organization
        public decimal Tbl_Sales_Organization_Add(Tbl_Sales_Organization tbl_Sales_org, bool automanual, string prefix,out string orgcode)
        {
            Tbl_Sales_Organization tso = new Tbl_Sales_Organization();
            try
            {
                if (tbl_Sales_org.ORG_ID == 0)
                {

                    Tbl_Sales_Organization Objtso = new Tbl_Sales_Organization()
                    {
                        ORG_CODE = tbl_Sales_org.ORG_CODE,
                        ORG_NAME = tbl_Sales_org.ORG_NAME,
                        ORG_HEAD_ID = tbl_Sales_org.ORG_HEAD_ID,
                        ORG_TYPE = tbl_Sales_org.ORG_TYPE,
                        ORG_PARENT_ID = tbl_Sales_org.ORG_PARENT_ID,
                        ORG_EMPLOYEE_IDS = tbl_Sales_org.ORG_EMPLOYEE_IDS,
                        sales_Organization = tbl_Sales_org.sales_Organization,
                        CREATED_BY = tbl_Sales_org.CREATED_BY,
                        CREATED_DATE = tbl_Sales_org.CREATED_DATE,
                        DELETED = tbl_Sales_org.DELETED,
                        COM_KEY=tbl_Sales_org.COM_KEY
                    };
                    dbcontext.Tbl_Sales_Organization.Add(Objtso);
                    dbcontext.SaveChanges();
                    if (automanual == true)
                    {
                        int len = 10 - (prefix + Objtso.ORG_ID).Length;
                        string code = prefix + "0" + Objtso.ORG_ID;
                        Tbl_Sales_Organization Objtbl_Sales_org = dbcontext.Tbl_Sales_Organization.Single(m => m.ORG_ID == Objtso.ORG_ID);
                        {
                            Objtbl_Sales_org.ORG_CODE = code;
                        };
                        dbcontext.SaveChanges();
                    }
                    tso.ORG_ID = Objtso.ORG_ID;
                    orgcode = Objtso.ORG_CODE;
                }
                else
                {
                    Tbl_Sales_Organization Objtbl_Sales_org = dbcontext.Tbl_Sales_Organization.Single(m => m.ORG_ID == tbl_Sales_org.ORG_ID);
                    {
                        Objtbl_Sales_org.ORG_CODE = tbl_Sales_org.ORG_CODE;
                        Objtbl_Sales_org.ORG_NAME = tbl_Sales_org.ORG_NAME;
                        Objtbl_Sales_org.ORG_HEAD_ID = tbl_Sales_org.ORG_HEAD_ID;
                        Objtbl_Sales_org.ORG_TYPE = tbl_Sales_org.ORG_TYPE;
                        Objtbl_Sales_org.ORG_PARENT_ID = tbl_Sales_org.ORG_PARENT_ID;
                        Objtbl_Sales_org.ORG_EMPLOYEE_IDS = tbl_Sales_org.ORG_EMPLOYEE_IDS;
                        Objtbl_Sales_org.sales_Organization = tbl_Sales_org.sales_Organization;
                        Objtbl_Sales_org.LAST_UPDATED_BY = tbl_Sales_org.CREATED_BY;
                        Objtbl_Sales_org.LAST_UPDATED_DATE = tbl_Sales_org.CREATED_DATE;
                        
                    };
                    tso.ORG_ID = Objtbl_Sales_org.ORG_ID;
                    orgcode = Objtbl_Sales_org.ORG_CODE;
                    dbcontext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return tso.ORG_ID;
        }

        //Edit Sales Organization Record Details
        public Tbl_Sales_Organization ET_Admin_SalesOrganization_Update_Get(int id)
        {
            try
            {
                return dbcontext.Tbl_Sales_Organization.Single(m => m.ORG_ID == id);
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
        public int ET_Admin_SalesOrganization_Deleted_DL(int id, bool type, int uid)
        {
            int result = 0;
            //int i = 1;
            try
            {
                var find = dbcontext.Tbl_Sales_Organization.Where(m => m.ORG_ID == id).ToList();
                if (find.Count() != 0)
                {
                    Tbl_Sales_Organization delete = dbcontext.Tbl_Sales_Organization.Single(m => m.ORG_ID == id);
                    delete.DELETED = type;
                    delete.DELETED_BY = uid;
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

        //Checking Duplicate code in Usermaster
        public string CheckDuplicateCode_DA(int id, string code)
        {
            if (id == 0)
            {
                int count = dbcontext.Tbl_Sales_Organization.Where(m => m.ORG_CODE == code).Count();
                if (count > 0)
                {
                    return "exist";
                }
            }
            else
            {
                int count = dbcontext.Tbl_Sales_Organization.Where(m => m.ORG_CODE == code && m.ORG_ID != id).Count();
                if (count > 0)
                {
                    return "exist";
                }
            }

            return "";
        }

        //Checking Duplicate Username in Usermaster
        public string CheckDuplicateOrganizationName_DL(int id, string name)
        {
            if (id == 0)
            {
                int count = dbcontext.Tbl_Sales_Organization.Where(m => m.ORG_NAME == name).Count();
                if (count > 0)
                {
                    return "exist";
                }
            }
            else
            {
                int count = dbcontext.Tbl_Sales_Organization.Where(m => m.ORG_NAME == name && m.ORG_ID != id).Count();
                if (count > 0)
                {
                    return "exist";
                }
            }

            return "";
        }

    }
}
