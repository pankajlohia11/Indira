using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessEntity.CustomModels;
using BusinessEntity.EntityModels;

namespace DataAccess.Admin_DA
{
    public class ET_Admin_CustomerModule_DA
    {
        EntityClasses dbcontext = new EntityClasses();

        public List<Tbl_Master_CompanyDetails> GetCustomers_DA()
        {
            dbcontext.Configuration.ProxyCreationEnabled = false;
            return dbcontext.Tbl_Master_CompanyDetails.Where(m => m.Cust_Supp != 1).ToList();
        }
        public List<Tbl_Master_CompanyDetails> GetSupplier_DA()
        {
            dbcontext.Configuration.ProxyCreationEnabled = false;
            return dbcontext.Tbl_Master_CompanyDetails.Where(m => m.Cust_Supp != 0).ToList();
        }
        public List<Tbl_Master_CompanyDetails> GetCustomerDetails_DA(decimal id)
        {
            return dbcontext.Tbl_Master_CompanyDetails.Where(m => m.COM_ID == id).ToList();
        }
        public List<Tbl_Master_CompanyContacts> GetContactDetails_DA(decimal id)
        {
            return dbcontext.Tbl_Master_CompanyContacts.Where(m => m.COM_ID == id).ToList();
        }
        public List<Tbl_Order_Details> GetOrderDetails_DA(decimal id)
        {
            return dbcontext.Tbl_Order_Details.Where(m => m.ORDER_ID == id).ToList();
        }

    }
}
