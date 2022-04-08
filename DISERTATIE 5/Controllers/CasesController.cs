using DISERTATIE_5.Models;
using DISERTATIE_5.Utils;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DISERTATIE_5.Controllers
{
    public class CasesController : Controller
    {
        // GET: Cases
        public ActionResult Search()
        {
            if (Session["Sec_user_id"] == null)
            {
                return RedirectToAction("LoginPage", "Login");
            }

            string tns = TNS.tns;
            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = tns;
            conn.Open();
            string statement = "SELECT CLIENT_ID, NAME FROM CLIENTS";
            OracleCommand sql = new OracleCommand(statement, conn);
            List<CasesSearchClient> clients = new List<CasesSearchClient>();
            OracleDataReader reader = sql.ExecuteReader();
            try
            {
                while (reader.Read())
                {
                    CasesSearchClient cl = new CasesSearchClient();
                    cl.Client_id = (long)reader.GetValue(0);
                    cl.Name = (string)reader.GetValue(1);
                    clients.Add(cl);
                }
            }
            finally
            {
                reader.Close();
                conn.Close();
            }
            ViewBag.CasesSearchClient = clients;
            return View();
        }
    }
}