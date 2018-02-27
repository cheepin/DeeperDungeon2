using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CH = util.CoroutineHelper;
using score;
namespace moving.enemy
{
	public class ClearChest : Enemy
	{
		[SerializeField]
		ScoreDisplay scoreDisplay;
		[SerializeField]
		FadeWhite fadeWhite;


		protected override void Die()
		{
			SoundManager.OpenKey();

			//---フラグ立て・処理
			rb2D.velocity = new Vector2(0, 0);
			animator.speed = 1.5f;
			PlayerPrefs.SetInt(ui.HardButton.id_CreardNomal,1);

			//---animatorに死亡処理 "player""Enemy"tagを持つオブジェクトに対するコライダーを無効に
			GetComponent<Animator>().SetTrigger("Die");

			//---コールバック
			ActionWhenDie?.Invoke();
			//---時間たった後消失処理
			StartCoroutine(
			CH.Chain(this,
			CH.DelaySecond(
				action: () =>
				{
					SoundManager.Aura();
					fadeWhite.gameObject.SetActive(true);
				},
				waitSecond: 1.0f),
			
			CH.DelaySecond(1.5f,()=>
			{
				gameObject.SetActive(false);
				scoreDisplay.gameObject.SetActive(true);
			}
			)));
		
		}

	}

}