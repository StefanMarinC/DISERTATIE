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

            List<CasesSearchClient> clients = new List<CasesSearchClient>();
            List<CasesSearchZone> zones = new List<CasesSearchZone>();
            List<CasesSearchOwner> owners = new List<CasesSearchOwner>();
            List<CasesSearchSubsType> subs_types = new List<CasesSearchSubsType>();

            string statement = "SELECT CLIENT_ID, NAME FROM CLIENTS ORDER BY NAME";
            OracleCommand sql = new OracleCommand(statement, conn);
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

            conn.Open();
            statement = "SELECT ZONE_ID, NAME FROM ZONE_TYPES ORDER BY NAME";
            sql = new OracleCommand(statement, conn);
            reader = sql.ExecuteReader();
            try
            {
                while (reader.Read())
                {
                    CasesSearchZone zone = new CasesSearchZone();
                    zone.Zone_id = (string)reader.GetValue(0);
                    zone.Name = (string)reader.GetValue(1);
                    zones.Add(zone);
                }
            }
            finally
            {
                reader.Close();
                conn.Close();
            }

            conn.Open();
            statement = "SELECT SEC_USER_ID, FULL_NAME FROM SEC_USERS WHERE ISADMIN=0 ORDER BY FULL_NAME";
            sql = new OracleCommand(statement, conn);
            reader = sql.ExecuteReader();
            try
            {
                while (reader.Read())
                {
                    CasesSearchOwner owner = new CasesSearchOwner();
                    owner.Owner_id = (decimal)reader.GetValue(0);
                    owner.Name = (string)reader.GetValue(1);
                    owners.Add(owner);
                }
            }
            finally
            {
                reader.Close();
                conn.Close();
            }

            conn.Open();
            statement = "SELECT SUBSCRIBER_TYPE_ID, SUBSCRIBER_NAME FROM SUBSCRIBER_TYPES ORDER BY SUBSCRIBER_NAME";
            sql = new OracleCommand(statement, conn);
            reader = sql.ExecuteReader();
            try
            {
                while (reader.Read())
                {
                    CasesSearchSubsType subs_type = new CasesSearchSubsType();
                    subs_type.Subs_type_id = (long)reader.GetValue(0);
                    subs_type.Name = (string)reader.GetValue(1);
                    subs_types.Add(subs_type);
                }
            }
            finally
            {
                reader.Close();
                conn.Close();
            }

            ViewBag.CasesSearchClient = clients;
            ViewBag.CasesSearchZone = zones;
            ViewBag.CasesSearchOwner = owners;
            ViewBag.CasesSearchSubsType = subs_types;
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SearchCases(string client_name, string zone_name, string owner, string case_id, string account_id, string customer_id, string ssn, string subscriber_name, string subs_type, string contract_number)
        {
            string tns = TNS.tns;
            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = tns;
            conn.Open();

            List<CasesSearch> cases = new List<CasesSearch>();
            List<decimal> admin_ids = new List<decimal>();

            string statement = "SELECT * FROM CASES_SEARCH CS WHERE 1=1";
            if (client_name != null)
            {
                statement += " AND CLIENT_NAME='" + client_name + "'";
            }
            if (zone_name != null)
            {
                statement += " AND ZONE_NAME='" + zone_name + "'";
            }
            if (owner != null)
            {
                statement += " AND OWNER='" + owner + "'";
            }
            if (case_id != null && case_id != "")
            {
                statement += " AND CASE_ID=" + case_id;
            }
            if (account_id != null && account_id != "")
            {
                statement += " AND ACCOUNT_ID=" + account_id;
            }
            if (ssn != null && ssn != "")
            {
                statement += " AND SSN='" + ssn + "'";
            }
            if (subscriber_name != null && subscriber_name != "")
            {
                statement += " AND NAME LIKE '" + subscriber_name + "'";
            }
            if (subs_type != null)
            {
                statement += "AND SUBSCRIBER_TYPE='" + subs_type + "'";
            }
            if (contract_number != null && contract_number != "")
            {
                statement += "AND CONTRACT_NUMBER='" + contract_number + "'";
            }
            statement += " ORDER BY CASE_ID";
            OracleCommand sql = new OracleCommand(statement, conn);
            OracleDataReader reader = sql.ExecuteReader();
            int cases_nr = 0;
            try
            {
                while (reader.Read())
                {
                    CasesSearch cl = new CasesSearch();
                    cl.unique_id = (decimal)reader.GetValue(0);
                    cl.client_name = (string)reader.GetValue(1);
                    cl.zone = (string)reader.GetValue(2);
                    cl.zone_name = (string)reader.GetValue(3);
                    cl.owner = (string)reader.GetValue(4);
                    cl.case_id = (long)reader.GetValue(5);
                    cl.account_id = (long)reader.GetValue(6);
                    cl.customer_id = (string)reader.GetValue(7);
                    cl.ssn = (string)reader.GetValue(8);
                    cl.name = (string)reader.GetValue(9);
                    cl.subscriber_type = (string)reader.GetValue(10);
                    cl.contract_number = (string)reader.GetValue(11);
                    cl.subscriber_id = (long)reader.GetValue(12);
                    cl.owner_id=(decimal)reader.GetValue(13);
                    cases.Add(cl);
                    cases_nr += 1;
                }
            }
            finally
            {
                reader.Close();
                conn.Close();
            }
            conn.Open();
            statement = "SELECT SEC_USER_ID FROM SEC_USERS WHERE ISADMIN=1";
            sql = new OracleCommand(statement, conn);
            reader = sql.ExecuteReader();
            try
            {
                while (reader.Read())
                {
                    admin_ids.Add((decimal)reader.GetValue(0));
                }
            }
            finally
            {
                reader.Close();
                conn.Close();
            }
            TempData["Cases_users"] = admin_ids;
            TempData["CASES"] = cases;
            TempData["Cases_nr"] = cases_nr;
            return RedirectToAction("Search", "Cases");
        }

        public ActionResult Case(int case_id)
        {
            if (Session["Sec_user_id"] == null)
            {
                return RedirectToAction("LoginPage", "Login");
            }

            string tns = TNS.tns;
            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = tns;

            conn.Open();
            string statement = "CASES_PKG.ACCESS_CASE";
            OracleCommand sql = new OracleCommand(statement, conn);
            decimal can_access = 0;
            sql.BindByName = true;
            sql.CommandType = CommandType.StoredProcedure;
            sql.Parameters.Add("P_CASE_ID", OracleDbType.Decimal, case_id, ParameterDirection.Input);
            sql.Parameters.Add("P_USER_ID", OracleDbType.Decimal, (decimal)Session["Sec_user_id"], ParameterDirection.Input);
            sql.Parameters.Add("P_CAN_ACCESS", OracleDbType.Decimal).Direction = ParameterDirection.Output;
            sql.ExecuteNonQuery();
            can_access = Convert.ToDecimal(((OracleDecimal)sql.Parameters["P_CAN_ACCESS"].Value).Value);
            if (can_access == 0)
            {
                TempData["AccessError"] = "AccessError";
                return RedirectToAction("Search", "Cases");
            }
            statement = "SELECT * FROM CASES_SEARCH CS WHERE CS.CASE_ID=" + case_id;
            sql = new OracleCommand(statement, conn);
            OracleDataReader reader = sql.ExecuteReader();
            CasesSearch cl = new CasesSearch();
            try
            {
                while (reader.Read())
                {
                    cl.unique_id = (decimal)reader.GetValue(0);
                    cl.client_name = (string)reader.GetValue(1);
                    cl.zone = (string)reader.GetValue(2);
                    cl.zone_name = (string)reader.GetValue(3);
                    cl.owner = (string)reader.GetValue(4);
                    cl.case_id = (long)reader.GetValue(5);
                    cl.account_id = (long)reader.GetValue(6);
                    cl.customer_id = (string)reader.GetValue(7);
                    cl.ssn = (string)reader.GetValue(8);
                    cl.name = (string)reader.GetValue(9);
                    cl.subscriber_type = (string)reader.GetValue(10);
                    cl.contract_number = (string)reader.GetValue(11);
                    cl.subscriber_id = (long)reader.GetValue(12);
                }
            }
            finally
            {
                reader.Close();
                conn.Close();
            }

            return View(cl);
        }
    }
}