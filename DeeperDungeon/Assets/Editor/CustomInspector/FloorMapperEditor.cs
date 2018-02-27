using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UI;


[CustomEditor(typeof(dungeon.FloorMapper))]
public class FloorMapperEditor : Editor
{
    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();

        dungeon.FloorMapper floorMapper = target as dungeon.FloorMapper;
        floorMapper.name = EditorGUILayout.TextField("Dungeon Name",floorMapper.name);
        floorMapper.NumberFloor = EditorGUILayout.IntField("Number Floor", floorMapper.NumberFloor);
        EditorUtility.SetDirty(floorMapper);

		//---可変リストを表示
        SerializedProperty boardCreators = serializedObject.FindProperty("boardMap");
        SerializedProperty itemMapper = serializedObject.FindProperty("itemMapper");
        SerializedProperty enemyMapper = serializedObject.FindProperty("enemyMapper");


        serializedObject.Update();
        EditorGUILayout.PropertyField(boardCreators, true);
        EditorGUILayout.PropertyField(itemMapper, true);
        EditorGUILayout.PropertyField(enemyMapper, true);


        if (EditorGUI.EndChangeCheck())
        {
            serializedObject.ApplyModifiedProperties();
        }
    }
}



