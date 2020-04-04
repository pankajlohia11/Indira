using BusinessEntity.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Admin_DA;

namespace BusinessLogic.Admin_BL
{
    public class ET_Admin_CustomerModule_BL
    {
        ET_Admin_CustomerModule_DA objDA = new ET_Admin_CustomerModule_DA();
        public List<Tbl_Master_CompanyDetails> GetCustomerDetails_BL(decimal id)
        {
            return objDA.GetCustomerDetails_DA(id);
        }
        public List<Tbl_Master_CompanyContacts> GetContactDetails_BL(decimal id)
        {
            return objDA.GetContactDetails_DA(id);
        }
            public List<Tbl_Master_CompanyDetails> GetCustomers_BL()
        {
            return objDA.GetCustomers_DA();
        }
        public List<Tbl_Master_CompanyDetails> GetSupplier_BL()
        {
            return objDA.GetSupplier_DA();
        }
    }
}
