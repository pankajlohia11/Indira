using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessEntity.CustomModels
{
    public class Tbl_Master_Products_CM
    {
        public decimal PRODUCT_ID { get; set; }

        [Required]
        [StringLength(50)]
        public string SHORT_NAME { get; set; }

        public decimal? UOM { get; set; }
        public string UOM_Text { get; set; }

        [StringLength(50)]
        public string EAN_NO { get; set; }

        public decimal? CATEGORY { get; set; }

        public decimal? CATEGORY_ID { get; set; }

        public decimal? PRODUCT { get; set; }

        [Required]
        [StringLength(500)]
        public string PRODUCT_NAME { get; set; }

        [StringLength(50)]
        public string PRODUCT_CODE { get; set; }

        [StringLength(500)]
        public string REMARKS { get; set; }

        [StringLength(300)]
        public string MACHINE_NAME { get; set; }

        [StringLength(50)]
        public string IPADDRESS { get; set; }

        public decimal? CREATED_BY { get; set; }

        public DateTime? CREATED_DATE { get; set; }

        public decimal? LAST_UPDATED_BY { get; set; }

        public DateTime? LAST_UPDATED_DATE { get; set; }

        public decimal? DELETED_BY { get; set; }

        public DateTime? DELETED_DATE { get; set; }

        public bool? DELETED { get; set; }

        public decimal? PA_CODE { get; set; }
        public decimal? COM_KEY { get; set; }
        public decimal CAT_CODE { get; set; }
        public string CAT_NAME { get; set; }
    }
}
