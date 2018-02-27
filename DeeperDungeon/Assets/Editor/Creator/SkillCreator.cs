using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace skill
{
	public class SkillCreator : EditorWindow
	{

		 [MenuItem ("Extension/SkillCreator")]
		static void  Init () 
		{
			GetWindow (typeof (SkillCreator),false,"SkillCreator");
		}

		public skill.SkillData skillData;
		string skillName;
		int learningCost;
		int spellCost;
		int maxLevel;
		bool activeSkill;
		string skillDescription;

		void  OnGUI () 
		{
			GUILayout.BeginVertical();

			GUILayout.Label ("Inventory Item Editor", EditorStyles.boldLabel);
			EditorGUILayout.Space();
			skillName = EditorGUILayout.TextField("SkillName",skillName);
			learningCost = EditorGUILayout.IntField("LearningCost",learningCost);
			spellCost = EditorGUILayout.IntField("spellCost",spellCost);
			maxLevel = EditorGUILayout.IntField("maxLevel",maxLevel);
			activeSkill =  EditorGUILayout.Toggle("ActiveSkill",activeSkill);
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Description");
			skillDescription = EditorGUILayout.TextArea(skillDescription,GUILayout.Height(30));


			if(GUILayout.Button("Create New Skill"))
			{
				CreateAsset(skillName);
				Debug.Log($"skillName:{skillName}が生成されました。スキルマネージャーに登録するのを忘れないでください");
			}


		}



		public void CreateAsset(string dataname) 
		{
			skill.SkillData asset = ScriptableObject.CreateInstance<skill.SkillData> ();
 
			string path = AssetDatabase.GetAssetPath (Selection.activeObject);
			if (path == "") 
			{
				path = "Assets";
			} 
			else if (Path.GetExtension (path) != "") 
			{
				path = path.Replace (Path.GetFileName (AssetDatabase.GetAssetPath (Selection.activeObject)), "");
			}
 
			string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath ("Assets/Data/Skill/" + dataname + ".asset");
			asset.learningCost = learningCost;
			asset.spellCost = spellCost;
			asset.maxLevel = maxLevel;
			asset.skillName = dataname;
			asset.activeSkill = activeSkill;
			asset.description = skillDescription;
			AssetDatabase.CreateAsset (asset, assetPathAndName);

			var emblemData = CreateInstance<item.EmblemData>();
			emblemData.skillName = skillName;
			emblemData.price = 0;
			emblemData.rarity = 0;
			AssetDatabase.CreateAsset(emblemData,$"Assets/Data/Item/{dataname}.asset");
			
				AssetDatabase.SaveAssets ();
        		AssetDatabase.Refresh();
			EditorUtility.FocusProjectWindow ();
			Selection.activeObject = asset;
		}
	}

}