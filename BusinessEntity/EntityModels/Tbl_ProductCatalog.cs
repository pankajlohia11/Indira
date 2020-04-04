namespace BusinessEntity.EntityModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Tbl_ProductCatalog
    {
        //Added to Handle the Serial Number for the Order Details Print
        //Edit and View.
        //public int SO_Serial { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal PC_ID { get; set; }

        [StringLength(50)]
        public string PC_CODE { get; set; }

        public decimal PRODUCT_CATEGORY_CODE { get; set; } 

        public decimal PRODUCT_SUBCATEGORY_CODE { get; set; }

        public decimal? PRODUCT_ID { get; set; }

        [StringLength(50)]
        public string PRODUCT_CODE { get; set; }

        public string PRODUCT_SPECIFICATION { get; set; }

        [StringLength(15)]
        public string PRODUCT_UOM { get; set; }

        public decimal UNIT_PRICE1 { get; set; }

        public decimal UNIT_PRICE2 { get; set; }

        public decimal UNIT_PRICE3 { get; set; }

        public decimal UNIT_PRICE4 { get; set; }

        public DateTime LAST_PRICE_REWISE_DATE { get; set; }

        public bool? ACTIVE_STATUS { get; set; }

        public decimal? CREATED_BY { get; set; }

        public DateTime? CREATED_DATE { get; set; }

        public decimal? LAST_UPDATED_BY { get; set; }

        public DateTime? LAST_UPDATED_DATE { get; set; }

        public decimal? DELETED_BY { get; set; }

        public DateTime? DELETED_DATE { get; set; }

        public bool? DELETED { get; set; }

        public decimal? COM_KEY { get; set; }

        public decimal? Summer_Price { get; set; }

        public decimal? Winter_Price { get; set; }

    }
}
