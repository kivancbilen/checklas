using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;

namespace CheckLas
{
    /// <summary>
    /// Summary description for Service
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class Service : System.Web.Services.WebService
    {
        public static List<connlist> clist = new List<connlist>();

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void Login()
        {
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString =
            "Data Source=213.155.110.162;" +
            "Initial Catalog=Checklas;" +
            "User id=sa;" +
            "Password=Checklas2017;";
            //if (string.Equals(user, "manager"))
            //{
                conn.Open();
            //}
            //else
            //{

            //}
            string guid = "";
            if (conn.State == System.Data.ConnectionState.Open)
            {
                Guid g = Guid.NewGuid();
                guid = g.ToString();
                clist.Add(new connlist { _conn = conn, _guid = guid});
            }
            //return guid;
            //List<string> abc = new List<string>();
            //abc.Add(guid);
            //abc.Add(guid + "----");
            List<LoginList> ll = new List<LoginList>();
            ll.Add(new LoginList { sessionId = guid });
            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            Context.Response.ContentType = "application/json";
            Context.Response.Write(js.Serialize(ll));
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void GetItemsByBrand(string guid, string brand)
        {
            SqlConnection conn = clist.Where(z => z._guid == guid).Select(x => x._conn).First();
            string query = "select OITM.ItemCode,ItemName,OITB.ItmsGrpCod,ItmsGrpNam,isnull(Price,0),        U_kmpanya	,U_ebat	,U_lastikTipi	,U_yukEndeks	,U_hizEndeks	,U_rft	,U_isaretler	,U_oe	,U_oeMark	,U_mensei	,U_yakitPer	,U_desibel	,U_islakZemin	,U_kullanim	U_eMark	,U_yayindaMi	,U_renk	,U_konsantreOrani	,U_aModel	,U_alasim	,U_tonaj	,U_devirHizi	,U_amper	,U_xl	,U_mevsim	,U_basinc	,U_portal	,U_eTicaret	,U_desen	,U_katalog,U_marka, Price from OITM inner join OITB on OITB.ItmsGrpCod = OITM.ItmsGrpCod inner join ITM1 on ITM1.ItemCode = OITM.ItemCode and ITM1.PriceList=1 where OITM.U_marka='" + brand + "'";
            SqlCommand command = new SqlCommand(query, conn);
            SqlDataAdapter sqladapter = new SqlDataAdapter(command);
            DataTable results = new DataTable();
            sqladapter.Fill(results);
            results.Rows.OfType<DataRow>();

            List<Item> itlist = results.DataTableToList<Item>();


            //for (int i = 0; i < results.Rows.Count; i++)
            //{
            //    var a = results.Rows[i];
            //    itlist.Add(new Item { ItemCode = a[0].ToString(), ItemName = a[1].ToString(), ItmsGrpCod = a[2].ToString(), ItmGrpName = a[3].ToString(), Price = Convert.ToDouble(a[4]) });
            //}
            ItemResponse ret =  new ItemResponse { responseflag = true, itemlist = itlist };

            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            Context.Response.ContentType = "application/json";
            Context.Response.Write(js.Serialize(ret));

        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public ItemResponse GetItems(string guid, List<Request> itr)
        {
            SqlConnection conn = clist.Where(z => z._guid == guid).Select(x => x._conn).First();
            string query = "select OITM.ItemCode,ItemName,OITB.ItmsGrpCod,ItmsGrpNam,isnull(Price,0) Price,        U_kmpanya	,U_ebat	,U_lastikTipi	,U_yukEndeks	,U_hizEndeks	,U_rft	,U_isaretler	,U_oe	,U_oeMark	,U_mensei	,U_yakitPer	,U_desibel	,U_islakZemin	,U_kullanim	U_eMark	,U_yayindaMi	,U_renk	,U_konsantreOrani	,U_aModel	,U_alasim	,U_tonaj	,U_devirHizi	,U_amper	,U_xl	,U_mevsim	,U_basinc	,U_portal	,U_eTicaret	,U_desen	,U_katalog,U_marka from OITM inner join OITB on OITB.ItmsGrpCod = OITM.ItmsGrpCod inner join ITM1 on ITM1.ItemCode = OITM.ItemCode and ITM1.PriceList=1 where 1=1 and ";
            for (int i = 0; i < itr.Count; i++)
            {
                if (i + 1 == itr.Count)
                {
                    query = query + itr[i].Table + "." + itr[i].Field + " " + itr[i].condition.ToString() +"'"+ itr[i].Value + "' ";
                }
                else
                {
                    query = query + itr[i].Table + "." + itr[i].Field + " " + itr[i].condition.ToString() +"'"+ itr[i].Value + "' " + itr[i].op.ToString() + " ";
                }
            }
            query = query + " 1=1";
            SqlCommand command = new SqlCommand(query, conn);
            SqlDataAdapter sqladapter = new SqlDataAdapter(command);
            DataTable results = new DataTable();
            sqladapter.Fill(results);
            results.Rows.OfType<DataRow>();
            List<Item> itlist = results.DataTableToList<Item>();


            //for (int i = 0; i < results.Rows.Count; i++)
            //{
            //    var a = results.Rows[i];
            //    itlist.Add(new Item { ItemCode = a[0].ToString(), ItemName = a[1].ToString(), ItmsGrpCod = a[2].ToString(), ItmGrpName = a[3].ToString(), Price = Convert.ToDouble(a[4]) });
            //}
            return new ItemResponse { responseflag = true, itemlist = itlist };
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void Check(string guid)
        {
            SqlConnection conn = clist.Where(z => z._guid == guid).Select(x => x._conn).First();
            bool check;
            if (conn.State == ConnectionState.Open)
            {
                check = true;
            }
            else
            {
                check = false;
            }
            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            Context.Response.ContentType = "application/json";
            Context.Response.Write(js.Serialize(check));
        }

        public class connlist 
        {
            public SqlConnection _conn { get; set; }
            public string _guid { get; set; }
        }

        public class LoginList
        {
            public string sessionId { get; set; }
            public DateTime expires{ get; set; }
        }

        public class Item
        {
            public string ItemCode { get; set; }
            public string ItemName { get; set; }
            public string ItmsGrpCod { get; set; }
            public string ItmsGrpNam { get; set; }
            public double Price { get; set; }
            public string U_kmpanya { get; set; }
            public string U_ebat { get; set; }
            public string U_lastikTipi { get; set; }
            public string U_yukEndeks { get; set; }
            public string U_hizEndeks { get; set; }
            public string U_rft { get; set; }
            public string U_isaretler { get; set; }
            public string U_oe { get; set; }
            public string U_oeMark { get; set; }
            public string U_mensei { get; set; }
            public string U_yakitPer { get; set; }
            public string U_desibel { get; set; }
            public string U_islakZemin { get; set; }
            public string U_kullanim { get; set; }
            public string U_eMark { get; set; }
            public string U_yayindaMi { get; set; }
            public string U_renk { get; set; }
            public string U_konsantreOrani { get; set; }
            public string U_aModel { get; set; }
            public string U_alasim { get; set; }
            public string U_tonaj { get; set; }
            public string U_devirHizi { get; set; }
            public string U_amper { get; set; }
            public string U_xl { get; set; }
            public string U_mevsim { get; set; }
            public string U_basinc { get; set; }
            public string U_portal { get; set; }
            public string U_eTicaret { get; set; }
            public string U_desen { get; set; }
            public string U_katalog { get; set; }
            public string U_marka { get; set; }

        }


        public class ItemResponse
        {
            public bool responseflag { get; set; }
            public List<Item> itemlist { get; set; }
        }

        public class Request
        {
            public string Table { get; set; }
            public string Field { get; set; }
            public string condition { get; set; }
            public string Value { get; set; }
            public string op { get; set; }
        }

    }

    public static class Helper 
    {
        /// <summary>
        /// Converts a DataTable to a list with generic objects
        /// </summary>
        /// <typeparam name="T">Generic object</typeparam>
        /// <param name="table">DataTable</param>
        /// <returns>List with generic objects</returns>
        public static List<T> DataTableToList<T>(this DataTable table) where T : class, new()
        {
            try
            {
                List<T> list = new List<T>();

                foreach (var row in table.AsEnumerable())
                {
                    T obj = new T();

                    foreach (var prop in obj.GetType().GetProperties())
                    {
                        try
                        {
                            PropertyInfo propertyInfo = obj.GetType().GetProperty(prop.Name);
                            propertyInfo.SetValue(obj, Convert.ChangeType(row[prop.Name], propertyInfo.PropertyType), null);
                        }
                        catch
                        {
                            continue;
                        }
                    }

                    list.Add(obj);
                }

                return list;
            }
            catch
            {
                return null;
            }
        }
    }
}
