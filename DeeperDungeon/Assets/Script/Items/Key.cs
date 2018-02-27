using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using skill;
namespace item
{
	public class Key : Emblem
	{
		[SerializeField]
		EmblemData keyData=null;
		// Use this for initialization
		void Start()
		{
			SetUp(keyData);
		}

		// Update is called once per frame
		void Update()
		{

		}

		protected override void ItemAction(GameObject getter)
		{
			int dummy = 0;
			SkillManager.SetSkillListFromEmblem(keyData.skillName);
			SkillManager.LevelUpStatus("Key",ref dummy,null,true);

			SoundManager.Get();
		}
	}

}