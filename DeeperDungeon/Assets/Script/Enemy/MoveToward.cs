using UnityEngine;
using System.Collections;
using System;
using util;

public class MoveToward : MonoBehaviour
{
	public DirectionHelper.Direction NowDirection{get;set; }
	public float moveDistancePerFrame;

	// Update is called once per frame
	void Update()
	{
		transform.position += DirectionHelper.MapByNowDirection(NowDirection,moveDistancePerFrame,0);
	}
}
