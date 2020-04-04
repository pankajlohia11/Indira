namespace BusinessEntity.EntityModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Tbl_Sales_Order_Header
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal SO_ID { get; set; }

        [StringLength(50)]
        public string SO_Code { get; set; }

        public decimal? SO_SalespersonID { get; set; }

        public decimal? SO_CutomerID { get; set; }

        [StringLength(100)]
        public string SO_CustomerName { get; set; }

        public decimal? SO_SupplierID { get; set; }

        [StringLength(1)]
        public string SO_SupplierName { get; set; }

        [StringLength(1)]
        public string SO_CusPONO { get; set; }

        [StringLength(1)]
        public string SO_CusPODate { get; set; }

        [StringLength(1)]
        public string SO_CusSCNO { get; set; }

        [StringLength(1)]
        public string SO_CusSCDate { get; set; }

        [StringLength(1)]
        public string SO_SupPONO { get; set; }

        [StringLength(1)]
        public string SO_SupPODate { get; set; }

        [StringLength(1)]
        public string SO_SupSCNO { get; set; }

        [StringLength(1)]
        public string SO_SupSCDate { get; set; }

        public decimal? SO_Cuscurrency { get; set; }

        public decimal? SO_Cuspayment { get; set; }

        public decimal? SO_Cuspaymentdays { get; set; }

        [StringLength(1)]
        public string SO_Cusdeliitems { get; set; }

        [StringLength(1)]
        public string SO_Cusadvtype { get; set; }

        public decimal? SO_Cusadvamt { get; set; }

        public decimal? SO_Cusadvper { get; set; }

        public decimal? SO_Cusadvdays { get; set; }

        public decimal? SO_Supcurrency { get; set; }

        public decimal? SO_Suppayment { get; set; }

        public decimal? SO_Suppaymentdays { get; set; }

        [StringLength(1)]
        public string SO_Supdeliitems { get; set; }

        [StringLength(1)]
        public string SO_Supadvtype { get; set; }

        public decimal? SO_Supadvamt { get; set; }

        public decimal? SO_Supadvper { get; set; }

        public decimal? SO_Supadvdays { get; set; }

        [StringLength(1)]
        public string SO_SupDisctype { get; set; }

        public decimal? SO_SupDiscamt { get; set; }

        public decimal? SO_SupDiscper { get; set; }

        public decimal? SO_Cusadvbalamt { get; set; }

        public decimal? SO_Cusadvpaidamt { get; set; }

        public bool? SO_Cusadvstatus { get; set; }

        public bool? SO_Supadvstatus { get; set; }

        [Key]
        [Column(Order = 1)]
        public decimal SO_Currency { get; set; }

        [StringLength(1)]
        public string SO_Dele_items { get; set; }

        public decimal? SO_Payment { get; set; }

        public decimal? SO_Paymentdays { get; set; }

        public decimal? SO_Comm { get; set; }

        [StringLength(1)]
        public string SO_Advtype { get; set; }

        public decimal? SO_Advamt { get; set; }

        public decimal? SO_Advper { get; set; }

        public decimal? SO_Advdays { get; set; }

        [StringLength(1)]
        public string SO_Disctype { get; set; }

        public decimal? SO_Discamt { get; set; }

        public decimal? SO_Discper { get; set; }

        [StringLength(1)]
        public string SO_Deleterms { get; set; }

        public decimal? SO_Salesgroup { get; set; }

        public int? SO_Orderstates { get; set; }

        [StringLength(1)]
        public string SO_Remarks { get; set; }

        public decimal? SO_Advbalamt { get; set; }

        public decimal? SO_Advpaidamt { get; set; }

        public bool? SO_Advstatus { get; set; }

        [StringLength(300)]
        public string MACHINE_NAME { get; set; }

        [StringLength(50)]
        public string IPADDRESS { get; set; }

        public decimal? CREATED_BY { get; set; }

        public DateTime? CREATED_DATE { get; set; }

        public decimal? LAST_UPDATED_BY { get; set; }

        public DateTime? LAST_UPDATED_DATE { get; set; }

        public decimal? DELETED_BY { get; set; }

        public DateTime? DELETED_DATE { get; set; }

        public bool? DELETED { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(50)]
        public string COM_KEY { get; set; }
    }
}
