using UnityEngine;
using System.Collections;
using System;

namespace moving.enemy
{
	public class CyanPumpkin : Pumpkin
	{

		protected override void Start()
		{
			enableMoveWhenAttack = true;
			beamSpeed = 34f;
			base.Start();
		}

		protected override void SpriteAttack(GameObject _player, int numEnter)
		{
			//---HP1以上の時だけに限定しないと残りカスが当たるので
			if(statusData.HP > 0)
			{
				Func<int, StatusData, int> functor = (attack, status) => DamageManager.EnchantAttack(attack, status, StatusData.Enchant.Ice);
				player.ReceiveDamage(EnchantAttack+TempPlayerData.Attack, nowDirection, functor);
			}

		}
	}

}