namespace BusinessEntity.EntityModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Tbl_SupplierPriceListDetails
    {
        [Key]
        public int SL_ID { get; set; }

        public decimal? SLD_ID { get; set; }

        public decimal? SLD_ProductID { get; set; }

        public string SLD_ArticleNo { get; set; }

        public decimal? SLD_UnitPrice { get; set; }

        [StringLength(50)]
        public string SLD_UOM { get; set; }

        public bool SLD_Status { get; set; }
    }
}
