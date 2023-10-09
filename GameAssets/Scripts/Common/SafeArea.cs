using UnityEngine;

namespace Common
{
    public class SafeArea
    {
        private readonly Vector2 m_AnchorMin;
        private readonly Vector2 m_AnchorMax;

        public SafeArea()
        {
            var safeArea = Screen.safeArea;

            m_AnchorMin = safeArea.position;
            m_AnchorMax = safeArea.position + safeArea.size;

            DevideVectorByScreenSize(ref m_AnchorMin);
            DevideVectorByScreenSize(ref m_AnchorMax);
        }

        public void ScaleRectTransform(RectTransform rectTransform)
        {
            rectTransform.anchorMin = m_AnchorMin;
            rectTransform.anchorMax = m_AnchorMax;
        }

        private void DevideVectorByScreenSize(ref Vector2 vector2)
        {
            vector2.x /= Screen.width;
            vector2.y /= Screen.height;
        }
    }
}