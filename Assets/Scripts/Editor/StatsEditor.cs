using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

[CustomEditor(typeof(Stat))]
public class StatsEditor : Editor {

    Stat stats;

    private void OnEnable()
    {
        stats = (Stat) target;
    }

    public override void OnInspectorGUI()
    {
        stats.tab = GUILayout.Toolbar(stats.tab, new string[] { "Health", "Mana", "D-S" });
        //base.OnInspectorGUI();

        switch (stats.tab)
        {
            case 0:
                stats.mHealthDisplay = (Image)EditorGUILayout.ObjectField("Health Display: ", stats.mHealthDisplay, typeof(Image), true);
                displayStatInfo(stats.mCurrentHealth, stats.mMaxHealth);
                displayHealthButtons();
                break;
            case 1:
                stats.mManaDisplay = (Image)EditorGUILayout.ObjectField("Mana Display: ", stats.mManaDisplay, typeof(Image), true);
                displayStatInfo(stats.mCurrentMana, stats.mMaxMana);
                displayManaButtons();
                break;
            case 2:
                stats.mDSDisplay = (Image)EditorGUILayout.ObjectField("D-S Display: ", stats.mDSDisplay, typeof(Image), true);
                displayStatInfo(stats.mCurrentDS, stats.mMaxDS);
                displayDSButtons();
                break;
        }
    }

    private void displayStatInfo(int current, int max)
    {
        Rect inspectorRect = EditorGUILayout.BeginVertical();
        Rect progressRect = new Rect(new Vector2(inspectorRect.xMin, inspectorRect.yMin + 10), new Vector2(inspectorRect.width * .6f, 20));
        Rect labelRect = new Rect(new Vector2(inspectorRect.xMax - inspectorRect.width * .35f, inspectorRect.yMin + 10), new Vector2(inspectorRect.width * .3f, 20));

        
        EditorGUI.ProgressBar(progressRect, (float)current / max, "Current: " + current);
        EditorGUI.LabelField(labelRect, "Max: " + max);
        GUILayout.Space(40);
        EditorGUILayout.EndVertical();
    }

    private void displayHealthButtons()
    {
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Zero"))
        {
            stats.mCurrentHealth = 0;
        }

        if (GUILayout.Button("Half"))
        {
            stats.mCurrentHealth = stats.mMaxHealth / 2;
        }

        if (GUILayout.Button("Max"))
        {
            stats.mCurrentHealth = stats.mMaxHealth;
        }
        GUILayout.EndHorizontal();
    }

    private void displayManaButtons()
    {
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Zero"))
        {
            stats.mCurrentMana = 0;
        }

        if (GUILayout.Button("Half"))
        {
            stats.mCurrentMana = stats.mMaxMana / 2;
        }

        if (GUILayout.Button("Max"))
        {
            stats.mCurrentMana = stats.mMaxMana;
        }
        GUILayout.EndHorizontal();
    }

    private void displayDSButtons()
    {
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Zero"))
        {
            stats.mCurrentDS = 0;
        }

        if (GUILayout.Button("Half"))
        {
            stats.mCurrentDS = stats.mMaxDS / 2;
        }

        if (GUILayout.Button("Max"))
        {
            stats.mCurrentDS = stats.mMaxDS;
        }
        GUILayout.EndHorizontal();
    }
}
