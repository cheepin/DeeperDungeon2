using System.Collections;
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using moving.player;
using skill.spell;


namespace skill
{
	[Serializable]
	public class HoldSkillData : util.ConvertableBase64
	{


		public HoldSkillData() : base("SkillData")
		{
		}

		public List<string> CurrentSkillEmblemList
		{
			get
			{
				return currentSkillEmblemList;
			}
			set
			{
				currentSkillEmblemList = value;
			}
		}

		public Dictionary<string, int> LevelsCounter
		{
			get
			{
				return levelsCounter;
			}
			set
			{
				levelsCounter = value;
			}
		}

		public Dictionary<string,int> KeyStoneSkillCounter
		{
			get
			{
				return toolSkillCounter;
			}
			set
			{
				toolSkillCounter = value;
			}

		}

		public void AddSkill(string skillName)
		{
			CurrentSkillEmblemList.Add(skillName);
		}

		private List<string> currentSkillEmblemList = new List<string>();
		private Dictionary<string, int> levelsCounter = new Dictionary<string, int>();
		private Dictionary<string, int> toolSkillCounter = new Dictionary<string, int>();

	}

	public class SkillManager : util.Singleton<SkillManager>
	{
		GameObject EventSystem = null;
		Player player = null;
		public HoldSkillData mySkillData = null;
		//---現在所持しているスキルのリストと辞書型
		[SerializeField]
		SkillDataPacker skillDataPacker=null;
		/// <summary>
		/// スキルデータのリスト
		/// </summary>
		static public SkillDataPacker SkillDataPacker {get{ return Instance.skillDataPacker;} }


		Text NumberOfGem;
		Text SkillPointText;

		static public T GetSkillData<T>(string skillName) where T:SkillData
		{
			//Debug.Assert(Instance.skillDataDict.ContainsKey(skillName), $"入力されたスキル{skillName}はスキルマネージャーに登録されていません");
			return Instance.skillDataPacker[skillName] as T;
		}

		static public void Save()
		{
			Instance.mySkillData.SaveData();
		}

		static public void Load()
		{
			HoldSkillData skilldata = new HoldSkillData();
			Instance.mySkillData = (HoldSkillData)skilldata.LoadData();

			//---プレイヤーデリゲートを登録
			Instance.player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
			var moreThanLevel1 = Instance.mySkillData.CurrentSkillEmblemList.Where((X)=>Instance.mySkillData.LevelsCounter[X]>0);
			foreach(var skillName in moreThanLevel1)
			{
				var baseSpell = Activator.CreateInstance(Type.GetType("skill.spell."+skillName)) as skill.spell.BaseSpell;
				baseSpell.RegisterListener(Instance.player);
			}


		}

		protected override void Awake()
		{
			base.Awake();

			//---ロードされていなかったら
			if(mySkillData==null)
			{
				mySkillData = new HoldSkillData();

			}
			//---ジェムのテキストを取得
			NumberOfGem = GameObject.Find("NumberOfGem").GetComponent<Text>();
			
		}


		// Use this for initialization
		void Start()
		{
			//---スキルツリーに入れ替わった時にEventSystemとスキルポイントテキストを取得するデリゲートを追加
			SceneManager.sceneLoaded += (scene, mode) =>
			{
				if(scene.name == "SkillTree")
				{
					Instance.EventSystem = GameObject.Find("EventSystem");
					SkillPointText = GameObject.Find("SkillPoint").GetComponent<Text>();
					SkillPointText.text = "SkillPoint:" + Instance.player.MyPlayerData.NumberOfGem;

				}
			};

			dungeon.DontDestroyHelper.SetDontDestroy(gameObject);
			Instance.player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
			if(player == null)
				Debug.Log("Player Not Found");


		}

		// Update is called once per frame
		void Update()
		{

		}
		static public void BackGame()
		{
			Instance.EventSystem.SetActive(false);
			dungeon.DungeonManager.CloseSkillTree();
		}

		static public void SetGemWhenFillExpBar(int numBerOfGem)
		{
			Instance.NumberOfGem.text = numBerOfGem.ToString();
		}

		static public int GetLevelCount(string skillName)
		{
			if(Instance.mySkillData.LevelsCounter.ContainsKey(skillName))
				return Instance.mySkillData.LevelsCounter[skillName];
			else
			{
				const int initLevel = 0;
				Instance.mySkillData.LevelsCounter[skillName] = initLevel;
				return Instance.mySkillData.LevelsCounter[skillName];
			}
		}

		static public void LevelUpStatus(string skillName, ref int nextLevel, Func<Player, int, bool> func, bool passiveSkill)
		{
			//---スキルジェムがコストより少なかった場合、もしくはMAXレベルに到達していた場合は何もせずに終了する
			if(Instance.player.MyPlayerData.NumberOfGem < Instance.skillDataPacker[skillName].learningCost ||
			   nextLevel + 1 > Instance.skillDataPacker[skillName].maxLevel + (Instance.mySkillData.KeyStoneSkillCounter.ContainsKey(skillName)? Instance.mySkillData.KeyStoneSkillCounter[skillName]: 0))
				return;

			//---レベルアップ処理
			ToNextLevel(skillName, ref nextLevel, func, passiveSkill,true);
			//---ジェムを消費
			if(Instance.SkillPointText!=null)
			{
				Instance.SkillPointText.text = "SkillPoint:" + (Instance.player.MyPlayerData.NumberOfGem -= Instance.skillDataPacker[skillName].learningCost);
				//---音
				SoundManager.Click1();

			}
		}

		static void ToNextLevel(string skillName, ref int nextLevel, Func<Player, int, bool> func, bool passiveSkil,bool up)
		{
			//---パッシブスキルなら用意してたアクションを実行（主にプレイヤーを強化するデリゲートが入っている）
			if(passiveSkil)
				func?.Invoke(Instance.player, nextLevel);
			//---現在のスキルレベルを登録
			if(up)
				Instance.mySkillData.LevelsCounter[skillName] = ++Instance.mySkillData.LevelsCounter[skillName];
			else
				Instance.mySkillData.LevelsCounter[skillName] = --Instance.mySkillData.LevelsCounter[skillName];

			//---参照intに現在のレベルを付与
			nextLevel =　Instance.mySkillData.LevelsCounter[skillName];
		}

		static public void LevelDownStatus(string skillName,ref int nextLevel, Func<Player, int, bool> func, bool passiveSkill)
		{
			//---レベルダウン処理
			ToNextLevel(skillName,ref nextLevel, func, passiveSkill,false);
		}

		static public List<string> GetSkillButtonList()
		{
			return Instance.mySkillData.CurrentSkillEmblemList;
		}

		public Action<string> ActionWhenSetKeyStoneEmblem;
		/// <summary>
		/// エンブレムを拾ったら登録する
		/// </summary>
		/// <param name="skillname"></param>
		/// <returns></returns>
		
		static public int SetSkillListFromEmblem(string skillname)
		{
			//---持っていなかったらCurrentSkillEmblemListに追加
			if(!Instance.mySkillData.CurrentSkillEmblemList.Contains(skillname))
			{
				Instance.mySkillData.CurrentSkillEmblemList.Add(skillname);
				Instance.mySkillData.LevelsCounter[skillname] = 0;
				//---キーストーンスキルだったら辞書を作成
				if(Instance.skillDataPacker[skillname].keyStoneSkill)
				{
					Instance.ActionWhenSetKeyStoneEmblem?.Invoke(skillname);
					Instance.mySkillData.KeyStoneSkillCounter[skillname] = 0;
				}
					
				return 0;
			}
			//---ToolSkillはMaxLevelを上げる
			else if(Instance.skillDataPacker[skillname].keyStoneSkill)
			{
				var toolSkillDict = Instance.mySkillData.KeyStoneSkillCounter;
				if(!toolSkillDict.ContainsKey(skillname))
					toolSkillDict[skillname] = 1;
				else
					toolSkillDict[skillname] += 1;

				return 0;
			}
			//---既に持ってたら次の経験値の半分をオーブにする
			else
			{
				return Instance.player.MyPlayerData.AmountToNextLevel/2;
			}
		}

		//---現在習得しているアクティブ属性スキルを取得
		static public List<String> GetCurrentActiveSkill()
		{
			//---アクティブ属性でかつレベル１以上のスキルを返す
			List<string> activeSkillList = Instance.skillDataPacker.SkillList.Where((x) => x.activeSkill).Select((x) => x.skillName).ToList();
			return Instance.mySkillData.CurrentSkillEmblemList.Where((x) => activeSkillList.Contains(x) && Instance.mySkillData.LevelsCounter[x] > 0)
					.ToList();

		}
		//---スキルを発動
		static public void CastActiveSpell(Func<Player, int, bool> spell, string spellName)
		{
			//---ジェムがコスト分ちゃんとあるか確認
			int spellCost = Instance.skillDataPacker[spellName].spellCost;
			if(Instance.player.MyPlayerData.NumberOfGem < spellCost)
				return;

			int manaCost = Instance.skillDataPacker[spellName].manaCost;
			if(Instance.player.TempPlayerData.Mana < manaCost)
				return;
			//---詠唱に成功したらジェムとマナを消費
			if(spell(Instance.player, Instance.mySkillData.LevelsCounter[spellName]))
			{
				Instance.player.MyPlayerData.NumberOfGem -= spellCost;
				Instance.player.TempPlayerData.Mana -= manaCost;
			}

		}
		/// <summary>
		///	現在のツールスキルの値を取得
		///	無かったら０を返す
		/// </summary>
		/// <param name="skillName">調べたいスキルの名前</param>
		/// <returns></returns>
		static public int GetKeyStoneSkillLevel(string skillName)
		{
			return Instance.mySkillData.KeyStoneSkillCounter.ContainsKey(skillName)? Instance.mySkillData.KeyStoneSkillCounter[skillName] : 0;
		}
		/// <summary>
		/// 指定したスキルレベルのMaxレベルを一つ下げる
		/// 主にKeyで使用
		/// </summary>
		/// <param name="skillName"></param>
		static public void DecreaseMaxLevel(string skillName)
		{
			--Instance.mySkillData.KeyStoneSkillCounter[skillName];
		}

	}

}