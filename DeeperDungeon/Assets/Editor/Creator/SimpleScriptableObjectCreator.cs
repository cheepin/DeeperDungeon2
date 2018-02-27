using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using util;
using System.Reflection;
using System;
using System.Linq;


namespace utilEditor
{
	[Serializable]
	public class SimpleScriptableObjectCreator : ScriptableObjectCreator {

		protected string baseTypeName;
		Action listener;
		UnityEngine.Object objSomething=null;
		ScriptableObject scriptableData;
		bool onceScaned = false;
		string fileName;
		string folderName;
		[MenuItem ("Extension/SimpleScriptableObjectCreator")]
		static void  Init () 
		{
			GetWindow(typeof (AllScriptableObjectCreator),false,"ScriptableObjectCreator");
		}

		void OnGUI()
		{
			if(onceScaned)
				GUI.enabled = false;
			objSomething = EditorGUILayout.ObjectField(objSomething,typeof(UnityEngine.Object),false,GUILayout.Height(20));
			if(objSomething != null )
			{
				//---インプットされたデータを元にインスタンスを生成
				scriptableData = CreateInstance(objSomething.name);

				//---ファイルネームエリア
				listener += ()=>fileName = EditorGUILayout.TextField("FileName",fileName);

				//---ベースクラス名を取得（ソートに使う）
				baseTypeName = scriptableData.GetType().BaseType.ToString();

				//---生成されるフォルダを指定
				listener += ()=> folderName = EditorGUILayout.TextField("FolderName",folderName);

				//---このインプットフィールドを使用禁止へ
				objSomething = null;
				onceScaned = true;
			}
			GUILayout.Space(10);
			GUI.enabled = true;
			//---登録されたリスナーデリゲートを元にインプットフィールドを生成
	
			if(!onceScaned)
				GUI.enabled = false;
			if(GUILayout.Button("Create"))
			{
				CreateAsset(scriptableData,fileName,folderName);

				//---項目をリセット
				onceScaned = false;
				listener = null;
			}
		}

		
		
	}
}

