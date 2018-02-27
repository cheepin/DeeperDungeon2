using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//TODO placerを共通化
namespace dungeon
{
	[Serializable]
	public struct ItemDropRate
	{
		public item.EmblemData item;
		public float dropRate;
	}

	public class ItemPlacer : Placer 
	{
		protected override void  InstantiateObject(Vector3 itemPos,int itemIdx)
		{
			GameObject newEmblemRes = null;
			if(itemList[itemIdx].item is item.EmblemData)
			{
				//---キーだったら
				var emblem = itemList[itemIdx].item as item.EmblemData;
				if(emblem.skillName =="Key")
				{
					newEmblemRes = Resources.Load("Items/Key") as GameObject;
				}
				//---他のエンブレム
				else
					newEmblemRes = Resources.Load("Items/SkillEmblem/Emblem") as GameObject;
			}
			//---ポーション生成
			else if(itemList[itemIdx].item is item.PotionData)
			{
				newEmblemRes = Resources.Load("Items/Potion") as GameObject;
			}
			var newEmblem = Instantiate(newEmblemRes,itemPos,Quaternion.identity,ObjHolder.transform);
			newEmblem.GetComponent<Item>().SetUp(itemList[itemIdx].item);
		}

	}
}

namespace util
{
	using dungeon;

	static class Tile
	{
		static public List<TileType> Scan(TileType[][] Tiles,ref List<Vector2> Pos)
		{
			int xPos = 0;
			var FloorTilePos = new List<Vector2>();
			List<TileType> tiles= 

			Tiles.SelectMany((a,x)=>
			{
				xPos = x;return a;
			})

			.Where((c,yPos)=>
			{
				if(c == TileType.Floor)
				{	
					FloorTilePos.Add(new Vector2(xPos,yPos-(xPos*Tiles[0].Length)));
					return true; 
				}
				else
				{
					return false;
				}
			})
			.ToList();

			Pos = FloorTilePos;
			return tiles;
		}
	}

}