using DISERTATIE_5.Models;
using DISERTATIE_5.Utils;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DISERTATIE_5.Controllers
{
    public class LegalController : Controller
    {
        // GET: Legal
        public ActionResult AddLegalFile(int case_id)
        {
            if (Session["Sec_user_id"] == null)
            {
                return RedirectToAction("LoginPage", "Login");
            }
            string tns = TNS.tns;
            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = tns;
            conn.Open();
            List<string> courts = new List<string>();
            List<string> bailiffs = new List<string>();
            List<string> lawyers = new List<string>();
            List<string> notaries = new List<string>();
            List<string> statuses = new List<string>();
            try
            {
                string statement = "SELECT NAME FROM LEG_COURTS ORDER BY NAME";
                OracleCommand sql = new OracleCommand(statement, conn);
                OracleDataReader reader = sql.ExecuteReader();
                try {
                    while (reader.Read())
                    {
                        courts.Add(reader.GetString(0));
                    }
                }
                finally
                {
                    reader.Close();
                }
                statement = "SELECT FIRST_NAME||' '||LAST_NAME FROM LEG_BAILIFFS ORDER BY FIRST_NAME";
                sql = new OracleCommand(statement, conn);
                reader = sql.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        bailiffs.Add(reader.GetString(0));
                    }
                }
                finally
                {
                    reader.Close();
                }
                statement = "SELECT NAME FROM LEG_LAWYERS ORDER BY NAME";
                sql = new OracleCommand(statement, conn);
                reader = sql.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        lawyers.Add(reader.GetString(0));
                    }
                }
                finally
                {
                    reader.Close();
                }
                statement = "SELECT NAME FROM LEG_NOTARIES ORDER BY NAME";
                sql = new OracleCommand(statement, conn);
                reader = sql.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        notaries.Add(reader.GetString(0));
                    }
                }
                finally
                {
                    reader.Close();
                }
                statement = "SELECT NAME FROM LEG_STATUS_TYPES ORDER BY NAME";
                sql = new OracleCommand(statement, conn);
                reader = sql.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        statuses.Add(reader.GetString(0));
                    }
                }
                finally
                {
                    reader.Close();
                }
            }
            finally
            {
                conn.Close();
            }
            ViewBag.Courts = courts;
            ViewBag.Bailiffs = bailiffs;
            ViewBag.Lawyers = lawyers;
            ViewBag.Notaries = notaries;
            ViewBag.Statuses = statuses;
            ViewBag.CaseID = case_id;
            return View();
        }

        [HttpPost]
        public ActionResult AddLegalFile(AddLegalFile legalFile)
        {
            if (Session["Sec_user_id"] == null)
            {
                return RedirectToAction("LoginPage", "Login");
            }
            string tns = TNS.tns;
            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = tns;

            conn.Open();
            string statement = "CASES_PKG.ADD_LEGAL_FILE";
            OracleCommand sql = new OracleCommand(statement, conn);
            sql.BindByName = true;
            sql.CommandType = CommandType.StoredProcedure;
            sql.Parameters.Add("P_CASE_ID", OracleDbType.Decimal, Session["case_id"], ParameterDirection.Input);
            sql.Parameters.Add("P_FILE_NUMBER", OracleDbType.Varchar2, legalFile.fileNumber, ParameterDirection.Input);
            sql.Parameters.Add("P_COURT", OracleDbType.Varchar2, legalFile.court, ParameterDirection.Input);
            sql.Parameters.Add("P_BAILIFF", OracleDbType.Varchar2, legalFile.bailiff, ParameterDirection.Input);
            sql.Parameters.Add("P_LAWYER", OracleDbType.Varchar2, legalFile.lawyer, ParameterDirection.Input);
            sql.Parameters.Add("P_NOTARY", OracleDbType.Varchar2, legalFile.notary, ParameterDirection.Input);
            sql.Parameters.Add("P_STATUS", OracleDbType.Varchar2, legalFile.status, ParameterDirection.Input);
            sql.Parameters.Add("P_START_DATE", OracleDbType.Date, legalFile.start_date, ParameterDirection.Input);
            sql.Parameters.Add("P_FINISHED_OK", OracleDbType.Decimal).Direction = ParameterDirection.Output;
            sql.ExecuteNonQuery();
            decimal finished_ok = Convert.ToDecimal(((OracleDecimal)sql.Parameters["P_FINISHED_OK"].Value).Value);
            switch (finished_ok)
            {
                case 1:
                    return RedirectToAction("Case", "Cases", new { case_id = Session["case_id"] });
                default:
                    TempData["ErrorAddLegalFile"] = "Something went wrong!";
                    return RedirectToAction("AddLegalFile", "Legal", new { case_id = Session["case_id"] });
            }
        }
    }
}