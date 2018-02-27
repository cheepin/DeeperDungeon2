using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using moving.enemy;
using System.Linq;

namespace moving.player
{
	public class PlayerBrade : BradeAttack
	{

		protected override void OnTriggerEnter2D(Collider2D collision)
		{
			Debug.Assert(ActionWhenCorridor != null, "Actionが設定されていません");
			if(targetTag.Any(X => X == collision.transform.tag))
			{
				ActionWhenCorridor(collision.gameObject);
			}
		}


	}

}