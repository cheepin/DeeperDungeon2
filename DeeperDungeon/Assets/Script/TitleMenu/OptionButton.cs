using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionButton : MonoBehaviour {

	public void OpenOption()
	{
		optionData.OptionManager.fromTitle = true;
		SoundManager.Click2();
		SceneManager.LoadScene("Option",LoadSceneMode.Single);
	}
	public void OpenOptionFromGame()
	{
		dungeon.DungeonManager.OpenOption();
	}
}
