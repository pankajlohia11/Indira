namespace BusinessEntity.EntityModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Tbl_SalesProduct_Target
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal PGT_ID { get; set; }

        public decimal? SPT_ID { get; set; }

        public decimal? PGT_PRODUCT_GROUP_ID { get; set; }

        public decimal? M1 { get; set; }

        public decimal? M2 { get; set; }

        public decimal? M3 { get; set; }

        public decimal? M4 { get; set; }

        public decimal? M5 { get; set; }

        public decimal? M6 { get; set; }

        public decimal? M7 { get; set; }

        public decimal? M8 { get; set; }

        public decimal? M9 { get; set; }

        public decimal? M10 { get; set; }

        public decimal? M11 { get; set; }

        public decimal? M12 { get; set; }

        public decimal? CREATED_BY { get; set; }

        public DateTime? CREATED_DATE { get; set; }

        public decimal? LAST_UPDATED_BY { get; set; }

        public DateTime? LAST_UPDATED_DATE { get; set; }

        public decimal? DELETED_BY { get; set; }

        public DateTime? DELETED_DATE { get; set; }

        public bool? DELETED { get; set; }
    }
}
