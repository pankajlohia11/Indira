namespace BusinessEntity.EntityModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class error_master
    {
        [Key]
        public int error_master_id { get; set; }

        [StringLength(128)]
        public string err_title { get; set; }

        [StringLength(256)]
        public string err_message { get; set; }

        [Column(TypeName = "text")]
        public string err_details { get; set; }

        [StringLength(32)]
        public string err_type { get; set; }

        public int err_deletestatus { get; set; }

        public DateTime? err_datetime { get; set; }
    }
}
