namespace BusinessEntity.EntityModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Tbl_Packing_ListDetails
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal PD_ID { get; set; }

        public decimal? PD_PID { get; set; }

        public decimal? PD_ProductID { get; set; }

        [StringLength(55)]
        public string PD_ArticleNo { get; set; }

        [StringLength(100)]
        public string PD_PalletNo { get; set; }

        [StringLength(55)]
        public string PD_DesignNo { get; set; }

        [StringLength(55)]
        public string PD_NoOfPieces { get; set; }

        [StringLength(55)]
        public string PD_TotalMeters { get; set; }

        [StringLength(55)]
        public string PD_NwtinKGS { get; set; }

        [StringLength(55)]
        public string PD_GwtinKGS { get; set; }

        public string PD_IndividualPieceLength { get; set; }

        //Added for Segregating the (YARNS,FABRICS & FINISHED GOODS)
        public string PD_PackingUnits { get; set; }

        public string PD_LotNo { get; set; }

        public string PD_NoOfCones { get; set; }

        public string PD_Remarks { get; set; }
    }
}
