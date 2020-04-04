namespace BusinessEntity.EntityModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_POSchedule
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal PS_ID { get; set; }

        public decimal? PS_PID { get; set; }

        public decimal? PS_ProductID { get; set; }

        [StringLength(50)]
        public string PD_ArticleNo { get; set; }

        public DateTime? PD_Date { get; set; }

        public decimal? PD_Quantity { get; set; }
    }
}
