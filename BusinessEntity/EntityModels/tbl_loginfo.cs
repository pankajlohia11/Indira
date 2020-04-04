namespace BusinessEntity.EntityModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_loginfo
    {
        [Key]
        public int log_id { get; set; }

        [StringLength(45)]
        public string log_dockey { get; set; }

        [Required]
        [StringLength(50)]
        public string log_operation { get; set; }

        [Required]
        [StringLength(100)]
        public string log_userid { get; set; }

        public DateTime log_date { get; set; }

        [StringLength(45)]
        public string log_recordkey { get; set; }

        public string log_Remarks { get; set; }
    }
}
