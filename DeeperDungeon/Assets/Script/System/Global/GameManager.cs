using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour {

    //private  BoardScript boardscript;
    public static GameManager instance = null;


    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
        //boardscript.ToString();
        
    }
    
    // Use this for initialization
	
	// Update is called once per frame
	void Update () {
		
	}
}
