using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using util;
namespace moving.enemy
{
	public class SkeletonWarrior : EnemyWithInRangeAttack 
	{
		protected float attackTime;	
		protected int bradeDamage;
		protected BradeAttack bradeAttack;
		protected string bradeAttackPath="SkeletonWarriorBrade";

		public override bool ReceiveDamage(int attack, util.DirectionHelper.Direction nowDirection,System.Func<int,StatusData,int> damageAction)
		{
			return base.ReceiveDamage(attack,nowDirection,damageAction);
		}


		protected override void Start()
		{
			StartCoroutine(CoroutineHelper.WaitForEndOfFrame(()=>
			{
				var brade = Instantiate(ResourceLoader.GetResouce(bradeAttackPath),transform);
				bradeAttack = brade.GetComponent<BradeAttack>();
				bradeAttack.ActionWhenCorridor = BradeAttack;
				bradeAttack.gameObject.SetActive(false);

			}));
			
			base.Start();	
		}

		protected void BradeAttack(GameObject _player)
		{
			//---ポイズンダメージのカリー化
			if(statusData.HP>0)
			{
				_player.GetComponent<MovingObject>()?.ReceiveDamage(bradeDamage+TempPlayerData.Attack,nowDirection,DamageManager.NormalAttack);
			}
			return ;
		}


		protected override void CopyStatusFromScriptableObject(StatusData statusData,EnemyData scriptableObjectData)
		{
			var data = scriptableObjectData as SkeletonWarriorData;
			statusData.MaxHP   = data.HP;
			statusData.HP      = data.HP;
			statusData.Attack  = data.Attack;
			statusData.Defense = data.Defense;
			chargeTime = data.ChargeTime;
			attackTime = data.AttackTime;
			bradeDamage = data.BradeAttack;
			ToAttackSight = data.ToAttackSight;
			moveTime = data.MoveSpeed;

		
		}

		protected override void DoSomethingWhenDetectPlayer(Transform hittedObj)
		{
		}

		protected override bool ConditionStopDetectPlayer()=>!detectPlayer;

		protected override IEnumerator ActionInRange(Transform hittedObj, Vector2 targetEnd)
		{
			if(statusData.HP>0)
			{
				animator.SetTrigger("Attack");

				StartCoroutine(CoroutineHelper.Chain(this,
					CoroutineHelper.DelaySecond(chargeTime,()=>
					{
						 if(!NowReceivingDamage)
						 {
							bradeAttack.ParentDirection = nowDirection;
							bradeAttack.gameObject.SetActive(true);
							SoundManager.SwordBrade();
						 }
					}),

					CoroutineHelper.DelaySecond(attackTime,()=>
					{
						nowAttack = false;
						stopMoving=false;

					})

				));
			}
			yield return new WaitForSeconds(0.2f);
		}



		protected override RaycastHit2D[] ThrowCastType(Vector3 position, Vector3 targetPos, int layerMask)
		{
			return Physics2D.BoxCastAll(transform.position,new Vector2(1,1),0, DirectionHelper.GetDirection(nowDirection),ToAttackSight,blockingLayer);
			
		}


		

	}
}
