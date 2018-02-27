using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace score
{
	public class BackTitleButton : MonoBehaviour
	{
		public void BackTitle()
		{
			SceneManager.LoadScene("Title");
		}

	}

}