using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CoinPileManager))]
public class CoinPileManagerEditor : Editor
{
    CoinPileManager cpm;
    SerializedProperty m_enum_value;
    SerializedProperty m_Sequence;
    SerializedProperty m_1_min;
    SerializedProperty m_1_max;

    GUIContent m_Enum_Content = new GUIContent("Algorithm");
    GUIContent m_Sequence_Content = new GUIContent("Sequence");
    GUIContent m_Min_Content = new GUIContent("Min");
    GUIContent m_Max_Content = new GUIContent("Max");

    private void OnEnable()
    {
        cpm = target as CoinPileManager;
        m_enum_value = serializedObject.FindProperty("Algorithm");
        m_Sequence = serializedObject.FindProperty("sequence");
        m_1_min = serializedObject.FindProperty("min");
        m_1_max = serializedObject.FindProperty("max");

    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(m_enum_value, m_Enum_Content);
        switch (cpm.Algorithm)
        {
            case CoinPileManager.algo._1_RandomNumber:
                {
                    EditorGUILayout.PropertyField(m_1_min, m_Min_Content, GUILayout.MinWidth(EditorGUIUtility.currentViewWidth * 0.5f), GUILayout.ExpandWidth(true));
                    EditorGUILayout.PropertyField(m_1_max, m_Max_Content, GUILayout.MinWidth(EditorGUIUtility.currentViewWidth * 0.5f), GUILayout.ExpandWidth(true));
                    break;
                }
            case CoinPileManager.algo._2_Sequence:
                {
                    EditorGUILayout.PropertyField(m_Sequence, m_Sequence_Content);
                }
                break;
            default:
                break;
        }

        if(GUILayout.Button("Reset All Piles"))
        {
            cpm.ResetAllPiles();
        }
        serializedObject.ApplyModifiedProperties();
    }
}
