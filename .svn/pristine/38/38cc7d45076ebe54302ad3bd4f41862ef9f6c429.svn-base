namespace BusinessEntity.EntityModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Tbl_Offers_Master
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal OFFER_ID { get; set; }

        public decimal COM_ID { get; set; }

        public bool? IS_SPECIFIC { get; set; }

        public decimal? CUST_ID { get; set; }

        [StringLength(50)]
        public string OFFER_NO { get; set; }

        public DateTime OFFER_DATE { get; set; }

        [StringLength(500)]
        public string DESCRIPTION { get; set; }

        [StringLength(100)]
        public string PLACE_DEST { get; set; }

        [StringLength(100)]
        public string DESTINATION { get; set; }

        public decimal CONTACT_PERSON { get; set; }

        [StringLength(500)]
        public string PAYMENT { get; set; }

        [StringLength(500)]
        public string SHIPMENT { get; set; }

        public decimal VALIDITY_DAYS { get; set; }

        public DateTime VALIDITY_DATE { get; set; }

        [StringLength(50)]
        public string CUST_SUPP { get; set; }

        public bool? IS_ORDER { get; set; }

        public int? AGENCY_TRADING { get; set; }

        [StringLength(50)]
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
        public string OFFER_CODE { get; set; }

        public decimal? COM_KEY { get; set; }

        public decimal? PaymentTerms { get; set; }
    }
}
