using UnityEngine;
using System.Collections;


namespace moving.enemy
{
	public class SkeletonWizard_Lv3 : SkeletonWizard_Lv2
	{
		protected override void Start()
		{
			enchantTime = 15;
			base.Start();
		
		}

		protected override void SpriteAttack(GameObject _player,int numEnter)
		{
			//---HP1以上の時だけに限定しないと残りカスが当たるので
			if(statusData.HP>0)
			{
				var k = _player.GetComponent<Enemy>();
				//---速度を上げる効果の重複をさける
				if(k.TempPlayerData.MovingSpeed ==0)
				{
					FlashColor(k);
					k?.ReceiveHealing(enchantAttack,(amount)=>k.TempPlayerData.MovingSpeed +=0.2f, nowDirection,DamageManager.NormalAttack,"SPD↑");
				}
				//---HPが減ってたら回復
				else if(k.statusData.HP < k.statusData.MaxHP)
					k?.ReceiveHealing(enchantAttack,(amount)=>k.statusData.HP +=enchantAttack, nowDirection,DamageManager.NormalAttack);
				//---それ以外は攻撃力をあげる
				else
				{
					k?.ReceiveHealing(enchantAttack,(amount)=>
					{
						if(k.TempPlayerData.Attack==0) 
						{
							k.TempPlayerData.Attack += 40;
						} 
					
					}, nowDirection,DamageManager.NormalAttack,"ATR↑");
				}


			}
			
		}
	}

}