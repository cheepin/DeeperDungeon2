using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CH = util.CoroutineHelper;
namespace debugTool
{
	public class FPSMeter : MonoBehaviour
	{
		Text text;
		// Use this for initialization
		void Start()
		{
			text = GetComponent<Text>();
			StartCoroutine(CH.DelaySecondLoop(1.5f,-1,()=>
			{
				float fps = 1f/Time.deltaTime;
				text.text = fps.ToString(); 
			}));
		}

		// Update is called once per frame
		void Update()
		{
			
		}
	}

}