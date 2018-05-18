using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InventoryScanner.Data;
using System.Data;
using System.Data.Common;

namespace InventoryScanner.Data.Functions
{
    public static class SqliteFunctions
    {

        public static void AddTableToDB(DataTable table, string primaryKeyColumn, string scanId)
        {
            var createStatement = BuildCreateStatement(table, primaryKeyColumn);

            DBFactory.GetSqliteDatabase(scanId).ExecuteNonQuery(createStatement);

            // Update the DB with a copy of the original table. This is done so that the row states are set to
            // 'Added'. This ensures that the update command will insert the new rows.
            DBFactory.GetSqliteDatabase(scanId).UpdateTable("SELECT * FROM " + table.TableName, CopyTable(table));
        }

        /// <summary>
        /// Returns a copy of the specified table with all rows states set as added.
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public static DataTable CopyTable(DataTable table)
        {
            var newTable = table.Clone();

            foreach (DataRow row in table.Rows)
            {
                newTable.Rows.Add(row.ItemArray);
            }

            return newTable;
        }

        private static string BuildCreateStatement(DataTable table, string primaryKeyColumn)
        {
            // List for primary keys.
            var keys = new List<string>();



            string statement = "CREATE TABLE ";

            // Add the table name from the results parameter.
            // **REMEMEBER TO ADD THE TABLE NAME TO THE RESULTS DATATABLE BEFORE CALLING THIS FUNCTION**
            statement += " `" + table.TableName + "` ( ";

            // Iterate through the table columns.
            foreach (DataColumn column in table.Columns)
            {

                //// Add the field/column name and data type to the statement.
                statement += "`" + column.ColumnName.ToString() + "` ";

                var type = column.DataType;
                if (type == typeof(string))
                {
                    statement += "varchar(" + column.MaxLength + ")";
                }
                else if (type == typeof(int))
                {
                    statement += "int(" + column.MaxLength + ")";
                }
                else if (type == typeof(DateTime))
                {
                    statement += "datetime";
                }
                else if (type == typeof(sbyte))
                {
                    statement += "tinyint(1)";
                }
                else
                {
                    throw new Exception("Unexpected type.");
                }




                //// If the current field/column is a primary key, add it to the keys list.
                //if (row["Key"].ToString() == "PRI")
                //{
                //    keys.Add(row["Field"].ToString());
                //}

                // Add a column delimiter if we are not on the last item.
                if (table.Columns.IndexOf(column) != (table.Columns.Count - 1)) statement += ", ";
            }

            // Add primary keys declaration.
            statement += ", PRIMARY KEY (" + primaryKeyColumn + ")";





            //if (keys.Count > 0)
            //{
            //    // Declaration header and open parentheses.
            //    statement += ", PRIMARY KEY (";

            //    foreach (string key in keys)
            //    {
            //        // Add keys string and delimiter, if needed.
            //        statement += key;
            //        if (keys.IndexOf(key) != (keys.Count - 1)) statement += ", ";
            //    }

            //    // Close parentheses.
            //    statement += ")";
            //}

            // End of statement close parentheses.
            statement += ");";

            Console.WriteLine(statement);


            return statement;
        }
    }
}
