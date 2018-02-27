using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SelectedSceneButton : MonoBehaviour {

	[SerializeField]
	string sceneName;

	// Use this for initialization
	void Start () {
		
	}
	
	public void LoadScene()
	{
		SaveDataManager.SetLoadFlag(false);
		SoundManager.Push();
		SceneManager.LoadScene(sceneName);
	}
}
