using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using UnityEngine.Advertisements;
using CH = util.CoroutineHelper;
namespace moving.player
{

	public class RecoveryPanel : MonoBehaviour
	{
		[SerializeField]
		Button recoverButton=null;
		Player player;
		
		public void Open(Player _player)
		{
			player = _player;

			if(player.MyPlayerData.OnceRecover)
			{
				recoverButton.interactable =false;
			}
			gameObject.SetActive(true);
			Time.timeScale = 0;
			
		}

		public void Recover()
		{
			player.SetInvisible(true);
			recoverButton.interactable = false;
			//---広告
		#if !UNITY_EDITOR
			Action<ShowResult> rewardAction = (result)=>
			{
				if(result == ShowResult.Finished)
				{
					player.MyPlayerData.HP = player.MyPlayerData.MaxHP;
					player.Dead = false;
					player.MyPlayerData.OnceRecover = true;
					//---演出
					player.animator.SetTrigger("Recover");
					SoundManager.LevelUp();
					//---閉じる
					Time.timeScale = 1;
					gameObject.SetActive(false);
					player.StartCoroutine(CH.DelaySecond(3.0f,()=>player.SetInvisible(false) ));
				}
			};
			ads.AdvertisementsManager.RewardShow(rewardAction);
		#endif
		#if UNITY_EDITOR
			player.MyPlayerData.HP = player.MyPlayerData.MaxHP;
			player.Dead = false;
			player.MyPlayerData.OnceRecover = true;
			//---演出
			player.animator.SetTrigger("Recover");
			SoundManager.LevelUp();
			//---閉じる
			gameObject.SetActive(false);
			player.StartCoroutine(CH.DelaySecond(3.0f,()=>player.SetInvisible(false)));
			Time.timeScale = 1;
		#endif
			

		}

		public void BackTitle()
		{
			Time.timeScale = 1;
			SceneManager.LoadScene("Title", LoadSceneMode.Single);
			gameObject.SetActive(false);

		}
	}

}