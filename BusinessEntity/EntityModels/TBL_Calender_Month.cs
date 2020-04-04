namespace BusinessEntity.EntityModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TBL_Calender_Month
    {
        [Key]
        public decimal MonthId { get; set; }

        [StringLength(50)]
        public string Month { get; set; }
    }
}
