//using System.Collections.Generic;
//using UnityEngine;
//using UnityEditor;
//using util;
//using System.Reflection;
//using System;
//using System.Linq;

//namespace enemy
//{
//	[Serializable]
//	public class EnemyCreator : ScriptableObjectCreator {

//		public ScriptableObject scriptableData;
//		public SpriteManData spriteMan;
//		protected string baseTypeName;
//		Action listener;
//		Rect listArea = new Rect(0,35,120,125);
//		UnityEngine.Object objSomething=null;
//		UnityEngine.Object obj2=null;

//		bool onceScaned = false;
//		string[] datalist = {"EnemyData","SpriteManData"};
//		string fileName;
//		string searchPath="";

//		[MenuItem ("Extension/EnemyCreator")]
//		static void  Init () 
//		{
//			GetWindow(typeof (EnemyCreator),false,"EnemyCreator");
//		}

//		private void OnEnable()
//		{
//			scriptableData = CreateInstance<SpriteManData>();
//			listArea = new Rect(0,35,120,125);
//		}

//		void OnGUI()
//		{
//			if(onceScaned)
//				GUI.enabled = false;
//			objSomething = EditorGUILayout.ObjectField(objSomething,typeof(UnityEngine.Object),false,GUILayout.Height(20));
//			if(objSomething != null )
//			{
//				//---インプットされたデータを元にインスタンスを生成
//				scriptableData = CreateInstance(objSomething.name);
//				//---ファイルネームエリア
//				listener += ()=>fileName = EditorGUILayout.TextField("FileName",fileName);
//				//---ベースクラス名を取得（ソートに使う）
//				baseTypeName = scriptableData.GetType().BaseType.ToString();
//				Debug.Log(baseTypeName);
//				//---ScriptableObjectをスキャンしてインプットフィールドを生成
//				var infolist = ScanType(scriptableData,baseTypeName);
//				listener = CreateInputField(infolist,scriptableData,listener);	
//				//---このインプットフィールドを使用禁止へ
//				objSomething = null;
//				onceScaned = true;
//			}
//			GUILayout.Space(10);
//			GUI.enabled = true;
//			//---登録されたリスナーデリゲートを元にインプットフィールドを生成
//			listener?.Invoke();
	
//			if(!onceScaned)
//				GUI.enabled = false;
//			if(GUILayout.Button("Create"))
//			{
//				CreateAsset(scriptableData,fileName,"Enemy");
//			}
//		}

		
		
//	}
//}

