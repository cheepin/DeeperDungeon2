using System.Linq;
using System.IO;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.U2D;
using UnityEditor.Sprites;


namespace utilEditor
{
	public class MakeSpriteAtlas : EditorWindow {

		[MenuItem ("Extension/MakeSpriteAtlas")]
		static void  Init () 
		{
			GetWindow(typeof (MakeSpriteAtlas),false,"MakeOverride");
		}
		RuntimeAnimatorController controller;
		AnimationClip clip1;
		List<Texture2D> psdList;
		string fileName;
		string pathName;
		string targetPath;

		void OnGUI()
		{
			pathName = EditorGUILayout.TextField("Assets/Resources/{ }/",pathName,GUILayout.Height(20));
			fileName = EditorGUILayout.TextField("FileName",fileName,GUILayout.Height(20));
			targetPath = EditorGUILayout.TextField("TargetPass: Asset/{ }",targetPath,GUILayout.Height(20));

			EditorGUILayout.Space();
			if(GUILayout.Button("Create"))
			{
				psdList = utilEditor.Assets.GetAssetsFromDirectory<Texture2D>(targetPath,"psd");
				CreateAtlas();
			}
		}

		SpriteAtlas spriteAtlas;
		void CreateAtlas()
		{
			Texture2D texture2D = new Texture2D(1024,1024);
			texture2D.PackTextures(psdList.ToArray(),100);
			AssetDatabase.CreateAsset(texture2D,$"Assets/Resources/{pathName}/{fileName}");
		}

	}
}


