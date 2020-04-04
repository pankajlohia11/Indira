using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessEntity.EntityModels;

namespace BusinessEntity.CustomModels
{
    public class CompanyModule__CM
    {
        public List<Tbl_Master_CompanyDetails> CustomerMaster { get; set; }
        public List<Tbl_Master_CompanyDetails> CustomerMasterList { get; set; }
        public List<Tbl_Order_Details> OrderDetails { get; set; }
        public List<Tbl_Master_CompanyContacts> CompanyContacts { get; set; }
    }
}
