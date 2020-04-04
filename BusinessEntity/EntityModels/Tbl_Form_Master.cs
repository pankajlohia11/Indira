namespace BusinessEntity.EntityModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Tbl_Form_Master
    {
        [Key]
        public int Form_Key { get; set; }

        [Required]
        [StringLength(50)]
        public string Form_Name { get; set; }

        [Required]
        [StringLength(50)]
        public string Form_Group { get; set; }

        public short Deleted { get; set; }
    }
}
