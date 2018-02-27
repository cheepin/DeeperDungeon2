using System;
using System.Collections.Generic;
using UnityEngine;

namespace dungeon
{
	public class DontDestroyHelper : MonoBehaviour
	{

		static public void SetDontDestroy(GameObject gameObject)
		{
			util.DontDestroyManager.Set("Dungeon", gameObject);
			util.DontDestroyManager.Set("FirstDungeon-15F", gameObject);
			util.DontDestroyManager.Set("FirstDungeon-23F", gameObject);
			util.DontDestroyManager.Set("FirstDungeon-30F", gameObject);
			util.DontDestroyManager.Set("FirstDungeon-42F", gameObject);
			util.DontDestroyManager.Set("FirstDungeon-50F", gameObject);
			util.DontDestroyManager.Set("FirstDungeon-Prepared1", gameObject);
		}
		public void SetDontDestroy2(GameObject gameObject)
		{
			SetDontDestroy(gameObject);
		}
	}

}