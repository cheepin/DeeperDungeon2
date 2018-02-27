using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace skill
{
	public class SkillSlotPanel : MonoBehaviour
	{

		[SerializeField]
		int column = 4;
		[SerializeField]
		int row = 4;
		[SerializeField]
		Vector2 ButtonOffset = new Vector2(-200, 50);
		[SerializeField]
		Vector2 ButtonInterval = new Vector2(-100, 100);
		void Start()
		{
			var emptySlotRes = Resources.Load("SkillTreeButton/EmptySlot_b");
			var activeSkills = SkillManager.GetCurrentActiveSkill();
			int i = 0;

			//配置用デリゲート
			Action<Vector2> foreachPos = (newPos) =>
			{
				GameObject newButton;
				if(i < activeSkills.Count)
				{
					var skillButtonRes = Resources.Load("SkillTreeButton/SkillButton");
					newButton = Instantiate(skillButtonRes, transform) as GameObject;
					newButton.GetComponent<SkillButton>().SetUpButton(SkillManager.GetSkillData<SkillData>(activeSkills[i]), false);

				}
				else
				{
					newButton = Instantiate(emptySlotRes, transform) as GameObject;
					newButton.transform.GetChild(0).gameObject.SetActive(false);
				}
			//---RectTransformを取得してアンカーとポジションを設置
				var rectTransform = newButton.GetComponent<RectTransform>();
				rectTransform.anchorMax = new Vector2(1.0f, 0);
				rectTransform.anchorMin = new Vector2(1.0f, 0);
				rectTransform.anchoredPosition = newPos;


				++i;
			};

			util.FillElementUI.FillColumns(ButtonOffset, ButtonInterval, column, row, foreachPos);
		}

	}



}