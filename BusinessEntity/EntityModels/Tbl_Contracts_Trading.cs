namespace BusinessEntity.EntityModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Tbl_Contracts_Trading
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal TRADING_ID { get; set; }

        public decimal CUST_ID { get; set; }

        public decimal SUPPLIER_ID { get; set; }

        [StringLength(50)]
        public string DEAL_NO { get; set; }

        public DateTime? DEAL_DATE { get; set; }

        [StringLength(50)]
        public string CUST_PONO { get; set; }

        public DateTime? CUST_PODT { get; set; }

        [StringLength(50)]
        public string CUST_SCNO { get; set; }

        public DateTime? CUST_SCDT { get; set; }

        public decimal? CUST_CURRENCY { get; set; }

        public decimal? CUST_PAYMENT { get; set; }

        public decimal? CUST_PAYMENT_DAYS { get; set; }

        [StringLength(50)]
        public string CUST_DEL_ITEMS { get; set; }

        [StringLength(30)]
        public string CUST_ADV_TYPE { get; set; }

        public decimal? CUST_ADV_AMT { get; set; }

        public decimal? CUST_ADVANCE_PERCEN { get; set; }

        public decimal? CUST_ADV_DAYS { get; set; }

        [StringLength(50)]
        public string DISCOUNT_TYPE { get; set; }

        public decimal? DISCOUNT_AMT { get; set; }

        public decimal? DISCOUNT_PERCEN { get; set; }

        [StringLength(50)]
        public string SUPPLIER_PONO { get; set; }

        public DateTime? SUPPLIER_PODT { get; set; }

        [StringLength(50)]
        public string SUPPLIER_SCNO { get; set; }

        public DateTime? SUPPLIER_SCDT { get; set; }

        public decimal? SUPP_CURRENCY { get; set; }

        public decimal? SUP_PAYMENT { get; set; }

        public decimal? SUP_PAYMENT_DAYS { get; set; }

        [StringLength(50)]
        public string SUP_DELITEMS { get; set; }

        [StringLength(30)]
        public string SUPP_ADV_TYPE { get; set; }

        public decimal? SUP_ADV_AMT { get; set; }

        public decimal? SUPP_ADVANCE_PERCEN { get; set; }

        public decimal? SUP_ADV_DAYS { get; set; }

        [StringLength(50)]
        public string SUPP_DISCOUNT_TYPE { get; set; }

        public decimal? SUPP_DISCOUNT_AMT { get; set; }

        public decimal? SUPP_DISCOUNT_PERCEN { get; set; }

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

        public decimal? CUST_ADV_BAL_AMT { get; set; }

        public decimal? CUST_ADV_PAID_AMT { get; set; }

        public decimal? SUPP_ADV_BAL_AMT { get; set; }

        public decimal? SUPP_ADV_PAID_AMT { get; set; }

        public bool? CUST_ADV_STATUS { get; set; }

        public bool? SUPP_ADV_STATUS { get; set; }

        public bool? Cancelled { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Cancelled_By { get; set; }

        public DateTime? Cancelled_Date { get; set; }
    }
}
