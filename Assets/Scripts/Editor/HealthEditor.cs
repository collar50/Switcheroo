using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Health))]
public class HealthEditor : Editor
{
    Health health;

    private void OnEnable()
    {
        health = (Health)target;
    }

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();
        



        health.tab = GUILayout.Toolbar(health.tab, new string[] { "Health", "Mana", "D-S" });

        switch (health.tab)
        {
            case 0:
                //displayStatInfo(health.getCurrentValue(), )
                break;

        }
    }

    private void displayStatInfo(int current, int max)
    {
        Rect inspectorRect = EditorGUILayout.BeginVertical();
        Rect progressRect = new Rect(new Vector2(inspectorRect.xMin, inspectorRect.yMin + 30), new Vector2(inspectorRect.width * .6f, 20));

        GUILayout.BeginHorizontal();
        EditorGUI.ProgressBar(progressRect, (float)current/max, "Current: " + current);
        //EditorGUI.LabelField(r2, "Max Health: " + health.MaxValue);
        GUILayout.EndHorizontal();
        GUILayout.Space(40);
        EditorGUILayout.EndVertical();


        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Zero"))
        {
            current = 0;
        }

        if (GUILayout.Button("Half"))
        {
            current = max / 2;
        }

        if (GUILayout.Button("Max"))
        {
            current = max;
        }
        GUILayout.EndHorizontal();
    }
}
