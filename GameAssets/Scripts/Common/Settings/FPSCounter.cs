using System.Collections;
using UnityEngine;

namespace Settings
{
    public class FPSCounter : MonoBehaviour
    {
        private const string TextPrefix = "FPS: ";
        private const int FontSize = 36;

        private Rect m_Location = new(25, 25, 170, 60);

        private float m_FPSCount;

        private IEnumerator ShowFPS()
        {
            GUI.depth = 2;
            var waitForSeconds = new WaitForSeconds(0.1f);
            while (true)
            {
                m_FPSCount = 1f / Time.unscaledDeltaTime;
                yield return waitForSeconds;
            }
        }

        private void OnEnable() => StartCoroutine(ShowFPS());
        private void OnDisable() => StopAllCoroutines();

        private void OnGUI()
        {
            Texture texture = Texture2D.linearGrayTexture;
            GUI.DrawTexture(m_Location, texture, ScaleMode.StretchToFill, true);
            GUI.color = Color.black;
            GUI.skin.label.fontSize = FontSize;
            GUI.Label(m_Location, $"{TextPrefix}{Mathf.Round(m_FPSCount)}");
        }
    }
}