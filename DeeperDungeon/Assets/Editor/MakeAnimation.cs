using UnityEngine;
using System.Collections.Generic;
using System.Linq;

using UnityEditor;
using System.Reflection;



public class MakeAnimation : EditorWindow
{
	//UnityEngine.Object objSomething;
	UnityEngine.Object[]  objSomething = new UnityEngine.Object[3];
	string animName;
	string folderPath;



	[MenuItem ("Extension/MakeAnimation")]
	static void  Init () 
	{
		GetWindow(typeof (MakeAnimation),false,"MakeAnimation");
	}



	void OnGUI()
	{
		EditorGUILayout.Space();
		folderPath = EditorGUILayout.TextField("Asset/Animation/{ }/",folderPath,GUILayout.Height(20));
		EditorGUILayout.Space(); 
		animName = EditorGUILayout.TextField("FileName",animName,GUILayout.Height(20));
		EditorGUILayout.Space();
		if(GUILayout.Button("Paste Now Select"))
		{
			int i = 0;
			foreach(var item in Selection.objects)
				objSomething[i++] = item;
			
		}

		objSomething[0] = EditorGUILayout.ObjectField(objSomething[0],typeof(UnityEngine.Object),false,GUILayout.Height(20));
		objSomething[1] = EditorGUILayout.ObjectField(objSomething[1],typeof(UnityEngine.Object),false,GUILayout.Height(20));
		objSomething[2] = EditorGUILayout.ObjectField(objSomething[2],typeof(UnityEngine.Object),false,GUILayout.Height(20));
		if(GUILayout.Button("Create"))
			MakeAnimationCaller();
		
	}


    public void MakeAnimationCaller()
    {
		//---スプライトシートのパスを取得　ロード
		string SpriteFilePath =  AssetDatabase.GetAssetPath(objSomething[0].GetInstanceID());
		var mySprite = AssetDatabase.LoadAllAssetsAtPath(SpriteFilePath); 

		//---スプライトシートからobjSomethingと同じ名前を持つスプライトを取得、リスト化
		var _objlist = objSomething.ToList();
		var ss = mySprite.Where((X)=> _objlist.Find((Y)=>Y.name == X.name) !=null);
		var spriteList = ss.ToList();
	
		MakeAnime(spriteList,animName);
	}

    /* 
	 * int direction
	 * 0: front
	 * 1: left
	 * 2: right
	 * 3: back
	 */
    private void MakeAnime(List<Object> TargetSprites, string pathName)
    {
		AnimationClip NewClip = new AnimationClip()
		{
			wrapMode = WrapMode.Loop
			
		};
		Debug.Log(NewClip);

		SerializedObject serializedClip = new SerializedObject(NewClip);
        SerializedProperty settings = serializedClip.FindProperty("m_AnimationClipSettings");
		Debug.Log(settings);
        while (settings.Next(true))
        {
            if (settings.name == "m_LoopTime")
            {
                break;
            }
        }
        settings.boolValue = true;
        serializedClip.ApplyModifiedProperties();
		

        List<ObjectReferenceKeyframe> Keyframes = new List<ObjectReferenceKeyframe>();
        for (int i = 0; i < TargetSprites.Count; i++)
        {
			Keyframes.Add (new ObjectReferenceKeyframe()
			{
				time = 0.25F * i,
				value = TargetSprites[i]
			});
		}

		Keyframes.Add (new ObjectReferenceKeyframe()
			{
				time = 0.25F *  TargetSprites.Count,
				value = TargetSprites[TargetSprites.Count-1]
			});

        EditorCurveBinding curveBinding = new EditorCurveBinding();
        curveBinding.type = typeof(SpriteRenderer);
        curveBinding.path = string.Empty;
        curveBinding.propertyName = "m_Sprite";

        AnimationUtility.SetObjectReferenceCurve(NewClip, curveBinding, Keyframes.ToArray());
        AssetDatabase.CreateAsset(NewClip, $"Assets/Animation/{folderPath}/{pathName} .anim");


		objSomething = new UnityEngine.Object[3];
		animName = "";


    }
}