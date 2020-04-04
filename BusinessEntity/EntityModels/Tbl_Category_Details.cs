namespace BusinessEntity.EntityModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Tbl_Category_Details
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal CAT_DETAILS_ID { get; set; }

        [Required]
        [StringLength(100)]
        public string CATEGORY { get; set; }

        [Required]
        [StringLength(100)]
        public string SUB_CATEGORY { get; set; }

        [Required]
        [StringLength(100)]
        public string DESCRIPTION { get; set; }

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
        public string CATEGORY_CODE { get; set; }

        public decimal? COM_KEY { get; set; }
    }
}
