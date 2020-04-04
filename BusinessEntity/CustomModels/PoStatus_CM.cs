using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.CustomModels
{
    public class PoStatus_CM
    {
        public decimal postatusID { get; set; }
        public decimal? supplierID { get; set; }
        public decimal? ProductId { get; set; }
        public decimal? PD_UnitPrice { get; set; }
        public decimal PO_ID { get; set; }
        public string PO_Code { get; set; }
        public DateTime? Po_Date { get; set; }
        public string supplierName { get; set; }
        public string Product_Desc { get; set; }
        public string UOM { get; set; }
        public decimal PO_Quantity { get; set; }
        public decimal Receipt_Qty { get; set; }
        public decimal OutStanding  { get; set; }
        public decimal? Qty { get; set; }
        public DateTime? SchDate { get; set; }
        public decimal Days_Pending { get; set; }

    }
}
