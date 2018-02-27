using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace dungeon
{
	public class FixedBoardCreator : BoardPlacer
	{

		// Use this for initialization
		void Start()
		{
			var player = GameObject.FindGameObjectWithTag("Player");
			player.transform.position = new Vector3(10,10);
			var playerComp = player.GetComponent<moving.player.Player>();
			playerComp.rb2D.velocity = new Vector2(0,0);
		}

		// Update is called once per frame
		void Update()
		{

		}
	}

}