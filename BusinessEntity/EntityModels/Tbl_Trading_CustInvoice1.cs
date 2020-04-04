namespace BusinessEntity.EntityModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Tbl_Trading_CustInvoice1
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Tbl_Trading_CustInvoice1()
        {
            Tbl_Invoice_Details = new HashSet<Tbl_Invoice_Details>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal INVOICE_ID { get; set; }

        [Required]
        [StringLength(50)]
        public string INV_NO { get; set; }

        public DateTime? INV_DATE { get; set; }

        public decimal CUST_ID { get; set; }

        public decimal? TAX { get; set; }

        public decimal? TAX_AMT { get; set; }

        public decimal? OTHER_CHARGES { get; set; }

        public decimal? DEDUCTION { get; set; }

        public decimal INV_AMT { get; set; }

        public decimal NET_AMT { get; set; }

        public decimal? BAL_AMT { get; set; }

        [StringLength(500)]
        public string DESCRIPTION { get; set; }

        [StringLength(500)]
        public string REPORT_DES { get; set; }

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

        public decimal? PAID_AMT { get; set; }

        [StringLength(50)]
        public string INV_TYPE { get; set; }

        public decimal? LC_ID { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_Invoice_Details> Tbl_Invoice_Details { get; set; }
    }
}
