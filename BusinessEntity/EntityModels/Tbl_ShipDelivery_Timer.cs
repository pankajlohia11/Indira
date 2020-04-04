namespace BusinessEntity.EntityModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Tbl_ShipDelivery_Timer
    {
        [Key]
        [Column(Order = 0)]
        public decimal USER_ID { get; set; }

        [Key]
        [Column(Order = 1)]
        public DateTime SHIP_DELIV_TIMER { get; set; }
    }
}
