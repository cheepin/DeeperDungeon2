using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using moving.player;
using util;
using System.Linq;
using moving.enemy;
using Object = UnityEngine.Object;

//TODO:登録したデリゲートをロード時にまたプレイヤーに与えるにはどうすればいいか
namespace skill.spell
{

	[Serializable]
	public class BaseSpell
	{
		public Func<Player, int, bool> spellAction;
		protected virtual bool DoEffect(Player player, int currentlevel)
		{
			return true;
		}
		public BaseSpell(){}
		protected int SpellLevel => skill.SkillManager.GetLevelCount(ToString().Replace("skill.spell.",""));
		
		/// <summary>
		/// すでに"MethodName"で登録されていたらfalse
		/// されてなかったらtrue
		/// のちにオーバーライド側で登録
		/// </summary>
		public virtual bool RegisterListener(Player player,Delegate action=null,string MethodName="")
		{
			var invocatoinList = action?.GetInvocationList();
			if(invocatoinList != null)
				if(invocatoinList.Any(X => X.Method.Name == MethodName))
				{
					return false;
				}
			return true;
		}


	}

	//---emptyスロット用のダミースキル
	public class EmptySpell : BaseSpell
	{
		public EmptySpell()
		{
			spellAction = (i, ai) =>
			{
				return true;
			};
		}


	}

	public class Healing : BaseSpell
	{
		public Healing()
		{
			spellAction = DoEffect;
		}

		protected override bool DoEffect(Player player, int level)
		{
			if(player.MyPlayerData.HP == player.MyPlayerData.MaxHP)
				return false;
			Object.Instantiate(ResourceLoader.GetResouce("SkillActivate"), player.transform);
			var healingData = SkillManager.GetSkillData<HealingData>("Healing");
			player.MyPlayerData.HP += healingData.HealAmount  * level;
			return true;
		}
	}

	public class Camp : BaseSpell
	{
		public Camp()
		{
			spellAction = DoEffect;
			
		}

		Action<int> CampEffect;

		protected override bool DoEffect(Player player, int level)
		{
			RegisterListener(player,CampEffect);
			return true;
		}

		void SetDelegate(Player player)
		{
			CampEffect  = (c) =>
			{
				int healAmount = SkillManager.GetSkillData<CampData>("Camp").HealAmount;
				player.MyPlayerData.HP += healAmount * SpellLevel;
			};
		}

		public override bool RegisterListener(Player player, Delegate action, string methodName = "")
		{
			SetDelegate(player);
			player.MyPlayerData.SetActionWhenParamUpdate("CurrentFloor",CampEffect,nameof(CampEffect),false);
			return true;
		}

	}

	public class Herbalism : BaseSpell
	{
		public Herbalism()
		{
			spellAction = DoEffect;
		}

		protected override bool DoEffect(Player player, int level)
		{
			if(player.MyPlayerData.HP == player.MyPlayerData.MaxHP)
				return false;
			else
			{
				int maxHP = player.MyPlayerData.HP;
				var skillData = SkillManager.GetSkillData<HerbalismData>(nameof(Herbalism));
				Object.Instantiate(ResourceLoader.GetResouce("SkillActivate"), player.transform);
				player.StartCoroutine
				(
					CoroutineHelper.DelaySecondLoop(skillData.Interval, skillData.HealAmount, () =>
					  {
						if(player.MyPlayerData.HP < player.MyPlayerData.MaxHP)
							player.MyPlayerData.HP += level;
					})
				);
				return true;
			}
		}
	}

	public class Stlength : BaseSpell
	{
		public Stlength()
		{
			spellAction = (player, i) =>
			{
				if(DifficultyManager.Instance.Difficult== DifficultyManager.Difficulty.Hard)
				{
					player.MyPlayerData.Attack += (SkillManager.GetSkillData<DefenseData>(nameof(Stlength)).UpAmount-2);

				}
				else
				{
					player.MyPlayerData.Attack += SkillManager.GetSkillData<DefenseData>(nameof(Stlength)).UpAmount;
				}
				return true;
			};
		}
	}
	public class MaxHP : BaseSpell
	{
		public MaxHP()
		{
			spellAction = (player, i) =>
			{
				player.MyPlayerData.MaxHP +=  SkillManager.GetSkillData<DefenseData>(nameof(MaxHP)).UpAmount;
				return true;
			};
		}
	}
	public class MaxMana : BaseSpell
	{
		public MaxMana()
		{
			spellAction = (player, i) =>
			{
				player.MyPlayerData.MaxMana += SkillManager.GetSkillData<DefenseData>(nameof(MaxMana)).UpAmount;
				return true;
			};
		}
	}
	public class Defense : BaseSpell
	{
		public Defense()
		{
			spellAction = (player, i) =>
			{
				if(DifficultyManager.Instance.Difficult== DifficultyManager.Difficulty.Hard)
				{
					player.MyPlayerData.Defense += (SkillManager.GetSkillData<DefenseData>("Defense").UpAmount-2);
				}
				else
				{
					player.MyPlayerData.Defense += SkillManager.GetSkillData<DefenseData>("Defense").UpAmount;
				}
				
				return true;
			};
		}
	}

	public class Gungnir : BaseSpell
	{
		public Gungnir()
		{
			spellAction = (player, i) =>
			{
				player.MyPlayerData.Attack += SkillManager.GetSkillData<DefenseData>(nameof(Gungnir)).UpAmount;
				return true;
			};
		}
	}

	public class DragonMeat : BaseSpell
	{
		public DragonMeat()
		{
			spellAction = (player, i) =>
			{
				player.MyPlayerData.MaxHP += SkillManager.GetSkillData<DefenseData>(nameof(DragonMeat)).UpAmount;
				return true;
			};
		}
	}

	public class ShieldOfAjax : BaseSpell
	{
		public ShieldOfAjax()
		{
			spellAction = (player, i) =>
			{
				player.MyPlayerData.Defense += SkillManager.GetSkillData<DefenseData>(nameof(ShieldOfAjax)).UpAmount;
				return true;
			};
		}
	}

	public class Elixir : BaseSpell
	{
		public Elixir()
		{
			spellAction = (player, i) =>
			{
				player.MyPlayerData.MaxMana += SkillManager.GetSkillData<DefenseData>(nameof(Elixir)).UpAmount;
				return true;
			};
		}
	}

	public class DragonSkin : BaseSpell
	{
		public DragonSkin()
		{
			spellAction = (player, i) =>
			{
				player.MyPlayerData.FireResistance += SkillManager.GetSkillData<DefenseData>(nameof(DragonSkin)).UpAmount;
				player.MyPlayerData.IceResistance += SkillManager.GetSkillData<DefenseData>(nameof(DragonSkin)).UpAmount;
				player.MyPlayerData.LightningResistance += SkillManager.GetSkillData<DefenseData>(nameof(DragonSkin)).UpAmount;
				player.MyPlayerData.PoisonResistance += SkillManager.GetSkillData<DefenseData>(nameof(DragonSkin)).UpAmount;

				return true;
			};
		}
	}

	public class BashAttack : BaseSpell
	{
		public BashAttack()
		{
			spellAction = (player, i) =>
			{
				player.MyPlayerData.Defense += 1;
				return true;
			};
		}
	}
	public class Mapping : BaseSpell
	{
		public Mapping()
		{
			spellAction = (player, i) =>
			{
				return true;
			};
		}
	}

	public class FireResistance : BaseSpell
	{
		public FireResistance()
		{
			spellAction = (player, i) =>
			{
				player.MyPlayerData.FireResistance += SkillManager.GetSkillData<FireResistanceData>("FireResistance").IncreaseAmountByLevel;
				return true;
			};
		}
	}
	public class PoisonResistance : BaseSpell
	{
		public PoisonResistance()
		{
			spellAction = (player, i) =>
			{
				player.MyPlayerData.PoisonResistance += player.MyPlayerData.FireResistance += SkillManager.GetSkillData<FireResistanceData>(nameof(PoisonResistance)).IncreaseAmountByLevel;;
				return true;
			};
		}
	}
	public class IceResistance : BaseSpell
	{
		public IceResistance()
		{
			spellAction = (player, i) =>
			{
				player.MyPlayerData.IceResistance += SkillManager.GetSkillData<FireResistanceData>(nameof(IceResistance)).IncreaseAmountByLevel;
				return true;
			};
		}
	}

	public class LightningResistance : BaseSpell
	{
		public LightningResistance()
		{
			spellAction = (player, i) =>
			{
				player.MyPlayerData.LightningResistance += SkillManager.GetSkillData<FireResistanceData>(nameof(LightningResistance)).IncreaseAmountByLevel;
				return true;
			};
		}
	} 

	public class Key : BaseSpell
	{
		public Key()
		{
			spellAction = (player, i) =>
			{
				return true;
			};
		}
	} 
	public class Vampire : BaseSpell
	{
		public Vampire()
		{
			spellAction = (player, i) =>
			{
				var skillData = SkillManager.GetSkillData<VampireData>(nameof(Vampire));
				player.MyPlayerData.LightningResistance -= skillData.DecreaseLightningResistance;
				player.MyPlayerData.FireResistance -= skillData.DecreaseFireResistance;
				player.MyPlayerData.Attack += skillData.UpAmountAttack;


				RegisterListener(player,player.ActionWhenAttackEnemy,nameof(DrainHP));
				return true;
			};
		}

		void DrainHP(int attack,Player player,Enemy enemy)
		{
			player.MyPlayerData.HP += SkillManager.GetSkillData<VampireData>(nameof(Vampire)).DrainHp*SpellLevel;
		}
		//---action:player側のデリゲート
		//---methodName:登録するメソッド
		public override bool RegisterListener(Player player, Delegate action=null, string methodName = "")
		{
			if(base.RegisterListener(player, player.ActionWhenAttackEnemy, methodName))
				player.ActionWhenAttackEnemy += DrainHP;
			return true;
		}
	} 

	public class Pursuit : BaseSpell
	{
		public Pursuit()
		{
			spellAction = (player, i) =>
			{
				RegisterListener(player,player.PursuitAction,nameof(PursuitAttack));
				return true;
			};
		}

		public override bool RegisterListener(Player player,Delegate action,string methodName="")
		{ 
			if(base.RegisterListener(player,action,methodName))
				player.PursuitAction += PursuitAttack; 
			return true;
		}

		bool PursuitAttack(Player player)
		{
			var skillData = SkillManager.GetSkillData<PursuitData>(nameof(Pursuit));
			if(player.TempPlayerData.Mana<skillData.manaCost)
				return false;
			player.TempPlayerData.Mana -= skillData.manaCost;
			player.StartCoroutine(Attack(player));
			return true;
		}

		IEnumerator Attack(Player player)
		{
			var skillData = SkillManager.GetSkillData<PursuitData>(nameof(Pursuit));
			player.TempPlayerData.Attack += (int)(player.MyPlayerData.Attack*skillData.AttackUpCoeffient);

			int i = 0;
			var paticle = player.transform.Find("PursuitParticle").gameObject;
			paticle.SetActive(true);
			SoundManager.Concentration();
			yield return new WaitForSeconds(0.2f);
			while(i++<3)
			{
				player.rb2D.velocity = util.DirectionHelper.MapByNowDirection(player.nowDirection, 30,0);
				yield return new WaitForSeconds(0.012f);
				

			}
			player.rb2D.velocity = new Vector2(0,0);
			player.animator.SetTrigger("Attack");
			player.StartCoroutine(CoroutineHelper.Chain(player,player.DoAttackMotion(),CoroutineHelper.Do(()=>player.NowAttack=false)));
			yield return new WaitForSeconds(0.5f);
			player.nowPursuit = false;
			paticle.SetActive(false);
			player.TempPlayerData.Attack -= (int)(player.MyPlayerData.Attack*skillData.AttackUpCoeffient);

		}

	}

	//---重複発動できないスペル
	public  abstract class SingletonSpell<T> :BaseSpell
	{
		static protected bool AlreadyExist
		{
			get 
			{
				return alreadyExist;
			}
			set
			{
				if(value)
					Debug.Assert(false,"AlradyExistは派生クラスでTrueにできません。設定できるのはFalseのみです");
				else
					alreadyExist = value;
			}
		}
		static bool alreadyExist=false;

		public SingletonSpell()
		{
			spellAction = MySpellAction;
		}

		bool MySpellAction(Player player,int level)
		{
			if(!AlreadyExist)
			{
				alreadyExist = true;
				ImplementSpellAction(player,level);
				return true;
			}

			else
				return false;
		}
		protected abstract void ImplementSpellAction(Player player,int level);
	}



	public class Parry:SingletonSpell<Parry>
	{
		protected override void ImplementSpellAction(Player player, int level)
		{
			//---エフェクトの読み込み
			var effect1 = UnityEngine.Object.Instantiate(ResourceLoader.GetResouce("Parry"), player.transform) as GameObject;
			var effect2 = UnityEngine.Object.Instantiate(ResourceLoader.GetResouce("SkillActivate"), player.transform) as GameObject;

			effect1.SetActive(false);

			//---ダメージを受けた時にコールバックされるデリゲート
			bool nowActive = false;
			Func<bool> ParryIng = () =>
			{
				//---効果音が連打しないようにストッパーをかける
				if(!nowActive)
				{
					nowActive = true;
					SoundManager.Parry();
					effect1.SetActive(true);
					player.StartCoroutine(CoroutineHelper.DelaySecond(0.25f, () => nowActive = false));
					player.StartCoroutine(CoroutineHelper.DelaySecond(0.2f, () => effect1.SetActive(false)));

				}
				
				

				return true;
			};
			player.ActionWhenPreReceiveDamage += ParryIng;

			//---バーの生成
			var skillData = SkillManager.GetSkillData<ParryData>(nameof(Parry));
			StatusBar newBar = StatusBar.CreateGeneralBar(Color.yellow, 30, 30);
			newBar.StartDecreaseValueByTime(player,skillData.BaseGageDecreaseSpeed + (level * skillData.CoefficientByLevelDecreaseSpeed),() =>
			{
				player.ActionWhenPreReceiveDamage -= ParryIng;
				AlreadyExist = false;
			});
		}


		public Parry()
		{
		}
	}

	public class LimitBreak:SingletonSpell<LimitBreak>
	{
		protected override void ImplementSpellAction(Player player, int level)
		{
			//---エフェクトの読み込み
			var effect2 = UnityEngine.Object.Instantiate(ResourceLoader.GetResouce("SkillActivate"), player.transform) as GameObject;
			Debug.Log(AlreadyExist);

			int attackPowerUpSum=0;
			int defensePowerUpSum = 0;

			StatusUp(player,level,ref attackPowerUpSum,ref defensePowerUpSum);

			//---バーの生成
			StatusBar newBar = StatusBar.CreateGeneralBar(Color.yellow, 30, 30);

			newBar.StartDecreaseValueByTime(player, 1,()=>
			{
				player.TempPlayerData.Attack -= attackPowerUpSum;
				player.TempPlayerData.Defense -= defensePowerUpSum;

				//---反動パワーダウンタイムの開始
				int attackPowerDownSum=0;
				int defensePowerDownSum = 0;

				StatusDown(player,level,ref attackPowerDownSum,ref defensePowerDownSum);
				player.StartCoroutine(CoroutineHelper.WaitForEndOfFrame(()=>
				{
					StatusBar RecoilBar = StatusBar.CreateGeneralBar(Color.white, 30, 30);
					//---ゲージを減らす
					RecoilBar.StartDecreaseValueByTime(player,0.5f,()=>{
						//---反動タイム終了後に落ちた能力を戻す
						player.TempPlayerData.Attack += attackPowerDownSum;
						player.TempPlayerData.Defense += defensePowerDownSum;
						AlreadyExist = false;
					});
				}
				));

			});
		}

		void StatusUp(Player player,int level,ref int attackPowerUpSum,ref int defensePowerUpSum)
		{
			var skillData = SkillManager.GetSkillData<LimitBreakData>(nameof(LimitBreak));
			attackPowerUpSum = skillData.AttackUpAmount*level;
			defensePowerUpSum = skillData.DefenseUpAmount*level;

			player.TempPlayerData.Attack += attackPowerUpSum;
			player.TempPlayerData.Defense += defensePowerUpSum;

		}

		void StatusDown(Player player ,int level,ref int attackPowerDownSum,ref int defensePowerDownSum)
		{
			var skillData = SkillManager.GetSkillData<LimitBreakData>(nameof(LimitBreak));
			attackPowerDownSum =skillData.AttackDownAmount*level;
			defensePowerDownSum = skillData.DefenseDownAmount*level;

			player.TempPlayerData.Attack -= attackPowerDownSum;
			player.TempPlayerData.Defense -= defensePowerDownSum;
		}


		public LimitBreak()
		{
		}
	}
	public class Adrenaline:SingletonSpell<Adrenaline>
	{
		protected override void ImplementSpellAction(Player player, int level)
		{
			//---エフェクトの読み込み
			var effect1 = UnityEngine.Object.Instantiate(ResourceLoader.GetResouce("frameBall"),player.transform) as GameObject;
			var effect2 = UnityEngine.Object.Instantiate(ResourceLoader.GetResouce("SkillActivate"),player.transform) as GameObject;
			effect1.SetActive(false);
				
			//---バーの生成
			int startValue = 0;
			var skillData = SkillManager.GetSkillData<AdrenalineData>("Adrenaline");
			int maxValue = skillData.BaseGageLimit*level;
			StatusBar newBar = StatusBar.CreateGeneralBar(Color.red,startValue,maxValue);

			//---敵を倒した時にコールバックされるデリゲート

			int basePowerup =  player.MyPlayerData.Attack/skillData.DivAttack;
			

			int powerupSum = 0;
			int chargeCount = 0;
			//---敵を攻撃した時に呼ばれるデリゲート
			Action<int,Player,Enemy> secretionAdrenaline = (attack,_player,_enemy)=>
			{
				//---敵を倒した時攻撃力を上げる
				if(_enemy.statusData.HP <= 0)
				{
					if(chargeCount++<maxValue)
					{
						//---バーの生成
						newBar.SetCurrentValue(++startValue,maxValue);
						powerupSum = chargeCount*basePowerup;
						//---攻撃力を上げる
						_player.TempPlayerData.Attack += basePowerup;
						
						//---演出
						effect1.SetActive(true);
						SoundManager.LevelUp();
						player.StartCoroutine(CoroutineHelper.DelaySecond(1.3f,()=>effect1.SetActive(false)));
							
					}
				}
			};
			player.ActionWhenAttackEnemy += secretionAdrenaline;

			//---ダメージを受けた時にコールバックされるデリゲート
			Player.ReceiveDamageAction spellExit = null;
			int restoreAttack = player.MyPlayerData.Attack;
			spellExit = (ref int attack,Player _player)=>
			{
				//---攻撃力を元に戻す
				player.TempPlayerData.Attack -= powerupSum;
				
				//---ダメージを２倍
				_player.MyPlayerData.HP -= attack;
				attack*=2;

				//---イベントの解除
				player.ActionWhenReceiveDamage -= spellExit;
				player.ActionWhenAttackEnemy -= secretionAdrenaline;
				
				//---バーを消す
				UnityEngine.Object.Destroy(newBar.gameObject);
				AlreadyExist = false;
			};
			player.ActionWhenReceiveDamage += spellExit;


		}
		public Adrenaline()
		{
		}
	}


	public class EnergyBall : BaseSpell
	{
		public EnergyBall()
		{
			spellAction = (player, level) =>
			{
				
				var transform = player.gameObject.transform;

				Vector3 t = new Vector3(transform.position.x,transform.position.y,-3);
				var arrow = UnityEngine.Object.Instantiate(ResourceLoader.GetResouce("Arrow"),t,Quaternion.identity) as GameObject;
				var arrowComp = arrow.GetComponent<skill.ArrowSpell>();
				arrowComp.Force = 1.1f;
				arrowComp.NowDirection = player.nowDirection;
				arrowComp.Caster = player;
				arrowComp.TargetObjectTag = "Enemy";
				arrowComp.whenCollisionAction = (_player,_enemy)=>
				{
					var skillData = SkillManager.GetSkillData<FireBallData>("EnergyBall");
					Func<int,StatusData,int> lightningAttack = (attack,data)=>moving.DamageManager.EnchantAttack(attack,data,StatusData.Enchant.Electrical);
					_enemy.ReceiveDamage(skillData.BaseDamage + (level*skillData.IncreaseAmountByLevel),player.nowDirection,lightningAttack);
				};

				var spriteRotator = arrowComp.GetComponent<moving.SpriteRotator>();
				spriteRotator.Direction = player.nowDirection;

				SoundManager.Magic();
				return true;
			};
		}
		
	} 

	public class FireBall : BaseSpell
	{
		public FireBall()
		{
			spellAction = (player, level) =>
			{
				
				var transform = player.gameObject.transform;
				var skillData = SkillManager.GetSkillData<FireBallData>("FireBall");
				Vector3 t = new Vector3(transform.position.x,transform.position.y,-3);
				var arrow = UnityEngine.Object.Instantiate(ResourceLoader.GetResouce("FireBall"),t,Quaternion.identity) as GameObject;
				var arrowComp = arrow.GetComponent<skill.ArrowSpell>();
				arrowComp.Force = 0.34f;
				arrowComp.NowDirection = player.nowDirection;
				arrowComp.Caster = player;
				arrowComp.TargetObjectTag = "Enemy";

				SoundManager.Explocive();

				var fireParticleAction = arrow.GetComponent<CauseActionParticle>();
				
				//---コライダーを取得
				RaycastHit2D[] hittedObjs = Physics2D.BoxCastAll(player.transform.position,new Vector2(1,1),0,DirectionHelper.MapByNowDirection(player.nowDirection,1,0),3.0f);
				for(int i = 0; i < hittedObjs.Count(); i++)
				{
					if(hittedObjs[i].transform.tag == "Enemy")
						fireParticleAction.SetTargetObject(i,hittedObjs[i].transform.gameObject);
				}

				//---当たった時の処理
				fireParticleAction.ActionWhenCorridor = (enemy,attack) => 
				{
					var enemyComp = enemy.GetComponent<Enemy>();

					enemyComp.ReceiveDamage(skillData.BaseDamage+ (level*skillData.IncreaseAmountByLevel),player.nowDirection,(atack,data)=>moving.DamageManager.EnchantAttack(atack,data,StatusData.Enchant.Fire));
					//---複数回攻撃
					int dumpDamage=1;
					player.StartCoroutine(CoroutineHelper.DelaySecondLoop(0.1f,level,()=>enemyComp.ReceiveDamage(( (skillData.BaseDamage/2) + (level*skillData.IncreaseAmountByLevel) /dumpDamage++) ,player.nowDirection,(atack,data)=>moving.DamageManager.EnchantAttack(atack,data,StatusData.Enchant.Fire))));
					arrow.SetActive(false);
				};

				//---時間経ったら消える
				player.StartCoroutine(CoroutineHelper.DelaySecond(0.24f,()=>{if(arrow!=null) arrow?.SetActive(false);}));

				return true;
			};
		}
		
	} 

	public class VineTrap : BaseSpell
	{
		public VineTrap()
		{
			spellAction = (player, level) =>
			{
				
				var transform = player.gameObject.transform;

				Vector3 effectPos = new Vector3(transform.position.x,transform.position.y,-3);
				effectPos += util.DirectionHelper.MapByNowDirection(player.nowDirection,3,0);

				UnityEngine.Object.Instantiate(ResourceLoader.GetResouce("SkillActivate"),effectPos,Quaternion.identity);
				var vineTrap = UnityEngine.Object.Instantiate(ResourceLoader.GetResouce("VineTrap"),effectPos,Quaternion.identity) as GameObject;
				var vintComp = vineTrap.GetComponent<skill.TrapSpell>();
				vintComp.Force = 0;
				vintComp.NowDirection = player.nowDirection;
				vintComp.Caster = player;
				vintComp.TargetObjectTag = "Enemy";
				vintComp.whenCollisionAction = (_player,enemy)=>
				{
					var skillData = SkillManager.GetSkillData<FireBallData>(nameof(VineTrap));
					Func<int,StatusData,int> lightningAttack =(dummy1,dummy2)=> moving.DamageManager.EnchantAttack(dummy1,dummy2,StatusData.Enchant.Electrical);
					enemy.ReceiveDamage(skillData.BaseDamage+ (level*skillData.IncreaseAmountByLevel),enemy.nowDirection,lightningAttack);
				};

				vineTrap.SetActive(false);
				player.StartCoroutine(CoroutineHelper.DelaySecond(0.6f,()=>vineTrap.SetActive(true)));
				player.StartCoroutine(CoroutineHelper.DelaySecond(5.0f,()=>{ if(vineTrap!=null)  vineTrap.SetActive(false); }));
				return true;
			};
		}
		
	} 

	public class FrostFall : BaseSpell
	{
		public FrostFall()
		{
			spellAction = (player, level) =>
			{
				
				var transform = player.gameObject.transform;

				Vector3 effectPos = new Vector3(transform.position.x,transform.position.y,-3);
				effectPos += util.DirectionHelper.MapByNowDirection(player.nowDirection,3,0);

				//---エフェクトのインスタンス化
				UnityEngine.Object.Instantiate(ResourceLoader.GetResouce("SkillActivate"),effectPos,Quaternion.identity);
				var frost = UnityEngine.Object.Instantiate(ResourceLoader.GetResouce("FrostFall"),effectPos,Quaternion.identity) as GameObject;
				var frostParticle = frost.GetComponentInChildren<CauseActionParticle>();
				List<GameObject> hittedObjs = new List<GameObject>();
				//---コールドダメージのデリゲートをカリー化
				Func<int,StatusData,int> iceDamage = (attack,data)=>moving.DamageManager.EnchantAttack(attack,data,StatusData.Enchant.Ice);
				frostParticle.ActionWhenCorridor = (_enemy,attack)=>
				{
					if(!hittedObjs.Any(X=> X==_enemy))
					{
						var enemy = _enemy.GetComponent<Enemy>();
						var skillData = SkillManager.GetSkillData<FireBallData>(nameof(FrostFall));
						enemy.ReceiveDamage(skillData.BaseDamage+ (level*skillData.IncreaseAmountByLevel),player.nowDirection,iceDamage);
						hittedObjs.Add(_enemy);
					}
					
				};

				frost.SetActive(false);
				//---一定時間経過したら閉じる
				player.StartCoroutine(CoroutineHelper.DelaySecond(0.6f,()=>frost.SetActive(true)));
				player.StartCoroutine(CoroutineHelper.DelaySecond(1.15f,()=>{ if(frost!=null)frost?.SetActive(false);}));
				return true;
			};
		}
		
	} 

	public class StrikeHard : BaseSpell
	{
		public StrikeHard()
		{
			spellAction = (player, level) =>
			{
				var transform = player.gameObject.transform;
				UnityEngine.Object.Instantiate(ResourceLoader.GetResouce("SkillActivate"),transform.position,Quaternion.identity);
				player.NowAttack = true;
				player.animator.SetTrigger("Attack");
				player.animator.speed = 0.1f;
				player.StartCoroutine(
					CoroutineHelper.Chain(player,
					CoroutineHelper.DelaySecond(0.67f,()=>player.animator.speed = 1.0f),
					CoroutineHelper.DelaySecond(0.10f,()=>InstantiateEdgeBrade(transform,player,level)),
					CoroutineHelper.DelaySecond(0.1f,()=>player.NowAttack = false)
					));

				return true;
			};

			

		}

		void InstantiateEdgeBrade(Transform transform,Player player,int level)
		{
			var edge = Object.Instantiate(ResourceLoader.GetResouce("EdgeBrade"),transform.position,Quaternion.identity,transform);
			var edgeBrade = edge.GetComponent<PlayerBrade>();
			edgeBrade.ParentDirection = player.nowDirection;
			edgeBrade.ActionWhenCorridor = (enemy)=>AttackEnemy(player,enemy,level);
			edge.SetActive(true);
		}
		void AttackEnemy(Player player, GameObject enemy,int level)
		{
			SoundManager.Explocive();

			Object.Instantiate(ResourceLoader.GetResouce("Fire"),enemy.transform);
			Object.Instantiate(ResourceLoader.GetResouce("Spark"),enemy.transform);
			var skillData = SkillManager.GetSkillData<PursuitData>(nameof(StrikeHard));
			enemy.GetComponent<Enemy>().ReceiveDamage( (int)(player.MyPlayerData.Attack*skillData.AttackUpCoeffient)+(level*20),player.nowDirection,moving.DamageManager.NormalAttack);
		}
	} 
	
	public class MindOverMatter : BaseSpell
	{
		public MindOverMatter()
		{
			spellAction = (player, level) =>
			{

				RegisterListener(player,player.ActionWhenReceiveDamage,"MindOverMatterAffect");
				return true;
			};

			

		}

		public void MindOverMatterAffect(ref int damageAmount,Player player)
		{
			var skillData = SkillManager.GetSkillData<MindOverMatterData>(nameof(MindOverMatter));
			int reduceMana = (int)(damageAmount*(skillData.BaseDamageCoefficient+(SpellLevel*skillData.DamageCoefficientByLevel)));
			if(player.TempPlayerData.Mana> reduceMana)
			{
				//---データ処理
				damageAmount -= reduceMana;
				player.TempPlayerData.Mana -= reduceMana;
				//---HPが１以上の場合回復
				if(player.MyPlayerData.HP>0)
					player.MyPlayerData.HP += reduceMana;
				
			}


		}

		public override bool RegisterListener(Player player,Delegate action,string methodName="")
		{ 
			if(base.RegisterListener(player,action,methodName))
				player.ActionWhenReceiveDamage += MindOverMatterAffect; 
			return true;
		}

	} 

	public class LifeOrPower : BaseSpell
	{
		Action<int> actionWhenHPChangedLifeOrPower;
		public LifeOrPower()
		{
			spellAction = (player, level) =>
			{
				SetDelegate(player);
				RegisterListener(player,actionWhenHPChangedLifeOrPower,"HP");
				
				return true;
			};
		}
		
		public void SetDelegate(Player player)
		{
			bool nowActive = false;
			int upAmount=0;
			var effect1 = UnityEngine.Object.Instantiate(ResourceLoader.GetResouce("frameBall"),player.transform) as GameObject;
			effect1.SetActive(false);

			actionWhenHPChangedLifeOrPower = (currentHP) =>
			{
				
				if(currentHP <= (player.MyPlayerData.MaxHP/2) && !nowActive)
				{
					//---エフェクト処理
					effect1.SetActive(true);
					player.StartCoroutine(CoroutineHelper.DelaySecond(1.3f,()=>effect1.SetActive(false)));
					nowActive =true;

					//---攻撃力アップ
					var skillData = SkillManager.GetSkillData<LifeOrPowerData>(nameof(LifeOrPower));
					upAmount = (int)(player.MyPlayerData.Attack * skillData.BaseCoefficient+(SpellLevel*skillData.PowerUpCoefficientByLevel));
					player.TempPlayerData.Attack += upAmount;
					Debug.Log(upAmount);
					Debug.Log($"player.TempPlayerData.Attack{player.TempPlayerData.Attack}");
				}
				else if(currentHP > (player.MyPlayerData.MaxHP/2) && nowActive)
				{
					Debug.Log(upAmount);
					player.TempPlayerData.Attack -=upAmount;
					nowActive = false;
				}
			};
		}

		public override bool RegisterListener(Player player, Delegate action, string methodName = "")
		{
			SetDelegate(player);
			player.MyPlayerData.SetActionWhenParamUpdate("HP",actionWhenHPChangedLifeOrPower,nameof(actionWhenHPChangedLifeOrPower),false);
			return true;
		}

	} 


	public class SlashWithMana:BaseSpell
	{
		
		public SlashWithMana()
		{
			spellAction = (player, i) =>
			{
				RegisterListener(player,player.ActionWhenAttemptAttack,nameof(DecreaseManaAndUpAttack));
				return true;
			};
		}

		Action<Player,int> DecreaseManaAndUpAttack;
		Action<int,Player,Enemy> RestoreStatus;
		Action<int,Player,Enemy> SparkEffect;
		//---action:player側のデリゲート
		//---methodName:登録するメソッド
		public override bool RegisterListener(Player player, Delegate action=null, string methodName = "")
		{
			if(base.RegisterListener(player, player.ActionWhenAttackEnemy, methodName))
			{
				SetDelegate(player);
				player.ActionWhenAttemptAttack += DecreaseManaAndUpAttack;

			}
			return true;
		}
		
		void SetDelegate(Player player)
		{
			//---データの読み込み
			var slashWithManaData =  SkillManager.GetSkillData<SlashWithManaData>(nameof(SlashWithMana));
			int consumptionMana = slashWithManaData.ConsumptionMana;
			
			//---デリゲートの生成
			DecreaseManaAndUpAttack = (_player,attack)=>
			{
				//---元の攻撃力に係数をかける
				int powerUpSum = (int)(slashWithManaData.PowerUpCoefficient * player.MyPlayerData.Attack);
				int consumption = consumptionMana * SpellLevel;
				player.ActionWhenAttackEnemy -= SparkEffect; 
				if(player.TempPlayerData.Mana>consumption)
				{
					//---マナ消費とステータスアップ
					player.TempPlayerData.Mana -= consumption;
					player.TempPlayerData.Attack += powerUpSum;
					//---攻撃後にステータスを戻す
					player.StartCoroutine(CoroutineHelper.DelaySecond(0.4f,
						()=>player.TempPlayerData.Attack-=powerUpSum));
					//---スパークエフェクトのデリゲートを登録
					player.ActionWhenAttackEnemy += SparkEffect; 

				}
			
			};
			//---スパークエフェクトをActionWhenAttackEnemyに付ける
			SparkEffect = (attack,_player,enemy)=>
			{
				SoundManager.Magic2();
				Object.Instantiate(ResourceLoader.GetResouce("Spark"),enemy.transform);
				_player.ActionWhenAttackEnemy -=SparkEffect;
			};
				

		}

	}


}

