using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using moving.enemy;

namespace dungeon
{
	[Serializable]
	public class EnemyAppearanceRate
	{
		public Enemy EnemyData;
		public float AppearanceRate;
	}
	
	public class EnemyPlacer : Placer
	{
		public Vector2 PlayerPos {get;set; }

		protected override void  InstantiateObject(Vector3 itemPos,int itemIdx)
		{
			int range = 5;
			//---プレイヤーが５マス以内に居なかったらモンスター配置
			if(!CheckArea(itemPos,new Rect(PlayerPos.x-range,PlayerPos.y-range,range*2,range*2)))
			{
				var enemyData = itemList[itemIdx].item as EnemyData;
				try
				{
					var newEnemyRes = Resources.Load($"Enemy/{enemyData.EnemyName}") as GameObject;
					if(newEnemyRes==null)
						throw new NullReferenceException();
					Instantiate(newEnemyRes,itemPos,Quaternion.identity,ObjHolder.transform);
				}
				catch
				{
					Debug.Assert(false,$"{itemList[itemIdx].item}");
					Debug.Assert(false,$"{enemyData.EnemyName}が存在しません");
				}
				
				
			}
		}

		//---指定したEvaluationPosが範囲内にあるかどうか調べる
		//---要はプレイヤーの近くにモンスターを湧かせなくするために使う
		static bool CheckArea(Vector2 EvaluationPos,Rect area)
		{
			if(area.xMin<EvaluationPos.x && area.xMax>EvaluationPos.x)
				if(area.yMin<EvaluationPos.y && area.yMax>EvaluationPos.y)
					return true;
			return false;
		}
	}
	



}

