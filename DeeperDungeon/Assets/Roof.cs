using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roof : MonoBehaviour {

	private void OnTriggerEnter2D(Collider2D collision)
	{
		collision.gameObject.GetComponent<SpriteRenderer>().sortingOrder =0;
		
	}
	private void OnTriggerExit2D(Collider2D collision)
	{
		collision.gameObject.GetComponent<SpriteRenderer>().sortingOrder =1;
		
	}
}
