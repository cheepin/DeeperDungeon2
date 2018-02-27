using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace ui
{
	public class Credit : MonoBehaviour
	{

		public void ShowCredit()
		{
			var infoPanelRes = Resources.Load("SkillTreeButton/InfoDisplayPanel");
			var infoPanel = Instantiate(infoPanelRes, new Vector2(0, 0), Quaternion.identity,transform.parent) as GameObject;
			var rect = infoPanel.GetComponent<RectTransform>();
			rect.anchorMax = new Vector2(0.5f,0.5f);
			rect.anchorMin = new Vector2(0.5f,0.5f);
			rect.anchoredPosition = new Vector3(0, 0);
			
			var infoText = infoPanel.GetComponentInChildren<Text>();
			infoText.fontSize = 22;
			infoText.alignment = TextAnchor.MiddleLeft;
			infoText.text = "Paper Popup Backgrounds --- darkwood67: http://darkwood67.deviantart.com/gallery/ \n\n";
			infoText.text += "Gem Jewel Diamond Glass --- Osmic: https://opengameart.org/content/gem-jewel-diamond-glass";

		}

		public void Review()
		{
			Application.OpenURL("market://details?id=com.lycorisaudio.deeperdungeon2");
			
		}
	}

}