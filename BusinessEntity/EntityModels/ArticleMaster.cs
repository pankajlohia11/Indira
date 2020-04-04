namespace BusinessEntity.EntityModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ArticleMaster")]
    public partial class ArticleMaster
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal AM_ID { get; set; }

        [Required]
        public string AM_Name { get; set; }

        [Required]
        public string AM_WarpSpecification { get; set; }

        [Required]
        public string AM_WeftSpecification { get; set; }

        public decimal AM_LoomSpeed { get; set; }

        public decimal AM_Efficiency { get; set; }

        public decimal AM_RunningHoursPerDay { get; set; }

        public decimal? AM_NoOfBeams { get; set; }

        public decimal? AM_NoOfEndsPerWarpSet { get; set; }

        public decimal? AM_WarpConeWeight { get; set; }

        public string AM_RejectionCriteria { get; set; }

        public string AM_MeterageLimitation { get; set; }

        public decimal? AM_WeftConeWeight { get; set; }

        public decimal? AM_WarpCount1 { get; set; }

        public decimal? AM_WarpCount2 { get; set; }

        public decimal? AM_WeftCount1 { get; set; }

        public decimal? AM_WeftCount2 { get; set; }

        public decimal? AM_EndsInSelvedge { get; set; }

        public decimal? AM_WarpEnds { get; set; }

        public decimal? AM_WeftPicks { get; set; }

        public decimal? AM_WidthOfFabric { get; set; }

        public decimal? AM_WarpCrimpStandard { get; set; }

        public decimal? AM_WeftCrimpStandard { get; set; }

        public decimal? AM_WarpWaste { get; set; }

        public decimal? AM_WeftWaste { get; set; }

        public bool? Deleted { get; set; }

        [StringLength(50)]
        public string AM_ArticleNo { get; set; }
    }
}
