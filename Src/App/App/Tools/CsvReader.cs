﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using App.Models;
using System.Linq;

namespace App.Tools
{


    public class CsvReader
    {
        public List<EnergyData> ReadCsv(string path)
        {
            var records = new List<EnergyData>();

            using (var reader = new StreamReader(path))
            {
                string line;
                bool isHeader = true;
                int skipLines = 11; // Number of lines to skip (metadata + headers)

                // Skip the first few lines (metadata and headers)
                for (int i = 0; i < skipLines; i++)
                {
                    reader.ReadLine();
                }

                // Read and process the actual data rows
                while ((line = reader.ReadLine()) != null)
                {
                    var values = line.Split(';');

                    // Skip lines with insufficient data
                    if (values.Length < 13)
                    {
                        continue;
                    }

                    var record = new EnergyData
                    {
                        Month = GetMonth(values[0]),
                        Year = GetYear(values[0]),
                        Hydropower = ParseNullableInt(values[1]),
                        NuclearPower = ParseNullableInt(values[2]),
                        ThermalPower = ParseNullableInt(values[3]),
                        TotalProduction = ParseNullableInt(values[4]),
                        PumpingStorage = ParseNullableInt(values[5]),
                        NetProduction = ParseNullableInt(values[6]),
                        Import = ParseNullableInt(values[7]),
                        Export = ParseNullableInt(values[8]),
                        DomesticConsumption = ParseNullableInt(values[9]),
                        Losses = ParseNullableInt(values[10]),
                        FinalConsumption = ParseNullableInt(values[11]),
                        ExportImportBalance = ParseNullableInt(values[12])
                    };

                    records.Add(record);
                }
            }

            return records;
        }

        private int? ParseNullableInt(string value)
        {
            if (int.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out int result))
            {
                return result;
            }
            return null;
        }

        private int GetYear(string value)
        {
            if (value is "") return 0;
            int returnVal = 0;
            returnVal = int.Parse(value.Substring(value.Length - 5, 5));
            return returnVal;
        }
        private string GetMonth(string value)
        {
            if (value is "") return "";

            string first3Letters = value.Substring(0, 3);
            
            switch (first3Letters)
            {
                default: return "";
                case "Jan": return "January";
                case "Feb": return "February";
                case "Mär": return "March";
                case "Apr": return "April";
                case "Mai": return "May";
                case "Jun": return "June";
                case "Jul": return "July";
                case "Aug": return "August";
                case "Sep": return "September";
                case "Okt": return "October";
                case "Nov": return "November";
                case "Dez": return "December";
            }

        }
    }

}
