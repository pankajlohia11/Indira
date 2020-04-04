namespace BusinessEntity.EntityModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Tbl_LookUpType
    {
        [Key]
        public int LT_ID { get; set; }

        [StringLength(50)]
        public string LT_Description { get; set; }
    }
}
