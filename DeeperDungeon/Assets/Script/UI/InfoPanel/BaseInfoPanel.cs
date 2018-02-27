using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using moving.player;
using System;
using CH = util.CoroutineHelper;
namespace ui
{
	public class BaseInfoPanel : MonoBehaviour
	{
		protected Player player = null;
		const string id_haveActiveSkillSetInfo = "id_haveActiveSkillSetInfo ";
		/// <summary>
		/// ConditionToActivateが発生するデリゲートを指定
		/// たとえばレベルが上がった時など
		/// </summary>
		/// <param name="activate">この引数を登録したいデリゲートに登録 </param>
		protected virtual void RergisterAction(Action<string> activate)	{}

		/// <summary>
		/// コールバックされた時の条件を設定
		/// </summary>
		/// <returns></returns>
		protected virtual bool ConditionToActivate(string content){throw new  NotImplementedException();}

		/// <summary>
		/// PlayerPrefに登録するID名
		/// </summary>
		protected virtual string PrefID {get;set; }

		void Start()
		{
			player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
			RergisterAction(Activate);
			gameObject.SetActive(false);
		}

		void Activate(string content)
		{
			//---今まで説明を受けた事がなかったら発動
			int haveActiveInfo = PlayerPrefs.GetInt(PrefID);
			if(haveActiveInfo != 1 && ConditionToActivate(content))
			{
				var text = gameObject.GetComponentInChildren<Text>().text;
				gameObject.GetComponentInChildren<Text>().text = LanguageManager.ParseString(text);
				gameObject.SetActive(true);
				//---説明を受けた事を登録 今後現れない
				PlayerPrefs.SetInt(PrefID, 1);

				Time.timeScale = 0;
			}
		}

		


	} 
}