using DISERTATIE_5.Utils;
using ExcelDataReader;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace DISERTATIE_5.Controllers
{
    public class LoadingController : Controller
    {
        // GET: Loading
        public ActionResult Loading()
        {
            if (Session["Sec_user_id"] == null)
            {
                return RedirectToAction("LoginPage", "Login");
            }
            return View();
        }

        [HttpGet]
        public ActionResult DownloadTemplate()
        {
            string fullPath = Path.Combine(Server.MapPath("~/Resources/LoadingTemplate"), "LoadingTemplate.xlsx");
            return File(fullPath, "application/vnd.ms-excel", "LoadingTemplate.xlsx");
        }

        [HttpPost]
        public ActionResult LoadData(HttpPostedFileBase fileUpload)
        {
            if (Request.Files["fileUpload"].ContentLength > 0)
            {
                string path1 = string.Format("{0}/{1}", Server.MapPath("~/Resources/LoadData"), DateTime.Now.ToString("dd-MM-yyyy hh-mm-ss ") + Request.Files["fileUpload"].FileName);
                if (!Directory.Exists(path1))
                {
                    Directory.CreateDirectory(Server.MapPath("~/Resources/LoadData"));
                }
                Request.Files["fileUpload"].SaveAs(path1);

                Stream stream = fileUpload.InputStream;

                IExcelDataReader reader = ExcelReaderFactory.CreateOpenXmlReader(stream);

                DataTable dt = new DataTable();
                DataRow row;
                DataTable dt_;
                try
                {
                    dt_ = reader.AsDataSet().Tables[0];
                    for (int i = 0; i < dt_.Columns.Count; i++)
                    {
                        dt.Columns.Add(dt_.Rows[0][i].ToString());
                    }
                    int rowcounter = 0;
                    for (int row_ = 1; row_ < dt_.Rows.Count; row_++)
                    {
                        row = dt.NewRow();

                        for (int col = 0; col < dt_.Columns.Count; col++)
                        {
                            row[col] = dt_.Rows[row_][col].ToString();
                            rowcounter++;
                        }
                        dt.Rows.Add(row);
                    }

                }
                catch
                {
                    ModelState.AddModelError("File", "Unable to Upload file!");
                    return View();
                }

                string tns = TNS.tns;
                OracleConnection conn = new OracleConnection();
                conn.ConnectionString = tns;

                conn.Open();
                string statement = "LOADING_PKG.GET_IMPORT_ID";
                OracleCommand sql = new OracleCommand(statement, conn);
                sql = new OracleCommand(statement, conn);
                sql.BindByName = true;
                sql.CommandType = CommandType.StoredProcedure;
                sql.Parameters.Add("P_ID", OracleDbType.Decimal).Direction = ParameterDirection.Output;
                sql.ExecuteNonQuery();
                decimal batch_number = Convert.ToDecimal(((OracleDecimal)sql.Parameters["P_ID"].Value).Value);
                string date_format = "dd/MM/yyyy HH:mm:ss";
                
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    statement = "INSERT INTO LOADING_BUFF VALUES (LOADING_BUFF_SEQ.NEXTVAL, ";
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        if (dt.Columns[j].ColumnName == "DEBTOR_SSN")
                        {
                            statement += "'" + dt.Rows[i][j].ToString() + "', ";
                        }
                        else if (dt.Rows[i][j].ToString() != null && dt.Rows[i][j].ToString() != "" && float.TryParse(dt.Rows[i][j].ToString(), out _))
                        {
                            statement += dt.Rows[i][j].ToString() + ", ";
                        }
                        else if (DateTime.TryParseExact(dt.Rows[i][j].ToString(), date_format, new CultureInfo("en-US"), DateTimeStyles.None, out _))
                        {
                            statement += "TRUNC(TO_DATE('" + dt.Rows[i][j].ToString() + "', 'dd/mm/yyyy hh24:mi:ss')), ";
                        }
                        else if (dt.Rows[i][j].ToString() != null && dt.Rows[i][j].ToString() != "")
                        {
                            statement += "'" + dt.Rows[i][j].ToString() + "', ";
                        }
                        else
                        {
                            statement += "NULL, ";
                        }
                        if (j == dt.Columns.Count - 1)
                        {
                            statement += batch_number + ")";
                        }
                    }
                    sql= new OracleCommand(statement, conn);
                    sql.ExecuteNonQuery();
                }

                statement = "LOADING_PKG.IMPORT_DATA";
                sql = new OracleCommand(statement, conn);
                sql.BindByName = true;
                sql.CommandType = CommandType.StoredProcedure;
                sql.Parameters.Add("P_BACH_NUMBER", OracleDbType.Decimal, batch_number, ParameterDirection.Input);
                sql.Parameters.Add("P_OWNER", OracleDbType.Decimal, (decimal)Session["Sec_user_id"], ParameterDirection.Input);
                sql.Parameters.Add("P_FINISHED_OK", OracleDbType.Decimal).Direction = ParameterDirection.Output;
                sql.ExecuteNonQuery();
                decimal finished_ok = Convert.ToDecimal(((OracleDecimal)sql.Parameters["P_FINISHED_OK"].Value).Value);
                switch (finished_ok)
                {
                    case 1:
                        return RedirectToAction("Loading", "Loading");
                    case 0:
                        TempData["Import_error"] = "Import was not finished! Please verify the data from file!";
                        return RedirectToAction("Loading", "Loading");
                    default:
                        return RedirectToAction("Loading", "Loading");
                }
                
            }
            else
            {
                return RedirectToAction("Loading", "Loading");
            }
        }
    }
}