using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.CustomModels
{
    public class CheckStockQtyofanItem_CM
    {
        public DateTime Date { get; set; }
        public string TrnTpe { get; set; }
        public string PartyName { get; set; }
        public decimal? Product_Code { get; set; }
        public string productName { get; set; }
        public string Product_Description { get; set; }
        public string Article_NO { get; set; }
        public decimal Quantity { get; set; }
        public string UOM { get; set; }
        public decimal OpeningQty { get; set; }
        public decimal OpnStockQty { get; set; }
        public decimal ClosingStockQty { get; set; }
        public decimal ClosingQty { get; set; }
    }
}
