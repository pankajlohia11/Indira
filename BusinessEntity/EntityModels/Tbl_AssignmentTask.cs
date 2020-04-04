namespace BusinessEntity.EntityModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Tbl_AssignmentTask
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal ASSIGN_ID { get; set; }

        [Required]
        [StringLength(250)]
        public string TASK { get; set; }

        [Required]
        [StringLength(50)]
        public string PRIORITY { get; set; }

        public decimal? ASSIGN_TO { get; set; }

        public DateTime? ASSIGNED_DT { get; set; }

        public decimal? STATUS { get; set; }

        public decimal? ASSIGNED_BY { get; set; }

        public DateTime? DONE_DT { get; set; }

        [StringLength(500)]
        public string REMARKS { get; set; }

        [StringLength(300)]
        public string MACHINE_NAME { get; set; }

        [StringLength(50)]
        public string IPADDRESS { get; set; }

        public decimal? CREATED_BY { get; set; }

        public DateTime? CREATED_DATE { get; set; }

        public decimal? LAST_UPDATED_BY { get; set; }

        public DateTime? LAST_UPDATED_DATE { get; set; }

        public decimal? DELETED_BY { get; set; }

        public DateTime? DELETED_DATE { get; set; }

        public bool? DELETED { get; set; }

        public virtual Tbl_Master_User Tbl_Master_User { get; set; }
    }
}
