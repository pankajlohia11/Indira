namespace BusinessEntity.EntityModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_GoodsInwardDetail
    {
        [Key]
        public int GD_ID { get; set; }

        public decimal? GD_PID { get; set; }

        public decimal? GD_ProductID { get; set; }

        [StringLength(45)]
        public string GD_ArticleNo { get; set; }

        [StringLength(45)]
        public string GD_UOM { get; set; }

        [StringLength(45)]
        public string GD_LotNo { get; set; }

        [StringLength(20)]
        public string GD_ExpiryDate { get; set; }

        public decimal? GD_POQuantity { get; set; }

        public decimal? GD_GIQuantity { get; set; }

        [StringLength(20)]
        public string GD_RejectedQuantity { get; set; }

        public decimal? GD_BalanceQty { get; set; }

        public decimal? GD_ChallanQty { get; set; }

        public decimal? GD_UnitPrice { get; set; }

        [StringLength(20)]
        public string GD_GIValue { get; set; }
    }
}
