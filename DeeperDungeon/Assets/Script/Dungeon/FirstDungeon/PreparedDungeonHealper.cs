using System;
using System.Collections.Generic;
using UnityEngine;
using CH=util.CoroutineHelper;
using UnityEngine.SceneManagement;
using skill;
using UnityEngine.Advertisements;
namespace dungeon
{
	public class PreparedDungeonHealper : MonoBehaviour
	{
		public void SetUpFirstSkill(GameObject dummy)
		{
			if(DungeonManager.Instance.playerData==null)
			{
				SkillManager.SetSkillListFromEmblem("MaxHP");
				SkillManager.SetSkillListFromEmblem("MaxMana");
				SkillManager.SetSkillListFromEmblem("Stlength");
				SkillManager.SetSkillListFromEmblem("Defense");
				
			}
		}

		public void LoadDungeon(int floor)
		{
			DungeonManager dungeonManager = DungeonManager.Instance;
			bool success = false;
			string sceneName="";
			switch(floor)
			{
				case 8:
					sceneName = "FirstDungeon-Prepared1";
					success = true;
					break;
				case 15:
					sceneName = "FirstDungeon-15F";
					success = true;
					break;

				case 23:
					sceneName = "FirstDungeon-23F";
					success = true;
					break;
				case 30:
					sceneName = "FirstDungeon-30F";
					success = true;
					break;
				case 37:
					sceneName = "FirstDungeon-42F";
					success = true;
					break;
				case 50:
					sceneName = "FirstDungeon-50F";
					success = true;
					break;
				default:
					break;
			}
			if(success)
			{

				if (Advertisement.isSupported && floor!=50)
				{
					ads.AdvertisementsManager.Show();
				}
				dungeonManager.StartCoroutine(
					CH.WaitForEndOfFrame(()=>dungeonManager.StartCoroutine(
						CH.Chain(dungeonManager,CH.Do(()=>SceneManager.LoadScene(sceneName, LoadSceneMode.Single))))));

				dungeonManager.CangelFeedIn = true;
			}
			else
			{
				dungeonManager.CangelFeedIn= false;
			}
			//return success;
			
		}



	}
}