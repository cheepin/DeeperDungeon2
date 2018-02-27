using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using util;
namespace moving.enemy
{
	public class Wind : EnemyWithInRangeAttack 
	{
		protected int electricDamage;
		float attackTime;
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
			Func<int,StatusData,int> func = (dummy1,dummy2) => DamageManager.EnchantAttack(dummy1,dummy2,StatusData.Enchant.Electrical);
			if(statusData.HP>0)
			{
				player.ReceiveDamage(electricDamage +TempPlayerData.Attack,nowDirection,func);
			}
			return ;
		}


		protected override void CopyStatusFromScriptableObject(StatusData statusData,EnemyData scriptableObjectData)
		{
			var spriteManData = scriptableObjectData as WindData;
			statusData.MaxHP   = spriteManData.HP;
			statusData.HP      = spriteManData.HP;
			statusData.Attack  = spriteManData.Attack;
			statusData.Defense = spriteManData.Defense;
			chargeTime = spriteManData.ChargeTime;
			attackTime = spriteManData.AttackTime;
			electricDamage = spriteManData.ElectricDamage;
			ToAttackSight = spriteManData.ToAttackSight;
			moveTime = spriteManData.MoveSpeed;
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

				//---衝突　壁の中で発動しないようにずらす
				int i =0;
				List<Vector2> directionList = new List<Vector2>()
				{
					 Vector2.zero,Vector2.down,Vector2.left,Vector2.right,Vector2.up
				};
				while(i<directionList.Count)
				{
					GetComponent<Collider2D>().enabled = false;
					var colliderWall = Physics2D.Linecast((Vector2)transform.position+new Vector2(0,0.5f),(Vector2)transform.position+directionList[i]/2,blockingLayer);
					GetComponent<Collider2D>().enabled = true;
					if(colliderWall.transform==null)
					{
						child.transform.localPosition =  (Vector3)directionList[i]/2;
						break;
					}
					else
					{
						i++;
					}
				}
				StartCoroutine(CoroutineHelper.Chain(this,CoroutineHelper.DelaySecond(chargeTime,()=>{ if(!NowReceivingDamage){ SoundManager.Wind();child.SetActive(true); } }),
					CoroutineHelper.DelaySecond(attackTime,()=>
					{
						child.SetActive(false);
						stopMoving=false;
						nowAttack = false;
					})

				));
			}
			yield return new WaitForSeconds(1.2f);
		}

		protected override RaycastHit2D[] ThrowCastType(Vector3 position, Vector3 targetPos, int layerMask)
		{
			return Physics2D.BoxCastAll(transform.position, new Vector2(ToAttackSight,ToAttackSight), 0, new Vector2(0, 0), 0, blockingLayer);
			
		}


		

	}
}

