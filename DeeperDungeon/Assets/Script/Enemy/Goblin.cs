using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace moving.enemy
{
	public class Goblin : EnemyWithInRangeAttack 
	{
		float coolDownTime;
		float dashSpeed;
		float dashTime;
		protected override void CopyStatusFromScriptableObject(StatusData statusData,EnemyData scriptableObjectData)
		{
			var GoblinData = scriptableObjectData as GoblinData;
			statusData.MaxHP   = GoblinData.HP;
			statusData.HP      = GoblinData.HP;
			statusData.Attack  = GoblinData.Attack;
			statusData.Defense = GoblinData.Defense;
			coolDownTime = GoblinData.CoolDownTime;
			dashSpeed = GoblinData.DashSpeed;
			chargeTime = GoblinData.ChargeTime;
			ToAttackSight = 15;
			moveTime = GoblinData.MoveSpeed;
			dashTime = GoblinData.DashTime;
		}


		protected override bool ConditionStopDetectPlayer()=>!nowAttack;

		protected override RaycastHit2D[] ThrowCastType(Vector3 position, Vector3 targetPos, int layerMask)=>
			Physics2D.BoxCastAll(transform.position, new Vector2(8,8), 0, new Vector2(0, 0), 0, blockingLayer);

		//突進攻撃
		protected override IEnumerator ActionInRange(Transform hittedObj, Vector2 targetEnd)
		{
			stopMoving=true;
			nowAttack = true;
			StartCoroutine(util.CoroutineHelper.DelaySecondLoopWithAfterAction(chargeTime/15,15,()=>{GetComponent<SpriteRenderer>().color -= new Color(0,0.1f,0.1f,0);},
				()=>
				{
					stopMoving=false;inverseMoveTime = 1f/dashSpeed;
					StartCoroutine(util.CoroutineHelper.DelaySecond(dashTime,()=>
					{
						inverseMoveTime = 1f/moveTime;
						GetComponent<SpriteRenderer>().color = MyColor;
						StartCoroutine(util.CoroutineHelper.DelaySecond(coolDownTime,()=>nowAttack=false));

					}));
				}));
			yield return null;
		}


	}
}

