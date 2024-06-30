using UnityEditor;
using UnityEngine;

namespace WheelOfLuck.Editor
{
    [CustomEditor(typeof(WheelItemsPack), true)]
    public class WheelItemsPackEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Reset Collected Items"))
            {
                WheelItemsPack itemsPack = target as WheelItemsPack;

                itemsPack?.Reset();
            }
        }
    }
}