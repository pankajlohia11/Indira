namespace BusinessEntity.EntityModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Tbl_Stock_Details
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal STOCK_ID { get; set; }

        public decimal? PO_ID { get; set; }

        public decimal? STORE_ID { get; set; }

        public decimal PRODUCT_ID { get; set; }

        public decimal? UOM { get; set; }

        public decimal? QUANTITY { get; set; }

        public decimal? PENDING_QUANTITY { get; set; }

        public decimal? SHIP_ISSUED_QUANTITY { get; set; }
    }
}
