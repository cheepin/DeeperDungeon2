using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
//---これを入れるだけで作成時破壊されなくなる
//---シングルトンにできる

namespace util
{
	public class MakeDontDestroy : Singleton<MakeDontDestroy>
	{

		[SerializeField]
		string[] sceneName = null;

		private void Start()
		{
			foreach(var name in sceneName)
			{
				DontDestroyManager.Set(name,gameObject);
			}
		}

	} 
}
