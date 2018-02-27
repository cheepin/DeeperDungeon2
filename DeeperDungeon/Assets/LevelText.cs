using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using System;

public class LevelText : MonoBehaviour {

	// Use this for initialization
	Action lambda;
	Text text;
	void Start () {

		text = GetComponent<Text>();
		StartCoroutine(util.CoroutineHelper.WaitForEndOfFrame(()=>
		{
			var player = GameObject.Find("Martz").GetComponent<moving.player.Player>();
			SetText(player.MyPlayerData.Level);
			player.MyPlayerData.SetActionWhenParamUpdate("LevelUp",(level)=>SetText(level) ,nameof(SetText),false);
			
		}));
	}

	void SetText(int level)  =>  text.text = $"Level {++level}";
	


	// Update is called once per frame
	void Update () {
		
	}
}
