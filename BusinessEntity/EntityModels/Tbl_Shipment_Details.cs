namespace BusinessEntity.EntityModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Tbl_Shipment_Details
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal SD_ID { get; set; }

        public decimal? SD_PID { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int SD_Type { get; set; }

        [Key]
        [Column(Order = 2)]
        public decimal SD_ScheduleID { get; set; }

        [Key]
        [Column(Order = 3)]
        public decimal SD_OrderDetailID { get; set; }

        [Key]
        [Column(Order = 4)]
        public decimal SD_ProductID { get; set; }

        public decimal? SD_ScheduledQuantity { get; set; }

        public decimal? SD_Quantity { get; set; }
    }
}
