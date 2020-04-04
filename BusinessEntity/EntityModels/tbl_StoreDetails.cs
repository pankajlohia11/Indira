namespace BusinessEntity.EntityModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_StoreDetails
    {
        [Key]
        public int SD_Id { get; set; }

        public decimal? SD_SM_ID { get; set; }

        public decimal? SD_Itemcode { get; set; }

        [StringLength(500)]
        public string SD_ItemDescription { get; set; }

        [StringLength(50)]
        public string SD_UOM { get; set; }

        public decimal SD_OpeningStock { get; set; }

        public decimal SD_OpeningRate { get; set; }
    }
}
