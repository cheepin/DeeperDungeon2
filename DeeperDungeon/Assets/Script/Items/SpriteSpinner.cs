using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSpinner : MonoBehaviour {

	// Use this for initialization
	void Start () {
		int rotate = 0;
		StartCoroutine(util.CoroutineHelper.DelaySecondLoop(0.05f,-1,()=>{transform.rotation = Quaternion.Euler(0,rotate+=5,0);}));
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
