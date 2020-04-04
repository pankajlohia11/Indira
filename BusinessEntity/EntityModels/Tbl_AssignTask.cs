namespace BusinessEntity.EntityModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Tbl_AssignTask
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal TSK_ID { get; set; }

        public string TSK_Title { get; set; }

        public string TSK_Desc { get; set; }

        public short? TSK_Priority { get; set; }

        public bool? TSK_Type { get; set; }

        public string TSK_AssignTo { get; set; }

        public DateTime? Expec_Date { get; set; }

        public DateTime? TSK_Compl_Date { get; set; }

        [Required]
        [StringLength(50)]
        public string TSK_Status { get; set; }

        public decimal TSK_UserId { get; set; }

        public decimal? CREATED_BY { get; set; }

        public DateTime? CREATED_DATE { get; set; }

        public decimal? LAST_UPDATED_BY { get; set; }

        public DateTime? LAST_UPDATED_DATE { get; set; }

        public decimal? DELETED_BY { get; set; }

        public DateTime? DELETED_DATE { get; set; }

        public bool? DELETED { get; set; }

        public decimal COM_KEY { get; set; }
    }
}
