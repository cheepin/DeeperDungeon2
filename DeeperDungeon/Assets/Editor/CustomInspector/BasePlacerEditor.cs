using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using utilEditor;


namespace dungeon
{
	public class BasePlacerEditor : Editor
	{
		float allDropRate;
		public override void OnInspectorGUI()
		{
			EditorGUI.BeginChangeCheck();

			var placer = target as Placer;
			placer.DataPath = EditorGUILayout.TextField(placer.DataPath);
			if(GUILayout.Button("Set All ITems"))
			{
				SetAllItem(placer.DataPath);
			}
			EditorGUILayout.Space();

			allDropRate = EditorGUILayout.FloatField(allDropRate);
			if(GUILayout.Button("SetAllDropRate"))
			{
				SetAllDropRate(allDropRate);
			}


			var itemList = serializedObject.FindProperty("itemList");
			EditorGUILayout.PropertyField(itemList, true);
			if(EditorGUI.EndChangeCheck())
			{
				serializedObject.ApplyModifiedProperties();
				serializedObject.Update();

			}
		}

		void SetAllDropRate(float allDropRate)
		{
			var item = target as ItemPlacer;
			for(int i = 0; i < item.itemList.Count; i++)
			{
				var tempList = item.itemList[i];
				tempList.dropRate = allDropRate;
				item.itemList[i] = tempList;
			}
			EditorUtility.SetDirty(item);
		}


		void SetAllItem(string targetPath)
		{
			var item = target as Placer;
			var resource = utilEditor.Assets.GetAssetsFromDirectory<ScriptableObject>(targetPath, ".asset");
			item.itemList.Clear();
			for(int i = 0; i < resource.Count; i++)
			{
				DropRatePerList tempList = new DropRatePerList
				{
					item = resource[i],
					dropRate = 0
				};

				item.itemList.Add(tempList);
			}
			EditorUtility.SetDirty(item);

		}



	} 
}