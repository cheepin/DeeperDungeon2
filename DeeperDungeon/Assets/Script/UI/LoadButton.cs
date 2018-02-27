using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class LoadButton : MonoBehaviour {


	void Start () {
		var save = new PlayerData();
		if(!PlayerPrefs.HasKey(save.DataName))
		{
			GetComponent<Button>().interactable =false;
			GetComponentInChildren<Text>().color -= new Color(0,0,0,0.5f);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Click()
	{

		SaveDataManager.SetLoadFlag(true);
		SoundManager.Push();
		//---プレイヤーデータのロード
		var save = new PlayerData();
		PlayerData SaveData = (PlayerData)save.LoadData();
		SaveDataManager.SetPlayerDataFromLoad(SaveData);
		//---Difficultyマネージャの設定
		moving.player.DifficultyManager.Instance.fromLoad = true;
		//TODO:ロード時のクラッシュが直るまでデータを消さないようにする　終わったら消す
#if UNITY_EDITOR
		SaveDataManager.RemoveDataFromPath();
#endif

#if UNITY_ANDROID
		SaveDataManager.RemoveDataPref(SaveData.DataName);
	#endif

		SceneManager.LoadScene("Dungeon");
	}
}
