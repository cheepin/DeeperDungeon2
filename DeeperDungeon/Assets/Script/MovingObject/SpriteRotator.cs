using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using util;
namespace moving
{
	public class SpriteRotator : MonoBehaviour
	{

		[SerializeField]
		int Down;
		[SerializeField]
		int Left;
		[SerializeField]
		int Right;
		[SerializeField]
		int Up;

		public DirectionHelper.Direction Direction{get;set;}

		// Use this for initialization
		void Start()
		{
			var spriteTransform = GetComponent<SpriteRenderer>().gameObject.transform;
			switch(Direction)
			{
				case DirectionHelper.Direction.Down:
					spriteTransform.Rotate(new Vector3(0,0,Down));
					break;
				case DirectionHelper.Direction.Left:
					spriteTransform.Rotate(new Vector3(0,0,Left));
					break;
				case DirectionHelper.Direction.Right:
					spriteTransform.Rotate(new Vector3(0,0,Right));
					break;
				case DirectionHelper.Direction.Up:
					spriteTransform.Rotate(new Vector3(0,0,Up));
					break;
				default:
					break;
			}



		}

		// Update is called once per frame
		void Update()
		{

		}
	}

}