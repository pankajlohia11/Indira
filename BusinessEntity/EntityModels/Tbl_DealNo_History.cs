namespace BusinessEntity.EntityModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Tbl_DealNo_History
    {
        [Key]
        public decimal DealID { get; set; }

        public decimal? Old_DealID { get; set; }

        [StringLength(10)]
        public string Prefix { get; set; }

        [StringLength(10)]
        public string Suffix { get; set; }
    }
}
