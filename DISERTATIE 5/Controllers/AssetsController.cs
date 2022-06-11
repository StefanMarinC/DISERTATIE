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
    public class AssetsController : Controller
    {
        // GET: Assets
        public ActionResult Search()
        {
            if (Session["Sec_user_id"] == null)
            {
                return RedirectToAction("LoginPage", "Login");
            }
            return View();
        }

        public JsonResult GetAsset_Categories()
        {
            string tns = TNS.tns;
            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = tns;
            conn.Open();
            List<Asset_category> asset_Categories = new List<Asset_category>();
            string statement = "SELECT ID, NAME FROM ASSETS_CATEGORIES";
            OracleCommand sql = new OracleCommand(statement, conn);
            OracleDataReader reader = sql.ExecuteReader();
            try
            {
                while (reader.Read())
                {
                    Asset_category asset_Category = new Asset_category();
                    asset_Category.category_id = reader.GetInt32(0);
                    asset_Category.category_name = reader.GetString(1);
                    asset_Categories.Add(asset_Category);
                }
            }
            finally
            {
                reader.Close();
                conn.Close();
            }
            List<SelectListItem> assetCateg = new List<SelectListItem>();
            SelectListItem none = new SelectListItem();
            none.Value = "0";
            none.Text = "- None -";
            assetCateg.Add(none);
            foreach (var item in asset_Categories)
            {
                var category = new SelectListItem()
                {
                    Value = item.category_id.ToString(),
                    Text = item.category_name.ToString()
                };

                assetCateg.Add(category);
            }
            return Json(new SelectList(assetCateg, "Value", "Text"));
        }

        public JsonResult GetAsset_Category_Types_by_categ(int categ_id)
        {
            string tns = TNS.tns;
            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = tns;
            conn.Open();
            List<Asset_category_type> Asset_category_type = new List<Asset_category_type>();
            string statement = "SELECT ID, NAME FROM ASSETS_CATEGORIES_TYPES WHERE CATEGORY_ID=" + categ_id;
            OracleCommand sql = new OracleCommand(statement, conn);
            OracleDataReader reader = sql.ExecuteReader();
            try
            {
                while (reader.Read())
                {
                    Asset_category_type asset_Category_Type = new Asset_category_type();
                    asset_Category_Type.type_id = reader.GetInt32(0);
                    asset_Category_Type.type_name = reader.GetString(1);
                    Asset_category_type.Add(asset_Category_Type);
                }
            }
            finally
            {
                reader.Close();
                conn.Close();
            }
            List<SelectListItem> assetTypes = new List<SelectListItem>();
            SelectListItem none = new SelectListItem();
            none.Value = "0";
            none.Text = "- None -";
            assetTypes.Add(none);
            foreach (var item in Asset_category_type)
            {
                var category = new SelectListItem()
                {
                    Value = item.type_id.ToString(),
                    Text = item.type_name.ToString()
                };

                assetTypes.Add(category);
            }
            return Json(new SelectList(assetTypes, "Value", "Text"));
        }

        public JsonResult GetAsset_Categ_Type_Subtypes_by_type(int type_id)
        {
            string tns = TNS.tns;
            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = tns;
            conn.Open();
            List<Asset_categ_type_subtype> Asset_categ_type_subtype = new List<Asset_categ_type_subtype>();
            string statement = "SELECT ID, NAME FROM ASSETS_CATEG_TYPES_SUBTYPES WHERE CATEGORY_TYPE_ID=" + type_id;
            OracleCommand sql = new OracleCommand(statement, conn);
            OracleDataReader reader = sql.ExecuteReader();
            try
            {
                while (reader.Read())
                {
                    Asset_categ_type_subtype asset_Categ_Type_Subtype = new Asset_categ_type_subtype();
                    asset_Categ_Type_Subtype.subtype_id = reader.GetInt32(0);
                    asset_Categ_Type_Subtype.subtype_name = reader.GetString(1);
                    Asset_categ_type_subtype.Add(asset_Categ_Type_Subtype);
                }
            }
            finally
            {
                reader.Close();
                conn.Close();
            }
            List<SelectListItem> assetSubtypes = new List<SelectListItem>();
            SelectListItem none = new SelectListItem();
            none.Value = "0";
            none.Text = "- None -";
            assetSubtypes.Add(none);
            foreach (var item in Asset_categ_type_subtype)
            {
                var category = new SelectListItem()
                {
                    Value = item.subtype_id.ToString(),
                    Text = item.subtype_name.ToString()
                };

                assetSubtypes.Add(category);
            }
            return Json(new SelectList(assetSubtypes, "Value", "Text"));
        }

        [HttpGet]
        public ActionResult AddAsset()
        {
            if (Session["Sec_user_id"] == null)
            {
                return RedirectToAction("LoginPage", "Login");
            }
            return View();
        }

        [HttpPost]
        public ActionResult AddAsset(string category, string type, string subtype, string assset_status, string building_status, string city, string construction_year,
                                     string thermal_rehabilitation, string year_of_last_rehabilitation, string company_status, string colour, string value,
                                     string license_plate, string manufacturing_year, string model_type, string cilindrical_capacity)
        {
            string tns = TNS.tns;
            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = tns;
            conn.Open();

            string statement = "ASSETS_PKG.ADD_ASSET";
            OracleCommand sql = new OracleCommand(statement, conn);
            decimal finished_ok = 0;
            sql.BindByName = true;
            sql.CommandType = CommandType.StoredProcedure;
            sql.Parameters.Add("P_CATEGORY", OracleDbType.Decimal, category, ParameterDirection.Input);
            sql.Parameters.Add("P_CATEGORY_TYPE", OracleDbType.Decimal, type, ParameterDirection.Input);
            sql.Parameters.Add("P_SUBTYPE", OracleDbType.Decimal, subtype, ParameterDirection.Input);
            sql.Parameters.Add("P_ASSET_TYPE", OracleDbType.Varchar2, assset_status, ParameterDirection.Input);
            sql.Parameters.Add("P_BUILDING_STATUS", OracleDbType.Varchar2, building_status, ParameterDirection.Input);
            sql.Parameters.Add("P_CITY", OracleDbType.Varchar2, city, ParameterDirection.Input);
            sql.Parameters.Add("P_CONSTRUCTION_YEAR", OracleDbType.Varchar2, construction_year, ParameterDirection.Input);
            sql.Parameters.Add("P_THERMAL_REHABILITATION", OracleDbType.Varchar2, thermal_rehabilitation, ParameterDirection.Input);
            sql.Parameters.Add("P_YEAR_REHABILITATION", OracleDbType.Varchar2, year_of_last_rehabilitation, ParameterDirection.Input);
            sql.Parameters.Add("P_COMPANY_STATUS", OracleDbType.Varchar2, company_status, ParameterDirection.Input);
            sql.Parameters.Add("P_COLOUR", OracleDbType.Varchar2, colour, ParameterDirection.Input);
            sql.Parameters.Add("P_VALUE", OracleDbType.Varchar2, value, ParameterDirection.Input);
            sql.Parameters.Add("P_LICENSE_PLATE", OracleDbType.Varchar2, license_plate, ParameterDirection.Input);
            sql.Parameters.Add("P_MANUFACTURING_YEAR", OracleDbType.Varchar2, manufacturing_year, ParameterDirection.Input);
            sql.Parameters.Add("P_MODEL_TYPE", OracleDbType.Varchar2, model_type, ParameterDirection.Input);
            sql.Parameters.Add("P_CILINDRICAL_CAPACITY", OracleDbType.Varchar2, cilindrical_capacity, ParameterDirection.Input);


            sql.Parameters.Add("P_FINISHED_OK", OracleDbType.Decimal).Direction = ParameterDirection.Output;
            sql.ExecuteNonQuery();
            finished_ok = Convert.ToDecimal(((OracleDecimal)sql.Parameters["P_FINISHED_OK"].Value).Value);

            switch (finished_ok)
            {
                case 1:
                    return RedirectToAction("Search", "Assets");
                default:
                    TempData["ErrorAddAsset"] = "Something went wrong!";
                    return RedirectToAction("AddAsset", "Assets");
            }
        }
    }

    public class Asset_category
    {
        public int category_id { get; set; }
        public string category_name { get; set; }
    }

    public class Asset_category_type
    {
        public int type_id { get; set; }
        public string type_name { get; set; }
    }

    public class Asset_categ_type_subtype
    {
        public int subtype_id { get; set; }
        public string subtype_name { get; set; }
    }
}