namespace BusinessEntity.EntityModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Tbl_MailInbox
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal MI_ID { get; set; }

        public string MI_UID { get; set; }

        public string MI_Subject { get; set; }

        public string MI_Message { get; set; }

        public string MI_Address { get; set; }

        public string MI_Name { get; set; }

        public DateTime? MI_Date { get; set; }

        public int MI_ReadStatus { get; set; }

        public decimal MI_UserId { get; set; }

        public decimal? CREATED_BY { get; set; }

        public DateTime? CREATED_DATE { get; set; }

        public decimal? LAST_UPDATED_BY { get; set; }

        public DateTime? LAST_UPDATED_DATE { get; set; }

        public decimal? DELETED_BY { get; set; }

        public DateTime? DELETED_DATE { get; set; }

        public bool? DELETED { get; set; }

        public decimal COM_KEY { get; set; }

        public bool? MI_Attachment { get; set; }

        public string MI_cc { get; set; }

        public string MI_To { get; set; }
    }
}
