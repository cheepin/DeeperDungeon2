using UnityEngine;
using System.Collections;
using util;

namespace moving.enemy
{
	public class CyanSkeletonWarrior : SkeletonWarrior
	{

		protected override void Start()
		{
			bradeAttackPath = "CyanSkeletonWarriorBrade";
			base.Start();
		}

		protected override IEnumerator ActionInRange(Transform hittedObj, Vector2 targetEnd)
		{
			if(statusData.HP>0)
			{
				

				StartCoroutine(CoroutineHelper.Chain(this,
					CoroutineHelper.DelaySecond(0.45f,()=>animator.SetTrigger("Attack")),
					CoroutineHelper.DelaySecond(chargeTime,()=>
					{
						 if(!NowReceivingDamage)
						 {
							bradeAttack.ParentDirection = nowDirection;
							bradeAttack.GetComponent<MoveToward>().NowDirection = nowDirection;
							bradeAttack.gameObject.SetActive(true);
							SoundManager.SwordBrade();
						 }
					}),

					CoroutineHelper.DelaySecond(attackTime,()=>
					{
						nowAttack = false;
						stopMoving=false;
						bradeAttack.transform.localPosition = new Vector3(0,0,-2);
						bradeAttack.GetComponent<SpriteRenderer>().color = Color.white;
					})

				));
			}
			yield return new WaitForSeconds(0.2f);
		}
	}

}