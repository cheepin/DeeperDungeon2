using UnityEngine;
using System.Collections;
namespace moving.enemy
{
	public class SkeletonWizard_Lv2 : SkeletonWizard
	{
		protected override bool ConditionToProceedToActionRange(RaycastHit2D hittedObj)
		{
			var _transform = hittedObj.transform;
			//---敵でかつまだ魔法を受けてない場合
			if(_transform.CompareTag("Enemy"))
			{
				return ConditionToSelectToBuffEnemy(_transform.GetComponent<Enemy>());
			}
			return false;
		}

		protected virtual bool ConditionToSelectToBuffEnemy(Enemy enemy){ return enemy.TempPlayerData.Attack == 0 ? true:false;  }

		protected override void SpriteAttack(GameObject _player, int numEnter)
		{
			//---HP1以上の時だけに限定しないと残りカスが当たるので
			if(statusData.HP>0)
			{
				var k = _player.GetComponent<Enemy>();
				k?.ReceiveHealing(enchantAttack,(amount)=>
				{
					if(k.TempPlayerData.Attack==0) 
					{
						k.TempPlayerData.Attack += amount;
						FlashColor(k);
					} 
					
				}, nowDirection,DamageManager.NormalAttack,"ATR↑");
			}
		}


		

	} 
}
