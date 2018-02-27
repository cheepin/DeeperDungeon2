using UnityEngine;
using System.Collections;
using util;
namespace moving.enemy
{

	public class SkeletonWarrior_Lv4 : SkeletonWarrior
	{
		GameObject shadow;
		// Use this for initialization
		protected override void Start()
		{
			sprite = GetComponent<SpriteRenderer>();
			shadow = transform.Find("Shadow").gameObject;
			shadow.GetComponent<SpriteRenderer>().color = new Color(0,0,0,0);
			base.Start();
		}
		
		protected override IEnumerator ActionInRange(Transform hittedObj, Vector2 targetEnd)
		{
			if(statusData.HP>0)
			{
				stopMoving = false;

				StartCoroutine(CoroutineHelper.Chain(this,
							
					CoroutineHelper.DelaySecond(0.81f,()=>{inverseMoveTime = 1.0f/0.063f;sprite.color = new Color(0,0,0,0);shadow.GetComponent<SpriteRenderer>().color = new Color(1,1,1,1); }),
					CoroutineHelper.DelaySecond(chargeTime,()=>
					{
						stopMoving = true;
						rb2D.velocity = new Vector2(0,0);
						sprite.color = MyColor;
						shadow.GetComponent<SpriteRenderer>().color = new Color(0,0,0,0); 
						animator.SetTrigger("Attack");
					}),
					CoroutineHelper.DelaySecond(0.1f,()=>
					{
						 if(!NowReceivingDamage)
						 {
							bradeAttack.ParentDirection = nowDirection;
							bradeAttack.gameObject.SetActive(true);
							stopMoving = true;
							SoundManager.SwordBrade();
						 }
						
					}),

					CoroutineHelper.DelaySecond(attackTime,()=>
					{
						nowAttack = false;
						stopMoving=false;
						inverseMoveTime = 1.0f/moveTime;
					})

				));
			}
			yield return new WaitForSeconds(0.2f);
		}


	}

}