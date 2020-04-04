namespace BusinessEntity.EntityModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Tbl_MailSettings
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal MS_ID { get; set; }

        [Required]
        public string MS_IncomingHostName { get; set; }

        [Required]
        public string MS_OutGoingHostName { get; set; }

        [Required]
        public string MS_EmailID { get; set; }

        [Required]
        public string MS_Password { get; set; }

        public bool MS_UseSSL { get; set; }

        public int MS_IncomingPort { get; set; }

        public int MS_OutGoingPort { get; set; }

        public bool MS_Authentication { get; set; }

        public decimal MS_UserId { get; set; }

        public decimal? CREATED_BY { get; set; }

        public DateTime? CREATED_DATE { get; set; }

        public decimal? LAST_UPDATED_BY { get; set; }

        public DateTime? LAST_UPDATED_DATE { get; set; }

        public decimal? DELETED_BY { get; set; }

        public DateTime? DELETED_DATE { get; set; }

        public bool? DELETED { get; set; }

        public decimal COM_KEY { get; set; }

        [Column(TypeName = "text")]
        public string Deleted_uids { get; set; }
    }
}
