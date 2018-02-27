using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DisableWhenSceneUnLoaded : MonoBehaviour {

	// Use this for initialization
	void Awake ()
	{
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnDestroy()
	{
		gameObject.SetActive(false);
	}
}
