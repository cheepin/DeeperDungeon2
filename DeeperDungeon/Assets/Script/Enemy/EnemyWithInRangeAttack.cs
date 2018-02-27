using System;
using System.Linq;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace moving.enemy
{
	public class EnemyWithInRangeAttack : Enemy{

		protected float ToAttackSight;
		protected float chargeTime;
		protected Coroutine actionInrangeCorotine;
		protected float scanInterval = 0.17f;


		// Use this for initialization
		protected override void  Start () 
		{
			base.Start();
			StartCoroutine(DetectPlayerInOwnArea());

			Debug.Assert(chargeTime !=0,"chargeTimeが設定されていません");
			Debug.Assert(ToAttackSight !=0,"ToAttackSightが設定されていません");
		}
	
		//---ActionRange内の関数　BoxCast かLineCastかどっちかを選択する事が多い
		protected virtual  RaycastHit2D[]  ThrowCastType(Vector3 position,Vector3 targetPos,int layerMask){ throw new NotImplementedException();}
		//---Castに引っかかったオブジェがActionRangeへ進むための条件を記述
		protected virtual bool ConditionToProceedToActionRange(RaycastHit2D hittedObj)
		{
			return	hittedObj.transform.tag == "Player";
		}

		protected virtual IEnumerator ActionInRange(Transform hittedObj,Vector2 targetEnd)
		{
			throw new NotImplementedException();
		}

		Action DetectPlayerInArea;
		IEnumerator DetectPlayerInOwnArea()
		{
			while(!dead)
			{
				if(!nowAttack && statusData.HP>0)
				{
					Vector3 targetEnd = util.DirectionHelper.MapByNowDirection(nowDirection,ToAttackSight,0);
					var target = transform.position + targetEnd;
					if(DetectPlayerInArea==null)
						DetectPlayerInArea = () =>
						{
							var hitted = ThrowCastType(transform.position,target,blockingLayer);
							foreach(var hittedObj in hitted)
							{
						
								if(ConditionToProceedToActionRange(hittedObj) && detectPlayer)
								{
									nowAttack = true;
									stopMoving = true;
									if(!nowReceiveDamage)
										rb2D.velocity = new Vector2(0,0);
									StartCoroutine(ActionInRange(hittedObj.transform,targetEnd));
									return;
								}
							} 
						};
			
				}
				yield return new WaitForSeconds(scanInterval);

			}
		}

		private void LateUpdate()
		{
			DetectPlayerInArea?.Invoke();
			DetectPlayerInArea = null;
		}
	}
}