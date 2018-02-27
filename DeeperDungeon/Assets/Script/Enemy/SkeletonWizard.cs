using System.Collections;
using UnityEngine;
using System.Linq;
using CH = util.CoroutineHelper;

namespace moving.enemy
{
	public class SkeletonWizard : EnemyWithInRangeAttack {

		protected int enchantAttack;
		protected int enchantTime = 40;
		ParticleSystem fire;
		GameObject child;

		protected override bool ConditionToProceedToActionRange(RaycastHit2D hittedObj)
		{
			var _transform = hittedObj.transform;
			//---敵でかつまだ魔法を受けてない場合
			if(_transform.CompareTag("Enemy"))
			{
				var friendEnemy = _transform.GetComponent<Enemy>();
				return friendEnemy.statusData.HP != friendEnemy.statusData.MaxHP ? true:false;
			}
			return false;
		}
		


		public override bool ReceiveDamage(int attack, util.DirectionHelper.Direction nowDirection,System.Func<int,StatusData,int> damageAction)
		{
			fire.Clear(true);
			child.SetActive(false);
			
			return base.ReceiveDamage(attack,nowDirection,damageAction);
		}

		protected override void CopyStatusFromScriptableObject(StatusData statusData,EnemyData scriptableObjectData)
		{
			var data = scriptableObjectData as SkeletonWizardData;
			statusData.MaxHP   = data.HP;
			statusData.HP      = data.HP;
			statusData.Attack  = data.Attack;
			statusData.Defense = data.Defense;
			moveTime = data.MoveSpeed;
			ToAttackSight = data.ToAttackSight;
			chargeTime = data.ChargeTime;
			enchantAttack =data.HealAmount;
		}
			
		protected override void Start()
		{
			supportType = true;
			scanInterval = 0.5f;
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
			GetComponent<Collider2D>().enabled = false;
			var  hittedObj =  Physics2D.BoxCastAll(transform.position,new Vector2(ToAttackSight,ToAttackSight),0, util.DirectionHelper.GetDirection(nowDirection),0,blockingLayer);
			GetComponent<Collider2D>().enabled = true;
			return hittedObj;
		}

		protected override IEnumerator ActionInRange(Transform hittedObj,Vector2 targetEnd)
		{
			if(hittedObj != null)
			{
				stopMoving = true;
				nowAttack = true;
				targetFriend = hittedObj.gameObject;
				child.GetComponent<CauseActionParticle>().SetTargetObject(targetFriend);
				child.SetActive(true);
				fire.trigger.SetCollider(0, hittedObj);

				yield return new WaitForSeconds(chargeTime);

				yield return StartCoroutine(FireShotTowardPlayer(targetEnd));
				stopMoving = false;
				nowAttack = false;
				yield return new WaitForSeconds(1.5f);
			}
		}

		protected virtual void SpriteAttack(GameObject _player,int numEnter)
		{
			//---HP1以上の時だけに限定しないと残りカスが当たるので
			if(statusData.HP>0)
			{
				var k = _player.GetComponent<Enemy>();
				k?.ReceiveHealing(enchantAttack,(amount)=>k.statusData.HP+=amount , nowDirection,DamageManager.NormalAttack);
			}
			
		}

	
		
		IEnumerator FireShotTowardPlayer(Vector3 targetEnd)
		{
			animator.SetTrigger("Attack");
			SoundManager.Charge();
			yield return new WaitForSeconds(chargeTime);
			SoundManager.Magic();
			yield return  StartCoroutine(util.CoroutineHelper.DelaySecondLoop(0.01f,enchantTime,()=>
			{
				var particle = new ParticleSystem.Particle[fire.particleCount];
				fire.GetParticles(particle);
				foreach(int i in Enumerable.Range(0,particle.Length))
				{
					Vector3 newPos = Vector3.MoveTowards(particle[i].position,targetFriend.transform.position + new Vector3(0,0.1f) ,Time.deltaTime*78.0f);
					particle[i].position = new Vector3(newPos.x,newPos.y,-2);
					particle[i].startSize = 1.7f;
				}

				fire.SetParticles(particle,particle.Length);
			}));
			fire.Clear();
			child.SetActive(false);
			yield return new WaitForSeconds(1.0f);
		}

	

		protected override void AttackPlayer()
		{
			if(player !=null)
			{
				if(statusData.HP>0)
					player.ReceiveDamage(statusData.Attack, nowDirection,DamageManager.NormalAttack);
			}

		}

		protected void FlashColor(Enemy k)
		{
			bool isRed=false;

			k.StartCoroutine(CH.DelaySecondLoop(0.35f,-1,()=>
			{
				if(isRed)
					k.GetComponent<SpriteRenderer>().color = k.MyColor;
				else
					k.GetComponent<SpriteRenderer>().color = Color.red;
				isRed = !isRed;
			} ));
		}
		
	}
}