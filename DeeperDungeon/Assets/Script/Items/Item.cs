using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	}

	public virtual void SetUp(ScriptableObject scriptable)
	{

	}

	private void OnTriggerEnter2D (Collider2D collision)
	{
		if(collision.gameObject.CompareTag("Player"))
		{
			
			ItemAction(collision.gameObject);
			gameObject.SetActive(false);

		}
	}

	protected virtual void ItemAction(GameObject getter){}

}
