namespace BusinessEntity.EntityModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_ItemTransfersDetails
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal TD_ID { get; set; }

        public decimal? TD_PID { get; set; }

        public decimal? TD_ProductID { get; set; }

        [StringLength(45)]
        public string GD_ArticleNo { get; set; }

        [StringLength(45)]
        public string GD_UOM { get; set; }

        public decimal? GD_TransferQty { get; set; }

        public decimal? GD_TransferAvailableQty { get; set; }
    }
}
