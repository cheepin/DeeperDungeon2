using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

using OptionManager = optionData.OptionManager;
public class SetupSetting : MonoBehaviour {

	private void Awake()
	{
		if(PlayerPrefs.GetInt(OptionManager.id_X_LeftAxisSensitive)==0)
				PlayerPrefs.SetInt(OptionManager.id_X_LeftAxisSensitive,50);
		if(PlayerPrefs.GetInt(OptionManager.id_X_RightAxisSensitive)==0)
				PlayerPrefs.SetInt(OptionManager.id_X_RightAxisSensitive,50);
		if(PlayerPrefs.GetInt(OptionManager.id_Y_UpAxisSensitive)==0)
				PlayerPrefs.SetInt(OptionManager.id_Y_UpAxisSensitive,50);
		if(PlayerPrefs.GetInt(OptionManager.id_Y_DownAxisSensitive)==0)
				PlayerPrefs.SetInt(OptionManager.id_Y_DownAxisSensitive,50);

		if(PlayerPrefs.GetFloat(OptionManager.id_AnalogStickSize)==0)
				PlayerPrefs.SetFloat(OptionManager.id_AnalogStickSize,0.8f);
		if(PlayerPrefs.GetFloat(OptionManager.id_AnalogStickOpaque)==0)
				PlayerPrefs.SetFloat(OptionManager.id_AnalogStickOpaque,0.8f);

		if(PlayerPrefs.GetFloat(OptionManager.id_AttackButonSize)==0)
				PlayerPrefs.SetFloat(OptionManager.id_AttackButonSize,0.8f);
		if(PlayerPrefs.GetFloat(OptionManager.id_AttackButonOpaque)==0)
				PlayerPrefs.SetFloat(OptionManager.id_AttackButonOpaque,0.8f);
		if(!PlayerPrefs.HasKey(OptionManager.id_SEVolume))
		{
			PlayerPrefs.SetInt(OptionManager.id_SEVolume,-11);

		}

	}

}
