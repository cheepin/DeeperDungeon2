using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
        var button = GetComponent<Button>();
        button.onClick.AddListener(OpenItemWindow);
	}
	
	// Update is called once per frame
	void Update () {
	}

    void OpenItemWindow()
    {
        print("Click!");
    }
}
