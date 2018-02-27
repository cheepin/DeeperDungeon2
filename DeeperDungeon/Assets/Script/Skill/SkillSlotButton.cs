using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class SkillSlotButton : MonoBehaviour,IEndDragHandler
{


	public void OnEndDrag(PointerEventData eventData)
	{
		print("End");
		eventData.hovered.ForEach((x)=>{print(x);});
	}




	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
