namespace BusinessEntity.EntityModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_Notifications
    {
        [Key]
        [Column(Order = 0)]
        public int N_ID { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(150)]
        public string N_Type { get; set; }

        [Key]
        [Column(Order = 2)]
        public DateTime N_Date { get; set; }

        [Key]
        [Column(Order = 3)]
        public decimal N_UserId { get; set; }

        [Key]
        [Column(Order = 4)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int N_Status { get; set; }

        [Key]
        [Column(Order = 5)]
        public string N_Message { get; set; }

        [Key]
        [Column(Order = 6)]
        public decimal N_HeaderID { get; set; }

        public decimal? CREATED_BY { get; set; }

        public DateTime? CREATED_DATE { get; set; }

        public decimal? LAST_UPDATED_BY { get; set; }

        public DateTime? LAST_UPDATED_DATE { get; set; }

        public decimal? DELETED_BY { get; set; }

        public DateTime? DELETED_DATE { get; set; }

        public bool? DELETED { get; set; }
    }
}
