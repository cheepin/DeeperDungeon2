using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using Random = UnityEngine.Random;


namespace dungeon
{
	[Serializable]
	public struct DropRatePerList
	{
		public ScriptableObject item;
		public float dropRate;
	}

	public class Placer : MonoBehaviour
	{
		public TileType[][] Tiles {get;set; }
		[SerializeField]
		public List<DropRatePerList> itemList = null;
		[SerializeField]
		bool counterDisplayMode=false;
		[SerializeField]
		public string DataPath;
		protected GameObject ObjHolder{get;private set;}

		public virtual void PlaceObj()
		{
			//---フロアタイルの座標が入る
			List<Vector2> FloorTilePos =new List<Vector2>();
			
			//---フロアタイルだけを抽出
			List<TileType> floorTile = util.Tile.Scan(Tiles,ref FloorTilePos);

			//---アイテムのドロップ率の合計を獲得
			float sumItemDroprates = itemList.Select((x)=>x.dropRate).Aggregate((sums,x)=>sums+x);

			//---アイテムの配置
			int numberOfItemInThisFloor = (int)(floorTile.Count* sumItemDroprates);
			Debug.Assert(numberOfItemInThisFloor<floorTile.Count,"アイテムの合計DropRateが１を越えています");
			List<ScriptableObject> indexCounter = new List<ScriptableObject>();

			//---親ホルダーの生成
			var holderRes = Resources.Load("GameSystem/ObjHolder") as GameObject;
			ObjHolder = Instantiate(holderRes);

			for(int i = 0; i < numberOfItemInThisFloor; i++)
			{
				//---配置決定
				int itemIndex = Random.Range(0,FloorTilePos.Count);
				Debug.Assert(itemIndex<=FloorTilePos.Count,"ランダム値がフロアタイルリストを越えました　スクリプトを確認してください");
				var itemPos = new Vector3(FloorTilePos[itemIndex].x,FloorTilePos[itemIndex].y,-1);
				FloorTilePos.RemoveAt(itemIndex);
				
				//---抽選開始
				float sum = 0;
				int itemIdx = 0;
				float itemLottery = Random.Range(0,sumItemDroprates);
				foreach(var item in itemList.Select((x,idx)=>new {x.dropRate,idx }))
				{
					sum += item.dropRate;
					if(sum>itemLottery)
					{
						itemIdx = item.idx;
						break;
					}
				}
				
				InstantiateObject(itemPos,itemIdx);

				indexCounter.Add(itemList[itemIdx].item);
			}

			//---生成したアイテムの各カウントを表示
			DisplayCounter(counterDisplayMode,indexCounter);
			
		}

		protected virtual void  InstantiateObject(Vector3 itemPos,int itemIdx)
		{}

		/// <summary>
		/// 指定したタイルから抽選で一つだけを指定
		/// </summary>
		/// <param name="Tiles">抽選されるタイル群</param>
		/// <param name="selectTileType">抽選するタイルのタイプ</param>
		/// <returns></returns>
		public static Vector3 LotteryPlaceFromTiles(TileType[][] Tiles,TileType selectTileType)
		{
			int x;
			int y;
			do
			{
				x = Random.Range(0,Tiles.Length);
				y = Random.Range(0,Tiles[0].Length);
			}while(Tiles[x][y]!=selectTileType);

			return new Vector3(x,y);
		}


		void DisplayCounter(bool counterDisplayMode,List<ScriptableObject> indexCounter) 
		{
			if(counterDisplayMode)
			{
				var list = indexCounter.GroupBy((x) => x).Select((x) => new
				{
					Value = x.Key,
					Count = x.Count()
				});

				foreach(var item in list)
				{
					print($"{item.Value} is {item.Count}");
				} 
			}
		}


	}
}