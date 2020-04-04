using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess;
using DataAccess.Admin_DA;
using BusinessEntity.EntityModels;
using System.Data;
using System.Data.Entity;
using BusinessEntity.CustomModels;

namespace DataAccess.Admin_DA
{
    public class ET_Sales_OrderDetails_DL
    {
        EntityClasses dbcontext = new EntityClasses();

        //public List<Tbl_Order_Details_CM> Tbl_OrderList_DL(int Comkey, bool type)
        //{
        //    try
        //    {
        //        //var data = dbcontext.Tbl_Master_Order.Where(m => m.COM_KEY == Comkey && m.DELETED == type).ToList();

        //        var data = (from Ol in dbcontext.Tbl_Master_Order
        //                    join Od in dbcontext.Tbl_Order_Details on Ol.SO_ID equals Od.AGEN_TRAD_PO_ID
        //                    join Mu in dbcontext.Tbl_Master_User on Ol.SO_SalesPersonID equals Mu.USER_ID
        //                    join Cgry in dbcontext.Tbl_Master_Category on Od.CATEGORY_ID equals Cgry.CAT_CODE
        //                    join Uom in dbcontext.Tbl_Master_UOM on Od.UOM equals Uom.UOM_ID
        //                    select new Tbl_Order_Details_CM
        //                    {
        //                        SO_ID = Ol.SO_ID,
        //                        USER_NAME = Mu.USER_NAME,
        //                        AGEN_TRAD_PO = Od.AGEN_TRAD_PO,
        //                        CAT_NAME = Cgry.CAT_NAME,
        //                        UOM_NAME = Uom.UOM_NAME,
        //                        QUANTITY = Od.QUANTITY,
        //                        PRICE = Od.PRICE
        //                    }).ToList();
        //        return data;
        //    }
        //    catch (Exception exe)
        //    {
        //        throw exe;
        //    }
        //}

        public List<Tbl_Master_User> Sales_person_dropdown_DL(int com_key)
        {
            dbcontext.Configuration.ProxyCreationEnabled = false;
            try
            {
                var ObjSales_Org = dbcontext.Tbl_Sales_Organization.Where(m => m.COM_KEY == com_key && m.DELETED==false).ToList();
                string s = "";
                for (int i = 0; i < ObjSales_Org.Count(); i++)
                {
                    if (i == 0)
                    {
                        s = ObjSales_Org[i].ORG_EMPLOYEE_IDS.ToString();
                    }
                    else
                    {
                        s = s + "," + ObjSales_Org[i].ORG_EMPLOYEE_IDS.ToString();
                    }
                }
                List<decimal> UID = s.Split(',').Select(m => Convert.ToDecimal(m)).ToList();
                var Users = dbcontext.Tbl_Master_User.Where(m => m.USER_ID > 0 && UID.Contains(m.USER_ID) && m.COM_KEY == com_key).ToList();

                return Users;
            }
            catch (Exception exe)
            {
                throw exe;
            }
        }
        public DataTable OrgEmp_DL(int com_key,decimal orgtype)
        {
            dbcontext.Configuration.ProxyCreationEnabled = false;

            DataTable dt_Org = new DataTable();
            if (dt_Org.Columns.Count == 0)
            {
                dt_Org.Columns.Add("OrgID");
                dt_Org.Columns.Add("OrgName");
                dt_Org.Columns.Add("EmpID");
            }
            var ObjSales_Org = dbcontext.Tbl_Sales_Organization.Where(m => m.COM_KEY == com_key && m.sales_Organization== orgtype).ToList();
            string s = "";
            for (int i = 0; i < ObjSales_Org.Count(); i++)
            {
                List<decimal> emps = ObjSales_Org[i].ORG_EMPLOYEE_IDS.ToString().Split(',').Select(m => Convert.ToDecimal(m)).ToList();
                for (int j = 0; j < emps.Count(); j++)
                {
                    DataRow dr = dt_Org.NewRow();
                    dr["OrgID"] = ObjSales_Org[i].ORG_ID;
                    dr["OrgName"] = ObjSales_Org[i].ORG_NAME;
                    dr["EmpID"] = emps[j];
                    dt_Org.Rows.Add(dr);
                }
            }
            return dt_Org;
        }

        /// <summary>
        /// Clones an Sales Order
        /// </summary>
        /// <param name="orderId">Original Order id</param>
        /// <param name="prefix">Prefix for Naming the Order.</param>
        /// <param name="orderCode">Order Code(ORDxxxxxx)</param>
        /// <returns></returns>
        public decimal ET_Sales_CloneOrder_DL(int orderId, string prefix,out string orderCode)
        {
            //var originalEntity = dbcontext.Tbl_Master_Order.AsNoTracking()
            //                 .FirstOrDefault(e => e.SO_ID == orderId);
            //dbcontext.Tbl_Master_Order.Add(originalEntity);
            //dbcontext.SaveChanges();
            decimal k = 0;
            var context = new EntityClasses();
            var transaction = context.Database.BeginTransaction();
            bool success = true;
            Tbl_Master_Order originalOrder = dbcontext.Tbl_Master_Order.Single(m => m.SO_ID == orderId);
            Tbl_Master_Order objtom = new Tbl_Master_Order()
                {
                    SO_ID = 0,
                    SO_Code = originalOrder.SO_Code,
                    SO_OrderDate = DateTime.Now,
                    SO_SalesPersonID = originalOrder.SO_SalesPersonID,
                    SO_ORGID = originalOrder.SO_ORGID,
                    SO_OrderType = originalOrder.SO_OrderType,
                    SO_CutomerID = originalOrder.SO_CutomerID,
                    SO_SupplierID = originalOrder.SO_SupplierID,
                    SO_CusPONO = originalOrder.SO_CusPONO,
                    SO_PriceType = originalOrder.SO_PriceType,
                    SO_SupSCNO = originalOrder.SO_SupSCNO,
                    SO_CusPODate = DateTime.Now,
                    SO_SupSCDate = DateTime.Now,
                    SO_CusCurrency = originalOrder.SO_CusCurrency,
                    SO_CusPaymentTermID = originalOrder.SO_CusPaymentTermID,
                    SO_CusDeliveryTerms = originalOrder.SO_CusDeliveryTerms,
                    SO_Commision = originalOrder.SO_Commision,
                    SO_Remarks = originalOrder.SO_Remarks,
                    DELETED = false,
                    CREATED_BY = originalOrder.CREATED_BY,
                    CREATED_DATE = DateTime.Now,
                    SO_Approval = 0,
                    COM_KEY = originalOrder.COM_KEY,
                    SO_QuotationID = originalOrder.SO_QuotationID,
                    SO_Approver = originalOrder.SO_Approver,
                    SO_Discount = originalOrder.SO_Discount,
                    SO_TaxRemarks = originalOrder.SO_TaxRemarks,
                    SO_TaxApplicable = originalOrder.SO_TaxApplicable,
                    SO_ProductType = originalOrder.SO_ProductType,
                    Q_CatalogId = originalOrder.Q_CatalogId
                };
                context.Tbl_Master_Order.Add(objtom);
                //context.SaveChanges();
                if (context.SaveChanges() == 0)
                {
                    success = false;
                }

            string code = prefix + "0" + objtom.SO_ID;
            Tbl_Master_Order Tbl_Master_Order = context.Tbl_Master_Order.Single(m => m.SO_ID == objtom.SO_ID);
            {
                Tbl_Master_Order.SO_Code = code;
            };
            //context.SaveChanges();
            if (context.SaveChanges() == 0)
            {
                success = false;
            }
            k = objtom.SO_ID;
            orderCode = objtom.SO_Code;

            var orderDetails = (from Ol in dbcontext.Tbl_Master_Order
                        join Od in dbcontext.Tbl_Order_Details on Ol.SO_ID equals Od.AGEN_TRAD_PO_ID
                        join Pct in dbcontext.Tbl_Product_Master on Od.PRODUCT_ID equals Pct.P_ID
                        where Ol.SO_ID == orderId
                        select new Tbl_Order_Details_CM
                        {
                            SO_Serial = 1,
                            SO_ID = Ol.SO_ID,
                            ORDER_ID = Od.ORDER_ID,
                            UOM_NAME = (dbcontext.tbl_LookUp.FirstOrDefault(m => m.LU_Type == 2 && m.LU_Code == Od.UOM).LU_Description),
                            UOM_CODE =Od.UOM,
                            QUANTITY = Od.QUANTITY,
                            PRICE = Od.PRICE,
                            PRODUCT_ID = Pct.P_ID,
                            SHORT_NAME = Pct.P_ShortName,
                            SUPPLIEROFFER_ID = Od.SUPPLIEROFFER_ID,
                            OrderDescription = Od.OrderDescription,
                            CustomerDesc = Od.CustomerDes,
                            P_PackingQuantityUOM = Pct.P_PackingQuantityUOM,
                            DesignDetail = Od.DesignDetail,
                            DiscountPer = Od.DiscountPer ?? 0

                        }).ToList();

            for (int i = 0; i < orderDetails.Count; i++)
            {
                decimal suppliedofferid = 0;
                Tbl_Order_Details newOrderDetaills = new Tbl_Order_Details()
                {
                    AGEN_TRAD_PO_ID = objtom.SO_ID,
                    AGEN_TRAD_PO = objtom.SO_OrderType.ToString(),
                    PRODUCT_ID = orderDetails[i].PRODUCT_ID,
                    CATEGORY_ID = Convert.ToDecimal(orderDetails[i].CATEGORY_ID),
                    UOM = orderDetails[0].UOM_CODE,
                    CURRENCY_ID = Convert.ToDecimal(0),
                    PACKING = "",
                    QUANTITY = Convert.ToDecimal(orderDetails[i].QUANTITY),
                    PRICE = Convert.ToDecimal(orderDetails[i].PRICE),
                    Selling_Price = 0,
                    SUPPLIEROFFER_ID = suppliedofferid,
                    IS_CUSTOMEROFFER = false,
                    CUSTOMEROFFER_ID = 0,
                    IS_SUPPLIEROFFER = false,
                    CustomerDes = orderDetails[i].CustomerDesc,
                    OrderDescription = orderDetails[i].OrderDescription,
                    DesignDetail = orderDetails[i].DesignDetail,
                    DiscountPer = orderDetails[i].DiscountPer
                };
                context.Tbl_Order_Details.Add(newOrderDetaills);
            }
            if (context.SaveChanges() != orderDetails.Count)
            {
                success = false;
            }
            if (success)
            {
                transaction.Commit();
            }
            else
            {
                k = 0;
                transaction.Rollback();
            }
            return k;
        }

        public decimal ET_Sales_OrderDetails_Add_DL(Tbl_Master_Order obj, string prefix, bool automanual, string Orderdetails,out string OrdCode)
        {
            Tbl_Master_Order tomgl = new Tbl_Master_Order();
            decimal k = 0;
            var context = new EntityClasses();
            var transaction = context.Database.BeginTransaction();
            bool success = true;
            try
            {
                
                
                if (obj.SO_ID == 0)
                {
                    
                    Tbl_Master_Order objtom = new Tbl_Master_Order()
                    {
                        SO_ID = obj.SO_ID,
                        SO_Code = obj.SO_Code,
                        SO_OrderDate = obj.SO_OrderDate,
                        SO_SalesPersonID = obj.SO_SalesPersonID,
                        SO_ORGID = obj.SO_ORGID,
                        SO_OrderType = obj.SO_OrderType,
                        SO_CutomerID = obj.SO_CutomerID,
                        SO_SupplierID = obj.SO_SupplierID,
                        SO_CusPONO = obj.SO_CusPONO,
                        SO_PriceType=obj.SO_PriceType,
                        SO_SupSCNO = obj.SO_SupSCNO,
                        SO_CusPODate = obj.SO_CusPODate,
                        SO_SupSCDate = obj.SO_SupSCDate,
                        SO_CusCurrency = obj.SO_CusCurrency,
                        SO_CusPaymentTermID = obj.SO_CusPaymentTermID,
                        SO_CusDeliveryTerms = obj.SO_CusDeliveryTerms,
                        SO_Commision = obj.SO_Commision,
                        SO_Remarks = obj.SO_Remarks,
                        DELETED = obj.DELETED,
                        CREATED_BY = obj.CREATED_BY,
                        CREATED_DATE = DateTime.Now,
                        SO_Approval = obj.SO_Approval,
                        COM_KEY = obj.COM_KEY,
                        SO_QuotationID=obj.SO_QuotationID,
                        SO_Approver=obj.SO_Approver,
                        SO_Discount= obj.SO_Discount,
                        SO_TaxRemarks = obj.SO_TaxRemarks,
                        SO_TaxApplicable = obj.SO_TaxApplicable,
                        SO_ProductType = obj.SO_ProductType,
                        Q_CatalogId = obj.Q_CatalogId
                };
                    context.Tbl_Master_Order.Add(objtom);
                    //context.SaveChanges();
                    if (context.SaveChanges() == 0)
                    {
                        success = false;
                    }

                    if (automanual == true)
                    {
                        string code = prefix + "0" + objtom.SO_ID;
                        Tbl_Master_Order Tbl_Master_Order = context.Tbl_Master_Order.Single(m => m.SO_ID == objtom.SO_ID);
                        {
                            Tbl_Master_Order.SO_Code = code;
                        };
                        //context.SaveChanges();
                        if (context.SaveChanges() == 0)
                        {
                            success = false;
                        }
                    }
                    k = objtom.SO_ID;
                    tomgl.SO_ID = objtom.SO_ID;
                    OrdCode = objtom.SO_Code;
                }
                else
                {
                    
                    
                        Tbl_Master_Order objtom = context.Tbl_Master_Order.Single(m => m.SO_ID == obj.SO_ID);
                    {
                        objtom.SO_ID = obj.SO_ID;
                        objtom.SO_Code = obj.SO_Code;
                        objtom.SO_OrderDate = obj.SO_OrderDate;
                        objtom.SO_SalesPersonID = obj.SO_SalesPersonID;
                        objtom.SO_ORGID = obj.SO_ORGID;
                        objtom.SO_OrderType = obj.SO_OrderType;
                        objtom.SO_CutomerID = obj.SO_CutomerID;
                        objtom.SO_SupplierID = obj.SO_SupplierID;
                        objtom.SO_CusPONO = obj.SO_CusPONO;
                        objtom.SO_PriceType = obj.SO_PriceType;
                        objtom.SO_CusPODate = obj.SO_CusPODate;
                        objtom.SO_SupSCNO = obj.SO_SupSCNO;
                        objtom.SO_SupSCDate = obj.SO_SupSCDate;
                        objtom.SO_CusCurrency = obj.SO_CusCurrency;
                        objtom.SO_CusPaymentTermID = obj.SO_CusPaymentTermID;
                        objtom.SO_CusDeliveryTerms = obj.SO_CusDeliveryTerms;
                        objtom.SO_Commision = obj.SO_Commision;
                        objtom.SO_Remarks = obj.SO_Remarks;
                        objtom.LAST_UPDATED_BY = obj.LAST_UPDATED_BY;
                        objtom.LAST_UPDATED_DATE = DateTime.Now;
                        objtom.DELETED = obj.DELETED;
                        objtom.SO_QuotationID = obj.SO_QuotationID;
                        objtom.SO_Approver = obj.SO_Approver;
                        objtom.SO_Discount = obj.SO_Discount;
                        objtom.SO_TaxRemarks = obj.SO_TaxRemarks;
                        objtom.SO_TaxApplicable = obj.SO_TaxApplicable;
                        objtom.SO_ProductType = obj.SO_ProductType;
                        objtom.Q_CatalogId = obj.Q_CatalogId;
                    };
                    //context.SaveChanges();
                    if (context.SaveChanges() == 0)
                    {
                        success = false;
                    }
                    k = obj.SO_ID;
                    tomgl.SO_ID = objtom.SO_ID;
                    OrdCode = objtom.SO_Code;
                }


                if (obj.SO_ID == 0)
                {
                    // Insert new contacts data
                    string[] ChildRow = Orderdetails.Split('|');
                    string Description = "";
                    string Remarks = "";
                    decimal discount = 0;
                    string designDetail = string.Empty;
                    // var Categoryid = "";
                    // var Productid = "";
                    for (int i = 0; i < ChildRow.Length - 1; i++)
                    {
                        string[] ChildRecord = ChildRow[i].Split('}');

                        var Productid = Convert.ToDecimal(ChildRecord[0]);

                        var Categoryid = (from id in context.Tbl_Product_Master
                                          where id.P_ID == Productid
                                          select id.P_CategoryID).FirstOrDefault();

                        var Uom = (from id in context.Tbl_Product_Master
                                   where id.P_ID == Productid
                                   select id.P_UOM).FirstOrDefault();

                        decimal suppliedofferid = 0;
                        if (obj.SO_OrderType == 1)
                        {
                            Description = ChildRecord[7];
                            Remarks = ChildRecord[8];
                            discount = 0;
                            designDetail = string.Empty;
                        }
                        else
                        {
                            Description = ChildRecord[8];
                            Remarks = ChildRecord[9];
                            discount = Convert.ToDecimal(ChildRecord[7]);
                            designDetail = ChildRecord[10];
                        }
                        Tbl_Order_Details objorder = new Tbl_Order_Details()
                        {
                            AGEN_TRAD_PO_ID = tomgl.SO_ID,
                            AGEN_TRAD_PO = obj.SO_OrderType.ToString(),
                            PRODUCT_ID = Productid,
                            CATEGORY_ID = Convert.ToDecimal(Categoryid),
                            UOM = Uom,
                            CURRENCY_ID = Convert.ToDecimal(0),
                            PACKING = "",
                            QUANTITY = Convert.ToDecimal(ChildRecord[6]),
                            PRICE = Convert.ToDecimal(ChildRecord[5]),
                            Selling_Price = 0,
                            SUPPLIEROFFER_ID = suppliedofferid,
                            IS_CUSTOMEROFFER = false,
                            CUSTOMEROFFER_ID = 0,
                            IS_SUPPLIEROFFER = false,
                            CustomerDes= Remarks,
                            OrderDescription = Description,
                            DesignDetail= designDetail,
                            DiscountPer= discount
                        };
                        context.Tbl_Order_Details.Add(objorder);
                    }
                    if (context.SaveChanges() != ChildRow.Length - 1)
                    {
                        success = false;
                    }
                    if (success)
                    {
                        transaction.Commit();
                    }
                    else
                    {
                        k = 0;
                        transaction.Rollback();
                    }
                }
                else
                {
                    var oldData = context.Tbl_Order_Details.Where(m => m.AGEN_TRAD_PO_ID == tomgl.SO_ID);
                    string[] ChildRow = Orderdetails.Split('|');
                    decimal[] products = new decimal[ChildRow.Length - 1];
                    string Description = "";
                    string Remarks = "";
                    decimal discount=0;
                    string designDetail = string.Empty;
                    for (int i = 0; i < ChildRow.Length - 1; i++)
                    {
                        string[] ChildRecord = ChildRow[i].Split('}');

                        var Productid = Convert.ToDecimal(ChildRecord[0]);
                        var Categoryid = (from id in context.Tbl_Product_Master
                                          where id.P_ID == Productid
                                          select id.P_CategoryID).FirstOrDefault();

                        var Uom = (from id in context.Tbl_Product_Master
                                   where id.P_ID == Productid
                                   select id.P_UOM).FirstOrDefault();
                        //decimal suppliedofferid = Convert.ToDecimal(ChildRecord[10]);
                        decimal suppliedofferid = 0;
                        if (ChildRecord[3] != null && ChildRecord[3] != "")
                        {
                            products[i] = Convert.ToDecimal(ChildRecord[3]);
                        }
                        if (obj.SO_OrderType == 1)
                        {
                            Description = ChildRecord[7];
                            Remarks = ChildRecord[8];
                            discount = 0;
                            designDetail = string.Empty;
                        }
                        else
                        {
                            Description = ChildRecord[8];
                            Remarks = ChildRecord[9];
                            discount = Convert.ToDecimal(ChildRecord[7]);
                            designDetail = ChildRecord[10];
                        }
                        //Ravi:Checking the Order code rather than the ChildRow table.
                        //if (oldData.Select(a => a.PRODUCT_ID).Contains(Productid) && ChildRecord[2]!="")
                        if (oldData.Select(a => a.PRODUCT_ID).Contains(Productid) && ChildRecord[3] != "")
                        {
                            //Update Detail
                            decimal orderrefId = Convert.ToDecimal(ChildRecord[3]);
                            //decimal orderrefId = Convert.ToDecimal(obj.SO_ID);
                            IQueryable<Tbl_Order_Details> oldDataList= oldData.Where(a => a.PRODUCT_ID == Productid && a.ORDER_ID == orderrefId);
                            if (oldDataList != null)
                            {
                                Tbl_Order_Details objorddetails = context.Tbl_Order_Details.SingleOrDefault(m => m.ORDER_ID == oldDataList.Select(a => a.ORDER_ID).FirstOrDefault());
                                if (objorddetails != null)
                                {

                                    objorddetails.QUANTITY = Convert.ToDecimal(ChildRecord[6]);
                                    objorddetails.PRICE = Convert.ToDecimal(ChildRecord[5]);
                                    objorddetails.SUPPLIEROFFER_ID = suppliedofferid;
                                    objorddetails.OrderDescription = Description;
                                    objorddetails.CustomerDes = Remarks;
                                    objorddetails.DesignDetail = designDetail;
                                    objorddetails.DiscountPer = discount;
                                    context.SaveChanges();
                                }
                            }
                        }
                        else
                        {
                            //Insert Detail
                            Tbl_Order_Details objorder = new Tbl_Order_Details()
                            {
                                AGEN_TRAD_PO_ID = tomgl.SO_ID,
                                AGEN_TRAD_PO = obj.SO_OrderType.ToString(),
                                PRODUCT_ID = Convert.ToDecimal(ChildRecord[0]),
                                CATEGORY_ID = Convert.ToDecimal(Categoryid),
                                UOM = Uom,
                                CURRENCY_ID = Convert.ToDecimal(0),
                                PACKING = "",
                                QUANTITY = Convert.ToDecimal(ChildRecord[6]),
                                PRICE = Convert.ToDecimal(ChildRecord[5]),
                                Selling_Price = 0,
                                SUPPLIEROFFER_ID = suppliedofferid,
                                IS_CUSTOMEROFFER = false,
                                CUSTOMEROFFER_ID = 0,
                                IS_SUPPLIEROFFER = false,
                                OrderDescription = Description,
                                CustomerDes = Remarks,
                                DesignDetail = designDetail,
                                DiscountPer= discount
                            };
                            context.Tbl_Order_Details.Add(objorder);
                            if (context.SaveChanges() == 0)
                            {
                                success = false;
                            }

                        }
                    }
                    if (products.Count() > 0)
                    {
                        // Delete previous Offer data
                        //var delOrderdetails = context.Tbl_Order_Details.Where(m => m.AGEN_TRAD_PO_ID == tomgl.SO_ID && !products.Contains(m.ORDER_ID));
                        //if (delOrderdetails.Count() > 0)
                        //{
                        //    context.Tbl_Order_Details.RemoveRange(delOrderdetails);
                        //    context.SaveChanges();
                        //}
                        if (success)
                        {
                            transaction.Commit();
                        }
                        else
                        {
                            k = 0;
                            transaction.Rollback();
                        }
                    }
                }
            }
            catch (Exception exe)
            {
                
                k = 0;
                transaction.Rollback();
                throw exe;
            }
            finally
            {
                transaction.Dispose();
                context.Dispose();
            }

            return k;
        }

        public List<Tbl_Payment_Terms> Payment_terms_dropdown_DL(int com_key)
        {
            dbcontext.Configuration.ProxyCreationEnabled = false;
            try
            {
                var data = dbcontext.Tbl_Payment_Terms.Where(m => m.COM_KEY == com_key).OrderBy(m=>m.PT_Name).ToList();
                return data;
            }
            catch (Exception exe)
            {
                throw exe;
            }
        }

        public Tbl_Master_Order ET_Sales_OrderDetails_Update_GetbyID_DL(int id)
        {
            try
            {
                return dbcontext.Tbl_Master_Order.Single(m => m.SO_ID == id);
            }
            catch (Exception exe)
            {
                throw exe;
            }
        }
        public int ET_Sales_OrderDetails_Restore_Delete_DL(int id, decimal Updatedby, bool type)
        {
            int result = 0;
            try
            {
                Tbl_Master_Order obj = dbcontext.Tbl_Master_Order.Single(m => m.SO_ID == id);
                {
                    obj.DELETED = type;
                    obj.LAST_UPDATED_DATE = DateTime.Now;
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

        public Tbl_Order_Details_CM ET_General_Order_Details_View_DL(int id)
        {
            try
            {
                var data = (from MO in dbcontext.Tbl_Master_Order
                            join OD in dbcontext.Tbl_Order_Details on MO.SO_ID equals OD.AGEN_TRAD_PO_ID
                            join PM in dbcontext.Tbl_Product_Master on OD.PRODUCT_ID equals PM.P_ID
                            join CG in dbcontext.Tbl_ProductGroup on OD.CATEGORY_ID equals CG.PG_ID
                            //join CG in dbcontext.Tbl_Master_Category on OD.CATEGORY_ID equals CG.CAT_CODE
                            join CY in dbcontext.Tbl_Currency_Master on OD.CURRENCY_ID equals CY.CURRENCY_ID
                            join MU in dbcontext.Tbl_Master_User on MO.SO_SalesPersonID equals MU.USER_ID
                            join CD in dbcontext.Tbl_Master_CompanyDetails on MO.SO_CutomerID equals CD.COM_ID
                            join SCD in dbcontext.Tbl_Master_CompanyDetails on MO.SO_SupplierID equals SCD.COM_ID
                            where MO.SO_ID == id
                            select new Tbl_Order_Details_CM
                            {
                                AGEN_TRAD_PO = OD.AGEN_TRAD_PO,
                                CAT_NAME = CG.PG_NAME,
                                PRODUCT_Name = PM.P_Name,
                                QUANTITY = OD.QUANTITY,
                                UOM_NAME = (dbcontext.tbl_LookUp.FirstOrDefault(m => m.LU_Type == 2 && m.LU_Code == OD.UOM).LU_Description),
                                CURRENCY_NAME = CY.CURRENCY_NAME,
                                SO_OrderDate = MO.SO_OrderDate,
                                SO_ORGID = MO.SO_ORGID,
                                SO_OrderType = MO.SO_OrderType,
                                SO_CutomerID = MO.SO_CutomerID,
                                SO_SupplierID = MO.SO_SupplierID,
                                SO_CusPONO = MO.SO_CusPONO,
                                SO_CusPODate = MO.SO_CusPODate,
                                SO_SupSCNO = MO.SO_SupSCNO,
                                SO_SupSCDate = MO.SO_SupSCDate,
                                SO_CusCurrencyname = CY.CURRENCY_NAME,
                                SO_SupCurrencyname = CY.CURRENCY_NAME,
                                SO_CusPaymentTermID = MO.SO_CusPaymentTermID,
                                SO_CusDeliveryTerms = MO.SO_CusDeliveryTerms,
                                SO_Commision = MO.SO_Commision,
                                SO_Remarks = MO.SO_Remarks,
                                PACKING = OD.PACKING,
                                SHORT_NAME = PM.P_ShortName,
                                PRICE = OD.PRICE,
                                USER_NAME = MU.USER_NAME,
                                CUSTOMER_NAME = CD.COM_NAME,
                                SUPPLIER_NAME = SCD.COM_NAME,


                            }).FirstOrDefault();

                return data;
            }
            catch (Exception exe)
            {
                throw exe;
            }
        }

        public List<Tbl_Order_Details_CM> ET_Sales_OrderDetails_Update_Childtable_DL(int id)
        {
            try
            {
                var data = (from Ol in dbcontext.Tbl_Master_Order
                            join Od in dbcontext.Tbl_Order_Details on Ol.SO_ID equals Od.AGEN_TRAD_PO_ID
                            join Pct in dbcontext.Tbl_Product_Master on Od.PRODUCT_ID equals Pct.P_ID
                            where Ol.SO_ID == id
                            select new Tbl_Order_Details_CM
                            {
                                SO_Serial = 1,
                                SO_ID = Ol.SO_ID,
                                ORDER_ID=Od.ORDER_ID,
                                UOM_NAME = (dbcontext.tbl_LookUp.FirstOrDefault(m => m.LU_Type == 2 && m.LU_Code == Od.UOM).LU_Description),
                                QUANTITY = Od.QUANTITY,
                                PRICE = Od.PRICE,
                                PRODUCT_ID = Pct.P_ID,
                                SHORT_NAME = Pct.P_ShortName,
                                SUPPLIEROFFER_ID = Od.SUPPLIEROFFER_ID,
                                OrderDescription=Od.OrderDescription,
                                CustomerDesc=Od.CustomerDes,
                                P_PackingQuantityUOM = Pct.P_PackingQuantityUOM,
                                DesignDetail = Od.DesignDetail,
                                DiscountPer = Od.DiscountPer??0

                            }).ToList();

                List< Tbl_Order_Details_CM> modelItems = (List<Tbl_Order_Details_CM>) data.Select((orderItem, index) => new Tbl_Order_Details_CM() { SO_Serial = index + 1, SO_ID = orderItem.SO_ID, ORDER_ID=orderItem.ORDER_ID, UOM_NAME=orderItem.UOM_NAME, QUANTITY=orderItem.QUANTITY, PRICE=orderItem.PRICE, PRODUCT_ID=orderItem.PRODUCT_ID, SHORT_NAME = orderItem.SHORT_NAME, SUPPLIEROFFER_ID =orderItem.SUPPLIEROFFER_ID, OrderDescription=orderItem.OrderDescription, CustomerDesc=orderItem.CustomerDesc, P_PackingQuantityUOM=orderItem.P_PackingQuantityUOM, DesignDetail=orderItem.DesignDetail, DiscountPer=orderItem.DiscountPer }).ToList();
                return modelItems;
                //return data;
            }
            catch (Exception exe)
            {
                throw exe;
            }
        }

        public List<Tbl_Offer_Details_CM> ET_General_Order_SuplierProduct_Offer_DL(int id, int pid)
        {
            try
            {      

                var data = (from Ofid in dbcontext.Tbl_Offer_Details
                            join OfM in dbcontext.Tbl_Offers_Master on Ofid.OFFER_ID equals OfM.OFFER_ID
                            join pct in dbcontext.Tbl_Product_Master on pid equals pct.P_ID
                            where OfM.COM_ID == id && Ofid.PRODUCT_ID == pid
                            select new Tbl_Offer_Details_CM
                            {
                                OFFER_ID = OfM.OFFER_ID,
                                CUST_ID = OfM.CUST_ID,
                                CUST_SUPP = OfM.CUST_SUPP,
                                OFFER_NO = OfM.OFFER_NO,
                                PRODUCT_NAME = pct.P_ShortName,
                                PRICE = Ofid.PRICE,
                                VALIDITY_DATE = OfM.VALIDITY_DATE,
                                VALIDITY_DAYS = OfM.VALIDITY_DAYS,
                                OFFER_DATE = OfM.OFFER_DATE,
                                COM_ID = OfM.COM_ID,
                                PRODUCT_ID = Ofid.PRODUCT_ID,  
                                OFFER_CODE = OfM.OFFER_CODE

                            }).ToList();

                return data;
            }
            catch (Exception exe)
            {
                throw exe;
            }
        }

        public string CheckDuplicateCode_DL(decimal id, string OrderCode)
        {
            try
            {
                if (id == 0)
                {
                    var count = dbcontext.Tbl_Master_Order.Where(m => m.SO_Code == OrderCode).Count();
                    if (count > 0)
                    {
                        return "Exist";
                    }
                }
                else
                {
                    var count = dbcontext.Tbl_Master_Order.Where(m => m.SO_ID != id && m.SO_Code == OrderCode).Count();
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
