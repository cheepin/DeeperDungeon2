using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace score
{
	public class ScoreDisplay : MonoBehaviour
	{
		
		public RecordScore RecordScore{get;private set;}
		private void OnEnable()
		{
			Text text = GetComponentInChildren<Text>();
			
			ScoreManager.SaveScore(GameObject.FindGameObjectWithTag("Player").GetComponent<moving.player.Player>());
			RecordScore = ScoreManager.Instance.GetRecordScore;
			var textList = PrintScore(text);
			string difficulty;

			if(RecordScore.difficulty==0)
				difficulty = "Normal";
			else
				difficulty = "Hard";
			text.text = "You've cleared " + difficulty + " Dungeon!" +  System.Environment.NewLine+ System.Environment.NewLine;
			foreach(var statusText in textList)
			{
				text.text += statusText + System.Environment.NewLine+ System.Environment.NewLine;

			}
		}

		List<string> PrintScore(Text text)
		{
			string space = "  ";
			List<string> textList = new List<string>()
			{
				nameof(RecordScore.CurrentLevel) + space + RecordScore.CurrentLevel,
				nameof(RecordScore.MaxHP) + space + RecordScore.MaxHP,
				nameof(RecordScore.MaxMana) + space + RecordScore.MaxMana,
				nameof(RecordScore.Attack) + space + RecordScore.Attack,
				nameof(RecordScore.Defense) + space + RecordScore.Defense
			};
			return textList;

		}

		


	} 

}
