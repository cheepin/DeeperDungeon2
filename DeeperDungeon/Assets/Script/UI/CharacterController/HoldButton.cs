using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class HoldButton : MonoBehaviour,IPointerDownHandler,IPointerUpHandler{

    protected Action clickFunc,holdFunc;
    protected bool pressed;
	protected bool doHoldFuncBeforeUp=false;
    protected int chargeTime,chargeCount;
    protected Button button;
	private bool doingHoldFuncBeforeUp =false;
    // Use this for initialization
    protected virtual void Start()
    {
        Debug.Assert(clickFunc != null);//|| holdFunc != null);
        pressed = false;
        button = GetComponent<Button>();
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if(button.IsInteractable())
            pressed = true;

    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {
        if (pressed && !doHoldFuncBeforeUp)
        {
            if (chargeCount > chargeTime)
                holdFunc();
            else
                clickFunc();
        }
        chargeCount = 0;
        pressed = false;
		doingHoldFuncBeforeUp = false;
    }

    void Update()
    {
        if(pressed)
		{
            chargeCount++;
			if(doHoldFuncBeforeUp && chargeCount > chargeTime &&!doingHoldFuncBeforeUp)
			{
				holdFunc();
				chargeCount = 0;
				doingHoldFuncBeforeUp=true;
			}
		}
    }
}
