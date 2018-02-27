using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CH = util.CoroutineHelper;

namespace score
{
	public class FadeWhite : MonoBehaviour
	{
		private void OnEnable()
		{
			Color fadeColor = new Color(0,0,0,0.01f);
			var image = GetComponent<Image>();		
			StartCoroutine(CH.DelaySecondLoop(0.01f,100,()=>image.color+=fadeColor));
		}

	}

}