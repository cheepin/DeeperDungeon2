using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using skill;

public class MiniMapGenerator :util.Singleton<MiniMapGenerator>
{

	public GameObject FloorTile;
	public dungeon.DungeonManager dungeonManager;
	public dungeon.TileType[][] tiles;
	public int ScanRadius = 15;
	private int scanRange;
	private Dictionary<Vector2,bool> tilePosHolder;
	private Texture2D mapTexture; 
	private bool inited=false;
	GameObject player;
	Action lambda;

	protected override void OnInstanceDestroy()
	{
		util.DontDestroyManager.RemoveFuncWhenNewLevelLoaded(lambda);
	}

	void Start () {
		//---シーンを読み込まれた時に初期化するファンクタを追加
		lambda = () =>
		{
			//if(SkillManager.GetLevelCount("Mapping") > 0)
			{
				gameObject.SetActive(true);
				Init(inited);
			}
		};
		util.DontDestroyManager.SetFuncWhenNewLevelLoaded(lambda);
		
		Init(false);
		gameObject.SetActive(true);
	}


	private void Init(bool firstInit)
	{
		player = GameObject.FindGameObjectWithTag("Player");
		tiles =  dungeon.DungeonManager.GetFloorTilesData();
		tilePosHolder = new Dictionary<Vector2,bool>();

		var currentMapper = dungeon.DungeonManager.GetCurrentBoardCreator();
		mapTexture = new Texture2D(currentMapper.rows,currentMapper.columns);

		//---マップを初期化 透明にぬりつぶす
		for(int i = 0; i < currentMapper.rows; i++)
		{
			for(int j = 0; j < currentMapper.columns; j++)
			{
				mapTexture.SetPixel(i,j,new Color(0,0,0,0));
			}
		}
		mapTexture.Apply();
		GetComponent<RawImage>().texture = mapTexture;

		//---コールバックを解除
		detectStair = false;
		if(detectStairFunc!=null)
			detectStairFunc -= detectLambda;
		//---初回起動のみ
		if(!firstInit)
		{
			scanRange = ScanRadius/2;
			StartCoroutine(ScanMap());
			StartCoroutine(UpdateMap());
			inited = true;
		}
		
	}

	IEnumerator UpdateMap()
	{
		while(true)
		{
			mapTexture.Apply();
			detectStairFunc?.Invoke();
			FillBlackToPlayerPos(player.transform.position);
			yield return new WaitForSeconds(0.5f);

		}
	}

	//---自分の周りをスキャンして地図を作成
	Action detectStairFunc;
	Action detectLambda;
	bool detectStair=false;
	
	IEnumerator ScanMap()
	{
		while(true)
		{
			int posX = (int)player.transform.position.x;
			int posY = (int)player.transform.position.y;
			if(posX-scanRange >0 && posY-scanRange >0 && posX+scanRange<tiles.Length && posY+scanRange < tiles[0].Length)
			for(int i = -scanRange; i < scanRange; i++)
			{
				for(int k = -scanRange; k < scanRange; k++)
				{
					var inputPos = new Vector2(posX+i,posY+k);
					if(tiles[posX+i][posY+k]==dungeon.TileType.Floor && !tilePosHolder.ContainsKey(inputPos) && inputPos != dungeon.BoardPlacer.StairPos)
					{
						mapTexture.SetPixel(posX+i,posY+k,new Color(1.0f,1.0f,0f,1.0f));
						tilePosHolder.Add(inputPos,true);
					}
					//---階段を表示
					if(inputPos == dungeon.BoardPlacer.StairPos && !detectStair)
					{
						detectStair= true;
						detectLambda = ()=>FlashMap((int)inputPos.x,(int)inputPos.y);
						detectStairFunc += detectLambda;
						tilePosHolder.Add(inputPos,true);

					}
				}
			}
			yield return new WaitForSeconds(1.6f);
		}
	}
	bool flash = false;

	//---階段を点滅
	void FlashMap(int posX,int posY)
	{
		if(flash)
			mapTexture.SetPixel(posX,posY,new Color(1.0f,1.0f,1.0f,1.0f));
		else
			mapTexture.SetPixel(posX,posY,new Color(0.0f,0.0f,0f,0.0f));
		flash = !flash;
		
	}
	//---プレイヤーの位置を表示
	Vector2 prePlayerPos;
	void FillBlackToPlayerPos(Vector2 pos)
	{
		int posX = (int)pos.x;
		int posY = (int)pos.y;
		//---プレイヤーの場所が変わってたら元の黄色に戻す
		if(pos != prePlayerPos)
		{
			int prePosX = (int)prePlayerPos.x;
			int prePosY = (int)prePlayerPos.y;
			mapTexture.SetPixel(prePosX,prePosY,new Color(1.0f,1.0f,0.0f,1.0f));
			prePlayerPos = pos;

		}
		//---プレイヤーの位置を実表示
		mapTexture.SetPixel(posX,posY,new Color(1.0f,1.0f,1.0f,0.0f));
		
		
	}
}
