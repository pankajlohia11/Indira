using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.CustomModels
{
  public  class Product_Catalog_Details
    {
        public decimal PC_ID { get; set; }
        public decimal P_ID { get; set; }
        public string PC_CODE { get; set; }
        public string PC_Name { get; set; }
        public string PC_Description { get; set; }
        public decimal? PRODUCT_ID { get; set; }
        public string PRODUCT_CODE { get; set; }
        public string PRODUCT_SPECIFICATION { get; set; }
        public string PRODUCT_UOM { get; set; }
        public decimal UNIT_PRICE1 { get; set; }
        public decimal UNIT_PRICE2 { get; set; }
        public decimal UNIT_PRICE3 { get; set; }
        public decimal UNIT_PRICE4 { get; set; }
        public decimal? P_SummerPrice { get; set; }
        public decimal? P_WinterPrice { get; set; }


        public DateTime LAST_PRICE_REWISE_DATE { get; set; }
        public DateTime CATALOG_VALIDITY { get; set; }

        public bool? ACTIVE_STATUS { get; set; }
        public string UOM_NAME { get; set; }
        public string P_Name { get; set; }
    }
    public class ProductCatalogView_CM
    {
        public List<Product_Catalog_Details> headerObj { get; set; }
        public List<Product_Catalog_Details> detailsObj { get; set; }
    }
}
