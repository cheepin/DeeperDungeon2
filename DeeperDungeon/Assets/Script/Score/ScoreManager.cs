using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using util;
using moving.player;
using System.IO;
using System;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;
using System.Text;
using UnityEngine.SceneManagement;
namespace score
{
	public class ScoreManager : Singleton<ScoreManager>
	{
		public RecordScore GetRecordScore
		{
			get
			{
				return recordScore;
			}	
		}
		RecordScore recordScore;
		// Use this for initialization
		void Start()
		{
			var player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
			dungeon.DontDestroyHelper.SetDontDestroy(Instance.gameObject);
			#if UNITY_EDITOR
			player.ActionWhenDie += SaveScore;
			#endif 
		}

		static public void SaveScore(Player _player)
		{
			var playerData = _player.MyPlayerData;
			RecordScore _recodeScore = new RecordScore()
			{
				time = $"{System.DateTime.Now.ToShortDateString()} {System.DateTime.Now.ToShortTimeString()}",
				difficulty = playerData.Difficulty,
				floor = playerData.CurrentFloor,
				level = playerData.Level,
				MaxHP = playerData.MaxHP,
				MaxMana = playerData.MaxMana,
				Attack = playerData.Attack,
				Defense = playerData.Defense,
				CurrentLevel = playerData.CurrentLevel,
				PoisonResistance = playerData.PoisonResistance,
				IceResistance = playerData.IceResistance,
				FireResistance = playerData.FireResistance,
				lightningResistance = playerData.LightningResistance,
			};

			Instance.recordScore = _recodeScore;
			
			//var score = JsonUtility.ToJson(_recodeScore);
			
			////---プレイヤースコアの生成
			//var setting = new DataContractJsonSerializerSettings
			//{
			//	UseSimpleDictionaryFormat = true
			//};
			////---Skillスコアの生成
			////---MemoryStreamを生成してJsonでまとめる
			//var ser = new DataContractJsonSerializer(typeof(RecordSkill),setting);
			//using (MemoryStream memoryStream = new MemoryStream())
			//{
			//	var _skillDict = skill.SkillManager.Instance.mySkillData.LevelsCounter;
			//	//---データ用クラスの生成
			//	RecordSkill record =  new RecordSkill()
			//	{
			//		skillDict = _skillDict
			//	};
			//	ser.WriteObject(memoryStream,record);
			//	string skillJson = Encoding.UTF8.GetString(memoryStream.ToArray());

			//	//#if UNITY_EDITOR
			//	//File.AppendAllText("C:/Users/fujit/Downloads/savedata/Score.txt",$"{Environment.NewLine}{Environment.NewLine} {score} {Environment.NewLine} {skillJson}");
			//	//#endif
				
			
	
		}

		public void BackTitle()
		{
			SceneManager.LoadScene("Title");
		}

	} 
	[Serializable]
	public struct RecordScore
	{
		public string time;
		public int difficulty;
		public int floor;
		public int level;
		public int	MaxHP;
		public int MaxMana;
		public int	Attack;
		public int	Defense;
		public int	CurrentLevel;
		public int	CurrentFloor;
		public int	NumberOfGem;
		public int	PoisonResistance;
		public int	IceResistance;
		public int FireResistance;
		public int lightningResistance;

	}

	[DataContract]
	public class RecordSkill
	{
		[DataMember]
		public Dictionary<string,int>  skillDict;
	}
}
