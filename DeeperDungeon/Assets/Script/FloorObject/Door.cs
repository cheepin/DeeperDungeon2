using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using skill;
namespace floorObject
{
	public class Door : MonoBehaviour
	{
		[SerializeField]
		GameObject[] ownWall;
		[SerializeField]
		Sprite openDoorImage;
		
		bool opened=false;

		// Use this for initialization
		void Start()
		{
			
		}

		// Update is called once per frame
		void Update()
		{

		}

		private void OnTriggerEnter2D(Collider2D collision)
		{
			int keyLevel = SkillManager.GetLevelCount("Key");
			//---ドアに当たったのがプレイヤーで、かつキーを一つ以上持っていたら
			if(collision.gameObject.CompareTag("Player") && keyLevel>0 && !opened)
			{
				//---キーを消費
				SkillManager.LevelDownStatus("Key",ref keyLevel,null,true);
				SkillManager.DecreaseMaxLevel("Key");
				//---壁のColliderを無効に
				foreach(var wall in ownWall)
				{
					wall.GetComponent<Collider2D>().enabled = false;
				}
				//---ドアイメージの差し替え
				GetComponent<SpriteRenderer>().sprite = openDoorImage;
				//---ドアの状態フラグを開放に
				opened = true;
				SoundManager.OpenKey();
			}
		}


	}

}