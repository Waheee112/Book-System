using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Book_System.Classes
{
    public class JsonService
    {
        private readonly JsonSerializerOptions _options = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        // --- Serialization / Deserialization ---

        public string Convert<T>(List<T> data)
        {
            return JsonSerializer.Serialize(data, _options);
        }

        public List<T> ConvertBack<T>(string filedata)
        {
            if (string.IsNullOrWhiteSpace(filedata)) return new List<T>();
            return JsonSerializer.Deserialize<List<T>>(filedata) ?? new List<T>();
        }

        // --- File I/O Operations (القراءة والحفظ المباشر من الملفات) ---

        // دالة عامة لقراءة أي قائمة من ملف JSON
        public List<T> LoadFromFile<T>(string filePath)
        {
            if (!File.Exists(filePath))
                return new List<T>();

            string jsonContent = File.ReadAllText(filePath);
            return ConvertBack<T>(jsonContent);
        }

        // دالة عامة لحفظ أي قائمة في ملف JSON
        public void SaveToFile<T>(string filePath, List<T> data)
        {
            // التأكد من وجود المجلد
            string? directory = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            string jsonContent = Convert(data);
            File.WriteAllText(filePath, jsonContent);
        }
    }
}