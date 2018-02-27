using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using moving;
namespace skill
{

	public class TrapSpell : EffectObject
	{
		protected override void InvokeEffect(Collider2D collider2D)
		{
			var enemy = collider2D.GetComponent<moving.enemy.Enemy>();
			if(listClearFlag)
					enemyInstanceIdList.Clear();

			if(!enemyInstanceIdList.Any(X => X == enemy.GetInstanceID()))
			{
				enemyInstanceIdList.Add(enemy.GetInstanceID());
				whenCollisionAction(Caster,collider2D.gameObject.GetComponent<MovingObject>());
				
				listClearFlag = false;
			}
			
		}
		bool listClearFlag = true;
		List<int> enemyInstanceIdList = new List<int>();

		protected override void Start()
		{
			StartCoroutine(util.CoroutineHelper.DelaySecondLoop(0.75f,-1,()=>listClearFlag = true));
			StayEffect = true;
			base.Start();
		}

	} 

}
