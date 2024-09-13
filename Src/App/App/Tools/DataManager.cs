using System.Data.OleDb;
using System.Data;
using System;
using System.Data;
using System.IO;


namespace App.Tools
{
    public class DataManager
    {
        public static DataTable ConvertCsvToDataTable(string filePath)
        {
            //reading all the lines(rows) from the file.
            string[] rows = File.ReadAllLines(filePath);

            DataTable dtData = new DataTable();
            string[] rowValues = null;
            DataRow dr = dtData.NewRow();

            //Creating columns
            if (rows.Length > 0)
            {
                foreach (string columnName in rows[0].Split(','))
                    dtData.Columns.Add(columnName);
            }

            //Creating row for each line.(except the first line, which contain column names)
            for (int row = 1; row < rows.Length; row++)
            {
                rowValues = rows[row].Split(',');
                dr = dtData.NewRow();
                dr.ItemArray = rowValues;
                dtData.Rows.Add(dr);
            }

            return dtData;
        }
        public void ShowData(DataTable dtData)
        {
            if (dtData != null && dtData.Rows.Count > 0)
            {
                foreach (DataColumn dc in dtData.Columns)
                {
                    Console.Write(dc.ColumnName + " ");
                }
                Console.WriteLine("\n-----------------------------------------------");

                foreach (DataRow dr in dtData.Rows)
                {
                    foreach (var item in dr.ItemArray)
                    {
                        Console.Write(item.ToString() + "      ");
                    }
                    Console.Write("\n");
                }
                Console.ReadKey();
            }
        }

        public static List<DataRow> ConvertDataTableToList(DataTable dataTable)
        {
            return dataTable.AsEnumerable().ToList();
        }
    }
}
