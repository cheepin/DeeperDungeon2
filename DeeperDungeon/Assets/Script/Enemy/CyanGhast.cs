using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Player = moving.player.Player;
namespace moving.enemy
{
	public class CyanGhast : BlueGhast
	{
		protected override void Start()
		{
			particleTime = 0.5f;
			base.Start();
		}

		protected override void PoisonAttack(GameObject _player,int numEnter)
		{
			var player = _player.GetComponent<Player>();
			//---ポイズンダメージのカリー化
			int decreaseAmount = 15;
			Func<int,StatusData,int> func = DamageManager.CreateStatusDownDelegate(player,0.25f,"STR↓",0.18f,()=>player.TempPlayerData.Attack-= decreaseAmount,()=>player.TempPlayerData.Attack+= decreaseAmount);
			func += (attack,status)=>DamageManager.SetPoison(player,35,0,status);
			if(statusData.HP>0)
			{
				player.ReceiveDamage(32,nowDirection,func);
			}
			return ;
		}
	}

}