using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using UnityEngine;
using UnityEditor;
using utilEditor;
using System.IO;
namespace item
{
	[Serializable]
	public class EmblemCreator : ScriptableObjectCreator 
	{
		EmblemData emblemData;

		 [MenuItem ("Extension/EmblemCreator")]
		static void  Init () 
		{
			
			GetWindow(typeof (EmblemCreator),false,"EmbCreator");
		}

		string skillName="";
		int price;
		int rarity;
		skill.SkillData skillData;
		private void OnEnable()
		{
			skillData = new skill.SkillData ();
		}

		void OnGUI()
		{
			

			EditorGUILayout.Space();
			skillData = EditorGUILayout.ObjectField("Label:",skillData,typeof(skill.SkillData),false) as skill.SkillData;
			skillName = skillData.skillName;
			EditorGUILayout.Space();
			price = EditorGUILayout.IntField("price",price);	
			EditorGUILayout.Space();
			rarity = EditorGUILayout.IntField("rarity",rarity);	

			if(GUILayout.Button("Create Emblem"))
			{
				emblemData = CreateInstance<EmblemData>();
				emblemData.skillName = skillName;
				emblemData.price = price;
				emblemData.rarity = rarity;

				CreateAsset(emblemData,emblemData.skillName,"Item");
			}
			
		}
	}

}

namespace utilEditor
{
	public class ScriptableObjectCreator : EditorWindow
	{
		public static void CreateAsset<T> (T asset,string name,string directory) where T : ScriptableObject
		{
 
			string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath ($"Assets/Data/{directory}/" + name + ".asset");
 
			AssetDatabase.CreateAsset (asset, assetPathAndName);
			AssetDatabase.SaveAssets ();
       		AssetDatabase.Refresh();
		}

		//---入力されたクラスをスキャンしてフィールド名を取得する
		static public List<FieldInfo> ScanType(ScriptableObject data,string baseTypeName)
		{
			//---タイプ名とフィールド名を取得
			Type t = data.GetType();
			var fieldList = t.GetFields();
			//---ベースクラスのフィールドからインプットフィールドが生成されるようにソート
			var orderedFieldList = fieldList.OrderByDescending(X=>X.DeclaringType.ToString()==baseTypeName).ToList();
			return orderedFieldList;
		}

		//---クラスのフィールドからインプットフィールドを生成
		//orederedFieldList:フィールドのリスト
		//thisClass:自分のクラス
		//listener:EditorGuiLayoutメソッドを格納するためのリスナー
		static public Action CreateInputField(List<FieldInfo> orderedFieldList,ScriptableObject thisClass,Action listener)
		{
			foreach(var field in orderedFieldList)
			{
				switch(field.FieldType.Name)
				{
					case "String":
						listener += CreateTextFieldFromType(field,thisClass);
						break;
					case "Int32":
						listener += CreateInputFieldFromType(field,thisClass);
						break;
					case "Single":
						listener += CreateFloatFieldFromType(field,thisClass);
						break;
				}
			}
			return listener;
		}
		//---CreateInputFieldで使用される
		//---単体ではあまり使う気なし
		static public Action CreateInputFieldFromType(FieldInfo data,ScriptableObject thisClass)
		{
			return () => data.SetValue(thisClass,EditorGUILayout.IntField(data.Name,(int)data.GetValue(thisClass)) );
		}
		static public Action CreateTextFieldFromType(FieldInfo data,ScriptableObject thisClass)
		{
			return () => data.SetValue(thisClass,EditorGUILayout.TextField(data.Name,(string)data.GetValue(thisClass)) );
		}
		static public Action CreateFloatFieldFromType(FieldInfo data,ScriptableObject thisClass)
		{
			return () => data.SetValue(thisClass,EditorGUILayout.FloatField(data.Name,(float)data.GetValue(thisClass)) );
		}
	}


	static public class	Assets
	{
		//---指定したフォルダを元にアセットリストを取得する
		//---folderPath: (ex) "Resource/testFolder/"
		//---extension:(ex) ".psd"
		static public List<T> GetAssetsFromDirectory<T>(string folderPath,string extension) where T:UnityEngine.Object
		{
			var objList = Directory.GetFiles($"{Application.dataPath}/{folderPath}")
					.Where((X)=>X.Contains(extension))
					.Where((X)=>!X.Contains(".meta"))
					.Select((X)=>X.Substring(Application.dataPath.Length-6));
			List<T> pdbList = new List<T>();
			foreach(var item in objList)
			{
				pdbList.Add(AssetDatabase.LoadAssetAtPath<T>(item));
			} 
			return pdbList;

		}

	}

}
