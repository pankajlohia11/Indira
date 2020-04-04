namespace BusinessEntity.EntityModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Tbl_Master_BankAccount
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal BANK_ID { get; set; }

        public decimal COM_ID { get; set; }

        [Required]
        [StringLength(150)]
        public string BANK_NAME { get; set; }

        [StringLength(100)]
        public string DISPLAY_NAME { get; set; }

        [StringLength(500)]
        public string STREET { get; set; }

        [StringLength(50)]
        public string CITY { get; set; }

        [StringLength(20)]
        public string ZIP { get; set; }

        [StringLength(50)]
        public string COUNTRY { get; set; }

        [StringLength(50)]
        public string SWIFT { get; set; }

        [StringLength(50)]
        public string BLZ { get; set; }

        [StringLength(50)]
        public string BIC { get; set; }

        [StringLength(50)]
        public string IBAN { get; set; }

        [StringLength(50)]
        public string ACCOUNT { get; set; }

        [StringLength(50)]
        public string CONTACT_NAME { get; set; }

        [StringLength(20)]
        public string CONTACT_NO { get; set; }

        [StringLength(500)]
        public string REMARKS { get; set; }

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
        public string STATE { get; set; }

        public decimal? COM_KEY { get; set; }
    }
}
