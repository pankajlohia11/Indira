namespace BusinessEntity.EntityModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Tbl_OneToManyInvoice
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal OTMI_ID { get; set; }

        [StringLength(50)]
        public string OTMI_Code { get; set; }

        public decimal OTMI_CustomerID { get; set; }

        public decimal OTMI_SalesPerson { get; set; }

        public decimal? OTMI_DespatchIDs { get; set; }

        [Column(TypeName = "date")]
        public DateTime OTMI_InvoiceDate { get; set; }

        public decimal OTMI_OrderAmount { get; set; }

        public decimal OTMI_TaxPer { get; set; }

        public decimal OTMI_TaxAmount { get; set; }

        public decimal OTMI_InvoiceAmount { get; set; }

        public decimal? CREATED_BY { get; set; }

        public DateTime? CREATED_DATE { get; set; }

        public decimal? LAST_UPDATED_BY { get; set; }

        public DateTime? LAST_UPDATED_DATE { get; set; }

        public decimal? DELETED_BY { get; set; }

        public DateTime? DELETED_DATE { get; set; }

        public bool? DELETED { get; set; }

        public int COM_KEY { get; set; }

        public int? OTMI_Approval { get; set; }

        public decimal? OTMI_Approver { get; set; }

        [StringLength(200)]
        public string OTMI_PaymentTerms { get; set; }

        public decimal? OTMI_DiscountAmt { get; set; }

        public decimal? OTMI_FreightCost { get; set; }

        public decimal? OTMI_TransportCost { get; set; }
    }
}
