namespace BusinessEntity.EntityModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Tbl_Master_Enquiry
    {
        [Key]
        [Column(Order = 0)]
        public decimal ENQ_ID { get; set; }

        [Key]
        [Column(Order = 1)]
        public decimal COM_ID { get; set; }

        [Key]
        [Column(Order = 2)]
        public decimal ENQUIRY_STATUS { get; set; }

        public decimal? SALES_ID { get; set; }

        [StringLength(50)]
        public string OTHERS { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(150)]
        public string ENQUIRY_TITLE { get; set; }

        [StringLength(500)]
        public string ENQUIRY_DES { get; set; }

        public DateTime? ENQUIRY_DATE { get; set; }

        public decimal? PRODUCT_ID { get; set; }

        [StringLength(150)]
        public string CONTACT_PERSON { get; set; }

        [StringLength(150)]
        public string COSTING { get; set; }

        public bool? CONVERT_TOORDER { get; set; }

        public int? CONVERT_TYPE { get; set; }

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

        public virtual Tbl_Master_CompanyDetails Tbl_Master_CompanyDetails { get; set; }
    }
}
