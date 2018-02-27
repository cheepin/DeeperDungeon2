using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using moving.player;
public class MockAnalogStick : MonoBehaviour {

	MockPlayer player;
	// Use this for initialization
	void Start () {
		player = GameObject.FindWithTag("Player").GetComponent<MockPlayer>();
	}
	
	// Update is called once per frame
	 void Update () {

        float x = Mathf.Clamp(CrossPlatformInputManager.GetAxis("Horizontal")*1,-1,1);
        float y = Mathf.Clamp(CrossPlatformInputManager.GetAxis("Vertical")*1,-1,1);
        if (Input.GetKey("w")) y = 1;
        if (Input.GetKey("a")) x = -1;
        if (Input.GetKey("s")) y = -1;
        if (Input.GetKey("d")) x=  1;
        //if (x > 0) x = 1;
        //if (y > 0) y = 1;
		player.Moving(x,y);
    }
}
