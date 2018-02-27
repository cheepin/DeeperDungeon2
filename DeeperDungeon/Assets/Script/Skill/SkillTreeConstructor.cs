using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace skill
{
	public class SkillTreeConstructor : MonoBehaviour
	{

		[SerializeField]
		Vector3 buttonStartPos = new Vector3();
		[SerializeField]
		int horizontalGap = 4;
		[SerializeField]
		int verticalGap = 4;
		[SerializeField]
		int horizontalMax = 4;
		[SerializeField]
		int verticalMax = 4;
		[SerializeField]
		Vector3 buttonSize = new Vector3();
		[SerializeField]
		Vector3 textPos = new Vector3();
		[SerializeField]
		int fontSize = 10;
		[SerializeField]
		Vector3 infoButtonPos = new Vector3();
		[SerializeField]
		int infoButtonSize = 4;


		// Use this for initialization
		void Start()
		{


			List<String> nameList = SkillManager.GetSkillButtonList();

			//---ボタンオブジェクトをロード
			List<GameObject> buttonList = new List<GameObject>();
			nameList.ForEach((skill) =>
			{
				var skillButton = Resources.Load<GameObject>("SkillTreeButton/SkillButton");
				buttonList.Add(skillButton);
			});

			//---インフォボタンをロード・サイズ設定
			var infobutton = Resources.Load<GameObject>("SkillTreeButton/SkillInfoButton");
			infobutton.GetComponent<RectTransform>().sizeDelta = new Vector3(infoButtonSize, infoButtonSize, 1);

			//---ボタンの数によってページ数を決定
			int buttonofNumberPerPage = horizontalMax * verticalMax;
			int pageSize = (buttonList.Count / buttonofNumberPerPage) + 1;
			var parent = GetComponent<RectTransform>();
			float pageWidth = parent.rect.width;

			parent.sizeDelta = new Vector2(pageWidth * pageSize, parent.rect.height);

			//---設置に使う変数を初期化
			var positionerFromTopLeft = buttonStartPos;
			int horizontalButtonCount = 0;
			int verticalButtonCount = 0;
			int currentPage = 0;
			//---インスタンス化
			nameList.ForEach((skillname) =>
			{
				//ロード
				GameObject button = Resources.Load<GameObject>("SkillTreeButton/SkillButton");

				//---インスタンス処理
				var newSkillButton = Instantiate(button, positionerFromTopLeft, Quaternion.identity, transform).GetComponent<SkillButton>();
				var newInfoButton = Instantiate(infobutton, positionerFromTopLeft + infoButtonPos, Quaternion.identity, newSkillButton.transform);

				//---セットアップ
				newSkillButton.SetUpButton(SkillManager.GetSkillData<SkillData>(skillname), true);

				//---スキルボタンのサイズ、テキストの設定
				newSkillButton.transform.GetChild(0).position += textPos;
				newSkillButton.GetComponentInChildren<Text>().fontSize = fontSize;

				var rectTransform = newSkillButton.GetComponent<RectTransform>();
				rectTransform.sizeDelta = buttonSize;
				rectTransform.anchoredPosition = positionerFromTopLeft;

				//---インフォボタンのサイズ、テキスト設定
				newInfoButton.GetComponent<RectTransform>().anchoredPosition = infoButtonPos;
				newInfoButton.GetComponent<Button>().onClick.AddListener(newSkillButton.DisplayInfo);

				//---横列を整地
				if(++horizontalButtonCount < horizontalMax)
				{
					positionerFromTopLeft += new Vector3(horizontalGap, 0, 0);
				}
				//---行変更
				else if(++verticalButtonCount < verticalMax)
				{
					positionerFromTopLeft = new Vector3(buttonStartPos.x + (pageWidth * currentPage), buttonStartPos.y - verticalGap * verticalButtonCount, 0);
					horizontalButtonCount = 0;
				}
				//---ページ変更
				else if(verticalButtonCount >= verticalMax)
				{
					currentPage++;
					verticalButtonCount = 0;
					horizontalButtonCount = 0;
					positionerFromTopLeft = new Vector3(buttonStartPos.x + (pageWidth * currentPage), buttonStartPos.y, 0);
				}
			});

			//---フェードイン
			StartCoroutine(FadeIn.StartFadeIn());
		}

	}

}