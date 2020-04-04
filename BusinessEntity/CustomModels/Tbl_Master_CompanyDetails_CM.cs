using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.CustomModels
{
    public class Tbl_Master_CompanyDetails_CM
    {
        public string COM_CODE { get; set; }
        public decimal? COM_KEY { get; set; }
        public decimal COM_ID { get; set; }
        public string COM_USTID { get; set; }
        public decimal? COM_Sales_Person { get; set; }
        public string COM_NAME { get; set; }
        public string COM_STATE { get; set; }


        public string COM_DISPLAYNAME { get; set; }
        
        public string COM_STREET { get; set; }
        
        public string COM_CITY { get; set; }
        
        public string COM_ZIP { get; set; }
        
        public int? COM_COUNTRY { get; set; }
        public int? COM_CreditDays { get; set; }
        public string COM_COUNTRY_DISPLAY { get; set; }

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
    }
}
