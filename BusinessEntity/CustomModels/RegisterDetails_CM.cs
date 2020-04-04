using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.CustomModels
{
    public class RegisterDetails_CM
    {
        public decimal shipmentID { get; set; }
        public decimal salespersonID { get; set; }
        public string salespersonName { get; set; }

        public decimal? customerID { get; set; }
        public string customerName { get; set; }

        public decimal supplierID { get; set; }
        public decimal? OrderAmt { get; set; }
        public string supplierName { get; set; }
        public DateTime? ETD { get; set; }
        public string InvNo { get; set; }
        public DateTime? InvDate { get; set; }
        public decimal? InvAmount { get; set; }
        public decimal CommissionAmt { get; set; }
        public decimal ProductId { get; set; }
        public decimal OrderedQty { get; set; }
        public decimal? Price { get; set; }
        public decimal Amt { get; set; }
        public decimal CommissionAmtRece { get; set; }
        public string ProductName { get; set; }
        public string Product { get; set; }
        public decimal? Quantity { get; set; }
        public string Currency { get; set; }
        public string Debit_No { get; set; }
        public DateTime Debit_Date { get; set; }
        public decimal? Comm { get; set; }
        public DateTime RecdDate { get; set; }
        public double Comm_Disc { get; set; }
        public string User { get; set; }
        public double Bal_Amt { get; set; }
        public DateTime Due { get; set; }
        public DateTime? Inv_Date { get; set; }
        public string Sc_No { get; set; }
        public string Factor { get; set; }

    }
}
