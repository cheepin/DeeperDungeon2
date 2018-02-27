using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using moving.player;
using skill;
using System;
using CH = util.CoroutineHelper;
namespace ui
{
	public class SkillGemInfo : BaseInfoPanel
	{
		protected override string PrefID
		{
			get
			{
				return "id_SkillGemInfo";
			}
		}

		protected override bool ConditionToActivate(string str) => true;
		
		protected override void RergisterAction(Action<string> activate)
		{
			Action<int> carryedActivate = (dummy) =>activate("te"); 
			//---myPlayerDataが初期化されてから実行されるように遅延
			player.StartCoroutine(CH.DelaySecond(0.5f,()=> player.MyPlayerData.SetActionWhenParamUpdate("LevelUp",carryedActivate,"Activate",false)));
		}

		public void CloseButtonClicked()
		{
			gameObject.SetActive(false);
			Time.timeScale = 1;
		}

	} 
}