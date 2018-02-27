using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CurrentFloorDisplay : MonoBehaviour {

	// Use this for initialization
	Action lambda;

	void Start () {
		GetComponent<UnityEngine.UI.Text>().text = "Floor " + dungeon.DungeonManager.Instance.DungeonLevel.ToString();
		lambda = ()=>GetComponent<UnityEngine.UI.Text>().text = "Floor " + dungeon.DungeonManager.Instance.DungeonLevel.ToString();
		util.DontDestroyManager.SetFuncWhenNewLevelLoaded(lambda);
	}

	void OnDisable()
	{
		util.DontDestroyManager.RemoveFuncWhenNewLevelLoaded(lambda);
	}

	// Update is called once per frame
	void Update () {
		
	}
}
