namespace BusinessEntity.EntityModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_QuotationDetails
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal QD_ID { get; set; }

        public decimal QD_PID { get; set; }

        public decimal QD_ProductID { get; set; }

        [Required]
        [StringLength(50)]
        public string QD_UOM { get; set; }

        public decimal? QD_Unit_Price { get; set; }

        public decimal? QD_Quantity { get; set; }

        public decimal? QD_Amount { get; set; }

        public string QD_Description { get; set; }

        public int QD_CatalogPriceType { get; set; }
    }
}
