namespace BusinessEntity.EntityModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Tbl_Agency_Commission
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal COMM_ID { get; set; }

        public decimal SUPPLIER_ID { get; set; }

        [Required]
        [StringLength(50)]
        public string INV_NO { get; set; }

        public decimal COMM_AMT { get; set; }

        public decimal COMM_RECD { get; set; }

        public DateTime? RECD_DATE { get; set; }

        [StringLength(500)]
        public string PAYMENT_REFNO { get; set; }

        public decimal? BANK_ID { get; set; }

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

        public decimal? DEBIT_ID { get; set; }

        public virtual Tbl_Master_CompanyDetails Tbl_Master_CompanyDetails { get; set; }
    }
}
