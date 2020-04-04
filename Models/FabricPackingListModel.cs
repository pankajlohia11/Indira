using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Euro.Models
{
    public class FabricPackingListModel
    {
        [DisplayName("S. No.")]
        public int Serial { get; set; }
        [DisplayName("Pallet No.")]
        public string PalletNo { get; set; }

        [DisplayName("Design No.")]
        public string DesignNo { get; set; }

        [DisplayName("No.of pieces")]
        public string PiecesNo { get; set; }

        [DisplayName("Total Meters")]
        public string TotalMtrs { get; set; }

        [DisplayName("Nwt. in Kgs.")]
        public string NtWTKg { get; set; }

        [DisplayName("GWt. in Kgs.")]
        public string GWTKg { get; set; }

        [DisplayName("Individual piece lengths")]
        public string IndvPieceLength { get; set; }

        [DisplayName("Remarks")]
        public string Remarks { get; set; }
    }
}