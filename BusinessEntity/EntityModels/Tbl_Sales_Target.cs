namespace BusinessEntity.EntityModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Tbl_Sales_Target
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal ST_ID { get; set; }

        public decimal? GroupTarget_ID { get; set; }

        public decimal ST_FINANCIAL_YEAR { get; set; }

        public decimal ORG_ID { get; set; }

        public decimal ST_EMP_ID { get; set; }

        public decimal ST_ProductGroupID { get; set; }

        [Required]
        public string ST_ProductGroupName { get; set; }

        public decimal? ST_M1 { get; set; }

        public decimal? ST_M2 { get; set; }

        public decimal? ST_M3 { get; set; }

        public decimal? ST_M4 { get; set; }

        public decimal? ST_M5 { get; set; }

        public decimal? ST_M6 { get; set; }

        public decimal? ST_M7 { get; set; }

        public decimal? ST_M8 { get; set; }

        public decimal? ST_M9 { get; set; }

        public decimal? ST_M10 { get; set; }

        public decimal? ST_M11 { get; set; }

        public decimal? ST_M12 { get; set; }

        public decimal? ST_TARGET { get; set; }

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
