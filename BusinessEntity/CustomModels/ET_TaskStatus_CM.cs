using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.CustomModels
{
    public class ET_TaskStatus_CM
    {
        public int SO_Serial { get; set; }
        public string TSK_Title { get; set; }
        public string TSK_Description { get; set; }

        public short TSK_Priority { get; set; }
        public DateTime TSK_Expected_Date { get; set; }

        public string TSK_Assignto{ get; set; }
        public DateTime TSK_Compl_Date { get; set; }
        public Decimal TSK_Userid { get; set; }
        public string TSK_Status { get; set; }

    }
    public class ET_TaskStatus_Collection_CM
    {
        public List<ET_TaskStatus_CM> Headerobj { get; set; }
        //public List<Approval_CM> Detailsobj { get; set; }
    }



}