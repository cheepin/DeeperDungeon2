using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//[CustomEditor(typeof(SkillButton))]
//public class SkillButtonInputForm : Editor
//{

//	public override void OnInspectorGUI()
//	{
//		EditorGUI.BeginChangeCheck();
//		//SerializedProperty prop = serializedObject.FindProperty("data");

//		var buttonRaw = target as SkillButton;

//		buttonRaw.data.skillName = EditorGUILayout.TextField("Skill Name", buttonRaw.data.skillName);
//		buttonRaw.data.maxLevel = EditorGUILayout.IntField("MaxLevel", buttonRaw.data.maxLevel);
//		buttonRaw.data.cost = EditorGUILayout.IntField("cost", buttonRaw.data.cost);
//		buttonRaw.data.activeSkill = EditorGUILayout.Toggle("ActiveSkill",buttonRaw.data.activeSkill);
//		buttonRaw.data.spellCost = EditorGUILayout.IntField("SpellCost", buttonRaw.data.spellCost);

//		EditorGUILayout.LabelField("Description");

//		buttonRaw.data.skillDescription = EditorGUILayout.TextArea(buttonRaw.data.skillDescription,GUILayout.Height(80));
//		EditorUtility.SetDirty(target);

//	}
//}
