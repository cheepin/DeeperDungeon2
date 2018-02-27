using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SkillTreeButton : MonoBehaviour {

	public void Open()
	{
		SoundManager.Click2();
		dungeon.DungeonManager.OpenSkillTree();
	}

	public void Cheat()
	{
		SceneManager.LoadScene("Cheat",LoadSceneMode.Additive);
	}
}
