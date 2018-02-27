using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace moving.enemy
{

	public class IceSpriteMan : SpriteMan
	{
		protected override void Start()
		{
			useIceSound = true;
			base.Start();
		}

		protected override Vector3 FirePositioner(util.DirectionHelper.Direction nowdirection,int i ,Vector3 pos) 
		{
			int newX = (i-40)/10;
			if(nowdirection== util.DirectionHelper.Direction.Down || nowdirection== util.DirectionHelper.Direction.Up)
				return new Vector3(pos.x+newX,pos.y,-2);
			else 
				return new Vector3(pos.x,pos.y+newX,-2);
		}

		protected override System.Func<int,StatusData,int> AttackType() => (attack,status)=>DamageManager.EnchantAttack(attack,status,StatusData.Enchant.Ice);




	}

}