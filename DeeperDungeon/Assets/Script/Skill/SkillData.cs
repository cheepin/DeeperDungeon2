using System;
using UnityEngine;
using moving.player;
using skill.spell;

namespace skill
{
	[Serializable]
	public class SkillData : ScriptableObject 
	{
		public string skillName;
		public int spellCost;
		public int manaCost;
		public int learningCost;
		public int maxLevel;
		public bool activeSkill;
		public bool keyStoneSkill;
		[TextArea(12,3)]
		public string description;
		public	Func<Player,int,bool> spell;

		public void SetSpell(string skillName)
		{
			try
			{
				var k = Activator.CreateInstance(Type.GetType("skill.spell."+skillName)) as BaseSpell;
				spell = k.spellAction;
			}
			catch
			{
				Debug.Assert(false,$"アサインミス：{skillName}");
			}
		}

	}	

	

}