using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.CustomModels
{
    public class ShipmentReport_CM
    {
        public decimal shipmentID { get; set; }
        public decimal salespersonID { get; set; }
        public decimal? DD_OrderDetailID { get; set; }
        public string salespersonName { get; set; }
        public string SO_SupSCNO { get; set; }
        public string BlNo { get; set; }
        public int monthname { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        public DateTime? OTOI_InvoiceDate { get; set; }
        public decimal customerID { get; set; }
        public string customerName { get; set; }

        public decimal supplierID { get; set; }
        public decimal CommissionAmtTrade { get; set; }
        public string supplierName { get; set; }
        public DateTime? ETD { get; set; }
        public string InvNo { get; set; }
        public string SO_Currency { get; set; }
        public decimal? SO_Commision { get; set; }
        public DateTime? InvDate { get; set; }
        public DateTime? BlDAte { get; set; }
        public DateTime? SO_SupSCDate { get; set; }
        public decimal InvAmount { get; set; }
        public decimal CommissionAmt { get; set; }
        public string Tradpo { get; set; }
        public DateTime Sc_Date { get; set; }
        public int Comm { get; set; }
        public string Currency { get; set; }
        public string Sc_No {get;set;}
        public int Comm_Euro { get; set; }
        public string ShipmentProductType { get; set; }
        public decimal ShipmenPGParent { get; set; }
        public decimal ShipmenPGParentType { get; set; }
        public string ShipmenPGName { get; set; }
    }
}
