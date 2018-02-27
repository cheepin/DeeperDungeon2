using UnityEngine;
using System.Collections;
using System;
namespace moving.enemy
{

	public class Wind_Lv4 : Wind
	{

		protected override void PoisonAttack(GameObject _player,int numEnter)
		{
			var player = _player.GetComponent<moving.player.Player>();
			//---ポイズンダメージのカリー化
			Func<int,StatusData,int> func = (dummy1,dummy2) => DamageManager.SetPoison(player,electricDamage,dummy1,dummy2);
			int decreaseAttack = 25;
			func += DamageManager.CreateStatusDownDelegate(player,0.3f,"STR↓",0.18f,()=>player.TempPlayerData.Attack-=decreaseAttack,()=>player.TempPlayerData.Attack+=decreaseAttack);
			if(statusData.HP>0)
			{
				player.ReceiveDamage(statusData.Attack+TempPlayerData.Attack,nowDirection,func);
			}
			return ;
		}
	} 
}
