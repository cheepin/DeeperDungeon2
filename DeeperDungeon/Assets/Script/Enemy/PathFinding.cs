using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace Alg
{

[Serializable]
public class PathFinding
{
    Node[,] graph;
    public void GeneratePathFindingGraph(Vector2 mapSize)
    {
        graph = new Node[(int)mapSize.x, (int)mapSize.y];

        for (int x = 0; x < mapSize.x; x++)
        {
            for (int y = 0; y < mapSize.y; y++)
            {

                if (x > 0)
                    graph[x, y].neighbours.Add(graph[x-1,y]);
                if (x > mapSize.x-1)
                    graph[x, y].neighbours.Add(graph[x - 1, y]);

                if (y > 0)
                    graph[x, y].neighbours.Add(graph[x, y-1]);
                if (y> mapSize.x-1)
                    graph[x, y].neighbours.Add(graph[x - 1, y+1]);

            }
        }

    }

    public void MoveUnitTo(int x,int y)
    {
        //Dictionary<Node, float> dist = new Dictionary<Node, float>();
        //Dictionary<Node, Node>  prev = new Dictionary<Node, Node>();

    }


};



[Serializable]
public class Node
{
    public List<Node> neighbours;
    public Node()
    {
        neighbours = new List<Node>();
    }
}


}



