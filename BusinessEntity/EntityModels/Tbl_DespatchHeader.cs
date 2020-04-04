namespace BusinessEntity.EntityModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Tbl_DespatchHeader
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal D_ID { get; set; }

        [StringLength(50)]
        public string D_Code { get; set; }

        [Required]
        public string D_OrderID { get; set; }

        public decimal D_CustomerID { get; set; }

        public decimal D_SalesPerson { get; set; }

        public decimal D_StoreID { get; set; }

        [Column(TypeName = "date")]
        public DateTime D_DespatchDate { get; set; }

        [StringLength(10)]
        public string D_ModeOfTransport { get; set; }

        [StringLength(50)]
        public string D_VechileNo { get; set; }

        [StringLength(100)]
        public string D_TransporterName { get; set; }

        [StringLength(100)]
        public string D_DeliveryFrom { get; set; }

        [StringLength(100)]
        public string D_DeliveryTo { get; set; }

        public string D_Remarks { get; set; }

        public decimal? CREATED_BY { get; set; }

        public DateTime? CREATED_DATE { get; set; }

        public decimal? LAST_UPDATED_BY { get; set; }

        public DateTime? LAST_UPDATED_DATE { get; set; }

        public decimal? DELETED_BY { get; set; }

        public DateTime? DELETED_DATE { get; set; }

        public bool? DELETED { get; set; }

        public int COM_KEY { get; set; }

        public int? D_Status { get; set; }
    }
}
