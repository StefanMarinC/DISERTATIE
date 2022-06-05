using DISERTATIE_5.Utils;
using FluentScheduler;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace DISERTATIE_5.Models.Jobs
{
    public class JobCurrency:Registry
    {
        public JobCurrency()
        {
            Schedule<MyJob>().ToRunEvery(1).Days().At(hours:16, minutes:20);
        }
    }
    public class MyJob : IJob
    {

        void IJob.Execute()
        {
            string tns = TNS.tns;
            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = tns;
            conn.Open();
            DateTime last_update_currency = new DateTime(2005, 01, 03);
            string statement = "SELECT NVL(MAX(CURRENCY_DATE), TO_DATE('01/01/2000','DD/MM/YYYY')) FROM FX_EXCHANGE_RATES";
            OracleCommand sql = new OracleCommand(statement, conn);
            OracleDataReader reader = sql.ExecuteReader();
            try
            {
                while (reader.Read())
                {
                    if (reader.GetDateTime(0) >= last_update_currency)
                    {
                        last_update_currency = reader.GetDateTime(0).AddDays(1);
                    }
                }
            }
            finally
            {
                reader.Close();
                conn.Close();
            }

            conn.Open();
            statement = "SELECT NAME FROM CURRENCIES";
            sql = new OracleCommand(statement, conn);
            reader = sql.ExecuteReader();
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
            DateTime current_Date = DateTime.Today;
            for (DateTime dt = last_update_currency; dt <= current_Date; dt = dt.AddDays(1))
            {
                HtmlAgilityPack.HtmlWeb website = new HtmlAgilityPack.HtmlWeb();
                string link = "https://www.cursbnr.ro/arhiva-curs-bnr-" + dt.Year + "-" + dt.Month.ToString("00") + "-" + dt.Day.ToString("00");
                HtmlAgilityPack.HtmlDocument document = website.Load(link);
                var datalist = document.DocumentNode.SelectNodes("//tr").ToList();

                conn.Open();
                try
                {
                    foreach (var content in datalist)
                    {
                        foreach (string curr in currencyList)
                        {
                            int exists = content.InnerHtml.IndexOf(curr);
                            if (exists != -1)
                            {
                                statement = "INSERT INTO FX_EXCHANGE_RATES (CURRENCY_DATE, DIVIDER, FROM_CURRENCY, TO_CURRENCY) VALUES (:P1, :P2, :P3, :P4)";
                                sql = new OracleCommand(statement, conn);
                                int indexStart = content.InnerHtml.IndexOfAny("0123456789".ToCharArray());
                                int indexStop = content.InnerHtml.IndexOf('<', indexStart + 1);
                                string value = content.InnerHtml.Remove(0, indexStart);
                                value = value.Remove(indexStop - indexStart);
                                float divider = float.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
                                sql.Parameters.Add(":P1", OracleDbType.Date, dt, ParameterDirection.Input);
                                sql.Parameters.Add(":P2", OracleDbType.Double, divider, ParameterDirection.Input);
                                sql.Parameters.Add(":P3", OracleDbType.Varchar2, "RON", ParameterDirection.Input);
                                sql.Parameters.Add(":P4", OracleDbType.Varchar2, curr, ParameterDirection.Input);

                                sql.ExecuteNonQuery();
                            }
                        }
                    }
                }
                finally
                {
                    conn.Close();
                }
            }
        }
    }
}