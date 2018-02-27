using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using moving.player;
using skill;
using System;

namespace ui
{

	public class ActiveSkillSetInfo : BaseInfoPanel
	{
		protected override string PrefID
		{
			get
			{
				return "id_haveActiveSkillSetInfo";
			}
		}
		protected override bool ConditionToActivate(string skillName)
		{
			return SkillManager.GetCurrentActiveSkill().Count>0;
		}

		protected override void RergisterAction(Action<string> activate)
		{
			Action _activate = () => activate("dummy");
			dungeon.DungeonManager.Instance.ActionWhenSkillTreeEvent += _activate;
		}
		
		public void ClosedButtonClicked()
		{
			gameObject.SetActive(false);
			Time.timeScale = 1;
		}

	}

}