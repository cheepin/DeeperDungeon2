using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreParedDungeonManager : MonoBehaviour {

	[SerializeField]
	Vector2 startPos = new Vector2();
	[SerializeField]
	bool debugMode;
	// Use this for initialization
	void Start () {
		if(!debugMode)
		{
			var player = GameObject.FindGameObjectWithTag("Player");
			player.transform.position = startPos;
			var playerComp = player.GetComponent<moving.player.Player>();
			playerComp.Moving(0,0);
		}
		else
		{
			var player = GameObject.FindGameObjectWithTag("Player");
			player.transform.position = startPos;
			var playerComp = player.GetComponent<moving.player.MockPlayer>();
			playerComp.Moving(0,0);

		}
		StartCoroutine(FadeIn.StartFadeIn());
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
