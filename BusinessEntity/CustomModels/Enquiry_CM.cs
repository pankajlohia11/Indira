using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.CustomModels
{
    public class Enquiry_CM
    {
       public decimal E_ID { get; set; }

         public string E_Code { get; set; }
        public int E_Type { get; set; }
        public DateTime E_Date { get; set; }

        public decimal E_CustomerID { get; set; }
        public string E_CustomerName { get; set; }
        public decimal E_ContactID { get; set; }
        public string E_ContactName { get; set; }
       
        public decimal E_SalesPerson { get; set; }
        public string E_SalesPersonName { get; set; }
        public string Street { get; set; }
        public string CityState { get; set; }
        public string CountryZip { get; set; }

        public decimal? CREATED_BY { get; set; }

        public DateTime? CREATED_DATE { get; set; }

        public decimal? LAST_UPDATED_BY { get; set; }

        public DateTime? LAST_UPDATED_DATE { get; set; }

        public decimal? DELETED_BY { get; set; }

        public DateTime? DELETED_DATE { get; set; }

        public bool? DELETED { get; set; }

        public decimal COM_KEY { get; set; }
        public decimal ED_ProductID { get; set; }
        public string ED_ArticleNo { get; set; }
        public string ED_ProductName { get; set; }
        public string ED_UOM { get; set; }
        public decimal ED_PackingQty { get; set; }
        public decimal ED_Quantity { get; set; }
        public string ED_Description { get; set; }
    }
    public class Enquiry_View_CM
    {
        public List<Enquiry_CM> EnqHeader { get; set; }
        public List<Enquiry_CM> EnqChild { get; set; }
    }
}
