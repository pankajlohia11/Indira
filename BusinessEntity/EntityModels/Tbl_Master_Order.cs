namespace BusinessEntity.EntityModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Tbl_Master_Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal SO_ID { get; set; }

        [StringLength(50)]
        public string SO_Code { get; set; }

        [Column(TypeName = "date")]
        public DateTime SO_OrderDate { get; set; }

        public decimal SO_SalesPersonID { get; set; }

        public decimal? SO_ORGID { get; set; }

        public int SO_OrderType { get; set; }

        public decimal? SO_CutomerID { get; set; }

        public decimal? SO_SupplierID { get; set; }

        [StringLength(50)]
        public string SO_CusPONO { get; set; }

        public DateTime? SO_CusPODate { get; set; }

        [StringLength(50)]
        public string SO_SupSCNO { get; set; }

        public DateTime? SO_SupSCDate { get; set; }

        public decimal? SO_CusCurrency { get; set; }

        public decimal? SO_CusPaymentTermID { get; set; }

        public string SO_CusDeliveryTerms { get; set; }

        public decimal? SO_Commision { get; set; }

        public int SO_Approval { get; set; }

        public decimal? SO_Approver { get; set; }

        public decimal? SO_QuotationID { get; set; }

        public decimal SO_Discount { get; set; }

        public string SO_Remarks { get; set; }

        public int SO_PriceType { get; set; }

        public decimal? CREATED_BY { get; set; }

        public DateTime? CREATED_DATE { get; set; }

        public decimal? LAST_UPDATED_BY { get; set; }

        public DateTime? LAST_UPDATED_DATE { get; set; }

        public decimal? DELETED_BY { get; set; }

        public DateTime? DELETED_DATE { get; set; }

        public bool? DELETED { get; set; }

        public decimal COM_KEY { get; set; }

        public bool? SO_TaxApplicable { get; set; }

        public string SO_TaxRemarks { get; set; }
        public int SO_ProductType { get; set; }
        public string Q_CatalogId {get;set;}
    }
}
