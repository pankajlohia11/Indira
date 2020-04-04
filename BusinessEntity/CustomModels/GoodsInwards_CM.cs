using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.CustomModels
{
   public class GoodsInwards_CM
    {
        public decimal GI_ID { get; set; }

        public string GI_Code { get; set; }

        public DateTime GI_Date { get; set; }

         
        public string GI_Type { get; set; }

        public decimal GI_StoreCode { get; set; }

         
        public string GI_TransferNo { get; set; }

         
        public string GI_SupplierCode { get; set; }

         
        public string GI_ChallanNo { get; set; }

         
        public string GI_ReturnGINo { get; set; }

        public decimal? GI_POCode { get; set; }

        public DateTime GI_PODate { get; set; }

         
        public string GI_TransferStoreCode { get; set; }

         
        public string GI_ReceivedBy { get; set; }

        public decimal? GI_Approver { get; set; }

        public string StoreName { get; set; }
        public string GI_StoreAddress { get; set; }

        public string PONo { get; set; }
        public string GD_ArticleNo { get; set; }

         public string GD_ProductName { get; set; }
        public string GD_UOM { get; set; }

         
        public string GD_LotNo { get; set; }

         
        public string GD_ExpiryDate { get; set; }

        public decimal? GD_POQuantity { get; set; }

        public decimal? GD_GIQuantity { get; set; }

         
        public string GD_RejectedQuantity { get; set; }

        public decimal? GD_BalanceQty { get; set; }

        public decimal? GD_ChallanQty { get; set; }

        public decimal? GD_UnitPrice { get; set; }

         
        public string GD_GIValue { get; set; }
        
        public class GIProduct
        {
            public decimal PD_ProductID { get; set; }
            public string name { get; set; }
            public string uom { get; set; }
            public decimal poQuantity { get; set; }
            public decimal Unitprice { get; set; }
        }
    }
    public class GoodsInWards_View_CM
    {

        public List<GoodsInwards_CM> GIHeader { get; set; }
        public List<GoodsInwards_CM> GIChild { get; set; }
    }
}
