namespace BusinessEntity.EntityModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Tbl_DespatchDetails
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal DD_ID { get; set; }

        public decimal DD_PID { get; set; }

        public decimal DD_ProductID { get; set; }

        [Required]
        [StringLength(20)]
        public string DD_UOM { get; set; }

        public decimal DD_OrderQuantity { get; set; }

        public decimal? DD_DespatchQuantity { get; set; }

        public decimal? DD_OrderID { get; set; }

        [StringLength(50)]
        public string DD_OrderCode { get; set; }

        public decimal? DD_OrderDetailID { get; set; }
    }
}
