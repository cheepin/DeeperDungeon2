using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using util;
namespace moving.enemy
{
	public class Ghast : EnemyWithInRangeAttack 
	{
		protected float attackTime;
		protected SpriteRenderer spriter;
		protected virtual IEnumerator ActionWhenAttack(GameObject gameObject){yield return null; }
		bool damagedWhenBeforeAttacking;

		public override bool ReceiveDamage(int attack, util.DirectionHelper.Direction nowDirection,System.Func<int,StatusData,int> damageAction)
		{
			//---攻撃中に攻撃を受けたらイベントを発生
			if(nowAttack)
			{
				ActionWhenDamageBeforeAttacking = ()=>
				{
					damagedWhenBeforeAttacking = true;
					spriter.color = MyColor;
					stopMoving = false;
					nowAttack = false;
					ActionWhenDamageBeforeAttacking = null;
					damagedWhenBeforeAttacking = false;

				};
			}
			
			return base.ReceiveDamage(attack,nowDirection,damageAction);
		}

		protected override void Start()
		{
			spriter = GetComponent<SpriteRenderer>();
			base.Start();
		
		}

		void PoisonAttack(GameObject _player,int numEnter)
		{
			var player = _player.GetComponent<moving.player.Player>();
			//---ポイズンダメージのカリー化
			Func<int,StatusData,int> func = (dummy1,dummy2) => DamageManager.SetPoison(player,25,dummy1,dummy2);
			if(statusData.HP>0)
			{
				player.ReceiveDamage(statusData.Attack,nowDirection,func);
			}
			return ;
		}


		protected override void CopyStatusFromScriptableObject(StatusData statusData,EnemyData scriptableObjectData)
		{
			var GhastData = scriptableObjectData as GhastData;
			statusData.MaxHP   = GhastData.HP;
			statusData.HP      = GhastData.HP;
			statusData.Attack  = GhastData.Attack;
			statusData.Defense = GhastData.Defense;
			moveTime = GhastData.MoveSpeed;
			chargeTime = GhastData.ChargeTime;
			attackTime = GhastData.AttackTime;
			ToAttackSight = GhastData.ToAttackSight;
			
		}

		protected override void DoSomethingWhenDetectPlayer(Transform hittedObj)
		{
		}
		
		protected override IEnumerator ActionInRange(Transform hittedObj, Vector2 targetEnd)
		{
			stopMoving=true;
			yield return StartCoroutine(AttackMotion(hittedObj));

		}
		Action ActionWhenDamageBeforeAttacking;
		IEnumerator AttackMotion(Transform hittedObj)
		{
			yield return StartCoroutine(
				CoroutineHelper.Chain(this,CoroutineHelper.DelaySecond(chargeTime,()=>
				{ 
					spriter.color = new Color(0,0,0,0);
					stopMoving=false;
				}),

				CoroutineHelper.DelaySecond(attackTime,()=>
				{
					spriter.color = MyColor;
					animator.SetTrigger("Attack");
					stopMoving=true;
					rb2D.velocity = new Vector2(0,0);
				})

				));
			yield return ActionWhenAttack(hittedObj.gameObject);
			yield return new WaitForSeconds(1.0f);
			//---このメソッドを実行中に攻撃を受けていたら
			if(ActionWhenDamageBeforeAttacking==null)
			{
				nowAttack = false;
				stopMoving=false;
			}
			else
			{
				ActionWhenDamageBeforeAttacking();
			}
		}

		protected override RaycastHit2D[] ThrowCastType(Vector3 position, Vector3 targetPos, int layerMask)
		{
			return Physics2D.BoxCastAll(transform.position, new Vector2(ToAttackSight,ToAttackSight), 0, new Vector2(0, 0), 0, blockingLayer);
			
		}
		


		

	}
}

