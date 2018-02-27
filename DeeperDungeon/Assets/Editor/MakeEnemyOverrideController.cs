using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace utilEditor
{
	public class MakeEnemyOverrideController : EditorWindow {

		[MenuItem ("Extension/MakeOverrideConroller")]
		static void  Init () 
		{
			GetWindow(typeof (MakeEnemyOverrideController),false,"MakeOverride");
		}
		RuntimeAnimatorController controller;
		AnimationClip clip1;
		string fileName;
		string pathName;


		void OnGUI()
		{
			pathName = EditorGUILayout.TextField("Asset/Animation/{ }/",pathName,GUILayout.Height(20));
			fileName = EditorGUILayout.TextField("FileName",fileName,GUILayout.Height(20));
			controller = EditorGUILayout.ObjectField("BaseController",controller,typeof(UnityEngine.Object),false,GUILayout.Height(20)) as RuntimeAnimatorController;
			EditorGUILayout.Space();
			if(GUILayout.Button("Create"))
			{
				CreateOverride();
			}
		}

		void CreateOverride()
		{
			//---元のコントローラーのコピーを作成、新オーバーライドコントローラーにアサイン
			var copyController = Instantiate(controller); 
			AnimatorOverrideController newController = new AnimatorOverrideController
			{
				runtimeAnimatorController = controller
			};

			var copyControllerClipList = copyController.animationClips.Select((X,I)=>new {clip=X,myName =X.name});
			Dictionary<string,AnimationClip> selectionControllerClipList = Selection.objects.Cast<AnimationClip>().ToDictionary((X)=>X.name);
			List<AnimationClip> assienClip = copyControllerClipList.Select((X)=>selectionControllerClipList[X.myName]).ToList();
			var c = newController.animationClips.Zip(assienClip,(first,second)=> new  KeyValuePair<AnimationClip,AnimationClip>(first,second)).ToList();

			newController.ApplyOverrides(c);


			AssetDatabase.CreateAsset(newController,$"Assets/Animation/{pathName}/{fileName}.OverrideController");
		}

	}
}