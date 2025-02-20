using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(IndicatorManager))]
public class IndicatorManagerEditor : Editor
{
    public Material indicatorMaterial;
    
    public override void OnInspectorGUI()
    {
        var targetIndicator = (IndicatorManager)target;
        
        base.OnInspectorGUI();

        if (GUILayout.Button("Test indicator values"))
        {
            indicatorMaterial.SetColor("_Color", targetIndicator.color);
            indicatorMaterial.SetFloat("_Max", targetIndicator.max);
            indicatorMaterial.SetFloat("_Min", targetIndicator.min);
            indicatorMaterial.SetFloat("_BorderWidth", targetIndicator.borderWidth);
            indicatorMaterial.SetFloat("_speed", targetIndicator.speed);      
        }

        if (GUILayout.Button("Stop indicator preview"))
        {
            indicatorMaterial.SetFloat("_speed", 0);    
            indicatorMaterial.SetFloat("_BorderWidth", 0);
        }
    }
}
