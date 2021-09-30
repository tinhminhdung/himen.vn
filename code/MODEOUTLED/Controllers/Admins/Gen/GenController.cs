using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Configuration;
using System.Data;
using System.Data.OleDb;

namespace MODEOUTLED.Controllers.Admins.Gen
{
    public class GenController : Controller
    {
        SqlConnection dbConn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLConnectionString"].ConnectionString);
        public ActionResult Gencode()
        {
            return View();
        }

        [HttpPost]
        public string Gendb(string content)
        {
            string tb = "";
            if (content != "")
            {
                try
                {
                    string t = content;
                    t = t.Replace("\r\n\t", " ");
                    t = t.Replace("\t", " ");
                    t = t.Replace("\r", " ");
                    t = t.Replace("\n", "");
                    t = t.Replace("Go", "");
                    t = t.Replace("GO", "");
                    SqlCommand dbCmd = new SqlCommand(t.ToString(), dbConn);
                    dbCmd.CommandType = CommandType.Text;
                    dbConn.Open();
                    dbCmd.ExecuteNonQuery();
                    dbConn.Close();
                    tb  = "Thực hiện thành công !!";

                }
                catch
                {
                    tb = "Câu lệnh không đúng !!";
                    return tb;
                }
            }
            else
            {
                tb = "Câu lệnh sai !!";
                return tb;
            }
            return tb;
        }
    }
}
