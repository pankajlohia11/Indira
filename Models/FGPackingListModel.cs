using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Euro.Models
{
    public class FGPackingListModel
    {
        [DisplayName("S. No.")]
        public int Serial { get; set; }

        [DisplayName("No. of Pieces")]
        public string PiecesCount { get; set; }

        [DisplayName("Packing Units")]
        public string PackingUnits { get; set; }

        [DisplayName("Nwt. in Kgs.")]
        public string NtWTKg { get; set; }

        [DisplayName("GWt. in Kgs.")]
        public string GWTKg { get; set; }

        [DisplayName("Remarks")]
        public string Remarks { get; set; }
    }
}