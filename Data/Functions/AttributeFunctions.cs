using InventoryScanner.Data.Classes;
using InventoryScanner.Data.Munis;
using InventoryScanner.Data.Tables;
using System;
using System.Data;
using System.Drawing;
using System.Threading.Tasks;

namespace InventoryScanner.Data.Functions
{
    internal static class AttributeFunctions
    {
        public static void PopulateAttributeIndexes()
        {
            var BuildIdxs = Task.Run(() =>
            {

                AttributeInstances.DeviceAttributes.Locations = BuildIndex("dev_codes", AttributeTypes.Device.Location);
                AttributeInstances.DeviceAttributes.ChangeType = BuildIndex("dev_codes", AttributeTypes.Device.ChangeType);
                AttributeInstances.DeviceAttributes.EquipType = BuildIndex("dev_codes", AttributeTypes.Device.EquipType);
                AttributeInstances.DeviceAttributes.OSType = BuildIndex("dev_codes", AttributeTypes.Device.OSType);
                AttributeInstances.DeviceAttributes.StatusType = BuildIndex("dev_codes", AttributeTypes.Device.StatusType);

                PopulateMunisAttributes();
                PopulateSubnetLocationAttributes();
                PopulateMunisToAssetAttributes();
                // PopulateDepartments();

            });

            BuildIdxs.Wait();
        }

        private static void PopulateSubnetLocationAttributes()
        {
            var tmpAttribs = new DbAttributes();

            using (var results = DBFactory.GetSqliteCacheDatabase().DataTableFromQueryString(Queries.Assets.SelectSubnetLocations()))
            {
                foreach (DataRow row in results.Rows)
                {
                    tmpAttribs.Add(row[SubnetLocationsTable.Description].ToString(), row[SubnetLocationsTable.Subnet].ToString(), Convert.ToInt32(row[SubnetLocationsTable.Id]));
                }
            }

            AttributeInstances.DeviceAttributes.SubnetLocation = tmpAttribs;
        }

        private static void PopulateMunisAttributes() // How to pull this from cache?
        {
            var tmpAttribs = new DbAttributes();

            using (var results = DBFactory.GetSqliteCacheDatabase().DataTableFromQueryString(Queries.Munis.SelectLocations()))//MunisDatabase.ReturnSqlTable(Queries.Munis.SelectLocations()))
            {
                foreach (DataRow row in results.Rows)
                {
                    tmpAttribs.Add(row[MunisLocations.Description].ToString().Trim(), row[MunisLocations.Code].ToString().Trim(), Convert.ToInt32(row[MunisLocations.Code]));
                }
            }

            AttributeInstances.MunisAttributes.MunisLocations = tmpAttribs;
        }

        private static void PopulateMunisToAssetAttributes()
        {
            var tmpAttribs = new DbAttributes();

            using (var results = DBFactory.GetSqliteCacheDatabase().DataTableFromQueryString(Queries.Assets.SelectMunisAndAssetLocationCodes()))
            {
                foreach (DataRow row in results.Rows)
                {
                    tmpAttribs.Add(row[DeviceCodesTable.HumanValue].ToString(), row[MunisDepartmentsTable.MunisLocation].ToString(), Convert.ToInt32(row[MunisDepartmentsTable.Id]));
                }
            }

            AttributeInstances.MunisAttributes.MunisToAssetLocations = tmpAttribs;
        }


        private static DbAttributes BuildIndex(string attribTable, string attribName)
        {
            //try
            //{
            using (DataTable results = DBFactory.GetSqliteCacheDatabase().DataTableFromQueryString(Queries.Assets.SelectAttributeCodes(attribTable, attribName)))
            {
                var tmpAttrib = new DbAttributes();
                foreach (DataRow r in results.Rows)
                {
                    string displayValue = "";
                    if (r.Table.Columns.Contains("munis_code"))
                    {
                        if (r["munis_code"] != DBNull.Value)
                        {
                            displayValue = r[ComboCodesBaseCols.DisplayValue].ToString() + " - " + r["munis_code"].ToString();
                        }
                        else
                        {
                            displayValue = r[ComboCodesBaseCols.DisplayValue].ToString();
                        }
                    }
                    else
                    {
                        displayValue = r[ComboCodesBaseCols.DisplayValue].ToString();
                    }

                    Color attribColor = Color.Empty;
                    if (!string.IsNullOrEmpty(r[ComboCodesBaseCols.Color].ToString()))
                    {
                        attribColor = ColorTranslator.FromHtml(r[ComboCodesBaseCols.Color].ToString());
                    }

                    tmpAttrib.Add(displayValue, r[ComboCodesBaseCols.CodeValue].ToString(), Convert.ToInt32(r[ComboCodesBaseCols.Id]), attribColor);
                }
                return tmpAttrib;
            }
            //}
            //catch (Exception ex)
            //{
            //    // ErrorHandling.ErrHandle(ex, System.Reflection.MethodBase.GetCurrentMethod());
            //    return null;
            //}
        }
    }
}