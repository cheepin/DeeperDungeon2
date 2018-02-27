using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using skill;
public class BackGameButton : MonoBehaviour,IPointerDownHandler {
	public void OnPointerDown(PointerEventData eventData)
	{
		SkillManager.BackGame();
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
