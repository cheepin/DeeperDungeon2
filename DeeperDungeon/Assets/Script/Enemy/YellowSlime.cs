﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace moving.enemy
{
	public class YellowSlime : Slime
	{
		protected override void PoisonAttack(GameObject _player,int numEnter)
		{
			var player = _player.GetComponent<moving.player.Player>();
			int downAmount = 20;


			Func<int, StatusData, int> attackEffect1 = DamageManager.CreateStatusDownDelegate(player,0.3f, "DEF↓",0.12f,
				() => player.TempPlayerData.Defense -= downAmount,
				() => player.TempPlayerData.Defense += downAmount);

			attackEffect1 += (attack,stasus)=>DamageManager.SetPoison(player,poisonDamage,attack,stasus);

			if(statusData.HP > 0)
			{
				player.ReceiveDamage(poisonDamage, nowDirection, attackEffect1);
			}
			return;
		}

	}

}