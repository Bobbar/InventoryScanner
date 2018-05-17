using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InventoryScanner.Data.Classes;
using InventoryScanner.Data;
using System.Data;
using System.Drawing;
using InventoryScanner.Data.Tables;


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
                
               // PopulateDepartments();
            });
            BuildIdxs.Wait();
        }

        
        private static DbAttributes BuildIndex(string attribTable, string attribName)
        {
            try
            {
                using (DataTable results = DBFactory.GetMySqlDatabase().DataTableFromQueryString(Queries.SelectAttributeCodes(attribTable, attribName)))
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
            }
            catch (Exception ex)
            {
               // ErrorHandling.ErrHandle(ex, System.Reflection.MethodBase.GetCurrentMethod());
                return null;
            }
        }
    }
}
