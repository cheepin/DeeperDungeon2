using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace ui
{
	
	public class HardButton : MonoBehaviour
	{
		public const string id_CreardNomal = "id_CreardNomal";
		[SerializeField] Button hardButton;
		Color myColor;
		void Start()
		{
			hardButton = GetComponent<Button>();
			Text buttontext = GetComponentInChildren<Text>();
			myColor =  GetComponentInChildren<Text>().color;
			int isCreared = PlayerPrefs.GetInt(id_CreardNomal);
			if(isCreared==1)
			{
				hardButton.interactable = true;
				buttontext.color = myColor;
			}
			else
			{
				hardButton.interactable = false;
				buttontext.color = myColor - new Color(0.0f,0.0f,0.0f,0.5f);

			}
		}


		void Update()
		{

		}
	}

}