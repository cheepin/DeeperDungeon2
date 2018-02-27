using UnityEngine;
using System.Collections;

namespace cheat
{
	public class StatusChangeButton : MonoBehaviour
	{
		[SerializeField]
		string skillName;
		[SerializeField]
		int changeValue;

		public void SetStatus()
		{
			CheatManager.SetStatus(skillName,changeValue);
		}
		
	}
}