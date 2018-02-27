using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace utilEditor
{
	//---DataPackerを作るためのカスタムインスペクター
	//---TはPacker本体　DataTypeは名前とデータ本体からなる構造体を指定する
	//---TはIPackerインターフェースを実装している必要がある
	public class DataPackerEditor<T,DataType> : Editor where T:UnityEngine.Object,utilEditor.IPacker<DataType>
	{
		public override void OnInspectorGUI()
		{
			EditorGUI.BeginChangeCheck();

			var emblemTexturePacker = target as T;
			EditorGUILayout.LabelField("FolderPath : (ex)BuyedAssets/Enemy/");
			emblemTexturePacker.TargetPath = EditorGUILayout.TextField("EmblemPath",emblemTexturePacker.TargetPath);

			if(GUILayout.Button("SetTexture"))
			{
				Debug.Log("Create!");
				CreateSetting(emblemTexturePacker);
				EditorUtility.SetDirty(emblemTexturePacker);
			}

			//---リスト表示

			SerializedProperty textureList = serializedObject.FindProperty("dataList");
			serializedObject.Update();
			EditorGUILayout.PropertyField(textureList, true);

			if (EditorGUI.EndChangeCheck())
			{
				serializedObject.ApplyModifiedProperties();
			}
		}

		protected virtual void CreateSetting(T packer)
		{
		}

	}

	//---DataPackerEditorで扱うMonobehaviorは必ずこれを実装している必要がある
	//---Tは名前とデータ本体を定義するための構造体
	//---TargetPathはPackするためのフォルダのパス

}