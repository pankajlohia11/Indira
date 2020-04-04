namespace BusinessEntity.EntityModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Tbl__Master_CompanyBank
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal B_ID { get; set; }

        public decimal B_PID { get; set; }

        [Required]
        public string B_NAME { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string B_AccountNo { get; set; }

        [StringLength(50)]
        public string SWIFT { get; set; }

        [StringLength(50)]
        public string BLZ { get; set; }

        [StringLength(50)]
        public string BIC { get; set; }

        [StringLength(50)]
        public string IBAN { get; set; }

        public decimal? CREATED_BY { get; set; }

        public DateTime? CREATED_DATE { get; set; }

        public decimal? LAST_UPDATED_BY { get; set; }

        public DateTime? LAST_UPDATED_DATE { get; set; }

        public decimal? DELETED_BY { get; set; }

        public DateTime? DELETED_DATE { get; set; }

        public bool? DELETED { get; set; }

        public int? COMPANY_KEY { get; set; }

        public decimal? COM_KEY { get; set; }
    }
}
