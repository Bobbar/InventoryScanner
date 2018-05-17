using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InventoryScanner.Data.Tables;

namespace InventoryScanner.Data
{
    public static class Queries
    {
        /// <summary>
        /// SELECT * FROM <paramref name="attribTable"/> LEFT OUTER JOIN munis_codes on <paramref name="attribName"/>.db_value = munis_codes.asset_man_code WHERE type_name ='<paramref name="attribName"/>' ORDER BY <see cref="ComboCodesBaseCols.DisplayValue"/>
        /// </summary>
        /// <param name="attribTable"></param>
        /// <param name="attribName"></param>
        /// <returns></returns>
        public static string SelectAttributeCodes(string attribTable, string attribName)
        {
            return "SELECT * FROM " + attribTable + " LEFT OUTER JOIN munis_codes on " + attribTable + ".db_value = munis_codes.asset_man_code WHERE type_name ='" + attribName + "' ORDER BY " + ComboCodesBaseCols.DisplayValue;
        }


        public static string SelectDepartmentByLocation(string locationCode)
        {
            return "SELECT * FROM " + MunisDepartments.TableName + " WHERE " + MunisDepartments.AssetLocation + " = '" + locationCode + "'";
        }

        public static string SelectScanItemsByDepartment(string departmentCode)
        {
            var query = "";
            query += "SELECT a_asset_number, fa_status,fa_class_code,fs_subclass_code,fa_tag_number,fa_serial_number,a_asset_desc,a_location,fa_purchase_memo";
            query += " FROM fa_master";
            query += " WHERE a_department_code = '" + departmentCode + "' AND fa_status = 'A' AND fs_subclass_code IN (411,403,422,410,430,400,438,415,437,416,429)";
            return query;
        }
    }
}
