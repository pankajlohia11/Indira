namespace BusinessEntity.EntityModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Tbl_OfflineCommunication
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal ATTACH_ID { get; set; }

        [Required]
        [StringLength(50)]
        public string COMM_TYPE { get; set; }

        public decimal CUST_ID { get; set; }

        [Required]
        [StringLength(100)]
        public string SUBJECT { get; set; }

        public DateTime? COMM_DATE { get; set; }

        [StringLength(1000)]
        public string COMMUNICATION { get; set; }

        [StringLength(500)]
        public string MAIL_TO { get; set; }

        [StringLength(100)]
        public string MAIL_FROM { get; set; }

        [StringLength(500)]
        public string MAIL_CC { get; set; }

        [StringLength(500)]
        public string MAIL_BCC { get; set; }

        [StringLength(200)]
        public string FILE_NAME { get; set; }

        [StringLength(300)]
        public string MACHINE_NAME { get; set; }

        [StringLength(50)]
        public string IPADDRESS { get; set; }

        public decimal? CREATED_BY { get; set; }

        public DateTime? CREATED_DATE { get; set; }
    }
}
