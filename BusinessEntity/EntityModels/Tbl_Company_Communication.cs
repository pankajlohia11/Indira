namespace BusinessEntity.EntityModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Tbl_Company_Communication
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal CC_ID { get; set; }

        public decimal CC_PID { get; set; }

        [Required]
        [StringLength(200)]
        public string SUBJECT { get; set; }

        public DateTime? DATE_COMM { get; set; }

        [StringLength(200)]
        public string FILE_NAME { get; set; }

        [Column(TypeName = "text")]
        public string CONVERSATION { get; set; }

        public bool TYPE { get; set; }

        public DateTime? CREATED_DATE { get; set; }

        public decimal? LAST_UPDATED_BY { get; set; }

        public DateTime? LAST_UPDATED_DATE { get; set; }

        public decimal? DELETED_BY { get; set; }

        public DateTime? DELETED_DATE { get; set; }

        public bool? DELETED { get; set; }

        public decimal? MS_UserId { get; set; }

        public decimal? CREATED_BY { get; set; }
    }
}
