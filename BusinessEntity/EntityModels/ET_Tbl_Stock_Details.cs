namespace BusinessEntity.EntityModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ET_Tbl_Stock_Details
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal STOCK_ID { get; set; }

        [Key]
        [Column(Order = 1)]
        public decimal STORE_ID { get; set; }

        [Key]
        [Column(Order = 2)]
        public decimal PRODUCT_ID { get; set; }

        [Key]
        [Column(Order = 3)]
        public DateTime DATE { get; set; }

        public decimal? OPEN_QTY { get; set; }

        public decimal? RECEIVED_QTY { get; set; }

        public decimal? ISSUED_QTY { get; set; }

        public decimal? CLOSED_QTY { get; set; }
    }
}
