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

        [HttpPost]
        public ActionResult Search(int? asset_id, string owner_name, string owner_ssn, int? category, int? type, int? subtype)
        {
            if (Session["Sec_user_id"] == null)
            {
                return RedirectToAction("LoginPage", "Login");
            }
            string tns = TNS.tns;
            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = tns;
            conn.Open();
            List<AssetSearch> AssetSearch = new List<AssetSearch>();
            string statement = "SELECT * FROM ASSETS_SEARCH A WHERE 1=1";
            if (asset_id != null)
            {
                statement += " AND A.ASSET_ID=" + asset_id;
            }
            if (owner_name != null && owner_name != "")
            {
                statement += " AND A.OWNER_NAME LIKE '%" + owner_name + "%'";
            }
            if (owner_ssn != null && owner_name != "")
            {
                statement += " AND A.OWNER_SSN LIKE '%" + owner_ssn + "%'";
            }
            if (category != null && category > 0)
            {
                statement += " AND A.ASSET_CATEGORY_ID=" + category;
            }
            if (type != null && type > 0)
            {
                statement += " AND A.ASSET_TYPE_ID=" + type;
            }
            if (subtype != null && subtype > 0)
            {
                statement += " AND A.ASSET_SUB_TYPE_ID=" + subtype;
            }
            statement += " ORDER BY 1";
            OracleCommand sql = new OracleCommand(statement, conn);
            OracleDataReader reader = sql.ExecuteReader();
            try
            {
                while (reader.Read())
                {
                    AssetSearch asset = new AssetSearch();
                    asset.asset_id = reader.GetDecimal(0);
                    asset.category = reader.GetString(1);
                    if (!reader.IsDBNull(3))
                    {
                        asset.type = reader.GetString(3);
                    }
                    if (!reader.IsDBNull(5))
                    {
                        asset.subtype = reader.GetString(5);
                    }
                    if (!reader.IsDBNull(7))
                    {
                        asset.description = reader.GetString(7);
                    }
                    AssetSearch.Add(asset);
                }

            }
            finally
            {
                reader.Close();
                conn.Close();
            }
            TempData["ASSETS"] = AssetSearch;
            TempData["Assets_nr"] = AssetSearch.Count();
            return RedirectToAction("Search", "Assets");
        }

        [HttpGet]
        public ActionResult Asset(int asset_id)
        {
            if (Session["Sec_user_id"] == null)
            {
                return RedirectToAction("LoginPage", "Login");
            }
            string tns = TNS.tns;
            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = tns;
            conn.Open();
            List<Asset_prop> assetProps = new List<Asset_prop>();
            string statement = "SELECT AP.NAME, API.PROPERTY_VALUE FROM ASSET_PROPERTIES AP JOIN ASSET_PROPERTIES_INT API ON API.PROPERTY_ID = AP.PROPERTY_ID WHERE API.ASSET_ID = " + asset_id;
            OracleCommand sql = new OracleCommand(statement, conn);
            OracleDataReader reader = sql.ExecuteReader();
            try
            {
                while (reader.Read())
                {
                    Asset_prop asset_Prop = new Asset_prop();
                    asset_Prop.property_name = reader.GetString(0);
                    asset_Prop.property_value = reader.GetString(1);
                    assetProps.Add(asset_Prop);
                }
            }
            finally
            {
                reader.Close();
                conn.Close();
            }
            conn.Open();
            List<Asset_owners> assetOwners = new List<Asset_owners>();
            statement = "SELECT S.FIRST_NAME || ' ' || S.LAST_NAME AS OWNER_NAME, S.SSN AS PERSON_SSN, S.SUBSCRIBER_ID AS PERSON_ID FROM ASSET_OWNERS AO JOIN SUBSCRIBERS S"
                         + " ON S.SUBSCRIBER_ID = AO.SUBSCRIBER_ID WHERE AO.ASSET_ID = " + asset_id;
            sql = new OracleCommand(statement, conn);
            reader = sql.ExecuteReader();
            try
            {
                while (reader.Read())
                {
                    Asset_owners asset_Owner = new Asset_owners();
                    asset_Owner.owner_name = reader.GetString(0);
                    asset_Owner.person_ssn = reader.GetString(1);
                    asset_Owner.person_id = reader.GetDecimal(2);
                    assetOwners.Add(asset_Owner);
                }
            }
            finally
            {
                reader.Close();
                conn.Close();
            }
            AssetData assetData = new AssetData();
            assetData.props = new List<Asset_prop>();
            assetData.owners = new List<Asset_owners>();
            assetData.props = assetProps;
            assetData.owners = assetOwners;
            assetData.asset_id = asset_id;
            return View(assetData);
        }

        [HttpPost]
        public JsonResult AddOwner(int asset_id, string ssn)
        {
            if (Session["Sec_user_id"] == null)
            {
                RedirectToAction("LoginPage", "Login");
            }
            string tns = TNS.tns;
            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = tns;
            conn.Open();
            string statement = "ASSETS_PKG.ASSOCIATE_OWNER";
            OracleCommand sql = new OracleCommand(statement, conn);
            decimal finished_ok = 0;
            sql.BindByName = true;
            sql.CommandType = CommandType.StoredProcedure;

            sql.Parameters.Add("P_ASSET_ID", OracleDbType.Decimal, asset_id, ParameterDirection.Input);
            sql.Parameters.Add("P_SSN", OracleDbType.Varchar2, ssn, ParameterDirection.Input);
            sql.Parameters.Add("P_FINISHED_OK", OracleDbType.Decimal).Direction = ParameterDirection.Output;
            sql.ExecuteNonQuery();
            finished_ok = Convert.ToDecimal(((OracleDecimal)sql.Parameters["P_FINISHED_OK"].Value).Value);
            switch (finished_ok)
            {
                case 1:
                    return Json(finished_ok);
                case 2:
                    TempData["ErrorAddOwner"] = "A person with this SSN doesn't exist!";
                    return Json(finished_ok);
                case 3:
                    TempData["ErrorAddOwner"] = "The person is already associated on this asset!";
                    return Json(finished_ok);
                default:
                    TempData["ErrorAddOwner"] = "Something went wrong!";
                    return Json(finished_ok);
            }
        }

        [HttpGet]
        public ActionResult DeleteOwner(int asset_id, int owner_id)
        {
            if (Session["Sec_user_id"] == null)
            {
                return RedirectToAction("LoginPage", "Login");
            }
            string tns = TNS.tns;
            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = tns;
            conn.Open();
            string statement = "DELETE FROM ASSET_OWNERS WHERE ASSET_ID=" + asset_id + " AND SUBSCRIBER_ID=" + owner_id ;
            OracleCommand sql = new OracleCommand(statement, conn);
            sql.ExecuteNonQuery();
            return RedirectToAction("Asset", "Assets", new { asset_id= asset_id });
        }

        [HttpGet]
        public ActionResult EditProperties(int asset_id)
        {
            return RedirectToAction("Asset", "Assets", new { asset_id = asset_id}) ;
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

    public class Asset_prop
    {
        public string property_name { get; set; }
        public string property_value { get; set; }
    }

    public class Asset_owners
    {
        public string owner_name { get; set; }
        public string person_ssn { get; set; }
        public decimal person_id { get; set; }
    }

    public class AssetData
    {
        public List<Asset_prop> props { get; set; }
        public List<Asset_owners> owners { get; set; }
        public decimal asset_id { get; set; }

    }
}