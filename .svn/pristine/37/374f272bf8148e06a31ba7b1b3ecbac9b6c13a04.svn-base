namespace BusinessEntity.EntityModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Tbl_AuditTrail
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal AUDITTRAIL_ID { get; set; }

        [Key]
        [Column(Order = 1)]
        public DateTime MODIFIED_DATE { get; set; }

        [Key]
        [Column(Order = 2)]
        public decimal RECORD_ID { get; set; }

        [StringLength(500)]
        public string ROW_DESC { get; set; }

        [StringLength(10)]
        public string CHANGE_TYPE { get; set; }

        [StringLength(300)]
        public string FIELD_NAME { get; set; }

        [StringLength(300)]
        public string TABLE_NAME { get; set; }

        [Key]
        [Column(Order = 3)]
        public decimal USER_ID { get; set; }

        [Column(TypeName = "text")]
        public string OLD_VALUE { get; set; }

        [Column(TypeName = "text")]
        public string NEW_VALUE { get; set; }

        [StringLength(300)]
        public string FORM_NAME { get; set; }

        [StringLength(300)]
        public string SCREEN_NAME { get; set; }

        [StringLength(300)]
        public string MACHINE_NAME { get; set; }

        [StringLength(50)]
        public string IPADDRESS { get; set; }
    }
}
