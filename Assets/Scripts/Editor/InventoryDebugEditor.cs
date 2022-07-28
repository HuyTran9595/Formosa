using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(InventoryDebug))]
public class InventoryDebugEditor : Editor
{
    InventoryDebug iDebug;
    SerializedProperty m_itemID_Property;
    SerializedProperty m_amount_Property;
    SerializedProperty m_Tier_Property;

    GUIContent m_Enum_Tier      = new GUIContent("Tier");
    GUIContent m_itemID_Content = new GUIContent("itemID");
    GUIContent m_amount_Content = new GUIContent("amount");
    GUIContent m_tier_Content   = new GUIContent("tier");

    private void OnEnable()
    {
        iDebug = target as InventoryDebug;
        m_itemID_Property   = serializedObject.FindProperty("itemID");
        m_amount_Property   = serializedObject.FindProperty("amount");
        m_Tier_Property     = serializedObject.FindProperty("tier");
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.LabelField("!!!  USE IT WITH CARE  !!!");
        EditorGUILayout.LabelField("!!  IT MIGHT BUG THE GAME  !!");
        EditorGUILayout.PropertyField(m_itemID_Property, m_itemID_Content);
        EditorGUILayout.PropertyField(m_Tier_Property,m_tier_Content);
        EditorGUILayout.PropertyField(m_amount_Property, m_amount_Content);

        if(GUILayout.Button("Add new Item"))
        {
            iDebug.AddItem();
        }
        serializedObject.ApplyModifiedProperties();
    }
}
