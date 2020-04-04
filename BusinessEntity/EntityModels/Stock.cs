namespace BusinessEntity.EntityModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Stock")]
    public partial class Stock
    {
        [Key]
        [Column(Order = 0)]
        public decimal PRODUCT_ID { get; set; }

        [Key]
        [Column(Order = 1)]
        public DateTime DATE { get; set; }

        [Key]
        [Column(Order = 2)]
        public decimal Store_ID { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(50)]
        public string Display_Name { get; set; }

        public decimal? OPEN_QTY { get; set; }
    }
}
