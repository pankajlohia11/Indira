namespace BusinessEntity.EntityModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Tbl_ProductGroup
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal PG_ID { get; set; }

        [StringLength(50)]
        public string PG_CODE { get; set; }

        [Required]
        [StringLength(50)]
        public string PG_NAME { get; set; }

        public decimal PG_TYPE { get; set; }

        public decimal? PG_PARENT_ID { get; set; }

        public decimal? CREATED_BY { get; set; }

        public DateTime? CREATED_DATE { get; set; }

        public decimal? LAST_UPDATED_BY { get; set; }

        public DateTime? LAST_UPDATED_DATE { get; set; }

        public decimal? DELETED_BY { get; set; }

        public DateTime? DELETED_DATE { get; set; }

        public bool? DELETED { get; set; }

        public decimal? COM_KEY { get; set; }

        [StringLength(2)]
        public string PG_Initial { get; set; }
    }
}
