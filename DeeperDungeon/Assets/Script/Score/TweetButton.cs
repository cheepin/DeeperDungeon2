using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using moving.player;
namespace score
{
	public class TweetButton : MonoBehaviour
	{
		[SerializeField]
		ScoreDisplay scoreDisplay;
		public void Tweet()
		{
			string difficulty;
			if(scoreDisplay.RecordScore.difficulty==0)
				difficulty = "Normal";
			else
				difficulty = "Hard";
			Application.OpenURL("https://twitter.com/intent/tweet?text=" + WWW.EscapeURL("You've cleared " + difficulty + " Dungeon!   " + DateTime.Now +
				" #DeeperDungeon" ));
		}
	
	}

}