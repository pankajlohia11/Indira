using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.CustomModels
{
    class Tbl_Master_CompanyContacts_CM
    {
        public decimal COM_ID { get; set; }
        
        public string COM_NAME { get; set; }
        
        public string COM_DISPLAYNAME { get; set; }
        
        public string COM_STREET { get; set; }
        
        public string COM_CITY { get; set; }
        
        public string COM_ZIP { get; set; }
        
        public string COM_COUNTRY { get; set; }
        
        public string COM_EMAIL { get; set; }
        
        public string COM_FAX { get; set; }
        
        public string COM_PHONE { get; set; }
        
        public string COM_MOBILE { get; set; }
        
        public string COM_WEB { get; set; }
        
        public string COM_REMARKS { get; set; }

        public int? Cust_Supp { get; set; }
        
        public string Company_ID { get; set; }
        
        public string Tax_No { get; set; }
        
        public string MACHINE_NAME { get; set; }
        
        public string IPADDRESS { get; set; }

        public decimal? CREATED_BY { get; set; }

        public DateTime? CREATED_DATE { get; set; }

        public decimal? LAST_UPDATED_BY { get; set; }

        public DateTime? LAST_UPDATED_DATE { get; set; }

        public decimal? DELETED_BY { get; set; }

        public DateTime? DELETED_DATE { get; set; }

        public bool? DELETED { get; set; }
        
        public string COM_STATE { get; set; }

        public decimal CONTACT_ID { get; set; }
        
        public string TITLE { get; set; }
        
        public string FIRST_NAME { get; set; }
        
        public string LAST_NAME { get; set; }
        
        public string JOB_TITLE { get; set; }
        
        public string PHONE { get; set; }
        
        public string FAX { get; set; }
        
        public string MOBILE { get; set; }
        
        public string EMAIL { get; set; }
        
        public string REMARKS { get; set; }

    }
}
