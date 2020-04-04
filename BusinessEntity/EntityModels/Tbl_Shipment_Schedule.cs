namespace BusinessEntity.EntityModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Tbl_Shipment_Schedule
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Tbl_Shipment_Schedule()
        {
            Tbl_Shipment_DeliverableInfo = new HashSet<Tbl_Shipment_DeliverableInfo>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal SHEDU_ID { get; set; }

        [Required]
        [StringLength(50)]
        public string AGEN_TRADE { get; set; }

        public decimal AGENCY_ID { get; set; }

        [Required]
        [StringLength(50)]
        public string DEAL_NO { get; set; }

        public decimal? SHIPPMENT_SCH_NO { get; set; }

        public DateTime SHIP_DATE { get; set; }

        [StringLength(50)]
        public string TOTAL_QUAN { get; set; }

        public bool? SCHED_STATUS { get; set; }

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

        public bool Invoice_Status { get; set; }

        public bool? Cancelled { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Cancelled_By { get; set; }

        public DateTime? Cancelled_Date { get; set; }

        [StringLength(250)]
        public string Remarks { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_Shipment_DeliverableInfo> Tbl_Shipment_DeliverableInfo { get; set; }
    }
}
