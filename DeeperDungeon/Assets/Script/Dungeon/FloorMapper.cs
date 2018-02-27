using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using moving.player;


//---BoardCreatorとItemPlacer、MonsterPlacerの管理
namespace dungeon { 

	 [System.Serializable]
    public class ObjMapper
    {
       public  int from;
       public  int to;
       public Placer placer;
    }


	public class FloorMapper : MonoBehaviour 
	{

		public int NumberFloor;
		public List<ObjMapper> boardMap;
		private Dictionary<int,Placer> boardMapDict;

		public List<ObjMapper> itemMapper;
		private Dictionary<int,Placer> itemMapDict = null;

		public List<ObjMapper> enemyMapper;
		private Dictionary<int,Placer> enemyMapDict = null;
		


		public BoardPlacer CurrentMapper {get;private set;}


		//---実質エントリーポイント
		public void InstantiateFloor(int floor)
		{
			if(boardMapDict==null)
			{
				boardMapDict = AssignOwnMapper(boardMap);
			}
			Debug.Assert(boardMapDict.ContainsKey(floor),$"floorMapperにこの階層[{floor}]のboardMapperが割当てられていません");
			CurrentMapper =  (BoardPlacer)Instantiate(boardMapDict[floor]);
			CurrentMapper.PlaceObj();

			//---アイテムマッパーからアイテムをインスタンス化
			ItemPlacer itemPlacer = GenerateFromMapper(CurrentMapper.Tiles,itemMapper,itemMapDict,floor) as ItemPlacer;
			itemPlacer.PlaceObj();

			//--エネミーマッパーからモンスターをインスタンス化
			EnemyPlacer enemyPlacer = GenerateFromMapper(CurrentMapper.Tiles,enemyMapper,enemyMapDict,floor) as EnemyPlacer;
			var player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
			player.transform.position = Placer.LotteryPlaceFromTiles(CurrentMapper.Tiles,TileType.Floor);
			enemyPlacer.PlayerPos = player.transform.position;
			enemyPlacer.PlaceObj();


		}

		static Dictionary<int,Placer> AssignOwnMapper(List<ObjMapper> floorMap )
		{
			var objMapDict = new Dictionary<int,Placer>();
			foreach(var objMap in floorMap)
			{
				for(int i = objMap.from; i <= objMap.to; i++)
				{
					Debug.Assert(!objMapDict.ContainsKey(i),"マッピングが被っています");
					objMapDict[i] = objMap.placer;
				}

			}
			return objMapDict;
		}

		

		static Placer GenerateFromMapper(TileType[][] tiles,List<ObjMapper> _itemMapper,Dictionary<int,Placer>_itemMapDict,int floor)
		{
			if(_itemMapDict == null)
			{
				_itemMapDict = AssignOwnMapper(_itemMapper);
			}
			Debug.Assert(_itemMapDict.ContainsKey(floor), "floorMapperにこの階層のitemMapperが割当てられていません");

			var Placer = _itemMapDict[floor];
			Placer.Tiles = tiles;
			return Placer;
		}



	}
}