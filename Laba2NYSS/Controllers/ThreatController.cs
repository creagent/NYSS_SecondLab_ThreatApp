using Laba2NYSS.Models;
using Laba2NYSS.Services;
using System;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;

namespace Laba2NYSS.Controllers
{
    static class ThreatController
    {
        private static BindingList<Threat> _threatList = new BindingList<Threat>();
        private static BindingList<Threat> _newThreatList = new BindingList<Threat>();

        private static string JSON_PATH = $"{Environment.CurrentDirectory}\\threatList.json";
        private static readonly string EXCEL_URL = "https://bdu.fstec.ru/files/documents/thrlist.xlsx";
        private static readonly string EXCEL_PATH = $"{Environment.CurrentDirectory}\\thrlist.xlsx";

        internal static BindingList<Threat> GetData()
        {

            if (File.Exists(JSON_PATH))
            {
                try
                {
                    _threatList = FileService.LoadJsonData(JSON_PATH);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    _threatList = new BindingList<Threat>();
                }
            }

            return _threatList;
        }

        internal static void UpdateInfo(out BindingList<ThreatUpdate> changedThreats, out BindingList<Threat> newThreats,
                             out BindingList<Threat> deletedThreats)
        {
            var downloadThread = new Thread(FileService.DownloadFile);
            downloadThread.Start(new string[2] { EXCEL_URL, EXCEL_PATH });
            
            if (File.Exists(JSON_PATH))
            {
                _threatList = FileService.LoadJsonData(JSON_PATH);
            }

            downloadThread.Join();

            DataSet data = FileService.ReadExcelFile(EXCEL_PATH);
            FillThreatList(data, _newThreatList);

            Compare(out changedThreats, out newThreats, out deletedThreats);

            _threatList = _newThreatList;
            Threat.SetLatestUpdateDate();
            FileService.SaveDataToJson(_threatList, JSON_PATH);

            _newThreatList = new BindingList<Threat>();
        }

        private static void Compare(out BindingList<ThreatUpdate> changedThreats, out BindingList<Threat> newThreats, 
                             out BindingList<Threat> deletedThreats)
        {
            changedThreats = new BindingList<ThreatUpdate>();
            newThreats = new BindingList<Threat>();
            deletedThreats = new BindingList<Threat>();

            foreach (var threat in _newThreatList)
            {
                if (_threatList.Where(x => x.Id == threat.Id).ToArray().Length == 0)
                {
                    newThreats.Add(threat);
                }
                else
                {
                    var oldThreat = _threatList.Where(x => x.Id == threat.Id).ToArray()[0];
                    foreach (var update in ThreatUpdate.CompareThreats(oldThreat, threat))
                    {
                        changedThreats.Add(update);
                    } 
                }
            }

            foreach (var threat in _threatList)
            {
                if (_newThreatList.Where(x => x.Id == threat.Id).ToArray().Length == 0)
                {
                    deletedThreats.Add(threat);
                }
            }
        }

        private static void FillThreatList(DataSet data, BindingList<Threat> list)
        {
            for (int i = 1; i < data.Tables[0].Rows.Count; i++)
            {
                var row = data.Tables[0].Rows[i];
                list.Add(
                    new Threat(row[0].ToString(),
                        name: row[1].ToString(),
                        description: row[2].ToString(),
                        source: row[3].ToString(),
                        @object: row[4].ToString(),
                        privacyViolation: row[5].ToString(),
                        integrityViolation: row[6].ToString(),
                        accessViolation: row[7].ToString())
                ); 
            }
        }
    }
}
