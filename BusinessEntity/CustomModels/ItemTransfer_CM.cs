using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.CustomModels
{
   public class ItemTransfer_CM
    {
        public decimal IT_ID { get; set; }

         
        public string IT_Code { get; set; }

        public DateTime IT_TransferDate { get; set; }

        public decimal? IT_TransferFromStore { get; set; }

        public decimal? IT_TransferToStore { get; set; }

         
        public string IT_TransferReceivedBy { get; set; }

         
        public string IT_TransferNote { get; set; }

        public decimal? CREATED_BY { get; set; }

        public DateTime? CREATED_DATE { get; set; }

        public decimal? LAST_UPDATED_BY { get; set; }

        public DateTime? LAST_UPDATED_DATE { get; set; }

        public bool? DELETED { get; set; }

        public decimal? COM_KEY { get; set; }
        public string FromStoreName { get; set; }
        public string ToStoreName { get; set; }

        public decimal TD_ID { get; set; }

        public decimal? TD_PID { get; set; }

        public decimal? TD_ProductID { get; set; }

         
        public string GD_ArticleNo { get; set; }

         
        public string GD_UOM { get; set; }

        public decimal? GD_TransferQty { get; set; }

        public decimal? GD_TransferAvailableQty { get; set; }
        public string GD_ProductName { get; set; }



    }
    public class ItemTransfer_View_CM
    {
        public List<ItemTransfer_CM> ITHeader { get; set; }
        public List<ItemTransfer_CM> ITChild { get; set; }
    }
}
