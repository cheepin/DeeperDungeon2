using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using util;
using moving.player;
namespace moving.enemy
{

	public class BlueGhast : Ghast 
	{
		protected float particleTime = 1.0f;
		GameObject child;
		CauseActionParticle fire;
		protected override void Start()
		{
			child = transform.Find("Ectoplasm").gameObject;
			fire = child.GetComponent<CauseActionParticle>();
			fire.ActionWhenCorridor = PoisonAttack;
			child.SetActive(false);
			base.Start();
		}
		
		protected override void DoSomethingWhenDetectPlayer(Transform hittedObj)
		{
			child.GetComponent<CauseActionParticle>().SetTargetObject(0,hittedObj.gameObject);
		}

		protected virtual void PoisonAttack(GameObject _player,int numEnter)
		{
			var player = _player.GetComponent<Player>();
			//---ポイズンダメージのカリー化
			Func<int,StatusData,int> func = (dummy1,dummy2) => DamageManager.SetPoison(player,32,dummy1,dummy2);
			if(statusData.HP>0)
			{
				player.ReceiveDamage(32,nowDirection,func);
			}
			return ;
		}

		protected override IEnumerator ActionWhenAttack(GameObject _player)
		{

			yield return StartCoroutine(
				CoroutineHelper.Chain(this,CoroutineHelper.DelaySecond(chargeTime,()=>
				{
					if(!NowReceivingDamage)
					{	
						child.SetActive(true);
						SoundManager.Bubble(); 
					} 
				}),
				CoroutineHelper.DelaySecond(particleTime,()=>
				{
					child.SetActive(false);
				})));
		}

	}
}