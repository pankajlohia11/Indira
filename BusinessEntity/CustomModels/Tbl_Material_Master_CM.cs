using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.CustomModels
{
    public class Tbl_Material_Master_CM
    {
        public decimal MATERIAL_ID { get; set; }
        
        public string MATERIAL_NAME { get; set; }
        
        public string MATERIAL_DESCRIPTION { get; set; }

        public int? COTTON_PER { get; set; }
        
        public string MACHINE_NAME { get; set; }
        
        public string IPADDRESS { get; set; }

        public decimal? CREATED_BY { get; set; }

        public DateTime? CREATED_DATE { get; set; }

        public decimal? LAST_UPDATED_BY { get; set; }

        public DateTime? LAST_UPDATED_DATE { get; set; }

        public decimal? DELETED_BY { get; set; }

        public DateTime? DELETED_DATE { get; set; }

        public bool? DELETED { get; set; }

        public string MATERIAL_CODE { get; set; }
    }
}
