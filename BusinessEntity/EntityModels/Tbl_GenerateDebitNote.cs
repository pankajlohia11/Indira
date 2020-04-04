namespace BusinessEntity.EntityModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Tbl_GenerateDebitNote
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal DN_ID { get; set; }

        [StringLength(50)]
        public string DN_Code { get; set; }

        public decimal DN_ShipmentID { get; set; }

        public decimal DN_ShipmentAmount { get; set; }

        public int DN_Status { get; set; }

        public string DN_FOBAmount { get; set; }

        public string DN_OrderCodes { get; set; }

        public int DN__FOBStatus { get; set; }

        public decimal? CREATED_BY { get; set; }

        public DateTime? CREATED_DATE { get; set; }

        public decimal? LAST_UPDATED_BY { get; set; }

        public DateTime? LAST_UPDATED_DATE { get; set; }

        public decimal? DELETED_BY { get; set; }

        public DateTime? DELETED_DATE { get; set; }

        public bool? DELETED { get; set; }

        public int COM_KEY { get; set; }
    }
}
