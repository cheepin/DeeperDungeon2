using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

//---
/// <summary>
/// パーティクルが何かに当たった時に登録していたデリゲートをトリガーするクラス
/// ParticleSystem内のTriggerの設定
///Collider:ターゲットオブジェクト  
///Inside:kill   
///Outside::ignore   
///Enter:callBack   
///Exit:Ignore   
///RadiousSize:1   
/// </summary>
public class CauseActionParticle : MonoBehaviour {
	
	/// <summary>
	/// (GameObject ターゲット,int 当たった数) 
	/// </summary>
	public Action<GameObject,int> ActionWhenCorridor;
	[SerializeField]
	public bool isTrrigerMode=false;
	ParticleSystem fire;
	List<GameObject> targetedObject = new List<GameObject>();

	private void Awake()
	{
		fire = GetComponent<ParticleSystem>();
	}

	/// <summary>
	/// 一体だけの場合はこちらのオーバーロードを使う
	/// セットされたTargetObjectは新たに上書きされる
	/// </summary>
	/// <param name="_target">ターゲットオブジェクト</param>
	public void SetTargetObject(GameObject _target)
	{
		targetedObject.Clear();
		targetedObject.Add(_target);
		fire.trigger.SetCollider(0,targetedObject[0].GetComponent<Collider2D>());
	}

	public void SetTargetObject(int index,GameObject _target)
	{
		targetedObject.Add(_target);
		fire.trigger.SetCollider(index,_target.transform);
	}

	//---これはパーティクルが存在する限り呼ばれる
	//---実際の当たり判定は関数内で作る必要がある
	private void OnParticleTrigger()
	{
        List<ParticleSystem.Particle> triggeredParticle = new List<ParticleSystem.Particle>();
		//---現在トリガーされているパーティクルを取得
		int numEnter = fire.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, triggeredParticle);
		
		//---一つ以上当たっていたら登録していたデリゲートを使う
		if(numEnter>0)
		{
			Debug.Assert(ActionWhenCorridor!=null,"Actionが設定されていません");
			foreach(var item in targetedObject)
			{
				ActionWhenCorridor.Invoke(item,numEnter);
			}
		}
        
	}

	private void OnParticleCollision(GameObject other)
	{
		if(other.transform.tag=="Enemy")
			ActionWhenCorridor.Invoke(other,0);
	}


}
