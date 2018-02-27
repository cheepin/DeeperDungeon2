using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using util;
namespace moving.enemy
{
	[Serializable]
	public struct PositionAndRotate
	{
		public Vector2 Pos;
		public Vector3 Rorate;
	}

	public class BradeAttack : MonoBehaviour
	{

		//---ActionWhenCorridor(GameObject target,int 当たった数)
		public Action<GameObject> ActionWhenCorridor{ get;set;}
		public DirectionHelper.Direction ParentDirection{ get;set;}
		public PositionAndRotate DownPosition;
		public PositionAndRotate LeftPosition;
		public PositionAndRotate RightPosition;
		public PositionAndRotate UpPosition;

		public string[] targetTag;

		private void OnEnable()
		{
			Vector3 rotate = new Vector3();

			switch(ParentDirection)
			{
				case DirectionHelper.Direction.Down:
					transform.localPosition = DownPosition.Pos;
					rotate = DownPosition.Rorate;
					break;
				case DirectionHelper.Direction.Left:
					transform.localPosition = LeftPosition.Pos;
					rotate = LeftPosition.Rorate;
					break;
				case DirectionHelper.Direction.Right:
					transform.localPosition = RightPosition.Pos;
					rotate = RightPosition.Rorate;
					break;
				case DirectionHelper.Direction.Up:
					transform.localPosition = UpPosition.Pos;
					rotate = UpPosition.Rorate;

					break;
				default:
					break;
			}

			transform.rotation = Quaternion.Euler(rotate);
			
			StartCoroutine(CoroutineHelper.Chain(this,
				CoroutineHelper.DelaySecond(0.1f,()=> GetComponent<SpriteRenderer>().color = new Color(1.0f,1.0f,1.0f,1.0f)),
				CoroutineHelper.DelaySecondLoop(0.01f,11,()=> GetComponent<SpriteRenderer>().color -= new Color(0,0,0,0.10f)),
				CoroutineHelper.WaitForEndOfFrame(()=>gameObject.SetActive(false))));
		}


		protected virtual void OnTriggerEnter2D(Collider2D collision)
		{
			Debug.Assert(ActionWhenCorridor!=null,"Actionが設定されていません");
			if(targetTag.Any(X=>X==collision.transform.tag))
				ActionWhenCorridor(collision.gameObject);
		}

	} 
}
