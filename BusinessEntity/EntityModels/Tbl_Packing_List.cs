namespace BusinessEntity.EntityModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Tbl_Packing_List
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal PL_ID { get; set; }

        [StringLength(15)]
        public string PL_Code { get; set; }

        public DateTime? PL_Date { get; set; }

        public string PL_OrderNo { get; set; }

        public string PL_Remarks { get; set; }

        public decimal? CREATED_BY { get; set; }

        public DateTime? CREATED_DATE { get; set; }

        public decimal? LAST_UPDATED_BY { get; set; }

        public DateTime? LAST_UPDATED_DATE { get; set; }

        public decimal? DELETED_BY { get; set; }

        public decimal? DELETED_DATE { get; set; }

        public bool? DELETED { get; set; }

        public decimal? COM_KEY { get; set; }

        public decimal? PL_CustomerID { get; set; }

        //Packing List Type Added
        public int PL_Type { get; set; }

        [StringLength(50)]
        public string PL_ShipmentCode { get; set; }
        public string PL_Summary { get; set; }
    }
}
