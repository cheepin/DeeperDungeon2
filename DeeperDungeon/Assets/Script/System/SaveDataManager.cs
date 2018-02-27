using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using skill;
using moving.player;


//---データのセーブ・ロードのみ使用
public class SaveDataManager : util.Singleton<SaveDataManager> {

	PlayerData playerData=null;
	bool loadedFlag=false;
	// Use this for initialization
	void Start () {
		
		DontDestroyOnLoad(gameObject);
	}
	
	static public  void SaveOperation<T>(T SaveObj)
	{
		util.saveload.Save.SaveObjToPath("mySaveData",SaveObj,"C:/Users/fujit/Downloads/savedata.txt");
	}

	static public void SetPlayerDataFromLoad(PlayerData loadedData)
	{
		Instance.playerData = loadedData;

	}

	static public void RemoveDataFromPath()
	{
		util.saveload.Remove.RemoveObjFromPath("C:/Users/fujit/Downloads/savedata/PlayerData.txt");

	}

	static public void RemoveDataPref(string key)
	{
		util.saveload.Remove.RemovePrefData(key);
	}
	static public PlayerData GetPlayerData()
	{
		Debug.Assert(Instance!=null,"直接ダンジョンシーンに入りました。デバッグモードがオフになってないか確認してください");
		if(Instance.loadedFlag)
		{
			var playData =  Instance.playerData;
			Instance.playerData = null;
			return playData;
		}
		else
			return null;

	}
	static public void SetLoadFlag(bool flag)
	{
		Instance.loadedFlag = flag;
	}
	
	static public void SaveWholeGameData()
	{	
		Debug.Assert(!dungeon.DungeonManager.IsDebugMode(),"現在デバッグモード中なのでセーブされません");
		//---プレイヤーデータの保存
		var k = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
		k.MyPlayerData.SaveData();

		//---スキルデータの保存
		SkillManager.Save();

		//---スキルスロットの保存
		var slots =  GameObject.FindGameObjectsWithTag("SkillSlot");
		foreach(var item in slots)
		{
			item.GetComponent<SkillSlot>().Save();
		}

	}
}
