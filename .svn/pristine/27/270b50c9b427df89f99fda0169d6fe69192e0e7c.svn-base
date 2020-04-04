namespace BusinessEntity.EntityModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("location")]
    public partial class location
    {
        [Key]
        public int location_id { get; set; }

        [Required]
        [StringLength(500)]
        public string name { get; set; }

        public int location_type { get; set; }

        public int parent_id { get; set; }

        public int is_visible { get; set; }
    }
}
