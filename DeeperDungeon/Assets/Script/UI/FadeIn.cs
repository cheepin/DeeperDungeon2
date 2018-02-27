using System;
using System.Collections;
using UnityEngine;
using CH = util.CoroutineHelper;
using UnityEngine.UI;


public class FadeIn : util.Singleton<FadeIn>{

	Image imageAlpha;
	[SerializeField]
	float fadeSpeed=0;
	[SerializeField]
	int NumberOfStage=0;
	void Start () {
		
	}

	static bool proceeding = false;
	static public IEnumerator StartFadeIn()
	{
		float fadeValueEachAtATime = 1.0f/Instance.NumberOfStage;
		GetImageCompIfNotExist();
		yield return WaitPreProceeding();
		yield return 
			Instance.StartCoroutine(
				CH.Chain(Instance,
				CH.DelaySecondLoop(Instance.fadeSpeed,Instance.NumberOfStage,()=>Instance.imageAlpha.color -= new Color(0,0,0,fadeValueEachAtATime)),
				CH.Do(()=> {proceeding = false;Instance.imageAlpha.enabled=false;})));
	}

	static public IEnumerator StartFadeOut()
	{
		Instance.imageAlpha.enabled=true;
		float fadeValueEachAtATime = 1.0f/Instance.NumberOfStage;
		GetImageCompIfNotExist();
		yield return WaitPreProceeding();

		yield return 
			Instance.StartCoroutine(
				CH.Chain(Instance,
				CH.DelaySecondLoop(Instance.fadeSpeed,Instance.NumberOfStage,()=>Instance.imageAlpha.color += new Color(0,0,0,fadeValueEachAtATime)),
				CH.Do(()=> { proceeding = false; })));
	}

	static	void GetImageCompIfNotExist()
	{
		if(Instance.imageAlpha==null)
		{
			Instance.imageAlpha = Instance.GetComponent<Image>();
			Instance.imageAlpha.color = new Color(0,0,0,1.0f);
		}
	}

	static IEnumerator WaitPreProceeding()
	{
		do
		{
			yield return new WaitForSeconds(0.1f);
		}while(proceeding);

		proceeding = true;
	}

}
