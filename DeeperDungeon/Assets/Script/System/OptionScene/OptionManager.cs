using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


namespace optionData
{



	public class OptionManager : MonoBehaviour {

		public const string id_X_LeftAxisSensitive = "X_AxisSensitiveLeft";
		public const string id_X_RightAxisSensitive = "X_AxisSensitiveRight";
		public const string id_Y_UpAxisSensitive = "Y_AxisSensitiveUp";
		public const string id_Y_DownAxisSensitive = "Y_AxisSensitiveDown";
		public const string id_AnalogStickSize = "AnalogStickSize";
		public const string id_AnalogStickOpaque = "AnalogStickOpaque";
		public const string id_AttackButonSize = "AttackButonSize";
		public const string id_AttackButonOpaque= "AttackButonOpaque";
		public const string id_SEVolume= "SEVolume";


		static public bool fromTitle=true;
		[SerializeField]
		private Slider x_LeftAnalogStickSensitive=null;
		[SerializeField]
		private Slider x_RightAnalogStickSensitive=null;
		[SerializeField]
		private Slider y_UpAnalogStickSensitive=null;
		[SerializeField]
		private Slider y_DownAnalogStickSensitive=null;
		[SerializeField]
		private Slider analogStickSize=null;
		[SerializeField]
		private Slider analogStickOpaque=null;
		[SerializeField]
		private Slider attackButonSize=null;
		[SerializeField]
		private Slider attackButonOpaque=null;
		[SerializeField]
		private Slider seVolume=null;
		[SerializeField]
		GameObject Page1=null;
		[SerializeField]
		GameObject Page2=null;
		[SerializeField]
		GameObject Page3=null;
		[SerializeField]
		Button backButton = null; 
		[SerializeField]
		GameObject eventSystem = null;

		private void Awake()
		{
			//---ロードデータを読み込む
			x_LeftAnalogStickSensitive.value = PlayerPrefs.GetInt(id_X_LeftAxisSensitive);
			x_RightAnalogStickSensitive.value = PlayerPrefs.GetInt(id_X_RightAxisSensitive);
			y_UpAnalogStickSensitive.value = PlayerPrefs.GetInt(id_Y_UpAxisSensitive);
			y_DownAnalogStickSensitive.value = PlayerPrefs.GetInt(id_Y_DownAxisSensitive);

			analogStickSize.value = PlayerPrefs.GetFloat(id_AnalogStickSize);	
			analogStickOpaque.value = PlayerPrefs.GetFloat(id_AnalogStickOpaque);	

			attackButonSize.value = PlayerPrefs.GetFloat(id_AttackButonSize);	
			attackButonOpaque.value = PlayerPrefs.GetFloat(id_AttackButonOpaque);	

			seVolume.value = PlayerPrefs.GetInt(id_SEVolume);

			Page1.SetActive(true);
			Page2.SetActive(false);
			if(fromTitle)
				backButton.transform.GetComponentInChildren<Text>().text = "Back Title";
			else
				backButton.transform.GetComponentInChildren<Text>().text = "Back Game";

		}

		public void BackTitlePressed()
		{
			PlayerPrefs.SetInt(id_X_LeftAxisSensitive,(int)x_LeftAnalogStickSensitive.value);
			PlayerPrefs.SetInt(id_X_RightAxisSensitive,(int)x_RightAnalogStickSensitive.value);
			PlayerPrefs.SetInt(id_Y_UpAxisSensitive,(int)y_UpAnalogStickSensitive.value);
			PlayerPrefs.SetInt(id_Y_DownAxisSensitive,(int)y_DownAnalogStickSensitive.value);

			PlayerPrefs.SetFloat(id_AnalogStickSize,analogStickSize.value);
			PlayerPrefs.SetFloat(id_AnalogStickOpaque,analogStickOpaque.value);

			PlayerPrefs.SetFloat(id_AttackButonSize,attackButonSize.value);
			PlayerPrefs.SetFloat(id_AttackButonOpaque,attackButonOpaque.value);

			PlayerPrefs.SetInt(id_SEVolume,(int)seVolume.value);
			SoundManager.SetVolume(seVolume.value);

			PlayerPrefs.Save();
			eventSystem.SetActive(false);

			if(fromTitle)
				SceneManager.LoadScene("Title",LoadSceneMode.Single);
			else
				dungeon.DungeonManager.CloseOption();
		}


		public void RestoreToDefault()
		{
			x_LeftAnalogStickSensitive.value = 50;
			x_RightAnalogStickSensitive.value = 50;
			y_UpAnalogStickSensitive.value = 50;
			y_DownAnalogStickSensitive.value = 50;
			analogStickSize.value= 0.8f;
			analogStickOpaque.value = 0.8f;
			attackButonSize.value = 0.8f;
			attackButonOpaque.value = 0.8f;
			seVolume.value = -11;
		}

		public void	NextPage()
		{
			Page1.SetActive(false);
			Page2.SetActive(true);
		}
		public void	NextPage2()
		{
			Page2.SetActive(false);
			Page3.SetActive(true);
		}
	}
}