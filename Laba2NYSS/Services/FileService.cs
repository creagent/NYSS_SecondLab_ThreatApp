using ExcelDataReader;
using Laba2NYSS.Models;
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Net;
using System.Windows;

namespace Laba2NYSS.Services
{
    static class FileService
    {
        internal static void DownloadFile(object obj)
        {
            var arr = obj as string[];
            var url = arr[0];
            var path = arr[1];

            using (var client = new WebClient())
            {
                try
                {
                    client.DownloadFile(url, path);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        internal static DataSet ReadExcelFile(string path)
        {
            DataSet data;
            using (var stream = File.Open(path, FileMode.Open, FileAccess.Read))
            {
                using (IExcelDataReader reader = ExcelReaderFactory.CreateReader(stream))
                {
                    data = reader.AsDataSet(
                        new ExcelDataSetConfiguration()
                        {
                            ConfigureDataTable = (tableReader) => new ExcelDataTableConfiguration()
                            {
                                UseHeaderRow = true,
                            }
                        }
                    );
                }
            }

            return data;
        }
        public static BindingList<Threat> LoadJsonData(string path)
        {
            bool fileExists = File.Exists(path);
            if (!fileExists)
            {
                File.CreateText(path).Dispose();
                return new BindingList<Threat>();
            }
            using (var reader = File.OpenText(path))
            {
                var fileText = reader.ReadToEnd();
                return JsonConvert.DeserializeObject<BindingList<Threat>>(fileText);
            }
        }

        public static void SaveDataToJson(object threatList, string path)
        {
            using (StreamWriter writer = File.CreateText(path))
            {
                string output = JsonConvert.SerializeObject(threatList);
                writer.Write(output);
            }
        }
    }
}
