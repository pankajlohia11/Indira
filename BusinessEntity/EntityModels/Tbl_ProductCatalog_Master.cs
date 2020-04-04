namespace BusinessEntity.EntityModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Tbl_ProductCatalog_Master
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal PC_MasterID { get; set; }

        [StringLength(50)]
        public string PC_CODE { get; set; }

        [StringLength(50)]
        public string PC_Name { get; set; }

        [StringLength(50)]
        public string PC_Description { get; set; }

        [StringLength(50)]
        public string PC_CatalogueCode { get; set; }

        public int PC_ProductCategory { get; set; }

        public int PC_SubProductCategory { get; set; }

        public decimal? CREATED_BY { get; set; }

        public DateTime? CREATED_DATE { get; set; }

        public decimal? LAST_UPDATED_BY { get; set; }

        public DateTime? LAST_UPDATED_DATE { get; set; }

        public DateTime? VALIDITY_DATE { get; set; }

        public decimal? DELETED_BY { get; set; }

        public DateTime? DELETED_DATE { get; set; }

        public bool? DELETED { get; set; }

        public decimal? COM_KEY { get; set; }
        public bool? ACTIVE_STATUS { get; set; }

        //Product Catalog type(1 - Customer, 2- Supplier)
        public int PC_CatalogType { get; set; }
    }
}
