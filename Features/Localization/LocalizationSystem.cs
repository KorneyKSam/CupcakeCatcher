using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Localization
{
    public static class LocalizationSystem
    {
        public static Language CurrentLanguage
        {
            get => m_CurrentLanguage;
            set
            {
                if (m_CurrentLanguage != value)
                {
                    m_CurrentLanguage = value;
                    Refresh();
                }
            }
        }

        private const string KeyNotFoundMessage = "Localization key not found: ";

        private static readonly Dictionary<TextMeshProUGUI, string> m_LinkedTexts = new();

        private static Dictionary<string, string> m_LocalizationDictionary;
        private static Language m_CurrentLanguage;

        public static string GetLocalizedValue(string key)
        {
            if (m_LocalizationDictionary == null)
            {
                Refresh();
            }

            m_LocalizationDictionary.TryGetValue(key, out var value);

            if (string.IsNullOrWhiteSpace(value))
            {
                Debug.LogError(KeyNotFoundMessage + key);
            }
            return value;
        }

        public static void LinkText(TMP_Text text, string key) => LinkText(text.GetComponent<TextMeshProUGUI>(), key);

        public static void LinkText(TextMeshProUGUI textMeshPro, string key)
        {
            if (m_LinkedTexts.ContainsKey(textMeshPro))
            {
                m_LinkedTexts[textMeshPro] = key;
            }
            else
            {
                m_LinkedTexts.Add(textMeshPro, key);
            }

            textMeshPro.text = GetLocalizedValue(key);
        }

        private static void Refresh()
        {
            m_LocalizationDictionary = GetLocalizationDictionary();
            TryToUpdateTexts();
        }

        private static Dictionary<string, string> GetLocalizationDictionary() => CSVLoader.LoadDictionary(CurrentLanguage.ToString());

        private static void TryToUpdateTexts()
        {
            if (m_LinkedTexts.Count > 0)
            {
                foreach (var linkedText in m_LinkedTexts)
                {
                    linkedText.Key.text = GetLocalizedValue(linkedText.Value);
                }
            }
        }
    }
}