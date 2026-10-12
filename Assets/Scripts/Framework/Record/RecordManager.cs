using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Framework
{
    public class RecordManager : Singleton<RecordManager>
    {
        public string GetDirectory => $"{Application.persistentDataPath}/Record";
        public string GetPath(long id) => $"{GetDirectory}/{id:X}.sav";

        public Record Create(long? id = null)
        {
            var record = Record.Create();
            record.recordId = id ?? DateTime.Now.Ticks;
            Save(record);
            return record;
        }

        public void Delete(long id)
        {
            string path = GetPath(id);
            if (File.Exists(path))
                File.Delete(path);
        }

        public void DeleteAll()
        {
            if (Directory.Exists(GetDirectory))
                Directory.Delete(GetDirectory, true);
        }

        public void Save(Record record)
        {
            record.lastSaveTime = DateTime.Now.Ticks;
            if (!Directory.Exists(GetDirectory))
                Directory.CreateDirectory(GetDirectory);
            string path = GetPath(record.recordId);
            string json = Helper.JsonSerialize(record);
            File.WriteAllText(path, json);
        }

        public Record Read(long id)
        {
            string path = GetPath(id);
            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                var record = Helper.JsonDeserialize<Record>(json, false);
                return record;
            }
            return null;
        }

        public List<Record> ReadAll()
        {
            List<Record> records = new();
            if (Directory.Exists(GetDirectory))
            {
                var files = Directory.GetFiles(GetDirectory);
                foreach (var file in files)
                {
                    string json = File.ReadAllText(file);
                    var record = Helper.JsonDeserialize<Record>(json, false);
                    if (record != null)
                        records.Add(record);
                }
            }
            return records;
        }
    }
}
