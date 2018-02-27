using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoardScript : MonoBehaviour {

    public GameObject Tiles;
    public GameObject OuterWall;
    public Transform TileHolder;

    //public Transform TileHolder;
    public int colomns=16;
    public int rows = 16;
    
    void SetUpTile()
    {
        for (int x = -1; x < rows+1; x++)
        {
            for (int y = -1; y < colomns+1; y++)
            {
                GameObject newTile;
                if (x == -1 || x == rows || y == -1 || y == colomns)
                {
                    newTile = Instantiate(OuterWall, new Vector3(x, y, 0), Quaternion.identity);
                    newTile.layer = 9;
                }
                else
                    newTile = Instantiate(Tiles, new Vector3(x, y, 0), Quaternion.identity);
                newTile.transform.SetParent(TileHolder);
            }
        }

        
    }

    void Start()
    {
        TileHolder = new GameObject("TileHolder").transform;
        SetUpTile();
        
    }


	
	// Update is called once per frame
	void Update () {
		
	}
}
