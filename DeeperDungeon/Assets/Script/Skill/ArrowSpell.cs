using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace skill
{
	using moving;
	public class ArrowSpell : EffectObject
	{
		
		// Use this for initialization
		protected override void Start()
		{
			base.Start();
			StartCoroutine(util.CoroutineHelper.DelaySecond(1.0f,()=>destroyOrder = ()=> {Destroy(gameObject);destroyOrder=null; }));	
		}
		Action destroyOrder;
		private void LateUpdate()
		{
			destroyOrder?.Invoke();
			
		}


		List<int> hittedEnemyIdLIst = new List<int>();
		protected override void InvokeEffect(Collider2D collision)
		{
			//---敵のIDがキャッシュされてなかった時だけ発動するパターン
			//---それぞれの敵に１回だけ当たるようにするため
			var enemyInstanceID = collision.GetComponent<moving.enemy.Enemy>().GetInstanceID();
			if(!hittedEnemyIdLIst.Any(X=>X==enemyInstanceID))
			{
				hittedEnemyIdLIst.Add(enemyInstanceID);
				whenCollisionAction(Caster,collision.GetComponent<MovingObject>());
			}
		}

		
	} 
}
