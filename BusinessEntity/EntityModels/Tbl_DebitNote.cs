namespace BusinessEntity.EntityModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Tbl_DebitNote
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal DEBIT_ID { get; set; }

        [StringLength(50)]
        public string DEBIT_NO { get; set; }

        public DateTime? DEBIT_DATE { get; set; }

        public decimal SUPPLIER_ID { get; set; }

        [StringLength(50)]
        public string DEAL_NO { get; set; }

        public decimal? CUST_ID { get; set; }

        [StringLength(50)]
        public string SC_NO { get; set; }

        [StringLength(50)]
        public string INV_NO { get; set; }

        public DateTime? INV_DATE { get; set; }

        public decimal? INV_AMT { get; set; }

        public decimal? RECD_AMT { get; set; }

        public decimal? COMM { get; set; }

        public decimal COMM_AMT { get; set; }

        public decimal? BAL_AMT { get; set; }

        [StringLength(1000)]
        public string FILE_PATH { get; set; }

        public bool? COMM_STATUS { get; set; }

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

        public decimal? SHIP_ID { get; set; }

        public virtual Tbl_Master_CompanyDetails Tbl_Master_CompanyDetails { get; set; }
    }
}
