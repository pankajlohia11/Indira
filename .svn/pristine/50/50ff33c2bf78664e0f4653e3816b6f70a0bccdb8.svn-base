namespace BusinessEntity.EntityModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_ItemTransfers
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal IT_ID { get; set; }

        [StringLength(45)]
        public string IT_Code { get; set; }

        public DateTime? IT_TransferDate { get; set; }

        public decimal? IT_TransferFromStore { get; set; }

        public decimal? IT_TransferToStore { get; set; }

        [StringLength(100)]
        public string IT_TransferReceivedBy { get; set; }

        [Column(TypeName = "text")]
        public string IT_TransferNote { get; set; }

        public decimal? CREATED_BY { get; set; }

        public DateTime? CREATED_DATE { get; set; }

        public decimal? LAST_UPDATED_BY { get; set; }

        public DateTime? LAST_UPDATED_DATE { get; set; }

        public bool? DELETED { get; set; }

        public decimal? COM_KEY { get; set; }

        [StringLength(45)]
        public string IT_GINo { get; set; }

        [StringLength(45)]
        public string IT_TransferType { get; set; }
    }
}
