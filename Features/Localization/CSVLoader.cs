using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Localization
{
    public static class CSVLoader
    {
        public const char Surround = '"';
        private const string ResourceFilePath = "Localization/Localization";

        public static readonly string[] LineSeparator = { "\r\n", "\r", "\n" };
        public static readonly string[] FieldSeparator = { "," };
        public static readonly Regex CSVParser = new(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");

        public static TextAsset LoadCSV() => Resources.Load<TextAsset>(ResourceFilePath);

        public static Dictionary<string, string> LoadDictionary(string languageID)
        {
            var dictionary = new Dictionary<string, string>();
            string[] lines = LoadCSV().text.Split(LineSeparator, System.StringSplitOptions.None);
            int attributeIndex = -1;
            string[] headers = lines[0].Split(FieldSeparator, System.StringSplitOptions.None);

            for (int i = 0; i < headers.Length; i++)
            {
                if (headers[i].Contains(languageID))
                {
                    attributeIndex = i;
                    break;
                }
            }

            for (int i = 1; i < lines.Length; i++)
            {
                string line = lines[i];
                string[] fields = CSVParser.Split(line);
                for (int j = 0; j < fields.Length; j++)
                {
                    fields[j] = fields[j].TrimStart(' ', Surround);
                    fields[j] = fields[j].TrimEnd(Surround);
                }


                if (fields.Length > attributeIndex)
                {
                    var key = fields[0];
                    if (dictionary.ContainsKey(key))
                    {
                        continue;
                    }
                    var value = fields[attributeIndex];
                    dictionary.Add(key, value);
                }

            }

            return dictionary;
        }
    }
}