using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace item
{
	public class DroppedEmblem : Emblem
	{
		[SerializeField]
		EmblemData embedEmblemData;
		// Use this for initialization
		void Start()
		{
			SetUp(embedEmblemData);
		}

		
	}

}