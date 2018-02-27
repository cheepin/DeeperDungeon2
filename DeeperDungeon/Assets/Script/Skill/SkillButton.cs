using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;
using UnityEngine.EventSystems;
using moving.player;


namespace skill
{
	public class SkillButton : MonoBehaviour,IPointerDownHandler{


		private int levelCount;
		bool inSkillSlot = true;
		public skill.SkillData SkillData{get;private set; }


		public void SetUpButton(skill.SkillData data,bool onSkillTree)
		{
			SkillData = data;
			GetComponent<Image>().sprite = ResourceLoader.Instance.EmblemTexturePacker[SkillData.name];
			levelCount = SkillManager.GetLevelCount(SkillData.skillName);

			//---スキルツリー上かどうかを判定
			if(onSkillTree)
			{
				inSkillSlot = false;
				//---テキストの作成
				GetComponentInChildren<Text>().text = CreateDiscriptionText();
			}
			else
			{
				GetComponentInChildren<Text>().gameObject.SetActive(false);
			}


		}

		string CreateDiscriptionText()
		{
			return $"{SkillData.skillName}\nCost: {SkillData.learningCost}\n{levelCount} /  {SkillData.maxLevel+SkillManager.GetKeyStoneSkillLevel(SkillData.skillName)}";
		}


		public void OnPointerDown(PointerEventData eventData)
		{
			//---スキルツリー以外での処理
			if(!inSkillSlot)
			{
				//---スキルマネージャーのレベルアップステータスを実行
				SkillManager.LevelUpStatus(SkillData.skillName, ref levelCount, SkillData.spell, !SkillData.activeSkill);
				GetComponentInChildren<Text>().text = CreateDiscriptionText();
				
			}
			else
			{
				print("Cast" + SkillData.skillName);
				var player = GameObject.FindGameObjectWithTag("Player");
				SkillData.spell(player.GetComponent<Player>(), levelCount);
			}

		}

		public void DisplayInfo()
		{
			var infoPanelRes = Resources.Load("SkillTreeButton/InfoDisplayPanel");
			var infoPanel = Instantiate(infoPanelRes, new Vector2(0, 0), Quaternion.identity,transform.parent.parent.parent) as GameObject;
			var rect = infoPanel.GetComponent<RectTransform>();
			rect.anchorMax = new Vector2(0.5f,0.5f);
			rect.anchorMin = new Vector2(0.5f,0.5f);
			rect.anchoredPosition = new Vector3(0, 0);
			
			string discriptionText = SkillData.activeSkill ? "[ ActiveSkill ]\n":"[ PassiveSkill ]\n";
			discriptionText += ui.LanguageManager.ParseString(SkillData.description);
			infoPanel.GetComponentInChildren<Text>().text = discriptionText;

		}


	}
}