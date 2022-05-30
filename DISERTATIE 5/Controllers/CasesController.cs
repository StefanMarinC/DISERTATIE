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
                    cl.owner_id = (decimal)reader.GetValue(13);
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

        public ActionResult Case(int? case_id)
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
            CaseInfo caseInfo = new CaseInfo();
            statement = "SELECT * FROM CASE_DETAILS_V C WHERE C.CASE_ID=" + case_id;
            sql = new OracleCommand(statement, conn);
            OracleDataReader reader = sql.ExecuteReader();
            CaseDetails cl = new CaseDetails();
            try
            {
                while (reader.Read())
                {
                    cl.case_id = (long)reader.GetValue(0);
                    cl.zone = (string)reader.GetValue(1);
                    cl.client_name = (string)reader.GetValue(2);
                    cl.balance = (float)reader.GetFloat(3);
                    cl.pa_status = (string)reader.GetValue(4);
                    cl.pa_made = (decimal)reader.GetValue(5);
                    cl.pa_broken = (decimal)reader.GetValue(6);
                    cl.pa_kept = (decimal)reader.GetValue(7);
                    cl.pa_cancelled = (decimal)reader.GetValue(8);
                }
            }
            finally
            {
                reader.Close();
                conn.Close();
            }
            caseInfo.caseDetails = cl;
            conn.Open();
            statement = "SELECT * FROM SUBSCRIBER_DATA_V SD WHERE SD.CASE_ID=" + case_id + " ORDER BY SD.MAIN DESC";
            sql = new OracleCommand(statement, conn);
            List<SubscriberData> subs_list = new List<SubscriberData>();
            List<SubscriberAddress> subs_address = new List<SubscriberAddress>();
            List<SubscriberPhone> subs_phones = new List<SubscriberPhone>();
            List<SubscriberEmail> subs_emails = new List<SubscriberEmail>();
            List<SubscriberContact> subs_contacts = new List<SubscriberContact>();
            List<SubscriberEmployer> subs_employers = new List<SubscriberEmployer>();
            FinAccountDetails finAccountDetails = new FinAccountDetails();
            List<FinancialItem> financialItems = new List<FinancialItem>();
            FinancialItem interest = new FinancialItem();
            reader = sql.ExecuteReader();
            try
            {
                while (reader.Read())
                {
                    SubscriberData subs = new SubscriberData();
                    subs.case_id = (long)reader.GetValue(0);
                    subs.subscriber_id = (long)reader.GetValue(1);
                    subs.first_name = (string)reader.GetValue(2);
                    subs.last_name = (string)reader.GetValue(3);
                    subs.subscriber_type = (string)reader.GetValue(4);
                    subs.main = (decimal)reader.GetDecimal(5);
                    subs.SSN = (string)reader.GetValue(6);
                    subs_list.Add(subs);

                    string statement2 = "SELECT * FROM SUBSCRIBER_ADDRESSES_V SA WHERE SA.SUBSCRIBER_ID=" + subs.subscriber_id + " ORDER BY SA.MAIN_ADDRESS DESC, SA.CREATION_DATE DESC";
                    OracleCommand sql2 = new OracleCommand(statement2, conn);
                    OracleDataReader reader2 = sql2.ExecuteReader();
                    try
                    {
                        while (reader2.Read())
                        {
                            SubscriberAddress subs_add = new SubscriberAddress();
                            subs_add.subscriber_id = (long)reader2.GetValue(0);
                            subs_add.address_type = (string)reader2.GetValue(1);
                            subs_add.main_address = reader2.GetDecimal(2);
                            subs_add.street = (string)reader2.GetValue(3);
                            subs_add.street_number = (string)reader2.GetValue(4);
                            subs_add.building = (string)reader2.GetValue(5);
                            subs_add.stair = (string)reader2.GetValue(6);
                            subs_add.floor = (string)reader2.GetValue(7);
                            subs_add.apartment = (string)reader2.GetValue(8);
                            subs_add.city = (string)reader2.GetValue(9);
                            subs_add.distrinct = (string)reader2.GetValue(10);
                            subs_add.country = (string)reader2.GetValue(11);
                            subs_add.source_type = (string)reader2.GetValue(12);
                            subs_add.created_by = (string)reader2.GetValue(13);
                            subs_add.creation_date = (DateTime)reader2.GetValue(14);
                            subs_address.Add(subs_add);
                        }
                    }
                    finally
                    {
                        reader2.Close();
                    }
                    statement2 = "SELECT * FROM SUBSCRIBER_PHONES_V SA WHERE SA.SUBSCRIBER_ID=" + subs.subscriber_id + " ORDER BY SA.MAIN_PHONE DESC, SA.CREATION_DATE DESC";
                    sql2 = new OracleCommand(statement2, conn);
                    reader2 = sql2.ExecuteReader();
                    try
                    {
                        while (reader2.Read())
                        {
                            SubscriberPhone subs_phone = new SubscriberPhone();
                            subs_phone.subscriber_id = (long)reader2.GetValue(0);
                            subs_phone.phone_type = (string)reader2.GetValue(1);
                            subs_phone.main_phone = (short)reader2.GetValue(2);
                            subs_phone.phone_number = (string)reader2.GetValue(3);
                            subs_phone.source_type = (string)reader2.GetValue(4);
                            subs_phone.created_by = (string)reader2.GetValue(5);
                            subs_phone.creation_date = (DateTime)reader2.GetValue(6);
                            subs_phones.Add(subs_phone);
                        }
                    }
                    finally
                    {
                        reader2.Close();
                    }
                    statement2 = "SELECT * FROM SUBSCRIBER_EMAILS_V SA WHERE SA.SUBSCRIBER_ID=" + subs.subscriber_id + " ORDER BY SA.MAIN_EMAIL DESC, SA.CREATION_DATE DESC";
                    sql2 = new OracleCommand(statement2, conn);
                    reader2 = sql2.ExecuteReader();
                    try
                    {
                        while (reader2.Read())
                        {
                            SubscriberEmail subs_email = new SubscriberEmail();
                            subs_email.subscriber_id = (long)reader2.GetValue(0);
                            subs_email.email_type = (string)reader2.GetValue(1);
                            subs_email.main_email = (short)reader2.GetValue(2);
                            subs_email.email = (string)reader2.GetValue(3);
                            subs_email.source_type = (string)reader2.GetValue(4);
                            subs_email.created_by = (string)reader2.GetValue(5);
                            subs_email.creation_date = (DateTime)reader2.GetValue(6);
                            subs_emails.Add(subs_email);
                        }
                    }
                    finally
                    {
                        reader2.Close();
                    }
                    statement2 = "SELECT * FROM SUBSCRIBER_CONTACTS_V SA WHERE SA.SUBSCRIBER_ID=" + subs.subscriber_id + " ORDER BY SA.FIRST_NAME DESC";
                    sql2 = new OracleCommand(statement2, conn);
                    reader2 = sql2.ExecuteReader();
                    try
                    {
                        while (reader2.Read())
                        {
                            SubscriberContact subs_contact = new SubscriberContact();
                            subs_contact.subscriber_id = (long)reader2.GetValue(0);
                            subs_contact.contact_type = (string)reader2.GetValue(1);
                            subs_contact.first_name = (string)reader2.GetValue(2);
                            subs_contact.last_name = (string)reader2.GetValue(3);
                            subs_contact.address = (string)reader2.GetValue(4);
                            subs_contact.city = (string)reader2.GetValue(5);
                            subs_contact.postal_code = (string)reader2.GetValue(6);
                            subs_contact.phone = (string)reader2.GetValue(7);
                            subs_contact.email = (string)reader2.GetValue(8);
                            subs_contact.source_type = (string)reader2.GetValue(9);
                            subs_contacts.Add(subs_contact);
                        }
                    }
                    finally
                    {
                        reader2.Close();
                    }
                    statement2 = "SELECT * FROM SUBSCRIBER_EMPLOYERS_V SA WHERE SA.SUBSCRIBER_ID=" + subs.subscriber_id + " ORDER BY SA.MAIN_EMPLOYER DESC, EMPLOYER_NAME DESC";
                    sql2 = new OracleCommand(statement2, conn);
                    reader2 = sql2.ExecuteReader();
                    try
                    {
                        while (reader2.Read())
                        {
                            SubscriberEmployer subs_employer = new SubscriberEmployer();
                            subs_employer.subscriber_id = (long)reader2.GetValue(0);
                            subs_employer.employer_name = (string)reader2.GetValue(1);
                            subs_employer.position = (string)reader2.GetValue(2);
                            subs_employer.main_employer = (string)reader2.GetValue(3);
                            subs_employer.source_type = (string)reader2.GetValue(4);
                            subs_employers.Add(subs_employer);
                        }
                    }
                    finally
                    {
                        reader2.Close();
                    }
                }
            }
            finally
            {
                reader.Close();
                conn.Close();
            }
            conn.Open();
            statement = "SELECT * FROM FIN_ACCOUNT_DETAILS_V V WHERE V.CASE_ID=" + case_id;
            sql = new OracleCommand(statement, conn);
            reader = sql.ExecuteReader();
            try
            {
                while (reader.Read())
                {
                    finAccountDetails.case_id = reader.GetDecimal(0);
                    finAccountDetails.client_name = reader.GetString(1);
                    finAccountDetails.zone = reader.GetString(2);
                    finAccountDetails.customer_id = reader.GetString(3);
                    finAccountDetails.account_currency = reader.GetString(4);
                    finAccountDetails.account_balance_date = reader.GetDateTime(5);
                    finAccountDetails.amount_paid = reader.GetFloat(6);
                    finAccountDetails.amount_paid_currency = reader.GetString(7);
                    finAccountDetails.amount_to_pay = reader.GetFloat(8);
                    finAccountDetails.amount_to_pay_currency = reader.GetString(9);
                }
            }
            finally
            {
                reader.Close();
                conn.Close();
            }
            conn.Open();
            statement = "SELECT * FROM FIN_FINANCIAL_ITEMS_V V WHERE V.CASE_ID=" + case_id;
            sql = new OracleCommand(statement, conn);
            reader = sql.ExecuteReader();
            try
            {
                while (reader.Read())
                {
                    FinancialItem financialItem = new FinancialItem();
                    financialItem.financial_item = reader.GetDecimal(0);
                    financialItem.case_id = reader.GetDecimal(1);
                    financialItem.item_name = reader.GetString(2);
                    financialItem.item_type = reader.GetString(3);
                    financialItem.item_date = reader.GetDateTime(4).ToShortDateString();
                    financialItem.booking_date = reader.GetDateTime(5).ToShortDateString();
                    financialItem.amount = reader.GetFloat(6);
                    financialItem.amount_currency = reader.GetString(7);
                    financialItem.amount_not_booked = reader.GetFloat(8);
                    financialItem.amount_not_booked_currency = reader.GetString(9);
                    financialItem.sign = reader.GetFloat(10);
                    if (financialItem.item_type == "INTEREST")
                    {
                        if (interest.amount > 0)
                        {
                            interest.amount += financialItem.amount;
                            interest.amount_not_booked += financialItem.amount_not_booked;
                        }
                        else
                        {
                            interest = financialItem;
                        }
                    }
                    else
                    {
                        financialItems.Add(financialItem);
                    }
                }
            }
            finally
            {
                if (interest.amount > 0)
                {
                    financialItems.Add(interest);
                }
                reader.Close();
                conn.Close();

            }
            caseInfo.subscriberDatas = subs_list;
            caseInfo.subscriberAddresses = subs_address;
            caseInfo.subscriberPhones = subs_phones;
            caseInfo.subscriberEmails = subs_emails;
            caseInfo.subscriberContacts = subs_contacts;
            caseInfo.subscriberEmployers = subs_employers;
            caseInfo.FinAccountDetails = finAccountDetails;
            caseInfo.financialItems = financialItems;

            return View(caseInfo);
        }

        [HttpGet]
        public ActionResult EditPerson(int? case_id, string subscriber_id)
        {
            if (Session["Sec_user_id"] == null)
            {
                return RedirectToAction("LoginPage", "Login");
            }
            int subs_id = Int32.Parse(subscriber_id.Remove(0, 4));
            Session["case_id"] = case_id;
            Session["subscriber_id"] = subs_id;
            EditSubscriberCase editSubscriberCase = new EditSubscriberCase();
            if (case_id > 0 && subs_id > 0)
            {
                string tns = TNS.tns;
                OracleConnection conn = new OracleConnection();
                conn.ConnectionString = tns;

                conn.Open();
                string statement = "SELECT * FROM SUBSCRIBER_CASE_EDIT_V SC WHERE SC.CASE_ID=" + case_id + " AND SC.SUBSCRIBER_ID=" + subs_id;
                OracleCommand sql = new OracleCommand(statement, conn);
                OracleDataReader reader = sql.ExecuteReader();
                try {
                    while (reader.Read())
                    {
                        editSubscriberCase.main = reader.GetBoolean(0);
                        editSubscriberCase.customer_type = reader.GetString(1);
                        editSubscriberCase.ssn = reader.GetString(2);
                        editSubscriberCase.first_name = reader.GetString(3);
                        editSubscriberCase.last_name = reader.GetString(4);
                        editSubscriberCase.gender = reader.GetString(5);
                        editSubscriberCase.birth_date = reader.GetDateTime(6);
                        editSubscriberCase.birth_place = reader.GetString(7);
                        editSubscriberCase.subscriber_type = reader.GetString(8);
                    }
                }
                finally
                {
                    reader.Close();
                    conn.Close();
                }
                List<string> listPersonType = new List<string>();
                listPersonType.Add("Person");
                listPersonType.Add("Company");

                List<string> listGender = new List<string>();
                listGender.Add("M");
                listGender.Add("F");

                List<String> listSubscriberType = new List<string>();
                conn.Open();
                statement = "SELECT SUBSCRIBER_NAME FROM SUBSCRIBER_TYPES ORDER BY SUBSCRIBER_NAME";
                sql = new OracleCommand(statement, conn);
                reader = sql.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        listSubscriberType.Add(reader.GetString(0));
                    }
                }
                finally
                {
                    reader.Close();
                    conn.Close();
                }

                ViewBag.editPersonCase = case_id;
                ViewBag.PersonTypeList = listPersonType;
                ViewBag.PersonGender = listGender;
                ViewBag.SubscriberType = listSubscriberType;
                return View(editSubscriberCase);

            }
            else
            {
                return RedirectToAction("Search", "Cases");
            }
            
        }

        [HttpPost]
        public ActionResult EditPerson(EditSubscriberCase subscriber)
        {
            int case_id = (int)Session["case_id"];
            int subscriber_id = (int)Session["subscriber_id"];
            string tns = TNS.tns;
            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = tns;

            conn.Open();
            string statement = "CASES_PKG.EDIT_PERSON";
            OracleCommand sql = new OracleCommand(statement, conn);
            decimal finished_ok = 0;
            sql.BindByName = true;
            sql.CommandType = CommandType.StoredProcedure;
            int main = 0;
            if (subscriber.main)
            {
                main = 1;
            }
            sql.Parameters.Add("P_MAIN", OracleDbType.Decimal, main, ParameterDirection.Input);
            sql.Parameters.Add("P_CUSTOMER_TYPE", OracleDbType.Varchar2, subscriber.customer_type, ParameterDirection.Input);
            sql.Parameters.Add("P_SSN", OracleDbType.Varchar2, subscriber.ssn, ParameterDirection.Input);
            sql.Parameters.Add("P_FIRST_NAME", OracleDbType.Varchar2, subscriber.first_name, ParameterDirection.Input);
            sql.Parameters.Add("P_LAST_NAME", OracleDbType.Varchar2, subscriber.last_name, ParameterDirection.Input);
            sql.Parameters.Add("P_GENDER", OracleDbType.Varchar2, subscriber.gender, ParameterDirection.Input);
            sql.Parameters.Add("P_BIRTH_DATE", OracleDbType.Date, subscriber.birth_date, ParameterDirection.Input);
            sql.Parameters.Add("P_BIRTH_PLACE", OracleDbType.Varchar2, subscriber.birth_place, ParameterDirection.Input);
            sql.Parameters.Add("P_SUBSCRIBE_TYPE", OracleDbType.Varchar2, subscriber.subscriber_type, ParameterDirection.Input);
            sql.Parameters.Add("P_CASE_ID", OracleDbType.Decimal, case_id, ParameterDirection.Input);
            sql.Parameters.Add("P_SUBSCRIBER_ID", OracleDbType.Decimal, subscriber_id, ParameterDirection.Input);

            sql.Parameters.Add("P_FINISHED_OK", OracleDbType.Decimal).Direction = ParameterDirection.Output;
            sql.ExecuteNonQuery();
            finished_ok =Convert.ToDecimal(((OracleDecimal)sql.Parameters["P_FINISHED_OK"].Value).Value);
            if (finished_ok == 1)
            {
                return RedirectToAction("Case", "Cases", new { case_id = case_id });
            }
            else
            {
                TempData["ErrorEditPerson"] = "Something went wrong";
                return RedirectToAction("EditPerson", "Cases", new { case_id = case_id, subscriber_id = subscriber_id });
            }
        }

        [HttpGet]
        public ActionResult AddPerson(int? case_id)
        {
            if (Session["Sec_user_id"] == null)
            {
                return RedirectToAction("LoginPage", "Login");
            }
            Session["case_id"] = case_id;
            string tns = TNS.tns;
            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = tns;

            List<string> listPersonType = new List<string>();
            listPersonType.Add("Person");
            listPersonType.Add("Company");

            List<string> listGender = new List<string>();
            listGender.Add("M");
            listGender.Add("F");

            List<String> listSubscriberType = new List<string>();
            conn.Open();
            string statement = "SELECT SUBSCRIBER_NAME FROM SUBSCRIBER_TYPES ORDER BY SUBSCRIBER_NAME";
            OracleCommand sql = new OracleCommand(statement, conn);
            OracleDataReader reader = sql.ExecuteReader();
            try
            {
                while (reader.Read())
                {
                    listSubscriberType.Add(reader.GetString(0));
                }
            }
            finally
            {
                reader.Close();
                conn.Close();
            }
            ViewBag.addPersonCase = case_id;
            ViewBag.PersonTypeList = listPersonType;
            ViewBag.PersonGender = listGender;
            ViewBag.SubscriberType = listSubscriberType;
            return View();
        }

        [HttpPost]
        public ActionResult AddPerson(AddSubscriberCase subscriber)
        {
            int case_id = (int)Session["case_id"];
            string tns = TNS.tns;
            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = tns;

            conn.Open();
            string statement = "CASES_PKG.ADD_PERSON";
            OracleCommand sql = new OracleCommand(statement, conn);
            decimal finished_ok = 0;
            sql.BindByName = true;
            sql.CommandType = CommandType.StoredProcedure;
            int main = 0;
            if (subscriber.main)
            {
                main = 1;
            }
            sql.Parameters.Add("P_MAIN", OracleDbType.Decimal, main, ParameterDirection.Input);
            sql.Parameters.Add("P_CUSTOMER_TYPE", OracleDbType.Varchar2, subscriber.debtor_type, ParameterDirection.Input);
            sql.Parameters.Add("P_SSN", OracleDbType.Varchar2, subscriber.ssn, ParameterDirection.Input);
            sql.Parameters.Add("P_FIRST_NAME", OracleDbType.Varchar2, subscriber.first_name, ParameterDirection.Input);
            sql.Parameters.Add("P_LAST_NAME", OracleDbType.Varchar2, subscriber.last_name, ParameterDirection.Input);
            sql.Parameters.Add("P_GENDER", OracleDbType.Varchar2, subscriber.gender, ParameterDirection.Input);
            sql.Parameters.Add("P_BIRTH_DATE", OracleDbType.Date, subscriber.birth_date, ParameterDirection.Input);
            sql.Parameters.Add("P_BIRTH_PLACE", OracleDbType.Varchar2, subscriber.birth_place, ParameterDirection.Input);
            sql.Parameters.Add("P_SUBSCRIBE_TYPE", OracleDbType.Varchar2, subscriber.subscriber_type, ParameterDirection.Input);
            sql.Parameters.Add("P_CASE_ID", OracleDbType.Decimal, case_id, ParameterDirection.Input);

            sql.Parameters.Add("P_FINISHED_OK", OracleDbType.Decimal).Direction = ParameterDirection.Output;
            sql.ExecuteNonQuery();
            finished_ok = Convert.ToDecimal(((OracleDecimal)sql.Parameters["P_FINISHED_OK"].Value).Value);

            switch (finished_ok)
            {
                case 1:
                    return RedirectToAction("Case", "Cases", new { case_id = case_id });
                case 2:
                    TempData["ErrorAddPerson"] = "The person is already associated on this case!";
                    return RedirectToAction("AddPerson", "Cases", new { case_id = case_id });
                default:
                    TempData["ErrorAddPerson"] = "Something went wrong";
                    return RedirectToAction("AddPerson", "Cases", new { case_id = case_id });
            }
        }

        [HttpGet]
        public ActionResult DeletePerson(int? case_id, string subscriber_id)
        {
            int subs_id = Int32.Parse(subscriber_id.Remove(0, 4));
            Session["case_id"] = case_id;
            Session["subscriber_id"] = subs_id;
            decimal finished_ok = 0;
            if (case_id > 0 && subs_id > 0)
            {
                string tns = TNS.tns;
                OracleConnection conn = new OracleConnection();
                conn.ConnectionString = tns;

                conn.Open();
                string statement = "CASES_PKG.DELETE_PERSON";
                OracleCommand sql = new OracleCommand(statement, conn);
                sql.BindByName = true;
                sql.CommandType = CommandType.StoredProcedure;
                sql.Parameters.Add("P_CASE_ID", OracleDbType.Decimal, case_id, ParameterDirection.Input);
                sql.Parameters.Add("P_SUBSCRIBER_ID", OracleDbType.Decimal, subs_id, ParameterDirection.Input);

                sql.Parameters.Add("P_FINISHED_OK", OracleDbType.Decimal).Direction = ParameterDirection.Output;
                sql.ExecuteNonQuery();
                finished_ok = Convert.ToDecimal(((OracleDecimal)sql.Parameters["P_FINISHED_OK"].Value).Value);
                switch (finished_ok) {
                    case 1:
                        return RedirectToAction("Case", "Cases", new { case_id = case_id });
                    case 2:
                        TempData["ErrorDeletePerson"] = "You can't delete the only person of the case!";
                        return RedirectToAction("Case", "Cases", new { case_id = case_id });
                    case 3:
                        TempData["ErrorDeletePerson"] = "You can't delete the main person of the case!";
                        return RedirectToAction("Case", "Cases", new { case_id = case_id });
                    default:
                        TempData["ErrorDeletePerson"] = "Something went wrong";
                        return RedirectToAction("Case", "Cases", new { case_id = case_id });
                }
            }
            else
            {
                return RedirectToAction("Search", "Cases");
            }
        }

        [HttpGet]
        public ActionResult AddPayment(int? case_id)
        {
            if (Session["Sec_user_id"] == null)
            {
                return RedirectToAction("LoginPage", "Login");
            }
            string tns = TNS.tns;
            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = tns;
            string statement = "SELECT NAME FROM CURRENCIES";
            OracleCommand sql = new OracleCommand(statement, conn);
            conn.Open();
            OracleDataReader reader = sql.ExecuteReader();
            List<string> currencyList = new List<string>();
            try
            {
                while (reader.Read())
                {
                    currencyList.Add(reader.GetString(0));
                }
            }
            finally
            {
                reader.Close();
                conn.Close();
            }
            ViewBag.CurrencyList = currencyList;
            Session["case_id"] = case_id;
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddPayment(DateTime payment_date, DateTime booking_date, float amount, string currency)
        {
            int case_id = (int)Session["case_id"];
            return RedirectToAction("Case", "Cases", new { case_id = case_id });
        }
    }
}