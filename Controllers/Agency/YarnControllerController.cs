using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Euro.Controllers.Agency
{
    public class YarnControllerController : Controller
    {
        // GET: YarnController
        public ActionResult YarnController()
        {
            return View();
        }
        public ActionResult GetData(string txt)
        {   //warp & weft with date arrangement
            DataTable dt = new DataTable();
            dt.Columns.Add("Warp", typeof(decimal));
            dt.Columns.Add("Weft", typeof(decimal));
            dt.Columns.Add("TransitWarp", typeof(decimal));
            dt.Columns.Add("TransitWeft", typeof(decimal));
            dt.Columns.Add("Date", typeof(DateTime));
            string[] txt1 = txt.Split('&');
            string[] s = txt1[0].Split('|');

            //Trasit
            DataTable dt1 = new DataTable();
            dt1.Columns.Add("TransitWarp", typeof(decimal));
            dt1.Columns.Add("TransitWeft", typeof(decimal));
            dt1.Columns.Add("Date", typeof(DateTime));
            string[] s2 = txt1[1].Split('|');
            DataRow dr1;
            for (int i = 0; i < s2.Length - 1; i++)
            {
                string[] s3 = s2[i].Split(',');
                dr1 = dt1.NewRow();
                dr1["TransitWarp"] = Convert.ToDecimal(s3[1]);
                dr1["TransitWeft"] = Convert.ToDecimal(s3[2]);
                dr1["Date"] = DateTime.ParseExact(s3[0], "dd-M-yyyy", null);
                dt1.Rows.Add(dr1);
            }



            DataRow dr;
            for (int i = 0; i < s.Length - 1; i++)
            {
                string[] s1 = s[i].Split(',');
                dr = dt.NewRow();
                dr["Warp"] = Convert.ToDecimal(s1[0]);
                dr["Weft"] = Convert.ToDecimal(s1[1]);
                dr["TransitWarp"] = 0;
                dr["TransitWeft"] = 0;
                dr["Date"] = DateTime.ParseExact(s1[2], "dd-M-yyyy", null);
                dt.Rows.Add(dr);
            }
            var result = dt.AsEnumerable().OrderBy(l => l.Field<DateTime>("Date"))
    .GroupBy(l => l.Field<DateTime>("Date"))
    .Select(cl => new
    {
        Warp = cl.Sum(c => c.Field<decimal>("Warp")),
        Weft = cl.Sum(c => c.Field<decimal>("Weft")),
        TransitWarp = cl.Sum(c => c.Field<decimal>("TransitWarp")),
        TransitWeft = cl.Sum(c => c.Field<decimal>("TransitWeft")),
        Date = cl.First().Field<DateTime>("Date"),
    }).ToList();
            dt.Rows.Clear();
            for (int i = 0; i < result.Count(); i++)
            {
                dr = dt.NewRow();
                dr["Warp"] = result[i].Warp;
                dr["Weft"] = result[i].Weft;
                if (i == 0)
                {
                    dr["TransitWarp"] = dt1.AsEnumerable().Where(c => c.Field<DateTime>("Date") <= result[i].Date).Sum(c => c.Field<decimal>("TransitWarp"));
                    dr["TransitWeft"] = dt1.AsEnumerable().Where(c => c.Field<DateTime>("Date") <= result[i].Date).Sum(c => c.Field<decimal>("TransitWeft"));

                }
                else
                {
                    dr["TransitWarp"] = dt1.AsEnumerable().Where(c => c.Field<DateTime>("Date") > result[i - 1].Date && c.Field<DateTime>("Date") <= result[i].Date).Sum(c => c.Field<decimal>("TransitWarp"));
                    dr["TransitWeft"] = dt1.AsEnumerable().Where(c => c.Field<DateTime>("Date") > result[i - 1].Date && c.Field<DateTime>("Date") <= result[i].Date).Sum(c => c.Field<decimal>("TransitWeft"));
                }
                dr["Date"] = result[i].Date;
                dt.Rows.Add(dr);
            }


            var result1 = dt.AsEnumerable().OrderBy(l => l.Field<DateTime>("Date"))
    .Select(cl => new
    {
        Warp = cl.Field<decimal>("Warp").ToString("0.000"),
        Weft = cl.Field<decimal>("Weft").ToString("0.000"),
        TransitWarp = cl.Field<decimal>("TransitWarp").ToString("0.000"),
        TransitWeft = cl.Field<decimal>("TransitWeft").ToString("0.000"),
        Date = cl.Field<DateTime>("Date").ToString("dd-M-yyyy"),
    }).ToList();
            var json = new JavaScriptSerializer().Serialize(result1);
            return Json(json, JsonRequestBehavior.AllowGet);
        }
    }
}