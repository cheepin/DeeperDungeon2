using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace moving.enemy
{
	
	public class EnemyData : ScriptableObject
	{
		public string EnemyName;
		public int HP;
		public int Attack;
		public int Defense;
		public float MoveSpeed;	
		public int Exp;
	}
}