using System.IO;
using UnityEditor;
using UnityEngine;

namespace Common
{
    public static class EditorTools
    {
#if UNITY_EDITOR
        [MenuItem("Editor Tools/Clear PersistentDataPath")]
        private static void ClearPersistentDataPath()
        {
            var info = new DirectoryInfo(Application.persistentDataPath);
            foreach (var file in info.GetFiles())
            {
                file.Delete();
            }
        }

        [MenuItem("Editor Tools/Open PersistentDataPath")]
        private static void OpenPersistentDataPath()
        {
            EditorUtility.RevealInFinder(Application.persistentDataPath);
        }

        [MenuItem("Editor Tools/UI/Anchors to Corners %,")]
        private static void AnchorsToCorners()
        {
            if (Selection.activeTransform is RectTransform t &&
                Selection.activeTransform.parent is RectTransform p)
            {
                t.anchorMin = new(t.anchorMin.x + t.offsetMin.x / p.rect.width,
                                  t.anchorMin.y + t.offsetMin.y / p.rect.height);

                t.anchorMax = new(t.anchorMax.x + t.offsetMax.x / p.rect.width,
                                  t.anchorMax.y + t.offsetMax.y / p.rect.height);

                t.offsetMin = t.offsetMax = new(0, 0);
            }
        }

        [MenuItem("Editor Tools/UI/Corners to Anchors %.")]
        private static void CornersToAnchors()
        {
            if (Selection.activeTransform is RectTransform t)
            {
                t.offsetMin = t.offsetMax = new Vector2(0, 0);
            }
        }
#endif
    }
}