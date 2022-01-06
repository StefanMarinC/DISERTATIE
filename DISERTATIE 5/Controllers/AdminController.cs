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
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult AdminClients()
        {
            if (Session["Sec_user_id"] == null)
            {
                return RedirectToAction("LoginPage", "Login");
            }
            string tns = TNS.tns;
            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = tns;
            conn.Open();
            string statement = "SELECT CLIENT_ID, NAME, CUI, ADDRESS, EMAIL, CONTRACT_NUMBER, CONTRACT_DATE, MAX_PA_PERIOD, BANK_ACCOUNT_ID, PA_AFTER_DAYS, ZIP_CODE, CITY, COUNTRY, PHONE, MAX_COUNT_OF_INST, MIN_COUNT_OF_INST FROM CLIENTS";
            OracleCommand sql = new OracleCommand(statement, conn);
            List<AdminClients> adminClients = new List<AdminClients>();
            OracleDataReader reader = sql.ExecuteReader();
            try
            {
                while (reader.Read())
                {
                    AdminClients cl = new AdminClients();
                    cl.CLIENT_ID = (long)reader.GetValue(0);
                    cl.NAME = (string)reader.GetValue(1);
                    cl.CUI = (string)reader.GetValue(2);
                    cl.ADDRESS = (string)reader.GetValue(3);
                    cl.EMAIL = (string)reader.GetValue(4);
                    cl.CONTRACT_NUMBER = (string)reader.GetValue(5);
                    cl.CONTRACT_DATE = (DateTime?)reader.GetValue(6);
                    cl.MAX_PA_PERIOD = (string)reader.GetValue(7);
                    cl.BANK_ACCOUNT_ID = (long?)reader.GetValue(8);
                    cl.PA_AFTER_DAYS = (decimal?)reader.GetValue(9);
                    cl.ZIP_CODE = (string)reader.GetValue(10);
                    cl.CITY = (string)reader.GetValue(11);
                    cl.COUNTRY = (string)reader.GetValue(12);
                    cl.PHONE = (string)reader.GetValue(13);
                    cl.MAX_COUNT_OF_INST = (decimal?)reader.GetValue(14);
                    cl.MIN_COUNT_OF_INST = (decimal?)reader.GetValue(15);
                    cl.BANK_ACCOUNTS = null;
                    adminClients.Add(cl);
                }
            }
            finally
            {
                reader.Close();
                conn.Close();
            }
            return View(adminClients);
        }
    }
}