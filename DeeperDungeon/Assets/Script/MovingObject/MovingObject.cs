using UnityEngine;
using System.Collections;
using System.Reflection;
using System;   
using Direction = util.DirectionHelper.Direction;
//The abstract keyword enables you to create classes and class members that are incomplete and must be implemented in a derived class.

namespace moving
{
	public struct TempPlayerData
	{
		public int Mana{get;set; }
		public int MaxHP{get;set; }
		public int Attack{get;set; }
		public int Defense{get;set; }
		/// <summary>
		/// MovingObjectの移動速度
		/// 安全の為0以上を入力すると強制的に０になる
		/// </summary>
		public float MovingSpeed
		{
			get 
			{
				return movingSpeed; 
			}
			set
			{
				movingSpeed = value;
			}
		}

		float movingSpeed;
	}

	public class MovingObject : MonoBehaviour
	{
		[NonSerialized]
		public float moveTime = 0.01f;           //Time it will take object to move, in seconds.
		[NonSerialized]
		public LayerMask blockingLayer;         //Layer on which collision will be checked.
		[NonSerialized]
		public Rigidbody2D rb2D; 
		[NonSerialized]
		public Direction nowDirection;
		public bool NowAttack{get{return nowAttack; } set{nowAttack = value; } }
		[NonSerialized]
		public Animator animator;
		protected CircleCollider2D boxCollider;      //The BoxCollider2D component attached to this object.


		public TempPlayerData TempPlayerData = new TempPlayerData();
		protected bool nowRunning = false;
		protected bool nowAttack = false;
		protected bool attackOrder = false;
		protected bool nowReceiveDamage = false;
		protected Coroutine smoothMovement;
		              //The Rigidbody2D component attached to this object.
		protected float inverseMoveTime;          //Used to make movement more efficient.
		protected bool nowMoving;
		protected bool hitWall = false;
		protected bool dead = false;
		protected bool stopMoving = false;

		//Protected, virtual functions can be overridden by inheriting classes.
		protected virtual void Start()
		{
			SetUp();
		}

		protected void SetUp()
		{
			//blockingLayer = LayerMask.NameToLayer("BlockingLayer");
			blockingLayer = new LayerMask();
			blockingLayer = LayerMask.GetMask("BlockingLayer");
		
			boxCollider = GetComponent<CircleCollider2D>();
			animator = GetComponent<Animator>();
			try
			{
				rb2D = GetComponent<Rigidbody2D>();
				if(rb2D == null)
					throw new NullReferenceException();
			}
			catch(NullReferenceException)
			{
				print("RigidBody is Not Found!");
			}

			inverseMoveTime = 1f / moveTime;
		}

		//---inputされたイベントスレッドで実行
		//---入力された値分、進む
		protected virtual bool Move(float xDir, float yDir)
		{
			float ditectMoveThreshold = 0.3f;
			float moveSpeed = inverseMoveTime + TempPlayerData.MovingSpeed;
			//---Xの移動絶対値がスレッショルドより高かったらmoveSpeedを横にかける
			if(xDir > ditectMoveThreshold || xDir < -ditectMoveThreshold)
			{
				rb2D.velocity = new Vector2(xDir * moveSpeed, rb2D.velocity.y);
			}

			//---Yの移動絶対値がスレッショルドより高かったらmoveSpeedを縦にかける
			if(yDir > ditectMoveThreshold || yDir < -ditectMoveThreshold)
			{
				rb2D.velocity = new Vector2(rb2D.velocity.x, yDir * moveSpeed);
			}

			//---Xの移動絶対値がスレッショルドより低かったら横移動を０に
			if(xDir < ditectMoveThreshold && xDir > -ditectMoveThreshold)
			{
				rb2D.velocity = new Vector2(0, rb2D.velocity.y);
			}

			//---Yの移動絶対値がスレッショルドより低かったら縦移動を０に
			if(yDir < ditectMoveThreshold && yDir > -ditectMoveThreshold)
			{
				rb2D.velocity = new Vector2(rb2D.velocity.x, 0);
			}

			return true;
		}



		//---コルーチン内またはUpdate内で実行
		//---end(主人公の位置などの目標)へ向かって1フレーム進む
		protected void SmoothMove(Vector3 end)
		{
			nowMoving = true;
			nowRunning = true;

			Vector3 prePosition = transform.position;
			float moveAmount = inverseMoveTime * Time.deltaTime + TempPlayerData.MovingSpeed;;


			//---攻撃を受けてない時のみ移動
			
			//---登録されていたらコールバックを実行　移動はしない
			if(stopMoving)
			{
				nowMoving = false;
				return;
			}
			else
			{
				//---(目的地-自分の位置)*移動速度　＝　1フレームでの移動位置
				Vector3 newPostion = Vector3.MoveTowards(rb2D.position, end, moveAmount);
				Vector3 deltaMove = newPostion - (Vector3)rb2D.position;
				nowDirection = AnimatorTowardDirection(ref animator, deltaMove, 0.10f);
				animator.speed = 0.5f;
				rb2D.velocity = new Vector2(deltaMove.x * inverseMoveTime, deltaMove.y * inverseMoveTime);

			}

			nowMoving = false;
			return;
		}

		protected virtual void AttemptMove(float xDir, float yDir)
		{
			RaycastHit2D hit = new RaycastHit2D();
			Move(xDir, yDir);
			if(hit.transform == null)
				return;

			if(hit.transform.CompareTag("Enemy"))
				OnCantMove(hit.transform.GetComponent<enemy.Enemy>());

		}

		virtual protected void Update()
		{
			if(nowRunning)
				animator.SetBool("Run", true);
			else if(!nowRunning)
				animator.SetBool("Run", false);
		}

		static protected Direction AnimatorTowardDirection(ref Animator animator, Vector2 inputAmount, float directoinThreshold)
		{
			int _nowDirection = 0;
			if(Mathf.Abs(inputAmount.x) > 0 || Mathf.Abs(inputAmount.y) > 0)
			{

				if(Mathf.Abs(inputAmount.x) > Mathf.Abs(inputAmount.y))
				{
					_nowDirection = (inputAmount.x > 0) ? (int)Direction.Right : (int)Direction.Left;
					animator.SetInteger("Direction", _nowDirection);
				}
				else
				{
					_nowDirection = (inputAmount.y > 0) ? (int)Direction.Up : (int)Direction.Down;
					animator.SetInteger("Direction", _nowDirection);
				}
			}
			return (Direction)_nowDirection;

		}

		public virtual bool ReceiveDamage(int attack, util.DirectionHelper.Direction nowDirection,Func<int,StatusData,int> statusData)
		{
			return true;
		}
		protected virtual void OnCantMove(enemy.Enemy component)
		{
		}
		public virtual IEnumerator DoAttackMotion()
		{
			yield return null;
		}
		protected virtual void Die()
		{
		}
	} 
}