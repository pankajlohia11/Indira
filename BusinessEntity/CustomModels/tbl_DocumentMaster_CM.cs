using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.CustomModels
{
    public class tbl_DocumentMaster_CM
    {
        public int auto_key { get; set; }
        public string autogen_formname { get; set; }
        public string autogen_type { get; set; }
        public string autogen_prefix { get; set; }
        public string autogen_suffix { get; set; }

        
        public string autogen_startno { get; set; }

        
        public string autogen_endno { get; set; }
        public string autogen_formgroup { get; set; }

        public int workflow_status { get; set; }

        public decimal? workflowapprover { get; set; }
        public string workflowapprovername { get; set; }
        public decimal? COM_KEY { get; set; }

    }
}
