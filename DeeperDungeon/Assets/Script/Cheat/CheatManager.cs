using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using moving.player;
using skill;
using System.Linq;

namespace cheat
{
	public class CheatManager : util.Singleton<CheatManager>
	{

		GameObject eventSystem;
		Player player;
		[SerializeField]
		Text MaxHpValueText = null;
		[SerializeField]
		Text MaxManaValueText= null;
		[SerializeField]
		Text AttackValueText= null;
		[SerializeField]
		Text DefenseValueText= null;
		[SerializeField]
		Text CurrentFloorValueText = null;
		[SerializeField]
		Text AllSkillValueText = null;
		[SerializeField]
		Text LvValueText = null;

		[SerializeField]
		GameObject cheatEventSystem;
		[SerializeField]
		GameObject page1;
		[SerializeField]
		GameObject page2;

		// Use this for initialization
		void Start()
		{
			Time.timeScale = 0;
			eventSystem = GameObject.Find("EventSystem");
			eventSystem?.SetActive(false);
			cheatEventSystem.SetActive(true);
			player = GameObject.Find("Martz")?.GetComponent<Player>();
			if(player!=null)
			{
				MaxHpValueText.text = Instance.player.MyPlayerData.MaxHP.ToString();
				MaxManaValueText.text = Instance.player.MyPlayerData.MaxMana.ToString();
				AttackValueText.text = Instance.player.MyPlayerData.Attack.ToString();
				DefenseValueText.text = Instance.player.MyPlayerData.Defense.ToString();

			}
		}

		// Update is called once per frame
		void Update()
		{

		}

		static public void SetStatus(string skillName,int changeValue)
		{

			switch(skillName)
			{
				case "MaxHP":
					Instance.player.MyPlayerData.MaxHP += changeValue;
					Instance.MaxHpValueText.text = Instance.player.MyPlayerData.MaxHP.ToString();
					break;
				case "MaxMana":
					Instance.player.MyPlayerData.MaxMana += changeValue;
					Instance.MaxManaValueText.text = Instance.player.MyPlayerData.MaxMana.ToString();
					break;
				case "Attack":
					Instance.player.MyPlayerData.Attack += changeValue;
					Instance.AttackValueText.text = Instance.player.MyPlayerData.Attack.ToString();
					break;
				case "Defense":
					Instance.player.MyPlayerData.Defense += changeValue;
					Instance.DefenseValueText.text = Instance.player.MyPlayerData.Defense.ToString();
					break;
				case "HP":
					Instance.player.MyPlayerData.HP += changeValue;
					break;
				case "CurrentFloor":
					Instance.player.MyPlayerData.CurrentFloor += changeValue;
					Instance.CurrentFloorValueText.text = Instance.player.MyPlayerData.CurrentFloor.ToString();
					break;
				
				case "AllSkill":
					Instance.player.MyPlayerData.NumberOfGem = 100;
					List<SkillData> list = SkillManager.Instance.GetAllSkillData();
					SkillManager.Instance.SetAllSkillData(list);
					Instance.AllSkillValueText.text = changeValue.ToString();
					break;
				case "Lv":
					Instance.player.MyPlayerData.CurrentExp += Instance.player.MyPlayerData.AmountToNextLevel;
					Instance.LvValueText.text = Instance.player.MyPlayerData.CurrentLevel.ToString();
					break;
				default:
					break;
			}
		}


		public void BackGame()
		{
			Time.timeScale = 1;
			cheatEventSystem.SetActive(false);
			eventSystem?.SetActive(true);
			SceneManager.UnloadSceneAsync("Cheat");
		}

		public void NextPage()
		{
			page1.SetActive(false);
			page2.SetActive(true);
		}

	} 

	static public class ClassSkillManagerEx
	{
		static public List<SkillData> GetAllSkillData(this SkillManager skillManager)
		{
			return SkillManager.SkillDataPacker.SkillList;
		}
		static public void SetAllSkillData(this SkillManager skillManager,List<SkillData> skillList)
		{
			skillList.ForEach(X=>
			{
				SkillManager.SetSkillListFromEmblem(X.skillName);
				
			});
		}
	}
}
