using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using util;
using moving;
namespace skill
{
	abstract public class EffectObject : MonoBehaviour
	{
		public Action<MovingObject ,MovingObject> whenCollisionAction;
		public float Force{get;set;}
		public DirectionHelper.Direction NowDirection{get;set; }
		public MovingObject Caster{get;set; }
		public string TargetObjectTag{get;set; }
		public bool StayEffect=false;
		public bool HasAction=true;
		Vector3 forceWithDirection;

		protected virtual void Start()
		{
			var collider = GetComponent<CircleCollider2D>();

			if(HasAction)
			{
				Debug.Assert(collider != null);
				if(!collider.isTrigger)
					Debug.Assert(false, "コライダーがトリガーになっていません"); 
			}
			Debug.Assert(Caster!=null);
			Debug.Assert(TargetObjectTag!=null);
			forceWithDirection = DirectionHelper.MapByNowDirection(NowDirection,Force,0);

		}

		protected virtual void Update()
		{
			transform.position += forceWithDirection;
		}

		private void OnTriggerEnter2D(Collider2D collision)
		{
			if(collision.transform.tag == TargetObjectTag && !StayEffect && HasAction)
			{
				InvokeEffect(collision);
			}
		}

		private void OnTriggerStay2D(Collider2D collision)
		{
			if(collision.transform.tag == TargetObjectTag && StayEffect && HasAction)
			{
				InvokeEffect(collision);
			}
		}

		private void OnCollisionEnter2D(Collision2D collision)
		{
			whenCollisionAction(Caster,collision.gameObject.GetComponent<MovingObject>());
		}


		protected abstract  void InvokeEffect(Collider2D collider2D);
	}

}