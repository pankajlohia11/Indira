namespace BusinessEntity.EntityModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Tbl_Trading_CO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal CO_ID { get; set; }

        public decimal CUST_ID { get; set; }

        [StringLength(50)]
        public string DEAL_NO { get; set; }

        public DateTime? DEAL_DATE { get; set; }

        [Required]
        [StringLength(50)]
        public string PO_NO { get; set; }

        public DateTime PO_DATE { get; set; }

        [Required]
        [StringLength(50)]
        public string SC_NO { get; set; }

        public DateTime SC_DATE { get; set; }

        public decimal CURRENCY { get; set; }

        [StringLength(50)]
        public string DEL_ITEMS { get; set; }

        public decimal? PAYMENT { get; set; }

        public decimal? PAYMENT_DAYS { get; set; }

        [StringLength(30)]
        public string ADV_TYPE { get; set; }

        public decimal? ADV_AMT { get; set; }

        public decimal? ADVANCE_PERCEN { get; set; }

        public decimal? ADV_DAYS { get; set; }

        [StringLength(50)]
        public string DISCOUNT_TYPE { get; set; }

        public decimal? DISCOUNT_AMT { get; set; }

        public decimal? DISCOUNT_PERCEN { get; set; }

        [StringLength(500)]
        public string REMARKS { get; set; }

        public decimal? SALES_GROUP { get; set; }

        public decimal? SALES_PERSON { get; set; }

        public int? ORDER_STATUS { get; set; }

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

        public decimal? ADV_BAL_AMT { get; set; }

        public decimal? ADV_PAID_AMT { get; set; }

        public bool ADV_STATUS { get; set; }

        public bool? Cancelled { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Cancelled_By { get; set; }

        public DateTime? Cancelled_Date { get; set; }

        public virtual Tbl_Master_CompanyDetails Tbl_Master_CompanyDetails { get; set; }

        public virtual Tbl_Master_User Tbl_Master_User { get; set; }
    }
}
