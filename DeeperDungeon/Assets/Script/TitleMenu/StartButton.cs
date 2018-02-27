using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using moving.player;
using UnityEngine.Advertisements;
public class StartButton : MonoBehaviour {

	[SerializeField]
	DifficultyManager difficultyManager=null;

	private void Start()
	{
		
	}

	public void LoadScene()
	{
		SaveDataManager.SetLoadFlag(false);
		SoundManager.Push();
		SceneManager.LoadScene("Dungeon");
	}

	public void LoadNormalScene()
	{
		difficultyManager.Difficult = DifficultyManager.Difficulty.Normal;
		LoadScene();
	}
	public void LoadHardScene()
	{
		difficultyManager.Difficult = DifficultyManager.Difficulty.Hard;
		LoadScene();
	}
}
