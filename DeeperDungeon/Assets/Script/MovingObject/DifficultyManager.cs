using System;
using UnityEngine;
using UnityEngine.SceneManagement;

using util;
using CH = util.CoroutineHelper;
namespace moving.player
{

	public class DifficultyManager : Singleton<DifficultyManager>
	{
		[SerializeField]
		float normalAmountExpCoefficient = 0.98f;
		public Difficulty Difficult{ get;set;}
		/// <summary>
		/// ロードされてたらオンにする
		/// </summary>
		public bool fromLoad = false;
		Player player;

		public enum Difficulty
		{
			Normal,Hard
		}

		protected override void Awake()
		{
			base.Awake();
			Instance.StartCoroutine(CH.Chain(Instance,
				CH.DelaySecondLoop(0.05f,()=>Instance.player==null,()=>
				{
					Instance.player = GameObject.FindGameObjectWithTag("Player")?.GetComponent<Player>();
				}),
				CH.Do(()=>RegisterListener())));
			print("awake");
		}

		void Start()
		{
			DontDestroyOnLoad(gameObject);
		}

		Action<int> actionWhenLevelUp;
		static void RegisterListener()
		{
			var playerData = Instance.player.MyPlayerData;
			print(Instance.fromLoad);
			if(Instance.fromLoad)
			{
				Instance.Difficult = (Difficulty)Instance.player.MyPlayerData.Difficulty;
				print(Instance.Difficult);
			}
			else
			{
				playerData.Difficulty = (int)Instance.Difficult;
			}
			Instance.actionWhenLevelUp = (a)=>IncreaseAmountToNextExpByDifficuly(playerData);
			//---イベントの登録
			playerData.SetActionWhenParamUpdate("LevelUp",Instance.actionWhenLevelUp,"diffy",false);
			Instance.player = null;
		}
		//---難易度に応じて次に必要なジェムの数を増やす
		static void IncreaseAmountToNextExpByDifficuly(PlayerData playerData)
		{
			if(Instance.Difficult==Difficulty.Normal)
			{
				playerData.AmountToNextLevel = (int) (playerData.AmountToNextLevel* Instance.normalAmountExpCoefficient);
				
			}
		}
	}

}