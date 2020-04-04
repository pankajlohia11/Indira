using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.CustomModels
{
   public class PackingList_ViewCM
    {
        public string CustomerName { get; set; }
        public string OrderCode { get; set; }
        public decimal PL_ID { get; set; }
         
        public string PL_Code { get; set; }

        public DateTime? PL_Date { get; set; }

        public decimal? PL_OrderNo { get; set; }

        public int PL_Type { get; set; }

        //Adding the Shipment Code here.
        public decimal OTOI_ShipmentID { get; set; }

        public string OTOI_ContainerNo { get; set; }

        public string PL_Remarks { get; set; }

        public decimal? CREATED_BY { get; set; }

        public DateTime? CREATED_DATE { get; set; }

        public decimal? LAST_UPDATED_BY { get; set; }

        public DateTime? LAST_UPDATED_DATE { get; set; }

        public decimal? DELETED_BY { get; set; }

        public decimal? DELETED_DATE { get; set; }

        public bool? DELETED { get; set; }

        public decimal? COM_KEY { get; set; }
        public decimal PD_ID { get; set; }

        public decimal? PD_PID { get; set; }

        public decimal? PD_ProductID { get; set; }
         
        public string PD_ArticleNo { get; set; }
         
        public string PD_PalletNo { get; set; }

        public string PD_ConesNr { get; set; }
        public string PD_LotNr { get; set; }
         
        public string PD_DesignNo { get; set; }

        public string PD_NoOfPieces { get; set; }

        public string PD_TotalMeters { get; set; }

        public string PD_NwtinKGS { get; set; }

        public string PD_GwtinKGS { get; set; }

        public string PD_PackingUnits { get; set; }

        public string PD_IndividualPieceLength { get; set; }
        public string ProductName { get; set; }
        public decimal? PL_CustomerID { get; set; }
        public string Orders { get; set; }
        public string Customer { get; set; }
        public string CompanyCode { get; set; }
        public string CompanyName { get; set; }
        public string Street { get; set; }
        public string CityState { get; set; }
        public string CountryZip { get; set; }
        public string USTID { get; set; }
        public decimal? VatPer { get; set; }
        public string imgurl { get; set; }
        public string SystemCompany { get; set; }
    }
    public class Paclistdetails_View_CM
    {
        public List<PackingList_ViewCM> QHeader { get; set; }
        public List<PackingList_ViewCM> QChild { get; set; }
    }
}
