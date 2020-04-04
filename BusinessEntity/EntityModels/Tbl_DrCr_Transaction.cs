namespace BusinessEntity.EntityModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Tbl_DrCr_Transaction
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal ID { get; set; }

        [StringLength(50)]
        public string REF_NO { get; set; }

        public DateTime? DATE { get; set; }

        public decimal? AMOUNT { get; set; }

        [StringLength(2)]
        public string DRCR { get; set; }

        public decimal? RECEIPT_AMT { get; set; }

        public decimal? BALANCE { get; set; }

        [StringLength(100)]
        public string ACCOUNT_NAME { get; set; }

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

        public decimal? CURRENCY_ID { get; set; }

        [StringLength(300)]
        public string PARTICULARS { get; set; }
    }
}
