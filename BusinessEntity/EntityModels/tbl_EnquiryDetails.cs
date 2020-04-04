namespace BusinessEntity.EntityModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_EnquiryDetails
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal ED_ID { get; set; }

        public decimal? ED_PID { get; set; }

        public decimal ED_ProductID { get; set; }

        public decimal ED_Quantity { get; set; }

        public string ED_Description { get; set; }
    }
}
