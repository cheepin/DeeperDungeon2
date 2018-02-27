using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace skill
{
	public class SkillDataPacker : MonoBehaviour, utilEditor.IPacker<SkillDataDict>
	{
		[SerializeField]
		List<SkillDataDict> dataList;
		public List<SkillData> SkillList{get;private set;}
		public SkillData this[string name]=> skillDict!=null ? skillDict[name] : CreateDict()[name] ;
		public string TargetPath
		{
			get
			{
				return targetPath;
			}

			set
			{
				targetPath = value;
			}
		}

		Dictionary<string,SkillData> skillDict;
		[SerializeField]
		string targetPath;


		public List<SkillDataDict> DataList
		{
			get
			{
				return dataList ?? (dataList = new List<SkillDataDict>());
			}

			set
			{
				dataList = value;
			}
		}

		public string DatalistFieldName => dataList.ToString();

		
		Dictionary<string,SkillData> CreateDict()
		{
			skillDict = new Dictionary<string,SkillData>();
			DataList.ForEach((X)=>skillDict.Add(X.name,X.skillData));

			//---skillListの作成
			SkillList = new List<SkillData>();
			DataList.ForEach((X)=>SkillList.Add(X.skillData));
			//---スペルの登録
			foreach(SkillData skillData in skillDict.Select((elem)=>elem.Value))
				skillData.SetSpell(skillData.skillName);

			return skillDict;
		}
	}

	[System.Serializable]
	public struct SkillDataDict
	{
		public string name;
		[SerializeField]
		public SkillData skillData;
	}
}

namespace utilEditor
{
	public interface IPacker<T>
	{
		List<T> DataList
		{
			get; set;
		}
		//---DataListのフィールド名を入れる ex:"dataList"
		string DatalistFieldName
		{
			get;
		}
		string TargetPath
		{
			get; set;
		}


	} 
}