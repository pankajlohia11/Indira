using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.CustomModels
{
    public class OneToOneInvoice_CM
    {
        public int OTOI_Serial { get; set; }
        public decimal OTOI_ID { get; set; }
        public string OTOI_Code { get; set; }

        public decimal OTOI_CustomerID { get; set; }
        public string OTOI_SalesPerson { get; set; }
        public decimal OTOI_ShipmentID { get; set; }
        public DateTime OTOI_InvoiceDate { get; set; }
        public string S_Code { get; set; }
        public string COM_DISPLAYNAME { get; set; }
        public string OTOI_CompanyName { get; set; }
        public string OTOI_CustomerName { get; set; }
        public string USER_NAME { get; set; }
        public decimal OTOI_ShipmentAmount { get; set; }
        public decimal Amt { get; set; }

        public decimal OTOI_TaxPer { get; set; }
        public string OTOI_TaxRemarks { get; set; }

        // public decimal OrderId { get; set; }
        public string PaymentTerms { get; set; }
        public string DeliveryTerms { get; set; }

        public decimal OTOI_TaxAmount { get; set; }

        public decimal OTOI_InvoiceAmount { get; set; }

        public decimal? CREATED_BY { get; set; }

        public DateTime? CREATED_DATE { get; set; }

        public decimal? LAST_UPDATED_BY { get; set; }

        public DateTime? LAST_UPDATED_DATE { get; set; }

        public decimal? DELETED_BY { get; set; }

        public DateTime? DELETED_DATE { get; set; }

        public bool? DELETED { get; set; }

        public int COM_KEY { get; set; }
        public string ArticleNo { get; set; }
        public string ProductName { get; set; }
        public string UOM { get; set; }
        public string UOM_Code { get; set; }
        public decimal? SD_Quantity { get; set; }
        public decimal Unit_Price { get; set; }
        public string CompanyCode { get; set; }
        public string Street { get; set; }
        public string CityState { get; set; }
        public string CountryZip { get; set; }
        public string USTID { get; set; }
        //public string PaymentTerms { get; set; }
        public string PaymentTermsDescription { get; set; }
        public decimal? VatPer { get; set; }
        public string imgurl { get; set; }
        public string SysCompany { get; set; }
        public decimal price { get; set; }
        public string Bl_no { get; set; }
        public string containerNo { get; set; }
        public string DesignDetail { get; set; }
        public string CustomerDes { get; set; }
        public string OrderDescription { get; set; }
        public decimal? Discount { get; set; }
        public string Description { get; set; }
        public decimal Discountper { get; set; }
        public int? Zipcode { get; set; }
        public string cusPONo { get; set; }
        public string popupOrderNo { get; set; }
    }
    public class OneToOneInvoiceView_CM
    {
        public List<OneToOneInvoice_CM> objHeader { get; set; }
        public List<OneToOneInvoice_CM> objDetail { get; set; }
    }
}