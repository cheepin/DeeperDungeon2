using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using skill;
using UnityEngine.SceneManagement;

using UnityEngine.UI;


public class SaveButton : MonoBehaviour {

	public string saveOrLoad;
	
	public void Click()
	{
		if(saveOrLoad=="Save")
		{
			SoundManager.Click2();
			GetComponent<Button>().interactable = false;

			var display = Resources.Load("SkillTreeButton/InfoDisplayPanel") as GameObject;
			var displayPanel = Instantiate(display,transform);
			displayPanel.GetComponent<RectTransform>().anchoredPosition = new Vector2(0,500);
			
			if(!dungeon.DungeonManager.IsDebugMode())
				displayPanel.GetComponentInChildren<Text>().text = ui.LanguageManager.ParseString("<EN>Current Data will be Saved in Next Floor.</EN><JP>次のフロアの階段を降りた時にセーブされます </JP>");
			else
				displayPanel.GetComponentInChildren<Text>().text = "デバッグモードなのでセーブされません";
			
			//---シーン切り替わり時に使用するラムダを登録
			dungeon.DungeonManager.Instance.WillSaveFinish = true;

		}
		else
			throw new NotImplementedException();
	}

}
