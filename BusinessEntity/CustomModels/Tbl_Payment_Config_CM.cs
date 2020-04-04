using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.CustomModels
{
    public class Tbl_Payment_Config_CM
    {
        public decimal Payment_Config_Id { get; set; }

        public int? Payment_From { get; set; }

        public int? Payment_To { get; set; }

        public string Payment_Discount_Type { get; set; }

        public decimal? Payment_Discount_Percentage { get; set; }

        public decimal? Payment_Discount_Amount { get; set; }

        public string Machine_Name { get; set; }

        public string IPAddress { get; set; }

        public string Created_By { get; set; }

        public DateTime? Created_Date { get; set; }

        public string Last_Updated_By { get; set; }

        public DateTime? Last_Updated_Date { get; set; }

        public decimal? Deleted_By { get; set; }

        public DateTime? Deleted_date { get; set; }

        public bool? Deleted { get; set; }

        public string Payment_Config_Code { get; set; }
    }
}
