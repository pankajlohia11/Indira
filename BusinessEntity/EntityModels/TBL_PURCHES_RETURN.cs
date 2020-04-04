namespace BusinessEntity.EntityModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TBL_PURCHES_RETURN
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal Purches_Code { get; set; }

        public decimal? Purches_Order_No { get; set; }

        public DateTime? Purches_order_date { get; set; }

        [StringLength(100)]
        public string Supplier_Name { get; set; }

        [StringLength(100)]
        public string Supplier_Contract_No { get; set; }

        public DateTime? Supplier_Contract_Date { get; set; }

        [StringLength(50)]
        public string Invoice_No { get; set; }

        public decimal? Invoice_Amount { get; set; }

        public decimal? Total_Return_Quantity { get; set; }

        public decimal? Total_Return_Amount { get; set; }

        public decimal? Total_Invoice_Amount { get; set; }

        public decimal? Gross_Total { get; set; }

        [StringLength(100)]
        public string Machine_Name { get; set; }

        [StringLength(100)]
        public string IP_Address { get; set; }

        public decimal? Created_By { get; set; }

        public DateTime? Created_Date { get; set; }

        public decimal? Last_Updated_By { get; set; }

        public DateTime? Last_Updated_Date { get; set; }

        public decimal? Deleted_By { get; set; }

        public DateTime? Deleted_Date { get; set; }

        public bool? Deleted { get; set; }

        [StringLength(100)]
        public string Schuduld_No { get; set; }

        public decimal? SalcePerson { get; set; }
    }
}
