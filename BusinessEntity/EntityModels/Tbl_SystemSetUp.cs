namespace BusinessEntity.EntityModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Tbl_SystemSetUp
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal COMPANY_ID { get; set; }

        public string COMPANY_NAME { get; set; }

        public string COMPANY_ADDRESS_LINE_1 { get; set; }

        public string COMPANY_ADDRESS_LINE_2 { get; set; }

        [StringLength(15)]
        public string COMPANY_COUNTRY { get; set; }

        public string COMPANY_STATE { get; set; }

        public string COMPANT_CITY { get; set; }

        [StringLength(10)]
        public string COMPANY_ZIP_CODE { get; set; }

        [StringLength(15)]
        public string COMPANY_VAT_NO { get; set; }

        [StringLength(50)]
        public string COMPANY_LOGO { get; set; }

        [StringLength(15)]
        public string DATEFORMAT { get; set; }

        public int? PASSWORDEXPIRYDAYS { get; set; }

        [StringLength(20)]
        public string ADMINISTRATORPASSWORD { get; set; }

        [StringLength(50)]
        public string SMTPHOSTNAME { get; set; }

        [StringLength(50)]
        public string EMAILID { get; set; }

        [StringLength(20)]
        public string EMAILPASSWORD { get; set; }

        public decimal? TAX { get; set; }

        public int? DOCUMENTDAYS { get; set; }

        public decimal? CREATED_BY { get; set; }

        public DateTime? CREATED_DATE { get; set; }

        public decimal? LAST_UPDATED_BY { get; set; }

        public DateTime? LAST_UPDATED_DATE { get; set; }

        public decimal? DELETED_BY { get; set; }

        public DateTime? DELETED_DATE { get; set; }

        public bool? DELETED { get; set; }

        public decimal? COM_KEY { get; set; }
    }
}
