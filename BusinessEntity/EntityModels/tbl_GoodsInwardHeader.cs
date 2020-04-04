namespace BusinessEntity.EntityModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_GoodsInwardHeader
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal GI_ID { get; set; }

        [StringLength(45)]
        public string GI_Code { get; set; }

        public DateTime? GI_Date { get; set; }

        [StringLength(45)]
        public string GI_Type { get; set; }

        public decimal GI_StoreCode { get; set; }

        [StringLength(45)]
        public string GI_TransferNo { get; set; }

        [StringLength(200)]
        public string GI_SupplierCode { get; set; }

        [StringLength(45)]
        public string GI_ChallanNo { get; set; }

        [StringLength(45)]
        public string GI_ReturnGINo { get; set; }

        public decimal? GI_POCode { get; set; }

        public DateTime? GI_PODate { get; set; }

        [StringLength(45)]
        public string GI_TransferStoreCode { get; set; }

        [StringLength(200)]
        public string GI_ReceivedBy { get; set; }

        public decimal? GI_Approver { get; set; }

        public decimal? CREATED_BY { get; set; }

        public DateTime? CREATED_DATE { get; set; }

        public decimal? LAST_UPDATED_BY { get; set; }

        public DateTime? LAST_UPDATED_DATE { get; set; }

        public bool? DELETED { get; set; }

        public decimal? COM_KEY { get; set; }

        [StringLength(200)]
        public string GI_StoreAddress { get; set; }
    }
}
