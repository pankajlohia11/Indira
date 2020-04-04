namespace BusinessEntity.EntityModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_EnquiryHeader
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal E_ID { get; set; }

        [StringLength(15)]
        public string E_Code { get; set; }

        public DateTime E_Date { get; set; }

        public int E_Type { get; set; }

        public decimal E_CustomerID { get; set; }

        public decimal E_ContactID { get; set; }

        public decimal E_SalesPerson { get; set; }

        public decimal? CREATED_BY { get; set; }

        public DateTime? CREATED_DATE { get; set; }

        public decimal? LAST_UPDATED_BY { get; set; }

        public DateTime? LAST_UPDATED_DATE { get; set; }

        public decimal? DELETED_BY { get; set; }

        public DateTime? DELETED_DATE { get; set; }

        public bool? DELETED { get; set; }

        public decimal COM_KEY { get; set; }
    }
}
