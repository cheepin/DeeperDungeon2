using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using util;
using UnityEngine.Advertisements;
namespace ads
{

	public class AdvertisementsManager : Singleton<AdvertisementsManager>
	{

		// Use this for initialization
		void Start()
		{
#if !UNITY_EDITOR
			 if (Advertisement.isSupported)
			{
				Advertisement.Initialize("1553912",false);
			 }
#endif


			DontDestroyOnLoad(gameObject);
		}
		static public void Show()
		{
			#if !UNITY_EDITOR
			if (Advertisement.isSupported)
			{
				if(DecisionWhetherShow())
				{
					if(Advertisement.IsReady("video"))
					{
						Advertisement.Show();
					}
				}
				else
				{
						print("cancel advertizement!");
				}
			}
			#endif
			
			
		}

		static public void RewardShow(Action<UnityEngine.Advertisements.ShowResult> resultCallback)
		{
			ShowOptions showOptions = new ShowOptions();
			showOptions.resultCallback +=resultCallback;
			if(DecisionWhetherShow())
				Advertisement.Show("rewardedVideo",showOptions);
			else
			{
				resultCallback.Invoke(ShowResult.Finished);
			}
			
			 
		}

		//---広告課金をしていたら広告を非表示(false)
		static bool DecisionWhetherShow()
		{
			//print(PlayerPrefs.GetInt(iap.PurchaseRemovingAds.id_removeAds));
			//if(PlayerPrefs.GetInt(iap.PurchaseRemovingAds.id_removeAds) == iap.PurchaseRemovingAds.varidID)
			//{
			//	return false;
			//}
			//else
				return true;
		}

	}

}