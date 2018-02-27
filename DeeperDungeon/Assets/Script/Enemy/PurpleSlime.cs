using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace moving.enemy
{
	public class PurpleSlime : Slime
	{

		protected override void PoisonAttack(GameObject _player,int numEnter)
		{
			var player = _player.GetComponent<moving.player.Player>();
			
			//int downAmount = 20;
			// attackEffect1 = DamageManager.CreateStatusDownDelegate(player,0.3f, "DEF↓",0.12f,
			//	() => player.TempPlayerData.Defense -= downAmount,
			//	() => player.TempPlayerData.Defense += downAmount);
			
			Func<int, StatusData, int> attackEffect1;
			attackEffect1 = (attack,stasus)=>DamageManager.SetPoison(player,poisonDamage,poisonDamage,stasus);

			float downMoveSpeed = 3.5f;
			//---既にMovingSpeedが変わっていたら発動しない
			if(player.TempPlayerData.MovingSpeed==0)
				attackEffect1 += DamageManager.CreateStatusDownDelegate(player,0.3f, "MOV↓",0.28f,
					() => player.TempPlayerData.MovingSpeed -= downMoveSpeed, 
					() => player.TempPlayerData.MovingSpeed += downMoveSpeed); 

			

			if(statusData.HP > 0)
			{
				player.ReceiveDamage(poisonDamage, nowDirection, attackEffect1);
			}
			return;
		}
	}

}