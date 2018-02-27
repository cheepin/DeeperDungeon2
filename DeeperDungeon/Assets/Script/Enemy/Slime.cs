using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using util;
namespace moving.enemy
{
	public class Slime : EnemyWithInRangeAttack 
	{
		float attackTime;
		protected int poisonDamage;
		GameObject child;
		ParticleSystem patriclesys;

		public override bool ReceiveDamage(int attack, util.DirectionHelper.Direction nowDirection,System.Func<int,StatusData,int> damageAction)
		{
			patriclesys?.Clear(true);
			child.SetActive(false);
			
			return base.ReceiveDamage(attack,nowDirection,damageAction);
		}

		protected override void Start()
		{
			patriclesys = GetComponentInChildren<ParticleSystem>();
			GetComponentInChildren<CauseActionParticle>().ActionWhenCorridor = PoisonAttack;
			child = transform.GetChild(0).gameObject;
			child.SetActive(false);
			base.Start();
		}

		protected virtual void PoisonAttack(GameObject _player,int numEnter)
		{
			var player = _player.GetComponent<moving.player.Player>();
			//---ポイズンダメージのカリー化
			Func<int,StatusData,int> func = (dummy1,dummy2) => DamageManager.SetPoison(player,poisonDamage,dummy1,dummy2);
			if(statusData.HP>0)
			{
				player.ReceiveDamage(poisonDamage,nowDirection,func);
			}
			return ;
		}


		protected override void CopyStatusFromScriptableObject(StatusData statusData,EnemyData scriptableObjectData)
		{
			var spriteManData = scriptableObjectData as SlimeData;
			statusData.MaxHP   = spriteManData.HP;
			statusData.HP      = spriteManData.HP;
			statusData.Attack  = spriteManData.Attack;
			statusData.Defense = spriteManData.Defense;
			moveTime = spriteManData.MoveSpeed;
			chargeTime = spriteManData.ChargeTime;
			attackTime = spriteManData.AttackTime;
			poisonDamage = spriteManData.PoisonDamage;
			ToAttackSight = spriteManData.ToAttackSight;
			
		}

		protected override void DoSomethingWhenDetectPlayer(Transform hittedObj)
		{
			child.GetComponent<CauseActionParticle>().SetTargetObject(hittedObj.gameObject);
		}

		protected override bool ConditionStopDetectPlayer()=>!nowAttack;

		protected override IEnumerator ActionInRange(Transform hittedObj, Vector2 targetEnd)
		{
			if(statusData.HP>0)
			{
				animator.SetTrigger("Attack");
				
				StartCoroutine(CoroutineHelper.Chain(this,CoroutineHelper.DelaySecond(chargeTime,()=>{
					if(!NowReceivingDamage||statusData.HP>0)
					{
						SoundManager.Bubble();
						child.SetActive(true); 
					}}),
					CoroutineHelper.DelaySecond(attackTime,()=>
					{
						child.SetActive(false);
						nowAttack = false;
						stopMoving=false;

					})

				));
			}
			yield return new WaitForSeconds(0.2f);
		}

		protected override RaycastHit2D[] ThrowCastType(Vector3 position, Vector3 targetPos, int layerMask)
		{


			return Physics2D.BoxCastAll(transform.position, new Vector2(ToAttackSight,ToAttackSight), 0, new Vector2(0, 0), 0, blockingLayer);
			
		}


		

	}




}

