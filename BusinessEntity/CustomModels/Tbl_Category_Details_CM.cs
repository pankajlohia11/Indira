using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.CustomModels
{
    public class Tbl_Category_Details_CM
    {
        public decimal CAT_DETAILS_ID { get; set; }

        public string CATEGORY { get; set; }

        public string SUB_CATEGORY { get; set; }

        public string DESCRIPTION { get; set; }

        public string MACHINE_NAME { get; set; }

        public string IPADDRESS { get; set; }

        public decimal? CREATED_BY { get; set; }

        public DateTime? CREATED_DATE { get; set; }

        public decimal? LAST_UPDATED_BY { get; set; }

        public DateTime? LAST_UPDATED_DATE { get; set; }

        public decimal? DELETED_BY { get; set; }

        public DateTime? DELETED_DATE { get; set; }

        public bool? DELETED { get; set; }

        public string CATEGORY_CODE { get; set; }
    }
}
