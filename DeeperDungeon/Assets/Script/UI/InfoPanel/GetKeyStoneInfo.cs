using UnityEngine;
using System.Collections;
using System;
namespace ui
{
	public class GetKeyStoneInfo : BaseInfoPanel
	{
		protected override string PrefID
		{
			get
			{
				return "GetKeyStoneInfo";
			}
		}
		protected override bool ConditionToActivate(string skillName)
		{
			if(skillName!="Key")
				return true;
			else
			{
				return false;
			}
		}

		protected override void RergisterAction(Action<string> activate)
		{
			skill.SkillManager.Instance.ActionWhenSetKeyStoneEmblem += activate;
		}

		public void CloseButtonClicked()
		{
			gameObject.SetActive(false);
			Time.timeScale = 1;
		}

	}

}