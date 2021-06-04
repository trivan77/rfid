using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace WS_rfid
{
     /// <summary>
     /// Summary description for WS_RFID
     /// </summary>
     [WebService(Namespace = "http://tempuri.org/")]
     [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
     [System.ComponentModel.ToolboxItem(false)]
     // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
     // [System.Web.Script.Services.ScriptService]

     

     public class WS_RFID : System.Web.Services.WebService
     {
          string strCon = "Data Source=.;Initial Catalog=RFID;Integrated Security=True";

          [WebMethod]
          public string HelloWorld()
          {
               return "Hello World";
          }

          [WebMethod]
          public DataTable Infor_IDCard(string IDCard)
          {
               DataTable t = new DataTable();
               SqlConnection con = new SqlConnection(strCon);
               string qry = "";
               SqlCommand cmd = new SqlCommand(qry, con);
               SqlDataAdapter a = new SqlDataAdapter(cmd);
               a.Fill(t);
               return t;
          }
     }
}
