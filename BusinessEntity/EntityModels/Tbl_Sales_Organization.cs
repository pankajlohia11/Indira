namespace BusinessEntity.EntityModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Tbl_Sales_Organization
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal ORG_ID { get; set; }

        [StringLength(50)]
        public string ORG_CODE { get; set; }

        [Required]
        [StringLength(50)]
        public string ORG_NAME { get; set; }

        public decimal ORG_HEAD_ID { get; set; }

        public decimal ORG_TYPE { get; set; }

        public decimal? ORG_PARENT_ID { get; set; }

        [Required]
        public string ORG_EMPLOYEE_IDS { get; set; }

        public decimal? CREATED_BY { get; set; }

        public DateTime? CREATED_DATE { get; set; }

        public decimal? LAST_UPDATED_BY { get; set; }

        public DateTime? LAST_UPDATED_DATE { get; set; }

        public decimal? DELETED_BY { get; set; }

        public DateTime? DELETED_DATE { get; set; }

        public bool? DELETED { get; set; }

        public decimal? COM_KEY { get; set; }

        public decimal? sales_Organization { get; set; }
    }
}
