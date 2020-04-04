using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessEntity.EntityModels;
using DataAccess.Admin_DA;
/*
    Author=Manoj
    Date = 9th Apr 2018
    Document Setup Business Logic 
 */
namespace BusinessLogic.Admin_BL
{
   public class ET_Admin_DocumentSetup_BL
    {
        // Creating Object for Document Seup Data Access
        ET_Admin_DocumentSetup_DA objDA = new ET_Admin_DocumentSetup_DA();

        //Document Master List Function
        public List<Tbl_Document_Master> DocumentMaster_BL(int com_key)
        {
            return objDA.DocumentMaster_DA(com_key);
        }

        //Document Master Add Function
        public decimal Tbl_Document_Master_Add(Tbl_Document_Master tbl_document_master)
        {
            return objDA.Tbl_Document_Master_Add(tbl_document_master);
        }
    }
}
