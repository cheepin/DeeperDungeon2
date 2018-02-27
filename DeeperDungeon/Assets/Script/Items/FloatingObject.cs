using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingObject : MonoBehaviour 
{
	[SerializeField]
	float MoveSpeed=0;
	[SerializeField]
	float reactSpeed=0.5f;

    protected ParticleSystem myParticalSystem;
	protected short numberOfOrb;

    float inverseMoveSpeed;
	int preEnteredNumber;
	ParticleSystem.Particle[] particles;
	
	//---新しくオーブが設定したコライダーに入ったら呼ばれる
	protected virtual void ActionWhenNewOrbEntered(int newComerNumber,List<ParticleSystem.Particle> enter){}

	//---コライダーの判別　Trueを出したコライダーにオーブは追従する
	protected virtual bool SetFollowCorridor(Transform other){return true; }

	protected virtual void SettingEachParticle(ParticleSystem.Particle[] particle,int length){}
	

	private void Awake()
	{
		myParticalSystem = GetComponentInChildren<ParticleSystem>();
	}

	protected  virtual void Start()
	{
		inverseMoveSpeed = 1f/MoveSpeed;
		//---パーティカルを個別に取得
		particles = new ParticleSystem.Particle[numberOfOrb];

		//---範囲内にプレイヤーがいたらプレイヤーに向かって飛ぶ
	//	StartCoroutine(SetExpIfExistPlayerInBoxCast(particles));
	}

	RaycastHit2D player = new RaycastHit2D();
	RaycastHit2D[] colliders = null;
	bool isSettingEachParticleCalled=false;
	bool throwBoxCast=false;
	//---LateUpdateなのはEmblemが通常UpdateだとGetParticleでの捕捉をミスるため
	protected virtual void LateUpdate()
	{
		//---制限時間内に回収しきれなかったら強制的に削除
		if(myParticalSystem.time >= myParticalSystem.main.duration)
			Destroy(transform.parent.gameObject);
		
		myParticalSystem.GetParticles(particles);
		//---パーティクル個々に色つけ
		if(!isSettingEachParticleCalled && myParticalSystem.particleCount>0)
		{
			SettingEachParticle(particles,particles.Length);
			isSettingEachParticleCalled=true;
			myParticalSystem.SetParticles(particles,particles.Length);
		}

	
		if(!throwBoxCast)
		{
			throwBoxCast = true;
			StartCoroutine(util.CoroutineHelper.DelaySecond(reactSpeed,()=>{colliders = Physics2D.BoxCastAll(transform.position, new Vector2(15,15), 0, new Vector2(0, 0));}));
		}
		if(colliders != null)
			foreach(var collider in colliders)
			{
				if(collider.transform != null)
					if(SetFollowCorridor(collider.transform))
					{
						player = collider;
						myParticalSystem.trigger.SetCollider(0, collider.transform);
					}
			}

		if(player.transform != null)
			for(int i = 0; i < particles.Length; i++)
			{
				var newPosition = Vector2.MoveTowards(particles[i].position, player.transform.position, Time.deltaTime * inverseMoveSpeed);
				particles[i].position = new Vector3(newPosition.x, newPosition.y, 0);
			}

		myParticalSystem.SetParticles(particles, particles.Length);
		
	}

	

	private void OnParticleTrigger()
	{
		ParticleSystem ps = GetComponent<ParticleSystem>();
		List<ParticleSystem.Particle> enter = new List<ParticleSystem.Particle>();

		int numEnter = ParticlePhysicsExtensions.GetTriggerParticles(ps,ParticleSystemTriggerEventType.Enter,enter);

		if(numEnter>preEnteredNumber)
		{
			int newComer =	numEnter -preEnteredNumber;
			ActionWhenNewOrbEntered(newComer,enter);
			preEnteredNumber = numEnter;
		}

		for (int i = 0; i < numEnter; i++) 
		{
			ParticleSystem.Particle p = enter[i];
			enter[i] = p;
		}

		ParticlePhysicsExtensions.SetTriggerParticles(ps,ParticleSystemTriggerEventType.Enter,enter);

		ParticleSystem.Burst[] burst = new ParticleSystem.Burst[1];
		myParticalSystem.emission.GetBursts(burst);
		if(numEnter >= burst[0].minCount)
		{
			//myParticalSystem.Clear();
			gameObject.SetActive(false);
		}
	}


}


	





