using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace item
{
	public class Potion : Item
	{
		PotionData potionData;

		// Use this for initialization
		public override void SetUp(ScriptableObject _data)
		{
			var data = _data as PotionData;
			potionData = data;
		
			Sprite sprite = ResourceLoader.Instance.EmblemTexturePacker[potionData.ItemName];
			GetComponentInChildren<SpriteRenderer>().sprite = sprite;
		}

		protected override void ItemAction(GameObject _player)
		{
			SoundManager.Get();
			var player = _player.GetComponent<moving.player.Player>();
			player.MyPlayerData.HP += (player.MyPlayerData.MaxHP/2);
		}
	}

}