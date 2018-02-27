using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace moving.enemy
{

	public class YellowGhast : Ghast
	{

		protected override void AttackPlayer()
		{
			if(player != null&&statusData.HP>0)
			{
				int decreaseAmount = 5;
				var damageAffect = DamageManager.CreateStatusDownDelegate(player,0.15f,"DEF↓",0.18f,()=>player.TempPlayerData.Defense-=decreaseAmount, ()=>player.TempPlayerData.Defense+=decreaseAmount);
				damageAffect += DamageManager.NormalAttack;
				player.ReceiveDamage(statusData.Attack, nowDirection,damageAffect);
			}

		}
	}

}