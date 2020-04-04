using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Admin_DA;
using DataAccess;
using BusinessLogic.Admin_BL;
using BusinessEntity.EntityModels;
using BusinessEntity.CustomModels;

namespace BusinessLogic.Admin_BL
{
    public class ET_Admin_GeneralOffer_BL
    {
        ET_Admin_GeneralOffer_DL ObjDL = new ET_Admin_GeneralOffer_DL();

        public List<Tbl_Offers_Master> GetOffersList(bool deleted)
        {
            return ObjDL.GetOffersList(deleted);
        }
        public decimal ET_Admin_GeneralOffer_Add_BL(Tbl_Offers_Master obj, string prefix, bool automanual, string Contactdata,out string genCode)
        {
            return ObjDL.ET_Admin_GeneralOffer_Add_DL(obj, prefix, automanual, Contactdata,out genCode);
            // return 1;
        }
        public Tbl_Offers_Master ET_Admin_GeneralOffer_Update_GetbyID_BL(int id)
        {
            return ObjDL.ET_Admin_GeneralOffer_Update_GetbyID_DL(id);
        }
        public int ET_Admin_GeneralOffer_Restore_Delete_BL(int id, decimal Updatedby, bool type)
        {
            return ObjDL.ET_Admin_GeneralOffer_Restore_Delete_DL(id, Updatedby, type);
        }
        public List<Tbl_Offer_Details_CM> ET_Admin_GeneralOffer_View_BL(int id)
        {
            return ObjDL.ET_Admin_GeneralOffer_View_DL(id);
        }
        public Tbl_Master_Products_CM ProductDetails_BL(int id, decimal companykey)
        {
            return ObjDL.ProductDetails_DL(id, companykey);
        }
        public List<Tbl_Offer_Details_CM> ET_Admin_GeneralOffer_Details_Update_GetbyID_BL(int id)
        {
            return ObjDL.ET_Admin_GeneralOffer_Details_Update_GetbyID_DL(id);
        }

        public string CheckDuplicateCode_BL(decimal OfferID,string Offercode)
        {
            return ObjDL.CheckDuplicateCode_DL(OfferID, Offercode);
        }
        public string CheckDuplicateOfferno_BL(decimal OfferID, string txtOfferNo)
        {
            return ObjDL.CheckDuplicateOfferno_DL(OfferID, txtOfferNo);
        }        
    }
}
