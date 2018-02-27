using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using moving.player;

namespace moving.enemy
{

	public class BlueSlime : Slime
	{
		protected override void PoisonAttack(GameObject _player,int numEnter)
		{
			var player = _player.GetComponent<moving.player.Player>();
			int downAmount = 50;
			Func<int,StatusData,int> func = DamageManager.CreateStatusDownDelegate(player,0.3f,"STR↓",0.12f,
				()=>player.TempPlayerData.Attack -=downAmount,
				()=>player.TempPlayerData.Attack +=downAmount);
			func += (attack,stasus)=>DamageManager.SetPoison(player,poisonDamage,attack,stasus);



			if(statusData.HP>0)
			{
				player.ReceiveDamage(statusData.Attack,nowDirection,func);
			}
			return ;
		}
	}

}
