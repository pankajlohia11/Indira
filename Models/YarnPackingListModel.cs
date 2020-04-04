using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Euro.Models
{
    public class YarnPackingListModel
    {
        [DisplayName("S. No.")]
        public int Serial { get; set; }
        [DisplayName("Pallet No.")]
        public string PallentNo { get; set; }

        [DisplayName("No.of Cones")]
        public string ConesCount { get; set; }

        [DisplayName("Lot No.")]
        public string LotNr { get; set; }

        [DisplayName("Nwt. in Kgs.")]
        public string NtWTKg { get; set; }

        [DisplayName("GWt. in Kgs.")]
        public string GWTKg { get; set; }

        [DisplayName("Remarks")]
        public string Remarks { get; set; }
    }

}