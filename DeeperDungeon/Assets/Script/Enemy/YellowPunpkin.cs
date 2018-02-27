using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace moving.enemy
{
	public class YellowPunpkin : Pumpkin 
	{
		protected override void Start()
		{
			enableMoveWhenAttack=true;
			base.Start();
		}

		protected override void SpriteAttack(GameObject _player,int numEnter)
		{
			//---HP1以上の時だけに限定しないと残りカスが当たるので
			if(statusData.HP>0)
			{
				Func<int,StatusData,int> functor = (attack,status)=>DamageManager.EnchantAttack(attack,status,StatusData.Enchant.Fire);
				player.ReceiveDamage(EnchantAttack+TempPlayerData.Attack, nowDirection,functor);
			}
			
		}
	}

}