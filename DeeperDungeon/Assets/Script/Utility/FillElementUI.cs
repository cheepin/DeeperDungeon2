using UnityEngine;
using System;

namespace util
{
	//パネルなどにある要素で満たすために使う。
	static public class FillElementUI
	{
		//---横方向から埋めていく
		//startPos:初期位置
		//elemInterval:エレメント（要素）の間隔
		//column row 列数行数
		//func インスタンスに使用する 
		static public void FillRows(Vector2 startPos,Vector2 elemInterval,int column,int row,Action<Vector2> func)
		{
			var positioner = startPos;
			for(int i = 0; i < column; i++)
			{
				for(int j = 0; j < row; j++)
				{
					func(positioner);
					positioner.x +=elemInterval.x;	
				}
				positioner.y +=elemInterval.y;	
				positioner.x = startPos.x;
			}
		}
		//---縦方向から埋めていく
		static public void FillColumns(Vector2 startPos,Vector2 elemInterval,int column,int row,Action<Vector2> func)
		{
			var positioner = startPos;
			for(int i = 0; i < row; i++)
			{
				for(int j = 0; j < column; j++)
				{
					func(positioner);
					positioner.y +=elemInterval.y;	
				}
				positioner.x +=elemInterval.x;	
				positioner.y = startPos.y;
			}
		}
	}

}