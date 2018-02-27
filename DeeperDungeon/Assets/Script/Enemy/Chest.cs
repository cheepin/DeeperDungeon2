using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using moving.enemy;
using System;
using util;

public class Chest : Enemy {
	[SerializeField]
	int exp=0;

	protected override void Start()
	{
		ActionWhenDie += ()=>SoundManager.LevelUp();
		enemyData.Exp = exp;
		base.Start();
	}

}
