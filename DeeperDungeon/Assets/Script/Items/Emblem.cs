using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using skill;
using item;
public class Emblem : Item {

	[SerializeField]
	string skillName = "";
	[SerializeField]
	SpriteAtlas spriteAtlas = null;
	EmblemData emblemData;
	GameObject expOrb;
	// Use this for initialization
	public override void SetUp(ScriptableObject _data)
	{
		expOrb = ResourceLoader.Instance.expOrb;
		var data = _data as EmblemData;
		emblemData = data;
		skillName = data.skillName;
		Sprite sprite = ResourceLoader.Instance.EmblemTexturePacker[emblemData.skillName];
		GetComponentInChildren<SpriteRenderer>().sprite = sprite;
	}

	protected override void ItemAction(GameObject getter)
	{
		int expAmount = SkillManager.SetSkillListFromEmblem(emblemData.skillName);
		if(expAmount!=0)
		{
			SoundManager.SkillActivate();
			var _expOrb = Instantiate(expOrb,transform.position,Quaternion.identity);
			_expOrb.GetComponentInChildren<ExpOrb>().Exp = expAmount;
		}
		else
		{
			SoundManager.Get();
		}
	}

}
