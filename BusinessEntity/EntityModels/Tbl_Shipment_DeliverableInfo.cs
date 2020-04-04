namespace BusinessEntity.EntityModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Tbl_Shipment_DeliverableInfo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal DEL_ID { get; set; }

        [Required]
        [StringLength(50)]
        public string AGEN_TRADE { get; set; }

        public decimal SHEDU_ID { get; set; }

        public decimal PRODUCT_ID { get; set; }

        public decimal? UOM { get; set; }

        public decimal? SHIP_UNDEFINED { get; set; }

        public decimal? SHIP_QUAN { get; set; }

        public decimal? SHIP_ISSUED_QUANTITY { get; set; }

        public decimal? RETURN_QUANTITY { get; set; }

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

        public virtual Tbl_Shipment_Schedule Tbl_Shipment_Schedule { get; set; }
    }
}
