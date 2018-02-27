using UnityEngine;
using System.Collections;
using util;
namespace moving.enemy
{
	public class YellowSkeletonWarrior : SkeletonWarrior
	{
		protected override void Start()
		{
			base.Start();
		}

		protected override IEnumerator ActionInRange(Transform hittedObj, Vector2 targetEnd)
		{
			if(statusData.HP>0)
			{
				stopMoving = false;

				StartCoroutine(CoroutineHelper.Chain(this,
							
					CoroutineHelper.DelaySecond(0.81f,()=>{inverseMoveTime = 1.0f/0.063f; }),
					CoroutineHelper.DelaySecond(chargeTime,()=>
					{
						stopMoving = true;
						rb2D.velocity = new Vector2(0,0);
						animator.SetTrigger("Attack");
					}),
					CoroutineHelper.DelaySecond(0.1f,()=>
					{
						 if(!NowReceivingDamage)
						 {
							bradeAttack.ParentDirection = nowDirection;
							bradeAttack.gameObject.SetActive(true);
							SoundManager.SwordBrade();
							stopMoving = true;
						 }
						
					}),

					CoroutineHelper.DelaySecond(attackTime,()=>
					{
						nowAttack = false;
						stopMoving=false;
						inverseMoveTime = 1.0f/moveTime;
					})

				));
			}
			yield return new WaitForSeconds(0.2f);
		}
		
	}

}