using System;
using System.Collections;
using UnityEngine;
using System.Linq;

namespace moving.enemy
{
	public class SpriteMan : EnemyWithInRangeAttack {

		int enchantAttack;
		ParticleSystem fire;
		GameObject child;
		float fireBallSpeed;
		protected bool useIceSound = false;
		public override bool ReceiveDamage(int attack, util.DirectionHelper.Direction nowDirection,System.Func<int,StatusData,int> damageAction)
		{
			child.SetActive(false);
			bool attackSuccess = base.ReceiveDamage(attack,nowDirection,damageAction);
			return attackSuccess;
		}

		protected override void CopyStatusFromScriptableObject(StatusData statusData,EnemyData scriptableObjectData)
		{
			var spriteManData = scriptableObjectData as SpriteManData;
			statusData.MaxHP   = spriteManData.HP;
			statusData.HP      = spriteManData.HP;
			statusData.Attack  = spriteManData.Attack;
			statusData.Defense = spriteManData.Defense;
			moveTime = spriteManData.MoveSpeed;
			ToAttackSight = spriteManData.RangeDistance;
			chargeTime = spriteManData.ChargeTime;
			enchantAttack =spriteManData.EnchantAttack;
			fireBallSpeed = spriteManData.FireBallSpeed;
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
			yield return new WaitForSeconds(chargeTime);
			StartCoroutine(FireShotTowardPlayer(targetEnd));
			
		}

		void SpriteAttack(GameObject _player,int numEnter)
		{
			//---HP1以上の時だけに限定しないと残りカスが当たるので
			if(statusData.HP>0)
			{
				Func<int,StatusData,int> functor = AttackType();
				player.ReceiveDamage(enchantAttack+TempPlayerData.Attack, nowDirection,functor);
			}
			
		}

		protected virtual Func<int,StatusData,int> AttackType() 
		{
			return (attack,status)=>DamageManager.EnchantAttack(attack,status,StatusData.Enchant.Fire);
		}
		protected virtual Vector3 FirePositioner(util.DirectionHelper.Direction nowdirection,int index,Vector3 target) => target;

		IEnumerator FireShotTowardPlayer(Vector3 targetEnd)
		{
			int j=0;
			if(!useIceSound)
				SoundManager.Explocive();
			else
				SoundManager.Wind();
			while(j++<80 && !NowReceivingDamage)
			{
				var particle = new ParticleSystem.Particle[fire.particleCount];
				fire.GetParticles(particle);
				foreach(int i in Enumerable.Range(0,particle.Length))
				{
					Vector3 newPos = Vector3.MoveTowards(particle[i].position,FirePositioner(nowDirection,j,targetEnd),Time.deltaTime * fireBallSpeed);
					particle[i].position = newPos;

					//if(Mathf.Abs(particle[i].position.x - targetEnd.x) < 1 && Mathf.Abs(particle[i].position.y - targetEnd.y) < 1)
					//	particle[i].remainingLifetime = 0;
				}
				fire.SetParticles(particle,particle.Length);
				yield return null;
			}
			fire.Clear();
			child.SetActive(false);
			yield return new WaitForSeconds(0.3f);

			nowAttack = false;
			stopMoving = false;
			yield return null;
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