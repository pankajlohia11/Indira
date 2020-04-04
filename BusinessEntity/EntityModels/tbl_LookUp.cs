namespace BusinessEntity.EntityModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_LookUp
    {
        [Key]
        public int LU_ID { get; set; }

        public int? LU_Type { get; set; }

        [StringLength(50)]
        public string LU_Code { get; set; }

        [StringLength(200)]
        public string LU_Description { get; set; }

        public bool? LU_Deleted { get; set; }
    }
}
