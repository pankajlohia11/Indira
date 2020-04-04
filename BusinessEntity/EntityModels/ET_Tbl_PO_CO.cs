namespace BusinessEntity.EntityModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ET_Tbl_PO_CO
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal ID { get; set; }

        public decimal? CO_ID { get; set; }

        public decimal? STOCK_ID { get; set; }

        public decimal? PRODUCT_ID { get; set; }

        public decimal? QUANTITY { get; set; }
    }
}
