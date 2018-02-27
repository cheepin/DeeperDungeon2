using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace util
{
	public class Singleton<T>:MonoBehaviour where T:MonoBehaviour
	{
		private static T _instance;

		protected virtual void Awake()
		{
			if(this != Instance)
			{	
				Destroy(gameObject);
			}
		}
			

		static public T Instance
		{
			get
			{
				if(_instance == null)
				{	
					_instance = FindObjectOfType<T>();
					if(_instance==null)
						print(typeof(T) +"  is Nothing");
				}
			
				return _instance;
			}
		}

		protected void OnDestroy()
		{
			if(_instance == this)
			{
				OnInstanceDestroy();
			}
		}
		//---シングルトンが破壊される時にコールバックされる
		protected virtual void OnInstanceDestroy(){}

	}

}