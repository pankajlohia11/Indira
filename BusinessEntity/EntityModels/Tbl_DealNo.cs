namespace BusinessEntity.EntityModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Tbl_DealNo
    {
        [Key]
        public decimal DealID { get; set; }

        [StringLength(10)]
        public string Prefix { get; set; }

        [StringLength(10)]
        public string Suffix { get; set; }

        public decimal? CreatedBy { get; set; }

        public decimal? LastUpdatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        public DateTime? LastUpdatedOn { get; set; }

        [StringLength(50)]
        public string Finance_year { get; set; }
    }
}
