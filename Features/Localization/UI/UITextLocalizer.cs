using System.Collections.Generic;
using UnityEngine;

namespace Localization
{
    [DisallowMultipleComponent]
    public class UITextLocalizer : MonoBehaviour
    {
        public List<UITextLocalization> UITextLocalizations;

        private void Start()
        {
            UITextLocalizations.ForEach(l => LocalizationSystem.LinkText(l.TextMeshPro, l.Key));
        }
    }
}