namespace BusinessEntity.EntityModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Tbl_Locations_Master
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal Store_ID { get; set; }

        [Required]
        [StringLength(50)]
        public string Store_Name { get; set; }

        [Required]
        [StringLength(50)]
        public string Display_Name { get; set; }

        [StringLength(50)]
        public string Store_KeeperName { get; set; }

        [StringLength(50)]
        public string Street { get; set; }

        [StringLength(50)]
        public string City { get; set; }

        [StringLength(50)]
        public string Zip { get; set; }

        [StringLength(50)]
        public string Country { get; set; }

        [StringLength(50)]
        public string ContactNo { get; set; }

        [StringLength(300)]
        public string Machine_Name { get; set; }

        [StringLength(50)]
        public string IP_Address { get; set; }

        public decimal? Created_By { get; set; }

        public DateTime? Created_Date { get; set; }

        public decimal? LAST_UPDATED_BY { get; set; }

        public DateTime? LAST_UPDATED_DATE { get; set; }

        public decimal? DELETED_BY { get; set; }

        public DateTime? DELETED_DATE { get; set; }

        public bool? Deleted { get; set; }

        [StringLength(50)]
        public string Store_Code { get; set; }

        [StringLength(50)]
        public string State { get; set; }

        public decimal? COM_KEY { get; set; }
    }
}
