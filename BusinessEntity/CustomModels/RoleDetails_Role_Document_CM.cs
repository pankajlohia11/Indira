using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessEntity.CustomModels
{
    public class RoleDetails_Role_Document_CM
    {
        [Required]
        [StringLength(150)]
        public int FORMS { get; set; }

        [Required]
        [StringLength(50)]
        public string FORM_NAME { get; set; }

        public bool IS_VIEW { get; set; }

        public bool IS_ADD { get; set; }

        public bool IS_EDIT { get; set; }

        public bool IS_DELETE { get; set; }

        public bool IS_FULLCONTROL { get; set; }
    }
}
