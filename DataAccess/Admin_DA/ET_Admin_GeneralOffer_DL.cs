using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessEntity.EntityModels;
using DataAccess.Admin_DA;
using BusinessEntity.CustomModels;


namespace DataAccess.Admin_DA
{
    public class ET_Admin_GeneralOffer_DL
    {
        EntityClasses dbcontext = new EntityClasses();

        // Offers List and restore
        public List<Tbl_Offers_Master> GetOffersList(bool deleted)
        {
            try
            {
                var data = dbcontext.Tbl_Offers_Master.Where(m => m.DELETED == deleted).Distinct().OrderByDescending(m=>m.OFFER_ID).ToList();
                return data;
            }
            catch (Exception exe)
            {
                throw exe;
            }
        }

        // Offers insert and update
        public decimal ET_Admin_GeneralOffer_Add_DL(Tbl_Offers_Master obj, string prefix, bool automanual, string Contactdata,out string genCode)
        {
            Tbl_Offers_Master objgl = new Tbl_Offers_Master();
            decimal data = 0;
            var context = new EntityClasses();
            var transaction = context.Database.BeginTransaction();
            bool success = true;
            try
            {
                if (obj.OFFER_ID == 0)
                {
                   
                    Tbl_Offers_Master objtom = new Tbl_Offers_Master()
                    {
                        COM_ID = obj.COM_ID,
                        OFFER_ID = obj.OFFER_ID,
                        OFFER_CODE = obj.OFFER_CODE,
                        IS_SPECIFIC = obj.IS_SPECIFIC,
                        CUST_SUPP = obj.CUST_SUPP,
                        CUST_ID = obj.CUST_ID,
                        OFFER_NO = obj.OFFER_NO,
                        OFFER_DATE = obj.OFFER_DATE,
                        DESCRIPTION = obj.DESCRIPTION,
                        PLACE_DEST = obj.PLACE_DEST,
                        DESTINATION = obj.DESTINATION,
                        CONTACT_PERSON = obj.CONTACT_PERSON,
                        PAYMENT = obj.PAYMENT,
                        SHIPMENT = obj.SHIPMENT,
                        PaymentTerms=obj.PaymentTerms,
                        VALIDITY_DAYS = obj.VALIDITY_DAYS,
                        VALIDITY_DATE = obj.VALIDITY_DATE,
                        IS_ORDER = obj.IS_ORDER,
                        AGENCY_TRADING = obj.AGENCY_TRADING,
                        DELETED = false,
                        CREATED_DATE = DateTime.Now,
                        CREATED_BY = obj.CREATED_BY,
                        LAST_UPDATED_DATE = DateTime.Now,
                        LAST_UPDATED_BY = obj.LAST_UPDATED_BY,
                        COM_KEY = obj.COM_KEY
                    };
                    context.Tbl_Offers_Master.Add(objtom);
                    if (context.SaveChanges() == 0)
                    {
                        success = false;
                    }
                    data = objtom.OFFER_ID;
                    if (automanual == true)
                    {
                        string code = prefix + "0" + objtom.OFFER_ID;
                        Tbl_Offers_Master Tbl_Offers_Master = context.Tbl_Offers_Master.Single(m => m.OFFER_ID == objtom.OFFER_ID);
                        {
                            Tbl_Offers_Master.OFFER_CODE = code;
                        };
                        if (context.SaveChanges() == 0)
                        {
                            success = false;
                        }
                    }
                    objgl.OFFER_ID = objtom.OFFER_ID;
                    objgl.COM_ID = objtom.COM_ID;
                    genCode = objtom.OFFER_CODE;
                }
                else
                {
                    Tbl_Offers_Master objtom = context.Tbl_Offers_Master.Single(m => m.OFFER_ID == obj.OFFER_ID);
                    {
                        objtom.COM_ID = obj.COM_ID;
                        objtom.OFFER_ID = obj.OFFER_ID;
                        objtom.OFFER_CODE = obj.OFFER_CODE;
                        objtom.IS_SPECIFIC = obj.IS_SPECIFIC;
                        objtom.CUST_SUPP = obj.CUST_SUPP;
                        objtom.CUST_ID = obj.CUST_ID;
                        objtom.OFFER_NO = obj.OFFER_NO;
                        objtom.OFFER_DATE = obj.OFFER_DATE;
                        objtom.DESCRIPTION = obj.DESCRIPTION;
                        objtom.PaymentTerms = obj.PaymentTerms;
                        objtom.PLACE_DEST = obj.PLACE_DEST;
                        objtom.DESTINATION = obj.DESTINATION;
                        objtom.CONTACT_PERSON = obj.CONTACT_PERSON;
                        objtom.PAYMENT = obj.PAYMENT;
                        objtom.SHIPMENT = obj.SHIPMENT;
                        objtom.VALIDITY_DAYS = obj.VALIDITY_DAYS;
                        objtom.VALIDITY_DATE = obj.VALIDITY_DATE;
                        objtom.IS_ORDER = obj.IS_ORDER;
                        objtom.AGENCY_TRADING = obj.AGENCY_TRADING;
                        objtom.LAST_UPDATED_DATE = DateTime.Now;
                        objtom.LAST_UPDATED_BY = obj.LAST_UPDATED_BY;
                    };
                    if (context.SaveChanges() == 0)
                    {
                        success = false;
                    }
                    data = obj.OFFER_ID;
                    objgl.OFFER_ID = objtom.OFFER_ID;
                    objgl.COM_ID = objtom.COM_ID;
                    genCode = objtom.OFFER_CODE;
                }

                // Delete previous Offer data
                Tbl_Offer_Details objdeletecontact = new Tbl_Offer_Details();
                context.Tbl_Offer_Details.RemoveRange(context.Tbl_Offer_Details.Where(m => m.OFFER_ID == objgl.OFFER_ID));
                context.SaveChanges();

                // Insert new contacts data
                string[] ChildRow = Contactdata.Split('|');
                for (int i = 0; i < ChildRow.Length - 1; i++)
                {
                    string[] ChildRecord = ChildRow[i].Split('}');
                    if (ChildRecord[6] == "")
                        ChildRecord[6] = "0";
                    var OfferProduct = (from a in context.Tbl_SupplierPriceList
                                        join b in context.Tbl_SupplierPriceListDetails on a.SPL_ID equals b.SLD_ID
                                        where a.SPL_SupplierKey == objgl.COM_ID && b.SLD_Status==true
                                        select new SUppProduct
                                        {
                                            PD_ProductID = b.SLD_ProductID ??0
                                        }).ToList();
                    var data2 = (from z in OfferProduct select z.PD_ProductID).ToList();
                    if (data2.Count > 0)
                    {
                        if (data2.Contains(Convert.ToDecimal(ChildRecord[0])))
                        {
                            var ActiveRecords = context.Tbl_SupplierPriceList.Where(m => m.SPL_SupplierKey == objgl.COM_ID && m.DELETED == false).ToList();
                            if (ActiveRecords.Count() > 0)
                            {
                                var splId = ActiveRecords[0].SPL_ID;
                                var deactive = context.Tbl_SupplierPriceListDetails.Where(m => m.SLD_ID == splId).ToList();
                                var data3 = (from z in deactive select z.SLD_ProductID).ToList();
                                var articleNo = "0";
                                if (data3.Contains(Convert.ToDecimal(ChildRecord[0])))
                                {
                                    articleNo = deactive[0].SLD_ArticleNo;
                                    var productId = Convert.ToDecimal(ChildRecord[0]);
                                    var set = context.Tbl_SupplierPriceListDetails.Where(m => m.SLD_ID == splId && m.SLD_ProductID == productId).ToList();

                                    set.ForEach(m => m.SLD_Status = false);
                                }
                                Tbl_SupplierPriceListDetails objSupplierdetails = new Tbl_SupplierPriceListDetails()
                                {
                                    SLD_ID = Convert.ToInt32(splId),
                                    SLD_ProductID = Convert.ToInt32(ChildRecord[0]),
                                    SLD_ArticleNo = articleNo,
                                    SLD_UOM = ChildRecord[3],
                                    SLD_UnitPrice = Convert.ToDecimal(ChildRecord[5]),
                                    SLD_Status = true

                                };
                                context.Tbl_SupplierPriceListDetails.Add(objSupplierdetails);
                                context.SaveChanges();
                            }
                        }
                        else
                        {
                            var ActiveRecords1 = context.Tbl_SupplierPriceList.Where(m => m.SPL_SupplierKey == objgl.COM_ID && m.DELETED == false).ToList();
                            var splId1 = ActiveRecords1[0].SPL_ID;
                            var deactive1 = context.Tbl_SupplierPriceListDetails.Where(m => m.SLD_ID == splId1).ToList();
                            var articleNo1 = deactive1[0].SLD_ArticleNo;
                            Tbl_SupplierPriceListDetails objSupplierdetails = new Tbl_SupplierPriceListDetails()
                            {
                                SLD_ID = Convert.ToInt32(splId1),
                                SLD_ProductID = Convert.ToInt32(ChildRecord[0]),
                                //SLD_ArticleNo = articleNo1,
                                SLD_UOM = ChildRecord[3],
                                SLD_UnitPrice = Convert.ToDecimal(ChildRecord[5]),
                                SLD_Status = true

                            };
                            context.Tbl_SupplierPriceListDetails.Add(objSupplierdetails);
                            context.SaveChanges();
                        }
                    }
                    else
                    {
                        Tbl_SupplierPriceList Objmc = new Tbl_SupplierPriceList()
                        {
                            SPL_Code = "",
                            SPL_SupplierKey = objgl.COM_ID,
                            CREATED_BY = obj.CREATED_BY,
                            LAST_UPDATED_BY = obj.CREATED_BY,
                            CREATED_DATE = DateTime.Now,
                            DELETED = false,
                            COM_KEY = obj.COM_KEY
                        };
                        context.Tbl_SupplierPriceList.Add(Objmc);
                        context.SaveChanges();
                        int len = 10 - (prefix + Objmc.SPL_ID).Length;
                        string code = prefix + new String('0', len) + Objmc.SPL_ID;
                        Tbl_SupplierPriceList Obj_Tbl_SupplierPriceList = context.Tbl_SupplierPriceList.Single(m => m.SPL_ID == Objmc.SPL_ID);
                        {
                            Obj_Tbl_SupplierPriceList.SPL_Code = code;
                        };
                        context.SaveChanges();
                        Tbl_SupplierPriceListDetails objSupplierdetails = new Tbl_SupplierPriceListDetails()
                        {
                            SLD_ID = Objmc.SPL_ID,
                            SLD_ProductID = Convert.ToInt32(ChildRecord[0]),
                            //SLD_ArticleNo = articleNo1,
                            SLD_UOM = ChildRecord[3],
                            SLD_UnitPrice = Convert.ToDecimal(ChildRecord[5]),
                            SLD_Status = true

                        };
                        context.Tbl_SupplierPriceListDetails.Add(objSupplierdetails);
                        context.SaveChanges();
                    }
                   
                    
                    Tbl_Offer_Details objcontact = new Tbl_Offer_Details()
                    {
                        OFFER_ID = objgl.OFFER_ID,
                        PRODUCT_ID = Convert.ToDecimal(ChildRecord[0]),
                        CATEGORY_NAME = ChildRecord[1],
                        CURRENCY_ID = Convert.ToInt16(ChildRecord[4]),
                        PRICE = Convert.ToDecimal(ChildRecord[5]),
                        QUANTITY = Convert.ToDecimal(ChildRecord[6]),
                        CREATED_BY = Convert.ToDecimal(obj.CREATED_BY),
                        CREATED_DATE = DateTime.Now,
                        LAST_UPDATED_BY = obj.LAST_UPDATED_BY,
                        LAST_UPDATED_DATE = DateTime.Now,
                        COM_KEY = Convert.ToDecimal(obj.COM_KEY),
                        DELETED = false,
                    };
                    context.Tbl_Offer_Details.Add(objcontact);
                    context.SaveChanges();
                }
                if (success)
                {
                    transaction.Commit();
                }
                else
                {
                    data = 0;
                    transaction.Rollback();
                }
            }
            catch (Exception ex)
            {
                data = 0;
                transaction.Rollback();
                throw ex;
            }
            finally
            {
                transaction.Dispose();
                context.Dispose();
            }
            return data;
        }

        public Tbl_Offers_Master ET_Admin_GeneralOffer_Update_GetbyID_DL(int id)
        {
            try
            {
                //var data = (from offer in dbcontext.Tbl_Offers_Master
                //            join offerdetails in dbcontext.Tbl_Offer_Details on offer.OFFER_ID equals offerdetails.OFFER_ID
                //            where offer.OFFER_ID == id).FirstOrDefault();

                return dbcontext.Tbl_Offers_Master.Single(m => m.OFFER_ID == id);
            }
            catch (Exception exe)
            {
                throw exe;
            }
        }
        public int ET_Admin_GeneralOffer_Restore_Delete_DL(int id, decimal Updatedby, bool type)
        {
            int result = 0;
            try
            {
                Tbl_Offers_Master obj = dbcontext.Tbl_Offers_Master.Single(m => m.OFFER_ID == id);
                {
                    obj.DELETED = type;
                    obj.DELETED_DATE = DateTime.Now;
                    obj.LAST_UPDATED_BY = Updatedby;
                }
                result = dbcontext.SaveChanges();
            }
            catch (Exception exe)
            {
                throw exe;
            }
            return result;
        }

        public List<Tbl_Offer_Details_CM> ET_Admin_GeneralOffer_View_DL(int id)
        {
            try
            {
                var data = (from offer in dbcontext.Tbl_Offer_Details
                            join product in dbcontext.Tbl_Product_Master on offer.PRODUCT_ID equals product.P_ID
                            join c in dbcontext.tbl_LookUp on product.P_UOM equals c.LU_Code into m
                            from n in m.DefaultIfEmpty()
                            join Moffer in dbcontext.Tbl_Offers_Master on offer.OFFER_ID equals Moffer.OFFER_ID
                            join cry in dbcontext.Tbl_Currency_Master on offer.CURRENCY_ID equals cry.CURRENCY_ID
                            join pay in dbcontext.Tbl_Payment_Terms on Moffer.PaymentTerms equals pay.PT_ID
                            where offer.OFFER_ID == id && offer.DELETED == false && n.LU_Type == 2
                            select new Tbl_Offer_Details_CM
                            {
                                COM_ID = Moffer.COM_ID ,
                                OFFER_ID = offer.OFFER_ID ,
                                PRODUCT_ID = offer.PRODUCT_ID ,
                                CATEGORY_NAME = offer.CATEGORY_NAME,
                                PRODUCT_NAME = product.P_Name,
                                PRICE = offer.PRICE ,
                                QUANTITY = offer.QUANTITY??0,
                                UOM_NAME = n.LU_Description,
                                CURRENCY_NAME = cry.CURRENCY_NAME,
                                PaymentName = pay.PT_Name,
                                SHORT_NAME = product.P_ShortName,
                                CUST_SUPP = Moffer.CUST_SUPP,
                                CONTACT_PERSON = Moffer.CONTACT_PERSON ,
                                OFFER_NO = Moffer.OFFER_NO,
                                OFFER_DATE = Moffer.OFFER_DATE ,
                                PLACE_DEST = Moffer.PLACE_DEST,
                                DESTINATION = Moffer.DESTINATION,
                                DESCRIPTION = Moffer.DESCRIPTION,
                                SHIPMENT = Moffer.SHIPMENT,
                                
                                VALIDITY_DATE = Moffer.VALIDITY_DATE,
                                VALIDITY_DAYS = Moffer.VALIDITY_DAYS ,
                                CUST_ID = Moffer.CUST_ID,
                                IS_SPECIFIC = Moffer.IS_SPECIFIC ?? true
                            }).ToList();
                return data;
            }
            catch (Exception exe)
            {
                throw exe;
            }
        }

        public Tbl_Master_Products_CM ProductDetails_DL(int id, decimal companykey)
        {
            try
            {
                dbcontext.Configuration.ProxyCreationEnabled = false;

                var data = (from item in dbcontext.Tbl_Product_Master
                            join category in dbcontext.Tbl_Master_Category on item.P_CategoryID equals category.CAT_CODE
                            join c in dbcontext.tbl_LookUp on item.P_UOM equals c.LU_Code
                            into m
                            from n in m.DefaultIfEmpty()
                            where item.P_ID == id && item.COM_KEY == category.COM_KEY && item.DELETED == false && n.LU_Type == 2
                            select new Tbl_Master_Products_CM
                            {
                                PRODUCT_NAME = item.P_ShortName,
                                CAT_NAME = category.CAT_NAME,
                                UOM_Text =n.LU_Description ,
                                PRODUCT_ID = item.P_ID,
                                CATEGORY_ID = category.CAT_CODE
                            }).FirstOrDefault();
                return data;
            }
            catch (Exception exe)
            {
                throw exe;
            }
        }

        public List<Tbl_Offer_Details_CM> ET_Admin_GeneralOffer_Details_Update_GetbyID_DL(int id)
        {
            try
            {
                var data = (from offer in dbcontext.Tbl_Offer_Details
                            join product in dbcontext.Tbl_Product_Master on offer.PRODUCT_ID equals product.P_ID
                            join c in dbcontext.tbl_LookUp on product.P_UOM equals c.LU_Code
                                  into m
                            from n in m.DefaultIfEmpty()
                            where offer.OFFER_ID == id && offer.DELETED == false && n.LU_Type == 2
                            select new Tbl_Offer_Details_CM
                            {
                                OFFER_ID = offer.OFFER_ID ,
                                PRODUCT_ID = offer.PRODUCT_ID ,
                                CATEGORY_NAME = offer.CATEGORY_NAME,
                                PRODUCT_NAME = product.P_ShortName,
                                PRICE = offer.PRICE,
                                QUANTITY = offer.QUANTITY ?? 0,
                                UOM_NAME = n.LU_Description,
                                CURRENCY_ID = offer.CURRENCY_ID
                            }).ToList();
                return data;
            }
            catch (Exception exe)
            {
                throw exe;
            }
        }

        public string CheckDuplicateCode_DL(decimal OfferID, string Offercode)
        {
            try
            {
                if (OfferID == 0)
                {
                    var count = dbcontext.Tbl_Offers_Master.Where(m => m.OFFER_ID == OfferID).Count();
                    if (count > 0)
                    {
                        return "Exist";
                    }
                }
                else
                {
                    var count = dbcontext.Tbl_Offers_Master.Where(m => m.OFFER_ID != OfferID && m.OFFER_CODE == Offercode).Count();
                    if (count > 0)
                    {
                        return "Exist";
                    }
                }
                return "";
            }
            catch (Exception exe)
            {
                throw exe;
            }
        }

        public string CheckDuplicateOfferno_DL(decimal OfferID, string txtOfferNo)
        {
            try
            {
                if (OfferID == 0)
                {
                    var count = dbcontext.Tbl_Offers_Master.Where(m => m.OFFER_ID == OfferID).Count();
                    if (count > 0)
                    {
                        return "Exist";
                    }
                }
                else
                {
                    var count = dbcontext.Tbl_Offers_Master.Where(m => m.OFFER_ID != OfferID && m.OFFER_NO == txtOfferNo).Count();
                    if (count > 0)
                    {
                        return "Exist";
                    }
                }
                return "";
            }
            catch (Exception exe)
            {
                throw exe;
            }
        }

    }
}
