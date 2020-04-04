namespace BusinessEntity.EntityModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ET_LOGIN_D
    {
        [Key]
        [Column(Order = 0)]
        public decimal USER_ID { get; set; }

        [Key]
        [Column(Order = 1)]
        public DateTime LOGIN_DATE { get; set; }

        public DateTime? LOGOUT_DATE { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(50)]
        public string SYSTEM_IP { get; set; }
    }
}
