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
            if (Session["Sec_user_id"] == null)
            {
                return RedirectToAction("LoginPage", "Login");
            }
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
            Session["case_id"] = case_id;
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
                    cl.balance_currency = (string)reader.GetString(4);
                    cl.pa_status = (string)reader.GetValue(5);
                    cl.pa_made = (decimal)reader.GetValue(6);
                    cl.pa_broken = (decimal)reader.GetValue(7);
                    cl.pa_kept = (decimal)reader.GetValue(8);
                    cl.pa_cancelled = (decimal)reader.GetValue(9);
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
            List<string> stornoReason = new List<string>();
            reader = sql.ExecuteReader();
            try
            {
                while (reader.Read())
                {
                    SubscriberData subs = new SubscriberData();
                    subs.case_id = (long)reader.GetValue(0);
                    subs.subscriber_id = (long)reader.GetValue(1);
                    subs.first_name = (string)reader.GetValue(2);
                    if (!reader.IsDBNull(3))
                    {
                        subs.last_name = (string)reader.GetValue(3);
                    }
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
                            if (DBNull.Value != reader2.GetValue(5))
                            {
                                subs_add.building = (string)reader2.GetValue(5);
                            }
                            if (DBNull.Value != reader2.GetValue(6))
                            {
                                subs_add.stair = (string)reader2.GetValue(6);
                            }
                            if (DBNull.Value != reader2.GetValue(7))
                            {
                                subs_add.floor = (string)reader2.GetValue(7);
                            }
                            if (DBNull.Value != reader2.GetValue(8))
                            {
                                subs_add.apartment = (string)reader2.GetValue(8);
                            }
                            subs_add.city = (string)reader2.GetValue(9);
                            subs_add.distrinct = (string)reader2.GetValue(10);
                            subs_add.country = (string)reader2.GetValue(11);
                            subs_add.source_type = (string)reader2.GetValue(12);
                            subs_add.created_by = (string)reader2.GetValue(13);
                            subs_add.creation_date = (DateTime)reader2.GetValue(14);
                            subs_add.address_id = reader2.GetDecimal(15);
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
                            subs_phone.subs_phone_id = reader2.GetDecimal(7);
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
                            subs_email.subs_email_id = reader2.GetDecimal(7);
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
                            subs_contact.subs_contact_id = reader2.GetDecimal(10);
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
                    financialItem.amount_over = reader.GetFloat(11);
                    financialItem.storno_id = reader.GetDecimal(12);
                    financialItem.before = reader.GetDecimal(13);
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
            conn.Open();
            statement = "SELECT NAME  FROM FIN_STORNO_REASONS";
            sql = new OracleCommand(statement, conn);
            reader = sql.ExecuteReader();
            try
            {
                while (reader.Read())
                {
                    stornoReason.Add(reader.GetString(0));
                }
            }
            finally
            {
                reader.Close();
                conn.Close();
            }
            conn.Open();
            List<string> emailTemplates = new List<string>();
            statement = "SELECT NAME FROM EML_TEMPLATES";
            sql = new OracleCommand(statement, conn);
            reader = sql.ExecuteReader();
            try
            {
                while (reader.Read())
                {
                    emailTemplates.Add(reader.GetString(0));
                }
            }
            finally
            {
                reader.Close();
                conn.Close();
            }
            List<Emails> emails = new List<Emails>();
            conn.Open();
            statement = "SELECT TL.NAME, EMAIL_TO, SENT_DATE, CASE_ID, EML_SENT_QUEUE_ID, STATUS FROM EML_SENT_QUEUES SQ JOIN EML_TEMPLATES TL ON TL.EML_TEMPLATE_ID=SQ.EML_TEMPLATE_ID WHERE CASE_ID=" + case_id + " ORDER BY SENT_DATE DESC";
            sql = new OracleCommand(statement, conn);
            reader = sql.ExecuteReader();
            try
            {
                while (reader.Read())
                {
                    Emails email = new Emails();
                    email.subject = reader.GetString(0);
                    email.email_to = reader.GetString(1);
                    email.sent_time = reader.GetDateTime(2);
                    email.case_id = reader.GetDecimal(3);
                    email.email_id = reader.GetDecimal(4);
                    email.status = reader.GetString(5);

                    emails.Add(email);
                }
            }
            finally
            {
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
            caseInfo.stornoReasons = stornoReason;
            caseInfo.emailTemplates = emailTemplates;
            caseInfo.emails = emails;

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
                try
                {
                    while (reader.Read())
                    {
                        editSubscriberCase.main = reader.GetBoolean(0);
                        editSubscriberCase.customer_type = reader.GetString(1);
                        editSubscriberCase.ssn = reader.GetString(2);
                        editSubscriberCase.first_name = reader.GetString(3);
                        if (!reader.IsDBNull(4))
                        {
                            editSubscriberCase.last_name = reader.GetString(4);
                        }
                        if (!reader.IsDBNull(5))
                        {
                            editSubscriberCase.gender = reader.GetString(5);
                        }
                        editSubscriberCase.birth_date = reader.GetDateTime(6);
                        if (!reader.IsDBNull(7))
                        {
                            editSubscriberCase.birth_place = reader.GetString(7);
                        }

                        editSubscriberCase.subscriber_type = reader.GetString(8);
                    }
                }
                finally
                {
                    reader.Close();
                    conn.Close();
                }

                List<string> listGender = new List<string>();
                listGender.Add("M");
                listGender.Add("F");
                listGender.Add("Not the case");

                List<string> listPersonType = new List<string>();
                listPersonType.Add("Person");
                listPersonType.Add("Company");

                List<string> listSubscriberType = new List<string>();

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
            if (Session["Sec_user_id"] == null)
            {
                return RedirectToAction("LoginPage", "Login");
            }
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
            finished_ok = Convert.ToDecimal(((OracleDecimal)sql.Parameters["P_FINISHED_OK"].Value).Value);
            if (finished_ok == 1)
            {
                return RedirectToAction("Case", "Cases", new { case_id = case_id });
            }
            else
            {
                TempData["ErrorEditPerson"] = "Something went wrong!";
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

            List<string> listGender = new List<string>();
            listGender.Add("M");
            listGender.Add("F");
            listGender.Add("Not the case");

            List<string> listPersonType = new List<string>();
            listPersonType.Add("Person");
            listPersonType.Add("Company");

            List<string> listSubscriberType = new List<string>();

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

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddPerson(bool main, string debtor_type, string ssn, string first_name, string last_name, string gender, DateTime birth_date, string birth_place, string subscriber_type)
        {
            if (Session["Sec_user_id"] == null)
            {
                return RedirectToAction("LoginPage", "Login");
            }
            int case_id = (int)Session["case_id"];

            if (debtor_type == "Person" && (DateTime.Today.AddYears(-18) < birth_date))
            {
                TempData["ErrorAddPerson"] = "The person is underage!";
                return RedirectToAction("AddPerson", "Cases", new { case_id = case_id });
            }
            else
            {
                string tns = TNS.tns;
                OracleConnection conn = new OracleConnection();
                conn.ConnectionString = tns;

                conn.Open();
                string statement = "CASES_PKG.ADD_PERSON";
                OracleCommand sql = new OracleCommand(statement, conn);
                decimal finished_ok = 0;
                sql.BindByName = true;
                sql.CommandType = CommandType.StoredProcedure;
                int v_main = 0;
                if (main)
                {
                    v_main = 1;
                }
                sql.Parameters.Add("P_MAIN", OracleDbType.Decimal, v_main, ParameterDirection.Input);
                sql.Parameters.Add("P_CUSTOMER_TYPE", OracleDbType.Varchar2, debtor_type, ParameterDirection.Input);
                sql.Parameters.Add("P_SSN", OracleDbType.Varchar2, ssn, ParameterDirection.Input);
                sql.Parameters.Add("P_FIRST_NAME", OracleDbType.Varchar2, first_name, ParameterDirection.Input);
                if (last_name == "")
                {
                    sql.Parameters.Add("P_LAST_NAME", OracleDbType.Varchar2, null, ParameterDirection.Input);
                }
                else
                {
                    sql.Parameters.Add("P_LAST_NAME", OracleDbType.Varchar2, last_name, ParameterDirection.Input);
                }
                if (gender == "Not the case")
                {
                    sql.Parameters.Add("P_GENDER", OracleDbType.Varchar2, null, ParameterDirection.Input);
                }
                else
                {
                    sql.Parameters.Add("P_GENDER", OracleDbType.Varchar2, gender, ParameterDirection.Input);
                }
                sql.Parameters.Add("P_BIRTH_DATE", OracleDbType.Date, birth_date, ParameterDirection.Input);
                if (birth_place == "")
                {
                    sql.Parameters.Add("P_BIRTH_PLACE", OracleDbType.Varchar2, null, ParameterDirection.Input);
                }
                else
                {
                    sql.Parameters.Add("P_BIRTH_PLACE", OracleDbType.Varchar2, birth_place, ParameterDirection.Input);
                }
                sql.Parameters.Add("P_SUBSCRIBE_TYPE", OracleDbType.Varchar2, subscriber_type, ParameterDirection.Input);
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
                        TempData["ErrorAddPerson"] = "Something went wrong!";
                        return RedirectToAction("AddPerson", "Cases", new { case_id = case_id });
                }
            }
        }

        [HttpGet]
        public ActionResult DeletePerson(int? case_id, string subscriber_id)
        {
            if (Session["Sec_user_id"] == null)
            {
                return RedirectToAction("LoginPage", "Login");
            }
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
                switch (finished_ok)
                {
                    case 1:
                        return RedirectToAction("Case", "Cases", new { case_id = case_id });
                    case 2:
                        TempData["ErrorDeletePerson"] = "You can't delete the only person of the case!";
                        return RedirectToAction("Case", "Cases", new { case_id = case_id });
                    case 3:
                        TempData["ErrorDeletePerson"] = "You can't delete the main person of the case!";
                        return RedirectToAction("Case", "Cases", new { case_id = case_id });
                    default:
                        TempData["ErrorDeletePerson"] = "Something went wrong!";
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
            ViewBag.addPaymentCase = case_id;
            Session["case_id"] = case_id;
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddPayment(DateTime payment_date, DateTime booking_date, float amount, string currency)
        {
            if (Session["Sec_user_id"] == null)
            {
                return RedirectToAction("LoginPage", "Login");
            }
            int case_id = (int)Session["case_id"];

            string tns = TNS.tns;
            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = tns;

            conn.Open();
            string statement = "FINANCIAL_PKG.ADD_PAYMENT";
            OracleCommand sql = new OracleCommand(statement, conn);
            decimal finished_ok = 0;
            sql.BindByName = true;
            sql.CommandType = CommandType.StoredProcedure;
            sql.Parameters.Add("P_CASE_ID", OracleDbType.Decimal, case_id, ParameterDirection.Input);
            sql.Parameters.Add("P_PAYMENT_DATE", OracleDbType.Date, payment_date, ParameterDirection.Input);
            sql.Parameters.Add("P_BOOKING_DATE", OracleDbType.Date, booking_date, ParameterDirection.Input);
            sql.Parameters.Add("P_AMOUNT", OracleDbType.Double, amount, ParameterDirection.Input);
            sql.Parameters.Add("P_CURRENCY", OracleDbType.Varchar2, currency, ParameterDirection.Input);

            sql.Parameters.Add("P_FINISHED_OK", OracleDbType.Decimal).Direction = ParameterDirection.Output;
            sql.ExecuteNonQuery();
            finished_ok = Convert.ToDecimal(((OracleDecimal)sql.Parameters["P_FINISHED_OK"].Value).Value);
            if (finished_ok == 0)
            {
                TempData["ErrorAddPayment"] = "Something went wrong!";
                return RedirectToAction("AddPayment", "Cases", new { case_id = case_id });
            }
            return RedirectToAction("Case", "Cases", new { case_id = case_id });
        }

        public ActionResult AddDebt(int? case_id)
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
            ViewBag.addDebtCase = case_id;
            Session["case_id"] = case_id;
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddDebt(DateTime item_date, float amount, string currency)
        {
            if (Session["Sec_user_id"] == null)
            {
                return RedirectToAction("LoginPage", "Login");
            }
            int case_id = (int)Session["case_id"];

            string tns = TNS.tns;
            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = tns;

            conn.Open();
            string statement = "FINANCIAL_PKG.ADD_DEBT";
            OracleCommand sql = new OracleCommand(statement, conn);
            decimal finished_ok = 0;
            sql.BindByName = true;
            sql.CommandType = CommandType.StoredProcedure;
            sql.Parameters.Add("P_CASE_ID", OracleDbType.Decimal, case_id, ParameterDirection.Input);
            sql.Parameters.Add("P_ITEM_DATE", OracleDbType.Date, item_date, ParameterDirection.Input);
            sql.Parameters.Add("P_AMOUNT", OracleDbType.Double, amount, ParameterDirection.Input);
            sql.Parameters.Add("P_CURRENCY", OracleDbType.Varchar2, currency, ParameterDirection.Input);

            sql.Parameters.Add("P_FINISHED_OK", OracleDbType.Decimal).Direction = ParameterDirection.Output;
            sql.ExecuteNonQuery();
            finished_ok = Convert.ToDecimal(((OracleDecimal)sql.Parameters["P_FINISHED_OK"].Value).Value);

            switch (finished_ok)
            {
                case 1:
                    return RedirectToAction("Case", "Cases", new { case_id = case_id });
                case 2:
                    TempData["ErrorAddDebt"] = "The debt currency must be as the balance currency!";
                    return RedirectToAction("AddDiscount", "Cases", new { case_id = case_id });
                case 3:
                    TempData["ErrorAddDiscount"] = "The item date of debt can't be before the date of original debt!";
                    return RedirectToAction("AddDiscount", "Cases", new { case_id = case_id });
                default:
                    TempData["ErrorAddDebt"] = "Something went wrong!";
                    return RedirectToAction("AddDebt", "Cases", new { case_id = case_id });
            }
        }

        [HttpGet]
        public ActionResult AddDiscount(int? case_id)
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
            conn.Open();
            statement = "SELECT DESCRIPTION FROM FIN_DISCOUNT_REASONS";
            sql = new OracleCommand(statement, conn);
            reader = sql.ExecuteReader();
            List<string> discountReason = new List<string>();
            try
            {
                while (reader.Read())
                {
                    discountReason.Add(reader.GetString(0));
                }
            }
            finally
            {
                reader.Close();
                conn.Close();
            }
            ViewBag.CurrencyList = currencyList;
            ViewBag.DiscountReason = discountReason;
            ViewBag.AddDiscountCase = case_id;
            Session["case_id"] = case_id;
            return View();
        }

        [HttpPost]
        public ActionResult AddDiscount(AddDiscount addDiscount)
        {
            if (Session["Sec_user_id"] == null)
            {
                return RedirectToAction("LoginPage", "Login");
            }
            int case_id = (int)Session["case_id"];

            string tns = TNS.tns;
            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = tns;

            conn.Open();
            string statement = "FINANCIAL_PKG.ADD_DISCOUNT";
            OracleCommand sql = new OracleCommand(statement, conn);
            decimal finished_ok = 0;
            sql.BindByName = true;
            sql.CommandType = CommandType.StoredProcedure;
            sql.Parameters.Add("P_CASE_ID", OracleDbType.Decimal, case_id, ParameterDirection.Input);
            sql.Parameters.Add("P_AMOUNT", OracleDbType.Double, addDiscount.amount, ParameterDirection.Input);
            sql.Parameters.Add("P_CURRENCY", OracleDbType.Varchar2, addDiscount.currency, ParameterDirection.Input);
            sql.Parameters.Add("P_COMMENTS", OracleDbType.Varchar2, addDiscount.comments, ParameterDirection.Input);
            sql.Parameters.Add("P_REASON", OracleDbType.Varchar2, addDiscount.reason, ParameterDirection.Input);

            sql.Parameters.Add("P_FINISHED_OK", OracleDbType.Decimal).Direction = ParameterDirection.Output;
            sql.ExecuteNonQuery();
            finished_ok = Convert.ToDecimal(((OracleDecimal)sql.Parameters["P_FINISHED_OK"].Value).Value);
            switch (finished_ok)
            {
                case 1:
                    return RedirectToAction("Case", "Cases", new { case_id = case_id });
                case 2:
                    TempData["ErrorAddDiscount"] = "The discount amount need to be equal with the balance!";
                    return RedirectToAction("AddDiscount", "Cases", new { case_id = case_id });
                default:
                    TempData["ErrorAddDiscount"] = "Something went wrong!";
                    return RedirectToAction("AddDiscount", "Cases", new { case_id = case_id });
            }
        }

        [HttpGet]
        public ActionResult AddCost(int? case_id)
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
            ViewBag.AddCostCase = case_id;
            Session["case_id"] = case_id;
            return View();
        }

        [HttpPost]
        public ActionResult AddCost(AddCost addCost)
        {
            if (Session["Sec_user_id"] == null)
            {
                return RedirectToAction("LoginPage", "Login");
            }
            int case_id = (int)Session["case_id"];

            string tns = TNS.tns;
            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = tns;

            conn.Open();
            string statement = "FINANCIAL_PKG.ADD_COST";
            OracleCommand sql = new OracleCommand(statement, conn);
            decimal finished_ok = 0;
            sql.BindByName = true;
            sql.CommandType = CommandType.StoredProcedure;
            sql.Parameters.Add("P_CASE_ID", OracleDbType.Decimal, case_id, ParameterDirection.Input);
            sql.Parameters.Add("P_ITEM_DATE", OracleDbType.Date, addCost.item_date, ParameterDirection.Input);
            sql.Parameters.Add("P_AMOUNT", OracleDbType.Double, addCost.amount, ParameterDirection.Input);
            sql.Parameters.Add("P_CURRENCY", OracleDbType.Varchar2, addCost.amount, ParameterDirection.Input);
            sql.Parameters.Add("P_COMMENTS", OracleDbType.Varchar2, addCost.cost_info, ParameterDirection.Input);

            sql.Parameters.Add("P_FINISHED_OK", OracleDbType.Decimal).Direction = ParameterDirection.Output;
            sql.ExecuteNonQuery();
            finished_ok = Convert.ToDecimal(((OracleDecimal)sql.Parameters["P_FINISHED_OK"].Value).Value);
            switch (finished_ok)
            {
                case 1:
                    return RedirectToAction("Case", "Cases", new { case_id = case_id });
                case 2:
                    TempData["ErrorAddCost"] = "The cost is before an existing debt!";
                    return RedirectToAction("AddCost", "Cases", new { case_id = case_id });
                case 3:
                    TempData["ErrorAddCost"] = "A cost can be added just when a legal file is open!";
                    return RedirectToAction("AddCost", "Cases", new { case_id = case_id });
                default:
                    TempData["ErrorAddCost"] = "Something went wrong!";
                    return RedirectToAction("AddCost", "Cases", new { case_id = case_id });
            }
        }

        [HttpGet]
        public ActionResult StornoItem(int case_id, int fin_item_id, string fin_item_type, string storno_reason)
        {
            if (Session["Sec_user_id"] == null)
            {
                return RedirectToAction("LoginPage", "Login");
            }
            decimal finished_ok = 0;
            if (fin_item_type == "PAYMENT")
            {
                finished_ok = StornoPayment(fin_item_id, storno_reason);
                if (finished_ok == 0)
                {
                    TempData["ErrorStorno"] = "Something went wrong!";
                }
            }
            else
            {
                finished_ok = StornoDebt(fin_item_id, storno_reason);
                if (finished_ok == 0)
                {
                    TempData["ErrorStorno"] = "Something went wrong!";
                }
                else if (finished_ok == 2)
                {
                    TempData["ErrorStorno"] = "Can't delete the original debt!";
                }
            }

            return RedirectToAction("Case", "Cases", new { case_id = case_id });
        }

        [NonAction]
        public decimal StornoPayment(int fin_item_id, string storno_reason)
        {
            string tns = TNS.tns;
            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = tns;

            conn.Open();
            string statement = "FINANCIAL_PKG.STORNO_PAYMENT";
            OracleCommand sql = new OracleCommand(statement, conn);
            decimal finished_ok = 0;
            sql.BindByName = true;
            sql.CommandType = CommandType.StoredProcedure;
            sql.Parameters.Add("P_FIN_ITEM_ID", OracleDbType.Decimal, fin_item_id, ParameterDirection.Input);
            sql.Parameters.Add("P_STORNO_REASON", OracleDbType.Varchar2, storno_reason, ParameterDirection.Input);
            sql.Parameters.Add("P_FINISHED_OK", OracleDbType.Decimal).Direction = ParameterDirection.Output;
            sql.ExecuteNonQuery();
            finished_ok = Convert.ToDecimal(((OracleDecimal)sql.Parameters["P_FINISHED_OK"].Value).Value);
            return finished_ok;
        }

        [NonAction]
        public decimal StornoDebt(int fin_item_id, string storno_reason)
        {
            string tns = TNS.tns;
            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = tns;

            conn.Open();
            string statement = "FINANCIAL_PKG.STORNO_DEBT";
            OracleCommand sql = new OracleCommand(statement, conn);
            decimal finished_ok = 0;
            sql.BindByName = true;
            sql.CommandType = CommandType.StoredProcedure;
            sql.Parameters.Add("P_FIN_ITEM_ID", OracleDbType.Decimal, fin_item_id, ParameterDirection.Input);
            sql.Parameters.Add("P_STORNO_REASON", OracleDbType.Varchar2, storno_reason, ParameterDirection.Input);
            sql.Parameters.Add("P_FINISHED_OK", OracleDbType.Decimal).Direction = ParameterDirection.Output;
            sql.ExecuteNonQuery();
            finished_ok = Convert.ToDecimal(((OracleDecimal)sql.Parameters["P_FINISHED_OK"].Value).Value);
            return finished_ok;
        }

        [HttpGet]
        public ActionResult AllocationsInfo(int case_id, int fin_item_id)
        {
            if (Session["Sec_user_id"] == null)
            {
                return RedirectToAction("LoginPage", "Login");
            }

            string tns = TNS.tns;
            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = tns;

            conn.Open();
            string statement = "SELECT * FROM FIN_ALLOCATION_ITEM_V V WHERE V.FIN_ACCOUNT_FIN_ITEM_ID=" + fin_item_id;
            OracleCommand sql = new OracleCommand(statement, conn);
            OracleDataReader reader = sql.ExecuteReader();
            AllocationsData allocationsData = new AllocationsData();
            allocationsData.finAllocationItem = new FinAllocationItem();
            allocationsData.finAllocationsInfos = new List<FinAllocationsInfo>();
            try
            {
                while (reader.Read())
                {
                    allocationsData.finAllocationItem.amount = reader.GetFloat(1);
                    allocationsData.finAllocationItem.amount_currency = reader.GetString(2);
                    allocationsData.finAllocationItem.amount_fnc = reader.GetFloat(3);
                    allocationsData.finAllocationItem.amount_fnc_currency = reader.GetString(4);
                    allocationsData.finAllocationItem.amount_allocated = reader.GetFloat(5);
                    allocationsData.finAllocationItem.amount_allocated_currency = reader.GetString(6);
                    allocationsData.finAllocationItem.amount_allocated_fnc = reader.GetFloat(7);
                    allocationsData.finAllocationItem.amount_allocated_fnc_currency = reader.GetString(8);
                    allocationsData.finAllocationItem.amount_over = reader.GetFloat(9);
                    allocationsData.finAllocationItem.amount_over_currency = reader.GetString(10);
                    allocationsData.finAllocationItem.amount_over_fnc = reader.GetFloat(11);
                    allocationsData.finAllocationItem.amount_over_fnc_currency = reader.GetString(12);
                }
            }
            finally
            {
                reader.Close();
                conn.Close();
            }

            conn.Open();
            statement = "SELECT * FROM FIN_ALLOCATIONS_INFO_V V WHERE V.FIN_M_ACC_FINANCIAL_ITEM_ID=" + fin_item_id;
            sql = new OracleCommand(statement, conn);
            reader = sql.ExecuteReader();
            try
            {
                while (reader.Read())
                {
                    FinAllocationsInfo finAllocationsInfo = new FinAllocationsInfo();
                    finAllocationsInfo.item_type = reader.GetString(0);
                    finAllocationsInfo.amount_p = reader.GetFloat(1);
                    finAllocationsInfo.amount_currency_p = reader.GetString(2);
                    finAllocationsInfo.allocated_amount_p = reader.GetFloat(3);
                    finAllocationsInfo.allocated_amount_currency_p = reader.GetString(4);
                    finAllocationsInfo.allocated_amount_m = reader.GetFloat(5);
                    finAllocationsInfo.allocated_amount_currency_m = reader.GetString(6);
                    finAllocationsInfo.allocated_amount_fnc = reader.GetFloat(7);
                    finAllocationsInfo.allocated_amount_fnc_currency = reader.GetString(8);

                    allocationsData.finAllocationsInfos.Add(finAllocationsInfo);
                }
            }
            finally
            {
                reader.Close();
                conn.Close();
            }
            ViewBag.allocationCase = case_id;
            Session["case_id"] = case_id;
            return View(allocationsData);
        }

        [HttpGet]
        public ActionResult DeleteAddress(int address_id)
        {
            if (Session["Sec_user_id"] == null)
            {
                return RedirectToAction("LoginPage", "Login");
            }
            int case_id = (int)Session["case_id"];

            string tns = TNS.tns;
            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = tns;

            conn.Open();
            string statement = "DELETE FROM SUBSCRIBER_ADDRESSES SA WHERE SUBSCRIBER_ADDRESS_ID=" + address_id;
            OracleCommand sql = new OracleCommand(statement, conn);
            sql.ExecuteNonQuery();
            conn.Close();
            return RedirectToAction("Case", "Cases", new { case_id = case_id });
        }

        [HttpGet]
        public ActionResult AddAddress(int subs_id)
        {
            if (Session["Sec_user_id"] == null)
            {
                return RedirectToAction("LoginPage", "Login");
            }
            string tns = TNS.tns;
            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = tns;

            conn.Open();
            string statement = "SELECT NAME FROM SOURCE_TYPES";
            OracleCommand sql = new OracleCommand(statement, conn);
            OracleDataReader reader = sql.ExecuteReader();
            List<string> sourceTypes = new List<string>();
            try
            {
                while (reader.Read())
                {
                    sourceTypes.Add(reader.GetString(0));
                }
            }
            finally
            {
                reader.Close();
                conn.Close();
            }
            conn.Open();
            statement = "SELECT NAME FROM ADDRESS_TYPES";
            sql = new OracleCommand(statement, conn);
            reader = sql.ExecuteReader();
            List<string> addressTypes = new List<string>();
            try
            {
                while (reader.Read())
                {
                    addressTypes.Add(reader.GetString(0));
                }
            }
            finally
            {
                reader.Close();
                conn.Close();
            }
            ViewBag.AddressTypes = addressTypes;
            ViewBag.SourceTypes = sourceTypes;
            ViewBag.AddAddressCase = Session["case_id"];
            Session["AddAddressSubsId"] = subs_id;
            return View();
        }

        [HttpPost]
        public ActionResult AddAddress(AddAddress address)
        {
            if (Session["Sec_user_id"] == null)
            {
                return RedirectToAction("LoginPage", "Login");
            }
            int case_id = (int)Session["case_id"];

            string tns = TNS.tns;
            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = tns;

            conn.Open();
            string statement = "CONTACT_DATA.ADD_ADDRESS";
            OracleCommand sql = new OracleCommand(statement, conn);
            decimal finished_ok = 0;
            sql.BindByName = true;
            sql.CommandType = CommandType.StoredProcedure;
            int subs_id = (int)Session["AddAddressSubsId"];
            Session["AddAddressSubsId"] = null;
            int v_main = 1;
            if (!address.main)
            {
                v_main = 0;
            }

            sql.Parameters.Add("P_MAIN", OracleDbType.Decimal, v_main, ParameterDirection.Input);
            sql.Parameters.Add("P_TYPE", OracleDbType.Varchar2, address.type, ParameterDirection.Input);
            sql.Parameters.Add("P_COUNTRY", OracleDbType.Varchar2, address.country, ParameterDirection.Input);
            sql.Parameters.Add("P_CITY", OracleDbType.Varchar2, address.city, ParameterDirection.Input);
            sql.Parameters.Add("P_DISTRICT", OracleDbType.Varchar2, address.district, ParameterDirection.Input);
            sql.Parameters.Add("P_STREET", OracleDbType.Varchar2, address.street, ParameterDirection.Input);
            sql.Parameters.Add("P_STREET_NUMBER", OracleDbType.Varchar2, address.street_number, ParameterDirection.Input);
            sql.Parameters.Add("P_BUILDING", OracleDbType.Varchar2, address.building, ParameterDirection.Input);
            sql.Parameters.Add("P_STAIR", OracleDbType.Varchar2, address.stair, ParameterDirection.Input);
            sql.Parameters.Add("P_FLOOR", OracleDbType.Varchar2, address.floor, ParameterDirection.Input);
            sql.Parameters.Add("P_APARTMENT", OracleDbType.Varchar2, address.apartment, ParameterDirection.Input);
            sql.Parameters.Add("P_SOURCE", OracleDbType.Varchar2, address.source, ParameterDirection.Input);
            sql.Parameters.Add("P_SUBSCRIBER_ID", OracleDbType.Decimal, subs_id, ParameterDirection.Input);
            sql.Parameters.Add("P_FINISHED_OK", OracleDbType.Decimal).Direction = ParameterDirection.Output;
            sql.ExecuteNonQuery();
            finished_ok = Convert.ToDecimal(((OracleDecimal)sql.Parameters["P_FINISHED_OK"].Value).Value);
            switch (finished_ok)
            {
                case 1:
                    return RedirectToAction("Case", "Cases", new { case_id = case_id });
                default:
                    TempData["ErrorAddAddress"] = "Something went wrong!";
                    return RedirectToAction("AddAddress", "Cases", new { subs_id = subs_id });
            }
        }

        [HttpGet]
        public ActionResult EditAddress(int address_id)
        {
            if (Session["Sec_user_id"] == null)
            {
                return RedirectToAction("LoginPage", "Login");
            }
            string tns = TNS.tns;
            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = tns;

            conn.Open();
            string statement = "SELECT NAME FROM SOURCE_TYPES";
            OracleCommand sql = new OracleCommand(statement, conn);
            OracleDataReader reader = sql.ExecuteReader();
            List<string> sourceTypes = new List<string>();
            try
            {
                while (reader.Read())
                {
                    sourceTypes.Add(reader.GetString(0));
                }
            }
            finally
            {
                reader.Close();
                conn.Close();
            }
            conn.Open();
            statement = "SELECT NAME FROM ADDRESS_TYPES";
            sql = new OracleCommand(statement, conn);
            reader = sql.ExecuteReader();
            List<string> addressTypes = new List<string>();
            try
            {
                while (reader.Read())
                {
                    addressTypes.Add(reader.GetString(0));
                }
            }
            finally
            {
                reader.Close();
                conn.Close();
            }
            conn.Open();
            statement = "SELECT * FROM SUBSCRIBER_ADDRESSES_V V WHERE V.SUBSCRIBER_ADDRESS_ID=" + address_id;
            sql = new OracleCommand(statement, conn);
            EditAddress address = new EditAddress();
            reader = sql.ExecuteReader();
            try
            {
                while (reader.Read())
                {
                    address.type = reader.GetString(1);
                    address.main = reader.GetBoolean(2);
                    address.street = reader.GetString(3);
                    address.street_number = reader.GetString(4);
                    if (!reader.IsDBNull(5))
                    {
                        address.building = reader.GetString(5);
                    }
                    if (!reader.IsDBNull(6))
                    {
                        address.stair = reader.GetString(6);
                    }
                    if (!reader.IsDBNull(7))
                    {
                        address.floor = reader.GetString(7);
                    }
                    if (!reader.IsDBNull(8))
                    {
                        address.apartment = reader.GetString(8);
                    }
                    address.city = reader.GetString(9);
                    address.district = reader.GetString(10);
                    address.country = reader.GetString(11);
                    address.source = reader.GetString(12);
                    address.address_id = reader.GetDecimal(15);
                }
            }
            finally
            {
                reader.Close();
                conn.Close();
            }
            ViewBag.AddressTypes = addressTypes;
            ViewBag.SourceTypes = sourceTypes;
            ViewBag.EditAddressCase = Session["case_id"];
            return View(address);
        }

        [HttpPost]
        public ActionResult EditAddress(EditAddress address)
        {
            if (Session["Sec_user_id"] == null)
            {
                return RedirectToAction("LoginPage", "Login");
            }
            int case_id = (int)Session["case_id"];

            string tns = TNS.tns;
            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = tns;

            conn.Open();
            string statement = "CONTACT_DATA.EDIT_ADDRESS";
            OracleCommand sql = new OracleCommand(statement, conn);
            decimal finished_ok = 0;
            sql.BindByName = true;
            sql.CommandType = CommandType.StoredProcedure;
            int v_main = 1;
            if (!address.main)
            {
                v_main = 0;
            }

            sql.Parameters.Add("P_MAIN", OracleDbType.Decimal, v_main, ParameterDirection.Input);
            sql.Parameters.Add("P_TYPE", OracleDbType.Varchar2, address.type, ParameterDirection.Input);
            sql.Parameters.Add("P_COUNTRY", OracleDbType.Varchar2, address.country, ParameterDirection.Input);
            sql.Parameters.Add("P_CITY", OracleDbType.Varchar2, address.city, ParameterDirection.Input);
            sql.Parameters.Add("P_DISTRICT", OracleDbType.Varchar2, address.district, ParameterDirection.Input);
            sql.Parameters.Add("P_STREET", OracleDbType.Varchar2, address.street, ParameterDirection.Input);
            sql.Parameters.Add("P_STREET_NUMBER", OracleDbType.Varchar2, address.street_number, ParameterDirection.Input);
            sql.Parameters.Add("P_BUILDING", OracleDbType.Varchar2, address.building, ParameterDirection.Input);
            sql.Parameters.Add("P_STAIR", OracleDbType.Varchar2, address.stair, ParameterDirection.Input);
            sql.Parameters.Add("P_FLOOR", OracleDbType.Varchar2, address.floor, ParameterDirection.Input);
            sql.Parameters.Add("P_APARTMENT", OracleDbType.Varchar2, address.apartment, ParameterDirection.Input);
            sql.Parameters.Add("P_SOURCE", OracleDbType.Varchar2, address.source, ParameterDirection.Input);
            sql.Parameters.Add("P_ADDRESS_ID", OracleDbType.Decimal, address.address_id, ParameterDirection.Input);
            sql.Parameters.Add("P_FINISHED_OK", OracleDbType.Decimal).Direction = ParameterDirection.Output;
            sql.ExecuteNonQuery();
            finished_ok = Convert.ToDecimal(((OracleDecimal)sql.Parameters["P_FINISHED_OK"].Value).Value);
            switch (finished_ok)
            {
                case 1:
                    return RedirectToAction("Case", "Cases", new { case_id = case_id });
                default:
                    TempData["ErrorEditAddress"] = "Something went wrong!";
                    return RedirectToAction("EditAddress", "Cases", new { address_id = address.address_id });
            }
        }

        [HttpGet]
        public ActionResult DeletePhone(int phone_id)
        {
            if (Session["Sec_user_id"] == null)
            {
                return RedirectToAction("LoginPage", "Login");
            }
            int case_id = (int)Session["case_id"];

            string tns = TNS.tns;
            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = tns;

            conn.Open();
            string statement = "DELETE FROM SUBSCRIBER_PHONES SA WHERE SUBSCRIBER_PHONE_ID=" + phone_id;
            OracleCommand sql = new OracleCommand(statement, conn);
            sql.ExecuteNonQuery();
            conn.Close();
            return RedirectToAction("Case", "Cases", new { case_id = case_id });
        }

        [HttpGet]
        public ActionResult AddPhone(int subs_id)
        {
            if (Session["Sec_user_id"] == null)
            {
                return RedirectToAction("LoginPage", "Login");
            }
            string tns = TNS.tns;
            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = tns;

            conn.Open();
            string statement = "SELECT NAME FROM SOURCE_TYPES";
            OracleCommand sql = new OracleCommand(statement, conn);
            OracleDataReader reader = sql.ExecuteReader();
            List<string> sourceTypes = new List<string>();
            try
            {
                while (reader.Read())
                {
                    sourceTypes.Add(reader.GetString(0));
                }
            }
            finally
            {
                reader.Close();
                conn.Close();
            }
            conn.Open();
            statement = "SELECT NAME FROM PHONE_TYPES";
            sql = new OracleCommand(statement, conn);
            reader = sql.ExecuteReader();
            List<string> phoneTypes = new List<string>();
            try
            {
                while (reader.Read())
                {
                    phoneTypes.Add(reader.GetString(0));
                }
            }
            finally
            {
                reader.Close();
                conn.Close();
            }
            ViewBag.PhoneTypes = phoneTypes;
            ViewBag.SourceTypes = sourceTypes;
            ViewBag.AddPhoneCase = Session["case_id"];
            Session["AddPhoneSubsId"] = subs_id;
            return View();
        }

        [HttpPost]
        public ActionResult AddPhone(AddPhone phone)
        {
            if (Session["Sec_user_id"] == null)
            {
                return RedirectToAction("LoginPage", "Login");
            }
            int case_id = (int)Session["case_id"];

            string tns = TNS.tns;
            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = tns;

            conn.Open();
            string statement = "CONTACT_DATA.ADD_PHONE";
            OracleCommand sql = new OracleCommand(statement, conn);
            decimal finished_ok = 0;
            sql.BindByName = true;
            sql.CommandType = CommandType.StoredProcedure;
            int subs_id = (int)Session["AddPhoneSubsId"];
            Session["AddPhoneSubsId"] = null;
            int v_main = 1;
            if (!phone.main)
            {
                v_main = 0;
            }

            sql.Parameters.Add("P_MAIN", OracleDbType.Decimal, v_main, ParameterDirection.Input);
            sql.Parameters.Add("P_TYPE", OracleDbType.Varchar2, phone.type, ParameterDirection.Input);
            sql.Parameters.Add("P_SOURCE", OracleDbType.Varchar2, phone.source, ParameterDirection.Input);
            sql.Parameters.Add("P_PHONE_NUMBER", OracleDbType.Varchar2, phone.phone_number, ParameterDirection.Input);
            sql.Parameters.Add("P_SUBSCRIBER_ID", OracleDbType.Decimal, subs_id, ParameterDirection.Input);
            sql.Parameters.Add("P_FINISHED_OK", OracleDbType.Decimal).Direction = ParameterDirection.Output;
            sql.ExecuteNonQuery();
            finished_ok = Convert.ToDecimal(((OracleDecimal)sql.Parameters["P_FINISHED_OK"].Value).Value);
            switch (finished_ok)
            {
                case 1:
                    return RedirectToAction("Case", "Cases", new { case_id = case_id });
                default:
                    TempData["ErrorAddPhone"] = "Something went wrong!";
                    return RedirectToAction("AddPhone", "Cases", new { subs_id = subs_id });
            }
        }

        [HttpGet]
        public ActionResult EditPhone(int phone_id)
        {
            if (Session["Sec_user_id"] == null)
            {
                return RedirectToAction("LoginPage", "Login");
            }
            string tns = TNS.tns;
            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = tns;

            conn.Open();
            string statement = "SELECT NAME FROM SOURCE_TYPES";
            OracleCommand sql = new OracleCommand(statement, conn);
            OracleDataReader reader = sql.ExecuteReader();
            List<string> sourceTypes = new List<string>();
            try
            {
                while (reader.Read())
                {
                    sourceTypes.Add(reader.GetString(0));
                }
            }
            finally
            {
                reader.Close();
                conn.Close();
            }
            conn.Open();
            statement = "SELECT NAME FROM PHONE_TYPES";
            sql = new OracleCommand(statement, conn);
            reader = sql.ExecuteReader();
            List<string> phoneTypes = new List<string>();
            try
            {
                while (reader.Read())
                {
                    phoneTypes.Add(reader.GetString(0));
                }
            }
            finally
            {
                reader.Close();
                conn.Close();
            }
            conn.Open();
            statement = "SELECT * FROM SUBSCRIBER_PHONES_V V WHERE V.SUBSCRIBER_PHONE_ID=" + phone_id;
            sql = new OracleCommand(statement, conn);
            EditPhone phone = new EditPhone();
            reader = sql.ExecuteReader();
            try
            {
                while (reader.Read())
                {
                    phone.type = reader.GetString(1);
                    phone.main = reader.GetBoolean(2);
                    phone.phone_number = reader.GetString(3);
                    phone.source = reader.GetString(4);
                    phone.phone_id = reader.GetDecimal(7);
                }
            }
            finally
            {
                reader.Close();
                conn.Close();
            }
            ViewBag.PhoneTypes = phoneTypes;
            ViewBag.SourceTypes = sourceTypes;
            ViewBag.EditPhoneCase = Session["case_id"];
            return View(phone);
        }

        [HttpPost]
        public ActionResult EditPhone(EditPhone phone)
        {
            if (Session["Sec_user_id"] == null)
            {
                return RedirectToAction("LoginPage", "Login");
            }
            int case_id = (int)Session["case_id"];

            string tns = TNS.tns;
            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = tns;

            conn.Open();
            string statement = "CONTACT_DATA.EDIT_PHONE";
            OracleCommand sql = new OracleCommand(statement, conn);
            decimal finished_ok = 0;
            sql.BindByName = true;
            sql.CommandType = CommandType.StoredProcedure;
            int v_main = 1;
            if (!phone.main)
            {
                v_main = 0;
            }

            sql.Parameters.Add("P_MAIN", OracleDbType.Decimal, v_main, ParameterDirection.Input);
            sql.Parameters.Add("P_TYPE", OracleDbType.Varchar2, phone.type, ParameterDirection.Input);
            sql.Parameters.Add("P_PHONE_NUMBER", OracleDbType.Varchar2, phone.phone_number, ParameterDirection.Input);
            sql.Parameters.Add("P_SOURCE", OracleDbType.Varchar2, phone.source, ParameterDirection.Input);
            sql.Parameters.Add("P_PHONE_ID", OracleDbType.Decimal, phone.phone_id, ParameterDirection.Input);
            sql.Parameters.Add("P_FINISHED_OK", OracleDbType.Decimal).Direction = ParameterDirection.Output;
            sql.ExecuteNonQuery();
            finished_ok = Convert.ToDecimal(((OracleDecimal)sql.Parameters["P_FINISHED_OK"].Value).Value);
            switch (finished_ok)
            {
                case 1:
                    return RedirectToAction("Case", "Cases", new { case_id = case_id });
                default:
                    TempData["ErrorEditPhone"] = "Something went wrong!";
                    return RedirectToAction("EditPhone", "Cases", new { phone_id = phone.phone_id });
            }
        }

        [HttpGet]
        public ActionResult DeleteEmail(int email_id)
        {
            if (Session["Sec_user_id"] == null)
            {
                return RedirectToAction("LoginPage", "Login");
            }
            int case_id = (int)Session["case_id"];

            string tns = TNS.tns;
            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = tns;

            conn.Open();
            string statement = "DELETE FROM SUBSCRIBER_EMAILS SA WHERE SUBSCRIBER_EMAIL_ID=" + email_id;
            OracleCommand sql = new OracleCommand(statement, conn);
            sql.ExecuteNonQuery();
            conn.Close();
            return RedirectToAction("Case", "Cases", new { case_id = case_id });
        }

        [HttpGet]
        public ActionResult AddEmail(int subs_id)
        {
            if (Session["Sec_user_id"] == null)
            {
                return RedirectToAction("LoginPage", "Login");
            }
            string tns = TNS.tns;
            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = tns;

            conn.Open();
            string statement = "SELECT NAME FROM SOURCE_TYPES";
            OracleCommand sql = new OracleCommand(statement, conn);
            OracleDataReader reader = sql.ExecuteReader();
            List<string> sourceTypes = new List<string>();
            try
            {
                while (reader.Read())
                {
                    sourceTypes.Add(reader.GetString(0));
                }
            }
            finally
            {
                reader.Close();
                conn.Close();
            }
            conn.Open();
            statement = "SELECT NAME FROM EMAIL_TYPES";
            sql = new OracleCommand(statement, conn);
            reader = sql.ExecuteReader();
            List<string> emailTypes = new List<string>();
            try
            {
                while (reader.Read())
                {
                    emailTypes.Add(reader.GetString(0));
                }
            }
            finally
            {
                reader.Close();
                conn.Close();
            }
            ViewBag.EmailTypes = emailTypes;
            ViewBag.SourceTypes = sourceTypes;
            ViewBag.AddEmailCase = Session["case_id"];
            Session["AddEmailSubsId"] = subs_id;
            return View();
        }

        [HttpPost]
        public ActionResult AddEmail(AddEmail email)
        {
            if (Session["Sec_user_id"] == null)
            {
                return RedirectToAction("LoginPage", "Login");
            }
            int case_id = (int)Session["case_id"];

            string tns = TNS.tns;
            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = tns;

            conn.Open();
            string statement = "CONTACT_DATA.ADD_EMAIL";
            OracleCommand sql = new OracleCommand(statement, conn);
            decimal finished_ok = 0;
            sql.BindByName = true;
            sql.CommandType = CommandType.StoredProcedure;
            int subs_id = (int)Session["AddEmailSubsId"];
            Session["AddEmailSubsId"] = null;
            int v_main = 1;
            if (!email.main)
            {
                v_main = 0;
            }

            sql.Parameters.Add("P_MAIN", OracleDbType.Decimal, v_main, ParameterDirection.Input);
            sql.Parameters.Add("P_TYPE", OracleDbType.Varchar2, email.type, ParameterDirection.Input);
            sql.Parameters.Add("P_SOURCE", OracleDbType.Varchar2, email.source, ParameterDirection.Input);
            sql.Parameters.Add("P_EMAIL", OracleDbType.Varchar2, email.email_address, ParameterDirection.Input);
            sql.Parameters.Add("P_SUBSCRIBER_ID", OracleDbType.Decimal, subs_id, ParameterDirection.Input);
            sql.Parameters.Add("P_FINISHED_OK", OracleDbType.Decimal).Direction = ParameterDirection.Output;
            sql.ExecuteNonQuery();
            finished_ok = Convert.ToDecimal(((OracleDecimal)sql.Parameters["P_FINISHED_OK"].Value).Value);
            switch (finished_ok)
            {
                case 1:
                    return RedirectToAction("Case", "Cases", new { case_id = case_id });
                default:
                    TempData["ErrorAddEmail"] = "Something went wrong!";
                    return RedirectToAction("AddEmail", "Cases", new { subs_id = subs_id });
            }
        }

        [HttpGet]
        public ActionResult EditEmail(int email_id)
        {
            if (Session["Sec_user_id"] == null)
            {
                return RedirectToAction("LoginPage", "Login");
            }
            string tns = TNS.tns;
            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = tns;

            conn.Open();
            string statement = "SELECT NAME FROM SOURCE_TYPES";
            OracleCommand sql = new OracleCommand(statement, conn);
            OracleDataReader reader = sql.ExecuteReader();
            List<string> sourceTypes = new List<string>();
            try
            {
                while (reader.Read())
                {
                    sourceTypes.Add(reader.GetString(0));
                }
            }
            finally
            {
                reader.Close();
                conn.Close();
            }
            conn.Open();
            statement = "SELECT NAME FROM EMAIL_TYPES";
            sql = new OracleCommand(statement, conn);
            reader = sql.ExecuteReader();
            List<string> emailTypes = new List<string>();
            try
            {
                while (reader.Read())
                {
                    emailTypes.Add(reader.GetString(0));
                }
            }
            finally
            {
                reader.Close();
                conn.Close();
            }
            conn.Open();
            statement = "SELECT * FROM SUBSCRIBER_EMAILS_V V WHERE V.SUBSCRIBER_EMAIL_ID=" + email_id;
            sql = new OracleCommand(statement, conn);
            EditEmail email = new EditEmail();
            reader = sql.ExecuteReader();
            try
            {
                while (reader.Read())
                {
                    email.type = reader.GetString(1);
                    email.main = reader.GetBoolean(2);
                    email.email_address = reader.GetString(3);
                    email.source = reader.GetString(4);
                    email.email_id = reader.GetDecimal(7);
                }
            }
            finally
            {
                reader.Close();
                conn.Close();
            }
            ViewBag.EmailTypes = emailTypes;
            ViewBag.SourceTypes = sourceTypes;
            ViewBag.EditEmailCase = Session["case_id"];
            return View(email);
        }

        [HttpPost]
        public ActionResult EditEmail(EditEmail email)
        {
            if (Session["Sec_user_id"] == null)
            {
                return RedirectToAction("LoginPage", "Login");
            }
            int case_id = (int)Session["case_id"];

            string tns = TNS.tns;
            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = tns;

            conn.Open();
            string statement = "CONTACT_DATA.EDIT_EMAIL";
            OracleCommand sql = new OracleCommand(statement, conn);
            decimal finished_ok = 0;
            sql.BindByName = true;
            sql.CommandType = CommandType.StoredProcedure;
            int v_main = 1;
            if (!email.main)
            {
                v_main = 0;
            }

            sql.Parameters.Add("P_MAIN", OracleDbType.Decimal, v_main, ParameterDirection.Input);
            sql.Parameters.Add("P_TYPE", OracleDbType.Varchar2, email.type, ParameterDirection.Input);
            sql.Parameters.Add("P_EMAIL", OracleDbType.Varchar2, email.email_address, ParameterDirection.Input);
            sql.Parameters.Add("P_SOURCE", OracleDbType.Varchar2, email.source, ParameterDirection.Input);
            sql.Parameters.Add("P_EMAIL_ID", OracleDbType.Decimal, email.email_id, ParameterDirection.Input);
            sql.Parameters.Add("P_FINISHED_OK", OracleDbType.Decimal).Direction = ParameterDirection.Output;
            sql.ExecuteNonQuery();
            finished_ok = Convert.ToDecimal(((OracleDecimal)sql.Parameters["P_FINISHED_OK"].Value).Value);
            switch (finished_ok)
            {
                case 1:
                    return RedirectToAction("Case", "Cases", new { case_id = case_id });
                default:
                    TempData["ErrorEditEmail"] = "Something went wrong!";
                    return RedirectToAction("EditEmail", "Cases", new { email_id = email.email_id });
            }
        }

        [HttpGet]
        public ActionResult DeleteContact(int contact_id)
        {
            if (Session["Sec_user_id"] == null)
            {
                return RedirectToAction("LoginPage", "Login");
            }
            int case_id = (int)Session["case_id"];

            string tns = TNS.tns;
            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = tns;

            conn.Open();
            string statement = "DELETE FROM SUBSCRIBER_CONTACTS SA WHERE SUBSCRIBER_CONTACT_ID=" + contact_id;
            OracleCommand sql = new OracleCommand(statement, conn);
            sql.ExecuteNonQuery();
            conn.Close();
            return RedirectToAction("Case", "Cases", new { case_id = case_id });
        }

        [HttpGet]
        public ActionResult AddContact(int subs_id)
        {
            if (Session["Sec_user_id"] == null)
            {
                return RedirectToAction("LoginPage", "Login");
            }
            string tns = TNS.tns;
            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = tns;

            conn.Open();
            string statement = "SELECT NAME FROM SOURCE_TYPES";
            OracleCommand sql = new OracleCommand(statement, conn);
            OracleDataReader reader = sql.ExecuteReader();
            List<string> sourceTypes = new List<string>();
            try
            {
                while (reader.Read())
                {
                    sourceTypes.Add(reader.GetString(0));
                }
            }
            finally
            {
                reader.Close();
                conn.Close();
            }
            conn.Open();
            statement = "SELECT NAME FROM CONTACT_TYPES";
            sql = new OracleCommand(statement, conn);
            reader = sql.ExecuteReader();
            List<string> contactTypes = new List<string>();
            try
            {
                while (reader.Read())
                {
                    contactTypes.Add(reader.GetString(0));
                }
            }
            finally
            {
                reader.Close();
                conn.Close();
            }
            ViewBag.ContactTypes = contactTypes;
            ViewBag.SourceTypes = sourceTypes;
            ViewBag.AddContactCase = Session["case_id"];
            Session["AddContactSubsId"] = subs_id;
            return View();
        }

        [HttpPost]
        public ActionResult AddContact(AddContact contact)
        {
            if (Session["Sec_user_id"] == null)
            {
                return RedirectToAction("LoginPage", "Login");
            }
            int case_id = (int)Session["case_id"];

            string tns = TNS.tns;
            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = tns;

            conn.Open();
            string statement = "CONTACT_DATA.ADD_CONTACT";
            OracleCommand sql = new OracleCommand(statement, conn);
            decimal finished_ok = 0;
            sql.BindByName = true;
            sql.CommandType = CommandType.StoredProcedure;
            int subs_id = (int)Session["AddContactSubsId"];
            Session["AddContactSubsId"] = null;

            sql.Parameters.Add("P_TYPE", OracleDbType.Varchar2, contact.type, ParameterDirection.Input);
            sql.Parameters.Add("P_FIRST_NAME", OracleDbType.Varchar2, contact.first_name, ParameterDirection.Input);
            sql.Parameters.Add("P_LAST_NAME", OracleDbType.Varchar2, contact.last_name, ParameterDirection.Input);
            sql.Parameters.Add("P_CITY", OracleDbType.Varchar2, contact.city, ParameterDirection.Input);
            sql.Parameters.Add("P_ADDRESS", OracleDbType.Varchar2, contact.address, ParameterDirection.Input);
            sql.Parameters.Add("P_POSTAL_CODE", OracleDbType.Varchar2, contact.postal_code, ParameterDirection.Input);
            sql.Parameters.Add("P_PHONE", OracleDbType.Varchar2, contact.phone, ParameterDirection.Input);
            sql.Parameters.Add("P_EMAIL", OracleDbType.Varchar2, contact.email, ParameterDirection.Input);
            sql.Parameters.Add("P_SOURCE", OracleDbType.Varchar2, contact.source, ParameterDirection.Input);
            sql.Parameters.Add("P_SUBSCRIBER_ID", OracleDbType.Decimal, subs_id, ParameterDirection.Input);
            sql.Parameters.Add("P_FINISHED_OK", OracleDbType.Decimal).Direction = ParameterDirection.Output;
            sql.ExecuteNonQuery();
            finished_ok = Convert.ToDecimal(((OracleDecimal)sql.Parameters["P_FINISHED_OK"].Value).Value);
            switch (finished_ok)
            {
                case 1:
                    return RedirectToAction("Case", "Cases", new { case_id = case_id });
                default:
                    TempData["ErrorAddContact"] = "Something went wrong!";
                    return RedirectToAction("AddContact", "Cases", new { subs_id = subs_id });
            }
        }

        [HttpGet]
        public ActionResult EditContact(int contact_id)
        {
            if (Session["Sec_user_id"] == null)
            {
                return RedirectToAction("LoginPage", "Login");
            }
            string tns = TNS.tns;
            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = tns;

            conn.Open();
            string statement = "SELECT NAME FROM SOURCE_TYPES";
            OracleCommand sql = new OracleCommand(statement, conn);
            OracleDataReader reader = sql.ExecuteReader();
            List<string> sourceTypes = new List<string>();
            try
            {
                while (reader.Read())
                {
                    sourceTypes.Add(reader.GetString(0));
                }
            }
            finally
            {
                reader.Close();
                conn.Close();
            }
            conn.Open();
            statement = "SELECT NAME FROM CONTACT_TYPES";
            sql = new OracleCommand(statement, conn);
            reader = sql.ExecuteReader();
            List<string> contactTypes = new List<string>();
            try
            {
                while (reader.Read())
                {
                    contactTypes.Add(reader.GetString(0));
                }
            }
            finally
            {
                reader.Close();
                conn.Close();
            }
            conn.Open();
            statement = "SELECT * FROM SUBSCRIBER_CONTACTS_V V WHERE V.SUBSCRIBER_CONTACT_ID=" + contact_id;
            sql = new OracleCommand(statement, conn);
            EditContact contact = new EditContact();
            reader = sql.ExecuteReader();
            try
            {
                while (reader.Read())
                {
                    contact.type = reader.GetString(1);
                    contact.first_name = reader.GetString(2);
                    contact.last_name = reader.GetString(3);
                    contact.address = reader.GetString(4);
                    contact.city = reader.GetString(5);
                    contact.postal_code = reader.GetString(6);
                    contact.phone = reader.GetString(7);
                    contact.email = reader.GetString(8);
                    contact.source = reader.GetString(9);
                    contact.subs_contact_id = reader.GetDecimal(10);
                }
            }
            finally
            {
                reader.Close();
                conn.Close();
            }
            ViewBag.ContactTypes = contactTypes;
            ViewBag.SourceTypes = sourceTypes;
            ViewBag.EditContactCase = Session["case_id"];
            return View(contact);
        }

        [HttpPost]
        public ActionResult EditContact(EditContact contact)
        {
            if (Session["Sec_user_id"] == null)
            {
                return RedirectToAction("LoginPage", "Login");
            }
            int case_id = (int)Session["case_id"];

            string tns = TNS.tns;
            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = tns;

            conn.Open();
            string statement = "CONTACT_DATA.EDIT_CONTACT";
            OracleCommand sql = new OracleCommand(statement, conn);
            decimal finished_ok = 0;
            sql.BindByName = true;
            sql.CommandType = CommandType.StoredProcedure;

            sql.Parameters.Add("P_TYPE", OracleDbType.Varchar2, contact.type, ParameterDirection.Input);
            sql.Parameters.Add("P_FIRST_NAME", OracleDbType.Varchar2, contact.first_name, ParameterDirection.Input);
            sql.Parameters.Add("P_LAST_NAME", OracleDbType.Varchar2, contact.last_name, ParameterDirection.Input);
            sql.Parameters.Add("P_CITY", OracleDbType.Varchar2, contact.city, ParameterDirection.Input);
            sql.Parameters.Add("P_ADDRESS", OracleDbType.Varchar2, contact.address, ParameterDirection.Input);
            sql.Parameters.Add("P_POSTAL_CODE", OracleDbType.Varchar2, contact.postal_code, ParameterDirection.Input);
            sql.Parameters.Add("P_PHONE", OracleDbType.Varchar2, contact.phone, ParameterDirection.Input);
            sql.Parameters.Add("P_EMAIL", OracleDbType.Varchar2, contact.email, ParameterDirection.Input);
            sql.Parameters.Add("P_SOURCE", OracleDbType.Varchar2, contact.source, ParameterDirection.Input);
            sql.Parameters.Add("P_CONTACT_ID", OracleDbType.Decimal, contact.subs_contact_id, ParameterDirection.Input);
            sql.Parameters.Add("P_FINISHED_OK", OracleDbType.Decimal).Direction = ParameterDirection.Output;
            sql.ExecuteNonQuery();
            finished_ok = Convert.ToDecimal(((OracleDecimal)sql.Parameters["P_FINISHED_OK"].Value).Value);
            switch (finished_ok)
            {
                case 1:
                    return RedirectToAction("Case", "Cases", new { case_id = case_id });
                default:
                    TempData["ErrorEditContact"] = "Something went wrong!";
                    return RedirectToAction("EditContact", "Cases", new { contact_id = contact.subs_contact_id });
            }
        }
        [HttpGet]
        public ActionResult SendEmail(int case_id, string email_template)
        {
            if (Session["Sec_user_id"] == null)
            {
                return RedirectToAction("LoginPage", "Login");
            }
            string tns = TNS.tns;
            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = tns;

            conn.Open();
            string statement = "ACTIONS.SEND_EMAIL";
            OracleCommand sql = new OracleCommand(statement, conn);
            decimal finished_ok = 0;
            sql.BindByName = true;
            sql.CommandType = CommandType.StoredProcedure;
            sql.Parameters.Add("P_CASE_ID", OracleDbType.Decimal, case_id, ParameterDirection.Input);
            sql.Parameters.Add("P_EML_TEMPLATE", OracleDbType.Varchar2, email_template, ParameterDirection.Input);
            sql.Parameters.Add("P_FINISHED_OK", OracleDbType.Decimal).Direction = ParameterDirection.Output;
            sql.ExecuteNonQuery();
            finished_ok = Convert.ToDecimal(((OracleDecimal)sql.Parameters["P_FINISHED_OK"].Value).Value);
            switch (finished_ok)
            {
                case 2:
                    TempData["ErrorSendEmail"] = "The main person must have a main email address!";
                    break;
                default:
                    TempData["ErrorSendEmail"] = "Something went wrong!";
                    break;
            }
            return RedirectToAction("Case", "Cases", new { case_id = case_id });
        }

        [HttpGet]
        public ActionResult PreviewEmail(int email_id)
        {
            if (Session["Sec_user_id"] == null)
            {
                return RedirectToAction("LoginPage", "Login");
            }

            string tns = TNS.tns;
            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = tns;
            conn.Open();
            string statement = "SELECT CONTENT FROM EML_SENT_QUEUES WHERE EML_SENT_QUEUE_ID=" + email_id;
            OracleCommand sql = new OracleCommand(statement, conn);
            OracleDataReader reader = sql.ExecuteReader();
            HtmlString html = new HtmlString("");
            try
            {
                while (reader.Read())
                {
                    html = new HtmlString((string)reader.GetOracleClob(0).Value);
                }
            }
            finally
            {
                reader.Close();
                conn.Close();
            }
            ViewBag.EmailContent = html;
            return View();
        }

        [HttpGet]
        public ActionResult AddPA(int case_id)
        {
            if (Session["Sec_user_id"] == null)
            {
                return RedirectToAction("LoginPage", "Login");
            }
            string tns = TNS.tns;
            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = tns;

            conn.Open();
            string statement = "SELECT NAME FROM INSTALLMENT_TYPES";
            OracleCommand sql = new OracleCommand(statement, conn);
            OracleDataReader reader = sql.ExecuteReader();
            List<string> installmentTypes = new List<string>();
            try
            {
                while (reader.Read())
                {
                    installmentTypes.Add(reader.GetString(0));
                }
            }
            finally
            {
                reader.Close();
                conn.Close();
            }
            conn.Open();
            statement = "SELECT FIRST_NAME||' '||LAST_NAME FROM ACCOUNTS A JOIN ACC_SUBS_INT ASI ON ASI.ACCOUNT_ID=A.ACCOUNT_ID JOIN SUBSCRIBERS S ON S.SUBSCRIBER_ID=ASI.SUBSCRIBER_ID WHERE A.CASE_ID=" + case_id + " AND ASI.MAIN=1";
            sql = new OracleCommand(statement, conn);
            reader = sql.ExecuteReader();
            string person = null;
            try
            {
                while (reader.Read())
                {
                    person = reader.GetString(0);
                }
            }
            finally
            {
                reader.Close();
                conn.Close();
            }
            conn.Open();
            statement = "SELECT AMOUNT_TO_PAY FROM FIN_ACCOUNT_DETAILS_V WHERE CASE_ID=" + case_id;
            sql = new OracleCommand(statement, conn);
            reader = sql.ExecuteReader();
            float balance = 0;
            try
            {
                while (reader.Read())
                {
                    balance = reader.GetFloat(0);
                }
            }
            finally
            {
                reader.Close();
                conn.Close();
            }
            ViewBag.InstallmentTypes = installmentTypes;
            ViewBag.Person = person;
            ViewBag.Balance = balance;
            Session["Balance"] = balance;
            return View();
        }

        [HttpPost]
        public ActionResult AddPA(AddPA pa)
        {
            if (Session["Sec_user_id"] == null)
            {
                return RedirectToAction("LoginPage", "Login");
            }
            if (pa.amount> (float)Session["Balance"])
            {
                TempData["ErrorAddPA"] = "The amount can't be greater then balance "+ Session["Balance"].ToString() + "!";
                return RedirectToAction("AddPA", "Cases", new { case_id = Session["case_id"] }) ;
            }

            string tns = TNS.tns;
            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = tns;

            conn.Open();
            string statement = "ACTIONS.CALCULATE_PA";
            OracleCommand sql = new OracleCommand(statement, conn);
            sql.BindByName = true;
            sql.CommandType = CommandType.StoredProcedure;

            sql.Parameters.Add("P_AMOUNT", OracleDbType.Double, pa.amount, ParameterDirection.Input);
            sql.Parameters.Add("P_PERIODS", OracleDbType.Decimal, pa.periods, ParameterDirection.Input);
            sql.Parameters.Add("P_INSTALLMENT_TYPE", OracleDbType.Varchar2, pa.installment_type, ParameterDirection.Input);
            sql.Parameters.Add("P_START_DATE", OracleDbType.Date, pa.start_date, ParameterDirection.Input);
            sql.Parameters.Add("P_END_DATE", OracleDbType.Date).Direction = ParameterDirection.Output;
            sql.Parameters.Add("P_INSTALLMENT_AMOUNT", OracleDbType.Double).Direction = ParameterDirection.Output;
            sql.ExecuteNonQuery();

            AddPAsummary summary = new AddPAsummary();
            summary.person = pa.person;
            summary.amount = pa.amount;
            summary.amount_to_be_paid = (float)Session["Balance"];
            summary.periods = pa.periods;
            summary.start_date = pa.start_date;
            summary.total_balance = (float)Session["Balance"];
            summary.installment_type = pa.installment_type;
            summary.end_date = Convert.ToDateTime(((OracleDate)sql.Parameters["P_END_DATE"].Value).Value);
            summary.installment = (float) Convert.ToDouble(((OracleDecimal)sql.Parameters["P_INSTALLMENT_AMOUNT"].Value).Value);
            TempData["pa_summary"] = summary;

            return RedirectToAction("AddPAsummary", "Cases");
        }

        [HttpGet]
        public ActionResult AddPAsummary()
        {
            if (Session["Sec_user_id"] == null)
            {
                return RedirectToAction("LoginPage", "Login");
            }

            return View((AddPAsummary)TempData["pa_summary"]);
        }

        [HttpPost]
        public ActionResult AddPAsummary(AddPAsummary pa)
        {
            if (Session["Sec_user_id"] == null)
            {
                return RedirectToAction("LoginPage", "Login");
            }
            string tns = TNS.tns;
            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = tns;

            conn.Open();
            string statement = "ACTIONS.ADD_PA";
            OracleCommand sql = new OracleCommand(statement, conn);
            sql.BindByName = true;
            sql.CommandType = CommandType.StoredProcedure;

            sql.Parameters.Add("P_CASE_ID", OracleDbType.Decimal, Session["case_id"], ParameterDirection.Input);
            sql.Parameters.Add("P_AMOUNT", OracleDbType.Double, pa.amount, ParameterDirection.Input);
            sql.Parameters.Add("P_PERIODS", OracleDbType.Decimal, pa.periods, ParameterDirection.Input);
            sql.Parameters.Add("P_START_DATE", OracleDbType.Date, pa.start_date, ParameterDirection.Input);
            sql.Parameters.Add("P_END_DATE", OracleDbType.Date, pa.end_date, ParameterDirection.Input);
            sql.Parameters.Add("P_INSTALLMENT_TYPE", OracleDbType.Varchar2, pa.installment_type, ParameterDirection.Input);
            sql.Parameters.Add("P_INSTALLMENT_AMOUNT", OracleDbType.Single, pa.installment, ParameterDirection.Input);
            sql.Parameters.Add("P_FINISHED_OK", OracleDbType.Decimal).Direction = ParameterDirection.Output;
            sql.ExecuteNonQuery();
            decimal finished_ok = Convert.ToDecimal(((OracleDecimal)sql.Parameters["P_FINISHED_OK"].Value).Value);
            switch (finished_ok)
            {
                case 1:
                    return RedirectToAction("Case", "Cases", new { case_id = Session["case_id"] });
                default:
                    TempData["ErrorAddPA"] = "Something went wrong!";
                    return RedirectToAction("AddPA", "Cases", new { case_id = Session["case_id"] });
            }
        }

        [HttpGet]
        public ActionResult ViewPA(int case_id)
        {
            if (Session["Sec_user_id"] == null)
            {
                return RedirectToAction("LoginPage", "Login");
            }
            string tns = TNS.tns;
            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = tns;

            conn.Open();
            string statement = "SELECT * FROM CURRENT_PA_DETAILS WHERE CASE_ID="+case_id;
            OracleCommand sql = new OracleCommand(statement, conn);
            OracleDataReader reader = sql.ExecuteReader();
            PAdetails pAdetails = new PAdetails();
            List<PAinstallDetails> PAinstallDetails = new List<PAinstallDetails>();
            try
            {
                while (reader.Read())
                {
                    pAdetails.id = reader.GetDecimal(0);
                    pAdetails.amount = reader.GetFloat(2);
                    pAdetails.sold_amount = reader.GetFloat(3);
                    pAdetails.amount_paid = reader.GetFloat(14);
                    pAdetails.begin_date = reader.GetDateTime(4);
                    pAdetails.due_date = reader.GetDateTime(5);
                    switch (reader.GetString(6)) 
                    {
                        case "P":
                            pAdetails.status = "Pending";
                            break;
                        case "C":
                            pAdetails.status = "Cancelled";
                            break;
                        case "B":
                            pAdetails.status = "Broken";
                            break;
                        case "K":
                            pAdetails.status = "Kept";
                            break;
                        default:
                            pAdetails.status = "";
                            break;
                    }
                    pAdetails.creation_date = reader.GetDateTime(7);
                }
            }
            finally
            {
                reader.Close();
                conn.Close();
            }
            if(pAdetails.status == null)
            {
                TempData["ErrorViewPA"] = "There is no payment agrement!";
                return RedirectToAction("Case", "Cases", new { case_id = case_id });
            }

            conn.Open();
            statement = "SELECT * FROM CURRENT_PA_INSTALL_DETAILS WHERE PA_INSTANCE_ID=" + pAdetails.id;
            sql = new OracleCommand(statement, conn);
            reader = sql.ExecuteReader();
            try
            {
                int i = 1;
                while (reader.Read())
                {
                    PAinstallDetails pAinstalls = new PAinstallDetails();
                    pAinstalls.number = i;
                    pAinstalls.due_date = reader.GetDateTime(4);
                    pAinstalls.sold = reader.GetFloat(3);
                    pAinstalls.paid = reader.GetFloat(2) - reader.GetFloat(3);
                    switch (reader.GetString(5))
                    {
                        case "P":
                            pAinstalls.status = "Pending";
                            break;
                        case "C":
                            pAinstalls.status = "Cancelled";
                            break;
                        case "B":
                            pAinstalls.status = "Broken";
                            break;
                        case "K":
                            pAinstalls.status = "Kept";
                            break;
                        default:
                            pAinstalls.status = "";
                            break;
                    }
                    PAinstallDetails.Add(pAinstalls);
                    i += 1;
                }
            }
            finally
            {
                reader.Close();
                conn.Close();
            }
            ViewPA paData = new ViewPA();
            paData.PAdetails = new PAdetails();
            paData.PAinstallDetails = new List<PAinstallDetails>();
            paData.PAdetails = pAdetails;
            paData.PAinstallDetails = PAinstallDetails;
            return View(paData);
        }
    }
}