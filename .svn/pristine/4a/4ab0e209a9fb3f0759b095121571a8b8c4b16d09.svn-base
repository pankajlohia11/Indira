using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessEntity.EntityModels;
/*
    Author=Manoj
    Date = 9th Apr 2018
    Document Setup Data Access 
 */
namespace DataAccess.Admin_DA
{
    public class ET_Admin_DocumentSetup_DA
    {
        // Creating Database Object for EntityClasses
        EntityClasses dbcontext = new EntityClasses();

        //Document Master List Function
        public List<Tbl_Document_Master> DocumentMaster_DA(int com_key)
        {
            try
            {
                var data = dbcontext.Tbl_Document_Master.Where(m => m.auto_key != 1 && m.COM_KEY == com_key).ToList();
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        //Document Master Add Function
        public decimal Tbl_Document_Master_Add(Tbl_Document_Master tbl_document_master)
        {
            Tbl_Document_Master tdm = new Tbl_Document_Master();
            try
            {
                if (tbl_document_master.workflowapprover==0)
                {
                    tbl_document_master.workflowapprover = null;
                }
                Tbl_Document_Master objDA = dbcontext.Tbl_Document_Master.Single(m => m.auto_key == tbl_document_master.auto_key);
                {
                    objDA.autogen_formgroup = tbl_document_master.autogen_formgroup;
                    objDA.autogen_formname = tbl_document_master.autogen_formname;
                    objDA.autogen_type = tbl_document_master.autogen_type;
                    objDA.autogen_prefix = tbl_document_master.autogen_prefix;
                    objDA.autogen_suffix = tbl_document_master.autogen_suffix;
                    objDA.autogen_startno = tbl_document_master.autogen_startno;
                    objDA.autogen_endno = tbl_document_master.autogen_endno;
                    objDA.workflowapprover = tbl_document_master.workflowapprover;
                    objDA.COM_KEY = tbl_document_master.COM_KEY;
                };
                tdm.auto_key = objDA.auto_key;
                dbcontext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return tdm.auto_key;
        }
    }
}
