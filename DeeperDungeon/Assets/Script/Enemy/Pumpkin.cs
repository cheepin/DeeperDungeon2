using System;
using System.Collections;
using UnityEngine;
using System.Linq;

namespace moving.enemy
{
	public class Pumpkin : EnemyWithInRangeAttack {

		protected bool enableMoveWhenAttack =false;
		protected int EnchantAttack{get;private set; }
		protected float beamSpeed = 120.0f;
		ParticleSystem fire;
		GameObject child;
		

		public override bool ReceiveDamage(int attack, util.DirectionHelper.Direction nowDirection,System.Func<int,StatusData,int> damageAction)
		{
			fire.Clear(true);
			child.SetActive(false);
			bool success = base.ReceiveDamage(attack,nowDirection,damageAction);
			return success;
		}

		protected override void CopyStatusFromScriptableObject(StatusData statusData,EnemyData scriptableObjectData)
		{
			var spriteManData = scriptableObjectData as PumpkinData;
			statusData.MaxHP   = spriteManData.HP;
			statusData.HP      = spriteManData.HP;
			statusData.Attack  = spriteManData.Attack;
			statusData.Defense = spriteManData.Defense;
			moveTime = spriteManData.MoveSpeed;
			ToAttackSight = spriteManData.ToAttackSight;
			chargeTime = spriteManData.ChargeTime;
			EnchantAttack =spriteManData.EnchantAttack;
		}
			
		protected override void Start()
		{
			child = transform.GetChild(0).gameObject;
			fire = child.GetComponent<ParticleSystem>();
			child.GetComponent<CauseActionParticle>().ActionWhenCorridor = SpriteAttack;
			child.SetActive(false);

			base.Start();
		}

		protected override void DoSomethingWhenDetectPlayer(Transform hittedObj)
		{
			child.GetComponent<CauseActionParticle>().SetTargetObject(hittedObj.gameObject);
		}

		protected override RaycastHit2D[] ThrowCastType(Vector3 position, Vector3 targetPos, int layerMask)
		{
			return Physics2D.LinecastAll(transform.position,targetPos,blockingLayer);
		}

		protected override IEnumerator ActionInRange(Transform hittedObj,Vector2 targetEnd)
		{
			child.SetActive(true);
			fire.trigger.SetCollider(0, hittedObj);
			SoundManager.Charge();
			yield return new WaitForSeconds(chargeTime);
			StartCoroutine(FireShotTowardPlayer(targetEnd));
		}

		protected virtual void SpriteAttack(GameObject _player,int numEnter)
		{
			//---HP1以上の時だけに限定しないと残りカスが当たるので
			if(statusData.HP>0)
			{
				Func<int,StatusData,int> functor = (attack,status)=>DamageManager.EnchantAttack(attack,status,StatusData.Enchant.Electrical);
				player.ReceiveDamage(EnchantAttack, nowDirection,functor);
			}
			
		}

	

		IEnumerator FireShotTowardPlayer(Vector3 targetEnd)
		{
			int j=0;
			SoundManager.Magic();
			while(j++<80 && !NowReceivingDamage)
			{
				if(enableMoveWhenAttack)
				{
					stopMoving = false;
				}
				var particle = new ParticleSystem.Particle[fire.particleCount];
				fire.GetParticles(particle);
				foreach(int i in Enumerable.Range(0,particle.Length))
				{
					Vector3 newPos = Vector3.MoveTowards(particle[i].position,util.DirectionHelper.MapByNowDirection(nowDirection,ToAttackSight,0),Time.deltaTime*beamSpeed);
					particle[i].position = new Vector3(newPos.x,newPos.y,-2);
				}
				fire.SetParticles(particle,particle.Length);
				yield return null;
			}
			fire.Clear();
			child.SetActive(false);
			yield return new WaitForSeconds(0.3f);
			nowAttack = false;
			stopMoving = false;
		}

	

		protected override void AttackPlayer()
		{
			if(player !=null)
			{
				if(statusData.HP>0)
					player.ReceiveDamage(statusData.Attack, nowDirection,DamageManager.NormalAttack);
			}

		}

		protected override void Update()
		{
			base.Update();
		}

	}
}