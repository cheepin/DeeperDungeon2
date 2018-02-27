using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using skill;
using CH  = util.CoroutineHelper;

namespace dungeon{
	[Serializable]
	public class UnityEventInt:UnityEvent<int>{}
	[DefaultExecutionOrder(-1)]
	public class DungeonManager : util.Singleton<DungeonManager> {
		
		[SerializeField]
		bool DebugMode=false;
		[SerializeField]
		bool Limit60FPS = false;
		[SerializeField]
		public FloorMapper FloorMapper;
		[SerializeField]
		public Text floorText;
		[SerializeField]
		public int StartFloor;
		[System.NonSerialized]
		public PlayerData playerData = null;
		GameObject eventSystem = null;
		
		int dungeonLevel = 1;
		public int DungeonLevel{get{return dungeonLevel; }set{dungeonLevel = value;} }
		/// <summary>
		/// セーブして終了する準備が完了したフラグ
		/// </summary>
		public bool WillSaveFinish{get;set; }
		public bool FromLoad{get;private set;}=false;
		bool firstBoot=true;

		static public TileType[][] GetFloorTilesData()
		{
			return Instance.FloorMapper.CurrentMapper.Tiles;
		}

		static public bool IsDebugMode()
		{
			return Instance.DebugMode;
		}
		static public BoardPlacer GetCurrentBoardCreator()
		{
			return Instance.FloorMapper.CurrentMapper;
		}

		[SerializeField] util.UnityEventGameObject InitializeEventInStart=null;
		[SerializeField] UnityEventInt CalledLoadScene=null;
		public bool CangelFeedIn{get;set; }
		
		protected override void Awake()
		{
			base.Awake();
			//---ロードデータがあったらロード
			if(!DebugMode)
			{
				Application.targetFrameRate = 60;
				playerData = SaveDataManager.GetPlayerData();
				SaveDataManager.SetLoadFlag(false);
			}
			else
			{
				Application.targetFrameRate = 60;
			}
			if(Limit60FPS)
			{
				#if UNITY_EDITOR
				Application.targetFrameRate = 60;
				#endif
			}
			if(playerData != null)
			{
				Instance.DungeonLevel = playerData.CurrentFloor;
				SkillManager.Load();
				FromLoad = true;
			}
			
			//---デバッグ用コマンド
			if(Instance.firstBoot && DebugMode)
			{
				Instance.DungeonLevel = StartFloor;
				Instance.firstBoot = false;
			}
			//---フロア階層の更新
			Instance.FloorMapper.InstantiateFloor(Instance.DungeonLevel);
			//---指定階は特殊なシーンをロード
			Debug.Assert(Instance.CalledLoadScene!=null,"CalledLoadSceneがセットされていません");
			Instance.CalledLoadScene.Invoke(Instance.DungeonLevel);
			if(!Instance.CangelFeedIn)
				Instance.StartCoroutine(FadeIn.StartFadeIn());

			
		}
	
		private void Start()
		{
			if(DebugMode)
			{
				print("デバッグモードで実行 セーブデータは読み込まれません");
			}
			InitializeEventInStart.Invoke(gameObject);
		}

		static public bool LoadPlayerData(ref PlayerData player)
		{
			if(Instance.playerData != null)
			{
				player.CopyData(Instance.playerData);
				return true;

			}
			else
			{
				return false;
			}
		}

		static void BeforeOpenOtherScene()
		{
			Time.timeScale = 0;
			Instance.eventSystem = GameObject.Find("EventSystem");
			Instance.eventSystem.SetActive(false);
		}

		static public void OpenOption()
		{
			optionData.OptionManager.fromTitle = false;
			BeforeOpenOtherScene();

			SceneManager.LoadScene("Option",LoadSceneMode.Additive);
		}
		static public void CloseOption()
		{
			//---オプションシーンをアンロード
			while(SceneManager.UnloadSceneAsync("Option").isDone){}
			Time.timeScale = 1;
			Instance.eventSystem.SetActive(true);

			//---オプション設定を更新
			AnalogStick.LoadThreshold();
			AnalogStick.LoadAnalogStickSize();

			AttackButton.LoadButtonSizeAndOpaque();
		}

		public Action ActionWhenSkillTreeEvent;

		static public void CloseSkillTree()
		{
			//---オプションシーンをアンロード
			while(SceneManager.UnloadSceneAsync("SkillTree").isDone){}
			Time.timeScale = 1;
			Instance.eventSystem.SetActive(true);
			Instance.ActionWhenSkillTreeEvent.Invoke();
		}
		static public void OpenSkillTree()
		{
			Instance.StartCoroutine(
				CH.Chain(Instance,
				CH.Do(()=>BeforeOpenOtherScene()),
				CH.Do(()=>SceneManager.LoadScene("SkillTree",LoadSceneMode.Additive))));
			
		}



	}	

	

	

}