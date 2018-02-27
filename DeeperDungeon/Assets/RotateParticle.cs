using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using util;
namespace skill
{
	public class RotateParticle : MonoBehaviour
	{
		ParticleSystem pSystem;
		ParticleSystem.ShapeModule shape;
		// Use this for initialization
		void Start()
		{
			pSystem = GetComponent<ParticleSystem>();
			shape = pSystem.shape;
			//shape.scale = new Vector3(5,5,5);
		}
		
		// Update is called once per frame
		void Update()
		{

		}
	}

}

namespace util
{
	static public class Vector3Ex
	{
		static public void AddPosZ(this Vector3 vector3,float z)
		{
			vector3 += new Vector3(0,0,z);
		}

	}
}