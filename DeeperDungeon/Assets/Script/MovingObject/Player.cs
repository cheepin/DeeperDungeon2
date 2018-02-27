using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using Direction = util.DirectionHelper.Direction;
using CoroutineHelper = util.CoroutineHelper;
using UnityEngine.SceneManagement;
using moving.enemy;

namespace moving.player
{
	public class Player : MovingObject {

		public PlayerData MyPlayerData;
		
		public Action<int,Player,Enemy> ActionWhenAttackEnemy;

		HealthBar healthBar;
		ManaBar manaBar;
		ExpBar expBar;
		Action<GameObject> ActionWhenBradeCollisionEnemy;
		PlayerBrade playerBrade;
		RecoveryPanel recoveryPanel;

		public bool Dead
		{
			get{return dead;}
			set{dead = value; }
		}
		bool specialAttackOrder = false;
		bool nowDamaged = false;
		private void Awake()
		{
			if(GameObject.FindGameObjectsWithTag("Player").Length > 1)
			{
				Destroy(gameObject);
				return;
			}
		}
		public Action ActionWhenMyPlayerDataInitialized=null;
		protected override void Start ()
		{
			if(!dungeon.DungeonManager.LoadPlayerData(ref MyPlayerData))
			{
				MyPlayerData = new PlayerData()
				{
					MaxMana = 30,
					AmountToNextLevel = 65,
					CurrentExp = 0,
					Attack = 33,
					MaxHP = 100,
					HP = 100,
					CurrentLevel = 1,
					Defense = 6,
					CurrentFloor = 1,
					NumberOfGem = 0,
					PoisonResistance = 0,
					IceResistance = 6,
					LightningResistance  = 6,
					FireResistance = 6
				};

				if(DifficultyManager.Instance.Difficult == DifficultyManager.Difficulty.Hard)
				{
					MyPlayerData.PoisonResistance = 0;
					MyPlayerData.IceResistance = 0;
					MyPlayerData.LightningResistance = 0;
					MyPlayerData.FireResistance = 0;
					MyPlayerData.Defense = 0;
					print("Its Hard Mode");
				}

			}
			moveTime = 0.2f;
			healthBar = GameObject.Find("HealthBar").GetComponent<HealthBar>();
			manaBar = GameObject.Find("ManaBar").GetComponent<ManaBar>();
			expBar = GameObject.Find("ExpBar").GetComponent<ExpBar>();

			healthBar.SetCurrentValue(MyPlayerData.HP, MyPlayerData.MaxHP);
			manaBar.SetCurrentValue(TempPlayerData.Mana, MyPlayerData.MaxMana);
			expBar.SetCurrentValue(MyPlayerData.CurrentExp, MyPlayerData.AmountToNextLevel);

			recoveryPanel = GameObject.Find("RecoveryPanel").GetComponent<RecoveryPanel>();
			recoveryPanel.gameObject.SetActive(false);

			ActionWhenMyPlayerDataInitialized?.Invoke();

			//---プレイヤーブレードの設定
			StartCoroutine(CoroutineHelper.WaitForEndOfFrame(() =>
			{
				var brade = Instantiate(ResourceLoader.GetResouce("EdgeBrade"), transform);
				playerBrade = brade.GetComponent<PlayerBrade>();
				playerBrade.ActionWhenCorridor = ActionWhenBradeCollisionEnemy;
				playerBrade.gameObject.SetActive(false);
			})
			);


			StartCoroutine(CoroutineHelper.DelaySecondLoop(0.38f, -1, () =>
			{
				if(TempPlayerData.Mana < MyPlayerData.MaxMana)
					++TempPlayerData.Mana;
			}));
			dungeon.DontDestroyHelper.SetDontDestroy(gameObject);
			SetListener();



			ActionWhenBradeCollisionEnemy = (enemy) =>
			{
				var attackSuccess = AttackEnemy(enemy);
				PursuitAttackChange(attackSuccess);
			};

			base.Start();
			
		}

		public void SetListener()
		{
			MyPlayerData.SetActionWhenParamUpdate("CurrentExp",(currentExp) =>
			{
				 expBar.SetCurrentValue(currentExp, MyPlayerData.AmountToNextLevel);
			},"ExpPlus",false);

			MyPlayerData.SetActionWhenParamUpdate("HP",(HP) =>
			{
				if(HP<=0 && !dead)
					Die();
			},"DieChecker",false);

		}
		public Action<Player> ActionWhenDie;
		protected override void Die()
		{
			animator.SetTrigger("Die");
			rb2D.velocity = new Vector2(0,0);
			dead = true;
			//---コールバック
			ActionWhenDie?.Invoke(this);
			StartCoroutine(util.CoroutineHelper.DelaySecondLoop(3.0f,1,()=>
			{
				recoveryPanel.Open(this);

			}));

			return ;
		}

		override protected void Update()
		{
			manaBar.SetCurrentValue(TempPlayerData.Mana, MyPlayerData.MaxMana);
			healthBar.SetCurrentValue(MyPlayerData.HP, MyPlayerData.MaxHP);
			base.Update();
		}

		public bool nowPursuit=false;
		public virtual void Moving(float x, float y)
		{
			if (nowAttack && !nowPursuit)
			{
				base.AttemptMove(0, 0);
				return;
			}

			Vector2 inputAmount = AnalogStick.CalculateInputDegree(x,y);
			if (Mathf.Abs(inputAmount.x) > 0 || Mathf.Abs(inputAmount.y) > 0)
			{
				nowRunning = true;
				if (Mathf.Abs(inputAmount.x) > 0.5)
					animator.SetInteger("Direction", (inputAmount.x > 0) ? (int)Direction.Right : (int)Direction.Left);
				else 
					animator.SetInteger("Direction", (inputAmount.y > 0) ? (int)Direction.Up : (int)Direction.Down);
			}

			//---現在方向を取得
			var preDireciton = nowDirection;
			nowDirection = (Direction)animator.GetInteger("Direction");
		


			//---位置が変わったら今走ってるコルーチンを削除
			//---これをやらないと滑った挙動になる
			if (nowDirection != preDireciton && smoothMovement != null)
			{
				nowMoving = false;
				print(nowMoving);
			}

			if (nowRunning == true && !dead && !nowAttack)
				base.AttemptMove(inputAmount.x, inputAmount.y);

			if (inputAmount.x == 0 && inputAmount.y == 0)
			{
				nowRunning = false;
			}

		}
		public Action<Player,int> ActionWhenAttemptAttack;
		public Func<Player,bool> PursuitAction;
		bool inPursuitTime=false;
		//---戦闘関連
		public void AttemptAttackEnemy()
		{
			//---攻撃を試みた時のデリゲートをコールバック
			ActionWhenAttemptAttack?.Invoke(this,MyPlayerData.Attack);

			if(inPursuitTime && PursuitAction!=null && !nowRunning)
			{
				nowPursuit = PursuitAction.Invoke(this);
			}

			if (!nowAttack && !nowPursuit)
			{
				nowRunning = false;
				animator.SetBool("Run", false);

				animator.SetTrigger("Attack");
				StartCoroutine(
					CoroutineHelper.WaitForEndOfFrame(()=> StartCoroutine(
					CoroutineHelper.Chain(this,DoAttackMotion(),
					CoroutineHelper.Do(()=>{nowAttack = false;})))));
			}

			if(!nowAttack && specialAttackOrder)
			{
				SetInvisible(true);
				animator.SetTrigger("Attack");
				StartCoroutine(
					CoroutineHelper.WaitForEndOfFrame(()=>StartCoroutine(
					CoroutineHelper.Chain(this,DoSpecialAttackMotion(()=>specialAttackOrder = false),
					CoroutineHelper.Do(()=>{nowAttack=false;SetInvisible(false); })))));
			}
		}

		public void SpecialAttackEnemy()
		{
			if(!nowAttack)
			{
				if (TempPlayerData.Mana >= 30)
				{
					specialAttackOrder = true;
					TempPlayerData.Mana -= 30;
				}
				else
					specialAttackOrder = false;
				AttemptAttackEnemy();
			}
		}

		IEnumerator DoSpecialAttackMotion(Action flagOperator)
		{


			nowAttack = true;
			int i = 0;
			while(i++<8)
			{
				rb2D.velocity = util.DirectionHelper.MapByNowDirection(nowDirection, 30,0);
				yield return new WaitForSeconds(0.017f);
			}
			flagOperator();
			yield return new WaitForSeconds(0.6f);
			
			
		}

		private void OnCollisionStay2D(Collision2D collision)
		{
			Transform hittedObj = collision.transform;
			if(hittedObj.tag == "Enemy" && specialAttackOrder)
				AttackEnemy(hittedObj.gameObject);
		}


		public override IEnumerator DoAttackMotion()
		{
			nowAttack = true;
			playerBrade.ParentDirection = nowDirection;
			playerBrade.gameObject.SetActive(true);
			//---エッジエフェクト
			SoundManager.SwordBrade();
			//---攻撃後の硬直時間
			yield return new WaitForSeconds(0.41f);
		}

		//---Pursuitチャンスタイム
		void PursuitAttackChange(bool attackSuccess)
		{
			if(attackSuccess)
			{
				inPursuitTime = true;
				StartCoroutine(util.CoroutineHelper.DelaySecond(0.8f,()=>{
				inPursuitTime = false;}));
			}
		}



		public bool AttackEnemy(GameObject _enemy)
		{
			var enemy = _enemy.GetComponent<Enemy>();
			int damageAmount = MyPlayerData.Attack+TempPlayerData.Attack;

			//--ReceiveDamageを発動　成功したらデリゲートと追撃フラグを立てる
			if(enemy.ReceiveDamage(damageAmount,nowDirection,DamageManager.NormalAttack))
			{
			
				ActionWhenAttackEnemy?.Invoke(damageAmount,this,enemy);
				return true;
			}
			return false;

		}
		public Func<bool> ActionWhenPreReceiveDamage = ()=>false;
		public delegate void ReceiveDamageAction(ref int damageAmount,Player player);
		public ReceiveDamageAction ActionWhenReceiveDamage = (ref int attack,Player playerdata)=>{ };

		public override bool ReceiveDamage(int attack,Direction nowDirection,Func<int,StatusData,int> damageAction)
		{
			List<bool> boolList = new List<bool>();
			//---攻撃を食らった時の前判定　パリィなど
			foreach (Func<bool> action in ActionWhenPreReceiveDamage.GetInvocationList())
				boolList.Add(action());
			//---パリィに成功したら攻撃は成功せず処理を終える
			if(boolList.Any(X=>X==true))
				return false;

			if (!nowDamaged && !specialAttackOrder && !nowPursuit)
			{
				nowDamaged = true;
				//---演出
				StartCoroutine(
					util.CoroutineHelper.Chain(this,
					DamageManager.FlushColor(GetComponent<SpriteRenderer>(),(spritess) => spritess.color = new Color(1.0f, 1.0f, 1.0f, 0.0f), (sprits) =>  sprits.color = new Color(1.0f, 1.0f, 1.0f, 1.0f)),util.CoroutineHelper.Do(
					()=>nowDamaged = false)));
				//---ヒット音を鳴らす
				SoundManager.ReceiveDamage();

				//---ダメージ処理
				int damageAmount = damageAction.Invoke(attack-TempPlayerData.Defense,MyPlayerData);
				if(damageAmount>0)
				{
					ActionWhenReceiveDamage(ref damageAmount,this);
					DamageController.CreateDamageText(damageAmount.ToString(),transform.position,new Color(0.8f,0.9f,0,1.0f));
				}
				else if(damageAmount<0)
					DamageController.CreateDamageText(("+")+damageAmount.ToString(),transform.position,new Color(0.2f,1.0f,0,1.0f));


				return true;
			}
			return false;
		}

		//---レベル関連
		public void SetExp(int expPoint)
		{
			MyPlayerData.CurrentExp += expPoint;
			expBar.SetCurrentValue(MyPlayerData.CurrentExp, MyPlayerData.AmountToNextLevel);
		}

		Func<bool> setInvisible = ()=>true;
		/// <summary>
		/// 無敵状態にする
		/// シーンの遷移の時などに使う
		/// </summary>
		public void SetInvisible(bool activeFlag)
		{
			if(activeFlag)
				ActionWhenPreReceiveDamage += setInvisible;
			else
				ActionWhenPreReceiveDamage -= setInvisible;

		}

	}

	
}

namespace moving
{
	using moving.player;
	using CHelp = CoroutineHelper;
	static public class DamageManager
	{
		static public int NormalAttack(int attack,StatusData MyPlayerData)
		{
			int damageAmount = attack - MyPlayerData.Defense;
			damageAmount += UnityEngine.Random.Range(-(damageAmount / 10) * 2, (damageAmount / 10) * 2);
			if(damageAmount<0)
				damageAmount=0;
			MyPlayerData.HP -= damageAmount;

			return damageAmount;
			
		}
		static public int EnchantAttack(int attack,StatusData MyPlayerData,StatusData.Enchant enchantType)
		{
			int damageAmount = attack;
			switch(enchantType)
			{
				case StatusData.Enchant.Fire:
					damageAmount -= MyPlayerData.FireResistance;
					break;
				case StatusData.Enchant.Ice:
					damageAmount -= MyPlayerData.IceResistance;
					break;
				case StatusData.Enchant.Electrical:
					damageAmount -= MyPlayerData.LightningResistance;
					break;
				default:
					break;
			}
			
			damageAmount += UnityEngine.Random.Range(-(damageAmount / 10) * 2, (damageAmount / 10) * 2);
			MyPlayerData.HP -= damageAmount;
			return damageAmount;
			
		}

		static public int SetPoison(MovingObject target,int duration, int notUse,StatusData myData)
		{
			myData.Poisoned = true;
			int remainTime = duration - myData.PoisonResistance;
			if(remainTime<=0)
				remainTime = 5;
			//---毒ダメージのコルーチン
			int i = 0;
			target.StartCoroutine(CHelp.Chain(target,
			CHelp.DelaySecondLoop(0.2f,()=>(myData.Poisoned&&remainTime>i++),()=>
			{
				target.GetComponent<SpriteRenderer>().color = Color.green;
				myData.HP -= 1;
			}),
			CHelp.Do
			(()=>
				target.GetComponent<SpriteRenderer>().color = Color.white
			)));
			return 1;
		}

		public static IEnumerator FlushColor(SpriteRenderer renderer, Action<SpriteRenderer> functor, Action<SpriteRenderer> functor2)
		{

			int countFrame = 0;
			while (countFrame++ < 16)
			{
				functor(renderer);
				yield return new WaitForSeconds(0.08f);
				functor2(renderer);
				yield return new WaitForSeconds(0.08f);
			}
			
			renderer.color = Color.white;
			yield break;
		}

	

		/// <summary>
		/// ステータスダウン用のデリゲートを作成する　敵行動専用
		/// 自動的に対応するステータスバーが実行される
		/// これで作成したデリゲートはそのままReceiveDamageに渡せる
		/// </summary>
		/// <param name="player">プレイヤー</param>
		/// <param name="decreaseSpeed">ゲージが減るスピード</param>
		/// <param name="action">実際にステータスを減らすデリゲート</param>
		/// <param name="RestoreAction">ステータスを戻す</param>
		static public Func<int,StatusData,int> CreateStatusDownDelegate(Player player,float decreaseSpeed,string displayAffect,float diplayDelay,Action action,Action RestoreAction)
		{
			Func<int,StatusData,int> fyunc  = (attack,statusData)=>
			{
				//---攻撃を受ける側の毒耐性の取得
				int poisonResist = player.MyPlayerData.PoisonResistance/2;
				int barMaxValue = 32;
				//---バーを作成する
				//---耐性に応じてバーの減るスピードを早める
				int barStartValue = barMaxValue-poisonResist;
				if(barStartValue<=5)
					barStartValue = 5;
				var bar = StatusBar.CreateGeneralBar(Color.cyan,barStartValue,barStartValue);
				bar.StartDecreaseValueByTime(player,decreaseSpeed,RestoreAction);
				action();
				DisplayEffect(player,displayAffect,diplayDelay);
				return 0;
			};

			return fyunc;

		}
		
		static public void DisplayEffect(MonoBehaviour monoBehaviour,string displayAffect,float displayDelay)
		{
			monoBehaviour.StartCoroutine(CHelp.DelaySecond(displayDelay,()=>DamageController.CreateDamageText(displayAffect,monoBehaviour.transform.position,new Color(0.8f,0.9f,0,1.0f))));
		}


	}

}