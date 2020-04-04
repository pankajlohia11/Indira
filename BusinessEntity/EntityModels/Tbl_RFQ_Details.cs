namespace BusinessEntity.EntityModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Tbl_RFQ_Details
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal RFQ_ID { get; set; }

        [StringLength(50)]
        public string REQ_Code { get; set; }

        public decimal? Enq_NO { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ENq_Date { get; set; }

        public decimal? Enq_Supplier { get; set; }

        public decimal? Enq_Customer { get; set; }

        public decimal? Sent_By { get; set; }

        public decimal? CREATED_BY { get; set; }

        public DateTime? CREATED_DATE { get; set; }

        public decimal? LAST_UPDATED_BY { get; set; }

        public DateTime? LAST_UPDATED_DATE { get; set; }

        public decimal? DELETED_BY { get; set; }

        public DateTime? DELETED_DATE { get; set; }

        public bool? DELETED { get; set; }

        public int? COM_KEY { get; set; }

        public string EnqPdfPth { get; set; }

        public bool? MailFlag { get; set; }

        public int? RFQ_Type { get; set; }
    }
}
