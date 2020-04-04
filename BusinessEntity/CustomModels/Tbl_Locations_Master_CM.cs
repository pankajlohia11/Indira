using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.CustomModels
{
    public class Tbl_Locations_Master_CM
    {
        public decimal Store_ID { get; set; }

        public string Store_Name { get; set; }

        public string Display_Name { get; set; }

        public string Store_KeeperName { get; set; }

        public string Street { get; set; }

        public string City { get; set; }

        public string Zip { get; set; }

        public string Country { get; set; }

        public string ContactNo { get; set; }

        public string Machine_Name { get; set; }

        public string IP_Address { get; set; }

        public decimal? Created_By { get; set; }

        public DateTime? Created_Date { get; set; }

        public decimal? LAST_UPDATED_BY { get; set; }

        public DateTime? LAST_UPDATED_DATE { get; set; }

        public decimal? DELETED_BY { get; set; }

        public DateTime? DELETED_DATE { get; set; }

        public bool? Deleted { get; set; }

        public string Store_Code { get; set; }

        public string State { get; set; }
    }
}
