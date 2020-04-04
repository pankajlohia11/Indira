using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.CustomModels
{
    public class Schedule_CM
    {
        //Added to Handle the Serial Number for the Order Details Print
        //Edit and View.
        public int SO_Serial { get; set; }
        public decimal SH_ID { get; set; }
        public string SH_Code { get; set; }
        public int SH_Type { get; set; }
        public string SO_Code { get; set; }
        public string P_ShortName { get; set; }
        public string P_ArticleNo { get; set; }
        public decimal SH_OrderID { get; set; }

        public decimal SH_OrderDetailID { get; set; }

        public decimal SH_ProductID { get; set; }

        public DateTime? SH_DATE { get; set; }
        public DateTime? SUP_DATE { get; set; }
        public decimal? SH_AvailableQuantity { get; set; }
        public decimal SH_SheduledQuantity { get; set; }
        public decimal SH_TotSchQuantity { get; set; }
        public decimal SH_OrderQuantity { get; set; }

        public bool? SH_Status { get; set; }
        public decimal SH_SalesPerson { get; set; }
        public string SH_Remarks { get; set; }

        public decimal? CREATED_BY { get; set; }

        public DateTime? CREATED_DATE { get; set; }

        public decimal? LAST_UPDATED_BY { get; set; }

        public DateTime? LAST_UPDATED_DATE { get; set; }

        public decimal? DELETED_BY { get; set; }

        public DateTime? DELETED_DATE { get; set; }

        public bool? DELETED { get; set; }

        public int? COM_KEY { get; set; }
        public string SalesPerson { get; set; }
        public string ScNo { get; set; }
        public string poNo { get; set; }
        public string SupplierName { get; set; }
        public string CustomerName { get; set; }
        public DateTime SO_Date { get; set; }
        public string DesignNo { get; set; }
        public string SupplierPONo { get; set; }
        public decimal? TotalSchAmount { get; set; }
    }
}
