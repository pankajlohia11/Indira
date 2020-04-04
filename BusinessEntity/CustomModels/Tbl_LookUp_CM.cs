using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessEntity.CustomModels
{
    public class Tbl_LookUp_CM
    {
        public int LU_ID { get; set; }

        public int? LU_Type { get; set; }

        [StringLength(50)]
        public string LU_Code { get; set; }

        [StringLength(200)]
        public string LU_Description { get; set; }
        
        [StringLength(50)]
        public string LT_Description { get; set; }
    }
}
