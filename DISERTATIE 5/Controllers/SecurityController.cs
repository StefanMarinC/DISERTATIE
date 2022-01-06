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
    public class SecurityController : Controller
    {
        // GET: Security
        public ActionResult Users()
        {
            string tns = Utils.TNS.tns;
            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = tns;
            string statement = null;
            OracleCommand sql;
            if (Session["Sec_user_id"] == null)
            {

                return RedirectToAction("LoginPage", "Login");
            }
            if (Session["IsAdmin"] == null)
            {
                tns = Utils.TNS.tns;
                conn = new OracleConnection();
                conn.ConnectionString = tns;
                conn.Open();
                statement = "SELECT ISADMIN FROM SEC_USERS WHERE USERNAME = '" + Session["Username"].ToString() + "'";
                sql = new OracleCommand(statement, conn);
                Decimal result = (decimal)sql.ExecuteScalar();
                conn.Close();
                int isadmin = (int)result;
                if (isadmin == 0)
                {
                    Session["IsAdmin"] = 0;
                    return RedirectToAction("Home", "Home");
                }
                Session["IsAdmin"] = 1;
            }
            statement = "SELECT SEC_USER_ID, USERNAME, FULL_NAME, ISADMIN, ACTIVE, BLOCKED FROM SEC_USERS";
            sql = new OracleCommand(statement, conn);
            List<UserSecurity> usersList = new List<UserSecurity>();
            conn.Open();
            OracleDataReader reader = sql.ExecuteReader();
            try
            {
                while (reader.Read())
                {
                    UserSecurity us = new UserSecurity();
                    us.SEC_USER_ID = (decimal)reader.GetValue(0);
                    us.USERNAME = (string)reader.GetValue(1);
                    us.FULL_NAME = (string)reader.GetValue(2);
                    us.ISADMIN = (decimal)reader.GetValue(3);
                    us.ACTIVE = (decimal)reader.GetValue(4);
                    us.BLOCKED = (decimal)reader.GetValue(5);
                    usersList.Add(us);
                }
            }
            finally
            {
                reader.Close();
                conn.Close();
            }

            return View(usersList);
        }

        public ActionResult ChangeBlockedUser(int block, int sec_user_id)
        {
            UpdateBlockedUser(block, sec_user_id);
            return RedirectToAction("Users", "Security");
        }

        [NonAction]
        public void UpdateBlockedUser(int block, int sec_user_id)
        {
            string tns = Utils.TNS.tns;
            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = tns;
            string statement = "UPDATE SEC_USERS SET BLOCKED=" + block + " WHERE SEC_USER_ID=" + sec_user_id;
            OracleCommand sql = new OracleCommand(statement, conn);
            conn.Open();
            sql.ExecuteNonQuery();
            conn.Close();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Users(string name, string username)
        {
            if (username == "" && name == "")
            {
                return RedirectToAction("Users", "Security");
            }
            string tns = Utils.TNS.tns;
            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = tns;
            string statement = "SELECT SEC_USER_ID, USERNAME, FULL_NAME, ISADMIN, ACTIVE, BLOCKED FROM SEC_USERS WHERE ";
            if (name != "" && username == "")
            {
                statement += " NAME LIKE '%" + name + "%'";
            }
            else if (name == "" && username != "")
            {
                statement += " USERNAME LIKE '%" + username + "%'";
            }
            else
            {
                statement += " USERNAME LIKE '%" + username + "%' AND FULL_NAME LIKE '%" + name + "%'";
            }
            OracleCommand sql = new OracleCommand(statement, conn);
            conn.Open();
            List<UserSecurity> usersList = new List<UserSecurity>();
            OracleDataReader reader = sql.ExecuteReader();
            try
            {
                while (reader.Read())
                {
                    UserSecurity us = new UserSecurity();
                    us.SEC_USER_ID = (decimal)reader.GetValue(0);
                    us.USERNAME = (string)reader.GetValue(1);
                    us.FULL_NAME = (string)reader.GetValue(2);
                    us.ISADMIN = (decimal)reader.GetValue(3);
                    us.ACTIVE = (decimal)reader.GetValue(4);
                    us.BLOCKED = (decimal)reader.GetValue(5);
                    usersList.Add(us);
                }
            }
            finally
            {
                reader.Close();
                conn.Close();
            }

            return View(usersList);
        }

        [HttpGet]
        public ActionResult AddUser()
        {
            if (Session["Sec_user_id"] == null)
            {
                return RedirectToAction("LoginPage", "Login");
            }
            return View();
        }

        [HttpPost]
        public ActionResult AddUser(AddUser user)
        {
            Session["AddUserError"] = null;
            Session["AddUserMessage"] = null;
            if (Session["Sec_user_id"] == null)
            {
                return RedirectToAction("LoginPage", "Login");
            }
            string tns = Utils.TNS.tns;
            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = tns;
            string statement = "SELECT NVL(MAX(SEC_USER_ID),0) FROM SEC_USERS WHERE USERNAME='" + user.USERNAME + "'";
            OracleCommand sql = new OracleCommand(statement, conn);
            conn.Open();
            Decimal exists = (decimal)sql.ExecuteScalar();
            var currentDate = DateTime.Now;
            currentDate = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day);
            if (exists > 0)
            {
                conn.Close();
                Session["AddUserError"] = "An account with this username already exists";
                return RedirectToAction("AddUser", "Security");
            }
            if (user.END_ACTIVE_DATE <= user.START_ACTIVE_DATE)
            {
                conn.Close();
                Session["AddUserError"] = "End active date must be greater then start active date";
                return RedirectToAction("AddUser", "Security");
            }
            if (user.END_ACTIVE_DATE <= currentDate)
            {
                conn.Close();
                Session["AddUserError"] = "End active date must start with tomorrow";
                return RedirectToAction("AddUser", "Security");
            }
            int isadmin;
            if (user.ISADMIN == true)
            {
                isadmin = 1;
            }
            else
            {
                isadmin = 0;
            }
            statement = "SECURITY.ADD_USER";
            sql = new OracleCommand(statement, conn);
            sql.BindByName = true;
            sql.CommandType = CommandType.StoredProcedure;
            sql.Parameters.Add("P_USERNAME", OracleDbType.Varchar2, user.USERNAME, ParameterDirection.Input);
            sql.Parameters.Add("P_FULL_NAME", OracleDbType.Varchar2, user.FULL_NAME, ParameterDirection.Input);
            sql.Parameters.Add("P_PASSWORD", OracleDbType.Varchar2, CriptPassword.ComputeHash(user.SEC_PASSWORD), ParameterDirection.Input);
            sql.Parameters.Add("P_ISADMIN", OracleDbType.Decimal, isadmin, ParameterDirection.Input);
            sql.Parameters.Add("P_START_ACTIV_DATE", OracleDbType.Date, user.START_ACTIVE_DATE, ParameterDirection.Input);
            sql.Parameters.Add("P_END_ACTIV_DATE", OracleDbType.Date, user.END_ACTIVE_DATE, ParameterDirection.Input);
            sql.Parameters.Add("P_USER", OracleDbType.Varchar2, Session["Username"].ToString(), ParameterDirection.Input);
            sql.ExecuteNonQuery();
            conn.Close();
            Session["AddUserMessage"] = "User added successfully";
            return RedirectToAction("AddUser", "Security");
        }
    }
}