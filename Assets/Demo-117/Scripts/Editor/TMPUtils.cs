using UnityEditor;
using UnityEngine;

namespace Demo_117.Editor
{
    public static class TMPUtils
    {
        [InitializeOnLoadMethod]
        public static void SetTMP_Settings()
        {
            var settings = AssetDatabase.LoadAssetAtPath<TMPro.TMP_Settings>("Assets/TextMesh Pro/TMP Settings.asset");
            if (settings == null)
            {
                Debug.LogError("TMP Settings not found");
                return;
            }

            var field = typeof(TMPro.TMP_Settings).GetField("s_Instance",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            if (field == null)
            {
                Debug.LogError("s_Instance field not found");
                return;
            }

            field.SetValue(null, settings);
        }
    }
}