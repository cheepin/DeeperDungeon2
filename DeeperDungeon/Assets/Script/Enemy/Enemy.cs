using System;
using System.Linq;

using Random = UnityEngine.Random;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Direction = util.DirectionHelper.Direction;
using CoroutineHelper = util.CoroutineHelper;
using moving.player;
//---約束
//---永続的なコルーチンは必ずWhile(!dead)にする

namespace moving.enemy
{
	public class Enemy : MovingObject
	{

		[SerializeField]
		protected EnemyData enemyData = null;
		[NonSerialized]
		public StatusData statusData;
		public Color MyColor{get;private set; }
		protected Player player;
		protected bool detectPlayer = false;
		protected bool supportType = false;
		protected GameObject targetFriend = null;
		protected SpriteRenderer sprite;
		
		bool nowHealing=false;
		GameObject expOrb;
		List<Action> behaviorWhenDamaged =new List<Action>();

		protected bool NowReceivingDamage{get;private set; } = false;


		protected virtual void CopyStatusFromScriptableObject(StatusData statusData, enemy.EnemyData scriptableObjectData)
		{
			statusData.MaxHP = scriptableObjectData.HP;
			statusData.HP = scriptableObjectData.HP;
			statusData.Attack = scriptableObjectData.Attack;
			statusData.Defense = scriptableObjectData.Defense;
			moveTime = scriptableObjectData.MoveSpeed;
		}

		//---プレイヤーの検知をやめる条件を設置
		protected virtual bool ConditionStopDetectPlayer() => !detectPlayer;
		//---プレイヤーを見つけた時の行動
		protected virtual void DoSomethingWhenDetectPlayer(Transform hittedObj){}

		protected override void Start()
		{
			//---オーブの取得
			expOrb = ResourceLoader.Instance.expOrb;
			//---自分のカラーを取得
			MyColor = GetComponent<SpriteRenderer>().color;
			//---Scriptableオブジェクトからデータを移す
			statusData = new StatusData("enemy");
			CopyStatusFromScriptableObject(statusData, enemyData);

			sprite = GetComponent<SpriteRenderer>();
			DamageController.Initialize();
			base.Start();




			StartCoroutine(DetectPlayer());
		}

		IEnumerator LoadingAsset(string path)
		{
			var resReq = Resources.LoadAsync<GameObject>(path);
			while(!resReq.isDone)
			{
				yield return null;
			}
			expOrb = resReq.asset as GameObject;
		}

		IEnumerator DetectPlayer()
		{
			while(!dead)
			{
				yield return new WaitForSeconds(Random.Range(1.5f, 3.0f));
				if(ConditionStopDetectPlayer())
				{
					var hittedObj = Physics2D.BoxCastAll(transform.position, new Vector2(20, 20), 0, new Vector2(0, 0), 0, blockingLayer);
					foreach(var item in hittedObj)
					{
						if(item.transform.tag == "Player")
						{
							detectPlayer = true;
							player = item.transform.GetComponent<Player>();
							DoSomethingWhenDetectPlayer(item.transform);
						}
					}

				}
			}
		}
		protected Action ActionWhenDie;
		protected override void Die()
		{
			//---フラグ立て・処理
			deadding = true;
			rb2D.velocity = new Vector2(0, 0);
			animator.speed = 1.5f;

			//---animatorに死亡処理 "player""Enemy"tagを持つオブジェクトに対するコライダーを無効に
			GetComponent<Animator>().SetTrigger("Die");
			//IgnoreCollider(Vector2.zero,true,"Player","Enemy");

			//---コールバック
			ActionWhenDie?.Invoke();
			//---時間たった後消失処理
			StartCoroutine(
			CoroutineHelper.DelaySecond(
				action: () =>
				{
					gameObject.SetActive(false);
				},
				waitSecond: 1.0f)
			);
			//---経験値を生む
			var expInstance = Instantiate(expOrb, transform.position, Quaternion.identity);
			expInstance.GetComponentInChildren<item.ExpOrb>().Exp = (short)enemyData.Exp;
		}

		protected override void Update()
		{
			//---攻撃を受けた時に何かデリゲートをストックしていたら移動の代わりにコールバック
			if(nowReceiveDamage && behaviorWhenDamaged.Count>0)
			{
				behaviorWhenDamaged[0]();
			}

			//---そうじゃなかったら普通に行動
			else if(!nowReceiveDamage && detectPlayer && !dead)
					if(!supportType)
						Move(player.transform.position.x, player.transform.position.y);
					else if(targetFriend != null)
						Move(targetFriend.transform.position.x, targetFriend.transform.position.y);
			base.Update();
		}

		private void OnCollisionEnter2D(Collision2D collision)
		{
			if(collision.gameObject.tag == "Player")
			{
				if(player == null)
				{
					player = collision.gameObject.GetComponent<Player>();
				}
				AttackPlayer();
			}
		}

		private void OnCollisionStay2D(Collision2D collision)
		{
			if(collision.gameObject.tag == "Player")
				AttackPlayer();
		}


		protected virtual void AttackPlayer()
		{
			if(player != null&&statusData.HP>0)
			{
				player.ReceiveDamage(statusData.Attack+TempPlayerData.Attack, nowDirection,DamageManager.NormalAttack);
			}

		}

		protected override bool Move(float xDir, float yDir)
		{

			Vector2 start = transform.position;
			Vector2 end = new Vector2(xDir, yDir);
			SmoothMove(end);
			return true;

		}



		static public Color DamageColor()
		{
			var color = new Color(3.0f, 0, 0, 1.0f);
			return color;
		}

		public override bool ReceiveDamage(int attack, Direction nowDirection,Func<int,StatusData,int> damageAction)
		{
			if(!dead)
			{
				//---攻撃を食らった時の音
				SoundManager.AttackHit();
				//---攻撃を食らったので赤くなる
				sprite.color = DamageColor();
				//---実際のダメージ処理
			
				int damageAmount = damageAction(attack-TempPlayerData.Defense,statusData);
			
				//---吹っ飛ぶ方向を決める
				const float blowOffDistance = 10.2f;
				Vector3 blowOffDirection = new Vector3();
				switch(nowDirection)
				{
					case Direction.Down:
						blowOffDirection = new Vector3(0, -blowOffDistance, 0);
						break;
					case Direction.Up:
						blowOffDirection = new Vector3(0, blowOffDistance, 0);
						break;
					case Direction.Left:
						blowOffDirection = new Vector3(-blowOffDistance, 0, 0);
						break;
					case Direction.Right:
						blowOffDirection = new Vector3(blowOffDistance, 0, 0);
						break;
				}
				//---吹っ飛ぶ処理をSmoothMovementに組み込む
				int index = behaviorWhenDamaged.Count;

				behaviorWhenDamaged.Add(()=> 
				{
					StartCoroutine(ReceiveDamageMotion(blowOffDirection));
					behaviorWhenDamaged.RemoveAt(0);
				}); 

				if(damageAmount>0)
					DamageController.CreateDamageText(damageAmount.ToString(), transform.position, Color.yellow);
			
				nowReceiveDamage = true;

				//---初めて死んでたら攻撃は成功判定でtrueを返す
				//---死体に攻撃してたらfalseを返す
				if(!dead && statusData.HP<1)
				{
					dead = true;
				}
				return true;
			}
			//---死んでるのに攻撃したので攻撃失敗判定
			return false;
		}


		public bool ReceiveHealing(int healAmount,Action<int> parameterUpAction, util.DirectionHelper.Direction nowDirection,Func<int,StatusData,int> damageAction,string disPlayWord=null)
		{ 
			if(!nowHealing)
			{
				nowHealing = true;
				parameterUpAction(healAmount);
				string display = disPlayWord ?? healAmount.ToString();
				DamageController.CreateDamageText(display, transform.position, Color.green);
				StartCoroutine(CoroutineHelper.DelaySecond(2.5f,()=>nowHealing = false));
			}
			return true;
		}
		bool deadding = false;
		
		protected virtual IEnumerator ReceiveDamageMotion(Vector2 blowOffDistance)
		{
			NowReceivingDamage = true;
			IgnoreCollider(blowOffDistance, true,"Enemy");
			rb2D.velocity = blowOffDistance;
			if(dead)
				IgnoreCollider(Vector2.zero,true,"Player","Enemy");

			yield return new WaitForSeconds(0.12f);
			if(behaviorWhenDamaged.Count == 0)
			{
				rb2D.velocity = new Vector2(0, 0);
				nowReceiveDamage = false;

			}

			yield return new WaitForSeconds(0.30f);
				
			sprite.color = MyColor;
			IgnoreCollider(blowOffDistance, false,"Enemy");

			if(dead && !deadding)
				Die();
			else
			{
				NowReceivingDamage = false;
			}
		
			
			

		}

		void IgnoreCollider(Vector2 direction, bool ignored,params string[] tags)
		{
			var colliders = Physics2D.BoxCastAll(transform.position, new Vector2(5, 5), 0, direction);
			foreach(var collider in colliders)
			{
				if(tags.Any(X=>X==collider.transform.tag))
					Physics2D.IgnoreCollision(boxCollider, collider.collider, ignored);
			}
		}


	}

}