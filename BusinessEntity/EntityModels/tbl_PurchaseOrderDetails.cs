namespace BusinessEntity.EntityModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_PurchaseOrderDetails
    {
        public decimal? PO_OrderDetailID { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal PD_ID { get; set; }

        public decimal? PD_PID { get; set; }

        public decimal? PD_ProductID { get; set; }

        [StringLength(50)]
        public string PD_ArticleNo { get; set; }

        [StringLength(50)]
        public string PD_UOM { get; set; }

        public decimal? PD_Quantity { get; set; }

        public decimal? PD_UnitPrice { get; set; }

        public decimal? PD_TotalAmount { get; set; }

        public DateTime? PD_DeliveryDate { get; set; }

        public string SupplierDes { get; set; }

        /// <summary>
        /// Design Detail added to Show the Design Details specific to each Purchase Order detail.(7.3.2019)
        /// </summary>
        public string DesignDetail { get; set; }
    }
}
