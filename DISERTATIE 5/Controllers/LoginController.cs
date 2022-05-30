using DISERTATIE_5.Models;
using DISERTATIE_5.Utils;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DISERTATIE_5.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult LoginPage()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LoginPage(UserLogin user)
        {
            ViewBag.Message = "";
            string tns = TNS.tns;
            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = tns;
            string passcripted = CriptPassword.ComputeHash(user.Password);
            conn.Open();
            string statement = "SELECT * FROM SEC_USERS WHERE USERNAME = '" + user.Username + "'";
            OracleCommand sql = new OracleCommand(statement, conn);
            OracleDataReader dr = sql.ExecuteReader();
            SEC_USERS sec_user = new SEC_USERS();
            try
            {
                while (dr.Read())
                {
                    sec_user.SEC_USER_ID = (decimal)dr.GetValue(0);
                    sec_user.USERNAME = (string)dr.GetValue(1);
                    sec_user.FULL_NAME = (string)dr.GetValue(2);
                    sec_user.SEC_PASSWORD = (string)dr.GetValue(3);
                    sec_user.ACTIVE = (decimal)dr.GetValue(4);
                    sec_user.ISADMIN = (decimal)dr.GetValue(5);
                    sec_user.START_ACTIVE_DATE = (DateTime)dr.GetValue(6);
                    sec_user.END_ACTIVE_DATE = (DateTime)dr.GetValue(7);
                    sec_user.BLOCKED = (decimal)dr.GetValue(9);
                    sec_user.CREATED_BY = (string)dr.GetValue(10);
                    sec_user.CREATION_DATE = (DateTime)dr.GetValue(11);
                    sec_user.LAST_UPDATED_BY = (string)dr.GetValue(12);
                    sec_user.LAST_UPDATE_DATE = (DateTime)dr.GetValue(13);
                    sec_user.FAILED_ATTEMPTS = (decimal)dr.GetValue(14);
                }
            }
            finally
            {
                dr.Close();
            }
            statement = "SELECT NVL(MAX(SEC_USER_ID),0) FROM SEC_USERS WHERE USERNAME = '" + user.Username + "' AND SEC_PASSWORD = '" + passcripted + "'";
            sql = new OracleCommand(statement, conn);
            Decimal user_id = (decimal)sql.ExecuteScalar();
            if (sec_user.SEC_USER_ID == 0)
            {
                conn.Close();
                ViewBag.Message = "An account with this username doesn't exist";
                return View();
            }
            if (sec_user.BLOCKED == 1)
            {
                conn.Close();
                ViewBag.Message = "The account is blocked!";
                return View();
            }
            if (sec_user.ACTIVE == 0)
            {
                conn.Close();
                ViewBag.Message = "The account is no longer active!";
                return View();
            }
            if (sec_user.SEC_USER_ID > 0 && user_id == 0)
            {
                ViewBag.Message = "Password is incorrect!";
                statement = "SECURITY_PKG.UPDATE_FAILED_ATTEMPTS";
                sql = new OracleCommand(statement, conn);
                sql.BindByName = true;
                sql.CommandType = CommandType.StoredProcedure;
                sql.Parameters.Add("P_USER_ID", OracleDbType.Decimal, sec_user.SEC_USER_ID, ParameterDirection.Input);
                sql.ExecuteNonQuery();
                conn.Close();
                return View();
            }
            statement = "UPDATE SEC_USERS SET FAILED_ATTEMPTS=0 WHERE SEC_USER_ID = " + sec_user.SEC_USER_ID;
            sql = new OracleCommand(statement, conn);
            sql.ExecuteNonQuery();
            Session["Sec_user_id"] = sec_user.SEC_USER_ID;
            Session["Username"] = sec_user.USERNAME;
            conn.Close();
            return RedirectToAction("Home", "Home", new { userId = sec_user.SEC_USER_ID });
        }


        public ActionResult Logout()
        {
            Session["Sec_user_id"] = null;
            return RedirectToAction("LoginPage", "Login");
        }
    }
}