using UnityEditor;
using UnityEngine;
using UnityEngine.UI;


[CustomEditor(typeof(FrustrationManager))]
public class FrustrationManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        FrustrationManager frustrationManager = (FrustrationManager)target;
        GUILayout.Label($"Time to fill from empty : {100f / frustrationManager._frustrationPerSeconds}");
    }
}
