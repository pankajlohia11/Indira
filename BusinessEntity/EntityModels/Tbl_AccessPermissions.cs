namespace BusinessEntity.EntityModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Tbl_AccessPermissions
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal ID { get; set; }

        public decimal ROLE_ID { get; set; }

        [Required]
        [StringLength(100)]
        public string FORMS { get; set; }

        [Required]
        [StringLength(50)]
        public string FORM_NAME { get; set; }

        public bool IS_VIEW { get; set; }

        public bool IS_ADD { get; set; }

        public bool IS_EDIT { get; set; }

        public bool IS_DELETE { get; set; }

        public bool IS_FULLCONTROL { get; set; }

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

        public bool DELETED { get; set; }
    }
}
