using System;
using System.Collections.Generic;
using UnityEngine;

namespace ui
{
	public class LanguageManager
	{
		static public readonly string id_Laungage = "id_Laungage";
		public enum Laungage
		{
			Engligh,Japanese
		}

		static List<string> localizeTagList = new List<string>()
		{
			"<EN>","<JP>"
		};
		static List<string> localizeEndTagList = new List<string>()
		{
			"</EN>","</JP>"
		};
		static readonly string engTag = "<EN>";
		static readonly string japTag = "<JP>";

		


		/// <summary>
		/// ローカライズタグが使われてるstringをパース
		/// </summary>
		/// <param name="translated">パースしたい文字列</param>
		static public string ParseString(string translated)
		{
			//---現在のローカル言語を取得
			int nowLocalizeID =  PlayerPrefs.GetInt(id_Laungage);
			string returnStr="";
			
			try
			{
				//---タグ(<JP>など)で始まる部分を取得、トリム
				string tag = localizeTagList[nowLocalizeID];
				string extractStr = translated.Substring(translated.IndexOf(tag)).Remove(0,tag.Length);
			
				returnStr = extractStr.Remove(extractStr.IndexOf(localizeEndTagList[nowLocalizeID]));
			}
			catch
			{
				Debug.Assert(false,"元の文に問題が有ります");
			}
			return returnStr;
		}
	} 
}
