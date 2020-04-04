namespace BusinessEntity.EntityModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Tbl_PayRecDetails
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal PAY_ID { get; set; }

        public decimal? SHIP_ID { get; set; }

        [Required]
        [StringLength(50)]
        public string PAYMENT_NO { get; set; }

        [Required]
        [StringLength(50)]
        public string INV_NO { get; set; }

        public DateTime INV_DATE { get; set; }

        public decimal INV_AMT { get; set; }

        public decimal PAID_AMT { get; set; }

        public decimal ADJ_AMT { get; set; }

        public decimal BAL_AMT { get; set; }

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

        [StringLength(50)]
        public string AGEN_TRAD { get; set; }
    }
}
