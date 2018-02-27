using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ui
{
	public class LanguageChanger : MonoBehaviour
	{
		[SerializeField] Dropdown dropdown;

		private void Start()
		{
			dropdown.value = PlayerPrefs.GetInt(LanguageManager.id_Laungage);
			
		}

		public void Select(int index)
		{
			PlayerPrefs.SetInt(LanguageManager.id_Laungage,index);
		}
	}

}