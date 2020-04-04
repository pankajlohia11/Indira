namespace BusinessEntity.EntityModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Tbl_Master_Comm
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal COMM_ID { get; set; }

        public decimal COM_ID { get; set; }

        [StringLength(1000)]
        public string COMMUNICATION { get; set; }

        public decimal? MADE_BY { get; set; }

        public DateTime? COMM_DATE { get; set; }

        public decimal? Whom_by { get; set; }

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

        public virtual Tbl_Master_CompanyDetails Tbl_Master_CompanyDetails { get; set; }
    }
}
